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
    ///* プログラム名： frmGainCor.frm                                               */
    ///* 処理概要　　： ゲイン校正                                               　　*/
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
    public partial class frmGainCor : Form
    {
        private bool myBusy;

        //追加2015/01/28hata
        private char presskye = (char)0;  //Keypressの値          
        private string PreUpDownValText = "";   //cwneDownTableDistanceを変更した前回の値
        private string PreUpDownEnterValText = "";   //cwneDownTableDistanceを確定した前回の値

        //Rev20.00 追加 by長野 2015/02/16
        private string PreYAxisMoveValText = "";   //cwneTableYAxisDistanceを変更した前回の値
        private string PreYAxisMoveEnterValText = "";   //cwneTableYAxisDistanceを確定した前回の値

        //Rev23.40 追加 by長野 2016/06/19
        private string PreXAxisMoveValText = "";   //cwneTableXAxisDistanceを変更した前回の値
        private string PreXAxisMoveEnterValText = "";   //cwneTableXAxisDistanceを確定した前回の値

        #region インスタンスを返すプロパティ

        // frmGainCorのインスタンス
        private static frmGainCor _Instance = null;

        /// <summary>
        /// frmGainCorのインスタンスを返す
        /// </summary>
        public static frmGainCor Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmGainCor();
                }

                return _Instance;
            }
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmGainCor()
        {
            InitializeComponent();
        }

        #endregion

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
        public bool IsBusy
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
                cwneScanView.Enabled = !myBusy;
                cwneSum.Enabled = !myBusy;
                cwneMA.Enabled = !myBusy;
                chkGainTableRot.Enabled = !myBusy;
                chkDownTable.Enabled = !myBusy;
                cwneDownTableDistance.Enabled = (!myBusy) & (chkDownTable.CheckState == CheckState.Checked);

                //Rev20.00 追加 by長野 2015/02/16
                chkGainTableYAxis.Enabled = !myBusy;
                cwneTableYAxisDistance.Enabled = (!myBusy) & (chkGainTableYAxis.CheckState == CheckState.Checked);

                //Rev24.00 追加 by長野 2016/05/09
                chkGainTableXAxis.Enabled = !myBusy;
                cwneTableXAxisDistance.Enabled = (!myBusy) & (chkGainTableXAxis.CheckState == CheckState.Checked);

                //追加2014/10/07hata_v19.51反映
                chkHaFuOfScan.Enabled = !myBusy;                //v18.00追加 byやまおか 2011/03/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                chkShiftScan.Enabled = !myBusy;                //v18.00追加 byやまおか 2011/03/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

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
            int ret = 0;
            
            
            //RAMディスクが構築されているかどうか  'v17.40変更 byやまおか 2010/10/26
			//If UseRamDisk And (Not RamDiskIsReady) Then Exit Sub
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//    If UseRamDisk Then      'v17.42修正 byやまおか 2010/11/04
			//        If (Not RamDiskIsReady) Then Exit Sub
			//    End If
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //追加2014/10/07hata_v19.51反映
            //メカが動ける(パネルがOFF)かチェック     'v18.00追加 byやまおか 2011/07/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
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
				//        'MilCaptureStop     'v17.00削除 byやまおか 2010/01/19
				//        Select Case DetType 'v17.00追加(ここから) byやまおか 2010/01/20
				//            Case DetTypeII, DetTypeHama
				//                MilCaptureStop
				//            Case DetTypePke
				//                PkeCaptureStop (hPke)     'changed by 山本 2009-09-16
				//        End Select          'v17.00追加(ここまで) byやまおか 2010/01/20
				//#End If

				//実行中の処理に対して停止要求をする     'v17.50上記の処理を関数化 by 間々田 2011/02/17
				modCT30K.CallUserStopSet();

				return;
			}

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

			//I.I.（またはFPD）電源のチェック  条件追加 by 間々田 2004/12/28
			if (!modSeqComm.PowerSupplyOK())
            {
				return;
            }

			//テーブル下降収集ありの場合 added by 間々田 2003-03-03
			float udPos = 0;
			if (chkDownTable.CheckState == System.Windows.Forms.CheckState.Checked)
            {

				udPos = (float)frmMechaControl.Instance.ntbUpDown.Value;

				//もう一度ストローク数チェックを行なう
                //変更2015/01/28hata
                //int dist = 0;
                //int.TryParse(cwneDownTableDistance.Text, out dist);
                float dist = 0f;
                float.TryParse(cwneDownTableDistance.Text, out dist);
                if (udPos + dist > CTSettings.GValUpperLimit)
                {
					//メッセージ表示：
					//   テーブル下限を超えて下降しようとしています！
					//   現在の昇降位置(mm): xxx
					//   テーブル下限値(mm): xxx
                    
                    //変更2014/10/07hata_v19.51反映
                    //MessageBox.Show(StringTable.GetResString(9583, Convert.ToString(udPos), "\t" + Convert.ToString(CTSettings.GValUpperLimit)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //v19.51 X線・検出器昇降の場合を追加 by長野 2014/03/03
                    if (CTSettings.t20kinf.Data.ud_type == 0)
                    {
                        //Interaction.MsgBox(StringTable.GetResString(9583, Convert.ToString(udPos), Constants.vbTab + Convert.ToString(modGlobal.GValUpperLimit)), MsgBoxStyle.Critical);
                        MessageBox.Show(StringTable.GetResString(9583, Convert.ToString(udPos), "\t" + Convert.ToString(CTSettings.GValUpperLimit)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        //Interaction.MsgBox(StringTable.GetResString(9584, Convert.ToString(udPos), ConcwneDownTableDistance.Textstants.vbTab + Convert.ToString(modGlobal.GValUpperLimit)), MsgBoxStyle.Critical);
                        MessageBox.Show(StringTable.GetResString(9584, Convert.ToString(udPos), "\t" + Convert.ToString(CTSettings.GValUpperLimit)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

					//テーブル下降収集用のストローク数を調整してこのルーチンから抜ける
                    cwneDownTableDistance.Text = (CTSettings.GValUpperLimit - udPos).ToString();// 【C#コントロールで代用】
					return;
				}

				//テーブル下降収集有りとして変数に保存
				modScanCorrect.DownTable = System.Windows.Forms.CheckState.Checked;

				//テーブル下降収集用のストローク数（移動距離）（ｍｍ）も変数に保存
				//modScanCorrect.DownTableDistance = cwneDownTableDistance.Value;
                float downTableDistance = 0.0f;
                float.TryParse(cwneDownTableDistance.Text, out downTableDistance);// 【C#コントロールで代用】
                modScanCorrect.DownTableDistance = downTableDistance;
			}
            else
            {
				//テーブル下降収集無として変数に保存
				modScanCorrect.DownTable = System.Windows.Forms.CheckState.Unchecked;
			}

            //Rev20.00 テーブルY軸移動収集ありの場合 追加 by 長野 2015-02-16
            float yAxisPos = 0;
            if (chkGainTableYAxis.CheckState == System.Windows.Forms.CheckState.Checked)
            {
                string msg = (CTResources.LoadResString(9406)) + "\r\n" + CTResources.LoadResString(StringTable.IDS_ClickOK);
                if (MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                {
                    return;
                }

                yAxisPos = (float)frmMechaControl.Instance.ntbTableXPos.Value;

                //もう一度ストローク数チェックを行なう
                //変更2015/01/28hata
                //int dist = 0;
                //int.TryParse(cwneDownTableDistance.Text, out dist);
                float dist = 0f;
                float.TryParse(cwneTableYAxisDistance.Text, out dist);
                if (yAxisPos + dist > CTSettings.t20kinf.Data.y_axis_upper_limit)
                {
                    //メッセージ表示：
                    //   テーブルY軸の上限を超えて移動しようとしています！
                    //   現在のY軸位置(mm): xxx
                    //   テーブルY軸上限値(mm): xxx

                    //Interaction.MsgBox(StringTable.GetResString(9583, Convert.ToString(udPos), Constants.vbTab + Convert.ToString(modGlobal.GValUpperLimit)), MsgBoxStyle.Critical);
                    MessageBox.Show(StringTable.GetResString(9585, Convert.ToString(yAxisPos), "\t" + Convert.ToString(CTSettings.t20kinf.Data.y_axis_upper_limit)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
    
                    //テーブルY軸移動収集用のストローク数を調整してこのルーチンから抜ける
                     cwneTableYAxisDistance.Text = (CTSettings.t20kinf.Data.y_axis_upper_limit - yAxisPos).ToString();// 【C#コントロールで代用】
                    return;
                }
                else if (yAxisPos + dist < CTSettings.t20kinf.Data.y_axis_lower_limit)
                {
                    //メッセージ表示：
                    //   テーブルY軸の下限を超えて移動しようとしています！
                    //   現在のY軸位置(mm): xxx
                    //   テーブルY軸下限値(mm): xxx

                    //Interaction.MsgBox(StringTable.GetResString(9583, Convert.ToString(udPos), Constants.vbTab + Convert.ToString(modGlobal.GValUpperLimit)), MsgBoxStyle.Critical);
                    MessageBox.Show(StringTable.GetResString(9587, Convert.ToString(yAxisPos), "\t" + Convert.ToString(CTSettings.t20kinf.Data.y_axis_upper_limit)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //テーブルY軸移動収集用のストローク数を調整してこのルーチンから抜ける
                    cwneTableYAxisDistance.Text = (CTSettings.t20kinf.Data.y_axis_lower_limit - yAxisPos).ToString();// 【C#コントロールで代用】
                    return;
                }

                //テーブルY軸移動収集有りとして変数に保存
                modScanCorrect.yAxisMoveTable = System.Windows.Forms.CheckState.Checked;

                //テーブルY軸移動収集用のストローク数（移動距離）（ｍｍ）も変数に保存
                //modScanCorrect.DownTableDistance = cwneDownTableDistance.Value;
                float yAxisMoveTableDistance = 0.0f;
                float.TryParse(cwneTableYAxisDistance.Text, out yAxisMoveTableDistance);// 【C#コントロールで代用】
                modScanCorrect.yAxisMoveTableDistance = yAxisMoveTableDistance;
            }
            else
            {
                //テーブル下降収集無として変数に保存
                modScanCorrect.yAxisMoveTable = System.Windows.Forms.CheckState.Unchecked;
            }

            //Rev23.40 テーブルFCD軸移動収集ありの場合 追加 by 長野 2016-06-19
            float xAxisPos = 0;
            if (chkGainTableXAxis.CheckState == System.Windows.Forms.CheckState.Checked)
            {
                string msg = (CTResources.LoadResString(9403)) + "\r\n" + CTResources.LoadResString(StringTable.IDS_ClickOK);
                if (MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                {
                    return;
                }

                xAxisPos = (float)frmMechaControl.Instance.ntbFCD.Value;

                //もう一度ストローク数チェックを行なう
                //変更2015/01/28hata
                //int dist = 0;
                //int.TryParse(cwneDownTableDistance.Text, out dist);
                float dist = 0f;
                float.TryParse(cwneTableXAxisDistance.Text, out dist);
                if (xAxisPos + dist > CTSettings.mechapara.Data.max_fcd)
                {
                    //メッセージ表示：
                    //   テーブルFCD軸の上限を超えて移動しようとしています！
                    //   現在のFCD軸位置(mm): xxx
                    //   テーブルFCD軸上限値(mm): xxx

                    //Interaction.MsgBox(StringTable.GetResString(9583, Convert.ToString(udPos), Constants.vbTab + Convert.ToString(modGlobal.GValUpperLimit)), MsgBoxStyle.Critical);
                    MessageBox.Show(StringTable.GetResString(9588, Convert.ToString(xAxisPos), "\t" + Convert.ToString(CTSettings.mechapara.Data.max_fcd)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //テーブルFCD軸移動収集用のストローク数を調整してこのルーチンから抜ける
                    cwneTableYAxisDistance.Text = (CTSettings.t20kinf.Data.y_axis_upper_limit - yAxisPos).ToString();// 【C#コントロールで代用】
                    return;
                }
                else if (yAxisPos + dist < CTSettings.t20kinf.Data.y_axis_lower_limit)
                {
                    //メッセージ表示：
                    //   テーブルFCD軸の下限を超えて移動しようとしています！
                    //   現在のFCD軸位置(mm): xxx
                    //   テーブルFCD軸下限値(mm): xxx

                    //Interaction.MsgBox(StringTable.GetResString(9583, Convert.ToString(udPos), Constants.vbTab + Convert.ToString(modGlobal.GValUpperLimit)), MsgBoxStyle.Critical);
                    MessageBox.Show(StringTable.GetResString(9589, Convert.ToString(xAxisPos), "\t" + Convert.ToString(CTSettings.mechapara.Data.min_fcd)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //テーブルFCD軸移動収集用のストローク数を調整してこのルーチンから抜ける
                    cwneTableYAxisDistance.Text = (CTSettings.t20kinf.Data.y_axis_lower_limit - yAxisPos).ToString();// 【C#コントロールで代用】
                    return;
                }

                //テーブルFCD軸移動収集有りとして変数に保存
                modScanCorrect.xAxisMoveTable = System.Windows.Forms.CheckState.Checked;

                //テーブルFCD軸移動収集用のストローク数（移動距離）（ｍｍ）も変数に保存
                //modScanCorrect.DownTableDistance = cwneDownTableDistance.Value;
                float xAxisMoveTableDistance = 0.0f;
                float.TryParse(cwneTableXAxisDistance.Text, out xAxisMoveTableDistance);// 【C#コントロールで代用】
                modScanCorrect.xAxisMoveTableDistance = xAxisMoveTableDistance;
            }
            else
            {
                //テーブル下降収集無として変数に保存
                modScanCorrect.xAxisMoveTable = System.Windows.Forms.CheckState.Unchecked;
            }

            //追加2014/10/07hata_v19.51反映
            //シフトスキャン収集にチェックがあるときは   'v18.00追加 byやまおか 2011/02/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev25.00 WスキャンON時も同じ動きになる by長野 2016/07/05
            if ((chkShiftScan.CheckState == System.Windows.Forms.CheckState.Checked))
            {
                modScanCorrect.Flg_GainShiftScan = System.Windows.Forms.CheckState.Checked;         //フラグON

            //シフトスキャン収集にチェックがないときは
            }
            else
            {
                modScanCorrect.Flg_GainShiftScan = System.Windows.Forms.CheckState.Unchecked;       //フラグOFF
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

			//ビジーフラグセット
			IsBusy = true;

			//ゲイン校正画像を配列に読み込む
            //2014/11/06hata キャストの修正
            //if (modScanCorrectNew.GetImageForGainCorrect(stsGain, 
            //                                             pgbGain, 
            //                                             Convert.ToInt32(cwneScanView.Value),
            //                                             Convert.ToInt32(cwneSum.Value), 
            //                                             chkGainTableRot.CheckState, 
            //                                             (float)cwneMA.Value, 
            //                                             (modScanCorrect.DownTable == CheckState.Checked ? modScanCorrect.DownTableDistance : 0)))
            ////Rev20.00 引数追加 by長野 2015/02/16
            //if (modScanCorrectNew.GetImageForGainCorrect(stsGain,
            //                                                pgbGain,
            //                                                Convert.ToInt32(cwneScanView.Value),
            //                                                Convert.ToInt32(cwneSum.Value),
            //                                                chkGainTableRot.CheckState,
            //                                                (float)cwneMA.Value,
            //                                                (modScanCorrect.DownTable == CheckState.Checked ? modScanCorrect.DownTableDistance : 0),
            //                                                (modScanCorrect.yAxisMoveTable == CheckState.Checked ? modScanCorrect.yAxisMoveTableDistance : 0)))
            //Rev23.40 引数追加 by長野 2016/06/19
            if (modScanCorrectNew.GetImageForGainCorrect(stsGain,
                                                            pgbGain,
                                                            Convert.ToInt32(cwneScanView.Value),
                                                            Convert.ToInt32(cwneSum.Value),
                                                            chkGainTableRot.CheckState,
                                                            (float)cwneMA.Value,
                                                            (modScanCorrect.DownTable == CheckState.Checked ? modScanCorrect.DownTableDistance : 0),
                                                            (modScanCorrect.yAxisMoveTable == CheckState.Checked ? modScanCorrect.yAxisMoveTableDistance : 0),
                                                            (modScanCorrect.xAxisMoveTable == CheckState.Checked ? modScanCorrect.xAxisMoveTableDistance : 0)))

            {
				//フォームを消去
                //変更2015/1/17hata_非表示のときにちらつくため
                //Hide();
                modCT30K.FormHide(this);

				//ゲイン校正結果フォームを表示する
                //変更2014/10/07hata_v19.51反映
				//frmGainCorResult.Instance.Dialog();
                //v18.00変更 byやまおか 2011/02/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                frmGainCorResult.Instance.Dialog(modScanCorrect.ModeCorConstants.ModeCor_origin);
                //基準位置のゲイン校正(常に実行)
                if (Convert.ToBoolean(modScanCorrect.Flg_GainShiftScan))
                {
                    //frmGainCorResult.Instance.Dialog(modScanCorrect.ModeCorConstants.ModeCor_shift);     //シフト位置のゲイン校正(チェックがあれば実行)
                    //Rev23.20 左右シフト対応 by長野 2015/11/19
                    //Rev25.00 左右シフトOFF時は右側だけ表示 by長野 2016/07/05
                    frmGainCorResult.Instance.Dialog(modScanCorrect.ModeCorConstants.ModeCor_shift_R);     //シフト位置のゲイン校正(チェックがあれば実行)
                    //if (CTSettings.scaninh.Data.lr_sft == 0)
                    //Rev25.00 Wスキャンを追加 by長野 2016/07/07
                    if (CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn)
                    {
                        frmGainCorResult.Instance.Dialog(modScanCorrect.ModeCorConstants.ModeCor_shift_L);     //シフト位置のゲイン校正(チェックがあれば実行)
                    }
                }

				//フォームをアンロードする
				this.Close();

                //Rev20.00 dispose追加 by長野 2015/01/28
                this.Dispose();
			}
            else
            {
                //Rev26.00 失敗の場合は[ガイド]タブのスキャンエリア・条件の設定完了フラグを落とす add by chouno 2017/01/16
                frmScanControl.Instance.setScanAreaAndCmpFlg(false);

				//「データ収集異常終了」と表示
				stsGain.Status = StringTable.GC_STS_CAPT_NG;

				//PkeFPDの場合は元のゲイン校正画像をプリロードする     'v17.02追加 byやまおか 2010/07/15

                if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke))
                {
                    //変更2014/10/07hata_v19.51反映
#if (!NoCamera) //'v17.10条件追加 byやまおか 2010/07/28

                    //ret =Pulsar.PkeSetGainData(Pulsar.hPke, 0, ScanCorrect.Gain_Image_L, 1);
                    ret = Pulsar.PkeSetGainData(Pulsar.hPke, ScanCorrect.Gain_Image_L, 1, ScanCorrect.GAIN_CORRECT_L);//v18.00変更 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                                        
                    //ストリングテーブル化　'v17.60 by長野 2011/05/22
					//If ret = 1 Then MsgBox "ゲイン校正データをセットできませんでした。", vbCritical
                    //if (Ipc32v5.ret == 1)
                    if (ret == 1)
                    {
                        MessageBox.Show(CTResources.LoadResString(20004), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
#endif
				}

        		//ビジーオフ
				IsBusy = false;
			}
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
            //フォームをアンロードする
			this.Close();
            //Rev20.00 dispose追加 by長野 2015/01/28
            this.Dispose(); ;
		}

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
        private void chkHaFuOfScan_CheckStateChanged(object sender, EventArgs e)
        {

            //常にチェックＯＮにする
            chkHaFuOfScan.CheckState = CheckState.Checked;

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
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void chkGainTableRot_CheckStateChanged(object sender, EventArgs e)
		{
			//最低ビュー数を求める（テーブル回転する・しないによって変更する）
            //変更2015/02/02hata_Max/Min範囲のチェック
            //cwneScanView.Minimum = modSeqComm.GetViewMin(chkGainTableRot.CheckState == CheckState.Checked);
            int _viewmini = modSeqComm.GetViewMin(chkGainTableRot.CheckState == CheckState.Checked);
            if (_viewmini > cwneScanView.Value) cwneScanView.Value = _viewmini;
            cwneScanView.Minimum = _viewmini;

			//ビュー数の範囲を表示
            lblViewMinMax.Text = StringTable.GetResString(StringTable.IDS_Range, cwneScanView.Minimum.ToString(), cwneScanView.Maximum.ToString());
		}

        //*************************************************************************************************
        //機　　能： テーブル下降収集チェックボックスクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void chkDownTable_CheckStateChanged(object sender, EventArgs e)
		{
			//テーブル下降収集チェックボックスにチェックが入っている時のみ，
			//テーブル下降収集用のストローク数を編集可にする
			cwneDownTableDistance.Enabled = (chkDownTable.CheckState == CheckState.Checked);
		}

        //*************************************************************************************************
        //機　　能： テーブル下降収集指定ストローク変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 指定ストロークがテーブル下限を超える場合は入力をキャンセルする
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void cwneDownTableDistance_ValueChanged(object sender, EventArgs e)
        {

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

        //追加2015/01/28hata
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

        //追加2015/01/28hata
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

        //*************************************************************************************************
        //機　　能： テーブルY軸移動収集チェックボックスクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V20.00  15/02/16 (検S1)長野　　　　　　      新規作成
        //*************************************************************************************************
        private void chkGainTableYAxis_CheckStateChanged(object sender, EventArgs e)
        {
            //テーブルY軸移動収集チェックボックスにチェックが入っている時のみ，
            //テーブルY軸移動収集用のストローク数を編集可にする
            cwneTableYAxisDistance.Enabled = (chkGainTableYAxis.CheckState == CheckState.Checked);
        }

        //*************************************************************************************************
        //機　　能： テーブルFCD軸移動収集チェックボックスクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.40  16/06/19 (検S1)長野　　　　　　      新規作成
        //*************************************************************************************************
        private void chkGainTableXAxis_CheckStateChanged(object sender, EventArgs e)
        {
            //テーブルFCD軸移動収集チェックボックスにチェックが入っている時のみ，
            //テーブルFCD軸移動収集用のストローク数を編集可にする
            cwneTableXAxisDistance.Enabled = (chkGainTableXAxis.CheckState == CheckState.Checked);
        }

        //*************************************************************************************************
        //機　　能： テーブル下降収集指定ストローク変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 指定ストロークがテーブル下限を超える場合は入力をキャンセルする
        //
        //履　　歴： V20.00  15/02/16 (検S1)長野　　　　　　      新規作成
        //*************************************************************************************************
        private void cwneTableYAxisDistance_ValueChanged(object sender, EventArgs e)
        {

            //【C#コントロールで代用】
            bool bnoval = false;    //追加2015/01/28hata
            float TableYAxisDistance = 0.0f;

            //変更2015/01/28hata
            //float.TryParse(cwneDownTableDistance.Text, out downTableDistance);
            if (!float.TryParse(cwneTableYAxisDistance.Text, out TableYAxisDistance))
                bnoval = true;

            //追加2015/01/28hata
            if (string.IsNullOrEmpty(PreYAxisMoveValText)) PreYAxisMoveValText = "0";
            if (string.IsNullOrEmpty(PreYAxisMoveEnterValText)) PreYAxisMoveEnterValText = "0";

            //変更2015/01/28hata_if文追加
            if (presskye == (char)Keys.Return)
            {
                //frmMechaControl.Instance.ntbTableXPos.Value：現在の昇降絶対位置の取得
                //if (modMecainf.mecainf.udab_pos + eventArgs.value > modGlobal.GValUpperLimit)
                if ((float)frmMechaControl.Instance.ntbTableXPos.Value + TableYAxisDistance > CTSettings.t20kinf.Data.y_axis_upper_limit)
                {
                    //メッセージ表示：
                    //   テーブルY軸の上限を超えて下降しようとしています！
                    //   現在のY軸位置(mm): xxx
                    //   テーブルY軸上限値(mm): xxx
                    MessageBox.Show(StringTable.GetResString(9585, "\t" + Convert.ToString((float)frmMechaControl.Instance.ntbTableXPos.Value), "\t" + Convert.ToString(CTSettings.t20kinf.Data.y_axis_upper_limit)),
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //eventArgs.value = eventArgs.previousValue;

                    //追加2015/01/28hata
                    cwneTableYAxisDistance.Text = PreYAxisMoveEnterValText;
                    presskye = (char)0;
                    return;
                }
                //frmMechaControl.Instance.ntbTableXPos.Value：現在の昇降絶対位置の取得
                //if (modMecainf.mecainf.udab_pos + eventArgs.value > modGlobal.GValUpperLimit)
                else if ((float)frmMechaControl.Instance.ntbTableXPos.Value + TableYAxisDistance < CTSettings.t20kinf.Data.y_axis_lower_limit)
                {
                    //メッセージ表示：
                    //   テーブルY軸の下限を超えて下降しようとしています！
                    //   現在のY軸位置(mm): xxx
                    //   テーブルY軸下限値(mm): xxx
                    MessageBox.Show(StringTable.GetResString(9587, "\t" + Convert.ToString((float)frmMechaControl.Instance.ntbTableXPos.Value), "\t" + Convert.ToString(CTSettings.t20kinf.Data.y_axis_lower_limit)),
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //eventArgs.value = eventArgs.previousValue;

                    //追加2015/01/28hata
                    cwneTableYAxisDistance.Text = PreYAxisMoveEnterValText;
                    presskye = (char)0;
                    return;
                }

                PreYAxisMoveEnterValText = TableYAxisDistance.ToString();
            }
            else
            {
                if (bnoval)
                {
                    //(.)のチェック
                    //(.)が2つ以上ある場合
                    int pos0 = cwneTableYAxisDistance.Text.IndexOf(".");
                    int pos1 = cwneTableYAxisDistance.Text.LastIndexOf(".");
                    if (pos0 != pos1)
                    {
                        //前回値に戻す
                        cwneTableYAxisDistance.Text = PreYAxisMoveValText;
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
                PreYAxisMoveValText = cwneTableYAxisDistance.Text;
            }
            presskye = (char)0;
        }

        //Rev20.00 追加 2015/02/16 by長野
        private void cwneTableYAxisDistance_KeyPress(object sender, KeyPressEventArgs e)
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
                case (char)45:  //(-)マイナスキー　

                case (char)46:  //(.)dotキー          
                    break;
                case (char)Keys.Return:
                    cwneTableYAxisDistance_ValueChanged(sender, EventArgs.Empty);
                    break;
                default:
                    e.KeyChar = (char)0;
                    e.Handled = true;
                    break;
            }
        }

        //Rev20.00 追加 2015/02/16 by長野
        private void cwneTableYAxisDistance_Leave(object sender, EventArgs e)
        {
            float yAxisTableDistance = 0f;
            if (string.IsNullOrEmpty(PreYAxisMoveEnterValText)) PreYAxisMoveEnterValText = "0";
            if (!float.TryParse(cwneTableYAxisDistance.Text, out yAxisTableDistance))
            {
                if (cwneTableYAxisDistance.Text == "") cwneTableYAxisDistance.Text = PreYAxisMoveEnterValText;
            }
            else
            {
                cwneTableYAxisDistance.Text = yAxisTableDistance.ToString();
            }
            presskye = (char)0;
            cwneTableYAxisDistance_ValueChanged(sender, EventArgs.Empty);
        }

        //*************************************************************************************************
        //機　　能： テーブルFCD軸収集指定ストローク変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 指定ストロークがテーブル下限を超える場合は入力をキャンセルする
        //
        //履　　歴： V23.40  16/06/19 (検S1)長野　　　　　　      新規作成
        //*************************************************************************************************
        private void cwneTableXAxisDistance_ValueChanged(object sender, EventArgs e)
        {

            //【C#コントロールで代用】
            bool bnoval = false;    //追加2015/01/28hata
            float TableXAxisDistance = 0.0f;

            //変更2015/01/28hata
            //float.TryParse(cwneDownTableDistance.Text, out downTableDistance);
            if (!float.TryParse(cwneTableXAxisDistance.Text, out TableXAxisDistance))
                bnoval = true;

            //追加2015/01/28hata
            if (string.IsNullOrEmpty(PreXAxisMoveValText)) PreXAxisMoveValText = "0";
            if (string.IsNullOrEmpty(PreXAxisMoveEnterValText)) PreXAxisMoveEnterValText = "0";

            //変更2015/01/28hata_if文追加
            if (presskye == (char)Keys.Return)
            {
                //frmMechaControl.Instance.ntbTableXPos.Value：現在の昇降絶対位置の取得
                //if (modMecainf.mecainf.udab_pos + eventArgs.value > modGlobal.GValUpperLimit)
                if ((float)frmMechaControl.Instance.ntbFCD.Value + TableXAxisDistance > CTSettings.mechapara.Data.max_fcd)
                {
                    //メッセージ表示：
                    //   テーブルFCD軸の上限を超えて移動しようとしています！
                    //   現在のFCD軸位置(mm): xxx
                    //   テーブルFCD軸上限値(mm): xxx
                    MessageBox.Show(StringTable.GetResString(9588, "\t" + Convert.ToString((float)frmMechaControl.Instance.ntbFCD.Value), "\t" + Convert.ToString(CTSettings.mechapara.Data.max_fcd)),
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //eventArgs.value = eventArgs.previousValue;

                    //追加2015/01/28hata
                    cwneTableYAxisDistance.Text = PreYAxisMoveEnterValText;
                    presskye = (char)0;
                    return;
                }
                //frmMechaControl.Instance.ntbTableXPos.Value：現在の昇降絶対位置の取得
                //if (modMecainf.mecainf.udab_pos + eventArgs.value > modGlobal.GValUpperLimit)
                else if ((float)frmMechaControl.Instance.ntbFCD.Value + TableXAxisDistance < CTSettings.mechapara.Data.min_fcd)
                {
                    //メッセージ表示：
                    //   テーブルFCD軸の下限を超えて移動しようとしています！
                    //   現在のFCD軸位置(mm): xxx
                    //   テーブルFCD軸下限値(mm): xxx
                    MessageBox.Show(StringTable.GetResString(9589, "\t" + Convert.ToString((float)frmMechaControl.Instance.ntbFCD.Value), "\t" + Convert.ToString(CTSettings.mechapara.Data.min_fcd)),
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //eventArgs.value = eventArgs.previousValue;

                    //追加2015/01/28hata
                    cwneTableYAxisDistance.Text = PreYAxisMoveEnterValText;
                    presskye = (char)0;
                    return;
                }

                PreXAxisMoveEnterValText = TableXAxisDistance.ToString();
            }
            else
            {
                if (bnoval)
                {
                    //(.)のチェック
                    //(.)が2つ以上ある場合
                    int pos0 = cwneTableXAxisDistance.Text.IndexOf(".");
                    int pos1 = cwneTableXAxisDistance.Text.LastIndexOf(".");
                    if (pos0 != pos1)
                    {
                        //前回値に戻す
                        cwneTableXAxisDistance.Text = PreXAxisMoveValText;
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
                PreXAxisMoveValText = cwneTableXAxisDistance.Text;
            }
            presskye = (char)0;
        }

        //Rev23.40 追加 2016/06/19 by長野
        private void cwneTableXAxisDistance_KeyPress(object sender, KeyPressEventArgs e)
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
                case (char)45:  //(-)マイナスキー　

                case (char)46:  //(.)dotキー          
                    break;
                case (char)Keys.Return:
                    cwneTableXAxisDistance_ValueChanged(sender, EventArgs.Empty);
                    break;
                default:
                    e.KeyChar = (char)0;
                    e.Handled = true;
                    break;
            }
        }

        //Rev23.40 追加 2016/06/19 by長野
        private void cwneTableXAxisDistance_Leave(object sender, EventArgs e)
        {
            float xAxisTableDistance = 0f;
            if (string.IsNullOrEmpty(PreXAxisMoveEnterValText)) PreXAxisMoveEnterValText = "0";
            if (!float.TryParse(cwneTableXAxisDistance.Text, out xAxisTableDistance))
            {
                if (cwneTableXAxisDistance.Text == "") cwneTableXAxisDistance.Text = PreXAxisMoveEnterValText;
            }
            else
            {
                cwneTableXAxisDistance.Text = xAxisTableDistance.ToString();
            }
            presskye = (char)0;
            cwneTableXAxisDistance_ValueChanged(sender, EventArgs.Empty);
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
        private void frmGainCor_Load(object sender, EventArgs e)
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

			//v17.60 英語用レイアウト調整 by長野 2011/05/25
			if (modCT30K.IsEnglish)
            {
				EnglishAdjustLayout();
			}

			//ビジーオフ
			IsBusy = false;

			//停止中
			stsGain.Status = StringTable.GC_STS_STOP;
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
		private void frmGainCor_FormClosed(object sender, FormClosedEventArgs e)
		{
			//ビジーオフ
			IsBusy = false;

			//終了時はフラグをリセット
			modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTScanCorrect);

            //Rev20.01 追加 by長野 2015/06/03
            frmXrayControl.Instance.UpdateWarmUp();

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
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void SetCaption()
		{
			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);

            //追加2014/10/07hata_v19.51反映
            //v19.51 X線・検出器昇降かテーブル昇降の場合で、captionを変更 by長野 2014/02/26
            if (CTSettings.t20kinf.Data.ud_type == 0)
            {
                chkDownTable.Text = CTResources.LoadResString(12116);
            }
            else
            {
                chkDownTable.Text = CTResources.LoadResString(12120);
            }

			lblMAUni.Text = modXrayControl.CurrentUni;                                                      //μA
			chkGainTableRot.Text = StringTable.BuildResStr(StringTable.IDS_Rotate, StringTable.IDS_Table);  //テーブル回転

            //追加2014/10/07hata_v19.51反映
            //スキャン収集     'v18.00追加 byやまおか 2011/03/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            chkHaFuOfScan.Text = CTResources.LoadResString(StringTable.IDS_Half) + ", " +  
                                 CTResources.LoadResString(StringTable.IDS_Full) + ", " + 
                                 CTResources.LoadResString(StringTable.IDS_Offset) +
                                 CTResources.LoadResString(StringTable.IDS_ScanCollection);//ハーフフルオフセット

            //Rev25.00 Wスキャン用に条件を追加 by長野 2016/07/07
            //Rev26.10 シフトスキャンの名称がWスキャンの場合は、Wスキャンで表示 by chouno 2018/01/16
            if (CTSettings.W_ScanOn || CTSettings.infdef.Data.scan_mode[3].GetString() == CTResources.LoadResString(25009))
            {
                chkShiftScan.Text = "W" + CTResources.LoadResString(StringTable.IDS_ScanCollection); //Wスキャン
            }
            else
            {
                chkShiftScan.Text = CTResources.LoadResString(StringTable.IDS_Shift) + CTResources.LoadResString(StringTable.IDS_ScanCollection); //シフト
            }


            //変更2014/10/07hata_v19.51反映
            //産業用CTモードの場合   'v18.00条件追加 byやまおか 2011/03/14 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if ((CTSettings.scaninh.Data.avmode == 0))
            {
                //メッセージ
                if (CTSettings.scaninh.Data.xray_remote == 0)
                {
                    //準備ができたらＯＫをクリックしてください。
                    lblMessage.Text = lblMessage.Text;
                }
                else
                {
                    //必要に応じて管電流値を小さくして、Ｘ線をオンしてください。
                    //準備ができたらＯＫをクリックしてください。
                     lblMessage.Text = CTResources.LoadResString(9567) + "\r\n" + lblMessage.Text;
                }

            //マイクロCTの場合
            }
            else
            {
			    //メッセージ
			    if (CTSettings.scaninh.Data.xray_remote == 0)
                {
				    //必要に応じて試料テーブルにゲイン校正ファントムを取り付けてください。
				    //準備ができたらＯＫをクリックしてください。
                    lblMessage.Text = CTResources.LoadResString(9570) + "\r\n" + lblMessage.Text;
			    }
                else
                {
				    //必要に応じて試料テーブルにゲイン校正ファントムを取り付けたり管電流値を小さくしたりして、Ｘ線をオンしてください。
				    //準備ができたらＯＫをクリックしてください。
				    lblMessage.Text = CTResources.LoadResString(9569) + "\r\n" + lblMessage.Text;
			    }
            }

            //v17.60 フォントがバラつき不自然なため削除 by 長野 2011/06/11
			//英語環境の場合、ラベルコントロールに使用するフォントをArialにする
			//If IsEnglish Then SetLabelFont Me

        }

        //*************************************************************************************************
        //機　　能： 各コントロールの位置・サイズ等の初期化
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.2  2006/01/17   ????????      新規作成
        //*************************************************************************************************
		private void InitControls()
		{
            //Rev25.00 配置順に記述＋ visibleに従い位置調整 by長野 2016/08/03
            ////Ｘ線外部制御不可の場合、管電流の設定を隠す
            //fraTubeCurrent.Visible = (CTSettings.scaninh.Data.xray_remote == 0);

            ////テーブル下降収集の表示
            //fraTableDownAcquire.Visible = (CTSettings.scaninh.Data.table_down_acquire == 0);

            ////Rev20.00 追加 by長野 2015/02/16
            //fraTableYAxisMoveAcquire.Visible = (CTSettings.scaninh.Data.table_y_axis_move_acquire == 0);

            ////Rev25.00 追加 by長野 2016/08/03
            //fraTableXAxisMoveAcquire.Visible = (CTSettings.scaninh.Data.table_x_axis_move_acquire == 0);

            ////テーブル回転収集の表示(PkeFPDの場合は非表示)   'v17.00追加 byやまおか 2010/02/17
            //chkGainTableRot.Visible = !(CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);

            ////追加2014/10/07hata_v19.51反映
            ////シフトスキャン機能     'v18.00追加 byやまおか 2011/02/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            ////Rev25.00 Wスキャン追加 by長野 2016/06/19
            ////fraShiftScan.Visible = CTSettings.DetShiftOn;
            //fraShiftScan.Visible = (CTSettings.DetShiftOn || CTSettings.S_ScanOn);
            //fraShiftScan.BorderStyle = BorderStyle.None;

            ///////////////////////////////////////////////////////////////////////////////////
            int NextPosX = 0;
            int NextPosY = 0;

            //Ｘ線外部制御不可の場合、管電流の設定を隠す
            fraTubeCurrent.Visible = (CTSettings.scaninh.Data.xray_remote == 0);

            NextPosX = fraTubeCurrent.Location.X;
            NextPosY = fraTubeCurrent.Location.Y + fraTubeCurrent.Height;

            //追加2014/10/07hata_v19.51反映
            //シフトスキャン機能     'v18.00追加 byやまおか 2011/02/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            //fraShiftScan.Visible = CTSettings.DetShiftOn;
            fraShiftScan.Visible = (CTSettings.DetShiftOn || CTSettings.W_ScanOn);
            fraShiftScan.BorderStyle = BorderStyle.None;
            if (fraShiftScan.Visible == true)
            {
                fraShiftScan.SetBounds(NextPosX, NextPosY,fraShiftScan.Width,fraShiftScan.Height);
                NextPosY = fraShiftScan.Location.Y + fraShiftScan.Height;
            }

            //テーブル回転収集の表示(PkeFPDの場合は非表示)   'v17.00追加 byやまおか 2010/02/17
            chkGainTableRot.Visible = !(CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);
            if (chkGainTableRot.Visible == true)
            {
                chkGainTableRot.SetBounds(NextPosX, NextPosY, chkGainTableRot.Width, chkGainTableRot.Height);
                NextPosY = chkGainTableRot.Location.Y + chkGainTableRot.Height;
            }

            //Rev20.00 追加 by長野 2015/02/16
            fraTableYAxisMoveAcquire.Visible = (CTSettings.scaninh.Data.table_y_axis_move_acquire == 0);
            if (fraTableYAxisMoveAcquire.Visible == true)
            {
                fraTableYAxisMoveAcquire.SetBounds(NextPosX, NextPosY, fraTableYAxisMoveAcquire.Width, fraTableXAxisMoveAcquire.Height);
                NextPosY = fraTableYAxisMoveAcquire.Location.Y + fraTableYAxisMoveAcquire.Height;
            }

            //Rev25.00 追加 by長野 2016/08/03
            fraTableXAxisMoveAcquire.Visible = (CTSettings.scaninh.Data.table_x_axis_move_acquire == 0);
            if (fraTableXAxisMoveAcquire.Visible == true)
            {
                fraTableXAxisMoveAcquire.SetBounds(NextPosX, NextPosY, fraTableXAxisMoveAcquire.Width, fraTableXAxisMoveAcquire.Height);
                NextPosY = fraTableXAxisMoveAcquire.Location.Y + fraTableXAxisMoveAcquire.Height;
            }

            //テーブル下降収集の表示
            fraTableDownAcquire.Visible = (CTSettings.scaninh.Data.table_down_acquire == 0);
            if (fraTableDownAcquire.Visible == true)
            {
                fraTableDownAcquire.SetBounds(NextPosX, NextPosY, fraTableDownAcquire.Width, fraTableDownAcquire.Height);
            }
        }

        //*************************************************************************************************
        //機    能  ：  ゲイン校正の設定値を表示する
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  初期値は下記のように指定する。
        //
        //                ﾋﾞｭｰ数       = コモン
        //                積算枚数     = コモン
        //                ﾋﾞｭｰ数の範囲 = コモン
        //
        //履    歴  ：  V2.0   00/02/08  (SI1)鈴山       新規作成
        //*************************************************************************************************
		private void SetControls()
		{
            //追加2014/11/28hata_v19.51_dnet
            //イベントを発生させる
            chkGainTableRot_CheckStateChanged(chkGainTableRot, EventArgs.Empty);
            
            //ビュー数
            cwneScanView.Maximum = CTSettings.GVal_ViewMax;
            //変更2015/02/02hata_Max/Min範囲のチェック
			//cwneScanView.Value = modScanCorrect.ViewNumAtGain;
            cwneScanView.Value = modLibrary.CorrectInRange(modScanCorrect.ViewNumAtGain, cwneScanView.Minimum, cwneScanView.Maximum);

			//積算枚数
            //変更2014/10/07hata_v19.51反映
            //cwneSum.Value = modScanCorrect.IntegNumAtGain;
            //最小値・最大値の設定
            //   最小:infdef.min_integ_number
            //   最大:infdef.max_integ_number
            //cwneSum.SetMinMax(CTSettings.GValIntegNumMin, CTSettings.GValIntegNumMax);
            if (cwneSum.Value > CTSettings.GValIntegNumMax) cwneSum.Value = CTSettings.GValIntegNumMax;
            if (cwneSum.Value < CTSettings.GValIntegNumMin) cwneSum.Value = CTSettings.GValIntegNumMin;
            cwneSum.Maximum = CTSettings.GValIntegNumMax;
            cwneSum.Minimum = CTSettings.GValIntegNumMin;

            //最大値・最小値の表示
            lblIntegMinMax.Text = StringTable.GetResString(StringTable.IDS_Range, cwneSum.Minimum.ToString(), cwneSum.Maximum.ToString());

            //値の設定
            //変更2014/11/28hata_v19.51_dnet
            //数字の初期値は10単位にする
            //cwneSum.Value = ScanCorrect.IntegNumAtRot;
            cwneSum.Value = ScanCorrect.RoundControlVale(ScanCorrect.IntegNumAtRot, cwneSum.Maximum, cwneSum.Minimum, 10F);

            //シフトスキャン収集     'v18.00追加 byやまおか 2011/02/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            //if (CTSettings.DetShiftOn)
            if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
            {
                //Rev25.00 Wスキャン追加 by長野 2016/07/07
                //chkShiftScan.CheckState = (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift ? CheckState.Checked : CheckState.Unchecked);
                if ((CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift) || CTSettings.scansel.Data.w_scan == 1)
                {
                    chkShiftScan.CheckState = CheckState.Checked;
                }
                else
                {
                    chkShiftScan.CheckState = CheckState.Unchecked;
                }
            }

			//管電流：Ｘ線制御画面のコントロールをコピー
			//v15.10変更 byやまおか 2009/10/29
            if (CTSettings.scaninh.Data.xray_remote == 0)
            {
				modLibrary.CopyCWNumEdit(frmXrayControl.Instance.cwneMA, cwneMA);
			}

			//テーブル回転有無   'added by 山本 2002-10-5
			chkGainTableRot.CheckState = modScanCorrect.GFlg_GainTableRot;  //chkGainTableRot_Click が実行される

			//テーブル下降収集用変数の値を反映させる        
            presskye = (char)0; //追加2015/01/28hata
            chkDownTable.CheckState = modScanCorrect.DownTable;             //テーブル下降収集の有無
			cwneDownTableDistance.Text = modScanCorrect.DownTableDistance.ToString(); //テーブル下降収集用のストローク数（移動距離）（mm）//【C#コントロールで代用】

            //Rev20.00 追加 by長野 2015/02/16
            chkGainTableYAxis.CheckState = modScanCorrect.yAxisMoveTable;
            cwneTableYAxisDistance.Text = modScanCorrect.yAxisMoveTableDistance.ToString();
            chkGainTableYAxis_CheckStateChanged(chkGainTableYAxis, EventArgs.Empty);

            //Rev23.40 追加 by長野 2016/06/19
            chkGainTableXAxis.CheckState = modScanCorrect.xAxisMoveTable;
            cwneTableXAxisDistance.Text = modScanCorrect.xAxisMoveTableDistance.ToString();
            chkGainTableXAxis_CheckStateChanged(chkGainTableXAxis, EventArgs.Empty);

            //追加2014/11/28hata_v19.51_dnet
            //イベントを発生させる
            chkDownTable_CheckStateChanged(chkDownTable, EventArgs.Empty);
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
        //履　　歴： V17.60  11/05/25 (検S1) 長野    新規作成
        //*************************************************************************************************
    	private void EnglishAdjustLayout()
		{
            //2014/11/06hata キャストの修正
            this.Width = Convert.ToInt32(this.Width * 1.05F);

            //2014/11/06hata キャストの修正
            lblViewNum.Left = 3;
            lblIntegNum.Left = 3;
            
            fraTubeCurrent.Width = 333;
            //Rev20.01 変更 by長野 2015/05/19
            //fraTubeCurrent.Left = 3;
            fraTubeCurrent.Left = 0;
            
            lblColon3.Left = lblColon1.Left;
            cwneMA.Left = cwneSum.Left;
            lblMAUni.Left = lblIntegNumUni.Left;

            //Rev20.01 追加 by長野 2015/05/19
            cwneTableYAxisDistance.Left = cwneTableYAxisDistance.Left + 25;
            label1.Left = label1.Left + 25;

            //Rev20.01 追加 by長野 2015/05/19
            cwneDownTableDistance.Left = cwneDownTableDistance.Left + 25;
            lblUDUni.Left = lblUDUni.Left + 25;
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
        private void cwneScanView_ValueChanged(object sender, EventArgs e)
        {
            decimal val1 = 0;

            //数字のインクリメントを合わせる10,100,200･･･
            //val1 = cwneScanView.Value / (decimal)100.0;
            //val1 = Math.Round(val1, 0, MidpointRounding.AwayFromZero) * 100;
            //if (val1 < cwneScanView.Minimum) val1 = cwneScanView.Minimum;
            //if (val1 > cwneScanView.Maximum) val1 = cwneScanView.Maximum;
            val1 = ScanCorrect.RoundControlVale(cwneScanView.Value, cwneScanView.Maximum, cwneScanView.Minimum, 100F);
            if (cwneScanView.Value != val1)
            {
                cwneScanView.Value = val1;
            }
        }




    }
}
