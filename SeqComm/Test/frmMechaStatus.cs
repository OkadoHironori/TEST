using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Test
{
	public partial class frmMechaStatus : Form
	{

  		public bool XStgFlg;
		public bool YStgFlg;

        public frmMechaControl fMCtrl = new frmMechaControl();

		//' Seq オブジェクトへの参照です。
        //private SeqComm.Seq withEventsField_commTest;
        public SeqComm.Seq commTest;
        //{
        //    get { return withEventsField_commTest; }
        //    set
        //    {
        //        if (withEventsField_commTest != null)
        //        {
        //            withEventsField_commTest.OnCommEnd -= commTest_OnCommEnd;
        //        }
        //        withEventsField_commTest = value;
        //        if (withEventsField_commTest != null)
        //        {
        //            withEventsField_commTest.OnCommEnd += commTest_OnCommEnd;
        //        }
        //    }
        //}

        //'#If AllTest Then
		// ''    Private WithEvents mctos As clsTActiveX
		//'#End If

        //シーケンサからの読み込み
		private void cmdSeqRead_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			if (Timer2.Enabled) {
				Timer2.Enabled = false;
				cmdSeqRead.BackColor = System.Drawing.SystemColors.Control;
			} else {
				Timer2.Enabled = true;
				cmdSeqRead.BackColor = System.Drawing.Color.Lime;
			}
		}

        //シーケンサへステータス送信
        private void cmdSeqWrite_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			if (Timer1.Enabled) {
				Timer1.Enabled = false;
				cmdSeqWrite.BackColor = System.Drawing.SystemColors.Control;
			} else {
				Timer1.Enabled = true;
				cmdSeqWrite.BackColor = System.Drawing.Color.Lime;
			}
		}


        private void commTest_OnCommEnd(int CommEndAns)
        {

#if AllTest
            int Ans = 0;
#endif

            if (Convert.ToBoolean(CommEndAns))
            {
                Console.Beep();
                MessageBox.Show("エラーコード ： " + CommEndAns, "通信エラー",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            else
            {
                var _with1 = commTest;


#if AllTest
                //ﾒｶﾃﾞﾊﾞｲｽ ｴﾗｰﾘｾｯﾄ
                if (_with1.DeviceErrReset)
                {
                    Ans = Module1.Mechaerror_reset(Module1.hDevID);
                }
                //昇降原点復帰
                if (_with1.UdOrigin)
                {
                    Ans = Module1.UdOrigin(Module1.hDevID, 0);
                }
                //回転原点復帰
                if (_with1.RotOrigin)
                {
                    Ans = Module1.RotateOrigin(Module1.hDevID, 0);
                }
                //微調X軸左
                if ((_with1.XStgLeft & (!XStgFlg)))
                {
                    Timer1.Enabled = false;
                    cmdSeqWrite.BackColor = System.Drawing.SystemColors.Control;
                    Ans = Module1.XStgManual(Module1.hDevID, 1, Convert.ToSingle(fMCtrl._txtSpeed_5.Text));
                    XStgFlg = true;
                }
                //微調X軸右
                if ((_with1.XStgRight & (!XStgFlg)))
                {
                    Timer1.Enabled = false;
                    cmdSeqWrite.BackColor = System.Drawing.SystemColors.Control;
                    Ans = Module1.XStgManual(Module1.hDevID, 0, Convert.ToSingle(fMCtrl._txtSpeed_5.Text));
                    XStgFlg = true;
                }
                if (((!_with1.XStgLeft) & (!_with1.XStgRight) & XStgFlg))
                {
                    Ans = Module1.XStgFastStop(Module1.hDevID, 0);
                    XStgFlg = false;
                    Timer1.Enabled = true;
                    cmdSeqWrite.BackColor = System.Drawing.Color.Lime;
                }
                //微調X軸原点復帰
                if (_with1.XStgOrigin)
                {
                    Ans = Module1.XStgOrigin(Module1.hDevID, 0);
                }
                //微調Y軸前進
                if ((_with1.YStgForward & (!YStgFlg)))
                {
                    Timer1.Enabled = false;
                    cmdSeqWrite.BackColor = System.Drawing.SystemColors.Control;
                    Ans = Module1.YStgManual(Module1.hDevID, 1, Convert.ToSingle(fMCtrl._txtSpeed_6.Text));
                    YStgFlg = true;
                }
                //微調Y軸後退
                if ((_with1.YStgBackward & (!YStgFlg)))
                {
                    Timer1.Enabled = false;
                    cmdSeqWrite.BackColor = System.Drawing.SystemColors.Control;
                    Ans = Module1.YStgManual(Module1.hDevID, 0, Convert.ToSingle(fMCtrl._txtSpeed_6.Text));
                    YStgFlg = true;
                }
                if (((!_with1.YStgForward) & (!_with1.YStgBackward) & YStgFlg))
                {
                    Ans = Module1.YStgFastStop(Module1.hDevID, 0);
                    YStgFlg = false;
                    Timer1.Enabled = true;
                    cmdSeqWrite.BackColor = System.Drawing.Color.Lime;
                }
                //微調Y軸原点復帰
                if (_with1.YStgOrigin)
                {
                    Ans = Module1.YStgOrigin(Module1.hDevID, 0);
                }
                //                'X線ON
                //                If .XrayOn Then
                //                    Set mctos = New clsTActiveX
                //                    mctos.Xrayonoff_Set (1)
                //                End If
                //                'X線OFF
                //               If .XrayOff Then
                //                    mctos.Xrayonoff_Set (2)
                //                End If

#endif


                //            '表示
                //            lblPcInhibit.Caption = .PcInhibit
                //            lblDoorInterlock.Caption = .stsDoorInterlock
                //            lblEmergency.Caption = .stsEmergency
                //            lblXray225Trip.Caption = .stsXray225Trip
                //            lblXray160Trip.Caption = .stsXray160Trip
                //            lblFilterTouch.Caption = .stsFilterTouch
                //            lblXray225Touch.Caption = .stsXray225Touch
                //            lblXray160Touch.Caption = .stsXray160Touch
                //            lblRotTouch.Caption = .stsRotTouch
                //            lblTiltTouch.Caption = .stsTiltTouch
                //            lblXDriveHeat.Caption = .stsXDriverHeat
                //            lblYDriveHeat.Caption = .stsYDriverHeat
                //            lblXrayDriveHeat.Caption = .stsXrayDriverHeat
                //            lblSeqCpuErr.Caption = .stsSeqCpuErr
                //            lblSeqBatteryErr.Caption = .stsSeqBatteryErr
                //            lblSeqKzCommErr.Caption = .stsSeqKzCommErr
                //            lblSeqKvCommErr.Caption = .stsSeqKvCommErr
                //            lblFilterTimeout.Caption = .stsFilterTimeout
                //            lblTiltTimeout.Caption = .stsTiltTimeout
                //            lblXTimeout.Caption = .stsXTimeout
                //            lblIIDriveHeat.Caption = .stsIIDriverHeat
                //
                //            lblXLLimit.Caption = .stsXLLimit
                //            lblXRLimit.Caption = .stsXRLimit
                //            lblXLeft.Caption = .stsXLeft
                //            lblXRight.Caption = .stsXRight
                //            lblXLTouch.Caption = .stsXLTouch
                //            lblXRTouch.Caption = .stsXRTouch
                //            lblRotXChange.Caption = .stsRotXChange
                //            lblDisXChange.Caption = .stsDisXChange
                //            lblYFLimit.Caption = .stsYFLimit
                //            lblYRLimit.Caption = .stsYBLimit
                //            lblYForward.Caption = .stsYForward
                //            lblYBackward.Caption = .stsYBackward
                //            lblYFTouch.Caption = .stsYFTouch
                //            lblYBTouch.Caption = .stsYBTouch
                //            lblRotYChange.Caption = .stsRotYChange
                //            lblDisYChange.Caption = .stsDisYChange
                //            lblIIFLimit.Caption = .stsIIFLimit
                //            lblIIRLimit.Caption = .stsIIBLimit
                //            lblIIForward.Caption = .stsIIForward
                //            lblIIBackward.Caption = .stsIIBackward
                //            lblVerIIChange.Caption = .stsVerIIChange
                //            lblRotIIChange.Caption = .stsRotIIChange
                //            lblGainIIChange.Caption = .stsGainIIChange
                //            lblDisIIChange.Caption = .stsDisIIChange
                //            lblTiltCwLimit.Caption = .stsTiltCwLimit
                //            lblTiltCCwLimit.Caption = .stsTiltCCwLimit
                //            lblTiltOrigin.Caption = .stsTiltOrigin
                //            lblTiltCw.Caption = .stsTiltCw
                //            lblTiltCCw.Caption = .stsTiltCCw
                //            lblTiltOriginRun.Caption = .stsTiltOriginRun
                //            lblColliLOLimit.Caption = .stsColliLOLimit
                //            lblColliLCLimit.Caption = .stsColliLCLimit
                //            lblColliROLimit.Caption = .stsColliROLimit
                //            lblColliRCLimit.Caption = .stsColliRCLimit
                //            lblColliUOLimit.Caption = .stsColliUOLimit
                //            lblColliUCLimit.Caption = .stsColliUCLimit
                //            lblColliDOLimit.Caption = .stsColliDOLimit
                //            lblColliDCLimit.Caption = .stsColliDCLimit
                //            lblColliLOpen.Caption = .stsColliLOpen
                //            lblColliLClose.Caption = .stsColliLClose
                //            lblColliROpen.Caption = .stsColliROpen
                //            lblColliRClose.Caption = .stsColliRClose
                //            lblColliUOpen.Caption = .stsColliUOpen
                //            lblColliUClose.Caption = .stsColliUClose
                //            lblColliDOpen.Caption = .stsColliDOpen
                //            lblColliDClose.Caption = .stsColliDClose
                //            lblFilter0.Caption = .stsFilter0
                //            lblFilter1.Caption = .stsFilter1
                //            lblFilter2.Caption = .stsFilter2
                //            lblFilter3.Caption = .stsFilter3
                //            lblFilter4.Caption = .stsFilter4
                //            lblFilter5.Caption = .stsFilter5
                //            lblFilter0Run.Caption = .stsFilter0Run
                //            lblFilter1Run.Caption = .stsFilter1Run
                //            lblFilter2Run.Caption = .stsFilter2Run
                //            lblFilter3Run.Caption = .stsFilter3Run
                //            lblFilter4Run.Caption = .stsFilter4Run
                //            lblFilter5Run.Caption = .stsFilter5Run
                //            lblXrayOn.Caption = .XrayOn
                //            lblXrayOff.Caption = .XrayOff
                //            lblII9.Caption = .stsII9
                //            lblII6.Caption = .stsII6
                //            lblII4.Caption = .stsII4
                //            lblIIPower.Caption = .stsIIPower
                //            lblErrReset.Caption = .DeviceErrReset
                //            lblUDOrigin.Caption = .UdOrigin
                //            lblRotOrigin.Caption = .RotOrigin
                //            lblXStgLeft.Caption = .XStgLeft
                //            lblXStgRight.Caption = .XStgRight
                //            lblXStgOrigin.Caption = .XStgOrigin
                //            lblYStgForward.Caption = .YStgForward
                //            lblYStgBackward.Caption = .YStgBackward
                //            lblYStgOrigin.Caption = .YStgOrigin
                //            lblstsSLight.Caption = .stsSLight
                //
                //            lblFID.Caption = Format(CStr(.stsFID / 10), "0.0")
                //            lblFCD.Caption = Format(CStr(.stsFCD / 10), "0.0")
                //            lblXPosition.Caption = Format(CStr(.stsXPosition / 10), "0.0")
                //            lblXMinSpeed.Caption = Format(CStr(.stsXMinSpeed / 10), "0.0")
                //            lblXMaxSpeed.Caption = Format(CStr(.stsXMaxSpeed / 10), "0.0")
                //            lblXSpeed.Caption = Format(CStr(.stsXSpeed / 10), "0.0")
                //            lblYMinSpeed.Caption = Format(CStr(.stsYMinSpeed / 10), "0.0")
                //            lblYMaxSpeed.Caption = Format(CStr(.stsYMaxSpeed / 10), "0.0")
                //            lblYSpeed.Caption = Format(CStr(.stsYSpeed / 10), "0.0")
                //            lblIIMinSpeed.Caption = Format(CStr(.stsIIMinSpeed / 10), "0.0")
                //            lblIIMaxSpeed.Caption = Format(CStr(.stsIIMaxSpeed / 10), "0.0")
                //            lblIISpeed.Caption = Format(CStr(.stsIISpeed / 10), "0.0")
                //
                //            frmMechaContorol.hsbSpeed(0).Max = .stsXMaxSpeed
                //            frmMechaContorol.hsbSpeed(0).Min = .stsXMinSpeed
                //            frmMechaContorol.hsbSpeed(1).Max = .stsYMaxSpeed
                //            frmMechaContorol.hsbSpeed(1).Min = .stsYMinSpeed
                //            frmMechaContorol.hsbSpeed(4).Max = .stsIIMaxSpeed
                //            frmMechaContorol.hsbSpeed(4).Min = .stsIIMinSpeed

            }

        }



		private void frmMechaStatus_Load(System.Object eventSender, System.EventArgs eventArgs)
		{
			int ReturnValue = 0;

#if AllTest
            int Ans = 0;
            float RotAccelTime = 0;
#endif


			//seqcommオブジェクトの新規作成
            Module1.MySeq = new SeqComm.Seq();
            commTest = Module1.MySeq;

            //追加2015/03/13hata
            commTest.OnCommEnd += commTest_OnCommEnd;

			//    #If AllTest Then
			//'        Set mctos = New clsTActiveX
            //    #End If

            cmdSeqRead.Text = "ｼｰｹﾝｻ" + "\n" + "読込開始";
            cmdSeqWrite.Text = "ｼｰｹﾝｻ" + "\n" + "書込開始";

            //ｼｰｹﾝｻからﾒｶｽﾃｰﾀｽの読み出し
            ReturnValue = Module1.MechaStatusReasd();
			
            //    Timer1.Enabled = True
			
            //ﾒｶｺﾝﾄﾛｰﾙﾌｫｰﾑの表示
			fMCtrl.Show();
			//初期設定
            fMCtrl.hsbSpeed[0].Value = commTest.stsXMinSpeed;
            fMCtrl.hsbSpeed[1].Value = commTest.stsXMinSpeed;
            fMCtrl.hsbSpeed[4].Value = commTest.stsIIMinSpeed;
            
            fMCtrl.txtSpeed[0].Text = Convert.ToString(commTest.stsXMinSpeed);
			fMCtrl.txtSpeed[1].Text = Convert.ToString(commTest.stsYMinSpeed);
            fMCtrl.txtSpeed[2].Text = Convert.ToString(Convert.ToDouble(fMCtrl.lblRotMinSpeed.Text) + 0.0001);
            fMCtrl.txtSpeed[3].Text = Convert.ToString(Convert.ToDouble(fMCtrl.lblUDMinSpeed.Text) + 0.0001);
			fMCtrl.txtSpeed[4].Text = Convert.ToString(commTest.stsIIMinSpeed);
            fMCtrl.txtSpeed[5].Text = Convert.ToString(Convert.ToDouble(fMCtrl.lblXStgMinSpeed.Text) + 0.0001);
            fMCtrl.txtSpeed[6].Text = Convert.ToString(Convert.ToDouble(fMCtrl.lblYStgMinSpeed.Text) + 0.0001);

            //回転
            fMCtrl.hsbSpeed[2].Maximum = (Convert.ToInt32(fMCtrl.lblRotMaxSpeed.Text) * 10 + fMCtrl.hsbSpeed[2].LargeChange - 1);
            fMCtrl.hsbSpeed[2].Minimum = Convert.ToInt32(Convert.ToDouble(fMCtrl.lblRotMinSpeed.Text) * 10);
            //昇降
            fMCtrl.hsbSpeed[3].Maximum = (Convert.ToInt32(fMCtrl.lblUDMaxSpeed.Text) * 10 + fMCtrl.hsbSpeed[3].LargeChange - 1);
            fMCtrl.hsbSpeed[3].Minimum = Convert.ToInt32(Convert.ToDouble(fMCtrl.lblUDMinSpeed.Text) * 10);
            //微調X
            fMCtrl.hsbSpeed[5].Maximum = (Convert.ToInt32(fMCtrl.lblXStgMaxSpeed.Text) * 10 + fMCtrl.hsbSpeed[5].LargeChange - 1);
            fMCtrl.hsbSpeed[5].Minimum =Convert.ToInt32(Convert.ToDouble(fMCtrl.lblXStgMinSpeed.Text) * 10);
            //微調Y
            fMCtrl.hsbSpeed[6].Maximum = (Convert.ToInt32(fMCtrl.lblYStgMaxSpeed.Text) * 10 + fMCtrl.hsbSpeed[6].LargeChange - 1);
            fMCtrl.hsbSpeed[6].Minimum = Convert.ToInt32(Convert.ToDouble(fMCtrl.lblYStgMinSpeed.Text) * 10);

			fMCtrl.txtSpeed[7].Text = Convert.ToString(commTest.stsXrayXMinSp);
			fMCtrl.txtSpeed[8].Text = Convert.ToString(commTest.stsXrayYMinSp);
            fMCtrl.txtSpeed[9].Text = Convert.ToString(commTest.stsXrayRotMinSp);


            Module1.Init(this);
#if AllTest
            modMecainf.mecainf = default(modMecainf.mecainfType);

            //ﾒｶﾃﾞﾊﾞｲｽｵｰﾌﾟﾝ
            //Ans = Module1.MechaDevOpen(ref Module1.hDevID);
            //Ans = Module1.UdInit(Module1.hDevID);
            //Ans = Module1.RotateInit(Module1.hDevID);
            ////    Ans = PhmInit(hDevID)
            //Ans = Module1.PioDevOpen(ref Module1.hDevPIO);
            //Ans = Module1.SwOpeStart();
            ////回転手動動作の加速時間設定読込
            //Ans = Module1.GetRotAccel(ref RotAccelTime);
            //fMCtrl.lblRotAccelTm.Text = RotAccelTime.ToString();

            //追加2015/02/09hata
            //共有メモリ作成
            Module1.InitializeSharedCTCommon();

            ////ﾒｶﾃﾞﾊﾞｲｽｵｰﾌﾟﾝ
            Ans = Module1.MechaDevOpen(ref Module1.hDevID);
            //メカエラーリセット
            Ans = Module1.Mechaerror_reset(Module1.hDevID);
            //ＰＩＯボードの初期化
            Ans = Module1.PioDevOpen(ref Module1.hDevPIO);
            //回転軸の初期化  
            Ans = Module1.RotateInit(Module1.hDevID);
            //昇降軸の初期化 
            Ans = Module1.UdInit(Module1.hDevID);

            //追加2015/03/14hata
            try
            {
                //微調X軸の初期化
                Ans = Module1.XStgInit(Module1.hDevID);
                Module1.XstgFlg = true;

                //微調Y軸の初期化
                Ans = Module1.YStgInit(Module1.hDevID);
                Module1.YstgFlg = true;
            }
            catch
            {
            }
            
            //制御スイッチの初期化
            Ans = Module1.SwOpeStart();
            //回転手動動作の加速時間設定読込
            Ans = Module1.GetRotAccel(ref RotAccelTime);
            fMCtrl.lblRotAccelTm.Text = RotAccelTime.ToString();




            ////        'Ｘ線外部制御
            ////        'イベント処理開始メソッド
            ////        Ans = mctos.EventValue_Set(1)
            ////        'ウォームアップ強制完了
            ////        Ans = mctos.WarmUpQuit_Set(1)
            ////        '状態要求メソッド
            ////        Ans = mctos.X_AllEventRaise_Set(1)
			//    #End If
#endif

            //起動しないようにする
            //if (ReturnValue == 0)
            //{
            //    Timer1.Enabled = true;
            //    cmdSeqWrite.BackColor = System.Drawing.Color.Lime;
            //}

		}



        private void frmMechaStatus_FormClosed(System.Object eventSender, System.Windows.Forms.FormClosedEventArgs eventArgs)
        {
            //追加2015/03/13hata
            commTest.OnCommEnd -= commTest_OnCommEnd;
             
#if AllTest
            int Ans = 0;
            
            //Ｘ線外部制御
            //イベント処理終了メソッド
            //        Ans = mctos.EventValue_Set(2)

            //ﾒｶﾃﾞﾊﾞｲｽｸﾛｰｽﾞ
            Ans = Module1.SwOpeEnd();
            Ans = Module1.PioChkEnd();
            Ans = Module1.PioDevClose(Module1.hDevPIO);
            Ans = Module1.MechaDevClose(Module1.hDevID);
            //        Set mctos = Nothing

            //追加2015/02/09hata
            //共有メモリ破棄
            Module1.ExitSharedCTCommon();

#endif

            //ﾒｶｺﾝﾄﾛｰﾙﾌｫｰﾑのｱﾝﾛｰﾄﾞ
            fMCtrl.Close();

            //seqcommオブジェクトの解放"
            commTest.Dispose();
            commTest = null;

        }


		//'20140125_コメント<使用していない>
		//'#If AllTest Then
		// ''    Private Sub mctos_MechDataDisp(Val1 As FeinFocus.MechData)
		// ''
		// ''        Dim idat1   As Integer  'X線ON/OFF(0:OFF,1:ON)
		// ''
		// ''        idat1 = Val1.m_XrayOnSet
		// ''
		// ''        'X線照射状態の書込み
		// ''        Select Case idat1
		// ''        Case 0
		// ''            frmMechaContorol.lblXray.Caption = "OFF"
		// ''            PlcBitWrite "stsXrayOn", False
		// ''        Case 1
		// ''            frmMechaContorol.lblXray.Caption = "ON"
		// ''            PlcBitWrite "stsXrayOn", True
		// ''        Case Else
		// ''        End Select
		// ''
		// ''    End Sub
		//'#End If

        
        bool Tim1Sts = false;
        //int iSelect = -1;
        private void Timer1_Tick(System.Object eventSender, System.EventArgs eventArgs)
        {

            //20140125_コメントhata<使用していない>
            //    #If AllTest Then
            //        Dim Ans As Long
            //        Dim com_name   As String '構造体名
            //        Dim name       As String 'コモン名
            //        Dim float_data As Single '実数型の読み書き用
            //        Dim long_data  As Long   '整数型の読み書き用
            //        Dim error      As Long   '戻り値
            //        Dim Val_rot_pos     As Single  '回転位置ｄｅｇ
            //        Dim Val_udab_pos      As Single '昇降位置
            //        Dim sts As Integer
            //    #End If

            //Timer1.Enabled = false;
            
            string device ="";
            string data = "";
            bool bdata = false;

            if (fMCtrl == null) return;

            if (Tim1Sts) return;

            Tim1Sts = true;

            //
            //    'ｼｰｹﾝｻからﾒｶｽﾃｰﾀｽの読み出し
            //    Ans = MechaStatusReasd
            //
            //
            //    With commTest
            //        'ﾒｶﾃﾞﾊﾞｲｽ ｴﾗｰﾘｾｯﾄ
            //        If .DeviceErrReset Then
            //            Ans = Mechaerror_reset(hDevID)
            //        End If
            //        '昇降原点復帰
            //        If .UdOrigin Then
            //            Ans = UdOrigin(hDevID)
            //        End If
            //        '回転原点復帰
            //        If .RotOrigin Then
            //            Ans = RotateOrigin(hDevID)
            //        End If
            //'        '微調X軸左
            //'        If (.XStgLeft And (Not XStgFlg)) Then
            //'            Ans = XStgManual(hDevID, 1, CSng(frmMechaContorol.txtSpeed(5).Text))
            //'            XStgFlg = True
            //'        End If
            //'        '微調X軸右
            //'        If (.XStgRight And (Not XStgFlg)) Then
            //'            Ans = XStgManual(hDevID, 0, CSng(frmMechaContorol.txtSpeed(5).Text))
            //'            XStgFlg = True
            //'        End If
            //'        If ((Not .XStgLeft) And (Not .XStgRight) And XStgFlg) Then
            //'            Ans = XStgFastStop(hDevID)
            //'            XStgFlg = False
            //'        End If
            //        '微調X軸原点復帰
            //        If .XStgOrigin Then
            //            Ans = XStgOrigin(hDevID)
            //        End If
            //        '微調Y軸前進
            //        If (.YStgForward And (Not YStgFlg)) Then
            //            Ans = YStgManual(hDevID, 1, CSng(frmMechaContorol.txtSpeed(6).Text))
            //            YStgFlg = True
            //        End If
            //        '微調Y軸後退
            //        If (.YStgBackward And (Not YStgFlg)) Then
            //            Ans = YStgManual(hDevID, 0, CSng(frmMechaContorol.txtSpeed(6).Text))
            //            YStgFlg = True
            //        End If
            //        If ((Not .YStgForward) And (Not .YStgBackward) And YStgFlg) Then
            //            Ans = YStgFastStop(hDevID)
            //            YStgFlg = False
            //        End If
            //        '微調Y軸原点復帰
            //        If .YStgOrigin Then
            //            Ans = YStgOrigin(hDevID)
            //        End If
            //        'X線ON
            //        If .XrayOn Then
            //            sts = mctos.Xrayonoff_Set(1)
            //        End If
            //        'X線OFF
            //        If .XrayOff Then
            //            sts = mctos.Xrayonoff_Set(2)
            //        End If
            //    End With


            //        With commTest
            //            '表示
            //            lblPcInhibit.Caption = .PcInhibit
            //            lblRunReadySW.Caption = .stsRunReadySW
            //            lblDoorInterlock.Caption = .stsDoorInterlock
            //            lblEmergency.Caption = .stsEmergency
            //            lblXray225Trip.Caption = .stsXray225Trip
            //            lblXray160Trip.Caption = .stsXray160Trip
            //            lblFilterTouch.Caption = .stsFilterTouch
            //            lblXray225Touch.Caption = .stsXray225Touch
            //            lblXray160Touch.Caption = .stsXray160Touch
            //            lblRotTouch.Caption = .stsRotTouch
            //            lblTiltTouch.Caption = .stsTiltTouch
            //'            lblDetectTouch.Caption = .stsDetectTouch
            //'            lblColliTouch.Caption = .stsColliTouch
            //            lblXDriveHeat.Caption = .stsXDriverHeat
            //            lblYDriveHeat.Caption = .stsYDriverHeat
            //            lblXrayDriveHeat.Caption = .stsXrayDriverHeat
            //            lblSeqCpuErr.Caption = .stsSeqCpuErr
            //            lblSeqBatteryErr.Caption = .stsSeqBatteryErr
            //            lblSeqKzCommErr.Caption = .stsSeqKzCommErr
            //            lblSeqKvCommErr.Caption = .stsSeqKvCommErr
            //            lblFilterTimeout.Caption = .stsFilterTimeout
            //            lblTiltTimeout.Caption = .stsTiltTimeout
            //            lblXTimeout.Caption = .stsXTimeout
            //            lblIIDriveHeat.Caption = .stsIIDriverHeat
            //            lblSeqCounterErr.Caption = .stsSeqCounterErr
            //            lblXDriveErr.Caption = .stsXDriveErr
            //
            //            lblXrayXErr.Caption = .stsXrayXErr
            //            lblXrayYErr.Caption = .stsXrayYErr
            //            lblXrayRotErr.Caption = .stsXrayRotErr
            //
            //            lblXLLimit.Caption = .stsXLLimit
            //            lblXRLimit.Caption = .stsXRLimit
            //            lblXLeft.Caption = .stsXLeft
            //            lblXRight.Caption = .stsXRight
            //            lblXLTouch.Caption = .stsXLTouch
            //            lblXRTouch.Caption = .stsXRTouch
            //            lblRotXChange.Caption = .stsRotXChange
            //            lblDisXChange.Caption = .stsDisXChange
            //            lblYFLimit.Caption = .stsYFLimit
            //            lblYRLimit.Caption = .stsYBLimit
            //            lblYForward.Caption = .stsYForward
            //            lblYBackward.Caption = .stsYBackward
            //            lblYFTouch.Caption = .stsYFTouch
            //            lblYBTouch.Caption = .stsYBTouch
            //            lblRotYChange.Caption = .stsRotYChange
            //            lblDisYChange.Caption = .stsDisYChange
            //            lblIIFLimit.Caption = .stsIIFLimit
            //            lblIIRLimit.Caption = .stsIIBLimit
            //            lblIIForward.Caption = .stsIIForward
            //            lblIIBackward.Caption = .stsIIBackward
            //            lblVerIIChange.Caption = .stsVerIIChange
            //            lblRotIIChange.Caption = .stsRotIIChange
            //            lblGainIIChange.Caption = .stsGainIIChange
            //            lblDisIIChange.Caption = .stsDisIIChange
            //            lblSPIIChange.Caption = .stsSPIIChange
            //            lblTableInterlock.Caption = .stsTableMovePermit
            //            lblYIlArea.Caption = .stsMechaPermit
            //'            lblUpIlArea.Caption = .stsUpIlArea
            //'            lblRotIlArea.Caption = .stsRotIlArea
            //            lblTiltCwLimit.Caption = .stsTiltCwLimit
            //            lblTiltCCwLimit.Caption = .stsTiltCCwLimit
            //            lblTiltOrigin.Caption = .stsTiltOrigin
            //            lblTiltCw.Caption = .stsTiltCw
            //            lblTiltCCw.Caption = .stsTiltCCw
            //            lblTiltOriginRun.Caption = .stsTiltOriginRun
            //            lblColliLOLimit.Caption = .stsColliLOLimit
            //            lblColliLCLimit.Caption = .stsColliLCLimit
            //            lblColliROLimit.Caption = .stsColliROLimit
            //            lblColliRCLimit.Caption = .stsColliRCLimit
            //            lblColliUOLimit.Caption = .stsColliUOLimit
            //            lblColliUCLimit.Caption = .stsColliUCLimit
            //            lblColliDOLimit.Caption = .stsColliDOLimit
            //            lblColliDCLimit.Caption = .stsColliDCLimit
            //            lblColliLOpen.Caption = .stsColliLOpen
            //            lblColliLClose.Caption = .stsColliLClose
            //            lblColliROpen.Caption = .stsColliROpen
            //            lblColliRClose.Caption = .stsColliRClose
            //            lblColliUOpen.Caption = .stsColliUOpen
            //            lblColliUClose.Caption = .stsColliUClose
            //            lblColliDOpen.Caption = .stsColliDOpen
            //            lblColliDClose.Caption = .stsColliDClose
            //            lblFilter0.Caption = .stsFilter0
            //            lblFilter1.Caption = .stsFilter1
            //            lblFilter2.Caption = .stsFilter2
            //            lblFilter3.Caption = .stsFilter3
            //            lblFilter4.Caption = .stsFilter4
            //            lblFilter5.Caption = .stsFilter5
            //            lblFilter0Run.Caption = .stsFilter0Run
            //            lblFilter1Run.Caption = .stsFilter1Run
            //            lblFilter2Run.Caption = .stsFilter2Run
            //            lblFilter3Run.Caption = .stsFilter3Run
            //            lblFilter4Run.Caption = .stsFilter4Run
            //            lblFilter5Run.Caption = .stsFilter5Run
            //            lblXrayOn.Caption = .XrayOn
            //            lblXrayOff.Caption = .XrayOff
            //            lblII9.Caption = .stsII9
            //            lblII6.Caption = .stsII6
            //            lblII4.Caption = .stsII4
            //            lblIIPower.Caption = .stsIIPower
            //            lblErrReset.Caption = .DeviceErrReset
            //            lblUDOrigin.Caption = .UdOrigin
            //            lblRotOrigin.Caption = .RotOrigin
            //            lblXStgLeft.Caption = .XStgLeft
            //            lblXStgRight.Caption = .XStgRight
            //            lblXStgOrigin.Caption = .XStgOrigin
            //            lblYStgForward.Caption = .YStgForward
            //            lblYStgBackward.Caption = .YStgBackward
            //            lblYStgOrigin.Caption = .YStgOrigin
            //            lblstsSLight.Caption = .stsSLight
            //
            //            lblXrayXLLimit.Caption = .stsXrayXLLimit
            //            lblXrayXRLimit.Caption = .stsXrayXRLimit
            //            lblXrayXL.Caption = .stsXrayXL
            //            lblXrayXR.Caption = .stsXrayXR
            //            lblRotXrayXCh.Caption = .stsRotXrayXCh
            //            lblDisXrayXCh.Caption = .stsDisXrayXCh
            //            lblXrayYFLimit.Caption = .stsXrayYFLimit
            //            lblXrayYBLimit.Caption = .stsXrayYBLimit
            //            lblXrayYF.Caption = .stsXrayYF
            //            lblXrayYB.Caption = .stsXrayYB
            //            lblRotXrayYCh.Caption = .stsRotXrayYCh
            //            lblDisXrayYCh.Caption = .stsDisXrayYCh
            //            lblXrayCWLimit.Caption = .stsXrayCWLimit
            //            lblXrayCCWLimit.Caption = .stsXrayCCWLimit
            //            lblXrayCW.Caption = .stsXrayCW
            //            lblXrayCCW.Caption = .stsXrayCCW
            //            lblXrayRotLock.Caption = .stsXrayRotLock
            //
            //            lblEXMOn.Caption = .stsEXMOn
            //            lblEXMReady.Caption = .stsEXMReady
            //            lblEXMNormal1.Caption = .stsEXMNormal1
            //            lblEXMNormal2.Caption = .stsEXMNormal2
            //            lblEXMWU.Caption = .stsEXMWU
            //            lblEXMRemote.Caption = .stsEXMRemote
            //
            //            lblDoorKey.Caption = .stsDoorKey
            //            lblDoorLock.Caption = .stsDoorLock
            //
            //            lblCTIIPos.Caption = .stsCTIIPos
            //            lblTVIIPos.Caption = .stsTVIIPos
            //            lblCTIIDrive.Caption = .stsCTIIDrive
            //            lblTVIIDrive.Caption = .stsTVIIDrive
            //            lblTVII9.Caption = .stsTVII9
            //            lblTVII6.Caption = .stsTVII6
            //            lblTVII4.Caption = .stsTVII4
            //            lblTVIIPower.Caption = .stsTVIIPower
            //            lblCameraPower.Caption = .stsCameraPower
            //            lblIrisLOpen.Caption = .stsIrisLOpen
            //            lblIrisLClose.Caption = .stsIrisLClose
            //            lblIrisROpen.Caption = .stsIrisROpen
            //            lblIrisRClose.Caption = .stsIrisRClose
            //            lblIrisUOpen.Caption = .stsIrisUOpen
            //            lblIrisUClose.Caption = .stsIrisUClose
            //            lblIrisDOpen.Caption = .stsIrisDOpen
            //            lblIrisDClose.Caption = .stsIrisDClose
            //
            //            lblFID.Caption = Format(CStr(.stsFID / 10), "0.0")
            //            lblFCD.Caption = Format(CStr(.stsFCD / 10), "0.0")
            //            lblXPosition.Caption = Format(CStr(.stsXPosition / 100), "0.00")
            //            lblYPosition.Caption = Format(CStr(.stsYPosition / 100), "0.00")
            //            lblXMinSpeed.Caption = Format(CStr(.stsXMinSpeed / 10), "0.0")
            //            lblXMaxSpeed.Caption = Format(CStr(.stsXMaxSpeed / 10), "0.0")
            //            lblXSpeed.Caption = Format(CStr(.stsXSpeed / 10), "0.0")
            //            lblYMinSpeed.Caption = Format(CStr(.stsYMinSpeed / 10), "0.0")
            //            lblYMaxSpeed.Caption = Format(CStr(.stsYMaxSpeed / 10), "0.0")
            //            lblYSpeed.Caption = Format(CStr(.stsYSpeed / 10), "0.0")
            //            lblIIMinSpeed.Caption = Format(CStr(.stsIIMinSpeed / 10), "0.0")
            //            lblIIMaxSpeed.Caption = Format(CStr(.stsIIMaxSpeed / 10), "0.0")
            //            lblIISpeed.Caption = Format(CStr(.stsIISpeed / 10), "0.0")
            //
            //            lblXrayXMinSp.Caption = Format(CStr(.stsXrayXMinSp / 100), "0.00")
            //            lblXrayXMaxSp.Caption = Format(CStr(.stsXrayXMaxSp / 100), "0.00")
            //            lblXrayXSpeed.Caption = Format(CStr(.stsXrayXSpeed / 100), "0.00")
            //            lblXrayXPos.Caption = Format(CStr(.stsXrayXPos / 100), "0.00")
            //            lblXrayYMinSp.Caption = Format(CStr(.stsXrayYMinSp / 100), "0.00")
            //            lblXrayYMaxSp.Caption = Format(CStr(.stsXrayYMaxSp / 100), "0.00")
            //            lblXrayYSpeed.Caption = Format(CStr(.stsXrayYSpeed / 100), "0.00")
            //            lblXrayFCD.Caption = Format(CStr(.stsXrayFCD / 100), "0.00")
            //            lblXrayYPos.Caption = Format(CStr(.stsXrayYPos / 100), "0.00")
            //            lblXrayRotMinSp.Caption = Format(CStr(.stsXrayRotMinSp / 10000), "0.0000")
            //            lblXrayRotMaxSp.Caption = Format(CStr(.stsXrayRotMaxSp / 10000), "0.0000")
            //            lblXrayRotSpeed.Caption = Format(CStr(.stsXrayRotSpeed / 10000), "0.0000")
            //            lblXrayRotAccel.Caption = Format(CStr(.stsXrayRotAccel), "0")
            //            lblXrayRotPos.Caption = Format(CStr(.stsXrayRotPos / 10000), "0.0000")
            //
            //            lblEXMMaxW.Caption = Format(CStr(.stsEXMMaxW), "0")
            //            lblEXMMaxTV.Caption = Format(CStr(.stsEXMMaxTV), "0")
            //            lblEXMMinTV.Caption = Format(CStr(.stsEXMMinTV), "0")
            //            lblEXMMaxTC.Caption = Format(CStr(.stsEXMMaxTC / 100), "0.00")
            //            lblEXMMinTC.Caption = Format(CStr(.stsEXMMinTC / 100), "0.00")
            //            lblEXMLimitTV.Caption = Format(CStr(.stsEXMLimitTV), "0")
            //            lblEXMLimitTC.Caption = Format(CStr(.stsEXMLimitTC / 100), "0.00")
            //            lblEXMTVSet.Caption = Format(CStr(.stsEXMTVSet), "0")
            //            lblEXMTCSet.Caption = Format(CStr(.stsEXMTCSet / 100), "0.00")
            //            lblEXMTV.Caption = Format(CStr(.stsEXMTV), "0")
            //            lblEXMTC.Caption = Format(CStr(.stsEXMTC / 100), "0.00")
            //            lblEXMErrCode.Caption = Format(CStr(.stsEXMErrCode), "00")
            //
            //            frmMechaContorol.hsbSpeed(0).Max = .stsXMaxSpeed
            //            frmMechaContorol.hsbSpeed(0).Min = .stsXMinSpeed
            //            frmMechaContorol.hsbSpeed(1).Max = .stsYMaxSpeed
            //            frmMechaContorol.hsbSpeed(1).Min = .stsYMinSpeed
            //            frmMechaContorol.hsbSpeed(4).Max = .stsIIMaxSpeed
            //            frmMechaContorol.hsbSpeed(4).Min = .stsIIMinSpeed
            //
            //            frmMechaContorol.hsbSpeed(7).Max = .stsXrayXMaxSp
            //            frmMechaContorol.hsbSpeed(7).Min = .stsXrayXMinSp
            //            frmMechaContorol.hsbSpeed(8).Max = .stsXrayYMaxSp
            //            frmMechaContorol.hsbSpeed(8).Min = .stsXrayYMinSp
            //            frmMechaContorol.hsbSpeed(9).Max = .stsXrayRotMaxSp
            //            frmMechaContorol.hsbSpeed(9).Min = .stsXrayRotMinSp
            //        End With


#if AllTest
            //mecainf取得
            modMecainf.GetMecainf(ref modMecainf.mecainf);

            //ｽﾃｰﾀｽﾁｪｯｸ
            //    Ans = PioChkStart(10)
            //        com_name = "mecainf"
            //昇降状態書込み
            ////    name = "ud_error"
            //    error = getcommon_long(com_name, name, long_data)
            //    Ans = PlcBitWrite("stsUDErr", CBool(long_data))
                 //if (Module1.PlcBitWrite(ref "stsUDErr", ref Convert.ToBoolean(modMecainf.mecainf.ud_error)))
            device = "stsUDErr";
            bdata = Convert.ToBoolean(modMecainf.mecainf.ud_error);
            if (Module1.PlcBitWrite(ref device, ref bdata) != 0)
                goto ReturnHandler;
 
            ////    name = "ud_limit"
            ////    error = getcommon_long(com_name, name, long_data)
            ////    Ans = PlcBitWrite("stsUDLimit", CBool(long_data))
            //if (Module1.PlcBitWrite(ref "stsUDLimit", ref Convert.ToBoolean(modMecainf.mecainf.ud_limit)))
            device = "stsUDLimit";
            bdata = Convert.ToBoolean(modMecainf.mecainf.ud_limit);
            if (Module1.PlcBitWrite(ref device, ref bdata) != 0)
                goto ReturnHandler;
            
            ////    name = "ud_busy"
            ////    error = getcommon_long(com_name, name, long_data)
            ////    Ans = PlcBitWrite("stsUDBusy", CBool(long_data))
            //if (Module1.PlcBitWrite(ref "stsUDBusy", ref Convert.ToBoolean(modMecainf.mecainf.ud_busy)))
            device = "stsUDBusy";
            bdata = Convert.ToBoolean(modMecainf.mecainf.ud_busy);
            if (Module1.PlcBitWrite(ref device, ref bdata) != 0)
                goto ReturnHandler;
            
            //昇降位置書込み
            ////    name = "udab_pos"
            ////    error = getcommon_float(com_name, name, float_data)
            ////    frmMechaContorol.lblUDPosition.Caption = Str(float_data)
            ////    Ans = PlcWordWrite("stsUDPosition", Str(CLng(float_data * 100)))
            //My.MyProject.Forms.frmMechaContorol.lblUDPosition.Text = Conversion.Str(modMecainf.mecainf.udab_pos);
            //if (Module1.PlcWordWrite(ref "stsUDPosition", ref Conversion.Str(Convert.ToInt32(modMecainf.mecainf.udab_pos * 100))))
            fMCtrl.lblUDPosition.Text = modMecainf.mecainf.udab_pos.ToString();
            device = "stsUDPosition";
            data = (Convert.ToInt32(modMecainf.mecainf.udab_pos * 100)).ToString();
            if (Module1.PlcWordWrite(ref device, ref data) != 0)
                goto ReturnHandler;

            //回転状態書込み
            ////    name = "rot_error"
            ////    error = getcommon_long(com_name, name, long_data)
            ////    Ans = PlcBitWrite("stsRotErr", CBool(long_data))
            //if (Module1.PlcBitWrite(ref "stsRotErr", ref Convert.ToBoolean(modMecainf.mecainf.rot_error)))
            device = "stsRotErr";
            bdata = Convert.ToBoolean(modMecainf.mecainf.rot_error);
            if (Module1.PlcBitWrite(ref device, ref bdata) != 0)
                goto ReturnHandler;
            
            ////    name = "rot_busy"
            ////    error = getcommon_long(com_name, name, long_data)
            ////    Ans = PlcBitWrite("stsRotBusy", CBool(long_data))
            //if (Module1.PlcBitWrite(ref "stsRotBusy", ref Convert.ToBoolean(modMecainf.mecainf.rot_busy)))
            device = "stsRotBusy";
            bdata = Convert.ToBoolean(modMecainf.mecainf.rot_busy);
            if (Module1.PlcBitWrite(ref device, ref bdata) != 0)
                goto ReturnHandler;
            
            //回転位置書込み
            ////    name = "rot_pos"
            ////    error = getcommon_long(com_name, name, long_data)
            ////    frmMechaContorol.lblRotPosition.Caption = Str(long_data / 100)
            ////    Ans = PlcWordWrite("stsRotPosition", Str(long_data))
            //My.MyProject.Forms.frmMechaContorol.lblRotPosition.Text = Conversion.Str(modMecainf.mecainf.rot_pos / 100);
            //if (Module1.PlcWordWrite(ref "stsRotPosition", ref Conversion.Str(modMecainf.mecainf.rot_pos)))
            fMCtrl.lblRotPosition.Text = (modMecainf.mecainf.rot_pos / 100).ToString();
            device = "stsRotPosition";
            data = modMecainf.mecainf.rot_pos.ToString();
            if (Module1.PlcWordWrite(ref device, ref data) != 0)
                goto ReturnHandler;
            
            //微調X軸状態書込み
            //    name = "xstg_error"
            //    error = getcommon_long(com_name, name, long_data)
            //    Ans = PlcBitWrite("stsXStgErr", CBool(long_data))
            //if (Module1.PlcBitWrite(ref "stsXStgErr", ref Convert.ToBoolean(modMecainf.mecainf.xstg_error)))
            device = "stsXStgErr";
            bdata = Convert.ToBoolean(modMecainf.mecainf.xstg_error);
            if (Module1.PlcBitWrite(ref device, ref bdata) != 0)
                goto ReturnHandler;
            
            //    name = "xstg_limit"
            //    error = getcommon_long(com_name, name, long_data)
            //    Ans = PlcBitWrite("stsXStgLimit", CBool(long_data))
            //    name = "xstg_busy"
            //    error = getcommon_long(com_name, name, long_data)
            //    Ans = PlcBitWrite("stsXStgBusy", CBool(long_data))

            //微調X軸位置書込み
            ////    name = "xstg_pos"
            ////    error = getcommon_float(com_name, name, float_data)
            ////    frmMechaContorol.lblXStgPosition.Caption = Str(float_data)
            ////    Ans = PlcWordWrite("stsXStgPosition", Str(CLng(float_data * 100)))
            //My.MyProject.Forms.frmMechaContorol.lblXStgPosition.Text = Conversion.Str(modMecainf.mecainf.xstg_pos);
            //if (Module1.PlcWordWrite(ref "stsXStgPosition", ref Conversion.Str(Convert.ToInt32(modMecainf.mecainf.xstg_pos * 100))))
            fMCtrl.lblXStgPosition.Text = (modMecainf.mecainf.xstg_pos).ToString();
            device = "stsXStgPosition";
            data = (Convert.ToInt32(modMecainf.mecainf.xstg_pos * 100)).ToString();
            if (Module1.PlcWordWrite(ref device, ref data) != 0)
                goto ReturnHandler;
            
            //微調Y軸状態書込み
            ////    name = "ystg_error"
            ////    error = getcommon_long(com_name, name, long_data)
            ////    Ans = PlcBitWrite("stsYStgErr", CBool(long_data))
            ////if (Module1.PlcBitWrite(ref "stsYStgErr", ref Convert.ToBoolean(modMecainf.mecainf.ystg_error)))
            device = "stsYStgErr";
            bdata = Convert.ToBoolean(modMecainf.mecainf.ystg_error);
            if (Module1.PlcBitWrite(ref device, ref bdata) != 0)
                goto ReturnHandler;
            
            //    name = "ystg_limit"
            //    error = getcommon_long(com_name, name, long_data)
            //    Ans = PlcBitWrite("stsYStgLimit", CBool(long_data))
            //    name = "ystg_busy"
            //    error = getcommon_long(com_name, name, long_data)
            //    Ans = PlcBitWrite("stsYStgBusy", CBool(long_data))

            //微調Y軸位置書込み
            ////    name = "ystg_pos"
            ////    error = getcommon_float(com_name, name, float_data)
            ////    frmMechaContorol.lblYStgPosition.Caption = Str(float_data)
            ////    Ans = PlcWordWrite("stsYStgPosition", Str(CLng(float_data * 100)))
            //My.MyProject.Forms.frmMechaContorol.lblYStgPosition.Text = Conversion.Str(modMecainf.mecainf.ystg_pos);
            //if (Module1.PlcWordWrite(ref "stsYStgPosition", ref Conversion.Str(Convert.ToInt32(modMecainf.mecainf.ystg_pos * 100))))
            fMCtrl.lblYStgPosition.Text = modMecainf.mecainf.ystg_pos.ToString();
            device = "stsYStgPosition";
            data = (Convert.ToInt32(modMecainf.mecainf.ystg_pos * 100)).ToString();
            if (Module1.PlcWordWrite(ref device, ref data) != 0)
                goto ReturnHandler;
            
            //ファントム状態書込み
            ////    name = "phm_error"
            ////    error = getcommon_long(com_name, name, long_data)
            ////    Ans = PlcBitWrite("stsPhmErr", CBool(long_data))
            //if (Module1.PlcBitWrite(ref "stsPhmErr", ref Convert.ToBoolean(modMecainf.mecainf.phm_error)))
            device = "stsPhmErr";
            bdata = Convert.ToBoolean(modMecainf.mecainf.phm_error);
            if (Module1.PlcBitWrite(ref device, ref bdata) != 0)
                goto ReturnHandler;
            
            ////    name = "phm_limit"
            ////    error = getcommon_long(com_name, name, long_data)
            ////    Ans = PlcBitWrite("stsPhmLimit", CBool(long_data))
            //if (Module1.PlcBitWrite(ref "stsPhmLimit", ref Convert.ToBoolean(modMecainf.mecainf.phm_limit)))
            device = "stsPhmLimit";
            bdata = Convert.ToBoolean(modMecainf.mecainf.phm_limit);
            if (Module1.PlcBitWrite(ref device, ref bdata) != 0)
                goto ReturnHandler;
            
            ////    name = "phm_busy"
            ////    error = getcommon_long(com_name, name, long_data)
            ////    Ans = PlcBitWrite("stsPhmBusy", CBool(long_data))
            //if (Module1.PlcBitWrite(ref "stsPhmBusy", ref Convert.ToBoolean(modMecainf.mecainf.phm_busy)))
            device = "stsPhmBusy";
            bdata = Convert.ToBoolean(modMecainf.mecainf.phm_busy);
            if (Module1.PlcBitWrite(ref device, ref bdata) != 0)
                goto ReturnHandler;
            
            ////    name = "phm_onoff"
            ////    error = getcommon_long(com_name, name, long_data)
            ////    Ans = PlcBitWrite("stsPhmOnOff", CBool(long_data))
            //if (Module1.PlcBitWrite(ref "stsPhmOnOff", ref Convert.ToBoolean(modMecainf.mecainf.phm_onoff)))
            device = "stsPhmOnOff";
            bdata = Convert.ToBoolean(modMecainf.mecainf.phm_onoff);
            if (Module1.PlcBitWrite(ref device, ref bdata) != 0)
                goto ReturnHandler;

#endif


            ////ﾃｰﾌﾞﾙX運転速度設定
            if (Module1.SpeedData[0] != 0)
            {
                device = "XSpeed";
                data = Convert.ToString(Module1.SpeedData[0]);
                if (Module1.PlcWordWrite(ref device, ref data) != 0)
                    goto ReturnHandler;
            }

            //ﾃｰﾌﾞﾙY運転速度設定

            if (Module1.SpeedData[1] != 0)
            {
                device = "YSpeed";
                data = Convert.ToString(Module1.SpeedData[1]);
                if (Module1.PlcWordWrite(ref device, ref data) != 0)
                    goto ReturnHandler;
            }

            //I.I.運転速度設定
            if (Module1.SpeedData[4] != 0)
            {
                device = "IISpeed";
                data = Convert.ToString(Module1.SpeedData[4]);
                if (Module1.PlcWordWrite(ref device, ref data) != 0)
                    goto ReturnHandler;
            }

            //光学系X運転速度設定
            if (Module1.SpeedData[7] != 0)
            {
                device = "XrayXSpeed";
                data = Convert.ToString(Module1.SpeedData[7]);
                if (Module1.PlcWordWrite(ref device, ref data) != 0)
                    goto ReturnHandler;
                }

            //光学系Y運転速度設定
            if (Module1.SpeedData[8] != 0)
            {
                device = "XrayYSpeed";
                data = Convert.ToString(Module1.SpeedData[8]);
                if (Module1.PlcWordWrite(ref device, ref data) != 0)
                    goto ReturnHandler;
            }
            //光学系回転運転速度設定
            if (Module1.SpeedData[9] != 0)
            {
                device = "XrayRotSpeed";
                data = Convert.ToString(Module1.SpeedData[9]);
                if (Module1.PlcWordWrite(ref device, ref data) != 0)
                    goto ReturnHandler;
            }

            //X線EXM管電圧設定
            device = "EXMTVSet";
            data = fMCtrl.txtEXMTVSet.Text;
            if (Module1.PlcWordWrite(ref device, ref data) != 0)
                goto ReturnHandler;

            //X線EXM管電流設定
            device = "EXMTCSet";
            data = Convert.ToString(Convert.ToDouble(fMCtrl.txtEXMTCSet.Text) * 100);
            if (Module1.PlcWordWrite(ref device, ref data) != 0)
                goto ReturnHandler;
            
            ////通信ﾁｪｯｸの書込み
            device = "CommCheck";
            bdata = true;
            if (Module1.PlcBitWrite(ref device, ref bdata) != 0)
                goto ReturnHandler;
            
            
            //    If Ans Then
            //        Timer1.Enabled = False
            //    Else
            //Timer1.Enabled = true;
            //    End If

            Tim1Sts = false; 

            return;

            ReturnHandler:
                Tim1Sts = false; 



        }

        private void Timer2_Tick(System.Object eventSender, System.EventArgs eventArgs)
        {

#if AllTest
            int Ans = 0;
#endif


            //    Dim com_name   As String '構造体名
            //    Dim name       As String 'コモン名
            //    Dim float_data As Single '実数型の読み書き用
            //    Dim long_data  As Long   '整数型の読み書き用
            //    Dim error      As Long   '戻り値
            //    Dim Val_rot_pos     As Single  '回転位置ｄｅｇ
            //    Dim Val_udab_pos      As Single '昇降位置
            //    Dim sts As Integer

            Timer2.Enabled = false;
            lblCommBusy.Text = Convert.ToString(commTest.stsCommBusy);

            //ｼｰｹﾝｻからﾒｶｽﾃｰﾀｽの読み出し
            if (Module1.MechaStatusReasd() != 0)
                return;
            
#if AllTest
            Ans = Module1.PioChkStart(10);
#endif

            //追加2014/10/06hata_v1951反映
            if (commTest.UpDownIndex)
            {
                //Ans = Module1.UdIndex(Module1.hDevID, 0, (float)commTest.stsUDIndexPos, 0, 0);
            }

            //    With commTest
            //        'ﾒｶﾃﾞﾊﾞｲｽ ｴﾗｰﾘｾｯﾄ
            //        If .DeviceErrReset Then
            //            Ans = Mechaerror_reset(hDevID)
            //        End If
            //        '昇降原点復帰
            //        If .UdOrigin Then
            //            Ans = UdOrigin(hDevID)
            //        End If
            //        '回転原点復帰
            //        If .RotOrigin Then
            //            Ans = RotateOrigin(hDevID)
            //        End If
            //        '微調X軸左
            //        If (.XStgLeft And (Not XStgFlg)) Then
            //            Ans = XStgManual(hDevID, 1, CSng(frmMechaContorol.txtSpeed(5).Text))
            //            XStgFlg = True
            //        End If
            //        '微調X軸右
            //        If (.XStgRight And (Not XStgFlg)) Then
            //            Ans = XStgManual(hDevID, 0, CSng(frmMechaContorol.txtSpeed(5).Text))
            //            XStgFlg = True
            //        End If
            //        If ((Not .XStgLeft) And (Not .XStgRight) And XStgFlg) Then
            //            Ans = XStgFastStop(hDevID)
            //            XStgFlg = False
            //        End If
            //        '微調X軸原点復帰
            //        If .XStgOrigin Then
            //            Ans = XStgOrigin(hDevID)
            //        End If
            //        '微調Y軸前進
            //        If (.YStgForward And (Not YStgFlg)) Then
            //            Ans = YStgManual(hDevID, 1, CSng(frmMechaContorol.txtSpeed(6).Text))
            //            YStgFlg = True
            //        End If
            //        '微調Y軸後退
            //        If (.YStgBackward And (Not YStgFlg)) Then
            //            Ans = YStgManual(hDevID, 0, CSng(frmMechaContorol.txtSpeed(6).Text))
            //            YStgFlg = True
            //        End If
            //        If ((Not .YStgForward) And (Not .YStgBackward) And YStgFlg) Then
            //            Ans = YStgFastStop(hDevID)
            //            YStgFlg = False
            //        End If
            //        '微調Y軸原点復帰
            //        If .YStgOrigin Then
            //            Ans = YStgOrigin(hDevID)
            //        End If
            //        'X線ON
            //        If .XrayOn Then
            //            sts = mctos.Xrayonoff_Set(1)
            //        End If
            //        'X線OFF
            //        If .XrayOff Then
            //            sts = mctos.Xrayonoff_Set(2)
            //        End If
            //    End With

            Timer2.Enabled = true;

        }
        private void Timer3_Tick(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (fMCtrl == null) return;

            var _with3 = commTest;
            //表示
            lblPcInhibit.Text = Convert.ToString(_with3.PcInhibit);
            lblRunReadySW.Text = Convert.ToString(_with3.stsRunReadySW);
            lblDoorInterlock.Text = Convert.ToString(_with3.stsDoorInterlock);
            lblEmergency.Text = Convert.ToString(_with3.stsEmergency);
            lblXray225Trip.Text = Convert.ToString(_with3.stsXray225Trip);
            lblXray160Trip.Text = Convert.ToString(_with3.stsXray160Trip);
            lblFilterTouch.Text = Convert.ToString(_with3.stsFilterTouch);
            lblXray225Touch.Text = Convert.ToString(_with3.stsXray225Touch);
            lblXray160Touch.Text = Convert.ToString(_with3.stsXray160Touch);
            lblRotTouch.Text = Convert.ToString(_with3.stsRotTouch);
            lblTiltTouch.Text = Convert.ToString(_with3.stsTiltTouch);
            //            lblDetectTouch.Caption = .stsDetectTouch
            //            lblColliTouch.Caption = .stsColliTouch
            lblXDriveHeat.Text = Convert.ToString(_with3.stsXDriverHeat);
            lblYDriveHeat.Text = Convert.ToString(_with3.stsYDriverHeat);
            lblXrayDriveHeat.Text = Convert.ToString(_with3.stsXrayDriverHeat);
            lblSeqCpuErr.Text = Convert.ToString(_with3.stsSeqCpuErr);
            lblSeqBatteryErr.Text = Convert.ToString(_with3.stsSeqBatteryErr);
            lblSeqKzCommErr.Text = Convert.ToString(_with3.stsSeqKzCommErr);
            lblSeqKvCommErr.Text = Convert.ToString(_with3.stsSeqKvCommErr);
            lblFilterTimeout.Text = Convert.ToString(_with3.stsFilterTimeout);
            lblTiltTimeout.Text = Convert.ToString(_with3.stsTiltTimeout);
            lblXTimeout.Text = Convert.ToString(_with3.stsXTimeout);
            lblIIDriveHeat.Text = Convert.ToString(_with3.stsIIDriverHeat);
            lblSeqCounterErr.Text = Convert.ToString(_with3.stsSeqCounterErr);
            lblXDriveErr.Text = Convert.ToString(_with3.stsXDriveErr);

            lblXrayXErr.Text = Convert.ToString(_with3.stsXrayXErr);
            lblXrayYErr.Text = Convert.ToString(_with3.stsXrayYErr);
            lblXrayRotErr.Text = Convert.ToString(_with3.stsXrayRotErr);

            lblXLLimit.Text = Convert.ToString(_with3.stsXLLimit);
            lblXRLimit.Text = Convert.ToString(_with3.stsXRLimit);
            lblXLeft.Text = Convert.ToString(_with3.stsXLeft);
            lblXRight.Text = Convert.ToString(_with3.stsXRight);
            lblXLTouch.Text = Convert.ToString(_with3.stsXLTouch);
            lblXRTouch.Text = Convert.ToString(_with3.stsXRTouch);
            lblRotXChange.Text = Convert.ToString(_with3.stsRotXChange);
            lblDisXChange.Text = Convert.ToString(_with3.stsDisXChange);
            lblYFLimit.Text = Convert.ToString(_with3.stsYFLimit);
            lblYRLimit.Text = Convert.ToString(_with3.stsYBLimit);
            lblYForward.Text = Convert.ToString(_with3.stsYForward);
            lblYBackward.Text = Convert.ToString(_with3.stsYBackward);
            lblYFTouch.Text = Convert.ToString(_with3.stsYFTouch);
            lblYBTouch.Text = Convert.ToString(_with3.stsYBTouch);
            lblRotYChange.Text = Convert.ToString(_with3.stsRotYChange);
            lblDisYChange.Text = Convert.ToString(_with3.stsDisYChange);
            lblIIFLimit.Text = Convert.ToString(_with3.stsIIFLimit);
            lblIIRLimit.Text = Convert.ToString(_with3.stsIIBLimit);
            lblIIForward.Text = Convert.ToString(_with3.stsIIForward);
            lblIIBackward.Text = Convert.ToString(_with3.stsIIBackward);
            lblVerIIChange.Text = Convert.ToString(_with3.stsVerIIChange);
            lblRotIIChange.Text = Convert.ToString(_with3.stsRotIIChange);
            lblGainIIChange.Text = Convert.ToString(_with3.stsGainIIChange);
            lblDisIIChange.Text = Convert.ToString(_with3.stsDisIIChange);
            lblSPIIChange.Text = Convert.ToString(_with3.stsSPIIChange);
            lblTableInterlock.Text = Convert.ToString(_with3.stsTableMovePermit);
            lblYIlArea.Text = Convert.ToString(_with3.stsMechaPermit);
            //            lblUpIlArea.Caption = .stsUpIlArea
            //            lblRotIlArea.Caption = .stsRotIlArea
            lblTiltCwLimit.Text = Convert.ToString(_with3.stsTiltCwLimit);
            lblTiltCCwLimit.Text = Convert.ToString(_with3.stsTiltCCwLimit);
            lblTiltOrigin.Text = Convert.ToString(_with3.stsTiltOrigin);
            lblTiltCw.Text = Convert.ToString(_with3.stsTiltCw);
            lblTiltCCw.Text = Convert.ToString(_with3.stsTiltCCw);
            lblTiltOriginRun.Text = Convert.ToString(_with3.stsTiltOriginRun);
            lblColliLOLimit.Text = Convert.ToString(_with3.stsColliLOLimit);
            lblColliLCLimit.Text = Convert.ToString(_with3.stsColliLCLimit);
            lblColliROLimit.Text = Convert.ToString(_with3.stsColliROLimit);
            lblColliRCLimit.Text = Convert.ToString(_with3.stsColliRCLimit);
            lblColliUOLimit.Text = Convert.ToString(_with3.stsColliUOLimit);
            lblColliUCLimit.Text = Convert.ToString(_with3.stsColliUCLimit);
            lblColliDOLimit.Text = Convert.ToString(_with3.stsColliDOLimit);
            lblColliDCLimit.Text = Convert.ToString(_with3.stsColliDCLimit);
            lblColliLOpen.Text = Convert.ToString(_with3.stsColliLOpen);
            lblColliLClose.Text = Convert.ToString(_with3.stsColliLClose);
            lblColliROpen.Text = Convert.ToString(_with3.stsColliROpen);
            lblColliRClose.Text = Convert.ToString(_with3.stsColliRClose);
            lblColliUOpen.Text = Convert.ToString(_with3.stsColliUOpen);
            lblColliUClose.Text = Convert.ToString(_with3.stsColliUClose);
            lblColliDOpen.Text = Convert.ToString(_with3.stsColliDOpen);
            lblColliDClose.Text = Convert.ToString(_with3.stsColliDClose);

            lblShutter.Text = Convert.ToString(_with3.stsShutter);  //追加2014/10/07hata_v1951反映

            lblFilter0.Text = Convert.ToString(_with3.stsFilter0);
            lblFilter1.Text = Convert.ToString(_with3.stsFilter1);
            lblFilter2.Text = Convert.ToString(_with3.stsFilter2);
            lblFilter3.Text = Convert.ToString(_with3.stsFilter3);
            lblFilter4.Text = Convert.ToString(_with3.stsFilter4);
            lblFilter5.Text = Convert.ToString(_with3.stsFilter5);

            lblShutterRun.Text = Convert.ToString(_with3.stsShutterBusy);   //追加2014/10/07hata_v1951反映
            
            lblFilter0Run.Text = Convert.ToString(_with3.stsFilter0Run);
            lblFilter1Run.Text = Convert.ToString(_with3.stsFilter1Run);
            lblFilter2Run.Text = Convert.ToString(_with3.stsFilter2Run);
            lblFilter3Run.Text = Convert.ToString(_with3.stsFilter3Run);
            lblFilter4Run.Text = Convert.ToString(_with3.stsFilter4Run);
            lblFilter5Run.Text = Convert.ToString(_with3.stsFilter5Run);
            lblXrayOn.Text = Convert.ToString(_with3.XrayOn);
            lblXrayOff.Text = Convert.ToString(_with3.XrayOff);
            lblII9.Text = Convert.ToString(_with3.stsII9);
            lblII6.Text = Convert.ToString(_with3.stsII6);
            lblII4.Text = Convert.ToString(_with3.stsII4);
            lblIIPower.Text = Convert.ToString(_with3.stsIIPower);
            lblErrReset.Text = Convert.ToString(_with3.DeviceErrReset);
            lblUDOrigin.Text = Convert.ToString(_with3.UdOrigin);

            lblUdIndex.Text = Convert.ToString(_with3.UpDownIndex);         //追加2014/10/07hata_v1951反映
            lblUdIndexPos.Text = Convert.ToString(_with3.stsUDIndexPos);    //追加2014/10/07hata_v1951反映
            
            lblRotOrigin.Text = Convert.ToString(_with3.RotOrigin);
            lblXStgLeft.Text = Convert.ToString(_with3.XStgLeft);
            lblXStgRight.Text = Convert.ToString(_with3.XStgRight);
            lblXStgOrigin.Text = Convert.ToString(_with3.XStgOrigin);
            lblYStgForward.Text = Convert.ToString(_with3.YStgForward);
            lblYStgBackward.Text = Convert.ToString(_with3.YStgBackward);
            lblYStgOrigin.Text = Convert.ToString(_with3.YStgOrigin);
            lblstsSLight.Text = Convert.ToString(_with3.stsSLight);

            lblXrayXLLimit.Text = Convert.ToString(_with3.stsXrayXLLimit);
            lblXrayXRLimit.Text = Convert.ToString(_with3.stsXrayXRLimit);
            lblXrayXL.Text = Convert.ToString(_with3.stsXrayXL);
            lblXrayXR.Text = Convert.ToString(_with3.stsXrayXR);
            lblRotXrayXCh.Text = Convert.ToString(_with3.stsRotXrayXCh);
            lblDisXrayXCh.Text = Convert.ToString(_with3.stsDisXrayXCh);
            lblXrayYFLimit.Text = Convert.ToString(_with3.stsXrayYFLimit);
            lblXrayYBLimit.Text = Convert.ToString(_with3.stsXrayYBLimit);
            lblXrayYF.Text = Convert.ToString(_with3.stsXrayYF);
            lblXrayYB.Text = Convert.ToString(_with3.stsXrayYB);
            lblRotXrayYCh.Text = Convert.ToString(_with3.stsRotXrayYCh);
            lblDisXrayYCh.Text = Convert.ToString(_with3.stsDisXrayYCh);
            lblXrayCWLimit.Text = Convert.ToString(_with3.stsXrayCWLimit);
            lblXrayCCWLimit.Text = Convert.ToString(_with3.stsXrayCCWLimit);
            lblXrayCW.Text = Convert.ToString(_with3.stsXrayCW);
            lblXrayCCW.Text = Convert.ToString(_with3.stsXrayCCW);
            lblXrayRotLock.Text = Convert.ToString(_with3.stsXrayRotLock);

            lblEXMOn.Text = Convert.ToString(_with3.stsEXMOn);
            lblEXMReady.Text = Convert.ToString(_with3.stsEXMReady);
            lblEXMNormal1.Text = Convert.ToString(_with3.stsEXMNormal1);
            lblEXMNormal2.Text = Convert.ToString(_with3.stsEXMNormal2);
            lblEXMWU.Text = Convert.ToString(_with3.stsEXMWU);
            lblEXMRemote.Text = Convert.ToString(_with3.stsEXMRemote);

            lblDoorKey.Text = Convert.ToString(_with3.stsDoorKey);
            lblDoorLock.Text = Convert.ToString(_with3.stsDoorLock);

            lblCTIIPos.Text = Convert.ToString(_with3.stsCTIIPos);
            lblTVIIPos.Text = Convert.ToString(_with3.stsTVIIPos);
            lblCTIIDrive.Text = Convert.ToString(_with3.stsCTIIDrive);
            lblTVIIDrive.Text = Convert.ToString(_with3.stsTVIIDrive);
            lblTVII9.Text = Convert.ToString(_with3.stsTVII9);
            lblTVII6.Text = Convert.ToString(_with3.stsTVII6);
            lblTVII4.Text = Convert.ToString(_with3.stsTVII4);
            lblTVIIPower.Text = Convert.ToString(_with3.stsTVIIPower);
            lblCameraPower.Text = Convert.ToString(_with3.stsCameraPower);
            lblIrisLOpen.Text = Convert.ToString(_with3.stsIrisLOpen);
            lblIrisLClose.Text = Convert.ToString(_with3.stsIrisLClose);
            lblIrisROpen.Text = Convert.ToString(_with3.stsIrisROpen);
            lblIrisRClose.Text = Convert.ToString(_with3.stsIrisRClose);
            lblIrisUOpen.Text = Convert.ToString(_with3.stsIrisUOpen);
            lblIrisUClose.Text = Convert.ToString(_with3.stsIrisUClose);
            lblIrisDOpen.Text = Convert.ToString(_with3.stsIrisDOpen);
            lblIrisDClose.Text = Convert.ToString(_with3.stsIrisDClose);

            lblXOrgReq.Text = Convert.ToString(_with3.stsXOrgReq);
            lblYOrgReq.Text = Convert.ToString(_with3.stsYOrgReq);
            lblIIOrgReq.Text = Convert.ToString(_with3.stsIIOrgReq);
            lblIIChgOrgReq.Text = Convert.ToString(_with3.stsIIChgOrgReq);
            lblMechaRstBusy.Text = Convert.ToString(_with3.stsMechaRstBusy);
            lblMechaRstOK.Text = Convert.ToString(_with3.stsMechaRstOK);

            lblAutoRestrict.Text = Convert.ToString(_with3.stsAutoRestrict);
            lblYIndexSlow.Text = Convert.ToString(_with3.stsYIndexSlow);
            lblDoorPermit.Text = Convert.ToString(_with3.stsDoorPermit);

            lblFID.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsFID) / 10f), "0.0");
            lblFCD.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsFCD) / 10f), "0.0");
            lblXPosition.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsXPosition) / 100f), "0.00");
            lblYPosition.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsYPosition) / 100f), "0.00");
            lblXMinSpeed.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsXMinSpeed) / 10f), "0.0");
            lblXMaxSpeed.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsXMaxSpeed) / 10f), "0.0");
            lblXSpeed.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsXSpeed) / 10f), "0.0");
            lblYMinSpeed.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsYMinSpeed) / 10f), "0.0");
            lblYMaxSpeed.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsYMaxSpeed) / 10f), "0.0");
            lblYSpeed.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsYSpeed) / 10f), "0.0");
            lblIIMinSpeed.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsIIMinSpeed) / 10f), "0.0");
            lblIIMaxSpeed.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsIIMaxSpeed) / 10f), "0.0");
            lblIISpeed.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsIISpeed) / 10f), "0.0");

            lblXrayXMinSp.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsXrayXMinSp) / 100f), "0.00");
            lblXrayXMaxSp.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsXrayXMaxSp) / 100f), "0.00");
            lblXrayXSpeed.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsXrayXSpeed) / 100f), "0.00");
            lblXrayXPos.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsXrayXPos) / 100f), "0.00");
            lblXrayYMinSp.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsXrayYMinSp) / 100f), "0.00");
            lblXrayYMaxSp.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsXrayYMaxSp) / 100f), "0.00");
            lblXrayYSpeed.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsXrayYSpeed) / 100f), "0.00");
            lblXrayFCD.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsXrayFCD) / 100f), "0.00");
            lblXrayYPos.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsXrayYPos) / 100f), "0.00");
            lblXrayRotMinSp.Text = string.Format(Convert.ToString(Convert.ToDouble(_with3.stsXrayRotMinSp) / 10000f), "0.0000");
            lblXrayRotMaxSp.Text = string.Format(Convert.ToString(Convert.ToDouble(_with3.stsXrayRotMaxSp) / 10000f), "0.0000");
            lblXrayRotSpeed.Text = string.Format(Convert.ToString(Convert.ToDouble(_with3.stsXrayRotSpeed) / 10000f), "0.0000");
            lblXrayRotAccel.Text = string.Format(Convert.ToString(_with3.stsXrayRotAccel), "0");
            lblXrayRotPos.Text = string.Format(Convert.ToString(Convert.ToDouble(_with3.stsXrayRotPos) / 10000f), "0.0000");

            lblEXMMaxW.Text = string.Format(Convert.ToString(_with3.stsEXMMaxW), "0");
            lblEXMMaxTV.Text = string.Format(Convert.ToString(_with3.stsEXMMaxTV), "0");
            lblEXMMinTV.Text = string.Format(Convert.ToString(_with3.stsEXMMinTV), "0");
            lblEXMMaxTC.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsEXMMaxTC) / 100f), "0.00");
            lblEXMMinTC.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsEXMMinTC) / 100f), "0.00");
            lblEXMLimitTV.Text = string.Format(Convert.ToString(_with3.stsEXMLimitTV), "0");
            lblEXMLimitTC.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsEXMLimitTC) / 100f), "0.00");
            lblEXMTVSet.Text = string.Format(Convert.ToString(_with3.stsEXMTVSet), "0");
            lblEXMTCSet.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsEXMTCSet) / 100f), "0.00");
            lblEXMTV.Text = string.Format(Convert.ToString(_with3.stsEXMTV), "0");
            lblEXMTC.Text = string.Format(Convert.ToString(Convert.ToSingle(_with3.stsEXMTC) / 100f), "0.00");
            lblEXMErrCode.Text = string.Format(Convert.ToString(_with3.stsEXMErrCode), "00");

            fMCtrl.hsbSpeed[0].Maximum = (_with3.stsXMaxSpeed + fMCtrl.hsbSpeed[0].LargeChange - 1);
            fMCtrl.hsbSpeed[0].Minimum = _with3.stsXMinSpeed;
            fMCtrl.txtSpeed[0].Text = Convert.ToString(Convert.ToDouble(fMCtrl.hsbSpeed[0].Value) / 10f);

            fMCtrl.hsbSpeed[1].Maximum = (_with3.stsYMaxSpeed + fMCtrl.hsbSpeed[1].LargeChange - 1);
            fMCtrl.hsbSpeed[1].Minimum = _with3.stsYMinSpeed;
            fMCtrl.txtSpeed[1].Text = Convert.ToString(Convert.ToDouble(fMCtrl.hsbSpeed[1].Value) / 10f);

            fMCtrl.hsbSpeed[4].Maximum = (_with3.stsIIMaxSpeed + fMCtrl.hsbSpeed[4].LargeChange - 1);
            fMCtrl.hsbSpeed[4].Minimum = _with3.stsIIMinSpeed;
            fMCtrl.txtSpeed[4].Text = Convert.ToString(Convert.ToDouble(fMCtrl.hsbSpeed[4].Value) / 10f);

            fMCtrl.hsbSpeed[7].Maximum = (_with3.stsXrayXMaxSp + fMCtrl.hsbSpeed[7].LargeChange - 1);
            fMCtrl.hsbSpeed[7].Minimum = _with3.stsXrayXMinSp;
            fMCtrl.txtSpeed[7].Text = Convert.ToString(Convert.ToDouble(fMCtrl.hsbSpeed[7].Value) / 100f);

            fMCtrl.hsbSpeed[8].Maximum = (_with3.stsXrayYMaxSp + fMCtrl.hsbSpeed[8].LargeChange - 1);
            fMCtrl.hsbSpeed[8].Minimum = _with3.stsXrayYMinSp;
            fMCtrl.txtSpeed[8].Text = Convert.ToString(Convert.ToDouble(fMCtrl.hsbSpeed[8].Value) / 100f);

            fMCtrl.hsbSpeed[9].Maximum = (_with3.stsXrayRotMaxSp + fMCtrl.hsbSpeed[9].LargeChange - 1);
            fMCtrl.hsbSpeed[9].Minimum = _with3.stsXrayRotMinSp;
            fMCtrl.txtSpeed[9].Text = Convert.ToString(Convert.ToDouble(fMCtrl.hsbSpeed[9].Value) / 10000f);


 
        }

        private void frmMechaStatus_FormClosing(object sender, FormClosingEventArgs e)
        {
            Timer1.Enabled = false;
            Timer2.Enabled = false;
            Timer3.Enabled = false;



        }

        private void button1_Click(object sender, EventArgs e)
        {
            commTest.RestProcess();
        }
	}
}
