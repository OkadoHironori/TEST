using System;
using System.Text;
using System.IO.Ports;
using System.Globalization;
using System.Threading;
using System.Diagnostics;

namespace XrayCtrl
{
	public class cCtrlm : IDisposable
	{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
		Private frmCtrl As FormCtrl
		Private WithEvents frmCtrlTimer As Timer
		'1999-11-26 T.Shibui 6160FP X線制御用
		Private WithEvents frmCtrlMSComm1 As MSComm
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private ThreadEx frmCtrlTimer;
		private SerialPort frmCtrlMSComm1;
       
		/// <summary>
		/// スレッドの周期
		/// </summary>
		private const int Interval = 1000;

        //追加2014/06/09_hata
        /// <summary>
        // スレッド起動変数
        /// <summary>
        private bool CtrlMflg = false;			        // スレッドONフラグ
        
		private const double Pai = 3.14159265358979;			//円周率

//1999-12-25 T.Shibui
		private string RecBuffer;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		Dim Ret  As Integer
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		//送信データ比較用
		public int TmpOutXOn;
		public string TmpOutkV;
		public string TmpOutmA;
		private int ComK;
		private int ComA;
		private int ComI;
		private int ComN;
		private int ComF;
		private int ComE;
		private int ComS;

		private string ComKBuf;
		private string ComABuf;
		private string ComIBuf;
		private string ComNBuf;
		private string ComFBuf;
		private string ComEBuf;
		private string ComSBuf;

//2002-09-05 Shibui
		private int mStatusGet;
		private mLogic.DelayType mTStatusGet;
		private int mStart;
		private mLogic.DelayType mTStart;

		/// <summary>
		/// 排他用オブジェクト
		/// </summary>
		internal static object gLock = new object();

