using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CT30K.Common;
using CTAPI;

namespace CT30K
{
	public partial class frmMechaAllResetMove : Form
	{
		// 【C#コントロールで代用】		CWButton → Button
		private static frmMechaAllResetMove _Instance = null;

        //CT30K起動時か              'v17.51追加 by 間々田 2011/03/24
        bool IsStartUp;


		public frmMechaAllResetMove()
		{
			InitializeComponent();
		}

		public static frmMechaAllResetMove Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmMechaAllResetMove();
				}

				return _Instance;
			}
		}


		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''


        //'*************************************************************************************************
		//'機　　能： 「閉じる」ボタンクリック時処理
		//'
		//'           変数名          [I/O] 型        内容
		//'引　　数： なし
		//'戻 り 値： なし
		//'
		//'補　　足： なし
		//'
		//'履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//'*************************************************************************************************
        private void cmdClose_Click(System.Object eventSender, System.EventArgs eventArgs)
        {

            //cmdClose.Enabled = True
            //Screen.MousePointer = vbDefault
            //frmCTMenu.Enabled = True

            //v17.50上記を変更 by 間々田 2011/03/17
            cwbtnMechaAllResetMove.Value = false;

            //確実に消去                                                         'v17.51追加 by 間々田 2011/03/24
            //変更2015/1/17hata_非表示のときにちらつくため
            //Hide();
            modCT30K.FormHide(this);

            //フォームのアンロード
            this.Close();

            //CT30K起動時でかつ検出器不定ではないとき、ここでウォームアップを促す 'v17.51追加 by 間々田 2011/03/24
            if (IsStartUp & (mod2ndDetctor.DetMode != mod2ndDetctor.DetModeConstants.DetMode_None))
            {
                frmXrayControl.Instance.QueryWarmup();
            }
        }

		//'*************************************************************************************************
		//'機　　能： ＯＫ／停止ボタン切り替え時処理
		//'
		//'           変数名          [I/O] 型        内容
		//'引　　数： なし
		//'戻 り 値： なし
		//'
		//'補　　足： なし
		//'
		//'履　　歴： v17.5 2011/03/17 (豊川)間々田    新規作成
		//'*************************************************************************************************
        //private void cwbtnMechaAllResetMove_ValueChanged(System.Object eventSender, AxCWUIControlsLib._DCWButtonEvents_ValueChangedEvent eventArgs)
        private void cwbtnMechaAllResetMove_ValueChanged(object sender, EventArgs e)
        {

            //「閉じる」ボタンのEnabledプロパティを制御
            cmdClose.Enabled = !cwbtnMechaAllResetMove.Value;

            //メイン画面のEnabledプロパティを制御
            frmCTMenu.Instance.Enabled = !cwbtnMechaAllResetMove.Value;

            //マウスポインタの制御
            System.Windows.Forms.Cursor.Current = (cwbtnMechaAllResetMove.Value ? Cursors.AppStarting : Cursors.Default);

            //ボタンの文言を変更
            var _with1 = cwbtnMechaAllResetMove;
            //cwbtnMechaAllResetMove.OnText = "停止"
            //cwbtnMechaAllResetMove.OffText = "停止"
            //cwbtnMechaAllResetMove.OnText = CT30K.My.Resources.str20021;            //ストリングテーブル化 'v17.60 by長野 2011/05/22
            //cwbtnMechaAllResetMove.OffText = CT30K.My.Resources.str20021;            //ストリングテーブル化 'v17.60 by長野 2011/05/22
            cwbtnMechaAllResetMove.Caption = CTResources.LoadResString(20021);          //ストリングテーブル化 'v17.60 by長野 2011/05/22

        }
        
        //
		//'*************************************************************************************************
		//'機　　能： メカオールリセット中の処理
		//'
		//'           変数名          [I/O] 型        内容
		//'引　　数： なし
		//'戻 り 値： なし
		//'
		//'補　　足： なし
		//'
		//'履　　歴： v17.20  2010/08/31 (検S1)長野  新規
		//'           v17.50  2011/03/17 (豊川)間々田 コード見直し
		//'*************************************************************************************************
        private void cwbtnMechaAllResetMove_Click(object sender, EventArgs e)
        {
            //-->v17.50削除 なおシーケンサの返すステータスにCT30Kが設定してはいけない 2011/03/17 by 間々田
            //    Dim StartTime As Variant    '開始時間(秒)
            //    Dim PauseTime As Variant    '待ち時間(秒)
            //
            //    Const thePauseTime As Long = 110 'タイムアウト時間(秒)
            //
            //    Dim RstFlg As Long
            //    RstFlg = 0
            //
            //    MechaResetStopflg = False
            //
            //    With cwbtnMechaAllResetMove
            //
            //    'メニュー画面にロックをかける
            //    frmCTMenu.Enabled = False
            //    Screen.MousePointer = vbHourglass
            //    cmdClose.Enabled = False
            //
            //    cwbtnMechaAllResetMove.Value = Not cwbtnMechaAllResetMove.Value
            //
            //    If .Value Then
            //
            //    Call SeqBitWrite("MechaReset", True)
            //    MySeq.stsMechaRstBusy = True
            //    cwbtnMechaAllResetMove.OnText = "停止"
            //    cwbtnMechaAllResetMove.OffText = "停止"
            //
            //    'メカリセットのステータスがtrueでも必ずメカリセットは行うようにステータスを変更
            //    MySeq.stsMechaRstOK = False
            //
            //    PauseForDoEvents 0.5
            //
            //    '開始時間を設定
            //    StartTime = Now
            //    PauseTime = thePauseTime
            //
            //    'Do While (DateDiff("s", StartTime, Now) < PauseTime)
            //     Do While Not MySeq.stsMechaRstOK
            //
            //        '0.5秒おきにチェック
            //        PauseForDoEvents 0.5
            //
            //        If MechaResetStopflg Then
            //           RstFlg = 1
            //           MySeq.stsMechaRstBusy = False
            //           MechaAllResetEndProcess (RstFlg)
            //           Exit Sub
            //
            //        End If
            //
            //        'タイムアウト
            //        If DateDiff("s", StartTime, Now) > PauseTime Then
            //
            //           Call SeqBitWrite("MechaResetStop", True)
            //           MySeq.stsMechaRstBusy = False
            //           RstFlg = 2
            //           MechaAllResetEndProcess (RstFlg)
            //
            //           Exit Sub
            //
            //        End If
            //
            //        'インターロックOFF
            //        If (frmCTMenu.DoorStatus = DoorOpened) Then
            //
            //            Call SeqBitWrite("MechaResetStop", True)
            //            MySeq.stsMechaRstBusy = False
            //            RstFlg = 3
            //            MechaAllResetEndProcess (RstFlg)
            //            Exit Sub
            //
            //        End If
            //
            //        '非常停止スイッチ
            //        If (MySeq.stsEmergency = True) Then
            //
            //            Call SeqBitWrite("MechaResetStop", True)
            //            MySeq.stsMechaRstBusy = False
            //            '非常停止
            //            RstFlg = 4
            //            MechaAllResetEndProcess (RstFlg)
            //            Exit Sub
            //
            //        End If
            //
            //        'メニュー画面にロックをかける
            //        frmCTMenu.Enabled = False
            //        Screen.MousePointer = vbHourglass
            //        cmdClose.Enabled = False
            //
            //    Loop
            //
            //    'メカリセット完了
            //    RstFlg = 0
            //    MechaAllResetEndProcess (RstFlg)
            //    Exit Sub
            //
            //    Else
            //
            //    Call SeqBitWrite("MechaResetStop", True)
            //    RstFlg = 1
            //    MechaResetStopflg = True
            //    'MechaAllResetEndProcess (RstFlg)
            //    Exit Sub
            //
            //    End If
            //
            //    End With
            //<--v17.50削除 なおシーケンサの返すステータスにCT30Kが設定してはいけない 2011/03/17 by 間々田

            //↓v17.50以下に変更 2011/03/17 by 間々田

            #region 使用時デバッグが必要です。点滅処理

            //タイムアウト時間(秒)
            const int PauseTime = 110;

            DateTime StartTime = default(DateTime);

            string Result = null;
            var _with2 = cwbtnMechaAllResetMove;

            //すでにメカオールリセットが実行されている（「停止」ボタンがクリックされた）時
            if (_with2.Value)
            {
                //ボタンを使用不可にする
                _with2.Enabled = false;
                //メカリセット中フラグを落とす
                modSeqComm.SeqBitWrite("MechaResetStop", true);
                //メカリセット停止要求
                return;
            }

            //以下は「ＯＫ」ボタンがクリックされた時の処理
            if (!IsOkMechaReset())
                return;

            //ボタンをＯＮにする
            _with2.Value = true;

            //メカリセット中フラグを落とす
            modSeqComm.SeqBitWrite("MechaResetStop", false);
            //メカリセット停止フラグオフ要求
            modCT30K.PauseForDoEvents(1);
            //イベントを取る

            //メカリセット命令送信
            modSeqComm.SeqBitWrite("MechaReset", true);
            modCT30K.PauseForDoEvents(1);
            //イベントを取る

            //開始時間
            StartTime = DateTime.Now;

            //処理結果メッセージ
            //Result = "メカリセットを途中停止しました"
            Result = CTResources.LoadResString(20023);
            //v17.60 ストリングテーブル化 by長野 2011/05/25

            //メカリセット中の処理
            while (IsMechaResetBusy())
            {
                //タイムアウト？
                //if (DateAndTime.DateDiff(Microsoft.VisualBasic.DateInterval.Second, StartTime, DateAndTime.Now) > PauseTime)
                if ((DateTime.Now - StartTime).TotalSeconds > PauseTime)
                {
                    //Result = "メカリセットタイムアウトエラー"]
                    Result = CTResources.LoadResString(20024);              //ストリングテーブル化 v17.60  by長野 2011/05/25
                    modSeqComm.SeqBitWrite("MechaResetStop", true);         //メカリセット停止要求
                    break; 
                }
                else
                {
                    //IsMechaResetBusyをチェックしながら1秒待ち
                    PauseForDoEventsWithCheckingMechaResetBusy(1);
                }
            }

            //後処理：判定

            //インターロックOFF？
            if ((!modSeqComm.MySeq.stsDoorKey))
            {
                //Result = "検査室の扉が開いたため、" & vbCrLf & "メカリセットを途中停止しました"
                Result = CTResources.LoadResString(20025);                //v17.60 ストリングテーブル化 by 長野 2011/05/25
            }
            else if (modSeqComm.MySeq.stsEmergency)
            {
                //Result = "非常停止スイッチが押されたため、" & vbCrLf & "メカリセットを途中停止しました"
                Result = CTResources.LoadResString(20026);
            
            //正常完了
            }
            else if (modSeqComm.MySeq.stsMechaRstOK)
            {
                //Result = "メカリセットが完了しました"
                Result = CTResources.LoadResString(20022);                //ストリングテーブル化 by長野 2011/05/25
            }

            //処理結果の表示
            Label1.Text = Result;

            //ＯＫ／停止ボタンを非表示にする
            _with2.Visible = false;

            //↓v17.51追加 by 間々田 2011/03/23 念のため使用したシーケンサのフラグを落とす
            //メカリセット中フラグを落とす
            modSeqComm.SeqBitWrite("MechaResetStop", false);
            modCT30K.PauseForDoEvents(1);            //イベントを取る

            //メカリセット命令送信
            modSeqComm.SeqBitWrite("MechaReset", false);
            modCT30K.PauseForDoEvents(1);            //イベントを取る
            //↑v17.51追加 by 間々田 2011/03/23

            //画面を初期状態にする
            _with2.Value = false;

            #endregion
        }
        
        
		//'*************************************************************************************************
		//'機　　能： メカリセットの終了処理
		//'
		//'           変数名          [I/O] 型        内容
		//'引　　数： なし
		//'戻 り 値： なし
		//'
		//'補　　足： なし
		//'
		//'履　　歴： v17.20  2010/08/31 (検S1)長野  新規
		//'           v17.50  2011/03/17 (豊川)間々田 廃止：なお下の処理ではマウスポインタが砂時計のままになってしまう
		//'*************************************************************************************************
		//'Private Sub MechaAllResetEndProcess(ByVal RstFlg As Long)
		//'
		//'        Select Case (RstFlg)
		//'
		//'           Case 0
		//'
		//'           frmMechaAllResetMove.Label1 = "メカリセットが完了しました"
		//'
		//'           Case 1
		//'
		//'           frmMechaAllResetMove.Label1 = "メカリセットを途中停止しました"
		//'
		//'           Case 2
		//'
		//'           frmMechaAllResetMove.Label1 = "メカリセットタイムアウトエラー"
		//'
		//'           Case 3
		//'
		//'           frmMechaAllResetMove.Label1 = "検査室の扉が開いたため、" & vbCrLf & "メカリセットを途中停止しました"
		//'
		//'           Case 4
		//'
		//'           frmMechaAllResetMove.Label1 = "非常停止スイッチが押されたため、" & vbCrLf & "メカリセットを途中停止しました"
		//'
		//'        End Select
		//'
		//'        cwbtnMechaAllResetMove.Enabled = False
		//'        cwbtnMechaAllResetMove.Visible = False
		//'        cwbtnMechaAllResetMove.Value = False
		//'        cwbtnMechaAllResetMove.OnText = "OK"
		//'        cwbtnMechaAllResetMove.OffText = "OK"
		//'
		//'        cmdClose.Enabled = True
		//'        'メニュー画面にロックをかける
		//'        frmCTMenu.Enabled = False
		//'        Screen.MousePointer = vbHourglass
		//'
		//'End Sub

		//'*************************************************************************************************
		//'機　　能： フォームロード時の処理
		//'
		//'           変数名          [I/O] 型        内容
		//'引　　数： なし
		//'戻 り 値： なし
		//'
		//'補　　足： なし
		//'
		//'履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//'*************************************************************************************************
        private void frmMechaAllResetMove_Load(System.Object eventSender, System.EventArgs eventArgs)
        {

            //変数初期化             'v17.51追加 by 間々田 2011/03/24
            IsStartUp = false;

            //v17.60 ストリングテーブル化 by長野 2011/05/25
            StringTable.LoadResStrings(this);

            //    Label1.Caption = "テーブルX軸、Y軸、検出器(FDD)、検出器切替軸の" & _
            //'                     "メカリセットを行います。リセット中はテーブルがX線発生器の" & _
            //'                     "下まで移動します。ワークがX線発生器などに衝突する恐れ" & _
            //'                    "がありますので、ワークを一時、試料テーブルから取り外し" & _
            //'                    "てください。準備ができましたらOKを押してください。"
            //ストリングテーブル化 'v17.60 by 長野 2011/05/22
            Label1.Text = CTResources.LoadResString(20027) + "\r\n" +
                          CTResources.LoadResString(20028) + "\r\n" +
                          CTResources.LoadResString(20029) + "\r\n" +
                          CTResources.LoadResString(20030);

            //v17.60 ストリングテーブル化　bｙ長野 2011/05/25
            //cwbtnMechaAllResetMove.OnText = CT30K.My.Resources.str10001;
            //cwbtnMechaAllResetMove.OffText = CT30K.My.Resources.str10001;
            cwbtnMechaAllResetMove.Caption = CTResources.LoadResString(10001);
            
            this.Text = CTResources.LoadResString(20162);

            //ＯＫ／停止ボタンの設定
            //cwbtnMechaAllResetMove.Mode = CWUIControlsLib.CWButtonModes.cwModeIndicator;
            cwbtnMechaAllResetMove.Value = false;
            //v17.50追加 by 間々田 2011/03/17

        }


        //
        //   MySeq.stsMechaRstBusyを監視しつつ一定時間待つ    新規作成 by 間々田 2011/03/17
        //
		private void PauseForDoEventsWithCheckingMechaResetBusy(float PauseTime)
		{
			int StartTime = 0;

			//開始時間を設定
			StartTime = Winapi.GetTickCount();

			//一定時間待つ
			while ((Winapi.GetTickCount() < StartTime + PauseTime * 1000) & modSeqComm.MySeq.stsMechaRstBusy) {
				Application.DoEvents();
			}
		}

		//
		//'
		//'   MySeq.stsMechaRstBusyを返す
		//'   メカリセット中にMySeq.stsMechaRstBusyが落ちることがあるので     新規作成 by 間々田 2011/03/17
		//'
		private bool IsMechaResetBusy()
		{
			bool functionReturnValue = false;

			//メカリセットビジーの場合
			if (modSeqComm.MySeq.stsMechaRstBusy)
            {
				//それを戻り値とする
				functionReturnValue = true;

			//メカリセットビジーが落ちている場合
			} 
            else
            {
				//１秒待ってもう一度ステータスを取りにいく
				modCT30K.PauseForDoEvents(1);
				functionReturnValue = modSeqComm.MySeq.stsMechaRstBusy;
			}

            return functionReturnValue;

		}

		//'*************************************************************************************************
		//'機　　能： フォームの表示（CT30K起動時）
		//'
		//'           変数名          [I/O] 型        内容
		//'引　　数： なし
		//'戻 り 値： なし
		//'
		//'補　　足： なし
		//'
		//'履　　歴： v17.51  2011/03/24 (電Ｓ１)間々田     新規作成
        public void ShowAtStartup()
		{
			//検出器位置不定の場合はメカオールリセットを促す    'v17.51追加 by 間々田 2011/03/23
			Show(frmCTMenu.Instance);

			//CT30K起動時フラグセット
			IsStartUp = true;
		}
        
        
        //
		//'*************************************************************************************************
		//'機　　能： メカリセットが可能か
		//'
		//'           変数名          [I/O] 型        内容
		//'引　　数： なし
		//'戻 り 値： なし
		//'
		//'補　　足： なし
		//'
		//'履　　歴： v17.51  2011/03/24 (電Ｓ１)間々田     新規作成
		//'*************************************************************************************************
		private bool IsOkMechaReset()
		{
			bool functionReturnValue = false;

			//戻り値を初期化
			functionReturnValue = false;

			//運転準備ボタンが押されていなければ無効
			if (!modSeqComm.MySeq.stsRunReadySW) {
				//MsgBox "運転準備が未完のためメカリセットできません。" & vbCrLf & "運転準備スイッチを押して運転準備完了にしてください。", vbCritical
				//Interaction.MsgBox(CT30K.My.Resources.str20031 + CT30K.My.Resources.str20032);
                MessageBox.Show(CTResources.LoadResString(20031) + "\r\n" + CTResources.LoadResString(20032));
                return functionReturnValue;     //ストリングテーブル化　v17.60 by長野 2011/05/25
			}

			//メンテナンスのときは検査室扉が閉まっていることをチェックしない
			if (!modSeqComm.MySeq.stsDoorPermit) {

				//Ｘ線検査室の扉が開いている？
				if (!modSeqComm.MySeq.stsDoorKey) {
					//MsgBox "Ｘ線検査室の扉が開いているためメカリセットできません。" & vbCrLf & "X線検査室の扉を閉めてから､再度操作を行なって下さい｡", vbCritical
					//Interaction.MsgBox(CT30K.My.Resources.str20033 + CT30K.My.Resources.str20034);
                    MessageBox.Show(CTResources.LoadResString(20033) + "\r\n" + CTResources.LoadResString(20034));
                    return functionReturnValue;     //ストリングテーブル化 v17.60 by長野 2011/05/25
				}
			}

			//戻り値をセット
			functionReturnValue = true;
			return functionReturnValue;

        }

 
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
	}
}