		/// <summary>
		/// 
		/// </summary>
		public cCtrlm()
		{
			lock (gLock)
			{				
				Class_Initialize();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
            //削除2014/06/09_hata
            //排他処理しない
            //lock (gLock)
            //{
            //    Class_Terminate();
            //}
            Class_Terminate();
		}

		/// <summary>
		/// cValueサブクラスの参照
		/// </summary>
		public cValue cValue
		{
			get
			{
				lock (gLock)
				{
					return ModuleCTRLM.gValue;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void Class_Initialize()
		{
			if (ModuleCTRLM.gValue == null)
			{
				ModuleCTRLM.gValue = new cValue();
			}

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			If frmCtrl Is Nothing Then
				Set frmCtrl = New FormCtrl
				Load frmCtrl
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			if (frmCtrlMSComm1 == null)
			{
//				'通信設定
//				Set frmCtrlMSComm1 = frmCtrl.MSComm1
//				frmCtrlMSComm1.CommPort = 1
//				frmCtrlMSComm1.Settings = "9600,n,8,1"
//				frmCtrlMSComm1.PortOpen = True
//

                //frmCtrlMSComm1 = new SerialPort();
                //frmCtrlMSComm1.PortName = "COM1";
                //frmCtrlMSComm1.BaudRate = 9600;
                //frmCtrlMSComm1.Parity = Parity.None;
                //frmCtrlMSComm1.DataBits = 8;
                //frmCtrlMSComm1.StopBits = StopBits.One;
                //frmCtrlMSComm1.WriteBufferSize = 512;
                //frmCtrlMSComm1.ReadBufferSize = 1024;
                //frmCtrlMSComm1.DtrEnable = true;
                //frmCtrlMSComm1.RtsEnable = true;
                //frmCtrlMSComm1.DataReceived += new SerialDataReceivedEventHandler(frmctrlMSComm1_OnComm);
                //frmCtrlMSComm1.Open();

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				Set frmCtrlTimer = frmCtrl.Timer
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				frmCtrlTimer = new ThreadEx(new ThreadStart(frmCtrlTimer_Timer));
                frmCtrlTimer.Name = this.ToString();

                //追加2014/06/09_hata
                CtrlMflg = true;

                frmCtrlTimer.Start();
                //bCtrlLive = true;
	
				mLogic.Init(Interval);
			}
			if (ModuleCTRLM.gTosCount == 0)
			{
				//初期値設定
//2004-09-14 Shibui
//				lusInit

				ModuleCTRLM.CtrlInitialRead();

				ModuleCTRLM.ipWarmup = 0;	//起動時は必ずｳｫｰﾑｱｯﾌﾟ未完

//				CommOutput "S?" 'X線装置の状態取得

			}
			ModuleCTRLM.gTosCount = ModuleCTRLM.gTosCount + 1;
		}

		/// <summary>
		/// 
		/// </summary>
		private void Class_Terminate()
		{
			ModuleCTRLM.gTosCount = ModuleCTRLM.gTosCount - 1;
			if (ModuleCTRLM.gTosCount == 0)
			{

                //追加2014/06/09_hata
                CtrlMflg = false;

                ModuleCTRLM.gValue = null;
                IDisposable disposeObj = null;  

//2004-09-27 Shibui
//				Select Case ifEventValue
				switch (ModuleCTRLM.ifOldEventValue)
				{
					case 1:
                        if (frmCtrlMSComm1 != null)
                        {
                            if (frmCtrlMSComm1.IsOpen) { 
                                frmCtrlMSComm1.Close();
                            }
                        }
						frmCtrlMSComm1 = null;
						break;
					case 3:
						ModuleIOM.IOM_Terminate();
						break;
//2004-09-07 Shibui
//'2004-02-27 Shibui
//					Case 4
//						'浜ﾎﾄ
//						Set gclsCtrlXrayL9181S = Nothing
					case 4:
						//浜ﾎﾄ90kV
						disposeObj = modData.gclsCtrlXrayL9421S as IDisposable;
						modData.gclsCtrlXrayL9421S = null;
						break;
					case 5:
						//浜ﾎﾄ130kV
						disposeObj = modData.gclsCtrlXrayL9181S as IDisposable;
						modData.gclsCtrlXrayL9181S = null;
						break;
					case 6:
						//浜ﾎﾄ160kV
						disposeObj = modData.gclsCtrlXrayL9191 as IDisposable;
						modData.gclsCtrlXrayL9191 = null;
						break;

					//追加2009/08/19(KSS)hata_L10801対応  ---------->
					case 7:
						//浜ﾎﾄ230kV
						disposeObj = modData.gclsCtrlXrayL10801 as IDisposable;
						modData.gclsCtrlXrayL10801 = null;
						break;
					//追加2009/08/19(KSS)hata_L10801対応  ----------<

					//追加2010/02/19(KSS)hata_L8601対応  ---------->
					case 8:
						//浜ﾎﾄ90kV 分離型
						disposeObj = modData.gclsCtrlXrayL8601 as IDisposable;
						modData.gclsCtrlXrayL8601 = null;
						break;
					//追加2010/02/19(KSS)hata_L8601対応  ----------<

					//追加2010/05/11(KS1)hata_L9421-02T対応 ---------->
					case 9:
						//浜ﾎﾄ90kV(L9421-02T)
						disposeObj = modData.gclsCtrlXrayL9421_02 as IDisposable;
                        modData.gclsCtrlXrayL9421_02 = null;
						break;
					//追加2010/05/11(KS1)hata_L9421-02T対応 ----------<

					//追加2012/03/20(KS1)hata_L8121-02対応 ---------->
					case 10:
						//450kV用に使用予定
						break;

					case 11:
						//浜ﾎﾄ90kV(L8121-02)
						disposeObj = modData.gclsCtrlXrayL8121_02 as IDisposable;
						modData.gclsCtrlXrayL8121_02 = null;
						break;
					//追加2012/03/20(KS1)hata_L8121-02対応 ----------<

                    //追加2014/10/07(検S1)hata_64bit対応  ---------->
                    case modData.EVENT_L9181_02:
                        //浜ﾎﾄ(130kV L9181-02)
                        disposeObj = modData.gclsCtrlXrayL9181_02 as IDisposable;
                        modData.gclsCtrlXrayL9181_02 = null;
                        break;
                    //追加2014/10/07(検S1)hata_64bit対応  ----------<

                    //追加2015/10/20(検S1)長野 64bit対応  ---------->
                    case modData.EVENT_L12721:
                        //浜ﾎﾄ(130kV L12721)
                        disposeObj = modData.gclsCtrlXrayL12721 as IDisposable;
                        modData.gclsCtrlXrayL12721 = null;
                        break;
                    //追加2015/10/20(検S1)長野 64bit対応  ----------<

                    //追加2015/10/20(検S1)長野 64bit対応  ---------->
                    case modData.EVENT_L10711:
                        //浜ﾎﾄ(130kV L10711)
                        disposeObj = modData.gclsCtrlXrayL10711 as IDisposable;
                        modData.gclsCtrlXrayL10711 = null;
                        break;
                    //追加2015/10/20(検S1)長野 64bit対応  ----------<

				}

				if (disposeObj != null)
				{
					disposeObj.Dispose();
				}

                if (frmCtrlTimer != null)
                {
                    //追加2014/06/09_hata
                    frmCtrlTimer.Stop();
                    frmCtrlTimer = null;
                }
                


#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				frmCtrl.Close();
				frmCtrl = null;
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				ModuleCTRLM.CtrlInitialWrite();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmCtrlTimer_Timer()
		{
			do
			{
//2002-09-05 Shibui
				int StatusGet = 0;
				int Start = 0;
				int intRet = 0;
//Debug.Print "frmCtrlTimer"

                //Debug.WriteLine("Do frmCtrlTimer");

                if (!CtrlMflg) return; //追加2014/06/09_hata
                if (frmCtrlTimer == null) return;
                if (!frmCtrlTimer.IsAlive) return;
                if (frmCtrlTimer.Stoped) return;
 
                lock(gLock)
                {
                    try
                    {

                        switch (ModuleCTRLM.ifEventValue)
                        {
                            case 1:
                                //通信設定

                                //2002-09-05 Shibui
                                mLogic.iOnDelay(mStatusGet, ref mTStatusGet, 10, ref StatusGet);
                                if (mStatusGet == 1)
                                {
                                    CommOutput("S?");
                                    //X線装置の状態取得
                                    mStatusGet = 0;
                                }

                                if (ModuleCTRLM.ifEventValue != ModuleCTRLM.ifOldEventValue)
                                {
                                    //2004-02-14 Shibui
                                    XrayCtrlEnd(ModuleCTRLM.ifOldEventValue);	//X線制御終了処理
                                    //								If ifOldEventValue = 3 Then
                                    //								IOM_Terminate
                                    //								End If
                                    ModuleCTRLM.ifOldEventValue = ModuleCTRLM.ifEventValue;

                                    #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
                                    /*
								Set frmCtrlMSComm1 = frmCtrl.MSComm1
								frmCtrlMSComm1.CommPort = 1
'								frmCtrlMSComm1.CommPort = 2     'changed by 山本　2005-3-17　通信テスト

								frmCtrlMSComm1.Settings = "9600,n,8,1"
								frmCtrlMSComm1.PortOpen = True
*/
                                    #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                                    try
                                    {
                                        frmCtrlMSComm1.PortName = "COM1";
                                        frmCtrlMSComm1.BaudRate = 9600;
                                        frmCtrlMSComm1.Parity = Parity.None;
                                        frmCtrlMSComm1.DataBits = 8;
                                        frmCtrlMSComm1.StopBits = StopBits.One;
                                        frmCtrlMSComm1.WriteBufferSize = 512;
                                        frmCtrlMSComm1.ReadBufferSize = 1024;
                                        frmCtrlMSComm1.DtrEnable = true;
                                        frmCtrlMSComm1.RtsEnable = true;
                                        frmCtrlMSComm1.DataReceived += new SerialDataReceivedEventHandler(frmctrlMSComm1_OnComm);
                                        frmCtrlMSComm1.Open();
                                    }
                                    catch
                                    {
                                        return;
                                    }

                                    //2002-09-05 Shibui
                                    //								CommOutput "S?" 'X線装置の状態取得
                                    mStatusGet = 1;
                                    mStart = 1;
                                }

                                //2002-09-05 Shibui
                                mLogic.iOnDelay(mStart, ref mTStart, 10, ref Start);
                                if (Start == 1)
                                {
                                    ModuleCTRLM.CtrlmLogic();
                                    ModuleXRAYM.XraymLogic();

                                    //コマンド送信
                                    //X線ON/OFF
                                    if (ModuleXRAYM.ComOutXOn != TmpOutXOn)
                                    {
                                        if (ModuleXRAYM.ComOutXOn != 0)
                                        {
                                            CommOutput("N1");
                                        }
                                        else
                                        {
                                            CommOutput("F1");
                                        }
                                        TmpOutXOn = ModuleXRAYM.ComOutXOn;
                                    }
                                    //管電圧
                                    if (ModuleXRAYM.ComOutkV != TmpOutkV)
                                    {
                                        CommOutput(ModuleXRAYM.ComOutkV);
                                        TmpOutkV = ModuleXRAYM.ComOutkV;
                                    }
                                    //管電流
                                    if (ModuleXRAYM.ComOutmA != TmpOutmA)
                                    {
                                        CommOutput(ModuleXRAYM.ComOutmA);
                                        TmpOutmA = ModuleXRAYM.ComOutmA;
                                    }
                                }
                                break;

                            case 2:
                                if (ModuleCTRLM.ifEventValue != ModuleCTRLM.ifOldEventValue)
                                {
                                    //2004-02-14 Shibui
                                    XrayCtrlEnd(ModuleCTRLM.ifOldEventValue);	//X線制御終了処理
                                    //'2002-09-05 Shibui
                                    //'								Select Case ifEventValue
                                    //								Select Case ifOldEventValue
                                    //								Case 1
                                    //'2002-09-05 Shibui
                                    //									mStart = 0
                                    //									mStatusGet = 0
                                    //
                                    //									frmCtrlMSComm1.PortOpen = False
                                    //									Set frmCtrlMSComm1 = Nothing
                                    //								Case 3
                                    //									IOM_Terminate
                                    //								End Select
                                    ModuleCTRLM.ifOldEventValue = ModuleCTRLM.ifEventValue;
                                }
                                break;

                            case 3:
                                if (ModuleCTRLM.ifEventValue != ModuleCTRLM.ifOldEventValue)
                                {
                                    //2004-02-14 Shibui
                                    XrayCtrlEnd(ModuleCTRLM.ifOldEventValue);	//X線制御終了処理
                                    //								If ifOldEventValue = 1 Then
                                    //									frmCtrlMSComm1.PortOpen = False
                                    //									Set frmCtrlMSComm1 = Nothing
                                    //								End If

                                    ModuleCTRLM.ifOldEventValue = ModuleCTRLM.ifEventValue;
                                    ModuleIOM.IOM_Initialize();
                                    ModuleIOM.DoRefresh();
                                }
                                ModuleIOM.DiRefresh();

                                ModuleKevexCTRLM.KevexCtrlmLogic();
                                ModuleKevexXRAYM.KevexXraymLogic();

                                ModuleIOM.DoRefresh();
                                break;

                            //2004-02-14 Shibui
                            case modData.EVENT_L9181:
                                //浜ﾎﾄ（130kV L9181）

                                if (ModuleCTRLM.ifEventValue != ModuleCTRLM.ifOldEventValue)
                                {
                                    XrayCtrlEnd(ModuleCTRLM.ifOldEventValue);	//X線制御終了処理
                                    ModuleCTRLM.ifOldEventValue = ModuleCTRLM.ifEventValue;

                                    //設定ﾌｧｲﾙ読み込み
                                    intRet = modIniFiles.ReadTSB_001_L9181();
                                    if (intRet != modIniFiles.FILE_OK)
                                    {
                                        //ﾌｧｲﾙ読み込みｴﾗｰ
                                    }

                                    //管電圧･管電流 最大最小値設定
                                    ModuleCTRLM.ipXRMaxkV = modData.gudtXraySystemValue.E_XraySetMaxkV;
                                    ModuleCTRLM.ipXRMinkV = modData.gudtXraySystemValue.E_XraySetMinkV;
                                    ModuleCTRLM.ipXRMaxmA = modData.gudtXraySystemValue.E_XraySetMaxuA;
                                    ModuleCTRLM.ipXRMinmA = modData.gudtXraySystemValue.E_XraySetMinuA;

                                    if (modData.gclsCtrlXrayL9181S == null)
                                    {
                                        modData.gclsCtrlXrayL9181S = new clsCtrlXrayL9181S();
                                    }
                                }
                                break;

                            //2004-09-07 Shibui
                            case modData.EVENT_L9421:
                                //浜ﾎﾄ（90kV L9421）

                                if (ModuleCTRLM.ifEventValue != ModuleCTRLM.ifOldEventValue)
                                {
                                    XrayCtrlEnd(ModuleCTRLM.ifOldEventValue);	//X線制御終了処理
                                    ModuleCTRLM.ifOldEventValue = ModuleCTRLM.ifEventValue;

                                    //設定ﾌｧｲﾙ読み込み
                                    intRet = modIniFiles.ReadTSB_001_L9421();
                                    if (intRet != modIniFiles.FILE_OK)
                                    {
                                        //ﾌｧｲﾙ読み込みｴﾗｰ
                                    }

                                    //管電圧･管電流 最大最小値設定
                                    ModuleCTRLM.ipXRMaxkV = modData.gudtXraySystemValue.E_XraySetMaxkV;
                                    ModuleCTRLM.ipXRMinkV = modData.gudtXraySystemValue.E_XraySetMinkV;
                                    ModuleCTRLM.ipXRMaxmA = modData.gudtXraySystemValue.E_XraySetMaxuA;
                                    ModuleCTRLM.ipXRMinmA = modData.gudtXraySystemValue.E_XraySetMinuA;

                                    if (modData.gclsCtrlXrayL9421S == null)
                                    {
                                        modData.gclsCtrlXrayL9421S = new clsCtrlXrayL9421S();
                                    }
                                }
                                break;

                            case modData.EVENT_L9191:
                                //浜ﾎﾄ（160kV L9191）

                                if (ModuleCTRLM.ifEventValue != ModuleCTRLM.ifOldEventValue)
                                {
                                    XrayCtrlEnd(ModuleCTRLM.ifOldEventValue);	//X線制御終了処理
                                    ModuleCTRLM.ifOldEventValue = ModuleCTRLM.ifEventValue;

                                    //設定ﾌｧｲﾙ読み込み
                                    intRet = modIniFiles.ReadTSB_001_L9191();
                                    if (intRet != modIniFiles.FILE_OK)
                                    {
                                        //ﾌｧｲﾙ読み込みｴﾗｰ
                                    }

                                    //管電圧･管電流 最大最小値設定
                                    ModuleCTRLM.ipXRMaxkV = modData.gudtXraySystemValue.E_XraySetMaxkV;
                                    ModuleCTRLM.ipXRMinkV = modData.gudtXraySystemValue.E_XraySetMinkV;
                                    ModuleCTRLM.ipXRMaxmA = modData.gudtXraySystemValue.E_XraySetMaxuA;
                                    ModuleCTRLM.ipXRMinmA = modData.gudtXraySystemValue.E_XraySetMinuA;

                                    if (modData.gclsCtrlXrayL9191 == null)
                                    {
                                        modData.gclsCtrlXrayL9191 = new clsCtrlXrayL9191();
                                    }
                                }
                                break;

                            //追加2009/08/19（KSS)hata_L10801対応  ---------->
                            case modData.EVENT_L10801:
                                //浜ﾎﾄ（230kV L10801）

                                if (ModuleCTRLM.ifEventValue != ModuleCTRLM.ifOldEventValue)
                                {
                                    XrayCtrlEnd(ModuleCTRLM.ifOldEventValue);	//X線制御終了処理
                                    ModuleCTRLM.ifOldEventValue = ModuleCTRLM.ifEventValue;
                                    
                                    //設定ﾌｧｲﾙ読み込み
                                    intRet = modIniFiles.ReadTSB_001_L10801();
                                    if (intRet != modIniFiles.FILE_OK)
                                    {
                                        //ﾌｧｲﾙ読み込みｴﾗｰ
                                    }

                                    //管電圧･管電流 最大最小値設定
                                    ModuleCTRLM.ipXRMaxkV = modData.gudtXraySystemValue.E_XraySetMaxkV;
                                    ModuleCTRLM.ipXRMinkV = modData.gudtXraySystemValue.E_XraySetMinkV;
                                    ModuleCTRLM.ipXRMaxmA = modData.gudtXraySystemValue.E_XraySetMaxuA;
                                    ModuleCTRLM.ipXRMinmA = modData.gudtXraySystemValue.E_XraySetMinuA;

                                    if (modData.gclsCtrlXrayL10801 == null)
                                    {
                                        modData.gclsCtrlXrayL10801 = new clsCtrlXrayL10801();
                                    }
                                }
                                break;
                            //追加2009/08/19（KSS)hata_L10801対応  ----------<

                            //追加2010/02/19(KSS)hata_L8601対応  ---------->
                            case modData.EVENT_L8601:
                                //浜ﾎﾄ（90kV 分離型 L8601）

                                if (ModuleCTRLM.ifEventValue != ModuleCTRLM.ifOldEventValue)
                                {
                                    XrayCtrlEnd(ModuleCTRLM.ifOldEventValue);	//X線制御終了処理
                                    ModuleCTRLM.ifOldEventValue = ModuleCTRLM.ifEventValue;

                                    //設定ﾌｧｲﾙ読み込み
                                    intRet = modIniFiles.ReadTSB_001_L8601();
                                    if (intRet != modIniFiles.FILE_OK)
                                    {
                                        //ﾌｧｲﾙ読み込みｴﾗｰ
                                    }

                                    //管電圧･管電流 最大最小値設定
                                    ModuleCTRLM.ipXRMaxkV = modData.gudtXraySystemValue.E_XraySetMaxkV;
                                    ModuleCTRLM.ipXRMinkV = modData.gudtXraySystemValue.E_XraySetMinkV;
                                    ModuleCTRLM.ipXRMaxmA = modData.gudtXraySystemValue.E_XraySetMaxuA;
                                    ModuleCTRLM.ipXRMinmA = modData.gudtXraySystemValue.E_XraySetMinuA;

                                    if (modData.gclsCtrlXrayL8601 == null)
                                    {
                                        modData.gclsCtrlXrayL8601 = new clsCtrlXrayL8601();
                                    }
                                }
                                break;
                            //追加2010/02/19(KSS)hata_L8601対応  ----------<

                            //追加2010/05/11(KS1)hata_L9421-02T対応 ---------->
                            case modData.EVENT_L9421_02:
                                //浜ﾎﾄ（90kV L9421-02T）

                                if (ModuleCTRLM.ifEventValue != ModuleCTRLM.ifOldEventValue)
                                {
                                    XrayCtrlEnd(ModuleCTRLM.ifOldEventValue);	//X線制御終了処理
                                    ModuleCTRLM.ifOldEventValue = ModuleCTRLM.ifEventValue;

                                    //設定ﾌｧｲﾙ読み込み
                                    intRet = modIniFiles.ReadTSB_001_L9421();
                                    if (intRet != modIniFiles.FILE_OK)
                                    {
                                        //ﾌｧｲﾙ読み込みｴﾗｰ
                                    }

                                    //管電圧･管電流 最大最小値設定
                                    ModuleCTRLM.ipXRMaxkV = modData.gudtXraySystemValue.E_XraySetMaxkV;
                                    ModuleCTRLM.ipXRMinkV = modData.gudtXraySystemValue.E_XraySetMinkV;
                                    ModuleCTRLM.ipXRMaxmA = modData.gudtXraySystemValue.E_XraySetMaxuA;
                                    ModuleCTRLM.ipXRMinmA = modData.gudtXraySystemValue.E_XraySetMinuA;

                                    if (modData.gclsCtrlXrayL9421_02 == null)
                                    {
                                        modData.gclsCtrlXrayL9421_02 = new clsCtrlXrayL9421_02();
                                    }
                                }
                                break;
                            //追加2010/05/11(KS1)hata_L9421-02T対応 ----------<

                            //追加2012/03/20(KS1)hata_L8121-02対応  ---------->
                            case modData.EVENT_L8121_02:
                                //浜ﾎﾄ（150kV 分離型 L8121-02）

                                if (ModuleCTRLM.ifEventValue != ModuleCTRLM.ifOldEventValue)
                                {
                                    XrayCtrlEnd(ModuleCTRLM.ifOldEventValue);	//X線制御終了処理
                                    ModuleCTRLM.ifOldEventValue = ModuleCTRLM.ifEventValue;

                                    //設定ﾌｧｲﾙ読み込み
                                    intRet = modIniFiles.ReadTSB_001_L8121_02();
                                    if (intRet != modIniFiles.FILE_OK)
                                    {
                                        //ﾌｧｲﾙ読み込みｴﾗｰ
                                    }

                                    //管電圧･管電流 最大最小値設定
                                    ModuleCTRLM.ipXRMaxkV = modData.gudtXraySystemValue.E_XraySetMaxkV;
                                    ModuleCTRLM.ipXRMinkV = modData.gudtXraySystemValue.E_XraySetMinkV;
                                    ModuleCTRLM.ipXRMaxmA = modData.gudtXraySystemValue.E_XraySetMaxuA;
                                    ModuleCTRLM.ipXRMinmA = modData.gudtXraySystemValue.E_XraySetMinuA;

                                    if (modData.gclsCtrlXrayL8121_02 == null)
                                    {
                                        modData.gclsCtrlXrayL8121_02 = new clsCtrlXrayL8121_02();
                                    }
                                }
                                break;
                            //追加2012/03/20(KS1)hata_L8121-02対応  ----------<


                            //追加2014/10/07(検S1)hata_64bit対応 ---------->
                            case modData.EVENT_L9181_02:
                                //浜ﾎﾄ（90kV L9181-02T）

                                if (ModuleCTRLM.ifEventValue != ModuleCTRLM.ifOldEventValue)
                                {
                                    XrayCtrlEnd(ModuleCTRLM.ifOldEventValue);	//X線制御終了処理
                                    ModuleCTRLM.ifOldEventValue = ModuleCTRLM.ifEventValue;

                                    //設定ﾌｧｲﾙ読み込み
                                    intRet = modIniFiles.ReadTSB_001_L9181();
                                    if (intRet != modIniFiles.FILE_OK)
                                    {
                                        //ﾌｧｲﾙ読み込みｴﾗｰ
                                    }

                                    //管電圧･管電流 最大最小値設定
                                    ModuleCTRLM.ipXRMaxkV = modData.gudtXraySystemValue.E_XraySetMaxkV;
                                    ModuleCTRLM.ipXRMinkV = modData.gudtXraySystemValue.E_XraySetMinkV;
                                    ModuleCTRLM.ipXRMaxmA = modData.gudtXraySystemValue.E_XraySetMaxuA;
                                    ModuleCTRLM.ipXRMinmA = modData.gudtXraySystemValue.E_XraySetMinuA;

                                    if (modData.gclsCtrlXrayL9181_02 == null)
                                    {
                                        modData.gclsCtrlXrayL9181_02 = new clsCtrlXrayL9181_02();
                                    }
                                }
                                break;
                            //追加2014/10/07(検S1)hata_64bit対応 ----------<

                            //Rev23.10 追加2015/10/01（検S1)長野  ---------->
                            case modData.EVENT_L12721:
                                //浜ﾎﾄ（300kV L12721）

                                if (ModuleCTRLM.ifEventValue != ModuleCTRLM.ifOldEventValue)
                                {
                                    XrayCtrlEnd(ModuleCTRLM.ifOldEventValue);	//X線制御終了処理
                                    ModuleCTRLM.ifOldEventValue = ModuleCTRLM.ifEventValue;

                                    //設定ﾌｧｲﾙ読み込み
                                    intRet = modIniFiles.ReadTSB_001_L12721();
                                    if (intRet != modIniFiles.FILE_OK)
                                    {
                                        //ﾌｧｲﾙ読み込みｴﾗｰ
                                    }

                                    //管電圧･管電流 最大最小値設定
                                    ModuleCTRLM.ipXRMaxkV = modData.gudtXraySystemValue.E_XraySetMaxkV;
                                    ModuleCTRLM.ipXRMinkV = modData.gudtXraySystemValue.E_XraySetMinkV;
                                    ModuleCTRLM.ipXRMaxmA = modData.gudtXraySystemValue.E_XraySetMaxuA;
                                    ModuleCTRLM.ipXRMinmA = modData.gudtXraySystemValue.E_XraySetMinuA;

                                    if (modData.gclsCtrlXrayL12721 == null)
                                    {
                                        modData.gclsCtrlXrayL12721 = new clsCtrlXrayL12721();
                                    }
                                }
                                break;
                            //Rev23.10 追加2015/10/01（検S1)長野  ----------<

                            //Rev23.10 追加2015/10/01（検S1)長野  ---------->
                            case modData.EVENT_L10711:
                                //浜ﾎﾄ（160kV L10711）

                                if (ModuleCTRLM.ifEventValue != ModuleCTRLM.ifOldEventValue)
                                {
                                    XrayCtrlEnd(ModuleCTRLM.ifOldEventValue);	//X線制御終了処理
                                    ModuleCTRLM.ifOldEventValue = ModuleCTRLM.ifEventValue;

                                    //設定ﾌｧｲﾙ読み込み
                                    intRet = modIniFiles.ReadTSB_001_L10711();
                                    if (intRet != modIniFiles.FILE_OK)
                                    {
                                        //ﾌｧｲﾙ読み込みｴﾗｰ
                                    }

                                    //管電圧･管電流 最大最小値設定
                                    ModuleCTRLM.ipXRMaxkV = modData.gudtXraySystemValue.E_XraySetMaxkV;
                                    ModuleCTRLM.ipXRMinkV = modData.gudtXraySystemValue.E_XraySetMinkV;
                                    ModuleCTRLM.ipXRMaxmA = modData.gudtXraySystemValue.E_XraySetMaxuA;
                                    ModuleCTRLM.ipXRMinmA = modData.gudtXraySystemValue.E_XraySetMinuA;

                                    if (modData.gclsCtrlXrayL10711 == null)
                                    {
                                        modData.gclsCtrlXrayL10711 = new clsCtrlXrayL10711();
                                    }
                                }
                                break;
                            //Rev23.10 追加2015/10/01（検S1)長野  ----------<
                        
                        }

                    }
                    catch
                    {
                        //Debug.Print("cCtrlM_Error ");

                    }


                }		// Lock(gLock)	


                //追加2014/06/09_hata
                if (!CtrlMflg) return;

                if (frmCtrlTimer == null) return;
                if (frmCtrlTimer.Stoped) return;
                if (!frmCtrlTimer.IsAlive) return;

                //追加2014/06/09_hata
                frmCtrlTimer.Sleep(Interval);
                if (!CtrlMflg) return;
                
            }
            //変更2014/06/09_hata
            //while (!frmCtrlTimer.Sleep(Interval));
            while (CtrlMflg);

            
        }

		/// <summary>
		///   X線制御の終了処理
		///
		///   Index As Integer    終了させるX線種
		///
		///   2004-02-14 Shibui
		/// </summary>
		/// <param name="Index"></param>
		private void XrayCtrlEnd(int Index)
		{
			IDisposable disposeObj = null;

			switch (Index)
			{
				case modData.EVENT_NO:
					//動作なし
					break;

				case modData.EVENT_FEIN_FOCUS:
					//FeinFocus
					mStart = 0;
					mStatusGet = 0;
					frmCtrlMSComm1.Close();
					frmCtrlMSComm1 = null;
					break;

				case modData.EVENT_STOP:
					//停止
					break;

				case modData.EVENT_KEVEX:
					//Kevex
					ModuleIOM.IOM_Terminate();
					break;

				case modData.EVENT_L9181:
					//浜ﾎﾄ(130kV L9181)
					disposeObj = modData.gclsCtrlXrayL9181S as IDisposable;
					modData.gclsCtrlXrayL9181S = null;
					break;

//2004-09-07 Shibui
				case modData.EVENT_L9421:
					//浜ﾎﾄ(90kV L9421)
					disposeObj = modData.gclsCtrlXrayL9421S as IDisposable;
					modData.gclsCtrlXrayL9421S = null;
					break;
				case modData.EVENT_L9191:
					//浜ﾎﾄ(160kV L9191)
					disposeObj = modData.gclsCtrlXrayL9191 as IDisposable;
					modData.gclsCtrlXrayL9191 = null;
					break;

                //追加2009/08/19（KSS)hata_L10801対応  ---------->
				case modData.EVENT_L10801:
					//浜ﾎﾄ(230kV L10801)
					disposeObj = modData.gclsCtrlXrayL10801 as IDisposable;
					modData.gclsCtrlXrayL10801 = null;
					break;
                //追加2009/08/19（KSS)hata_L10801対応  ----------<

                //追加2010/02/24(KSS)hata_L8601対応  ---------->
				case modData.EVENT_L8601:
					//浜ﾎﾄ(90kV L8601)
					//X線ON逃げするため切れるのを確認する
					XrayStop();	//追加2010/02/25(KSS)hata
					disposeObj = modData.gclsCtrlXrayL8601 as IDisposable;
					modData.gclsCtrlXrayL8601 = null;
					break;
                //追加2010/02/24(KSS)hata_L8601対応  ----------<

                //追加2010/05/11(KS1)hata_L9421-02T対応 ---------->
				case modData.EVENT_L9421_02:
					//浜ﾎﾄ(90kV L9421-02T)
					disposeObj = modData.gclsCtrlXrayL9421_02 as IDisposable;
					modData.gclsCtrlXrayL9421_02 = null;
					break;
                //追加2010/05/11(KS1)hata_L9421-02T対応 ----------<

                //追加2012/03/20(KS1)hata_L8121-02対応  ---------->
				case modData.EVENT_L8121_02:
					//浜ﾎﾄ(150kV L8121-02)
					//X線ON逃げするため切れるのを確認する
					XrayStop();	//追加2012/03/20(KS1)hata
					disposeObj = modData.gclsCtrlXrayL8121_02 as IDisposable;
					modData.gclsCtrlXrayL8121_02 = null;
					break;
                //追加2012/03/20(KS1)hata_L8121-02対応  ----------<

                //追加2014/10/07(検S1)hata_64bit対応  ---------->
				case modData.EVENT_L9181_02:
					//浜ﾎﾄ(150kV L8121-02)
					//X線ON逃げするため切れるのを確認する
					XrayStop();	//追加2012/03/20(KS1)hata
					disposeObj = modData.gclsCtrlXrayL9181_02 as IDisposable;
					modData.gclsCtrlXrayL9181_02 = null;
					break;
                //追加2014/10/07(検S1)hata_64bit対応  ----------<

                //追加2015/10/02(検S1)長野 Rev23.10 追加 ---------->
                case modData.EVENT_L12721:
                    //浜ﾎﾄ(300kV L12721)
                    //X線ON逃げするため切れるのを確認する
                    XrayStop();	//追加2012/03/20(KS1)hata
                    disposeObj = modData.gclsCtrlXrayL12721 as IDisposable;
                    modData.gclsCtrlXrayL12721 = null;
                    break;
                //追加2015/10/02(検S1)長野 Rev23.10 追加   ----------<

                //追加2015/10/02(検S1)長野 Rev23.10 追加   ---------->
                case modData.EVENT_L10711:
                    //浜ﾎﾄ(160kV L10711)
                    //X線ON逃げするため切れるのを確認する
                    XrayStop();	//追加2012/03/20(KS1)hata
                    disposeObj = modData.gclsCtrlXrayL10711 as IDisposable;
                    modData.gclsCtrlXrayL10711 = null;
                    break;
                //追加2015/10/02(検S1)長野 Rev23.10 追加   ----------<



			}

			if (disposeObj != null)
			{
				disposeObj.Dispose();
			}
		}


//1999-11-26 T.Shibui 6160FP
// OnComm ｲﾍﾞﾝﾄを使用して、通信ｲﾍﾞﾝﾄおよびｴﾗｰをﾄﾗｯﾌﾟします。
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmctrlMSComm1_OnComm(object sender, EventArgs e)
		{
			lock(gLock)
			{
//				Dim EVMsg$
//				Dim ERMsg$
				int i = 0;

				int ibuffer = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				// CommEvent ﾌﾟﾛﾊﾟﾃｨの値に従って処理を行います。
				object Buffer = null;
				Select Case frmCtrlMSComm1.CommEvent
					' ｲﾍﾞﾝﾄ ﾒｯｾｰｼﾞ
					Case MSCommLib.OnCommConstants.comEvReceive
						Buffer = frmCtrlMSComm1.Input
	'					Debug.Print "受信 - " & StrConv(Buffer, vbUnicode)
					Case Else
						'            ComErr = 1
						'        Case comEvSend
						'        Case comEvCTS''            EVMsg$ = "CTS ﾗｲﾝの状態が変化しました。"
						'        Case comEvDSR''            EVMsg$ = "DSR ﾗｲﾝの状態が変化しました。"
						'        Case comEvCD''            EVMsg$ = "CD ﾗｲﾝの状態が変化しました。"
						'        Case comEvRing''            EVMsg$ = "呼び出し信号を検出しました。"
						'        Case comEvEOF''            EVMsg$ ="EOF を受信しました。"
						'        ' ｴﾗｰ ﾒｯｾｰｼﾞ
						'        Case comBreak''            ERMsg$ = "ﾌﾞﾚｰｸ信号を検出しました。"
						'        Case comCDTO''            ERMsg$ = "CD ﾀｲﾑｱｳﾄを検出しました。"
						'        Case comCTSTO''            ERMsg$ = "CTS ﾀｲﾑｱｳﾄを検出しました。"
						'        Case comDCB''            ERMsg$ = "DCB 取得時にｴﾗｰが発生しました。"
						'        Case comDSRTO''            ERMsg$ = "DSR ﾀｲﾑｱｳﾄを検出しました。"
						'        Case comFrame''            ERMsg$ = "ﾌﾚｰﾐﾝｸﾞ ｴﾗｰが発生しました。"
						'        Case comOverrun''            ERMsg$ = "ｵｰﾊﾞｰﾗﾝ ｴﾗｰが発生しました。"
						'        Case comRxOver''            ERMsg$ = "受信ﾊﾞｯﾌｧがｵｰﾊﾞｰﾌﾛｰしました。"
						'        Case comRxParity''            ERMsg$ = "ﾊﾟﾘﾃｨ ｴﾗｰが発生しました。"
						'        Case comTxFull''            ERMsg$ = "送信ﾊﾞｯﾌｧ ﾌﾙ ｴﾗｰが発生しました。"
						'        Case Else''            ERMsg$ = "未定義のｴﾗｰまたはｲﾍﾞﾝﾄが発生しました。"
				End Select
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				int length = frmCtrlMSComm1.BytesToRead;

				byte[] data = new byte[length];

				frmCtrlMSComm1.Read(data, 0, length);

				ComKBuf = "";
				ComABuf = "";
				ComIBuf = "";
				ComNBuf = "";
				ComFBuf = "";
				ComEBuf = "";
				ComSBuf = "";

				//受信データ判定

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				RecBuffer = (StrConv(Buffer, vbUnicode))
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				RecBuffer = Encoding.ASCII.GetString(data);

				for (i = 0; i <= RecBuffer.Length; i++)
				{
					switch (RecBuffer.Substring(i, 1))
					{
						case "K":
							ComKBuf = "";
							ComK = 1;
							ComA = 0;
							ComI = 0;
							ComN = 0;
							ComF = 0;
							ComE = 0;
							ComS = 0;
							break;
						case "A":
							ComABuf = "";
							ComK = 0;
							ComA = 1;
							ComI = 0;
							ComN = 0;
							ComF = 0;
							ComE = 0;
							ComS = 0;
							break;
						case "I":
							ComIBuf = "";
							ComK = 0;
							ComA = 0;
							ComI = 1;
							ComN = 0;
							ComF = 0;
							ComE = 0;
							ComS = 0;
							break;
						case "N":
							ComNBuf = "";
							ModuleXRAYM.ComXon = 1;
							ModuleXRAYM.ComXoff = 0;
							ComA = 0;
							ComI = 0;
							ComN = 1;
							ComF = 0;
							ComE = 0;
							ComS = 0;
							break;
						case "F":
							ComFBuf = "";
							ModuleXRAYM.ComXon = 0;
							ModuleXRAYM.ComXoff = 1;
							ComK = 0;
							ComA = 0;
							ComI = 0;
							ComN = 0;
							ComF = 1;
							ComE = 0;
							ComS = 0;
							break;
						case "E":
							ComEBuf = "";
							ModuleXRAYM.ComErr = 1;
							ComK = 0;
							ComA = 0;
							ComI = 0;
							ComN = 0;
							ComF = 0;
							ComE = 1;
							ComS = 0;
							break;
						case "S":
							ComSBuf = "";
							ComK = 0;
							ComA = 0;
							ComI = 0;
							ComN = 0;
							ComF = 0;
							ComE = 0;
							ComS = 1;
							break;
						case "?":
							break;
						case "\r":
							break;
						case ":":
							break;
						case "Z":
							ComK = 0;
							ComA = 0;
							ComI = 0;
							ComN = 0;
							ComF = 0;
							ComE = 0;
							ComS = 0;
							break;
						default:
							char c = Convert.ToChar(RecBuffer.Substring(i, 1));

							if (c >= '0' && '9' >= c)
							{
								if (ComK != 0)
								{
									ComKBuf = ComKBuf + c;
								}
								if (ComA != 0)
								{
									ComABuf = ComABuf + c;
								}
								if (ComI != 0)
								{
									ComIBuf = ComIBuf + c;
								}
								if (ComN != 0)
								{
									ComNBuf = ComNBuf + c;
								}
								if (ComE != 0)
								{
									ComEBuf = ComEBuf + c;
								}
								if (ComF != 0)
								{
									ComFBuf = ComFBuf + c;
								}
								if (ComS != 0)
								{
									ComSBuf = ComSBuf + c;
								}
							}
							break;
					}
				}

				if (!string.IsNullOrEmpty(ComKBuf))
				{
					float.TryParse(ComKBuf, out ModuleXRAYM.ComkV);
				}
				if (!string.IsNullOrEmpty(ComABuf))
				{
					float.TryParse(ComABuf, out ModuleXRAYM.CommA);
				}
				if (!string.IsNullOrEmpty(ComSBuf))
				{
					int.TryParse(ComSBuf, NumberStyles.HexNumber, null, out ibuffer); 

					//インターロック
					if ((ibuffer & 0x1) != 0)
					{
						ModuleXRAYM.ComInterLock = 1;
					}
					else
					{
						ModuleXRAYM.ComInterLock = 0;
					}

					//Standby
					if ((ibuffer & 0x2) != 0)
					{
						ModuleXRAYM.ComStandby = 1;
					}
					else
					{
						ModuleXRAYM.ComStandby = 0;
					}
				}

				RecBuffer = "";
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Buf"></param>
		public void CommOutput(string Buf)
		{
			frmCtrlMSComm1.Write(Buf + "\r");

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
'			Debug.Print "送信 - " & buf
			' すべてのﾃﾞｰﾀが送信されるまで待機します。
			Do
			   If Err Then
				   Exit Do
			   End If

			   Ret = DoEvents()
			Loop Until frmCtrlMSComm1.OutBufferCount = 0
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		}


		/// <summary>
		///追加2010/02/25(KSS)hata
		///X線終了させ、切れるのを確認する
		/// </summary>
		private void XrayStop()
		{
			double sngTime = 0;
			double sngTime1 = 0;
			float sngDelayTime = 0;

			sngTime = DateTime.Now.TimeOfDay.TotalSeconds;
			sngDelayTime = 30;
			//X線終了させる
			if (ModuleCTRLM.ipX_On == modData.CTRL_ON)
			{
				ModuleCTRLM.ifXrayonoff_Set = 2;
				while ((ModuleCTRLM.ipX_On == modData.CTRL_ON))
				{
					sngTime1 = DateTime.Now.TimeOfDay.TotalSeconds;
					if (sngTime < (sngTime1 - sngDelayTime))
					{
						break;
					}
					if (sngTime1 - sngTime < 0)
					{
						sngTime = sngTime1;
					}

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
					System.Windows.Forms.Application.DoEvents();
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

					System.Threading.Thread.Sleep(0);
				}
			}
		}
	}
}
