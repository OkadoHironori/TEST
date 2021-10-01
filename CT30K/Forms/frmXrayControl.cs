using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Diagnostics;

//
using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;
using XrayCtrl;


namespace CT30K
{
    //public partial class frmXrayControl : Form
    public partial class frmXrayControl : FixedForm
    {
        private const int XRAY_HEIGHT = 168 + 6;   //変更 by 間々田 2009/07/21

        //ActiveXのインスタンス
        //#If DebugOn Then                                    'デバッグ時は仮想Ｘ線制御とする by 間々田 2004/11/29
        //Dim WithEvents UC_Feinfocus As frmVirtualXrayControl
        //#Else
        //Dim WithEvents UC_Feinfocus As clsTActiveX  'X線制御
        //#End If
        //FeinFocus.exe→XrayCtrl.exeへ変更  'v16.20変更 byやまおか 2010/04/21

//#if DebugOn
//Rev23.10 変更 by長野 2015/10/02
#if XrayDebugOn
        private frmVirtualXrayControl UC_XrayCtrl;      //デバッグ時は仮想Ｘ線制御
#else
        private clsTActiveX UC_XrayCtrl;                //X線制御
#endif

        //使っていないので削除2015/02/02hata
        //private bool byEvent = false;     

        //フィラメント調整中、I.I.電源をOFFするためのフラグ 'v11.5追加 by 間々田 2006/04/25
        private bool IIPowerOffForFilamentAdjust = false;

        //Ｘ線オン・オフ
        private modCT30K.OnOffStatusConstants myXrayOn;

        //Ｘ線アベイラブル
        private modCT30K.OnOffStatusConstants myXrayAvailable;

        private int tryConnectCount = 0;
        private int CountXrayError = 0;         //X線制御器エラー回数　added by 山本　2002-1-7
        private int XrayErrorFlag = 0;          //X線制御器エラー表示フラッグ added by 山本 2002-3-21
        private bool XrayOffByTimer = false;    //タイマーによるＸ線オフメッセージ表示用フラグ

        //Rev20.00 追加 test by長野 2015/02/09
        private bool WUstsEvntLock = false;

        //イベント宣言
        public event EventHandler FilterChanged; //Rev23.40 追加 by長野 2016/06/19

        public class ChangedEventArgs : EventArgs
        {
            public decimal volt;
            public decimal current;
        }
        public event EventHandler<ChangedEventArgs> Changed;
        //public event EventHandler UpdateByTimer;
        public event EventHandler XrayAvailableOn;   //v16.01 追加 by 山影 10-02-25


        //コントロール更新用のデリゲート
        delegate void XrayMechDataUpdateDelegate();
        clsTActiveX.MechData myXrayMechData = default(clsTActiveX.MechData);

        delegate void XrayStatusValueUpdateDelegate();
        clsTActiveX.StatusValue myXrayStatusValue = default(clsTActiveX.StatusValue);
        
        delegate void XrayValueUpdateDelegate();
        clsTActiveX.XrayValue myXrayValue = default(clsTActiveX.XrayValue);

        delegate void XrayErrSetUpdateDelegate();
        clsTActiveX.ErrSet myXrayErrSet = default(clsTActiveX.ErrSet);

        delegate void XrayUserValueUpdateDelegate();
        clsTActiveX.UserValue myXrayUserValue = default(clsTActiveX.UserValue);


        //メカコントロールフォーム参照用
        private frmMechaControl myMechaControl;

        //private bool FirstDone = false;
        //Rev23.10 staticへ変更 by長野 2015/10/17
        private static bool FirstDone = false;

        //Ｘ線のウォームアップ状態
        //private modXrayControl.XrayWarmUpConstants myXrayWarmUp;

        private modXrayControl.XrayWarmUpConstants myXrayWarmUp;
        
        //Rev23.10 外部からの参照用 by長野 2015/10/06
        public modXrayControl.XrayWarmUpConstants XrayWarmUp
        {
            get
            {
                return myXrayWarmUp;
            }
            set
            {
                myXrayWarmUp = value;
            }
        }

        //Ｘ線ステータスプロパティ用変数
        private string myXrayStatus = null;

        //フィラメント調整ステータスプロパティ用変数
        private string myFilamentAdjustStatus;

        //管電流計算に使用するパラメータ        'ほかでも使用するのでこのフォームのトップに移動 2009/08/06
        public const double ParaK = 2;

        //管電圧計算に使用するパラメータ        'ほかでも使用するのでこのフォームのトップに移動 2009/08/06
        public const double ParaL = 1;

        //前回選択していたＸ線条件   'v17.47/v14.53追加 by 間々田 2011/03/09
        private int LastCondIndex;

        //インターロックOFF時にウォームアップ完了のフラグが立っていない対策 by長野 v17.71 2012-03-28
        private bool FirstDoneWarmupSts;

        //段階ウォームアップ用変数   'v17.72/v19.02追加 byやまおか 2012/05/16
        private int myStepWU_Num;               //現在の段階
        private int StepWU_StsCnt;              //段階ウォームアップ中の状態監視用   'v19.02追加 byやまおか 2012/07/23

        //追加2014/10/07hata_v19.51反映
        int XrayCondIndex;          //ヒットしたX線条件のインデックス  'v18.00追加 byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
        float XrayCondVolt;        //ヒットしたX線条件の管電圧        'v18.00追加 byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
        float XrayCondCurrent;     //ヒットしたX線条件の管電流        'v18.00追加 byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

        //ウォームアップログファイル用変数   'v19.02追加 byやまおか 2012/07/20
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
        Dim WupLogfileNo            As Integer
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
        private StreamWriter WupLogfileNo;
        private string WupLogfile;                  //WUPyyyymmddhhmmss.log
        private string WupLogDir_WUP;               //C:\CT\TEMP\WUP
        private string WupLogDir_WUP_YYYYMM;        //C:\CT\TEMP\WUP\yyyymm
        private string WupLogPath;                  //C:\CT\TEMP\WUP\yyyymm\WUPyyyymmddhhmmss.log
        private int WupLogFirstCount;               //ログファイルの行数カウントの調整用

        ////Ｘ線スライダー更新フラグ
        //private bool XrayKVSldChangeflg = false;
        //private bool XrayMASldChangeflg = false;

        //Rev23.20 X線制御開始成功フラグ by長野 2016/01/19
        public bool XrayControlStartSuccessAtFLoad = false;

        //
        // tmrUpdate_Timer にて使用する static フィールド
        //
        private static bool tmrUpdate_Timer_Done;
        //private static int tmrUpdate_Timer_Count;

        //private Button[] cmdFocus = null;
        public Button[] cmdFocus = null;
        
        public Button[] cmdCondition = null;
        private Label[] lblVac = null;

        private static frmXrayControl _Instance = null;

        //ｲﾍﾞﾝﾄﾌﾟﾛｼｰｼﾞｬの2重呼び出し防止
        private static bool XrayCtrl_Error = false;    //状態(True:実行中,False:停止中)

        //Rev22.00 Rev21.01の反映 WUP前の管電圧・管電流値 追加 by長野 2015/07/20
        private int bakWUPkV = 0;
        private float bakWUPmA = 0.0f;

        //Rev20.01 L,M,Hと焦点切り替えボタン押下後の処理を2重で呼び出さない対策 by長野 2015/06/03
        private bool myLMHChangeBusy = false;
        public bool LMHBusy
        {
            get
            {
                return myLMHChangeBusy;
            }
            set
            {
                myLMHChangeBusy = value;
            }
        }
        private bool myFocusChangeBusy = false;
        public bool FocusChangeBusy
        {
            get
            {
                return myFocusChangeBusy;
            }
            set
            {
                myFocusChangeBusy = value;
            }
        }

        //Rev25.03/Rev25.02 WUP準備フラグ add by chouno 2017/03/07
        bool isWUPPreparation = false;

        //Rev23.40 by長野 2016/04/05 / Rev23.21 ワッテージ制御にかかった場合でも正しく電流を表示するための対策 by長野 2016/03/10
        private long tmpMA = -1;

        /// <summary>
        /// 
        /// </summary>
        public frmXrayControl()
        {
            InitializeComponent();

            cmdFocus = new Button[] { cmdFocus1, cmdFocus2, cmdFocus3, cmdFocus4 };
            cmdCondition = new Button[] { cmdCondition0, cmdCondition1, cmdCondition2 };
            lblVac = new Label[] { lblVac0, lblVac1, lblVac2, lblVac3, lblVac4, lblVac5 }; 
        }

        /// <summary>
        /// 
        /// </summary>
        public static frmXrayControl Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmXrayControl();
                }

                return _Instance;
            }
        }


        //*******************************************************************************
        //機　　能： XrayStatusプロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.5  06/06/12   (WEB)間々田    新規作成
        //*******************************************************************************
        public string XrayStatus
        {
            get { return myXrayStatus; }
            set
            {
                //変更時のみ設定する
                if (value == myXrayStatus) return;
                myXrayStatus = value;

                frmScanControl frmScanControl = frmScanControl.Instance;

                //ﾌｨﾗﾒﾝﾄ調整中、I.I.電源をOFFしていたら、ここで I.I.電源をONする 'v11.5追加 by 間々田 2006/04/25
                if (IIPowerOffForFilamentAdjust)
                {
                    //変更2014/10/07hata_v19.51反映
                    //17.20 検出器切替用に条件式を追加 by 長野 2010-09-03
                    if (CTSettings.SecondDetOn & mod2ndDetctor.IsDet2mode)
                    {
                        modSeqComm.SeqBitWrite("TVIIPowerOn", true);
                        IIPowerOffForFilamentAdjust = false;
                    }
                    else
                    {
                        modSeqComm.SeqBitWrite("IIPowerOn", true);
                        IIPowerOffForFilamentAdjust = false;
                    }
                }

                //ステータス文字列をオンテキストに設定
                //cwbtnXray.Text = myXrayStatus;
                ctLblXray.Caption = myXrayStatus;

                //追加2014/10/07hata_v19.51反映
                //X線のステータスをシーケンサへ知らせる  'v19.13追加 byやまおか 2013/04/12
                //下記の状態のとき
                //過負荷、接続エラー、異常、冷却水異常
                if ((myXrayStatus == CTResources.LoadResString(12754)) | 
                         (myXrayStatus == CTResources.LoadResString(12909)) |
                         (myXrayStatus == StringTable.GC_Xray_Error) | 
                         (myXrayStatus == StringTable.GC_STS_COOLANT_ERROR))
                {
                    //シーケンサへ異常を送る
                    modSeqComm.SeqBitWrite("stsXrayErr", true);

                }
                else
                {
                    //シーケンサへ正常を送る
                    modSeqComm.SeqBitWrite("stsXrayErr", false);
                }

                //色の設定
                //if (myXrayStatus == StringTable.GC_STS_STANDBY_OK) cwbtnXray.BackColor = Color.Lime;            //準備完了                        
                //else if (myXrayStatus == StringTable.GC_STS_CPU_BUSY) cwbtnXray.BackColor = Color.Red;          //処理中                         
                //else if (myXrayStatus == StringTable.GC_STS_Scan) cwbtnXray.BackColor = Color.Red;              //動作中                         
                //else if (myXrayStatus == StringTable.GC_STS_STANDBY_NG) cwbtnXray.BackColor = Color.Yellow;     //準備未完了                         
                //else if (myXrayStatus == StringTable.GC_STS_BUSY) cwbtnXray.BackColor = Color.Red;              //動作中                         
                //else if (myXrayStatus == StringTable.GC_Xray_WarmUp) cwbtnXray.BackColor = Color.Red;           //ｳｫｰﾑｱｯﾌﾟ中                         
                //else if (myXrayStatus == StringTable.GC_Xray_On) cwbtnXray.BackColor = Color.Red;               //Ｘ線ＯＮ中                         
                //else if (myXrayStatus == CTResources.LoadResString(12754)) cwbtnXray.BackColor = Color.Red;       //過負荷                         
                //else if (myXrayStatus == CTResources.LoadResString(12755)) cwbtnXray.BackColor = Color.Yellow;    //プリヒート                         
                //else if (myXrayStatus == CTResources.LoadResString(12757)) cwbtnXray.BackColor = Color.Yellow;    //スタンバイ                         
                //else if (myXrayStatus == CTResources.LoadResString(12909)) cwbtnXray.BackColor = Color.Red;       //接続エラー                         
                //else if (myXrayStatus == StringTable.GC_Xray_WarmUp_NG) cwbtnXray.BackColor = Color.Yellow;     //ｳｫｰﾑｱｯﾌﾟ未完了                         
                //else if (myXrayStatus == StringTable.GC_Xray_Error) cwbtnXray.BackColor = Color.Red;            //異常                         
                //else if (myXrayStatus == CTResources.LoadResString(12910)) cwbtnXray.BackColor = Color.Yellow;    //接続中...                         
                //else if (myXrayStatus == StringTable.GC_STS_FLM_RUNNING)                                        //ﾌｨﾗﾒﾝﾄ調整中   'v11.5追加 by 間々田 2006/04/04 
                if (myXrayStatus == StringTable.GC_STS_STANDBY_OK) ctLblXray.OnColor = Color.Lime;            //準備完了                        
                else if (myXrayStatus == StringTable.GC_STS_CPU_BUSY) ctLblXray.OnColor = Color.Red;          //処理中                         
                else if (myXrayStatus == StringTable.GC_STS_Scan) ctLblXray.OnColor = Color.Red;              //動作中                         
                else if (myXrayStatus == StringTable.GC_STS_STANDBY_NG) ctLblXray.OnColor = Color.Yellow;     //準備未完了                         
                else if (myXrayStatus == StringTable.GC_STS_BUSY) ctLblXray.OnColor = Color.Red;              //動作中                         
                else if (myXrayStatus == StringTable.GC_Xray_WarmUp) ctLblXray.OnColor = Color.Red;           //ｳｫｰﾑｱｯﾌﾟ中                         
                else if (myXrayStatus == StringTable.GC_Xray_On) ctLblXray.OnColor = Color.Red;               //Ｘ線ＯＮ中                         
                else if (myXrayStatus == CTResources.LoadResString(12754)) ctLblXray.OnColor = Color.Red;       //過負荷                         
                else if (myXrayStatus == CTResources.LoadResString(12755)) ctLblXray.OnColor = Color.Yellow;    //プリヒート                         
                else if (myXrayStatus == CTResources.LoadResString(12757)) ctLblXray.OnColor = Color.Yellow;    //スタンバイ                         
                else if (myXrayStatus == CTResources.LoadResString(12909)) ctLblXray.OnColor = Color.Red;       //接続エラー                         
                else if (myXrayStatus == StringTable.GC_Xray_WarmUp_NG) ctLblXray.OnColor = Color.Yellow;     //ｳｫｰﾑｱｯﾌﾟ未完了                         
                else if (myXrayStatus == StringTable.GC_Xray_Error) ctLblXray.OnColor = Color.Red;            //異常                         
                else if (myXrayStatus == CTResources.LoadResString(12910)) ctLblXray.OnColor = Color.Yellow;    //接続中...                         
                else if (myXrayStatus == StringTable.GC_STS_FLM_RUNNING)                                        //ﾌｨﾗﾒﾝﾄ調整中   'v11.5追加 by 間々田 2006/04/04 
                {
                    //cwbtnXray.BackColor = Color.Red;
                   // ctLblXray.OffColor = SystemColors.ButtonShadow;

                    //ﾌｨﾗﾒﾝﾄ調整中、I.I.電源をOFFする 'v11.5追加 by 間々田 2006/04/25
                    //If byEvent Then    'v15.10 If削除 byやまおか 2010/01/08
                    if (!CTSettings.detectorParam.Use_FlatPanel)           //v17.00 if追加 byやまおか 2010/02/23
                    {
                        IIPowerOffForFilamentAdjust = true;

                        //変更2014/10/07hata_v19.51反映
                        //v17.20 検出器切替用に条件式を追加
                        if (CTSettings.SecondDetOn & mod2ndDetctor.IsDet2mode)
                        {
                            modSeqComm.SeqBitWrite("TVIIPowerOff", true);
                        }
                        else
                        {
                            modSeqComm.SeqBitWrite("IIPoweroff", true);
                        }                   
                    
                    }
                    //End If             'v15.10 If削除 byやまおか 2010/01/08
                }
                //else if (myXrayStatus == StringTable.GC_STS_CNT_RUNNING) cwbtnXray.BackColor = Color.Red;       //ｾﾝﾀﾘﾝｸﾞ中      'v11.5追加 by 間々田 2006/04/04                         
                //else if (myXrayStatus == StringTable.GC_STS_CNT_FAILED) cwbtnXray.BackColor = Color.Red;        //ｾﾝﾀﾘﾝｸﾞ失敗    'added by 山本 2006-12-14                         
                //else if (myXrayStatus == StringTable.GC_STS_WUP_FAILED) cwbtnXray.BackColor = Color.Red;        //ｳｫｰﾑｱｯﾌﾟ失敗   'v11.5追加 by 間々田 2006/04/26                         
                //else if (myXrayStatus == StringTable.GC_STS_FLM_FAILED) cwbtnXray.BackColor = Color.Red;        //ﾌｨﾗﾒﾝﾄ調整失敗 'v11.5追加 by 間々田 2006/04/26                         
                //else if (myXrayStatus == StringTable.GC_STS_WUP_NOTREADY) cwbtnXray.BackColor = Color.Yellow;   //ｳｫｰﾑｱｯﾌﾟ未完   'v11.5追加 by 間々田 2006/04/26                         
                //else if (myXrayStatus == StringTable.GC_STS_FLM_NOTREADY) cwbtnXray.BackColor = Color.Yellow;   //ﾌｨﾗﾒﾝﾄ調整未完 'v11.5追加 by 間々田 2006/04/26                         

                //else if (myXrayStatus == StringTable.GC_STS_INT) cwbtnXray.BackColor = Color.Red;               //中断           'v12.01追加 by 間々田 2006/12/14
                //else if (myXrayStatus == StringTable.GC_STS_STOPPED) cwbtnXray.BackColor = Color.Red;           //停止           'v12.01追加 by 間々田 2006/12/14                         
                //else if (myXrayStatus == StringTable.GC_STS_VAC_NOTREADY) cwbtnXray.BackColor = Color.Yellow;   //真空未完       'v12.01追加 by 間々田 2006/12/14                         
                //else if (myXrayStatus == StringTable.GC_STS_COOLANT_ERROR) cwbtnXray.BackColor = Color.Red;     //冷却水異常     'v17.10追加 byやまおか 2010/08/25                                 
                else if (myXrayStatus == StringTable.GC_STS_CNT_RUNNING) ctLblXray.OnColor = Color.Red;       //ｾﾝﾀﾘﾝｸﾞ中      'v11.5追加 by 間々田 2006/04/04                         
                else if (myXrayStatus == StringTable.GC_STS_CNT_FAILED) ctLblXray.OnColor = Color.Red;        //ｾﾝﾀﾘﾝｸﾞ失敗    'added by 山本 2006-12-14                         
                else if (myXrayStatus == StringTable.GC_STS_WUP_FAILED) ctLblXray.OnColor = Color.Red;        //ｳｫｰﾑｱｯﾌﾟ失敗   'v11.5追加 by 間々田 2006/04/26                         
                else if (myXrayStatus == StringTable.GC_STS_FLM_FAILED) ctLblXray.OnColor = Color.Red;        //ﾌｨﾗﾒﾝﾄ調整失敗 'v11.5追加 by 間々田 2006/04/26                         
                else if (myXrayStatus == StringTable.GC_STS_WUP_NOTREADY) ctLblXray.OnColor = Color.Yellow;   //ｳｫｰﾑｱｯﾌﾟ未完   'v11.5追加 by 間々田 2006/04/26                         
                else if (myXrayStatus == StringTable.GC_STS_FLM_NOTREADY) ctLblXray.OnColor = Color.Yellow;   //ﾌｨﾗﾒﾝﾄ調整未完 'v11.5追加 by 間々田 2006/04/26                         

                else if (myXrayStatus == StringTable.GC_STS_INT) ctLblXray.OnColor = Color.Red;               //中断           'v12.01追加 by 間々田 2006/12/14
                else if (myXrayStatus == StringTable.GC_STS_STOPPED) ctLblXray.OnColor = Color.Red;           //停止           'v12.01追加 by 間々田 2006/12/14                         
                else if (myXrayStatus == StringTable.GC_STS_VAC_NOTREADY) ctLblXray.OnColor = Color.Yellow;   //真空未完       'v12.01追加 by 間々田 2006/12/14                         
                else if (myXrayStatus == StringTable.GC_STS_COOLANT_ERROR) ctLblXray.OnColor = Color.Red;     //冷却水異常     'v17.10追加 byやまおか 2010/08/25                                 
                else if (myXrayStatus == StringTable.GC_STS_PANEL_ON) ctLblXray.OnColor = Color.Yellow;     //パネルON       'v18.00追加 byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07   //追加2014/10/07hata_v19.51反映

                //ｳｫｰﾑｱｯﾌﾟ中の時は点滅させる
                //cwbtnXray.OnImage.CWImage.BlinkInterval = (myXrayStatus == StringTable.GC_Xray_WarmUp ? CWSpeeds.cwSpeedMedium : CWSpeeds.cwSpeedOff);
                ctLblXray.BlinkInterval = (myXrayStatus == StringTable.GC_Xray_WarmUp ? BlinkSpeeds.cwSpeedMedium : BlinkSpeeds.cwSpeedOff);
                
                //文字長が長い場合フォントサイズを小さくする
                //cwbtnXray.Font = new Font(cwbtnXray.Font.Name, (Winapi.lstrlen(myXrayStatus) > 12 ? 9 : 11));
                ctLblXray.Font = new Font(ctLblXray.Font.Name, (Winapi.lstrlen(myXrayStatus) > 12 ? 9 : 11));

                //リフレッシュ
                //cwbtnXray.Refresh();
                ctLblXray.Refresh();

                //v15.10追加(ここから) byやまおか 2010/01/08
                //ｳｫｰﾑｱｯﾌﾟ中、ﾌｨﾗﾒﾝﾄ調整中
                if (myXrayStatus == StringTable.GC_Xray_WarmUp || myXrayStatus == StringTable.GC_STS_FLM_RUNNING)
                {
                    //X線操作を禁止する
                    SetWUControlEnabled(false);
                }
                //それ以外
                else
                {
                    //禁止したX線操作を有効にする
                    SetWUControlEnabled(true);
                }
                //v15.10追加(ここまで) byやまおか 2010/01/08

                //スキャンコントロールのプリセット設定を無効化(X線ON中に条件を変えさせないため)  'v17.48/v14.53追加 byやまおか 2011/03/21
                //スキャンコントロールのプリセット設定を無効化(X線ON中に条件を変えさせないため)  'v17.48/v14.53追加 byやまおか 2011/03/21
                frmScanControl.cmdPresetRef.Enabled = (myXrayStatus != StringTable.GC_Xray_On);
            }
        }


        //*******************************************************************************
        //機　　能： フィラメント調整ステータスプロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.0  09/08/01   (SS1)間々田    新規作成
        //*******************************************************************************
        public string FilamentAdjustStatus
        {
            get { return myFilamentAdjustStatus; }
            set
            {
                //変更時のみ設定する
                if (value == myFilamentAdjustStatus) return;
                myFilamentAdjustStatus = value;

                //メッセージクリア
                //Unload frmMessage
                modCT30K.HideMessage();     //変更 by 間々田 2009/08/24

                //調整中
                if (myFilamentAdjustStatus == CTResources.LoadResString(StringTable.IDS_AlignmentNow))
                {
                    //自動的にフィラメント調整が始まった場合
                    if (modXrayControl.FilamentAdjustAuto)
                    {
                        //frmMessage.lblMessage = "フィラメント調整が必要になりましたので完了するまでしばらくお待ち下さい。"
                        //ShowMessage "フィラメント調整が必要になりましたので完了するまでしばらくお待ち下さい。"                    '変更 by 間々田 2009/08/24
                        modCT30K.ShowMessage(CTResources.LoadResString(20102));   //ストリングテーブル化　'v17.60 by長野 2011/05/22
                    }
                    else if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeViscom)
                    {
                        modXrayControl.FilamentAdjustAuto = true;
                    }
                }
                //準備未完
                else if (myFilamentAdjustStatus == StringTable.GC_STS_STANDBY_NG2)
                {
                    //フィラメントが未完了になったらウォームアップフレーム内の
                    //「フィラメント調整も行う」チェックボックスにチェックを入れる
                    chkFilament.CheckState = CheckState.Checked;
                }

                //Ｘ線情報をファイルに書き込む
                modXrayinfo.WriteXrayInfo();
            }
        }


        //*******************************************************************************
        //機　　能： Ｘ線オン・オフステータスプロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.5  06/06/12   (WEB)間々田    新規作成
        //*******************************************************************************
        public modCT30K.OnOffStatusConstants MecaXrayOn
        {
            get { return myXrayOn; }
            set
            {
                //変更時のみ設定する
                if (value == myXrayOn) return;
                myXrayOn = value;
                
                frmCTMenu frmCTMenu = frmCTMenu.Instance;
                frmMechaControl frmMechaControl = frmMechaControl.Instance;

                //ツールバー上のＸ線ボタンに反映
                if (myXrayOn == modCT30K.OnOffStatusConstants.OnStatus)
                {
                    if (myXrayAvailable == modCT30K.OnOffStatusConstants.OnStatus)
                    {
                        frmCTMenu.XrayOnOffStatus = frmCTMenu.XrayOnOffStatusConstants.XrayOnAvail;
                    }
                    else
                    {
                        frmCTMenu.XrayOnOffStatus = frmCTMenu.XrayOnOffStatusConstants.XrayOnNotAvail;
                    }
                    //メカコントロール欄のX線にも反映    'v15.10追加 byやまおか 2009/11/26
                    //frmMechaControl.ImgXrayTube.Image = frmMechaControl.ImageList1.Images["XrayTubeON"];
                    frmMechaControl.ImgXrayTube.Image = Resources.XrayTubeR.ToBitmap();
                }
                else
                {
                    if (myXrayAvailable == modCT30K.OnOffStatusConstants.OnStatus)
                    {
                        frmCTMenu.XrayOnOffStatus = frmCTMenu.XrayOnOffStatusConstants.XrayOffAvail;
                    }
                    else
                    {
                        frmCTMenu.XrayOnOffStatus = frmCTMenu.XrayOnOffStatusConstants.XrayOffNotAvail;
                    }
                    //メカコントロール欄のX線にも反映    'v15.10追加 byやまおか 2009/11/26
                    //frmMechaControl.ImgXrayTube.Image = frmMechaControl.ImageList1.Images["XrayTubeOFF"];
                    frmMechaControl.ImgXrayTube.Image = Resources.XrayTubeG.ToBitmap();
                }

                //Ｘ線オンオフ状態をシーケンサに通知
                modSeqComm.SeqBitWrite("stsXrayOn", (myXrayOn == modCT30K.OnOffStatusConstants.OnStatus), false);

                //Ｘ線情報をファイルに書き込む
                modXrayinfo.WriteXrayInfo();
            }
        }


        //*******************************************************************************
        //機　　能： Ｘ線アベイラブルステータスプロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.5  06/06/12   (WEB)間々田    新規作成
        //*******************************************************************************
        public modCT30K.OnOffStatusConstants MecaXrayAvailable
        {
            get { return myXrayAvailable; }
            set
            {
                //変更時のみ設定する
                if (value == myXrayAvailable) return;
                myXrayAvailable = value;

                frmCTMenu frmCTMenu = frmCTMenu.Instance;

                //ツールバー上のＸ線ボタンに反映
                if (myXrayOn == modCT30K.OnOffStatusConstants.OnStatus)
                {
                    if (myXrayAvailable == modCT30K.OnOffStatusConstants.OnStatus)
                    {
                        frmCTMenu.XrayOnOffStatus = frmCTMenu.XrayOnOffStatusConstants.XrayOnAvail;
                        if (XrayAvailableOn != null)
                        {
                            XrayAvailableOn(this, EventArgs.Empty);                   //Ｘ線アベイラブルイベント v16.01 追加 by 山影 10-02-25
                        }
                    }
                    else
                    {
                        frmCTMenu.XrayOnOffStatus = frmCTMenu.XrayOnOffStatusConstants.XrayOnNotAvail;
                    }
                }
                else
                {
                    if (myXrayAvailable == modCT30K.OnOffStatusConstants.OnStatus)
                    {
                        frmCTMenu.XrayOnOffStatus = frmCTMenu.XrayOnOffStatusConstants.XrayOffAvail;
                    }
                    else
                    {
                        frmCTMenu.XrayOnOffStatus = frmCTMenu.XrayOnOffStatusConstants.XrayOffNotAvail;
                    }
                }

                //Ｘ線情報をファイルに書き込む
                modXrayinfo.WriteXrayInfo();
            }
        }


        //*******************************************************************************
        //機　　能： 「手動設定」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2009/07/21 (SS1)間々田  リニューアル
        //*******************************************************************************
        private void ctbtnManualSet_Click(object sender, EventArgs e)
        {
            //ボタンのオンオフを反転
            ctbtnManualSet.Value = !ctbtnManualSet.Value;

            //表示位置の調整
            ntbActVolt.Left = (ctbtnManualSet.Value ? 224 : cwneKV.Left);
            ntbActCurrent.Left = (ctbtnManualSet.Value ? 224 : cwneMA.Left);

            //手動設定時のみ設定用コントロールを表示する
            cwneKV.Visible = ctbtnManualSet.Value;
            lblKVuni.Visible = ctbtnManualSet.Value;
            cwsldKV.Visible = ctbtnManualSet.Value;
            cwneMA.Visible = ctbtnManualSet.Value;
            lblMAuni.Visible = ctbtnManualSet.Value;
            cwsldMA.Visible = ctbtnManualSet.Value;
        }

        //*******************************************************************************
        //機　　能： 設定管電圧変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*******************************************************************************
        private void ntbSetVolt_ValueChanged(object sender, NumTextBox.ValueChangedEventArgs e)
        {
            //    '管電圧設定中の場合
            //    If Not cwsldKV.Enabled Then
            //        If Value = cwsldKV.Value Then cwsldKV.Enabled = True
            //    End If

            //Ｘ線条件を更新
            UpdateXrayCondition();  //追加 by 間々田 2009/02/17

            //イベント生成
            ChangedEventArgs args = new ChangedEventArgs();
            args.volt = ntbSetVolt.Value;
            args.current = ntbSetCurrent.Value;
            if (Changed != null)
            {
                Changed(this, args);
            }
        }


        //*******************************************************************************
        //機　　能： 設定管電流変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*******************************************************************************
        private void ntbSetCurrent_ValueChanged(object sender, NumTextBox.ValueChangedEventArgs e)
        {
            //    '管電流設定中の場合
            //    If Not cwsldMA.Enabled Then
            //        If Value = cwsldMA.Value Then cwsldMA.Enabled = True
            //    End If

            //Ｘ線条件を更新
            UpdateXrayCondition();  //追加 by 間々田 2009/02/17

            //イベント生成
            ChangedEventArgs args = new ChangedEventArgs();
            args.volt = ntbSetVolt.Value;
            args.current = ntbSetCurrent.Value;
            if (Changed != null)
            {
                Changed(this, args);
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
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*******************************************************************************
        private void Form_Load(object sender, EventArgs e)
        {
            //変数初期化
            myXrayOn = modCT30K.OnOffStatusConstants.UnknownStatus;
            myXrayAvailable = modCT30K.OnOffStatusConstants.UnknownStatus;
            IIPowerOffForFilamentAdjust = false;
            tryConnectCount = 0;
            CountXrayError = 0;
            XrayErrorFlag = 0;
            XrayOffByTimer = false;
            FirstDone = false;
            //v17.71 追加 by長野 2012-03-28
            FirstDoneWarmupSts = false;

            //Rev20.00 追加 by長野 2015/02/09
            WUstsEvntLock = true;

            //前回選択していたＸ線条件   'v17.47/v14.53追加 by 間々田 2011/03/09
            LastCondIndex = -1;

            //フォームの表示位置：メカ制御画面の真下
            frmMechaControl frmMechaControl = frmMechaControl.Instance;

            //Me.Move .Left, .Top + .Height, FmControlWidth
            this.SetBounds(frmMechaControl.Left,
                           frmMechaControl.Top + frmMechaControl.Height,
                           modCT30K.FmControlWidth,
                           XRAY_HEIGHT);            //変更 by 間々田 2009/07/21

            //Rev23.10 追加 by長野 2015/10/15
            try
            {
                //キャプションのセット
                SetCaption();

                //v17.60 英語用レイアウト by 長野 2011/05/24
                if (modCT30K.IsEnglish == true)
                {
                    EnglishAdjustLayout();
                }

                //クラスオブジェクトを参照
                //Set UC_Feinfocus = XrayControl
                UC_XrayCtrl = modXrayControl.XrayControl;       //v16.20変更 byやまおか 2010/04/21

                //#if (!DebugOn)            
                //Rev23.10 変更 by長野 2015/10/02
#if (!XrayDebugOn)

                UC_XrayCtrl.ErrSetDisp += new clsTActiveX.ErrSetDispEventHandler(UC_XrayCtrl_ErrSetDisp);
                UC_XrayCtrl.MechDataDisp += new clsTActiveX.MechDataDispEventHandler(UC_XrayCtrl_MechDataDisp);
                UC_XrayCtrl.StatusValueDisp += new clsTActiveX.StatusValueDispEventHandler(UC_XrayCtrl_StatusValueDisp);
                UC_XrayCtrl.UserValueDisp += new clsTActiveX.UserValueDispEventHandler(UC_XrayCtrl_UserValueDisp);
                UC_XrayCtrl.XrayValueDisp += new clsTActiveX.XrayValueDispEventHandler(UC_XrayCtrl_XrayValueDisp);

#else
            UC_XrayCtrl.ErrSetDisp += new frmVirtualXrayControl.ErrSetDispEventHandler(UC_XrayCtrl_ErrSetDisp);
            UC_XrayCtrl.MechDataDisp += new frmVirtualXrayControl.MechDataDispEventHandler(UC_XrayCtrl_MechDataDisp);
            UC_XrayCtrl.StatusValueDisp += new frmVirtualXrayControl.StatusValueDispEventHandler(UC_XrayCtrl_StatusValueDisp);
            UC_XrayCtrl.UserValueDisp += new frmVirtualXrayControl.UserValueDispEventHandler(UC_XrayCtrl_UserValueDisp);
            UC_XrayCtrl.XrayValueDisp += new frmVirtualXrayControl.XrayValueDispEventHandler(UC_XrayCtrl_XrayValueDisp);
#endif

                //v17.20 起動時に０が入ったまま計算を進めて0割してしまう仮対策 by長野 10/09/09
                //変更2014/11/25hata
                //cwneKV.Minimum = 1;
                //cwneMA.Minimum = 1;
                if (cwneKV.Value < 1)
                {
                    cwneKV.Value = 1;
                    cwneKV.Minimum = 1;
                }
                if (cwneMA.Value < 1)
                {
                    cwneMA.Value = 1;
                    cwneMA.Minimum = 1;
                }

                //変更2014/10/07hata_v19.51反映
                //modXrayControl.XrayControlStart();
                //v18.00変更 byやまおか 2011/03/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                if (!modXrayControl.XrayControlStart())
                {
                    XrayControlStartSuccessAtFLoad = false;
                    //MsgBox "X線制御を開始できませんでした。", vbCritical
                    //v19.50 リソース化 by長野 2013/11/20
                    MessageBox.Show(CTResources.LoadResString(9601), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Rev23.20 終了はしないように変更 by長野 2016/01/11
                    //frmCTMenu.Instance.Close();
                    return;
                }
                else
                {
                    XrayControlStartSuccessAtFLoad = true;
                }

                //Rev23.10 X線切替後すぐにプロパティを読み出すと前のX線の値が残っているので、少し待つ
                modCT30K.PauseForDoEvents(3);

                //コントロールの初期設定
                InitControls();

                //メカ制御画面への参照
                myMechaControl = frmMechaControl;
                myMechaControl.IIMovingStopped += new EventHandler(myMechaControl_IIMovingStopped);

                //変更2014/10/07hata_v19.51反映
                //Viscomのときは更新タイマーを遅くする   'v17.72/v19.02追加 byやまおか 2012/05/16
                //    If (XrayType = XrayTypeViscom) Then
                //        tmrUpdate.Interval = 1500
                //    End If
                //Titan追加 by長野 2014/01/14 'v19.50 by長野
                //if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeViscom |
                //     modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan))
                //{
                //    tmrUpdate.Interval = 1500;
                //}

                //Titan追加 by長野 2014/01/14 'v19.50 by長野
                if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeViscom)
                {
                    tmrUpdate.Interval = 1500;
                }
                //else if(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan)
                else if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan)
                {
                    //tmrUpdate.Interval = 1000;
                    tmrUpdate.Interval = 1500; //Rev25.02 change by chouno 2017/02/07
                }
                else if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman)//Rev25.03/Rev25.02 change by chouno 2017/02/05 
                {
                    tmrUpdate.Interval = 1500;
                }

                //更新用タイマーオン
                tmrUpdate.Enabled = true;
                
                //追加2014/10/07hata_v19.51反映
                //Titan用Updateタイマー   'v18.00追加 byやまおか 2011/03/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                //Rev23.40 変更 by長野 2016/06/19
                //Rev25.03/Rev25.02 change by chouno 2017/02/07
                //if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) && (CTSettings.scaninh.Data.xray_remote == 0))
                if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan || modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman) && (CTSettings.scaninh.Data.xray_remote == 0))
                {
                    tmrTitanUpdate.Enabled = true;
                    tmrTitanUpdate.Interval = 10000;                //10秒ごとに書き込む
                }

                //    'Viscom用Liveタイマー   'v17.21追加 byやまおか 2010/10/06
                //    If (XrayType = XrayTypeViscom) Then
                //        tmrViscomLive.Enabled = True
                //    End If

                //Rev20.01 追加 by長野 2015/06/03
                modCT30K.PauseForDoEvents(1);

                //Ｘ線条件の設定
                int xcond = CTSettings.scansel.Data.x_condition;
                if (xcond > 0)
                    cmdCondition_Click(cmdCondition[Convert.ToInt32(xcond - 1)], EventArgs.Empty);    //追加 2009/06/15
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //Rev20.00 追加 by長野 2015/02/09
            //Rev20.00 WUPボタンを押したときに変更 by長野 2015/04/14
            //WUstsEvntLock = false;

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
        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Timerは停止させる   //追加2014/04/11(検S1)hata
            tmrUpdate.Enabled = false;
            tmrXrayTool.Enabled = false;
            tmrViscomLive.Enabled = false;


            //メカ制御画面への参照破棄
            if (modLibrary.IsExistForm(myMechaControl)) 
            //if (modLibrary.IsExistForm("frmMechaControl"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
            {
                myMechaControl.IIMovingStopped -= myMechaControl_IIMovingStopped;
                myMechaControl = null;
            }
            //Ｘ線条件を書き込む                                 '追加 2009/06/15
            CTSettings.scansel.Data.x_condition = modLibrary.GetCmdButton(cmdCondition) + 1;
            //modScansel.PutScansel(ref CTSettings.scansel.Data);
            CTSettings.scansel.Write();

            //浜ホトの場合、ウォームアップ中は管電圧・管電流をコモンに書き込まない  added by 間々田 2004/04/09
            //If (XrayType <> XrayTypeHamaL9181) Or (UC_Feinfocus.Up_Warmup <> 1) Then
            //If ((XrayType <> XrayTypeHamaL9181) And (XrayType <> XrayTypeHamaL9191)) Or (UC_Feinfocus.Up_Warmup <> 1) Then 'v9.5 修正 by 間々田 2004/09/15 浜ホト160kV対応
            //If ((XrayType <> XrayTypeHamaL9181) And (XrayType <> XrayTypeHamaL9191) And (XrayType <> XrayTypeHamaL9421)) Or (XrayWarmUp <> XrayWarmUpNow) Then 'v9.6 修正 by 間々田 2004/10/15 浜ホト90kV対応
            //If ((XrayType <> XrayTypeHamaL9181) And _
            //    (XrayType <> XrayTypeHamaL9191) And _
            //    (XrayType <> XrayTypeHamaL9421) And _
            //    (XrayType <> XrayTypeHamaL9421) And _
            //    (XrayType <> XrayTypeViscom)) Or _
            //    (XrayWarmUp <> XrayWarmUpNow) Then 'v9.6 修正 by 間々田 2004/10/15 浜ホト90kV対応
            //If ((XrayType <> XrayTypeHamaL9181) And _
            //    (XrayType <> XrayTypeHamaL9191) And _
            //    (XrayType <> XrayTypeHamaL9421) And _
            //    (XrayType <> XrayTypeHamaL9421) And _
            //    (XrayType <> XrayTypeHamaL10801) And _
            //    (XrayType <> XrayTypeViscom)) Or _
            //(XrayWarmUp <> XrayWarmUpNow) Then  'v15.10変更 byやまおか 2009/10/07
            //If ((XrayType <> XrayTypeHamaL9181) And _
            //    (XrayType <> XrayTypeHamaL9191) And _
            //    (XrayType <> XrayTypeHamaL9421) And _
            //    (XrayType <> XrayTypeHamaL10801) And _
            //    (XrayType <> XrayTypeHamaL8601) And _
            //    (XrayType <> XrayTypeViscom)) Or _
            //    (XrayWarmUp <> XrayWarmUpNow) Then  'v16.03/v16.20変更 byやまおか 2010/03/03
            //    If ((XrayType <> XrayTypeHamaL9181) And _
            //        (XrayType <> XrayTypeHamaL9191) And _
            //        (XrayType <> XrayTypeHamaL9421) And _
            //        (XrayType <> XrayTypeHamaL10801) And _
            //        (XrayType <> XrayTypeHamaL8601) And _
            //        (XrayType <> XrayTypeHamaL9421_02T) And _
            //        (XrayType <> XrayTypeViscom)) Or _
            //        (XrayWarmUp <> XrayWarmUpNow) Then  'v16.30 02T追加 byやまおか 2010/05/21
            if (((modXrayControl.XrayType != modXrayControl.XrayTypeConstants.XrayTypeHamaL9181) &&
                 (modXrayControl.XrayType != modXrayControl.XrayTypeConstants.XrayTypeHamaL9181_02) &&  //追加2014/11/05hata L9181-02に対応
                 (modXrayControl.XrayType != modXrayControl.XrayTypeConstants.XrayTypeHamaL9191) &&
                 (modXrayControl.XrayType != modXrayControl.XrayTypeConstants.XrayTypeHamaL9421) &&
                 (modXrayControl.XrayType != modXrayControl.XrayTypeConstants.XrayTypeHamaL10801) &&
                 (modXrayControl.XrayType != modXrayControl.XrayTypeConstants.XrayTypeHamaL8601) &&
                 (modXrayControl.XrayType != modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T) &&
                 (modXrayControl.XrayType != modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02) &&
                 (modXrayControl.XrayType != modXrayControl.XrayTypeConstants.XrayTypeHamaL10711) && //Rev23.10 追加 by長野 2015/10/01
                 (modXrayControl.XrayType != modXrayControl.XrayTypeConstants.XrayTypeHamaL12721) && //Rev23.10 追加 by長野 2015/10/01
                 (modXrayControl.XrayType != modXrayControl.XrayTypeConstants.XrayTypeViscom)) ||
                (modXrayControl.XrayWarmUp() != modXrayControl.XrayWarmUpConstants.XrayWarmUpNow))     //v17.71 追加 by長野 2012/03/14
            {
                CTSettings.scansel.Data.scan_kv = (float)cwneKV.Value;       //管電圧
                CTSettings.scansel.Data.scan_ma = (float)cwneMA.Value;       //管電流
                //modScansel.PutScansel(ref CTSettings.scansel.Data);
                CTSettings.scansel.Write();
            }

            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //Viscom用アライメント調整フォームをアンロード   'v11.5追加 by 間々田 2006/04/10
            //    Unload frmViscomAlignment
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //FeinFocus以外はＸ線をOFFする
            //    If XrayType <> XrayTypeFeinFocus Then
            //XrayControl.Xrayonoff_Set 2            'Ｘ線をオフする
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''            

            if (myXrayOn == modCT30K.OnOffStatusConstants.OnStatus) modXrayControl.XrayOff();

            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //    End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''


            //frmMessage.lblMessage = "Ｘ線制御ソフトを終了させています..."
            //frmMessage.lblMessage.Refresh
            //ShowMessage "Ｘ線制御ソフトを終了させています..."  '変更 by 間々田 2009/08/24
            modCT30K.ShowMessage(CTResources.LoadResString(20103));   //ストリングテーブル化 'v17.60 by長野  2011/05/22
            //Ｘ線制御終了処理
            modXrayControl.XrayControlStop();

            //Ｘ線制御オブジェクト参照を破棄
            //Set UC_Feinfocus = Nothing

            UC_XrayCtrl = null;  //v16.20変更 byやまおか 2010/04/21

            //Ｘ線制御オブジェクト破棄       'V9.5 追加 by 間々田 2004/09/24
            modXrayControl.XrayControl.Dispose();
            modXrayControl.XrayControl = null;
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
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************
        private void SetCaption()
        {
            //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);

            //v17.60 英語版の場合、管電圧、管電流は文字が多いのでTubeを省略 2011/06/11 by長野
            if (modCT30K.IsEnglish)
            {
                System.Globalization.TextInfo ti = System.Globalization.CultureInfo.InvariantCulture.TextInfo;

                ntbSetVolt.Caption = ti.ToTitleCase(ntbSetVolt.Caption.Replace("Tube ", "").ToLowerInvariant());
                ntbSetCurrent.Caption = ti.ToTitleCase(ntbSetCurrent.Caption.Replace("Tube ", "").ToLowerInvariant());
            }

            //v17.60 「X線条件」はストリングテーブル化してあるため不要 by長野 2011/05/25
            //Ｘ線条件フレーム
            //fraCondition.Caption = BuildResStr(IDS_Conditions, IDS_Xray)    'Ｘ線条件

            //管電流の単位の設定
            lblMAuni.Text = modXrayControl.CurrentUni;                              //μA
            ntbActCurrent.Unit = modXrayControl.CurrentUni;                         //μA
            ntbTargetCurrent.Unit = modXrayControl.CurrentUni;                      //μA

            //その他
            //    cmdXrayInfo.Caption = LoadResString(IDS_XrayInfo) & "..."       'Ｘ線情報
            //   cmdCondition(0).ToolTipText = "樹脂用"
            //   cmdCondition(1).ToolTipText = "アルミ用"
            //   cmdCondition(2).ToolTipText = "鉄用"
            
            //ストリングテーブル化　'v17.60 by長野 2011/05/22
            toolTip.SetToolTip(cmdCondition0, CTResources.LoadResString(20104));
            toolTip.SetToolTip(cmdCondition1, CTResources.LoadResString(20105));
            toolTip.SetToolTip(cmdCondition2, CTResources.LoadResString(20106));

            //Titanの場合は「フィルター厚」を表示する(「推奨フィルター厚」ではない) 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //v18.00追加 byやまおか 2011/03/15
            //Rev25.03/Rev25.02 change by chouno 2017/02/05
            //if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan))
            if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) || (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman))
            {
                ntbFilter.Caption = CTResources.LoadResString(16114);               //フィルター厚
                //Rev23.20 変更 by長野 2015/11/20
                //ntbFilter.Unit = "";                                                //単位なし
                //Rev23.20 変更 by長野 2015/11/20
                //ntbFilter.CaptionWidth = 87;
            }
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
            //ウォームアップステータス
            fraWarmup.Visible = (modXrayControl.XrayType != modXrayControl.XrayTypeConstants.XrayTypeFeinFocus);

            //管電圧／管電流（実測値）
            switch (modXrayControl.XrayType)
            {
                //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeToshibaEXM2_150
                //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeToshibaEXM2_150, XrayTypeViscom 'v11.5変更 by 間々田 2006/04/10
                //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeToshibaEXM2_150, XrayTypeViscom, XrayTypeHamaL10801   'v15.10変更 byやまおか 2009/10/07
                //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeToshibaEXM2_150, XrayTypeViscom, XrayTypeHamaL10801, XrayTypeHamaL8601    'v16.03/v16.20変更 byやまおか 2010/03/03
                //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeToshibaEXM2_150, XrayTypeViscom, XrayTypeHamaL10801, XrayTypeHamaL8601, XrayTypeHamaL9421_02T 'v16.30 02T追加 byやまおか 2010/05/21
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9191:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
                case modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150:
                case modXrayControl.XrayTypeConstants.XrayTypeViscom:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL8601:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02:     //v17.71 追加 by長野 2012/03/14
                case modXrayControl.XrayTypeConstants.XrayTypeGeTitan: //'v17.71 追加 by長野 2012/03/14  'v19.50 v19.41とv18.02の統合 by長野 2013/11/07//追加2014/10/07hata_v19.51反映
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181_02://追加2014/10/07hata_v19.51反映
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL12721://追加 Rev23.10 2015/10/01 by長野
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10711://追加 Rev23.10 2015/10/01 by長野
                case modXrayControl.XrayTypeConstants.XrayTypeSpellman:  //add Rev25.03/Rev25.02 2017/02/05 by chouno

                    //fraActKVMA.Visible = True
                    ntbActVolt.Visible = true;
                    ntbActCurrent.Visible = true;
                    break;

                default:            //v17.44追加 byやまおか 2011/02/16
                    ntbActVolt.Visible = false;
                    ntbActCurrent.Visible = false;
                    break;
            }

            //焦点切替えフレーム
            switch (modXrayControl.XrayType)
            {
                //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421
                //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL9421_02T  'v16.30 02T追加 byやまおか 2010/05/21
                case modXrayControl.XrayTypeConstants.XrayTypeKevex:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9191:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02:     //v17.71 追加 by長野 2012/03/14
                case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:          //'v17.71 追加 by長野 2012/03/14 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07   //追加2014/10/07hata_v19.51反映
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181_02:     //追加2014/10/07hata_v19.51反映
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10711:       //Rev23.10 追加 by長野 2015/10/01
                case modXrayControl.XrayTypeConstants.XrayTypeSpellman:  //add Rev25.03/Rev25.02 2017/02/05 by chouno

                    fraFocus.Visible = true;
                    break;
            }
            //Rev23.10 焦点切替無の場合はxfocusは-1にする by長野 2015/11/04
            if (fraFocus.Visible != true)
            {

                //Rev23.10 フォーカスの初期値を0とする by長野 2015/11/04
                //mecainf（コモン）取得
                //GetMecainf(mecainf);
                CTSettings.mecainf.Load();

                //mecainfの焦点を変更
                CTSettings.mecainf.Data.xfocus = -1;

                //mecainf（コモン）更新
                //PutMecainf(mecainf);
                CTSettings.mecainf.Write();
            }

            //焦点ボタンの配置
            switch (modXrayControl.XrayType)
            {
                //追加2014/10/07hata_v19.51反映
                case modXrayControl.XrayTypeConstants.XrayTypeKevex:
                    //Rev20.01 index1からスタートしてしていたので0からに修正 by長野 2015/05/20
                    //cmdFocus[1].Text = CTResources.LoadResString(12138);        //小
                    //cmdFocus[2].Text = CTResources.LoadResString(12137);        //大
                    //cmdFocus[3].Visible = false;
                    //cmdFocus[4].Visible = false;
                    ////2014/11/07hata キャストの修正
                    ////cmdFocus[1].Top = cmdFocus[1].Parent.Height * 1 / 3;      //均等配置のための調整
                    ////cmdFocus[2].Top = cmdFocus[1].Parent.Height * 2 / 3;      //均等配置のための調整
                    //cmdFocus[1].Top = Convert.ToInt32(cmdFocus[1].Parent.Height * 1F / 3F);      //均等配置のための調整
                    //cmdFocus[2].Top = Convert.ToInt32(cmdFocus[1].Parent.Height * 2F / 3F);      //均等配置のための調整
                    cmdFocus[0].Text = CTResources.LoadResString(12138);        //小
                    cmdFocus[1].Text = CTResources.LoadResString(12137);        //大
                    cmdFocus[2].Visible = false;
                    cmdFocus[3].Visible = false;
                    //2014/11/07hata キャストの修正
                    //cmdFocus[1].Top = cmdFocus[1].Parent.Height * 1 / 3;      //均等配置のための調整
                    //cmdFocus[2].Top = cmdFocus[1].Parent.Height * 2 / 3;      //均等配置のための調整
                    cmdFocus[0].Top = Convert.ToInt32(cmdFocus[1].Parent.Height * 1F / 3F);      //均等配置のための調整
                    cmdFocus[1].Top = Convert.ToInt32(cmdFocus[1].Parent.Height * 2F / 3F);      //均等配置のための調整
                    break;

                //追加2014/10/07hata_v19.51反映
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181_02:    //追加2014/10/07hata_v19.51反映
                    //Rev20.01 index1からスタートしてしていたので0からに修正 by長野 2015/05/20
                    //cmdFocus[1].Text = CTResources.LoadResString(12138);        //小
                    //cmdFocus[2].Text = CTResources.LoadResString(12132);        //中
                    ////cmdFocus(3).Caption = LoadResString(12137)      '大    'v14.13削除(応急処置) 07-11-19 byやまおか at現地
                    //cmdFocus[3].Visible = false;                                //大(非表示) 'v14.13追加(応急処置) 07-11-19 byやまおか at現地
                    //cmdFocus[4].Visible = false;
                    ////2014/11/07hata キャストの修正
                    ////cmdFocus[1].Top = cmdFocus[1].Parent.Height * 1 / 3;      //均等配置のための調整   'v14.13変更(応急処置) 分母4→3 07-11-19 byやまおか at現地
                    ////cmdFocus[2].Top = cmdFocus[1].Parent.Height * 2 / 3;      //均等配置のための調整   'v14.13変更(応急処置) 分母4→3 07-11-19 byやまおか at現地
                    //cmdFocus[1].Top = Convert.ToInt32(cmdFocus[1].Parent.Height * 1F / 3F);      //均等配置のための調整   'v14.13変更(応急処置) 分母4→3 07-11-19 byやまおか at現地
                    //cmdFocus[2].Top = Convert.ToInt32(cmdFocus[1].Parent.Height * 2F / 3F);      //均等配置のための調整   'v14.13変更(応急処置) 分母4→3 07-11-19 byやまおか at現地
                    cmdFocus[0].Text = CTResources.LoadResString(12138);        //小
                    cmdFocus[1].Text = CTResources.LoadResString(12132);        //中
                    //cmdFocus(3).Caption = LoadResString(12137)      '大    'v14.13削除(応急処置) 07-11-19 byやまおか at現地
                    cmdFocus[2].Visible = false;                                //大(非表示) 'v14.13追加(応急処置) 07-11-19 byやまおか at現地
                    cmdFocus[3].Visible = false;
                    //2014/11/07hata キャストの修正
                    //cmdFocus[1].Top = cmdFocus[1].Parent.Height * 1 / 3;      //均等配置のための調整   'v14.13変更(応急処置) 分母4→3 07-11-19 byやまおか at現地
                    //cmdFocus[2].Top = cmdFocus[1].Parent.Height * 2 / 3;      //均等配置のための調整   'v14.13変更(応急処置) 分母4→3 07-11-19 byやまおか at現地
                    cmdFocus[0].Top = Convert.ToInt32(cmdFocus[1].Parent.Height * 1F / 3F);      //均等配置のための調整   'v14.13変更(応急処置) 分母4→3 07-11-19 byやまおか at現地
                    cmdFocus[1].Top = Convert.ToInt32(cmdFocus[1].Parent.Height * 2F / 3F);      //均等配置のための調整   'v14.13変更(応急処置) 分母4→3 07-11-19 byやまおか at現地                   
                    //cmdFocus(3).Top = cmdFocus(1).Container.Height * 3 / 4  '均等配置のための調整  'v14.13削除(応急処置) 07-11-19 byやまおか at現地
                    break;

                //追加2014/10/07hata_v19.51反映
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9191:
                    //Rev20.01 index1からスタートしてしていたので0からに修正 by長野 2015/05/20
                    //cmdFocus[1].Text = "F1";
                    //cmdFocus[2].Text = "F2";
                    //cmdFocus[3].Text = "F3";
                    //cmdFocus[4].Text = "F4";
                    cmdFocus[0].Text = "F1";
                    cmdFocus[1].Text = "F2";
                    cmdFocus[2].Text = "F3";
                    cmdFocus[3].Text = "F4";

                   break;

                //浜ホト90kV             追加 by 間々田 2004/10/15
                //Case XrayTypeHamaL9421
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:    //v16.30 02T追加 byやまおか 2010/05/21
                    cmdFocus[0].Text = CTResources.LoadResString(12138);        //小
                    cmdFocus[1].Visible = false;
                    cmdFocus[2].Visible = false;
                    cmdFocus[3].Visible = false;
                    //2014/11/07hata キャストの修正
                    //cmdFocus[0].Top = cmdFocus1.Parent.Height * 1 / 2;          //均等配置のための調整
                    cmdFocus[0].Top = Convert.ToInt32(cmdFocus1.Parent.Height * 1 / 2F);          //均等配置のための調整
                    break;

                //浜ホト150kV           v17.71 追加 by 長野 2012/03/14
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10711:      //Rev23.10 追加 by長野 2015/10/01
                    cmdFocus[0].Text = CTResources.LoadResString(12138);            //小
                    cmdFocus[1].Text = CTResources.LoadResString(12132);            //中
                    cmdFocus[2].Text = CTResources.LoadResString(12137);            //大
                    cmdFocus[3].Visible = false;
                    //2014/11/07hata キャストの修正
                    //cmdFocus[0].Top = cmdFocus[0].Parent.Height * 1 / 4;     //均等配置のための調整
                    //cmdFocus[1].Top = cmdFocus[1].Parent.Height * 2 / 4;     //均等配置のための調整
                    //cmdFocus[2].Top = cmdFocus[2].Parent.Height * 3 / 4;     //均等配置のための調整
                    cmdFocus[0].Top = Convert.ToInt32(cmdFocus[0].Parent.Height * 1F / 4F);     //均等配置のための調整
                    cmdFocus[1].Top = Convert.ToInt32(cmdFocus[1].Parent.Height * 2F / 4F);     //均等配置のための調整
                    cmdFocus[2].Top = Convert.ToInt32(cmdFocus[2].Parent.Height * 3F / 4F);     //均等配置のための調整
                    break;

                case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:
                case modXrayControl.XrayTypeConstants.XrayTypeSpellman:  //add Rev25.03/Rev25.02 2017/02/05 by chouno
                    //Rev20.01 index1からスタートしてしていたので0からに修正 by長野 2015/05/20
                    ////v18.00追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    //cmdFocus[1].Text = modLibrary.RemoveNull(CTSettings.infdef.Data.focustype[0].GetString());
                    ////小
                    //cmdFocus[2].Text = modLibrary.RemoveNull(CTSettings.infdef.Data.focustype[1].GetString());
                    ////大
                    //cmdFocus[1].Visible = (CTSettings.scaninh.Data.xfocus[0] == 0);
                    //cmdFocus[2].Visible = (CTSettings.scaninh.Data.xfocus[1] == 0);
                    //cmdFocus[3].Visible = false;
                    //cmdFocus[4].Visible = false;
                    ////2014/11/07hata キャストの修正
                    ////cmdFocus[1].Top = cmdFocus[1].Parent.Height * 1 / 3;      //均等配置のための調整
                    ////cmdFocus[2].Top = cmdFocus[1].Parent.Height * 2 / 3;      //均等配置のための調整
                    //cmdFocus[1].Top = Convert.ToInt32(cmdFocus[1].Parent.Height * 1F / 3F);      //均等配置のための調整
                    //cmdFocus[2].Top = Convert.ToInt32(cmdFocus[1].Parent.Height * 2F / 3F);      //均等配置のための調整

                    cmdFocus[0].Text = modLibrary.RemoveNull(CTSettings.infdef.Data.focustype[0].GetString());
                    //小
                    cmdFocus[1].Text = modLibrary.RemoveNull(CTSettings.infdef.Data.focustype[1].GetString());
                    //大
                    cmdFocus[0].Visible = (CTSettings.scaninh.Data.xfocus[0] == 0);
                    cmdFocus[1].Visible = (CTSettings.scaninh.Data.xfocus[1] == 0);
                    cmdFocus[2].Visible = false;
                    cmdFocus[3].Visible = false;
                    //2014/11/07hata キャストの修正
                    //cmdFocus[1].Top = cmdFocus[1].Parent.Height * 1 / 3;      //均等配置のための調整
                    //cmdFocus[2].Top = cmdFocus[1].Parent.Height * 2 / 3;      //均等配置のための調整
                    cmdFocus[0].Top = Convert.ToInt32(cmdFocus[1].Parent.Height * 1F / 3F);      //均等配置のための調整
                    cmdFocus[1].Top = Convert.ToInt32(cmdFocus[1].Parent.Height * 2F / 3F);      //均等配置のための調整
                    break;            
            
            }

            //'真空度：浜ホト製160kV,230kVの場合のみ表示
            //fraVac.Visible = (XrayType = XrayTypeHamaL9191)
            fraVac.Visible = (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL9191) ||
                             (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801) ||  //v15.10変更 byやまおか 2009/10/06
                             (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10711) ||  //Rev23.10 by長野 追加 2015/10/01
                             (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL12721);    //v23.10 追加 by長野 2015/10/01


            //v29.99 今のところ不要のため、falseにしておく by長野 2013/04/08'''''ここから'''''
            //真空度：Viscom用   v11.5追加 by 間々田 2006/04/04
            //    stsVacuum.Visible = (XrayType = XrayTypeViscom)
            //    ntbFeedbackVac.Visible = (XrayType = XrayTypeViscom)
            stsVacuum.Visible = false;
            ntbFeedbackVac.Visible = false;
            //v29.99 今のところ不要のため、falseにしておく by長野 2013/04/08'''''ここまで'''''

            //'ターゲット電流：浜ホト製160kVの場合のみ表示
            //ntbTargetCurrent.Visible = (XrayType = XrayTypeHamaL9191)
            //ターゲット電流：浜ホト製160kVと230kVのとき表示 'v15.10変更 byやまおか 2009/10/06 XrayTypeHamaL12721
            ntbTargetCurrent.Visible = (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL9191) ||
                                       (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801) ||
                                       (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10711)|| //Rev23.10 by長野 追加 2015/10/01
                                       (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL12721); //Rev23.10 by長野 追加 2015/10/01

            //ウォームアップ残り時間     浜ホト対応 added by 間々田 2004/03/03
            switch (modXrayControl.XrayType)
            {
                //Rev23.20 追加 by長野 2015/12/24
                case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:
                case modXrayControl.XrayTypeConstants.XrayTypeSpellman:  //add Rev25.03/Rev25.02 2017/02/05 by chouno
                    lblWrestTimeM.AutoSize = true;

                    break;
                //浜ホト130kV,90kV対応
                //Case XrayTypeHamaL9181, XrayTypeHamaL9421   'v9.6 XrayTypeHamaL9421を追加 by 間々田 2004/09/14
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:        //v16.30 02T追加 byやまおか 2010/05/21
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181_02:         //追加2014/10/07hata_v19.51反映

                    //秒は表示しない
                    lblWrestTimeS.Visible = false;
                    Label59.Visible = false;

                    //分の位置を右にずらす
                    //lblWrestTimeM.Left = lblWrestTimeM.Left + 16; //!変更2014/04/18(検S1)hata
                    Label58.Left = Label58.Left + 16;
                    break;

                //'浜ホト160kV,230kV対応                            'v9.5 追加 by 間々田 2004/09/14
                //Case XrayTypeHamaL9191
                //Case XrayTypeHamaL9191, XrayTypeHamaL10801  'v15.10変更 byやまおか 2009/10/06
                //Case XrayTypeHamaL9191, XrayTypeHamaL10801, XrayTypeHamaL8601   'v16.03/v16.20変更 byやまおか 2010/03/03
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9191:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL8601:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02:         //v17.71 追加 by長野 2012/03/14
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10711:           //Rev23.10 追加 by長野 2015/10/01
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL12721:           //Rev23.10 追加 by長野 2015/10/01
                    //残り時間は表示しない
                    Label57.Visible = false;
                    lblWrestTimeM.Visible = false;
                    Label58.Visible = false;
                    lblWrestTimeS.Visible = false;
                    Label59.Visible = false;
                    break;
            }


            //Ｘ線情報ボタン
            switch (modXrayControl.XrayType)
            {
                //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421
                //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL10801    'v15.10変更 byやまおか 2009/10/06
                //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL10801, XrayTypeHamaL8601 'v16.03/v16.20変更 byやまおか 2010/03/03
                //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL10801, XrayTypeHamaL8601, XrayTypeHamaL9421_02T  'v16.30 02T追加 byやまおか 2010/05/21
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9191:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL8601:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02:         //v17.71 追加 by長野 2012/03/14
                case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:              //v17.71 追加 by長野 2012/03/14 'v19.50 GeTitanを追加 by長野 2014/02/03 //追加2014/10/07hata_v19.51反映
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181_02:         //追加2014/10/07hata_v19.51反映
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10711:           //Rev23.10 追加 by長野 2015/10/01
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL12721:           //Rev23.10 追加 by長野 2015/10/01
                case modXrayControl.XrayTypeConstants.XrayTypeSpellman:  //add Rev25.03/Rev25.02 2017/02/05 by chouno
                    cmdXrayInfo.Visible = true;
                    break;
            }


            //詳細ボタン
            switch (modXrayControl.XrayType)
            {
                //Case XrayTypeHamaL9191, XrayTypeViscom, XrayTypeToshibaEXM2_150
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9191:
                case modXrayControl.XrayTypeConstants.XrayTypeViscom:
                case modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:           //v15.10変更 byやまおか 2009/10/06
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL12721:           //Rev23.10 追加 by長野 2015/10/01
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10711:           //Rev23.10 追加 by長野 2015/10/01
                    cmdDetail.Visible = true;
                    break;
            }

                 //ウォームアップ残り時間     浜ホト対応 added by 間々田 2004/03/03
            switch (modXrayControl.XrayType)
            {
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10711:           //Rev23.10 追加 by長野 2015/10/01
                    if (modXrayControl.XrayControl.Up_XrayStatusSMD == 1)
                    {
                        cmdCondition[2].Visible = false;
                    }
                    else
                    {
                        cmdCondition[2].Visible = true;
                    }
                    break;

                default:
                    cmdCondition[2].Visible = true;
                    break;
            }

            //ウォームアップフレーム内
            //cmdWarmupStart.Visible = (XrayType = XrayTypeViscom)       'v15.10削除 byやまおか 2009/10/06
            //cwneWarmupSetVolt.Visible = (XrayType = XrayTypeViscom)    'v15.10削除 byやまおか 2009/10/06
            //Label2.Visible = (XrayType = XrayTypeViscom)               'v15.10削除 byやまおか 2009/10/06

            //ウォームアップの種類   'v16.01/v17.00追加 byやまおか 2010/02/22 //Rev23.10 L12721追加 by長野 2015/10/06　
            if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801 || modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL12721) modXrayControl.WUP_No = 3;      //起動時は3:WUP2

            //ウォームアップ開始ボタン   'v15.10追加 byやまおか 2009/11/02
            //変更2014/10/07hata_v19.51反映
            //cmdWarmupStart.Visible = (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeViscom) ||
            //                         (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801);          //常に表示したい
            cmdWarmupStart.Visible = (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeViscom) |
                                     (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801) |
                                     (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL12721) | //Rev23.10 追加 by長野 2015/10/01
                                     (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan |		//v18.00 Titan追加 byやまおか 2011/03/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                                     (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman));   //Rev25.03/Rev25.02 add by chouno 2017/02/05

            //管電圧設定欄               'v15.10追加 byやまおか 2009/11/02
            cwneWarmupSetVolt.Visible = (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeViscom) ||
                                        (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL12721) || //Rev23.10 追加 by長野 2015/10/01
                                        (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801);

            Label2.Visible = (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeViscom) ||
                             (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL12721) || //Rev23.10 追加 by長野 2015/10/01
                             (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801);

            //v29.99 今のところ不要のため表示しないように変更 by長野 2013/04/08'''''ここから'''''
            //フィラメント調整も行うチェックボックス
            //    chkFilament.Visible = (XrayType = XrayTypeViscom)   'Viscomだけ
            chkFilament.Visible = false;
            //v29.99 今のところ不要のため表示しないように変更 by長野 2013/04/08'''''ここまで'''''


            //段階ウォームアップのチェックボックス   'v17.72/v19.02追加 byやまおか 2012/05/14
            switch (modXrayControl.XrayType)
            {
                case modXrayControl.XrayTypeConstants.XrayTypeViscom:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:       //v19.02変更 浜ホトL10801追加 2012/07/05
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL12721:       //v23.10変更 浜ホトL12721追加 2015/10/01
                    chkStepWU.Visible = true;
                    break;
                default:
                    chkStepWU.Visible = false;
                    break;
            }


            //管電圧コントロールの設定       
            //増減単位 v11.4追加 by 間々田 2006/03/02
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
            .DiscreteInterval = 1
            .IncDecValue = .DiscreteInterval
            .FormatString = GetFormatString(.DiscreteInterval)
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版            
            cwneKV.Increment = 1;
            string cwneKVFormat = modLibrary.GetFormatString((float)cwneKV.Increment);
            int cwneKVIndex = cwneKVFormat.IndexOf(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            cwneKV.DecimalPlaces = ((cwneKVIndex == -1) ? 0 : cwneKVFormat.Substring(cwneKVIndex + 1).Length);

            //管電圧（実測値）にも反映
            ntbActVolt.DiscreteInterval = (float)cwneKV.Increment;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
            'スライダコントロールに反映
            cwsldKV.Axis.DiscreteInterval = .DiscreteInterval
            cwsldKV.Axis.FormatString = .FormatString
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            //最小値・最大値を設定
            cwneKV.Minimum = (decimal)modXrayControl.XrayMinVolt();
            cwneKV.Maximum = (decimal)modXrayControl.XrayMaxVolt();

            //スライダコントロールに反映
            //変更2014/10/07hata
            //cwsldKV.Minimum = (int)(cwneKV.Minimum / cwneKV.Increment);
            //cwsldKV.Maximum = (int)(cwneKV.Maximum / cwneKV.Increment);
            //cwsldKV.LargeChange = (int)(cwsldKV.LargeChange / cwneKV.Increment);
            //cwsldKV.SmallChange = (int)(cwsldKV.SmallChange / cwneKV.Increment);
            //2014/11/07hata キャストの修正
            //cwsldKV.Minimum = (int)cwneKV.Minimum ;
            //cwsldKV.Maximum = (int)cwneKV.Maximum;
            //cwsldKV.LargeChange = (int)cwsldKV.LargeChange;
            //cwsldKV.SmallChange = (int)cwsldKV.SmallChange;
            
            //削除2014/12/22hata_dNet
            //cwsldKV.Minimum = Convert.ToInt32(cwneKV.Minimum);
            //cwsldKV.Maximum = Convert.ToInt32(cwneKV.Maximum);
            
            //Rev23.20 Max,Minの定義でSmallChange,LargeChangeが自動で変わる場合があるため
            //設定順を変える by長野 2015/12/24
            // cwsldKV.LargeChange = Convert.ToInt32(cwsldKV.LargeChange);
            // cwsldKV.SmallChange = Convert.ToInt32(cwsldKV.SmallChange);
            // //追加2014/12/22hata_dNet
            // cwsldKV.Minimum = Convert.ToInt32(cwneKV.Minimum);
            //cwsldKV.Maximum = Convert.ToInt32(cwneKV.Maximum) + cwsldKV.LargeChange - 1;

            int tmpLargeChange = cwsldKV.LargeChange;
            int tmpSmallChange = cwsldKV.SmallChange;

            cwsldKV.Minimum = Convert.ToInt32(cwneKV.Minimum);
        
            cwsldKV.LargeChange = tmpLargeChange;
            cwsldKV.SmallChange = tmpSmallChange;
            cwsldKV.Maximum = Convert.ToInt32(cwneKV.Maximum) + tmpLargeChange - 1;

            //'Viscom用ウォームアップ時の管電圧（起動時は最大管電圧）
            //XrayVoltAtWarmingup = .Maximum
            //v15.11追加(ここから) byやまおか 2010/02/12
            //浜ホト230のときは
            //if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801)
            if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801 || modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL12721)//Rev23.10 追加 by長野 2015/10/01
            {
                //制御器のMAX値を取得する
                modXrayControl.WaitXraySMV_Ready();
                modXrayControl.XrayVoltAtWarmingup = modXrayControl.XrayControl.Up_XrayStatusSMV;
                //レンジを越えないようにする 'v19.02追加 byやまおか 2012/07/21
                //2014/11/07hata キャストの修正
                //modXrayControl.XrayVoltAtWarmingup = ((modXrayControl.XrayVoltAtWarmingup < cwneKV.Maximum) ? modXrayControl.XrayVoltAtWarmingup : (int)cwneKV.Maximum);
                modXrayControl.XrayVoltAtWarmingup = ((modXrayControl.XrayVoltAtWarmingup < cwneKV.Maximum) ? modXrayControl.XrayVoltAtWarmingup : Convert.ToInt32(cwneKV.Maximum));
            }
            //それ以外(Viscom)のときは
            else
            {
                //制御器のMAX値を設定する
                //2014/11/07hata キャストの修正
                //modXrayControl.XrayVoltAtWarmingup = (int)cwneKV.Maximum;
                modXrayControl.XrayVoltAtWarmingup = Convert.ToInt32(cwneKV.Maximum);
            }
            //v15.11追加(ここまで) byやまおか 2010/02/12

            //追加2014/10/07hata_v19.51反映
            //Titanの場合は、制御器からの値をセットする
            //if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan))
            //Rev25.02 change by chouno 2017/02/05
            if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) || (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman))
            {
                //ntbSetVolt.Value = (decimal)modXrayControl.XrayVoltSet();
                //Rev23.20 変更 by長野 2016/01/11
                ntbSetVolt.Value = (decimal)modLibrary.CorrectInRange((decimal)modXrayControl.XrayVoltSet(), cwneKV.Minimum, cwneKV.Maximum);
                modXrayControl.SetVolt((float)ntbSetVolt.Value);
            }

            //変更2014/10/28hata
            ////初期値を設定   'v15.11追加 byやまおか 2010/02/12
            //cwneKV.Value = ntbSetVolt.Value;
            ////スライダーにも反映     'v18.00追加 byやまおか 2011/03/05 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //cwsldKV.Value = (int)cwneKV.Value;         
            //変更2014/12/22hata_dNet
            //if ((cwsldKV.Minimum <= ntbSetVolt.Value) & (cwsldKV.Maximum >= ntbSetVolt.Value))
            if ((cwsldKV.Minimum <= ntbSetVolt.Value) & ((cwsldKV.Maximum + cwsldKV.LargeChange - 1) >= ntbSetVolt.Value))
            {
                //初期値を設定   'v15.11追加 byやまおか 2010/02/12
                cwneKV.Value = ntbSetVolt.Value;
                //スライダーにも反映     'v18.00追加 byやまおか 2011/03/05 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                //2014/11/07hata キャストの修正
                //cwsldKV.Value = (int)cwneKV.Value;
                cwsldKV.Value = Convert.ToInt32(cwneKV.Value);
            }
            else
            {
                //初期値を設定   'v15.11追加 byやまおか 2010/02/12
                cwneKV.Value = cwneKV.Minimum;
                //スライダーにも反映     'v18.00追加 byやまおか 2011/03/05 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                //2014/11/07hata キャストの修正
                //wsldKV.Value = (int)cwneKV.Value;
                cwsldKV.Value = Convert.ToInt32(cwneKV.Value);
            }

            //管電流コントロールの設定   
            //増減単位 v11.4追加 by 間々田 2006/03/02           
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
            .DiscreteInterval = IIf(XrayType = XrayTypeToshibaEXM2_150, 0.01, 1)
            .IncDecValue = .DiscreteInterval
            .FormatString = GetFormatString(.DiscreteInterval)
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版


            //増減単位 v11.4追加 by 間々田 2006/03/02
            //変更2014/10/07hata_v19.51反映
            //cwneMA.Increment = (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150) ? 0.01M : 1;
            switch (modXrayControl.XrayType)
            {
                //v18.00変更 byやまおか 2011/03/01 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                case modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150:
                    cwneMA.Increment = 0.01M;
                    break;
                case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:
                case modXrayControl.XrayTypeConstants.XrayTypeSpellman:  //add Rev25.03/Rev25.02 2017/02/05 by chouno
                    cwneMA.Increment = 0.1M;
                    break;
                default:
                    cwneMA.Increment = 1;
                    break;
            }
 
            string cwneMAFormat = modLibrary.GetFormatString((float)cwneMA.Increment);
            int cwneMAIndex = cwneMAFormat.IndexOf(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            cwneMA.DecimalPlaces = ((cwneMAIndex == -1) ? 0 : cwneMAFormat.Substring(cwneMAIndex + 1).Length);

            //管電流（実測値）にも反映
            ntbActCurrent.DiscreteInterval = (float)cwneMA.Increment;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
            'スライダコントロールに反映
            cwsldMA.Axis.DiscreteInterval = .DiscreteInterval
            cwsldMA.Axis.FormatString = .FormatString
            cwsldMA.IncDecValue = .DiscreteInterval
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版*

            //.Minimum = XrayMinCurrent()                             '最小値を設定
            //.Maximum = XrayMaxCurrent()                             '最大値を設定

            //以下に変更 by 間々田 2007/03/08 こうしないと小数点の末尾に「ごみ」が混入し、
            //意図した最小値・最大値にコントロールを設定できなくなる
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
            .SetMinMax Val(Format$(XrayMinCurrent(), .FormatString)), _
                       Val(Format$(XrayMaxCurrent(), .FormatString))
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
            decimal minValue = (decimal)modXrayControl.XrayMinCurrent();
            decimal maxValue = (decimal)modXrayControl.XrayMaxCurrent();
            decimal.TryParse(minValue.ToString(cwneMAFormat), out minValue);
            decimal.TryParse(maxValue.ToString(cwneMAFormat), out maxValue);
            cwneMA.Minimum = minValue;
            cwneMA.Maximum = maxValue;

            //スライダコントロールに反映
            //変更2014/10/28hata
            //cwsldMA.Minimum = (int)(cwneMA.Minimum / cwneMA.Increment);
            //cwsldMA.Maximum = (int)(cwneMA.Maximum / cwneMA.Increment);
            //cwsldMA.LargeChange = (int)(cwsldMA.LargeChange / cwneMA.Increment);
            //cwsldMA.SmallChange = (int)(cwsldMA.SmallChange / cwneMA.Increment);
            //2014/11/07hata キャストの修正
            //cwsldMA.Minimum = (int)cwneMA.Minimum ;
            //cwsldMA.Maximum = (int)cwneMA.Maximum;
            //cwsldMA.LargeChange = (int)cwsldMA.LargeChange;
            //cwsldMA.SmallChange = (int)cwsldMA.SmallChange;

            //削除2014/12/22hata_dNet
            //cwsldMA.Minimum = Convert.ToInt32(cwneMA.Minimum);
            //cwsldMA.Maximum = Convert.ToInt32(cwneMA.Maximum);
            cwsldMA.LargeChange = Convert.ToInt32(cwsldMA.LargeChange);
            cwsldMA.SmallChange = Convert.ToInt32(cwsldMA.SmallChange);
            //追加2014/12/22hata_dNet
            //cwsldMA.Minimum = Convert.ToInt32(cwneMA.Minimum);
            //Rev23.20 変更 by長野 2015/12/24
            cwsldMA.Minimum = Convert.ToInt32(cwneMA.Minimum / cwneMA.Increment);
            cwsldMA.Maximum = Convert.ToInt32(cwneMA.Maximum / cwneMA.Increment) + cwsldMA.LargeChange - 1;

            ntbSetCurrent.DiscreteInterval = (float)cwneMA.Increment;
            ntbSetCurrent.Unit = modXrayControl.CurrentUni;

            //初期値を設定   'v15.11追加 byやまおか 2010/02/12
            //変更2014/11/25hata
            //変更2014/10/28hata
            //if ((cwneMA.Maximum >= ntbSetVolt.Value) && (cwneMA.Minimum <= ntbSetVolt.Value))
            if ((cwneMA.Maximum >= ntbSetCurrent.Value) && (cwneMA.Minimum <= ntbSetCurrent.Value))
            {
                cwneMA.Value = ntbSetCurrent.Value;
                //追加2014/10/07hata_v19.51反映
                //スライダーにも反映     'v18.00追加 byやまおか 2011/03/05
                //2014/11/07hata キャストの修正
                //cwsldMA.Value = (int)cwneMA.Value;
                //cwsldMA.Value = Convert.ToInt32(cwneMA.Value);
                //Rev23.20 変更 by長野 2015/12/23 by長野
                cwsldMA.Value = Convert.ToInt32(cwneMA.Value / cwneMA.Increment);
            }
            else
            {
                cwneMA.Value = cwneMA.Minimum;
                //追加2014/10/07hata_v19.51反映
                //スライダーにも反映     'v18.00追加 byやまおか 2011/03/05
                //2014/11/07hata キャストの修正
                //cwsldMA.Value = (int)cwneMA.Value;
                //Rev23.20 変更 by長野 2015/12/23 by長野
                //cwsldMA.Value = Convert.ToInt32(cwneMA.Value);
                cwsldMA.Value = Convert.ToInt32(cwneMA.Value / cwneMA.Increment);
            }

            //ウォームアップ専用設定管電圧
            //Rev20.00 max/minが変更→値も変越されるためXrayVoltAtWarmingupのバックアップをもつ
            int bakXVWup = 0;
            bakXVWup = modXrayControl.XrayVoltAtWarmingup;
            cwneWarmupSetVolt.Minimum = cwneKV.Minimum;
            cwneWarmupSetVolt.Maximum = cwneKV.Maximum;
            //変更2015/02/02hata_Max/Min範囲のチェック
            //cwneWarmupSetVolt.Value = modXrayControl.XrayVoltAtWarmingup;
            //Rev20.00 変更 by長野 2015/03/09
            cwneWarmupSetVolt.Value = modLibrary.CorrectInRange(bakXVWup, cwneWarmupSetVolt.Minimum, cwneWarmupSetVolt.Maximum);
            modXrayControl.XrayVoltAtWarmingup = bakXVWup;
            
            //表示位置の調整
            ntbActVolt.Left = cwneKV.Left;
            ntbActCurrent.Left = cwneMA.Left;

            ////推奨フィルター厚
            //v19.50 起動時のイベントで必ず値が入るので不要 by長野 2013/11/13
            //ntbFilter.Clear();              //空欄にする
        }


        //*************************************************************************************************
        //機　　能： 更新処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************
        //private void MyUpdate()
        public void MyUpdate()
        {
    #region　//全面変更2014/10/07hata_v19.51反映
           //全面変更2014/10/07hata_v19.51反映
     //######################################################################################################################       
            ////'デバッグ用
            ////If frmTransImage.FrameRate <> 30 Then

            ////設定管電圧・設定管電流     'v11.5変更 by 間々田 2006/07/10
            //ntbSetVolt.Value = (decimal)modXrayControl.XrayVoltSet();
            //Application.DoEvents();                 //v17.701/v19.02追加 byやまおか 2012/03/28
            //System.Threading.Thread.Sleep(5);       //v17.72/v19.02追加 byやまおか 2012/05/16

            //ntbSetCurrent.Value = (decimal)modXrayControl.XrayCurrentSet();

            ////v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            ////If IsDrvReady Then      '追加 by 間々田 2009/07/31
            ////If IsDrvReady And (XrayType = XrayTypeViscom) Then    'v15.10変更 byやまおか 2009/11/12
            ////    If (IsDrvReady And (XrayType = XrayTypeViscom)) Or (XrayType = XrayTypeToshibaEXM2_150) Then    'v16.11変更 byやまおか 2010/03/29
            ////
            ////        '設定管電圧コントロール：操作していない時のみ更新
            ////        'If (Not (Me.ActiveControl Is cwsldKV)) And cwsldKV.Enabled Then
            ////        'If (Not (Me.ActiveControl Is cwsldKV)) And (Not (Me.ActiveControl Is cwneKV)) And IsDrvReady Then
            ////        'If (Not (Me.ActiveControl Is cwsldKV)) And (Not (Me.ActiveControl Is cwneKV)) Then '変更 by 間々田 2009/07/31
            ////        If (Not (Me.ActiveControl Is cwsldKV)) And (Not (Me.ActiveControl Is cwneKV)) And (cwsldKV.Enabled) And (cwneKV.Enabled) Then 'v16.20変更 byやまおか 2010/04/06
            ////            cwsldKV.Value = ntbSetVolt.Value
            ////
            ////            'cwneKV.Value にも値をセットする    v17.50追加 cwneKV.Valueに値が反映されていない場合があるので by 間々田 2011/03/02
            ////            If cwsldKV.Value <> cwneKV.Value Then
            ////                cwneKV.Value = cwsldKV.Value
            ////            End If
            ////
            ////        End If
            ////
            ////        '設定管電流コントロール：操作していない時のみ更新
            ////        'If (Not (Me.ActiveControl Is cwsldMA)) And cwsldMA.Enabled Then
            ////        'If (Not (Me.ActiveControl Is cwsldMA)) And (Not (Me.ActiveControl Is cwneMA)) And IsDrvReady Then
            ////        'If (Not (Me.ActiveControl Is cwsldMA)) And (Not (Me.ActiveControl Is cwneMA)) Then '変更 by 間々田 2009/07/31
            ////        If (Not (Me.ActiveControl Is cwsldMA)) And (Not (Me.ActiveControl Is cwneMA)) And (cwsldMA.Enabled) And (cwneMA.Enabled) Then 'v16.20変更 byやまおか 2010/04/06
            ////            cwsldMA.Value = ntbSetCurrent.Value
            ////
            ////            'cwneMA.Value にも値をセットする    v17.50追加 cwneMA.Valueに値が反映されていない場合があるので by 間々田 2011/03/02
            ////            If cwsldMA.Value <> cwneMA.Value Then
            ////                cwneMA.Value = cwsldMA.Value
            ////            End If
            ////
            ////        End If
            ////
            ////        'If IsDrvReady Then
            ////        '   cwsldKV.Enabled = True
            ////        '   cwsldMA.Enabled = True
            ////        '   cwsldKV.ActivePointer.FILLCOLOR = vbHighlight
            ////        '   cwsldMA.ActivePointer.FILLCOLOR = vbHighlight
            ////        'End If
            ////
            ////        '以下に変更 by 間々田 2009/07/31
            ////        If Not cwsldKV.Enabled Then
            ////            cwsldKV.Enabled = True
            ////            cwsldKV.ActivePointer.FILLCOLOR = vbHighlight
            ////        End If
            ////        If Not cwneKV.Enabled Then  'v15.03追加 byやまおか 2009/11/17
            ////            cwneKV.Enabled = True
            ////        End If
            ////
            ////        If Not cwsldMA.Enabled Then
            ////            cwsldMA.Enabled = True
            ////            cwsldMA.ActivePointer.FILLCOLOR = vbHighlight
            ////        End If
            ////        If Not cwneMA.Enabled Then  'v15.03追加 byやまおか 2009/11/17
            ////            cwneMA.Enabled = True
            ////        End If
            ////
            ////    End If
            ////v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''          
            ////End If

            ////管電圧／管電流（実測値）
            //switch (modXrayControl.XrayType)
            //{
            //    //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeToshibaEXM2_150, XrayTypeViscom
            //    //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeToshibaEXM2_150, XrayTypeViscom, XrayTypeHamaL10801   'v15.10変更 byやまおか 2009/10/07
            //    //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeToshibaEXM2_150, XrayTypeViscom, XrayTypeHamaL10801, XrayTypeHamaL8601    'v16.03/v16.20変更 byやまおか 2010/03/03
            //    //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeToshibaEXM2_150, XrayTypeViscom, XrayTypeHamaL10801, XrayTypeHamaL8601, XrayTypeHamaL9421_02T 'v16.30 02T追加 byやまおか 2010/05/21
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9191:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
            //    case modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150:
            //    case modXrayControl.XrayTypeConstants.XrayTypeViscom:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL8601:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02:     //v17.71 追加 by長野 2012/03/14
            //        ntbActVolt.Value = (decimal)modXrayControl.XrayVoltFeedback();          //管電圧（実測値）
            //        ntbActCurrent.Value = (decimal)modXrayControl.XrayCurrentFeedback();    //管電流（実測値）
            //        break;
            //}

            ////Ｘ線オン・オフ状態
            //MecaXrayOn = (modXrayControl.IsXrayOn() ? modCT30K.OnOffStatusConstants.OnStatus : modCT30K.OnOffStatusConstants.OffStatus);

            //if ((myXrayOn == modCT30K.OnOffStatusConstants.OffStatus) && XrayOffByTimer)
            //{
            //    //メッセージ表示：タイマーで設定した時間が経過したのでＸ線をＯＦＦしました。
            //    MessageBox.Show(CTResources.LoadResString(16107), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    XrayOffByTimer = false;
            //}

            ////Ｘ線アベイラブル
            //MecaXrayAvailable = (IsXrayAvailable() ? modCT30K.OnOffStatusConstants.OnStatus : modCT30K.OnOffStatusConstants.OffStatus);


            ////'デバッグ用
            ////If frmTransImage.FrameRate > 15 Then Exit Sub

            ////Ｘ線のウォームアップ状態を取得
            //myXrayWarmUp = modXrayControl.XrayWarmUp();

            ////ウォームアップ情報（焦点もここで表示）
            //if (modXrayControl.XrayType != modXrayControl.XrayTypeConstants.XrayTypeFeinFocus)
            //{
            //    UpdateWarmUp();
            //}

            ////浜ホト製160kV,230kVの場合
            ////If XrayType = XrayTypeHamaL9191 Then
            //if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL9191) ||
            //    (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801))   //v15.10変更 byやまおか 2009/10/07
            //{
            //    //真空度
            //    UpdateVacuumSVC(modXrayControl.XrayControl.Up_XrayVacuumSVC);

            //    //ターゲット電流（小数点以下１桁表示）
            //    ntbTargetCurrent.Value = (decimal)modXrayControl.XrayControl.Up_XrayTargetInfSTG;
                
            //    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //    //Viscom製の場合
            //    //    ElseIf XrayType = XrayTypeViscom Then
            //    //
            //    //        '真空ポンプのステータス：準備完了/準備未完
            //    //'        lblVacuumState.Caption = IIf(ViscomState1 And XST1_VAC_OK, GC_STS_STANDBY_OK, GC_STS_STANDBY_NG2)
            //    //        stsVacuum.Status = IIf(ViscomState1 And XST1_VAC_OK, GC_STS_STANDBY_OK, GC_STS_STANDBY_NG2)
            //    //
            //    //        '真空度
            //    //        ntbFeedbackVac.Value = GetFeedbackVac()
            //    //
            //    //        'コントロールの使用可・不可を制御する
            //    //        'cmdXrayOnOff(1).Enabled = (Not CBool(ViscomState1 And (XST1_DRV_BUSY Or XST1_WUP_RUNNING Or XST1_FLM_RUNNING Or XST1_CNT_RUNNING))) And (Not XrayCenteringManual) 'Ｘ線オンボタン
            //    //        frmCTMenu.Toolbar1.Buttons("Xray").Enabled = (Not CBool(ViscomState1 And (XST1_DRV_BUSY Or XST1_WUP_RUNNING Or XST1_FLM_RUNNING Or XST1_CNT_RUNNING))) And (Not XrayCenteringManual) 'Ｘ線オンボタン
            //    //        cwsldKV.Enabled = (Not CBool(ViscomState1 And (XST1_DRV_BUSY Or XST1_CNT_RUNNING))) And (Not XrayCenteringManual)
            //    //        cwsldMA.Enabled = (Not CBool(ViscomState1 And (XST1_DRV_BUSY Or XST1_CNT_RUNNING))) And (Not XrayCenteringManual)
            //    //        cwneKV.Enabled = cwsldKV.Enabled
            //    //        cwneMA.Enabled = cwsldMA.Enabled
            //    //
            //    //        'アライメント調整画面の更新
            //    //        'frmViscomAlignment.Update      '削除 by 間々田 2009/08/01 以下に変更
            //    //
            //    //        'フィラメントステータス
            //    //        If ViscomState1 And XST1_FLM_FAILED Then
            //    //            FilamentAdjustStatus = LoadResString(IDS_AlignmentFailure)  '調整失敗
            //    //        ElseIf ViscomState1 And XST1_FLM_READY Then
            //    //            FilamentAdjustStatus = GC_STS_STANDBY_OK                    '準備完了
            //    //        ElseIf ViscomState1 And XST1_FLM_RUNNING Then
            //    //            FilamentAdjustStatus = LoadResString(IDS_AlignmentNow)      '調整中
            //    //        Else
            //    //            FilamentAdjustStatus = GC_STS_STANDBY_NG2                   '準備未完
            //    //        End If
            //    //
            //    //        'ウォームアップが開始できるか
            //    //        IsWarmupAvailable = (Not CBool(ViscomState1 And (XST1_DRV_BUSY Or XST1_WUP_RUNNING Or XST1_FLM_RUNNING Or XST1_CNT_RUNNING))) And (Not XrayCenteringManual)
            //    //
            //    //        'ウォームアップ開始ボタン
            //    //        With cmdWarmupStart
            //    //            '開始ボタン
            //    //            If .Caption = LoadResString(IDS_btnStart) Then
            //    //                .Enabled = IsWarmupAvailable
            //    //            '中止ボタン
            //    //            Else
            //    //                .Enabled = (Not CBool(ViscomState1 And (XST1_FLM_RUNNING Or XST1_CNT_RUNNING))) And (Not XrayCenteringManual)
            //    //            End If
            //    //        End With
            //    //
            //    //        'X線ステータスをログファイルに書き込む   'v19.02追加 byやまおか 2012/07/20
            //    //        XrayStatusLogOut_withoutOpen
            //    //
            //    //        '更新されたことをイベント通知
            //    //        RaiseEvent UpdateByTimer
            //    //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            //}

            ////v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            ////If Not FirstDone Then
            ////    If (Not SecondDetOn) Then    '検出器切替ありの場合はここでウォームアップを促さない   'v17.51変更 by 間々田 2011/03/23
            ////
            ////        'Select Case XrayType
            ////        '
            ////        '    Dim VoltAtWarmup As Single  'v15.11追加 byやまおか 2010/02/12
            ////        '
            ////        '    'Ｘ線のタイプがFEINFOCUSの場合
            ////        '    Case XrayTypeFeinFocus
            ////        '
            ////        '        'メッセージ表示：Ｘ線制御器にてウォームアップを実施してください。
            ////        '        MsgBox LoadResString(9378), vbInformation
            ////        '
            ////        '    'Ｘ線のタイプがViscomの場合
            ////        '    Case XrayTypeViscom
            ////        '
            ////        '        'ウォームアップが未完了の時メッセージを表示
            ////        '        If myXrayWarmUp <> XrayWarmUpComplete Then
            ////        '            'ウォームアップ要求ダイアログ
            ////        '            'Dim VoltAtWarmup As Single     'v15.11上へ移動 byやまおか 2010/02/12
            ////        '            VoltAtWarmup = XrayVoltAtWarmingup
            ////        '            If frmQueryWarmup.Dialog(VoltAtWarmup) Then
            ////        '                cwneWarmupSetVolt.Value = VoltAtWarmup
            ////        '                cmdWarmupStart_Click
            ////        '            End If
            ////        '        End If
            ////        '
            ////        '    'Ｘ線のタイプが浜ホト230の場合
            ////        '    Case XrayTypeHamaL10801
            ////        '
            ////        '        'ウォームアップが未完了の時メッセージを表示
            ////        '        If myXrayWarmUp <> XrayWarmUpComplete Then
            ////        '            'ウォームアップ要求ダイアログ
            ////        '            VoltAtWarmup = cwneKV.Maximum
            ////        '            If frmQueryWarmup.Dialog(VoltAtWarmup) Then
            ////        '                cwneWarmupSetVolt.Value = VoltAtWarmup
            ////        '                cmdWarmupStart_Click
            ////        '            End If
            ////        '        End If
            ////        '
            ////        '    'Ｘ線のタイプがKEVEX、浜ホトの場合
            ////        '    Case Else
            ////        '
            ////        '        'ウォームアップが未完了の時メッセージを表示
            ////        '        If myXrayWarmUp <> XrayWarmUpComplete Then
            ////        '            'メッセージ表示：ウォームアップを実施してください。
            ////        '            MsgBox LoadResString(9443), vbInformation
            ////        '        End If
            ////        '
            ////        'End Select
            ////        '
            ////        'FirstDone = True
            ////
            ////        'v17.51上記の処理を関数化 by 間々田 2011/03/23
            ////        QueryWarmup
            ////
            ////    End If
            ////v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            //全面変更2014/10/07hata_v19.51反映
            //######################################################################################################################       
            #endregion 全面変更2014/10/07hata_v19.51反映

            //'デバッグ用
            //If frmTransImage.FrameRate <> 30 Then

            //設定管電圧・設定管電流     'v11.5変更 by 間々田 2006/07/10
            //ntbSetVolt.Value = (decimal)modXrayControl.XrayVoltSet();
            //Rev23.20 変更 by長野 2016/01/11
            ntbSetVolt.Value = (decimal)modLibrary.CorrectInRange((decimal)modXrayControl.XrayVoltSet(), ntbSetVolt.Min, ntbSetVolt.Max);

            Application.DoEvents();                 //v17.701/v19.02追加 byやまおか 2012/03/28
            System.Threading.Thread.Sleep(5);       //v17.72/v19.02追加 byやまおか 2012/05/16

            ntbSetCurrent.Value = (decimal)modXrayControl.XrayCurrentSet();


            //If IsDrvReady Then      '追加 by 間々田 2009/07/31
            //If IsDrvReady And (XrayType = XrayTypeViscom) Then    'v15.10変更 byやまおか 2009/11/12
            //v16.11変更 byやまおか 2010/03/29
            if ((modXrayControl.IsDrvReady() & 
                (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeViscom)) | 
                (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150))
            {

                //設定管電圧コントロール：操作していない時のみ更新
                //If (Not (Me.ActiveControl Is cwsldKV)) And cwsldKV.Enabled Then
                //If (Not (Me.ActiveControl Is cwsldKV)) And (Not (Me.ActiveControl Is cwneKV)) And IsDrvReady Then
                //If (Not (Me.ActiveControl Is cwsldKV)) And (Not (Me.ActiveControl Is cwneKV)) Then '変更 by 間々田 2009/07/31
                //v16.20変更 byやまおか 2010/04/06
                if (((!object.ReferenceEquals(this.ActiveControl, cwsldKV))) & ((!object.ReferenceEquals(this.ActiveControl, cwneKV))) & (cwsldKV.Enabled) & (cwneKV.Enabled))
                {
                    //2014/11/07hata キャストの修正
                    //cwsldKV.Value = (int)ntbSetVolt.Value;
                    //変更2015/02/02hata_Max/Min範囲のチェック
                    //cwsldKV.Value = Convert.ToInt32(ntbSetVolt.Value);
                    cwsldKV.Value = modLibrary.CorrectInRange(Convert.ToInt32(ntbSetVolt.Value), cwsldKV.Minimum, cwsldKV.Maximum);
                    
                    //cwneKV.Value にも値をセットする    v17.50追加 cwneKV.Valueに値が反映されていない場合があるので by 間々田 2011/03/02
                    if (cwsldKV.Value != cwneKV.Value)
                    {
                        cwneKV.Value = cwsldKV.Value;
                    }
                }

                //設定管電流コントロール：操作していない時のみ更新
                //If (Not (Me.ActiveControl Is cwsldMA)) And cwsldMA.Enabled Then
                //If (Not (Me.ActiveControl Is cwsldMA)) And (Not (Me.ActiveControl Is cwneMA)) And IsDrvReady Then
                //If (Not (Me.ActiveControl Is cwsldMA)) And (Not (Me.ActiveControl Is cwneMA)) Then '変更 by 間々田 2009/07/31
                //v16.20変更 byやまおか 2010/04/06
                if (((!object.ReferenceEquals(this.ActiveControl, cwsldMA))) & ((!object.ReferenceEquals(this.ActiveControl, cwneMA))) & (cwsldMA.Enabled) & (cwneMA.Enabled))
                {
                    //2014/11/07hata キャストの修正
                    //cwsldMA.Value = (int)ntbSetCurrent.Value;
                    //変更2015/02/02hata_Max/Min範囲のチェック
                    //cwsldMA.Value = Convert.ToInt32(ntbSetCurrent.Value);
                    cwsldMA.Value = modLibrary.CorrectInRange(Convert.ToInt32(ntbSetCurrent.Value), cwsldMA.Minimum, cwsldMA.Maximum);

                    //cwneMA.Value にも値をセットする    v17.50追加 cwneMA.Valueに値が反映されていない場合があるので by 間々田 2011/03/02
                    if (cwsldMA.Value != cwneMA.Value)
                    {
                        cwneMA.Value = cwsldMA.Value;
                    }
                }

                //If IsDrvReady Then
                //   cwsldKV.Enabled = True
                //   cwsldMA.Enabled = True
                //   cwsldKV.ActivePointer.FILLCOLOR = vbHighlight
                //   cwsldMA.ActivePointer.FILLCOLOR = vbHighlight
                //End If

                //以下に変更 by 間々田 2009/07/31
                if (!cwsldKV.Enabled)
                {
                    cwsldKV.Enabled = true;
                    #region Hscrollbarに該当する設定がないためコメントにする。
                    //Hscrollbarに該当する設定がないためコメントにする。
                    //cwsldKV.ActivePointer.FillColor = System.Convert.ToUInt32(System.Drawing.ColorTranslator.ToOle(System.Drawing.SystemColors.Highlight));
                    #endregion
                }
                //v15.03追加 byやまおか 2009/11/17
                if (!cwneKV.Enabled)
                {
                    cwneKV.Enabled = true;
                }

                if (!cwsldMA.Enabled)
                {
                    cwsldMA.Enabled = true;
                    #region Hscrollbarに該当する設定がないためコメントにする。
                    //Hscrollbarに該当する設定がないためコメントにする。
                    //cwsldMA.bActivePointer.FillColor = System.Convert.ToUInt32(System.Drawing.ColorTranslator.ToOle(System.Drawing.SystemColors.Highlight));
                    #endregion
                }
                //v15.03追加 byやまおか 2009/11/17
                if (!cwneMA.Enabled)
                {
                    cwneMA.Enabled = true;
                }

            }

            //End If

            //v18.00追加 byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) & (myXrayStatus != StringTable.GC_Xray_WarmUp))
            //Rev25.03/Rev25.02 change by chouno 2017/02/05
            if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan || modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman) & (myXrayStatus != StringTable.GC_Xray_WarmUp))
            {

                //Titanの場合も管電流だけは実際の設定値に更新する
                //設定管電流コントロール：スライダを操作しているときは更新しない
                //v18.00変更 byやまおか 2011/03/26
                if (((!object.ReferenceEquals(this.ActiveControl, cwsldMA))) & (cwsldMA.Enabled) & (cwneMA.Enabled))
                {
                    //2014/11/07hata キャストの修正
                    //cwsldMA.Value = (int)ntbSetCurrent.Value;
                    //cwsldMA.Value = Convert.ToInt32(ntbSetCurrent.Value);
                    //Rev23.20 変更 by長野 2015/12/23
                    //cwsldMA.Value = Convert.ToInt32(ntbSetCurrent.Value / cwneMA.Increment);
                    //Rev23.40 by長野 2016/06/19
                    int tmpVal = (Convert.ToInt32(ntbSetCurrent.Value / cwneMA.Increment));
                    if (cwsldMA.Minimum > tmpVal)
                    {
                        cwsldMA.Value = cwsldMA.Minimum;
                    }
                    else if (cwsldMA.Maximum < tmpVal)
                    {
                        cwsldMA.Value = cwsldMA.Maximum;
                    }
                    else
                    {
                        cwsldMA.Value = tmpVal;
                    }
                }

                if (!cwsldKV.Enabled)
                {
                    cwsldKV.Enabled = true;
                    #region Hscrollbarに該当する設定がないためコメントにする。
                    //Hscrollbarに該当する設定がないためコメントにする。
                    //cwsldKV.ActivePointer.FillColor = System.Convert.ToUInt32(System.Drawing.ColorTranslator.ToOle(System.Drawing.SystemColors.Highlight));
                    #endregion
                }
                if (!cwneKV.Enabled)
                {
                    cwneKV.Enabled = true;
                }

                if (!cwsldMA.Enabled)
                {
                    cwsldMA.Enabled = true;
                    #region Hscrollbarに該当する設定がないためコメントにする。
                    //Hscrollbarに該当する設定がないためコメントにする。
                    //cwsldMA.ActivePointer.FillColor = System.Convert.ToUInt32(System.Drawing.ColorTranslator.ToOle(System.Drawing.SystemColors.Highlight));
                    #endregion
                }
                if (!cwneMA.Enabled)
                {
                    cwneMA.Enabled = true;
                }

            }

            //管電圧／管電流（実測値）
            switch (modXrayControl.XrayType)
            {
                //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeToshibaEXM2_150, XrayTypeViscom
                //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeToshibaEXM2_150, XrayTypeViscom, XrayTypeHamaL10801   'v15.10変更 byやまおか 2009/10/07
                //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeToshibaEXM2_150, XrayTypeViscom, XrayTypeHamaL10801, XrayTypeHamaL8601    'v16.03/v16.20変更 byやまおか 2010/03/03
                //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeToshibaEXM2_150, XrayTypeViscom, XrayTypeHamaL10801, XrayTypeHamaL8601, XrayTypeHamaL9421_02T 'v16.30 02T追加 byやまおか 2010/05/21
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9191:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
                case modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150:
                case modXrayControl.XrayTypeConstants.XrayTypeViscom:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL8601:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02:
                case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181_02: //追加2014/10/07hata_v19.51反映
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL12721:   //Rev23.10 追加 by長野 2015/10/01
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10711:   //Rev23.10 追加 by長野 2015/10/01
                case modXrayControl.XrayTypeConstants.XrayTypeSpellman:     //add Rev25.03/Rev25.02 2017/02/05 by chouno
                    //v17.71 追加 by長野 2012/03/14 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    ntbActVolt.Value = (decimal)modXrayControl. XrayVoltFeedback();                    //管電圧（実測値）
                    ntbActCurrent.Value =  (decimal)modXrayControl.XrayCurrentFeedback();                    //管電流（実測値）
                    break;
            }

            //Ｘ線オン・オフ状態
            MecaXrayOn = (modXrayControl.IsXrayOn() ? modCT30K.OnOffStatusConstants.OnStatus : modCT30K.OnOffStatusConstants.OffStatus);

            if ((myXrayOn == modCT30K.OnOffStatusConstants.OffStatus) & XrayOffByTimer)
            {

                //メッセージ表示：タイマーで設定した時間が経過したのでＸ線をＯＦＦしました。
                MessageBox.Show(CTResources.LoadResString(16107), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                XrayOffByTimer = false;

            }

            //Ｘ線アベイラブル
            MecaXrayAvailable = (IsXrayAvailable() ? modCT30K.OnOffStatusConstants.OnStatus : modCT30K.OnOffStatusConstants.OffStatus);

            //スキャン条件のプリセット設定を無効化(X線ON中は管電圧/管電流を変えさせないため) 'v18.00追加 byやまおか 2011/07/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            var _with6 = frmScanControl.Instance;
            //_with6.cmdPresetRef.Enabled = Convert.ToBoolean(_with6.chkUserCondition.Enabled | _with6.chkImageCondition.Enabled) & Convert.ToBoolean(_with6.chkUserCondition.CheckState | _with6.chkImageCondition.CheckState);
            //Rev26.00 change by chouno 2017/08/31
            _with6.cmdPresetRef.Enabled = Convert.ToBoolean(myXrayOn == modCT30K.OnOffStatusConstants.OffStatus);
            
            //ウォームアップ時間を計算して表示する   'v18.00追加 byやまおか 2011/03/26
            if ((myXrayOn != modCT30K.OnOffStatusConstants.OnStatus))
            {
                //Rev23.20 未操作時のみ実行 by長野 2016/01/12
                if (((!object.ReferenceEquals(this.ActiveControl, cwsldKV))) & ((!object.ReferenceEquals(this.ActiveControl, cwneKV))) & (cwsldKV.Enabled) & (cwneKV.Enabled))
                {
                    modTitan.Ti_GetWarmupRestSec(Convert.ToInt32(cwneKV.Value));
                }
            }


            //'デバッグ用
            //If frmTransImage.FrameRate > 15 Then Exit Sub

            //Rev23.20 未操作時のみ実行 by長野 2016/01/12
            if (((!object.ReferenceEquals(this.ActiveControl, cwsldKV))) & ((!object.ReferenceEquals(this.ActiveControl, cwneKV))) & (cwsldKV.Enabled) & (cwneKV.Enabled))
            {
                //Ｘ線のウォームアップ状態を取得
                myXrayWarmUp = modXrayControl.XrayWarmUp();
            }

            //ウォームアップ情報（焦点もここで表示）
            if (modXrayControl.XrayType != modXrayControl.XrayTypeConstants.XrayTypeFeinFocus)
            {
                UpdateWarmUp();
            }

            //浜ホト製160kV,230kVの場合
            //If XrayType = XrayTypeHamaL9191 Then
            //v15.10変更 byやまおか 2009/10/07
            if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL9191) | 
                //(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801))
                (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801)|
                (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10711)| //Rev23.10 追加 by長野 2015/10/01
                (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL12721))//Rev23.10 追加 by長野 2015/10/01
            {

                //真空度
                UpdateVacuumSVC(modXrayControl.XrayControl.Up_XrayVacuumSVC);

                //ターゲット電流（小数点以下１桁表示）
                ntbTargetCurrent.Value = (decimal)modXrayControl.XrayControl.Up_XrayTargetInfSTG;

            }

            #region Viscom製の場合  v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''

            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //Viscom製の場合
            //else if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeViscom)
            //{

            //    //真空ポンプのステータス：準備完了/準備未完
            //    //        lblVacuumState.Caption = IIf(ViscomState1 And XST1_VAC_OK, GC_STS_STANDBY_OK, GC_STS_STANDBY_NG2)
            //    stsVacuum.Status = (ViscomState1 & XST1_VAC_OK ? GC_STS_STANDBY_OK : GC_STS_STANDBY_NG2);

            //    //真空度
            //    ntbFeedbackVac.Value = GetFeedbackVac();

            //    //コントロールの使用可・不可を制御する
            //    //cmdXrayOnOff(1).Enabled = (Not CBool(ViscomState1 And (XST1_DRV_BUSY Or XST1_WUP_RUNNING Or XST1_FLM_RUNNING Or XST1_CNT_RUNNING))) And (Not XrayCenteringManual) 'Ｘ線オンボタン
            //    frmCTMenu.Toolbar1.Items["Xray"].Enabled = (!Convert.ToBoolean(ViscomState1 & (XST1_DRV_BUSY | XST1_WUP_RUNNING | XST1_FLM_RUNNING | XST1_CNT_RUNNING))) & (!XrayCenteringManual);
            //    //Ｘ線オンボタン
            //    cwsldKV.Enabled = (!Convert.ToBoolean(ViscomState1 & (XST1_DRV_BUSY | XST1_CNT_RUNNING))) & (!XrayCenteringManual);
            //    cwsldMA.Enabled = (!Convert.ToBoolean(ViscomState1 & (XST1_DRV_BUSY | XST1_CNT_RUNNING))) & (!XrayCenteringManual);
            //    cwneKV.Enabled = cwsldKV.Enabled;
            //    cwneMA.Enabled = cwsldMA.Enabled;

            //    //アライメント調整画面の更新
            //    //frmViscomAlignment.Update      '削除 by 間々田 2009/08/01 以下に変更

            //    //フィラメントステータス
            //    if (ViscomState1 & XST1_FLM_FAILED)
            //    {
            //        FilamentAdjustStatus = CT30K.My.Resources.ResourceManager.GetString("str" + Convert.ToString(IDS_AlignmentFailure));
            //        //調整失敗
            //    }
            //    else if (ViscomState1 & XST1_FLM_READY)
            //    {
            //        FilamentAdjustStatus = GC_STS_STANDBY_OK;
            //        //準備完了
            //    }
            //    else if (ViscomState1 & XST1_FLM_RUNNING)
            //    {
            //        FilamentAdjustStatus = CT30K.My.Resources.ResourceManager.GetString("str" + Convert.ToString(IDS_AlignmentNow));
            //        //調整中
            //    }
            //    else
            //    {
            //        FilamentAdjustStatus = GC_STS_STANDBY_NG2;
            //        //準備未完
            //    }

            //    //ウォームアップが開始できるか
            //    IsWarmupAvailable = (!Convert.ToBoolean(ViscomState1 & (XST1_DRV_BUSY | XST1_WUP_RUNNING | XST1_FLM_RUNNING | XST1_CNT_RUNNING))) & (!XrayCenteringManual);

            //    //ウォームアップ開始ボタン
            //    var _with7 = cmdWarmupStart;
            //    //開始ボタン
            //    if (_with7.Text == CT30K.My.Resources.ResourceManager.GetString("str" + Convert.ToString(IDS_btnStart)))
            //    {
            //        _with7.Enabled = IsWarmupAvailable;
            //        //中止ボタン
            //    }
            //    else
            //    {
            //        _with7.Enabled = (!Convert.ToBoolean(ViscomState1 & (XST1_FLM_RUNNING | XST1_CNT_RUNNING))) & (!XrayCenteringManual);
            //    }

            //    //X線ステータスをログファイルに書き込む   'v19.02追加 byやまおか 2012/07/20
            //    XrayStatusLogOut_withoutOpen();

            //    //更新されたことをイベント通知
            //    if (UpdateByTimer != null)
            //    {
            //        UpdateByTimer();
            //    }
            //    //End If
            // }
            //    //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            #endregion //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //Titanの場合        'v18.00追加 byやまおか 2011/03/14 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //else if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan))
            //Rev25.03/Rev25.02 change by chouno 2017/02/05
            else if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) || (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman))
            {
                //焦点：X線OFFの時だけ焦点ボタンを使用可にする
                //fraFocus.Enabled = (myXrayOn == modCT30K.OnOffStatusConstants.OffStatus);
                //Rev23.20 変更 by長野 2015/12/23
                if(fraFocus.Enabled != (myXrayOn == modCT30K.OnOffStatusConstants.OffStatus))
                {
                    if(fraFocus.Enabled == false)
                    {
                        fraFocus.Enabled = true;
                    }
                }

                //SetCmdButton cmdFocus, scansel.xfocus + 1, , , True
                modTitan.Get_Ti_GetFocusSize();
                //v18.00変更 byやまおか 2011/07/09

                //フィルター厚を表示 'v18.00追加 byやまおか 2011/03/15
                int iPos = modCT30K.GetFilterStr(modSeqComm.GetFilterIndex()).IndexOf("mm");
                //Rev23.20 追加 by長野 2015/12/17
                string strFilter;
                if (iPos < 0)
                {
                    strFilter = modCT30K.GetFilterStr(modSeqComm.GetFilterIndex());
                }
                else
                {
                    strFilter = modCT30K.GetFilterStr(modSeqComm.GetFilterIndex()).Remove(iPos); 
                }
                
                decimal IsNumeric = (decimal)0.0;
                if(decimal.TryParse(strFilter, out IsNumeric) == true)
                {
                    ntbFilter.Value = Convert.ToDecimal(strFilter);
                }
                else
                {
                    ntbFilter.Text = strFilter;
                }

                //ウォームアップが開始できるか   'v18.00追加 byやまおか 2011/09/02
                modXrayControl.IsWarmupAvailable = Convert.ToBoolean((myXrayWarmUp == modXrayControl.XrayWarmUpConstants.XrayWarmUpNotComplete) & !(myXrayOn == modCT30K.OnOffStatusConstants.OnStatus));

                //ウォームアップ開始ボタン(ウォームアップ中は停止ボタン)を有効にする
                cmdWarmupStart.Enabled = modXrayControl.IsWarmupAvailable | (myXrayWarmUp == modXrayControl.XrayWarmUpConstants.XrayWarmUpNow);

            }

            //    'v19.50 ここだと初回にしか動かないため tmrUpDateに移動 by長野 2013/11/13
            //管電圧と管電流とフィルタ厚の組み合わせが同じなら   'v18.00追加 byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //If (XrayCondVolt = ntbSetVolt.Value) And (XrayCondCurrent = ntbSetCurrent.Value) And _
            //'   (ntbFilter.value = infdef.filter(XrayCondIndex, GetIINo())) Then
            //v18.00変更 byやまおか 2011/08/09
            //    If (XrayCondVolt = ntbSetVolt.Value) And (XrayCondCurrent = ntbSetCurrent.Value) And _
            //'       (StrComp(ntbFilter.Text, GetFilterTableStr(XrayCondIndex, GetIINo())) = 0) Then
            //        'X線条件ボタンをセット
            //        SetCmdButton cmdCondition, XrayCondIndex
            //    Else
            //        'X線条件ボタンをクリア
            //        SetCmdButton cmdCondition, -1
            //    End If
            

            //If Not FirstDone Then
            //検出器切替ありの場合はここでウォームアップを促さない   'v17.51変更 by 間々田 2011/03/23
            //if ((!CTSettings.SecondDetOn))
            //Rev23.10 全フォーム起動終了後に統一するため ｺﾒﾝﾄｱｳﾄ by長野 2015/10/29
            //if ((!CTSettings.SecondDetOn) || (!CTSettings.ChangeXrayOn))//Rev23.10 条件追加 by長野 2015/09/29 
            //{

            //    //Select Case XrayType
            //    //
            //    //    Dim VoltAtWarmup As Single  'v15.11追加 byやまおか 2010/02/12
            //    //
            //    //    'Ｘ線のタイプがFEINFOCUSの場合
            //    //    Case XrayTypeFeinFocus
            //    //
            //    //        'メッセージ表示：Ｘ線制御器にてウォームアップを実施してください。
            //    //        MsgBox LoadResString(9378), vbInformation
            //    //
            //    //    'Ｘ線のタイプがViscomの場合
            //    //    Case XrayTypeViscom
            //    //
            //    //        'ウォームアップが未完了の時メッセージを表示
            //    //        If myXrayWarmUp <> XrayWarmUpComplete Then
            //    //            'ウォームアップ要求ダイアログ
            //    //            'Dim VoltAtWarmup As Single     'v15.11上へ移動 byやまおか 2010/02/12
            //    //            VoltAtWarmup = XrayVoltAtWarmingup
            //    //            If frmQueryWarmup.Dialog(VoltAtWarmup) Then
            //    //                cwneWarmupSetVolt.Value = VoltAtWarmup
            //    //                cmdWarmupStart_Click
            //    //            End If
            //    //        End If
            //    //
            //    //    'Ｘ線のタイプが浜ホト230の場合
            //    //    Case XrayTypeHamaL10801
            //    //
            //    //        'ウォームアップが未完了の時メッセージを表示
            //    //        If myXrayWarmUp <> XrayWarmUpComplete Then
            //    //            'ウォームアップ要求ダイアログ
            //    //            VoltAtWarmup = cwneKV.Maximum
            //    //            If frmQueryWarmup.Dialog(VoltAtWarmup) Then
            //    //                cwneWarmupSetVolt.Value = VoltAtWarmup
            //    //                cmdWarmupStart_Click
            //    //            End If
            //    //        End If
            //    //
            //    //    'Ｘ線のタイプがKEVEX、浜ホトの場合
            //    //    Case Else
            //    //
            //    //        'ウォームアップが未完了の時メッセージを表示
            //    //        If myXrayWarmUp <> XrayWarmUpComplete Then
            //    //            'メッセージ表示：ウォームアップを実施してください。
            //    //            MsgBox LoadResString(9443), vbInformation
            //    //        End If
            //    //
            //    //End Select
            //    //
            //    //FirstDone = True



            //    //v17.51上記の処理を関数化 by 間々田 2011/03/23
            //    //v19.19 Viscomの時、全フォーム表示後に処理するように変更 by長野 2013/09/24
            //    //v19.191 浜ホト追加 by長野 2013/10/02
            //    if (!((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeViscom) |
            //        //(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801)))
            //          (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801) |
            //          (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL12721) |
            //          (CTSettings.scaninh.Data.multi_tube == 0)))//Rev23.10 追加 by長野 2015/10/01
            //    {
            //        QueryWarmup();
            //    }

            //}
        
        }


        //*******************************************************************************
        //機　　能： 起動時にウォームアップ要の場合、ウォームアップを促す処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v17.51 2011/03/23 (豊川)間々田   新規作成
        //*******************************************************************************
        public void QueryWarmup()
        {
            #region　//全面変更2014/10/07hata_v19.51反映
            //全面変更2014/10/07hata_v19.51反映
            //######################################################################################################################       
            //int m_XR_Status;   //X線異常（0:異常、1:正常）

            ////すでに促している場合は何もしない
            //if (FirstDone) return;

            //float VoltAtWarmup;
            //switch (modXrayControl.XrayType)
            //{
            //    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //    //        'Ｘ線のタイプがFEINFOCUSの場合
            //    //        Case XrayTypeFeinFocus
            //    //
            //    //            'メッセージ表示：Ｘ線制御器にてウォームアップを実施してください。
            //    //            MsgBox LoadResString(9378), vbInformation
            //    //
            //    //        'Ｘ線のタイプがViscomの場合
            //    //        Case XrayTypeViscom
            //    //
            //    //            'ウォームアップが未完了の時メッセージを表示
            //    //            If myXrayWarmUp <> XrayWarmUpComplete Then
            //    //                'ウォームアップ要求ダイアログ
            //    //                VoltAtWarmup = XrayVoltAtWarmingup
            //    //                If frmQueryWarmup.Dialog(VoltAtWarmup) Then
            //    //                    cwneWarmupSetVolt.Value = VoltAtWarmup
            //    //                    cmdWarmupStart_Click
            //    //                End If
            //    //            End If
            //    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''

            //    //Ｘ線のタイプが浜ホト230の場合
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:

            //        //ウォームアップが未完了の時メッセージを表示
            //        if (myXrayWarmUp != modXrayControl.XrayWarmUpConstants.XrayWarmUpComplete)
            //        {
            //            //連続してメッセージを表示するためここでフラグをOnにする
            //            FirstDone = true;
                        
            //            frmQueryWarmup frmQueryWarmup = frmQueryWarmup.Instance;

            //            //ウォームアップ要求ダイアログ
            //            VoltAtWarmup = (float)cwneKV.Maximum;
            //            if (frmQueryWarmup.Dialog(ref VoltAtWarmup))
            //            {
            //                cwneWarmupSetVolt.Value = (decimal)VoltAtWarmup;
            //                cmdWarmupStart_Click(this, EventArgs.Empty);
            //            }
            //        }
            //        break;

            //    //X線タイプが浜ホト150kVのとき 'v17.71 追加 by長野 2012-03-26
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02:

            //        //int m_XR_Status;   //X線異常（0:異常、1:正常）
            //        //X線のステータス取得
            //        m_XR_Status = modXrayControl.XrayControl.Up_XR_Status;

            //        frmCTMenu frmCTMenu = frmCTMenu.Instance;

            //        //インターロックON状態、かつ、プリヒート以外の状態でないと起動時にウォームアップ完了のフラグが0で返ってくるため、if文で条件分岐
            //        //電磁ロックがかかっている、かつ、運手準備ON かつ、プリヒート状態でない
            //        if (((frmCTMenu.DoorStatus == frmCTMenu.DoorStatusConstants.DoorLocked) && (modSeqComm.MySeq.stsRunReadySW)) &&　(m_XR_Status == 1))
            //        {
            //            //ウォームアップが未完了の時メッセージを表示
            //            if (myXrayWarmUp != modXrayControl.XrayWarmUpConstants.XrayWarmUpComplete)
            //            {
            //                //連続してメッセージを表示するためここでフラグをOnにする
            //                FirstDone = true;

            //                //メッセージ表示中はtmrSeqCommを止める   //追加2014/04/01
            //                bool Tmr = frmMechaControl.Instance.tmrSeqComm.Enabled;
            //                frmMechaControl.Instance.tmrSeqComm.Enabled = false;			//追加 by 間々田 2009/07/31

            //                //メッセージ表示：ウォームアップを実施してください。
            //                MessageBox.Show(CTResources.LoadResString(9443), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

            //                //tmrSeqCommを再開   //追加2014/04/01
            //                frmMechaControl.Instance.tmrSeqComm.Enabled = Tmr;			//追加 by 間々田 2009/07/31

            //            }

            //            //初回のウォームアップステータスが取得できたことを知らせるフラグを立てる
            //            FirstDoneWarmupSts = true;

            //            //ウォームアップ完了のフラグが取れたら、一度ウォームアップステータスを更新する
            //            UpdateWarmUp();
            //        }
            //        else
            //        {
            //            return;
            //        }
            //        break;

            //    //Ｘ線のタイプがKEVEX、浜ホトの場合
            //    default:

            //        //ウォームアップが未完了の時メッセージを表示
            //        if (myXrayWarmUp != modXrayControl.XrayWarmUpConstants.XrayWarmUpComplete)
            //        {
            //            //連続して表示するためここでフラグをOnにする
            //            FirstDone = true;

            //            //メッセージ表示中はtmrSeqCommを止める   //追加2014/04/01
            //            bool Tmr2 = frmMechaControl.Instance.tmrSeqComm.Enabled;
            //            frmMechaControl.Instance.tmrSeqComm.Enabled = false;			//追加 by 間々田 2009/07/31

            //            //メッセージ表示：ウォームアップを実施してください。
            //            MessageBox.Show(CTResources.LoadResString(9443), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

            //            //tmrSeqCommを再開   //追加2014/04/01
            //            frmMechaControl.Instance.tmrSeqComm.Enabled = Tmr2;			//追加 by 間々田 2009/07/31

                    
            //        }
            //        break;
            //}

            //FirstDone = true;
            //全面変更2014/10/07hata_v19.51反映
            //######################################################################################################################       
            #endregion 全面変更2014/10/07hata_v19.51反映

            int m_XR_Status;   //X線異常（0:異常、1:正常）
            float VoltAtWarmup;
            
            //追加2014/10/28hata
            if (!modCT30K.CT30KSetup) //起動開始中
                return;

            //すでに促している場合は何もしない
            if (FirstDone)
                return;


            //X線異常（0:異常、1:正常）
            switch (modXrayControl.XrayType)
            {

                //Ｘ線のタイプがFEINFOCUSの場合
                case modXrayControl.XrayTypeConstants.XrayTypeFeinFocus:

                    //連続してメッセージを表示するためここでフラグをOnにする
                    FirstDone = true;

                    //メッセージ表示：Ｘ線制御器にてウォームアップを実施してください。
                    //Interaction.MsgBox(CT30K.My.Resources.str9378, MsgBoxStyle.Information);
                    MessageBox.Show(CTResources.LoadResString(9378), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                 
                    break;

                //Ｘ線のタイプがViscomの場合
                case modXrayControl.XrayTypeConstants.XrayTypeViscom:

                    //ウォームアップが未完了の時メッセージを表示
                    if (myXrayWarmUp != modXrayControl.XrayWarmUpConstants.XrayWarmUpComplete)
                    {
                        //連続してメッセージを表示するためここでフラグをOnにする
                        FirstDone = true;

                        //ウォームアップ要求ダイアログ
                        VoltAtWarmup = modXrayControl.XrayVoltAtWarmingup;
                        if (frmQueryWarmup.Instance.Dialog(ref VoltAtWarmup))
                        {
                            //変更2015/02/02hata_Max/Min範囲のチェック
                           //cwneWarmupSetVolt.Value = (decimal)VoltAtWarmup;
                             cwneWarmupSetVolt.Value = modLibrary.CorrectInRange((decimal)VoltAtWarmup, cwneWarmupSetVolt.Minimum, cwneWarmupSetVolt.Maximum);
                            cmdWarmupStart_Click(cmdWarmupStart, new System.EventArgs());
                        }
                    }
                    break;

                //Ｘ線のタイプが浜ホト230の場合
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL12721://Rev23.10 追加 by長野 2015/10/01

                    //ウォームアップが未完了の時メッセージを表示
                    if (myXrayWarmUp != modXrayControl.XrayWarmUpConstants.XrayWarmUpComplete)
                    {
                        //連続してメッセージを表示するためここでフラグをOnにする
                        FirstDone = true;
                        
                        //ウォームアップ要求ダイアログ
                         VoltAtWarmup = (float)cwneKV.Maximum;

                         if (frmQueryWarmup.Instance.Dialog(ref VoltAtWarmup))
                         {
                             //変更2015/02/02hata_Max/Min範囲のチェック
                             //cwneWarmupSetVolt.Value = (decimal)VoltAtWarmup;
                             cwneWarmupSetVolt.Value = modLibrary.CorrectInRange((decimal)VoltAtWarmup, cwneWarmupSetVolt.Minimum, cwneWarmupSetVolt.Maximum);

                             cmdWarmupStart_Click(cmdWarmupStart, new System.EventArgs());
                         }
                    }
                    break;

                //X線タイプが浜ホト150kVのとき 'v17.71 追加 by長野 2012-03-26
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02:

                    //X線のステータス取得
                    m_XR_Status = modXrayControl.XrayControl.Up_XR_Status;

                    //インターロックON状態、かつ、プリヒート以外の状態でないと起動時にウォームアップ完了のフラグが0で返ってくるため、if文で条件分岐
                    //電磁ロックがかかっている、かつ、運手準備ON かつ、プリヒート状態でない
                    if (((frmCTMenu.Instance.DoorStatus == frmCTMenu.DoorStatusConstants.DoorLocked) & (modSeqComm.MySeq.stsRunReadySW)) & !(m_XR_Status == 1))
                    {
                        //ウォームアップが未完了の時メッセージを表示
                        if (myXrayWarmUp != modXrayControl.XrayWarmUpConstants.XrayWarmUpComplete)
                        {
                            //連続してメッセージを表示するためここでフラグをOnにする
                            FirstDone = true;
                            
                            //メッセージ表示：ウォームアップを実施してください。
                            MessageBox.Show(CTResources.LoadResString(9443), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }

                        //初回のウォームアップステータスが取得できたことを知らせるフラグを立てる
                        FirstDoneWarmupSts = true;

                        //ウォームアップ完了のフラグが取れたら、一度ウォームアップステータスを更新する
                        UpdateWarmUp();

                    }
                    else
                    {
                        return;
                    }
                    break;

                //Titanの場合 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:
                case modXrayControl.XrayTypeConstants.XrayTypeSpellman:     //add Rev25.03/Rev25.02 2017/02/05 by chouno

                    //管電圧スライダーをセット   'v18.00変更 byやまおか 2011/03/26
                    //2014/11/07hata キャストの修正
                    //cwsldKV.Value = (int)ntbSetVolt.Value;
                    //cwsldKV.Value = Convert.ToInt32(ntbSetVolt.Value);
                    //Rev23.20 変更 by長野 2016/01/25
                    cwsldKV.Value = modLibrary.CorrectInRange(Convert.ToInt32(ntbSetVolt.Value),cwsldKV.Minimum,cwsldKV.Maximum);

                    //管電流スライダーをセット
                    //2014/11/07hata キャストの修正
                    //cwsldMA.Value = (int)ntbSetCurrent.Value;
                    //cwsldMA.Value = Convert.ToInt32(ntbSetCurrent.Value);
                    //Rev23.20 変更 by長野 2015/12/21
                    cwsldMA.Value = Convert.ToInt32(ntbSetCurrent.Value / cwneMA.Increment);
                    
                    break;

                    //焦点を制御器から取得してセット(大焦点1、小焦点0) 'v18.00追加 byやまおか 2011/06/03
                    //Dim focussize As Long
                    //focussize = Ti_GetFocusSize()
                    //If (Ti_SetFocusSize(focussize) = 0) Then
                    //    'mecainf（コモン）取得
                    //    GetMecainf mecainf
                    //    'mecainfの焦点を変更
                    //    mecainf.xfocus = focussize
                    //    'mecainf（コモン）更新
                    //    PutMecainf mecainf
                    //End If

                //Ｘ線のタイプがKEVEX、浜ホトの場合
                default:

                    //ウォームアップが未完了の時メッセージを表示
                    if (myXrayWarmUp != modXrayControl.XrayWarmUpConstants.XrayWarmUpComplete)
                    {
                        //連続してメッセージを表示するためここでフラグをOnにする
                        FirstDone = true;

                        //メッセージ表示：ウォームアップを実施してください。
                        //MessageBox.Show(CTResources.LoadResString(9443), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MessageBox.Show(this,CTResources.LoadResString(9443), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        
                    }
                    break;

            }

            FirstDone = true;
        }


        //*******************************************************************************
        //機　　能： Ｘ線条件選択時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
        private void cmdCondition_Click(object sender, EventArgs e)
        {
            if (sender as Button == null) return;

            int Index = Array.IndexOf(cmdCondition, sender);
            if (Index < 0) return;

            //Rev20.01 LMHボタンと焦点切り替え処理の2重呼び出しはさせない 追加 by長野 2015/06/03 
            if (myLMHChangeBusy == true || myFocusChangeBusy == true)
            {
                return;
            }

            //Rev20.01 追加 by長野 2015/06/03
            myLMHChangeBusy = true;
            //Rev20.01 LMHボタンを押したときと焦点を押したときのみ、TempSetCurrentとTempSetVoltをクリアする by長野 2015/06/03
            modXrayControl.TempSetCurrent = -1;
            modXrayControl.TempSetVolt = -1;

            //Ｘ線条件設定処理
            SetXrayCondition(Index, modSeqComm.GetIINo(), frmMechaControl.Instance.FIDWithOffset);

            //Rev20.01 追加 by長野 2015/06/03
            myLMHChangeBusy = false;
        }


        //*******************************************************************************
        //機　　能： Ｘ線条件設定処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： condIndex       [I/ ] Integer   選択されているＸ線条件 0:L 1:M 2:H
        //           iiIndex         [I/ ] Integer   選択されているI.I.視野 0:9 1:6 3:4.5
        //           Fid             [I/ ] Single    FID値（オフセットつき）
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
        public void SetXrayCondition(int condIndex, int iiIndex, float Fid)
        {
            float theVolt;
            float theCurrent;

            Debug.WriteLine(condIndex.ToString() + "  " + iiIndex.ToString() + "  " + Fid.ToString() + "  ");


            //'管電流計算に使用するパラメータ        'ほかでも使用するのでこのフォームのトップに移動
            //Const ParaK As Double = 2
            //
            //'管電圧計算に使用するパラメータ        'ほかでも使用するのでこのフォームのトップに移動
            //Const ParaL As Double = 1

            //Ｘ線条件(L, M, H)が選択されていない場合、何もしない
            //変更2014/11/25hata
            //if (!modLibrary.InRange(condIndex, CTSettings.xtable.Data.xtable.GetLowerBound(0), CTSettings.xtable.Data.xtable.GetUpperBound(0))) return;
            int condUpper = CTSettings.xtable.Data.xtable.Length / (3 * 2) - 1;
            if (!modLibrary.InRange(condIndex, 0, CTSettings.xtable.Data.xtable.GetUpperBound(0))) return;

            //try文を追加2014/11/25hata
            try
            {

                //追加2014/10/07hata_v19.51反映
                //フィルタを切り替える  'v18.00追加 byやまおか 2011/07/05
                if ((CTSettings.scaninh.Data.shutterfilter == 0))
                {
                    //上記を関数化   'v18.00変更 byやまおか 2011/07/05 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    modMechaControl.ChangeXConditionFilter(condIndex, iiIndex);
                }

                //フラットパネルの場合は最大視野サイズ   'v17.00 if追加 byやまおか 2010/02/25
                if (CTSettings.detectorParam.Use_FlatPanel) iiIndex = 0;   //0:9in 1:6in 2:4.5in相当

                //I.I.視野が選択されていない場合、何もしない
                //変更2014/11/25hata
                //if (!modLibrary.InRange(iiIndex, CTSettings.xtable.Data.xtable.GetLowerBound(0), CTSettings.xtable.Data.xtable.GetUpperBound(0))) return;
                int iiUpper = CTSettings.xtable.Data.xtable.Length / (3 * 2) - 1;
                if (!modLibrary.InRange(iiIndex, CTSettings.xtable.Data.xtable.GetLowerBound(0), iiUpper)) return;

                //マウスポインタを砂時計にする
                Cursor.Current = Cursors.WaitCursor;

                //管電圧はテーブル値
                //theVolt = CTSettings.xtable.Data.xtable[0, condIndex, iiIndex];
                theVolt = CTSettings.xtable.GetVolt(iiIndex, condIndex);

                //ベースＦＩＤ（オフセットつき）
                float BaseFIDWithOffset;
                BaseFIDWithOffset = CTSettings.xtable.Data.base_fdd + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube];  //xtable.base_fdd:Ｘ線条件基準FDD（オフセットを含まず）

                //最小設定管電流値（μA）
                float theCurrentMin;
                //変更2014/10/07hata_v19.51反映
                //theCurrentMin = (float)((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150) ? cwneMA.Minimum * 1000 : cwneMA.Minimum);
                switch (modXrayControl.XrayType)
                {
                    //v18.00変更 byやまおか 2011/03/01 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    case modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150:
                    case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:
                    case modXrayControl.XrayTypeConstants.XrayTypeSpellman:     //add Rev25.03/Rev25.02 2017/02/05 by chouno

                        theCurrentMin = (float)(cwneMA.Minimum * 1000);
                        break;
                    default:
                        theCurrentMin = (float)cwneMA.Minimum;
                        break;
                }

                //最大設定管電流値（μA）    'v15.10追加 byやまおか 2009/11/02
                float theCurrentMax;
                //    theCurrentMax = IIf(XrayType = XrayTypeToshibaEXM2_150, cwneMA.Minimum * 1000, cwneMA.Maximum)
                //変更2014/10/07hata_v19.51反映
                //theCurrentMax = (float)((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150) ? cwneMA.Maximum * 1000 : cwneMA.Maximum);   //Rev16.0 修正 IWA
                switch (modXrayControl.XrayType)
                {
                    //v18.00変更 byやまおか 2011/03/01 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    case modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150:
                    case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:
                    case modXrayControl.XrayTypeConstants.XrayTypeSpellman:     //add Rev25.03/Rev25.02 2017/02/05 by chouno

                        theCurrentMax = (float)(cwneMA.Maximum * 1000);
                        break;
                    default:
                        theCurrentMax = (float)(cwneMA.Maximum);
                        break;
                }

                //最小管電流値におけるＦＩＤ
                float FidMin;
                //FidMin = (float)(Math.Pow((theCurrentMin / CTSettings.xtable.Data.xtable[1, condIndex, iiIndex]), (1 / ParaK)) * BaseFIDWithOffset);
                FidMin = (float)(Math.Pow((theCurrentMin / CTSettings.xtable.GetCurrent(iiIndex, condIndex)), (1 / ParaK)) * BaseFIDWithOffset);

                if (Fid < FidMin)
                {
                    //管電圧を調整する
                    theVolt = (float)(theVolt * Math.Pow((Fid / FidMin), ParaL));

                    //管電流は最小値
                    theCurrent = theCurrentMin;
                }
                else
                {
                    //管電流はFIDのk乗に比例した値を設定する
                    //theCurrent = (float)(CTSettings.xtable.Data.xtable[1, condIndex, iiIndex] * Math.Pow((Fid / BaseFIDWithOffset), ParaK));
                    theCurrent = (float)(CTSettings.xtable.GetCurrent(iiIndex, condIndex) * Math.Pow((Fid / BaseFIDWithOffset), ParaK));

                    //管電流最大値を超えた場合は、最大値にする   'v15.10追加 byやまおか 2009/11/02
                    theCurrent = ((theCurrent < theCurrentMax) ? theCurrent : theCurrentMax);
                }

                //変更2014/10/07hata_v19.51反映
                //if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150) theCurrent = theCurrent / 1000;    //単位をmAにする
                //v18.00 Titan追加 byやまおか 2011/03/01 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150) |
                    (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) |
                    (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman)) //Rev25.03/Rev25.02 add by chouno 2017/02/05
                {
                    theCurrent = theCurrent / 1000;     //単位をmAにする
                }

                //Rev20.01 追加 by長野 2015/06/04
                Thread.Sleep(1000);

                //管電圧・管電流を設定
                modXrayControl.SetVolt(theVolt);

                //連続で管電圧・管電流を設定すると失敗するので
                //Sleep (2000)    '2秒待つ    'v16.03/v16.20変更 byやまおか 2010/03/03
                //If XrayType = XrayTypeHamaL10801 Then Sleep (5000) '5秒待つ仮対策(ファームが対応したら削除する)  'v16.01追加 byやまおか 2010/02/26
                //If XrayType = XrayTypeHamaL10801 Then Sleep (3000)  '更に3秒待つ仮対策(ファームが対応したら削除する)  'v16.03変更 byやまおか 2010/03/03    'v16.20削除 byやまおか 2010/04/05

                //変更2014/11/25hata
                //何回もループするためSleepに変更する
                //modCT30K.PauseForDoEvents(1);       //1秒待つ    'v17.10変更 byやまおか 2010/09/01
                //Rev20.01 変更 by長野 2015/06/03
                Thread.Sleep(1000);

                float value = theCurrent;
                float.TryParse(value.ToString(modLibrary.GetFormatString((float)cwneMA.Increment)), out value);
                modXrayControl.SetCurrent(value);

                //追加2014/10/07hata_v19.51反映
                //v18.00追加 byやまおか 2011/06/24 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                //Rev25.03/Rev25.02 change by chouno 2017/02/05
                if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) || (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman))
                {
                    modCT30K.PauseForDoEvents(1);

                    //変更2015/02/02hata_Max/Min範囲のチェック
                    //cwneKV.Value = (decimal)theVolt;
                    ////2014/11/07hata キャストの修正
                    ////cwsldKV.Value = (int)theVolt;
                    //cwsldKV.Value = Convert.ToInt32(theVolt);
                    //cwneMA.Value = (decimal)theCurrent;
                    ////2014/11/07hata キャストの修正
                    ////cwsldMA.Value = (int)theCurrent;
                    //cwsldMA.Value = Convert.ToInt32(theCurrent);
                    cwneKV.Value = modLibrary.CorrectInRange((decimal)theVolt, cwneKV.Minimum, cwneKV.Maximum);
                    cwsldKV.Value = Convert.ToInt32(cwneKV.Value);
                    cwneMA.Value = modLibrary.CorrectInRange((decimal)theCurrent, cwneMA.Minimum, cwneMA.Maximum);
                    //cwsldMA.Value = Convert.ToInt32(cwneMA.Value);
                    //Rev23.20 変更 by長野 2015/12/23 by長野
                    cwsldMA.Value = Convert.ToInt32(cwneMA.Value / cwneMA.Increment);
                }

            }
            finally
            {
                //マウスポインタを元に戻す
                Cursor.Current = Cursors.Default;

            }

            //マウスポインタを元に戻す
            Cursor.Current = Cursors.Default;

            ////Rev20.01 追加 by長野 2015/06/03
            //Thread.Sleep(1000);

            return;
        }


        //*******************************************************************************
        //機　　能： 推奨フィルター厚設定処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2009/08/19 (SS1)間々田   新規作成
        //*******************************************************************************
        public void SetFilter(int condIndex, int iiIndex)
        {
            //Ｘ線条件(L, M, H)が選択されていて、かつI.I.視野が選択されている場合
            //変更2014/11/25hata
            //if (modLibrary.InRange(condIndex, CTSettings.infdef.Data.filter.GetLowerBound(0), CTSettings.infdef.Data.filter.GetUpperBound(0)) &&
            //    modLibrary.InRange(iiIndex, CTSettings.infdef.Data.filter.GetLowerBound(0), CTSettings.infdef.Data.filter.GetUpperBound(0)))
            int condUpper = CTSettings.infdef.Data.filter.Length / 3 - 1;
            int iiUpper = CTSettings.infdef.Data.filter.Length / 3 - 1;
            if (modLibrary.InRange(condIndex, CTSettings.infdef.Data.filter.GetLowerBound(0), condUpper) &&
                modLibrary.InRange(iiIndex, CTSettings.infdef.Data.filter.GetLowerBound(0), iiUpper))
            {
                //ntbFilter.Value = (decimal)CTSettings.infdef.Data.filter[condIndex, iiIndex];
                ntbFilter.Value = (decimal)CTSettings.infdef.Data.filter[iiIndex * 3 + condIndex];

                //変更2014/11/25hata
                //ntbFilter.BackColor = SystemColors.Window;
                ntbFilter.TextBackColor = SystemColors.Window;
            }
            //それ以外の場合
            else
            {
                ntbFilter.Clear();                              //空欄にする
                //変更2014/11/25hata
                //ntbFilter.BackColor = SystemColors.Control;     //背景色を灰色にする
                ntbFilter.TextBackColor = SystemColors.Control;     //背景色を灰色にする
            }
        }

        //追加2014/10/07hata_v19.51反映
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/07 ここから
        //*******************************************************************************
        //機　　能： 推奨フィルタ厚変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足：
        //
        //履　　歴： v18.00  2011/06/03  やまおか    新規作成
        //*******************************************************************************
        private void ntbFilter_ValueChanged(System.Object Sender, NumTextBox.ValueChangedEventArgs e)
        {

            object PreviousValue = e.PreviousValue;

            //if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan))
            //Rev25.03/Rev25.02 change by chouno 2017/02/05
            if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) || (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman))
            {
                //mecainf（コモン）に番号をセットする
                //GetMecainf(mecainf);
                CTSettings.mecainf.Load();
                
                CTSettings.mecainf.Data.xfilter = modSeqComm.GetFilterIndex();

                //PutMecainf(mecainf);
                CTSettings.mecainf.Write();

                //Rev23.40 追加 by長野 2016/06/19
                //イベント生成
                if (FilterChanged != null)
                {
                    FilterChanged(this, EventArgs.Empty);
                }
            }

        }
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/07 ここまで
        //*******************************************************************************
        //機　　能： 推奨フィルタ厚変更時処理(なし、シャッタ時)
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足：
        //
        //履　　歴： v24.00  2016/05/13  (検S1)長野  新規作成
        //*******************************************************************************
        private void ntbFilter_TextChange()
        {
            if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan))
            {
                CTSettings.mecainf.Data.xfilter = modSeqComm.GetFilterIndex();

                if (CTSettings.mecainf.Data.xfilter == 0 || CTSettings.mecainf.Data.x_ray_auto == 5)
                {
                    //mecainf（コモン）に番号をセットする
                    //GetMecainf(mecainf);
                    CTSettings.mecainf.Load();

                    //PutMecainf(mecainf);
                    CTSettings.mecainf.Write();

                    //Rev24.00 追加 by長野 2016/05/13
                    //イベント生成
                    if (FilterChanged != null)
                    {
                        FilterChanged(this, EventArgs.Empty);
                    }
                }
            }
        }
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/07 ここまで

        //*******************************************************************************
        //機　　能： Ｘ線条件更新処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： FDD, I.I.視野, 管電圧設定値, 管電流設定値に変化があればこの関数を呼ぶこと
        //
        //履　　歴： v15.0  2009/02/17  (SS1)間々田  新規作成
        //*******************************************************************************
        public void UpdateXrayCondition()
        {
            int i;
            float theVolt;
            float theCurrent;

            //最小管電流値におけるＦＩＤ
            float FidMin;

            //ベースＦＩＤ（オフセットつき）
            float BaseFIDWithOffset;
            BaseFIDWithOffset = CTSettings.xtable.Data.base_fdd + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube];      //xtable.base_fdd:Ｘ線条件基準FDD（オフセットを含まず）

            //最小設定管電流値（μA）
            float theCurrentMin;

            theCurrentMin = (float)((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150) ? cwneMA.Minimum * 1000 : cwneMA.Minimum);

            //最小設定管電圧値（kV） 'v16.01/v17.00追加 byやまおか 2010/20/18
            float theVoltMin;
            theVoltMin = (float)cwneKV.Minimum;

            //I.I.視野を求める
            int iiIndex;
            iiIndex = modSeqComm.GetIINo();
            //フラットパネルの場合は最大視野にする   'v17.00追加 byやまおか 2010/02/25
            if (CTSettings.detectorParam.Use_FlatPanel) iiIndex = 0;       //0:9in 1:6in 2:4.5in相当

            //Rev20.01 上に移動 by長野 2015/06/03
            //-->v17.47/v14.53追加 それまで選択していたＸ線条件を記憶（最大管電流のとき）by 間々田 2011/03/09
            int condIndex;
            condIndex = modLibrary.GetCmdButton(cmdCondition);

            //Rev20.01 追加 by長野 2015/06/04
            decimal tmpVolt = 0;
            decimal tmpCurrent = 0;


            //変更2014/11/25hata
            //if (modLibrary.InRange(iiIndex, CTSettings.xtable.Data.xtable.GetLowerBound(0), CTSettings.xtable.Data.xtable.GetUpperBound(0)))
            int iiUpper = CTSettings.xtable.Data.xtable.Length / (3 * 2 ) - 1 ;
            if (modLibrary.InRange(iiIndex, CTSettings.xtable.Data.xtable.GetLowerBound(0), iiUpper))
            {
                //theCurrent = ntbSetCurrent.Value

                //マッチしているＸ線条件を探す
                for (i = cmdCondition.GetLowerBound(0); i <= cmdCondition.GetUpperBound(0); i++)
                {
                    //最小管電流値におけるＦＩＤ
                    //FidMin = (float)(Math.Pow((theCurrentMin / CTSettings.xtable.Data.xtable[1, i, iiIndex]), (1 / ParaK)) * BaseFIDWithOffset);
                    FidMin = (float)(Math.Pow((theCurrentMin / CTSettings.xtable.GetCurrent(iiIndex,i)), (1 / ParaK)) * BaseFIDWithOffset);

                    //この条件の管電圧値
                    //theVolt = CTSettings.xtable.Data.xtable[0, i, iiIndex];
                    theVolt = CTSettings.xtable.GetVolt(iiIndex, i);

                    frmMechaControl frmMechaControl = frmMechaControl.Instance;

                    if (frmMechaControl.FIDWithOffset < FidMin)
                    {
                        //最小管電流時のＦＩＤよりも小さい場合、管電圧が調整される
                        theVolt = (float)(theVolt * Math.Pow((frmMechaControl.FIDWithOffset / FidMin), ParaL));
                        theVolt = ((theVolt < theVoltMin) ? theVoltMin : theVolt);      //v16.01/v17.00追加 byやまおか 2010/20/18
                        theCurrent = theCurrentMin;
                    }
                    else
                    {
                        //theCurrent = (float)(CTSettings.xtable.Data.xtable[1, i, iiIndex] * Math.Pow((frmMechaControl.FIDWithOffset / BaseFIDWithOffset), ParaK));
                        theCurrent = (float)(CTSettings.xtable.GetCurrent(iiIndex,i) * Math.Pow((frmMechaControl.FIDWithOffset / BaseFIDWithOffset), ParaK));
                    }

                    //追加2014/10/07hata_v19.51反映
                    //If (XrayType = XrayTypeToshibaEXM2_150) Then theCurrent = theCurrent / 1000 '単位をmAにする
                    //v18.00 Titan追加 byやまおか 2011/03/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150) |
                        (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) |
                        (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman)) //Rev25.03/Rev25.02 add by chouno 2017/02/05
                    {
                        theCurrent = theCurrent / 1000;     //単位をmAにする
                    }

                    //管電圧と管電流の組み合わせが同じならその条件を採用
                    decimal volt;
                    decimal current;
                    decimal.TryParse(theVolt.ToString(modLibrary.GetFormatString((float)cwneKV.Increment)), out volt);
                    decimal.TryParse(theCurrent.ToString(modLibrary.GetFormatString((float)cwneMA.Increment)), out current);
                    
                    //Rev20.01 追加 by長野 2015/06/04
                    if (i == condIndex)
                    {
                        tmpVolt = volt;
                        tmpCurrent = current;
                    }

                    if ((volt == ntbSetVolt.Value) && (current == ntbSetCurrent.Value))
                    {
                        //変更2014/10/07hata_v19.51反映
                        //modLibrary.SetCmdButton(cmdCondition, i);
                        //X線条件はここでセットしないことにした(Updateでセットする)   'v18.00変更 byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                        //X線条件の情報だけをPublicにセットする
                        XrayCondIndex = i;
                        XrayCondVolt = (float)ntbSetVolt.Value;
                        XrayCondCurrent = (float)ntbSetCurrent.Value;

                        //変更2014/10/07hata_v19.51反映
                        //SetFilter(i, iiIndex);
                        //If (XrayType <> XrayTypeGeTitan) Then SetFilter i, iiIndex     'Titanの場合はセットしない   'v18.00変更 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                        if ((CTSettings.scaninh.Data.shutterfilter != 0))
                            SetFilter(i, iiIndex);      //電動シャッターフィルターの場合はセットしない   'v18.00変更 byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                        
                        return;
                    }
                }

                //Rev20.01 追加 by長野 2015/06/04
                //どの条件にもヒットしなかった場合にも、ターゲットになる条件XrayCondition,XrayCondVolt,XrayCondCurrentは更新する
                //I.I.視野を切り替えてもXrayCondIndexとXrayCondVoltは更新されないため
                if (condIndex > -1)
                {
                    XrayCondIndex = condIndex;
                    XrayCondVolt = (float)tmpVolt;
                    XrayCondCurrent = (float)tmpCurrent;
                }
            }

            //Rev20.01 上に移動 by長野 2015/06/03
            ////-->v17.47/v14.53追加 それまで選択していたＸ線条件を記憶（最大管電流のとき）by 間々田 2011/03/09
            //int condIndex;
            //condIndex = modLibrary.GetCmdButton(cmdCondition);
       
            //それまでどれか選択していて、かつ最大管電流だった場合、Ｘ線条件を記憶 by 間々田 2011/03/09
            if (modLibrary.InRange(condIndex, cmdCondition.GetLowerBound(0), cmdCondition.GetUpperBound(0)) && (ntbSetCurrent.Value == cwneMA.Maximum))
            {
                LastCondIndex = condIndex;
            }
            else
            {
                LastCondIndex = -1;
            }
            //<--v17.47/v14.53追加 それまで選択していたＸ線条件を記憶（最大管電流のとき）by 間々田 2011/03/09

            //どれも選択しない
            modLibrary.SetCmdButton(cmdCondition, -1);

            //推奨フィルター厚を不定とする       '追加 by 間々田 2009/08/19
            //変更2014/10/07hata_v19.51反映
            //SetFilter(-1, iiIndex);
            //Rev25.03/Rev25.02 add by chouno 2017/02/05
            if ((modXrayControl.XrayType != modXrayControl.XrayTypeConstants.XrayTypeGeTitan) && (modXrayControl.XrayType != modXrayControl.XrayTypeConstants.XrayTypeSpellman))

                SetFilter(-1, iiIndex);            //Titanの場合はセットしない   'v18.00変更 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

        }


        //*******************************************************************************
        //機　　能： Ｘ線ツール用タイマー処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  2006/02/13  (SI3)間々田  新規作成
        //*******************************************************************************
        private void tmrXrayTool_Timer(object sender, EventArgs e)
        {
            //カウントダウン
            modXrayControl.XrayToolTimerCount = modXrayControl.XrayToolTimerCount - 1;

            //カウントダウン終了？
            if (modXrayControl.XrayToolTimerCount == 0)
            {
                //タイマー停止
                tmrXrayTool.Enabled = false;

                //Ｘ線オフ
                modXrayControl.XrayOff();

                //タイマーによるＸ線オフメッセージ表示用フラグオン
                XrayOffByTimer = true;
            }

            //時間を表示
            //frmXrayTool frmXrayTool = frmXrayTool.Instance;
            //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
            //if (modLibrary.IsExistForm(frmXrayTool)) frmXrayTool.Instance.MyUpdate();
            if (modLibrary.IsExistForm("frmXrayTool")) frmXrayTool.Instance.MyUpdate();
        }

//#if (!DebugOn)
//Rev23.10 変更 by長野
#if (!XrayDebugOn) //Rev23.10 2015/10/02
        //*******************************************************************************
        //機　　能： Ｘ線イベントを受け取る（エラー情報）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  2006/02/20  (SI3)間々田  新規作成
        //           v16.20 2010/04/21  やまおか     FeinFocus.exe→XrayCtrl.exe
        //*******************************************************************************
        //Private Sub UC_Feinfocus_ErrSetDisp(Val1 As FeinFocus.ErrSet)
        private void UC_XrayCtrl_ErrSetDisp(object sender, clsTActiveX.ErrSetEventArgs e)
        {
            myXrayErrSet = e.ErrSet;
            try
            {
                //スレッド間で操作できないため
                XrayErrSetUpdate();
            }
            catch
            {
            }
            finally
            {
            }                  
            
            //clsTActiveX.ErrSet Val1 = e.ErrSet;

            ////logOut "UC_Feinfocus_ErrSetDisp"
            //modCT30K.logOut("UC_XrayCtrl_ErrSetDisp");      //v16.20変更 byやまおか 2010/04/21

            //int idat1;

            ////ActiveX制御ｴﾗｰの内容を表示
            //idat1 = Val1.m_ErrNO;

            //switch (idat1)
            //{
            //    //正常の場合、抜ける
            //    case 0:
            //        return;

            //    //ｷｰｽｲｯﾁの切り替えに反応してしまう為、このｴﾗｰは無視する  'V3.0 append by 鈴山 2000/09/28
            //    case 20:
            //        break;

            //    //フィラメント断線が出てしまうため、このエラーは無視する 'v16.10/v17.00追加 byやまおか 2010/03/17
            //    case 24:
            //        break;

            //    //定義されているエラーの場合
            //    //Case 4, 7, 12, 21 To 25
            //    case 4:
            //    case 7:
            //    case 12:
            //    case 21:
            //    case 22:
            //    case 23:
            //    case 25:    //v16.10/v17.00変更 byやまおか 2010/03/17
            //        MessageBox.Show(CTResources.LoadResString(StringTable.IDS_ErrorNum) + idat1.ToString() + "\r\n" + "\r\n" +
            //                        CTResources.LoadResString(StringTable.IDS_ErrorNum + idat1), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        break;

            //    //定義されていないエラーの場合
            //    default:
            //        MessageBox.Show(CTResources.LoadResString(StringTable.IDS_ErrorNum) + idat1.ToString() + "\r\n" + "\r\n" +
            //                        CTResources.LoadResString(StringTable.IDS_UnkownError), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        break;
            //}

            ////エラーステータスに対する確認応答
            //idat1 = 1;                               //v9.5 追加 by 間々田 2004/09/16
            //modXrayControl.XrayControl.MessageOk_Set(idat1);
        }


        //*******************************************************************************
        //機　　能： Ｘ線イベントを受け取る
        //                   出力管電圧、出力管電流
        //                   Ｘ線オン・オフ
        //                   Ｘ線アベイラブル
        //                   真空度、ターゲット電流リミット
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //           V16.20 10/04/21   やまおか      FeinFocus.exe→XrayCtrl.exe
        //*******************************************************************************
        //Private Sub UC_Feinfocus_MechDataDisp(Val1 As FeinFocus.MechData)
        private void UC_XrayCtrl_MechDataDisp(object sender, clsTActiveX.MechDataEventArgs e)
        {
            //スレッド間で操作できないため
            myXrayMechData = e.MechData;
            try
            {
                XrayMechDataUpdate();
            }
            catch
            {
            }
            finally
            {
            }

            //clsTActiveX.MechData Val1 = e.MechData;

            ////logOut "UC_Feinfocus_MechDataDisp"
            //modCT30K.logOut("UC_XrayCtrl_MechDataDisp");    //v16.20変更 byやまおか 2010/04/21


            ////管電圧／管電流（実測値）
            //switch (modXrayControl.XrayType)
            //{
            //    //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421
            //    //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL10801    'v15.10変更 byやまおか 2009/10/07
            //    //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL10801, XrayTypeHamaL8601 'v16.03/v16.20変更 byやまおか 2010/03/03
            //    //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL10801, XrayTypeHamaL8601, XrayTypeHamaL9421_02T  'v16.30 02T追加 byやまおか 2010/05/21
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9191:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL8601:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02:     //v17.71 追加 by長野 2012/03/14
            //        ntbActVolt.Value = (decimal)Val1.m_Voltage;              //管電圧（実測値）
            //        ntbActCurrent.Value = (decimal)Val1.m_Curent;            //管電流（実測値）
            //        break;
            //}

            ////Ｘ線オン・オフ状態
            //MecaXrayOn = (modCT30K.OnOffStatusConstants)Val1.m_XrayOnSet;

            ////アベイラブル
            //MecaXrayAvailable = (modCT30K.OnOffStatusConstants)Val1.m_XAvail;

            ////浜ホト160kV,230kVの時                                            v9.5 追加 by 間々田 2004/09/13
            ////If XrayType = XrayTypeHamaL9191 Then
            //if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL9191) ||
            //    (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801))       //v15.10変更 byやまおか 2009/10/07
            //{
            //    //真空度
            //    UpdateVacuumSVC(Val1.m_XrayVacuumSVC);
            //    ntbVacSVV.Value = (decimal)modXrayControl.XrayControl.Up_XrayStatusSVV;     //v19.02追加 byやまおか 2012/07/20

            //    //ターゲット電流
            //    ntbTargetCurrent.Value = (decimal)Val1.m_XrayTargetInfSTG;                                       //小数点以下１桁表示
            //    //ntbTargetCurrent.BackColor = IIf((.m_XrayTargetLimit = 1) And (.m_XrayOnSet = 1), SunsetOrange, vbWhite) '背景色
            //    ntbTargetCurrent.BackColor = (((Val1.m_XrayTargetLimit == 1) && (Val1.m_XrayOnSet == 1)) ? modCT30K.SunsetOrange : SystemColors.Control);    //v15.10変更 byやまおか 2009/10/15
            //}


            ////焦点：ウォームアップが完了かつX線OFFの時だけ焦点ボタンを使用可にする
            //switch (modXrayControl.XrayType)
            //{
            //    //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421
            //    //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL10801   'v15.10変更 byやまおか 2009/10/07
            //    //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL10801, XrayTypeHamaL9421_02T  'v16.30 02T追加 byやまおか 2010/05/21
            //    case modXrayControl.XrayTypeConstants.XrayTypeKevex:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9191:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02:       //v17.71 追加 by長野 2012/03/14
            //        fraFocus.Enabled = (modXrayControl.XrayWarmUp() == modXrayControl.XrayWarmUpConstants.XrayWarmUpComplete) && !(Val1.m_XrayOnSet == 1);
            //        modLibrary.SetCmdButton(cmdFocus, Convert.ToInt32(modXrayControl.XrayControl.Up_Focussize), ControlEnabled: true);
            //        break;
            //}

            ////X線ステータスをログファイルに書き込む   'v19.02追加 byやまおか 2012/07/20
            //XrayStatusLogOut_withoutOpen();
        }


        //*******************************************************************************
        //機　　能： Ｘ線イベントを受け取る（焦点情報）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //           V16.20 10/04/21   やまおか      FeinFocus.exe→XrayCtrl.exe
        //*******************************************************************************
        //Private Sub UC_Feinfocus_UserValueDisp(Val1 As FeinFocus.UserValue)
        private void UC_XrayCtrl_UserValueDisp(object sender, clsTActiveX.UserValueEventArgs e)
        {
            //スレッド間で操作できないため
            myXrayUserValue = e.UserValue;
            try
            {
                XrayUserValueUpdate();
            }
            catch
            {
            }
            finally
            {
            }                  
           
            //clsTActiveX.UserValue Val1 = e.UserValue;

            ////logOut "UC_Feinfocus_UserValueDisp"
            //modCT30K.logOut("UC_XrayCtrl_UserValueDisp");   //v16.20変更 byやまおか 2010/04/21

            ////焦点情報を表示
            //switch (modXrayControl.XrayType)
            //{
            //    //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421
            //    //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL10801   'v15.10変更 byやまおか 2009/10/13
            //    //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421     'v16.03/v16.20変更 byやまおか 2010/03/03
            //    //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL9421_02T  'v16.30 02T追加 byやまおか 2010/05/21
            //    case modXrayControl.XrayTypeConstants.XrayTypeKevex:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9191:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02:     //v17.71 追加 by長野 2012/03/14
            //        //SetCmdButton cmdFocus, CLng(Val1.m_XrayFocusSize)
            //        modLibrary.SetCmdButton(cmdFocus, Convert.ToInt32(Val1.m_XrayFocusSize), ControlEnabled: true);     //v11.41変更 by 間々田 2006/03/29
            //        break;
            //}
        }


        //*******************************************************************************
        //機　　能： Ｘ線イベントを受け取る（設定管電圧・設定管電流情報）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //           V16.20 10/04/21   やまおか      FeinFocus.exe→XrayCtrl.exe
        //*******************************************************************************
        //Private Sub UC_Feinfocus_XrayValueDisp(Val1 As FeinFocus.XrayValue)
        private void UC_XrayCtrl_XrayValueDisp(object sender, clsTActiveX.XrayValueEventArgs e)
        {
            //スレッド間で操作できないため
            myXrayValue = e.XrayValue;
            try
            {
                XrayValueUpdate();
            }
            catch
            {
            }
            finally
            {
            }                  
            
            //clsTActiveX.XrayValue Val1 = e.XrayValue;

            //Debug.Print("XrayType = " + modXrayControl.XrayType);
            //Debug.Print("cwsldKV.Minimum = " + cwsldKV.Minimum.ToString());
            //Debug.Print("cwsldKV.Maximum = " + cwsldKV.Maximum.ToString()); 

            ////logOut "UC_Feinfocus_XrayValueDisp"
            //modCT30K.logOut("UC_XrayCtrl_XrayValueDisp");   //v16.20変更 byやまおか 2010/04/21

            ////v17.71 L8121_02で確実に管電流、管電圧を変更するための対策  by長野 2012-03-24
            ////設定管電圧・設定管電流
            //if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02)
            //{
            //    Val1.m_kVSet = modXrayControl.XrayControl.Up_XR_VoltSet;
            //    Val1.m_mASet = modXrayControl.XrayControl.Up_XR_CurrentSet;
            //}

            //ntbSetVolt.Value = (decimal)Val1.m_kVSet;
            //ntbSetCurrent.Value = (decimal)Val1.m_mASet;

            //byEvent = false;

            ////電圧スライダ設定
            //int val = (int)((decimal)Val1.m_kVSet / cwneKV.Increment);
            ////cwsldKV.Value = val;
            //if (val < cwsldKV.Minimum)
            //{
            //    //cwsldKV.Value = cwsldKV.Minimum;
            //    cwsldKV.Value = 30;
            //}
            //else
            //{
            //    cwsldKV.Value = val;
            //}
            //if (cwneKV.Value != cwsldKV.Value * cwneKV.Increment) cwneKV.Value = cwsldKV.Value * cwneKV.Increment;  //テキストボックス側にも確実に入れる 'v17.47/v17.53追加 2011/03/09 by 間々田

            ////電流スライダ設定
            ////cwsldMA.Value = (int)((decimal)Val1.m_mASet / cwneMA.Increment);
            //val = (int)((decimal)Val1.m_mASet / cwneMA.Increment);
            //if (val < cwsldMA.Minimum)
            //{
            //    cwsldMA.Value = cwsldMA.Minimum;
            //}
            //else
            //{
            //    cwsldMA.Value = (int)((decimal)Val1.m_mASet / cwneMA.Increment);
            //}
            //if (cwneMA.Value != cwsldMA.Value * cwneMA.Increment) cwneMA.Value = cwsldMA.Value * cwneMA.Increment;  //テキストボックス側にも確実に入れる 'v17.47/v17.53追加 2011/03/09 by 間々田

            //byEvent = true;
        }


        //*******************************************************************************
        //機　　能： Ｘ線イベントを受け取る（ウォームアップ情報）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //           V16.20 10/04/21   やまおか      FeinFocus.exe→XrayCtrl.exe
        //*******************************************************************************
        //Private Sub UC_Feinfocus_StatusValueDisp(Val1 As FeinFocus.StatusValue)
        private void UC_XrayCtrl_StatusValueDisp(object sender, clsTActiveX.StatusValueEventArgs e)
        {
            //スレッド間で操作できないため
            myXrayStatusValue = e.StatusValue;
            try
            {
                XrayStatusValueUpdate();
            }
            catch
            {
            }
            finally
            {
            }                  
            
            //clsTActiveX.StatusValue Val1 = e.StatusValue;
            //
            ////logOut "UC_Feinfocus_StatusValueDisp"
            //modCT30K.logOut("UC_XrayCtrl_StatusValueDisp");     //v16.20変更 byやまおか 2010/04/21

            ////ウォームアップ情報の更新
            //if (modXrayControl.XrayType != modXrayControl.XrayTypeConstants.XrayTypeFeinFocus)
            //{
            //    myXrayWarmUp = modXrayControl.XrayWarmUp();
            //    UpdateWarmUp();
            //}
        }

#else

        private void UC_XrayCtrl_ErrSetDisp(object sender, frmVirtualXrayControl.ErrSetEventArgs e)
        {
        }
        private void UC_XrayCtrl_MechDataDisp(object sender, frmVirtualXrayControl.MechDataEventArgs e)
        {
        }
        private void UC_XrayCtrl_UserValueDisp(object sender, frmVirtualXrayControl.UserValueEventArgs e)
        {
        }
        private void UC_XrayCtrl_XrayValueDisp(object sender, frmVirtualXrayControl.XrayValueEventArgs e)
        {
        }
        private void UC_XrayCtrl_StatusValueDisp(object sender, frmVirtualXrayControl.StatusValueEventArgs e)
        {
        }

#endif

        //*******************************************************************************
        //機　　能： 真空度の表示更新
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v9.5  04/09/21 (SI4)間々田     新規作成
        //*******************************************************************************
        private void UpdateVacuumSVC(string strValue)
        {
            int theLevel;
            int i;

            theLevel = 0;
            for (i = lblVac.GetLowerBound(0); i <= lblVac.GetUpperBound(0); i++)
            {
                if (strValue == Convert.ToString(lblVac[i].Tag))
                {
                    theLevel = i + 1;
                    break;
                }
            }

            for (i = lblVac.GetLowerBound(0); i <= lblVac.GetUpperBound(0); i++)
            {
                lblVac[i].BackColor = ((theLevel > i) ? Color.Lime : modCT30K.DarkGreen);
            }
        }


        //*******************************************************************************
        //機　　能： ウォームアップ情報表示
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        //private void UpdateWarmUp()
        //Rev20.01 変更 by長野 2015/05/20
        public void UpdateWarmUp()
        {
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //Viscom対応
            //    If XrayType = XrayTypeViscom Then
            //        'ウォームアップが中止された場合、
            //        If (lblWarmupStatus.Caption = GC_Xray_WarmUp) And (myXrayWarmUp <> XrayWarmUpNow) And XrayWarmUpCanceled Then
            //            XrayVoltAtWarmingup = Int(XrayMaxFeedBackVolt / 10) * 10 - 10
            //            cwneWarmupSetVolt.Value = XrayVoltAtWarmingup
            //        End If
            //    End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            modTitan.WarmupConstants sts = default(modTitan.WarmupConstants);


            //'段階ウォームアップ用ステップチェックボックスを無効化   'v17.72/v19.02追加 byやまおか 2012/05/17
            //chkStepWU.Enabled = (Not CBool(.Caption = GC_Xray_WarmUp)) 'v17.72/v19.02変更 下へ移動 byやまおか 2012/07/09

            switch (myXrayWarmUp)
            {
                case modXrayControl.XrayWarmUpConstants.XrayWarmUpNow:

                    lblWarmupStatus.Text = StringTable.GC_Xray_WarmUp;              //ｳｫｰﾑｱｯﾌﾟ中
                    break;

                case modXrayControl.XrayWarmUpConstants.XrayWarmUpComplete:

                    lblWarmupStatus.Text = CTResources.LoadResString(12060);          //完了
                    break;

                case modXrayControl.XrayWarmUpConstants.XrayWarmUpNotComplete:

                    switch (modXrayControl.XrayType)
                    {
                        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                        //                    Case XrayTypeKevex
                        //
                        //                        Select Case XrayControl.Up_Wrest_Mode
                        //                            Case 1: .Caption = LoadResString(12371) '２日
                        //                            Case 2: .Caption = LoadResString(12182) '２週
                        //                            Case 3: .Caption = LoadResString(12183) '３週
                        //                        End Select
                        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

                        //Case XrayTypeHamaL9181, XrayTypeHamaL9421
                        case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
                        case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
                        case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:
                        case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181_02:     //追加2014/10/07hata_v19.51反映

                            switch (modXrayControl.XrayControl.Up_Wrest_Mode)
                            {
                                case 1:
                                    lblWarmupStatus.Text = CTResources.LoadResString(12370);  //１日
                                    break;
                                case 2:
                                    lblWarmupStatus.Text = CTResources.LoadResString(12372);  //１ヶ月
                                    break;
                                case 3:
                                    lblWarmupStatus.Text = CTResources.LoadResString(12373);  //３ヶ月
                                    break;
                            }
                            break;

                        //Case XrayTypeViscom
                        case modXrayControl.XrayTypeConstants.XrayTypeViscom:
                        case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:       //v16.01/v17.00変更 byやまおか 2010/02/10
                        case modXrayControl.XrayTypeConstants.XrayTypeHamaL12721:       //Rev23.10 追加 by長野 2015/10/01
                        case modXrayControl.XrayTypeConstants.XrayTypeHamaL10711:       //Rev23.10 追加 by長野 2015/10/01

                            lblWarmupStatus.Text = CTResources.LoadResString(12368);          //未完
                            break;

                        //v17.71 L8121_02の場合、インターロックONかつ、プリヒート状態以外でないと起動時にウォームアップ完了のフラグが
                        //0で返ってきてしまうため、その場合はスタンバイ表示にする 2012-03-28 by長野
                        case modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02:

                            //FirstDoneWarmupStsがTrueで初回のウォームアップ完了のフラグがとれていることを示す
                            if (FirstDoneWarmupSts == true)
                            {
                                lblWarmupStatus.Text = CTResources.LoadResString(12368);      //未完
                            }
                            else
                            {
                                lblWarmupStatus.Text = CTResources.LoadResString(12757);      //スタンバイ
                            }
                            break;

                        //追加2014/10/07hata_v19.51反映
                        case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:
                        case modXrayControl.XrayTypeConstants.XrayTypeSpellman:  //add Rev25.03/Rev25.02 2017/02/05 by chouno
                            //v18.00追加 byやまおか 2011/03/01 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

                            //sts = modTitan.Ti_CheckWarmUpStatus(Convert.ToInt32(cwneKV.Value));
                            //Rev25.03/Rev24.00 修正 by長野 2016/0621
                            sts = modTitan.Ti_CheckWarmUpStatus(Convert.ToInt32(ntbSetVolt.Value)); 
                            if ((sts != modTitan.WarmupConstants.WU_READY))
                            {
                                var _with10 = lblWarmupStatus;
                                switch (sts)
                                {
                                    case modTitan.WarmupConstants.WU_OFF_OVER:
                                        _with10.Text = CTResources.LoadResString(16222);    //２週以上
                                        break;
                                    case modTitan.WarmupConstants.WU_OFF_2WEEK:
                                        _with10.Text =CTResources.LoadResString(16223);     //２週
                                        break;
                                    case modTitan.WarmupConstants.WU_OFF_2DAY:
                                        _with10.Text = CTResources.LoadResString(16224);    //２日
                                        break;
                                    case modTitan.WarmupConstants.WU_OFF_1DAY:
                                        _with10.Text = CTResources.LoadResString(16225);    //１日
                                        break;
                                    case modTitan.WarmupConstants.WU_OFF_6HOUR:
                                        _with10.Text = CTResources.LoadResString(16227);    //６時間
                                        break;
                                    case modTitan.WarmupConstants.WU_OFF_4HOUR:
                                        _with10.Text = CTResources.LoadResString(16228);    //４時間
                                        break;
                                    case modTitan.WarmupConstants.WU_OFF_2HOUR:
                                        _with10.Text = CTResources.LoadResString(16229);    //２時間
                                        break;
                                }
                            }
                            break;

                        default:

                            lblWarmupStatus.Text = CTResources.LoadResString(12368);          //未完
                            break;
                    }
                    break;

                case modXrayControl.XrayWarmUpConstants.XrayWarmUpFailed:       //v11.5追加 by 間々田 2006/04/10

                    //.Caption = "失敗"                                   '失敗
                    lblWarmupStatus.Text = CTResources.LoadResString(20107);      //ストリングテーブル化 'v17.60 by長野 2011/05/22
                    break;
            }

            //段階ウォームアップ用ステップチェックボックスを無効化   'v19.02変更 上から移動してきた byやまおか 2012/07/09
            chkStepWU.Enabled = (!Convert.ToBoolean(lblWarmupStatus.Text == StringTable.GC_Xray_WarmUp));


            //ウォームアップの残り時間を表示
            int wurs = 0;
            switch (modXrayControl.XrayType)
            {
                //Case XrayTypeHamaL9181, XrayTypeHamaL9421                
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:        //v16.30 02T追加 byやまおか
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181_02:        //追加2014/10/07hata_v19.51反映

                    //浜ホトの場合、新規追加のプロパティ Up_XrayWarmupSWSで残時間を判断する。
                    //また残時間表示は「約＊＊分」という表示とする。（約を付け、秒は表示しない）
                    lblWrestTimeM.Text = CTResources.LoadResString(10830) + modXrayControl.GetWrestTimeHama().ToString(); //LoadResString(10830):約
                    break;

                //追加2014/10/07hata_v19.51反映
                case modXrayControl.XrayTypeConstants.XrayTypeKevex:
                    lblWrestTimeM.Text = modXrayControl.XrayControl.Up_Wrest_TimeM.ToString();
                    lblWrestTimeS.Text = modXrayControl.XrayControl.Up_Wrest_TimeS.ToString();
                    break;

                //追加2014/10/07hata_v19.51反映
                case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:
                case modXrayControl.XrayTypeConstants.XrayTypeSpellman:  //add Rev25.03/Rev25.02 2017/02/05 by chouno
                    //v18.00追加 byやまおか 2011/03/14 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    wurs = modTitan.Ti_UpdateWarmupRestSec();
                    lblWrestTimeS.Text =(wurs % 60).ToString("00");         //秒
                    lblWrestTimeM.Text = Convert.ToString(wurs / 60);       //分
                    break;
            }

            //焦点：ウォームアップが完了かつX線OFFの時だけ焦点ボタンを使用可にする
            switch (modXrayControl.XrayType)
            {
                //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421
                //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL10801   'v15.10変更 byやまおか 2009/10/07
                //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421     'v16.01/v17.00変更 byやまおか 2010/02/10
                //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL9421_02T  'v16.30 02T追加 byやまおか 2010/05/21
                case modXrayControl.XrayTypeConstants.XrayTypeKevex:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9191:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02:         //v17.71 追加 by長野 2012/03/14
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181_02:        //追加2014/10/07hata_v19.51反映
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10711:          ////Rev23.10 追加 by長野 2015/10/01

                    fraFocus.Enabled = (myXrayWarmUp == modXrayControl.XrayWarmUpConstants.XrayWarmUpComplete) &&
                                       (myXrayOn == modCT30K.OnOffStatusConstants.OffStatus);


                    int xfocus = modXrayControl.XrayControl.Up_Focussize - 1;
                    if (xfocus >= 0)
                        modLibrary.SetCmdButton(cmdFocus, Convert.ToInt32(xfocus), ControlEnabled: true);
                    break;
            }
        }


        //*******************************************************************************
        //機　　能： ウォームアップステータスラベル変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  2006/02/06  (SI3)間々田  新規作成
        //*******************************************************************************
        private void lblWarmupStatus_Change(object sender, EventArgs e)
        {
            Label label = sender as Label;
			if (label == null) return;	

       		bool IsLastStep = false;
            int Val1 = 0;
            int StepWUkV;

            //メッセージクリア
            //Unload frmMessage
            modCT30K.HideMessage();     //変更 by 間々田 2009/08/24

            if (label.Text != null)
            {
                if (label.Text == StringTable.GC_Xray_WarmUp)       //ｳｫｰﾑｱｯﾌﾟ中
                {
                    lblWarmupStatus.BackColor = Color.Red;

                    //自動的にウォームアップが始まった場合
                    if (modXrayControl.WarmupStartAuto)
                    {
                        //frmMessage.lblMessage = "ウォームアップが必要になりましたので完了するまでしばらくお待ち下さい。"
                        //ShowMessage "ウォームアップが必要になりましたので完了するまでしばらくお待ち下さい。"  '変更 by 間々田 2009/08/24
                        modCT30K.ShowMessage(CTResources.LoadResString(20108));       //ストリングテーブル化 'v17.60 by長野 2011/05/22

                        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                        //ElseIf XrayType = XrayTypeViscom Then
                        //    WarmupStartAuto = True
                        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
                    }

                    if (!CTSettings.detectorParam.Use_FlatPanel)   //v17.00 if追加 byやまおか 2010/02/23
                    {
                        //変更2014/10/07hata_v19.51反映
                        //v17.30 検出器切替用に条件式を追加 by 長野 2010-09-03
                        if (CTSettings.SecondDetOn & mod2ndDetctor.IsDet2mode)
                        {
                            modSeqComm.SeqBitWrite("TVIIPowerOff", true);   //ウォームアップ開始時、I.I.電源をOFFする
                        }
                        else
                        {
                            modSeqComm.SeqBitWrite("IIPowerOff", true);     //ウォームアップ開始時、I.I.電源をOFFする
                        }
                    }
                }
                else if (label.Text == CTResources.LoadResString(12368))      //未完
                {  
                    lblWarmupStatus.BackColor = Color.Yellow;
                    //ウォームアップの段階をクリア   'v19.02追加 byやまおか 2012/07/20
                    //StepWU_CurrentNum = 0  'ここでクリアしない 'v19.02削除 byやまおか 2012/07/23
                }

                //追加2014/10/07hata_v19.51反映
                else if (label.Text == CTResources.LoadResString(16222)) lblWarmupStatus.BackColor = Color.Yellow;    //２週以上
                else if (label.Text == CTResources.LoadResString(16223)) lblWarmupStatus.BackColor = Color.Yellow;    //２週
                else if (label.Text == CTResources.LoadResString(16224)) lblWarmupStatus.BackColor = Color.Yellow;    //２日
                else if (label.Text == CTResources.LoadResString(16225)) lblWarmupStatus.BackColor = Color.Yellow;    //１日
                else if (label.Text == CTResources.LoadResString(16227)) lblWarmupStatus.BackColor = Color.Yellow;    //６時間
                else if (label.Text == CTResources.LoadResString(16228)) lblWarmupStatus.BackColor = Color.Yellow;    //４時間
                else if (label.Text == CTResources.LoadResString(16229)) lblWarmupStatus.BackColor = Color.Yellow;    //２時間

                else if (label.Text == CTResources.LoadResString(12370)) lblWarmupStatus.BackColor = Color.Yellow;    //１日
                else if (label.Text == CTResources.LoadResString(12371)) lblWarmupStatus.BackColor = Color.Yellow;    //２日
                else if (label.Text == CTResources.LoadResString(12182)) lblWarmupStatus.BackColor = Color.Yellow;    //２週
                else if (label.Text == CTResources.LoadResString(12183)) lblWarmupStatus.BackColor = Color.Yellow;    //３週
                else if (label.Text == CTResources.LoadResString(12372)) lblWarmupStatus.BackColor = Color.Yellow;    //１ヶ月
                else if (label.Text == CTResources.LoadResString(12373)) lblWarmupStatus.BackColor = Color.Yellow;    //３ヶ月
                else if (label.Text == CTResources.LoadResString(12060))      //完了
                {
                    lblWarmupStatus.BackColor = Color.Lime;
                    //If byEvent Then

                    //ウォームアップ未完了→完了の変化時の処理
                    #region 書き換え    //2014/10/07hata_v19.51反映
                    //変更2014/10/07hata_v19.51反映
                    //#######################################################################################################
                    //段階ウォームアップ対応（ここから）     'v19.02追加 byやまおか 2012/07/05

                    //段階ウォームアップのときは
                    if (chkStepWU.CheckState == CheckState.Checked)
                    {
 
                        switch (modXrayControl.XrayType)
                        {
                            //浜ホト230kVの場合
                            case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:
                            case modXrayControl.XrayTypeConstants.XrayTypeHamaL12721://Rev23.10 浜ホト300kV追加 by長野 2015/11/14
                                //最終ステップ？
                                //IsLastStep = (myStepWU_Num = STEPWU_NUM)
                                IsLastStep = (StepWU_CurrentNum == CTSettings.iniValue.STEPWU_NUM);    //v19.02変更 Property化 byやまおか 2012/07/20

                                //1～4段階(最終段階以外)の終了処理
                                if (!IsLastStep)
                                {
                                    //少し待つ
                                    modCT30K.PauseForDoEventsSleep(2);

                                    //制御器で状態確認(ウォームアップ完了)
                                    if (modXrayControl.XrayControl.Up_Warmup == (int)modXrayControl.XrayWarmUpConstants.XrayWarmUpComplete)
                                    {
                                        //問題なければもう少し待つ
                                        modCT30K.PauseForDoEventsSleep(5);

                                        //待ってる間に停止された？
                                        //If (myStepWU_Num <> 0) Then
                                        if (StepWU_CurrentNum != 0)     //v19.02変更 Property化 byやまおか 2012/07/20
                                        {
                                            //次の段階
                                            //myStepWU_Num = myStepWU_Num + 1
                                            //StepWUkV = STEPWU_KV(myStepWU_Num)
                                            StepWU_CurrentNum = StepWU_CurrentNum + 1;                  //v19.02変更 Property化 byやまおか 2012/07/20
                                            StepWUkV = CTSettings.iniValue.STEPWU_KV[StepWU_CurrentNum];     //v19.02変更 Property化 byやまおか 2012/07/20

                                            //ウォームアップ用管電圧のセット
                                            UC_XrayCtrl.XrayCMV_Set(Convert.ToInt32(StepWUkV));

                                            //Up_XrayStatusSMVプロパティがValueに変化するまで待つ
                                            if (!modXrayControl.WaitXrayCMV_Ready(Convert.ToInt32(StepWUkV))) return;

                                            //ウォームアップ完了しても、ステータスが未完になってしまう対策
                                            System.Threading.Thread.Sleep(3000);    //3秒待つ(確実に設定されてからWUPを開始する)

                                            //最終段階以外はWUP、最終段階はWUP2を実行する
                                            //Val1 = IIf((myStepWU_Num = STEPWU_NUM), 3, 1)    '1:WUP 2:WUP1 3:WUP2
                                            Val1 = ((StepWU_CurrentNum == CTSettings.iniValue.STEPWU_NUM) ? 3 : 1);     //v19.02変更 Property化 byやまおか 2012/07/20

                                            //次の段階のウォームアップ開始
                                            UC_XrayCtrl.XrayWarmUp_Set(Val1);
                                        }
                                    }
                                    //異常
                                    else
                                    {
                                        //ウォームアップの段階をクリア
                                        //myStepWU_Num = 0
                                        StepWU_CurrentNum = 0;      //v19.02変更 Property化 byやまおか 2012/07/20
                                    }
                                }
                                //最終段階の終了処理
                                else if (IsLastStep)
                                {
                                    //ウォームアップの段階をクリア
                                    //myStepWU_Num = 0
                                    StepWU_CurrentNum = 0;   //v19.02変更 Property化 byやまおか 2012/07/20
                                    //Rev20.00 test 追加 by長野 2015/02/09
                                    if (WUstsEvntLock == false)
                                    {
                                        //Rev22.00 Rev21.01の反映
                                        //modXrayControl.SetCurrent(10);
                                        bakWUPXrayCondition();
                                    }
                                }
                                break;

                            //浜ホト230kV以外
                            default:
                                //何もしない
                                break;
                        }
                    }
                    //段階ウォームアップじゃないとき
                    else
                    {
                        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                        //If XrayType <> XrayTypeViscom Then  'v11.5追加 by 間々田 2006/06/08
                        //    XrayOff                         'ウォームアップ未完了→完了の変化時、Ｘ線をOFFする
                        //End If
                        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

                        //ウォームアップ未完了→完了の変化時、Ｘ線をOFFする  'v18.00変更 byやまおか 2011/03/06 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                        switch (modXrayControl.XrayType)
                        {
                            //ViscomとTitanは何もしない
                            case modXrayControl.XrayTypeConstants.XrayTypeViscom:
                            case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:
                            case modXrayControl.XrayTypeConstants.XrayTypeSpellman:  //add Rev25.03/Rev25.02 2017/02/05 by chouno
                                break;
                            //その他はOFFする
                            //Rev20.00 test by長野 2015/02/09
                            case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:
                            case modXrayControl.XrayTypeConstants.XrayTypeHamaL12721://Rev23.10 追加 by長野 2015/10/01
                            //Rev20.00 test 追加 by長野 2015/02/09
                            if (WUstsEvntLock == false)
                            {
                               //Rev22.00 Rev21.01の反映
                               //modXrayControl.SetCurrent(10);
                                bakWUPXrayCondition();
                            }
                            break;
                            default:
                                modXrayControl.XrayOff();

                                break;
                        }

                        //ウォームアップ未完了→完了の変化時、I.I.電源をONする
                        //v17.20 検出器切替用に条件式を追加 by 長野 2010-09-03
                        if (CTSettings.SecondDetOn & mod2ndDetctor.IsDet2mode)
                        {
                            modSeqComm.SeqBitWrite("TVIIPowerOn", true);
                        }
                        else
                        {
                            modSeqComm.SeqBitWrite("IIPowerOn", true);
                        }

                        //ウォームアップ開始時に設定管電圧を変更した場合元に戻す
                        if (!(modXrayControl.BackXrayVoltSet < 0))
                        {
                            //SetKVMA BackXrayVoltSet
                            modXrayControl.SetVolt(modXrayControl.BackXrayVoltSet);   //v15.0変更 by 間々田 2009/04/07
                            modXrayControl.BackXrayVoltSet = -1;
                        }
                        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                        //ウォームアップ後のフィラメント調整ありの場合   'v11.5追加 by 間々田 2006/05/08
                        //else if (modXrayControl.FilamentAdjustAfterWarmup)
                        //{
                        //    SendCode(CODE_XRAY_WFIL);    //フィラメント調整「開始」
                        //    modXrayControl.FilamentAdjustAfterWarmup = false;
                        //}
                        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

                        //ウォームアップの段階をクリア
                        StepWU_CurrentNum = 0;  //v19.02追加 byやまおか 2012/07/20                                            

                    }
                    //段階ウォームアップ対応（ここまで）     'v19.02追加 byやまおか 2012/07/05

                    //End If
                    //#######################################################################################################
                    #endregion 書き換え    //2014/10/07hata_v19.51反映
                

                    //Rev20.00 上記と下記で処理が2重で記述されているる。上記の畑S殿の記述を優先し、ここはコメントアウト by長野 2014/12/15
                    //段階ウォームアップ対応（ここから）     'v19.02追加 byやまおか 2012/07/05

                    ////段階ウォームアップのときは
                    //if ((chkStepWU.CheckState == CheckState.Checked)) {


                    //    //v19.18 追加 byやまおか 2013/09/26
                    //    //最終ステップ?
                    //    IsLastStep = (StepWU_CurrentNum == CTSettings.iniValue.STEPWU_NUM);

                    //    switch (modXrayControl.XrayType) {

                    //        //浜ホト230kVの場合
                    //        case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:

                    //            //                                                        'v19.18 削除(Selectの外へ移動) byやまおか 2013/09/26
                    //            //                                                        'Dim IsLastStep  As Boolean
                    //            //
                    //            //                                                        '最終ステップ？
                    //            //                                                        'IsLastStep = (myStepWU_Num = STEPWU_NUM)
                    //            //                                                        IsLastStep = (StepWU_CurrentNum = STEPWU_NUM)   'v19.02変更 Property化 byやまおか 2012/07/20

                    //            //1～4段階(最終段階以外)の終了処理
                    //            if ((!IsLastStep)) {

                    //                //少し待つ
                    //                modCT30K.PauseForDoEventsSleep(2);

                    //                //制御器で状態確認(ウォームアップ完了)
                    //                if ((modXrayControl.XrayControl.Up_Warmup == (int)modXrayControl.XrayWarmUpConstants.XrayWarmUpComplete)) 
                    //                {
                    //                    //問題なければもう少し待つ
                    //                    modCT30K.PauseForDoEventsSleep(5);

                    //                    //待ってる間に停止された？
                    //                    //If (myStepWU_Num <> 0) Then
                    //                    if ((StepWU_CurrentNum != 0))   //v19.02変更 Property化 byやまおか 2012/07/20
                    //                    {
                    //                        //次の段階
                    //                        //myStepWU_Num = myStepWU_Num + 1
                    //                        //StepWUkV = STEPWU_KV(myStepWU_Num)
                    //                        StepWU_CurrentNum = StepWU_CurrentNum + 1;                          //v19.02変更 Property化 byやまおか 2012/07/20
                    //                        StepWUkV = CTSettings.iniValue.STEPWU_KV[StepWU_CurrentNum];        //v19.02変更 Property化 byやまおか 2012/07/20

                    //                        //ウォームアップ用管電圧のセット
                    //                        UC_XrayCtrl.XrayCMV_Set(Convert.ToInt32(StepWUkV));

                    //                        //Up_XrayStatusSMVプロパティがValueに変化するまで待つ
                    //                        if (!modXrayControl.WaitXrayCMV_Ready(Convert.ToInt32(StepWUkV))) return;

                    //                        //ウォームアップ完了しても、ステータスが未完になってしまう対策
                    //                        System.Threading.Thread.Sleep(3000);    //3秒待つ(確実に設定されてからWUPを開始する)

                    //                        //最終段階以外はWUP、最終段階はWUP2を実行する
                    //                        //Val1 = IIf((myStepWU_Num = STEPWU_NUM), 3, 1)    '1:WUP 2:WUP1 3:WUP2
                    //                        Val1 = ((StepWU_CurrentNum == CTSettings.iniValue.STEPWU_NUM) ? 3 : 1);     //v19.02変更 Property化 byやまおか 2012/07/20

                    //                        //次の段階のウォームアップ開始
                    //                        UC_XrayCtrl.XrayWarmUp_Set(Val1);
                    //                    }

                    //                //異常
                    //                } 
                    //                else 
                    //                {
                    //                    //ウォームアップの段階をクリア
                    //                    //myStepWU_Num = 0
                    //                    StepWU_CurrentNum = 0;      //v19.02変更 Property化 byやまおか 2012/07/20
                    //                }

                    //            //最終段階の終了処理
                    //            } else if ((IsLastStep)) {

                    //                //ウォームアップの段階をクリア
                    //                //myStepWU_Num = 0
                    //                StepWU_CurrentNum = 0;          //v19.02変更 Property化 byやまおか 2012/07/20
                    //            }
                    //            break;
                        

                    //        #region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                    //        ////Viscomの場合 'v19.18 追加 byやまおか 2013/09/26
                    //        //case modXrayControl.XrayTypeConstants.XrayTypeViscom:

                    //        //    //最終段階の終了処理
                    //        //    if ((IsLastStep)) {

                    //        //        //ウォームアップ開始時に設定管電圧を変更した場合は元に戻す
                    //        //        if (!(modXrayControl.BackXrayVoltSet < 0))
                    //        //        {

                    //        //            modXrayControl.SetVolt(modXrayControl.BackXrayVoltSet);
                    //        //            modXrayControl.BackXrayVoltSet = -1;

                    //        //            //ウォームアップ後のフィラメント調整ありの場合
                    //        //        }
                    //        //        else if (modXrayControl.FilamentAdjustAfterWarmup)
                    //        //        {

                    //        //            modViscom.SendCode(CODE_XRAY_WFIL);
                    //        //            modXrayControl.FilamentAdjustAfterWarmup = false;

                    //        //        }

                    //        //    }
                    //        //    break;
                    //        #endregion  //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

                    //        //浜ホト230kV以外
                    //        default:
 							    
                    //            //何もしない
                    //            break;  
                    //    }

                    //    //ウォームアップ未完了→完了の変化時I.I.電源をONにする v19.18 byやまおか 2013/09/26
                    //    if (CTSettings.SecondDetOn & mod2ndDetctor.IsDet2mode) 
                    //    {
                    //        modSeqComm.SeqBitWrite("TVIIPowerOn", true);
                    //    }
                    //    else 
                    //    {
                    //        modSeqComm.SeqBitWrite("IIPowerOn", true);
                    //    }

                    ////段階ウォームアップじゃないとき
                    //} else {

                    //    //If XrayType <> XrayTypeViscom Then  'v11.5追加 by 間々田 2006/06/08
                    //    //    XrayOff                         'ウォームアップ未完了→完了の変化時、Ｘ線をOFFする
                    //    //End If
                    //    //ウォームアップ未完了→完了の変化時、Ｘ線をOFFする  'v18.00変更 byやまおか 2011/03/06 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    //    switch (modXrayControl.XrayType) {
                    //        //ViscomとTitanは何もしない
                    //        case modXrayControl.XrayTypeConstants.XrayTypeViscom:
                    //        case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:
                    //            break;
                    //        //その他はOFFする
                    //        default:
                    //            modXrayControl.XrayOff();
                    //            break;
                    //    }

                    //    //ウォームアップ未完了→完了の変化時、I.I.電源をONする
                    //    //v17.20 検出器切替用に条件式を追加 by 長野 2010-09-03
                    //    if (CTSettings.SecondDetOn & mod2ndDetctor.IsDet2mode) 
                    //    {
                    //        modSeqComm.SeqBitWrite("TVIIPowerOn", true);
                    //    } 
                    //    else
                    //    {
                    //        modSeqComm.SeqBitWrite("IIPowerOn", true);
                    //    }

                    //    //ウォームアップ開始時に設定管電圧を変更した場合元に戻す
                    //    if (!(modXrayControl.BackXrayVoltSet < 0))
                    //    {
                    //        //SetKVMA BackXrayVoltSet
                    //        modXrayControl.SetVolt(modXrayControl.BackXrayVoltSet);
                    //        //v15.0変更 by 間々田 2009/04/07
                    //        modXrayControl.BackXrayVoltSet = -1;

                    //        //ウォームアップ後のフィラメント調整ありの場合   'v11.5追加 by 間々田 2006/05/08
                    //    }
                    //    else if (modXrayControl.FilamentAdjustAfterWarmup)
                    //    {
                    //        #region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                    //        //SendCode(CODE_XRAY_WFIL);						    //フィラメント調整「開始」
                    //        #endregion  //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
                    //        modXrayControl.FilamentAdjustAfterWarmup = false;
                    //    }

                    //    //ウォームアップの段階をクリア
                    //    StepWU_CurrentNum = 0;					    //v19.02追加 byやまおか 2012/07/20

                    //}
                    ////段階ウォームアップ対応（ここまで）     'v19.02追加 byやまおか 2012/07/05
                    ////End If
                    ////書き換え    //2014/10/07hata_v19.51反映（ここまで）
                    

                }
                //Case "失敗":               .BackColor = vbRed       '失敗   v11.5追加 by 間々田 2006/04/10
                else if (label.Text == CTResources.LoadResString(20107)) lblWarmupStatus.BackColor = Color.Red;   //ストリングテーブル化 'v17.60 by長野 2011/05/22
            }

            lblWarmupStatus.Refresh();

            //残り時間フレーム：ウォームアップが完了でない場合のみ表示する
            switch (modXrayControl.XrayType)
            {
                //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9421
                case modXrayControl.XrayTypeConstants.XrayTypeKevex:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:
                case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:          //v19.50 GeTitan追加 by長野 2013/11/20//追加2014/10/07hata_v19.51反映
                case modXrayControl.XrayTypeConstants.XrayTypeSpellman:  //add Rev25.03/Rev25.02 2017/02/05 by chouno
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181_02:     //追加2014/10/07hata_v19.51反映

                    fraRestTime.Visible = (lblWarmupStatus.Text != CTResources.LoadResString(12060));
                    break;

                //v19.50 条件が同じなので統合する by長野 2013/11/20
                //             Case XrayTypeGeTitan    'v18.00追加 byやまおか 2011/03/01 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                //                fraRestTime.Visible = (.Caption <> LoadResString(12060))
            
            }

            //ウォームアップ「開始」「停止」ボタンの切り替え     '追加 2009/08/17
            cmdWarmupStart.Text = CTResources.LoadResString((lblWarmupStatus.Text == StringTable.GC_Xray_WarmUp) ? StringTable.IDS_btnStop : StringTable.IDS_btnStart);

            //v17.60 文字長が長い場合フォントサイズを小さくする by　長野 2011/05/30
            lblWarmupStatus.Font = new Font(lblWarmupStatus.Font.Name, ((Winapi.lstrlen(lblWarmupStatus.Text.Trim()) > 12) ? 10 : 11));

            //Ｘ線情報をファイルに書き込む   'v11.5追加 by 間々田 2006/06/07
            modXrayinfo.WriteXrayInfo();
        }


        //*******************************************************************************
        //機　　能： Ｘ線アベイラブル状態
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [I/O] Boolean   True:範囲内, False:範囲外
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  2006/02/17  (SI3)間々田  新規作成
        //           v11.5  2006/04/04  (WEB)間々田  Viscom対応
        //*******************************************************************************
        private bool IsXrayAvailable()
        {
            bool functionReturnValue = false;

            switch (modXrayControl.XrayType)
            {
                //東芝 EXM2-150用
                case modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150:
                    //SeqComm.exeのプロパティを呼び出す
                    var _with12 = modSeqComm.MySeq;
                    functionReturnValue = modLibrary.InRange(ntbActVolt.Value, _with12.stsEXMTVSet - 3, _with12.stsEXMTVSet + 3) & 
                                          modLibrary.InRange(_with12.stsEXMTC, _with12.stsEXMTCSet - 30, _with12.stsEXMTCSet + 30);
                    break;

                //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                //        'Viscom製用         'v11.5追加 by 間々田 2006/04/04
                //        Case XrayTypeViscom
                //            ''IsXrayAvailable = InRange(GetTargetU(), GetFeedbackU() - 3, GetFeedbackU() + 3) And _
                //            ''                  InRange(GetTargetI(), GetFeedbackI() - 5, GetFeedbackI() + 5)
                //            ''v11.5変更 by 間々田 2006/06/13
                //            'IsXrayAvailable = InRange(cwneActKV.Value, GetTargetU() - 3, GetTargetU() + 3) And _
                //            '                  InRange(cwneActMA.Value, GetTargetI() - 5, GetTargetI() + 5)
                //
                //            ''v11.5変更 by 間々田 2006/07/12
                //            'If cwneSetCurrent.Value > cwneMA.Maximum / 2 Then
                //            '    IsXrayAvailable = InRange(cwneActKV.Value, cwneSetVolt.Value - 3, cwneSetVolt.Value + 3) And _
                //            '                      InRange(cwneActMA.Value, cwneSetCurrent.Value - 10, cwneSetCurrent.Value + 10)
                //            'Else
                //            '    IsXrayAvailable = InRange(cwneActKV.Value, cwneSetVolt.Value - 3, cwneSetVolt.Value + 3) And _
                //            '                      InRange(cwneActMA.Value, cwneSetCurrent.Value - 5, cwneSetCurrent.Value + 5)
                //            '
                //            'End If
                //
                //            'v12.01変更 by 間々田 2006/12/04
                //            'If ntbSetCurrent.Value > cwneMA.Maximum * 0.8 Then
                //            '    IsXrayAvailable = InRange(ntbActVolt.Value, ntbSetVolt.Value - 3, ntbSetVolt.Value + 3) And _
                //            '                      InRange(ntbActCurrent.Value, ntbSetCurrent.Value - 20, ntbSetCurrent.Value + 20)
                //            'ElseIf ntbSetCurrent.Value > cwneMA.Maximum / 2 Then
                //            '    IsXrayAvailable = InRange(ntbActVolt.Value, ntbSetVolt.Value - 3, ntbSetVolt.Value + 3) And _
                //            '                      InRange(ntbActCurrent.Value, ntbSetCurrent.Value - 10, ntbSetCurrent.Value + 10)
                //            'Else
                //            '    IsXrayAvailable = InRange(ntbActVolt.Value, ntbSetVolt.Value - 3, ntbSetVolt.Value + 3) And _
                //            '                      InRange(ntbActCurrent.Value, ntbSetCurrent.Value - 8, ntbSetCurrent.Value + 8)
                //            '
                //            'End If
                //
                //            'v15.0変更 by 間々田 2009/08/17 15μA以下の場合は管電圧と時間でアベイラブルにする
                //            If ntbSetCurrent.Value <= 15 Then
                //                IsXrayAvailable = InRange(ntbActVolt.Value, ntbSetVolt.Value - 3, ntbSetVolt.Value + 3) And _
                //                                  DateDiff("s", TimeAtXrayOn, Now) > XrayOnElapsedTime
                //            ElseIf ntbSetCurrent.Value > cwneMA.Maximum * 0.8 Then
                //                IsXrayAvailable = InRange(ntbActVolt.Value, ntbSetVolt.Value - 3, ntbSetVolt.Value + 3) And _
                //                                  InRange(ntbActCurrent.Value, ntbSetCurrent.Value - 20, ntbSetCurrent.Value + 20)
                //            ElseIf ntbSetCurrent.Value > cwneMA.Maximum / 2 Then
                //                IsXrayAvailable = InRange(ntbActVolt.Value, ntbSetVolt.Value - 3, ntbSetVolt.Value + 3) And _
                //                                  InRange(ntbActCurrent.Value, ntbSetCurrent.Value - 10, ntbSetCurrent.Value + 10)
                //            Else
                //                IsXrayAvailable = InRange(ntbActVolt.Value, ntbSetVolt.Value - 3, ntbSetVolt.Value + 3) And _
                //                                  InRange(ntbActCurrent.Value, ntbSetCurrent.Value - 8, ntbSetCurrent.Value + 8)
                //
                //            End If
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''


                //Titan用            'v18.00追加 byやまおか 2011/03/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:
                case modXrayControl.XrayTypeConstants.XrayTypeSpellman:  //add Rev25.03/Rev25.02 2017/02/05 by chouno
                    functionReturnValue = Convert.ToBoolean(modTitan.Ti_CheckAvailabled() ==1 );

//#if DebugOn　
//Rev23.10 変更 by長野 2015/10/02
//Rev23.20 コメントアウト by長野 2015/12/21
//#if XrayDebugOn
//                functionReturnValue = (modXrayControl.XrayControl.Up_X_Avail == 1);
//#endif

                    break;


                //その他
                default:
                    //FeinFocus.exeのプロパティを呼び出す
                    functionReturnValue = (modXrayControl.XrayControl.Up_X_Avail == 1);
                    break;
            }

            return functionReturnValue;
        }


        //*******************************************************************************
        //機　　能： Ｘ線出力管電圧変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： Viscom専用処理
        //
        //履　　歴： v11.5 2006/05/08  (WEB)間々田   新規作成
        //*******************************************************************************
        //Private Sub cwneActKV_ValueChanged(Value As Variant, PreviousValue As Variant, ByVal OutOfRange As Boolean)
        //
        //
        //End Sub

        #region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''  //Viscom用
        //private void ntbActVolt_ValueChanged(System.Object Sender, NumTextBox.ValueChangedEventArgs e)
        //{
        //    object PreviousValue = e.PreviousValue;

        //    //Viscomの場合
        //    int add_kv = 0;
        //    int viscom_wu_maxkv = 0;
        //    bool IsLastStep = false;
        //    int nowstep_kv = 0;

        //    //ウォームアップのとき何kVまで上がる？ '+何kVまでウォームアップする？
        //    if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeViscom)
        //    {

        //        //ウォーミングアップ中の場合
        //        if (modXrayControl.XrayWarmUp() == modXrayControl.XrayWarmUpConstants.XrayWarmUpNow)
        //        {

        //            //出力管電圧の最大値を記憶
        //            modXrayControl.XrayMaxFeedBackVolt = modLibrary.MaxVal(modXrayControl.XrayMaxFeedBackVolt, (int)ntbActVolt.Value);

        //            //'出力管電圧が設定管電圧＋10kVを超えた時、Ｘ線をオフする
        //            //If ntbActVolt.Value > (XrayVoltAtWarmingup + 10) Then XrayOff

        //            //段階ウォームアップ対応(ここから)   'v17.72/v19.02変更 byやまおか 2012/05/17
        //            viscom_wu_maxkv = 228;

        //            //段階ウォームアップのときは
        //            if ((chkStepWU.CheckState == System.Windows.Forms.CheckState.Checked))
        //            {


        //                //最終ステップ？
        //                //IsLastStep = (myStepWU_Num = STEPWU_NUM)
        //                IsLastStep = (StepWU_CurrentNum == CTSettings.iniValue.STEPWU_NUM);     //v19.02変更 Property化 byやまおか 2012/07/20

        //                //ステップ管電圧の設定
        //                //nowstep_kv = IIf(STEPWU_KV(myStepWU_Num) <= cwneKV.Maximum, STEPWU_KV(myStepWU_Num), cwneKV.Maximum)
        //                nowstep_kv = (CTSettings.iniValue.STEPWU_KV[StepWU_CurrentNum] <= cwneKV.Maximum ? CTSettings.iniValue.STEPWU_KV[StepWU_CurrentNum] : (int)cwneKV.Maximum);     //v19.02変更 Property化 byやまおか 2012/07/20

        //                //Viscomは228kVまでしか上がらないことを考慮する
        //                if ((nowstep_kv + 10 >= viscom_wu_maxkv))
        //                {
        //                    //add_kv = 3
        //                    add_kv = 10;    //区別をなくす 'v19.18/'v17.721 byやまおか 2012/11/05
        //                }
        //                else
        //                {
        //                    add_kv = 10;
        //                }

        //                //1～4段階(最終段階以外)の終了処理(設定値+add_kvを超えたとき)
        //                if (((!IsLastStep) & (ntbActVolt.Value >= nowstep_kv + add_kv)))
        //                {

        //                    //ひとまずX線をオフする
        //                    modXrayControl.XrayOff();

        //                    //v19.18/v19.721 追加(下から移動してきた) byやまおか 2012/11/05
        //                    //出力管電圧が設定最大管電圧を超えた場合、ウォーミングアップ設定管電圧を設定最大管電圧にする
        //                    if (ntbActVolt.Value > cwneKV.Maximum)
        //                    {
        //                        modXrayControl.XrayVoltAtWarmingup = (int)cwneKV.Maximum;
        //                        cwneWarmupSetVolt.Value = modXrayControl.XrayVoltAtWarmingup;
        //                    }

        //                    //少し待つ
        //                    modCT30K.PauseForDoEventsSleep(2);
        //                    //状態確認(ウォームアップ完了)
        //                    //If (CBool(ViscomState1 And XST1_WUP_READY) Or XrayWarmUpCanceled) And (myStepWU_Num <> 0) Then

        //                    if ((Convert.ToBoolean(ViscomState1 & XST1_WUP_READY) | modXrayControl.XrayWarmUpCanceled) & (StepWU_CurrentNum != 0))  //v19.02変更 Property化 byやまおか 2012/07/20
        //                    {
        //                        modCT30K.PauseForDoEventsSleep(8);
        //                        //待ってる間に停止された？
        //                        //If (myStepWU_Num <> 0) Then
        //                        if ((StepWU_CurrentNum != 0))     //v19.02変更 Property化 byやまおか 2012/07/20
        //                        {
        //                            //次の段階開始
        //                            //myStepWU_Num = myStepWU_Num + 1
        //                            //ViscomWarmupStart (STEPWU_KV(myStepWU_Num))
        //                            StepWU_CurrentNum = StepWU_CurrentNum + 1;                            //v19.02変更 Property化 byやまおか 2012/07/20
        //                            ViscomWarmupStart((STEPWU_KV(StepWU_CurrentNum)));                            //v19.02変更 Property化 byやまおか 2012/07/20
        //                        }

        //                        //異常
        //                    }
        //                    else
        //                    {
        //                        //ウォームアップの段階をクリア
        //                        //myStepWU_Num = 0
        //                        StepWU_CurrentNum = 0;  //v19.02変更 Property化 byやまおか 2012/07/20
        //                    }


        //                    //最終段階の終了処理(設定値+add_kvを超えたとき)
        //                }
        //                else if (((IsLastStep) & (ntbActVolt.Value >= CTSettings.iniValue.STEPWU_KV[CTSettings.iniValue.STEPWU_NUM] + add_kv)))
        //                {

        //                    //MAXの場合、ウォームアップの上限値に達して終了すると
        //                    //実はここを通らない可能性が高いが、特に問題ない。

        //                    //X線をオフする
        //                    modXrayControl.XrayOff();

        //                    //ウォームアップの段階をクリア
        //                    //myStepWU_Num = 0
        //                    StepWU_CurrentNum = 0;
        //                    //v19.02変更 Property化 byやまおか 2012/07/20

        //                    //v19.18/v19.721 追加(下から移動してきた) byやまおか 2012/11/05
        //                    //出力管電圧が設定最大管電圧を超えた場合、ウォーミングアップ設定管電圧を設定最大管電圧にする
        //                    if (ntbActVolt.Value > cwneKV.Maximum)
        //                    {
        //                        modXrayControl.XrayVoltAtWarmingup = (int)cwneKV.Maximum;
        //                        cwneWarmupSetVolt.Value = modXrayControl.XrayVoltAtWarmingup;
        //                    }
        //                }


        //                //通常ウォームアップ(段階ウォームアップじゃない)のときは
        //            }
        //            else
        //            {

        //                //Viscomは228kVまでしか上がらないことを考慮しない
        //                add_kv = 10;

        //                //出力管電圧が設定管電圧add_kvを超えたとき
        //                if ((ntbActVolt.Value >= (modXrayControl.XrayVoltAtWarmingup + add_kv)))
        //                {

        //                    //MAXの場合、ウォームアップの上限値に達して終了すると
        //                    //実はここを通らない可能性が高いが、特に問題ない。

        //                    //Ｘ線をオフする
        //                    modXrayControl.XrayOff();

        //                    //v19.18/v19.721 追加(下から移動してきた) byやまおか 2012/11/05
        //                    //出力管電圧が設定最大管電圧を超えた場合、ウォーミングアップ設定管電圧を設定最大管電圧にする
        //                    if (ntbActVolt.Value > cwneKV.Maximum)
        //                    {
        //                        modXrayControl.XrayVoltAtWarmingup = (int)cwneKV.Maximum;
        //                        cwneWarmupSetVolt.Value = modXrayControl.XrayVoltAtWarmingup;
        //                    }

        //                    //ウォームアップの段階をクリア
        //                    StepWU_CurrentNum = 0;      //v19.02追加 byやまおか 2012/07/20

        //                }

        //            }
        //            //段階ウォームアップ対応(ここまで)   'v17.72/v19.02変更 byやまおか 2012/05/17

        //            //v19.18/v17.721(上へ移動した) byやまおか 2013/09/26
        //            //            '出力管電圧が設定最大管電圧を超えた場合、ウォーミングアップ設定管電圧を設定最大管電圧にする
        //            //            If ntbActVolt.Value > cwneKV.Maximum Then
        //            //                XrayVoltAtWarmingup = cwneKV.Maximum
        //            //                cwneWarmupSetVolt.Value = XrayVoltAtWarmingup
        //            //            End If

        //        }

        //    }

        //}
        #endregion #region //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''//Viscom用


        //*******************************************************************************
        //機　　能： 管電圧欄：値変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cwneKV_ValueChanged(object sender, EventArgs e)
        {
            //If Not (Me.ActiveControl Is cwneKV) Then Exit Sub

            //スライダーに値を反映
            if (this.ActiveControl == cwneKV)               
            {
                //2014/11/07hata キャストの修正
                //cwsldKV.Value = (int)(cwneKV.Value / cwneKV.Increment);
                cwsldKV.Value = Convert.ToInt32(cwneKV.Value / cwneKV.Increment);
                //Rev20.00 復活 by長野 2015/01/24
                cwsldKVChange((int)cwsldKV.Value);
             }
            
        }


        //*******************************************************************************
        //機　　能： 管電流欄：値変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cwneMA_ValueChanged(object sender, EventArgs e)
        {
            //    If Not (Me.ActiveControl Is cwneMA) Then Exit Sub
            //
            //    '管電流値設定スライダーに値を反映
            //    cwsldMA.value = value

            //スライダーに反映
            if (this.ActiveControl == cwneMA)
            {
                //2014/11/07hata キャストの修正
                //cwsldMA.Value = (int)(cwneMA.Value / cwneMA.Increment);
                cwsldMA.Value = Convert.ToInt32(cwneMA.Value / cwneMA.Increment);
                //Rev20.00 復活 by長野 2015/01/24
                cwsldMAChange((int)cwsldMA.Value);
            }
        }


        //*******************************************************************************
        //機　　能： 管電圧値設定スライダー：値変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cwsldKV_ValueChanged(object sender, EventArgs e)
        {
            //管電圧欄に値を反映
            cwneKV.Value = cwsldKV.Value * cwneKV.Increment;
        }

        //*******************************************************************************
        //機　　能： 管電圧値設定スライダースクロール変更時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cwsldKV_Scroll(object sender, ScrollEventArgs e)
        {
            switch (e.Type)
            {
                case System.Windows.Forms.ScrollEventType.EndScroll:
                    //スクロールを止めた位置で実行
                    //cwsldKV.Focus();
                    cwsldKVChange(e.NewValue);
                    break;

                //case System.Windows.Forms.ScrollEventType.ThumbPosition:
                //    //スクロールを止めた位置と同じ
                //    //管電圧欄に値を反映
                //    //cwneKV.Value = cwsldKV.Value * cwneKV.Increment;
                //    break;

                //case System.Windows.Forms.ScrollEventType.ThumbTrack:
                //    //これは最初のポイント(MouseCaptureした瞬間)も出力してしまう
                //    //値を前回値と比較して実行するか／しないか処理が必要
                //    //スクロール移動中で実行
                //    cwneKV.Value = cwsldKV.Value * cwneKV.Increment;
                //  break;
            }
        }

        // 管電圧値設定スライダーの値変更終了時の処理
        //private void cwsldKV_PointerValueCommitted(object sender, MouseEventArgs e)  //TODO MouseUpイベント
        private void cwsldKVChange(int newScrollValue)
        {

            //Debug.Print("ActiveControl = " + this.ActiveControl.Name);
            //管電圧をＸ線制御器にセットする
            if ((this.ActiveControl == cwsldKV) || (this.ActiveControl == cwneKV))
            {
                cwsldKV.Enabled = false;
                cwneKV.Enabled = false;     //v15.03追加 byやまおか 2009/11/17

                //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                //FineFocusの場合は管電流も無効にする    'v17.44追加 byやまおか 2011/02/16
                //        If (XrayType = XrayTypeFeinFocus) Then
                //            cwsldMA.Enabled = False
                //            cwsldMA.ActivePointer.FILLCOLOR = vbDesktop
                //            cwneMA.Enabled = False
                //        End If
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

                modXrayControl.SetVolt((float)(cwsldKV.Value * cwneKV.Increment));

                //Rev20.00 追加 by長野 2015/02/16
                if ((pnlDummy.Visible) && (pnlDummy.Enabled)) pnlDummy.Focus();
                //Rev20.00 test コメントアウト by長野 2015/02/09
                modCT30K.PauseForDoEvents(0.5F);

                //v15.10追加(ここから) byやまおか 2009/10/15
                switch (modXrayControl.XrayType)
                {
                    //変更2014/10/07hata_v19.51反映
                    //FeinFocus.exeメソッドを使っているない場合は、何もしない 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    case modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150:
                    case modXrayControl.XrayTypeConstants.XrayTypeViscom:
                    case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:  //v18.00 Titan追加 byやまおか 2011/03/02
                    case modXrayControl.XrayTypeConstants.XrayTypeSpellman:  //add Rev25.03/Rev25.02 2017/02/05 by chouno 
                        break;
                    
                    //FeinFocus.exeメソッドを使っている場合は、ここでスライダーを有効にする
                    default:
                        cwsldKV.Enabled = true;
                        cwneKV.Enabled = true;      //v15.10追加 byやまおか 2009/12/01

                        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                        //FineFocusの場合は管電流も無効にする    'v17.44追加 byやまおか 2011/02/16
                        //                If (XrayType = XrayTypeFeinFocus) Then
                        //                    cwsldMA.Enabled = True
                        //                    cwsldMA.ActivePointer.FILLCOLOR = vbHighlight
                        //                    cwneMA.Enabled = True
                        //                End If
                        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

                        break;
                }
                //v15.10追加(ここまで) byやまおか 2009/10/15

                //Focusをスライダー以外にする
                //変更2015/01/24
                //if ((cwneKV.Visible) && (cwneKV.Enabled)) cwneKV.Focus();
                if ((pnlDummy.Visible) && (pnlDummy.Enabled)) pnlDummy.Focus();

            }
        }

        //*******************************************************************************
        //機　　能： 管電流値設定スライダー：値変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cwsldMA_ValueChanged(object sender, EventArgs e)
        {
            //Rev23.40 by長野 2016/04/05 / Rev23.21 ワッテージ制御がかかった場合でも正しく電流を表示するための対策 by長野 2016/03/10
            //Rev26.00 不要のためコメントアウト by chouno 2017/03/01
            //if (tmpMA != -1)
            //{
            //    cwsldMA.Value = (int)tmpMA;
            //    tmpMA = -1;
            //}

            //管電流欄に値を反映
            cwneMA.Value = cwsldMA.Value * cwneMA.Increment;
        }

        //*******************************************************************************
        //機　　能： 管電流値設定スライダースクロール変更時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cwsldMA_Scroll(object sender, ScrollEventArgs e)
        {
            switch (e.Type)
            {
                case System.Windows.Forms.ScrollEventType.EndScroll:
                    //スクロールを止めた位置で実行
                    //Rev20.00 test by長野 2015/02/16
                    //cwsldMA.Focus();
                    cwsldMAChange(e.NewValue);
                    break;

                //case System.Windows.Forms.ScrollEventType.ThumbPosition:
                //    //スクロールを止めた位置と同じ
                //    break;

                //case System.Windows.Forms.ScrollEventType.ThumbTrack:
                //　　//これは最初のポイント(MouseCaptureした瞬間)も出力してしまう
                //    //値を前回値と比較して実行するか／しないか処理が必要
                //    //スクロール移動中で実行
                //    break;
            }
        }

        // 管電流値設定スライダーの値変更終了時の処理
        //private void cwsldMA_PointerValueCommitted(object sender, MouseEventArgs e)  //TODO MouseUpイベント
        private void cwsldMAChange(int newScrollValue)
        {

            //管電流をＸ線制御器にセットする
            if ((this.ActiveControl == cwsldMA) || (this.ActiveControl == cwneMA))
            {
                cwsldMA.Enabled = false;
                cwneMA.Enabled = false;         //v15.03追加 byやまおか 2009/11/17

                //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                //FineFocusの場合は管電圧も無効にする    'v17.44追加 byやまおか 2011/02/16
                //        If (XrayType = XrayTypeFeinFocus) Then
                //            cwsldKV.Enabled = False
                //            cwsldKV.ActivePointer.FILLCOLOR = vbDesktop
                //            cwneKV.Enabled = False
                //        End If
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

                modXrayControl.SetCurrent((float)(cwsldMA.Value * cwneMA.Increment));

                //v15.10追加(ここから) byやまおか 2009/10/15
                switch (modXrayControl.XrayType)
                {
                    //変更2014/10/07hata_v19.51反映
                    //FeinFocus.exeメソッドを使っているない場合は、何もしない
                    //Case XrayTypeToshibaEXM2_150, XrayTypeViscom
                    //Case XrayTypeToshibaEXM2_150    'v17.701/v19.02変更 byやまおか 2012/03/27
                    case modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150:
                    case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:      //v18.00 Titan追加 byやまおか 2011/03/02  'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    case modXrayControl.XrayTypeConstants.XrayTypeSpellman:  //add Rev25.03/Rev25.02 2017/02/05 by chouno 
                        break;
                    case modXrayControl.XrayTypeConstants.XrayTypeViscom:       //v17.701/v19.02変更 byやまおか 2012/03/27
                        //Sleep 100
                        break;

                    //FeinFocus.exeメソッドを使っている場合は、ここでスライダーを有効にする
                    default:
                        //浜ホト230の場合    'v17.61追加 byやまおか 2011/09/15
                        //if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801)
                        if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801 || modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL12721) //Rev23.10 追加 by長野 2015/10/01
                        {
                            ////Rev20.00 test by長野 2015/02/09
                            //for (int cnt = 0; cnt < 3; cnt++)
                            //{
                            //    modCT30K.PauseForDoEvents(0.5F);

                            //    if (ntbSetCurrent.Value == cwsldMA.Value * cwneMA.Increment)
                            //    {
                            //        break;
                            //    }
                            //    else
                            //    {
                            //        modXrayControl.SetCurrent(10.0F, true);
                            //        modCT30K.PauseForDoEvents(0.5F);
                            //        modXrayControl.SetCurrent((float)(cwsldMA.Value * cwneMA.Increment),true);
                            //    }
                            //}

                            //制限ワットを超えると制限値に抑えられるための対応
                            if (ntbSetCurrent.Value != cwsldMA.Value * cwneMA.Increment)
                            {
                                //Rev20.00 追加 by長野 2015/02/16
                                if ((pnlDummy.Visible) && (pnlDummy.Enabled)) pnlDummy.Focus();

                                //Rev20.00 test コメントアウト by長野 2015/02/09
                                modCT30K.PauseForDoEvents(0.5F);

                                //変更2015/02/02hata_Max/Min範囲のチェック
                                //cwneMA.Value = ntbSetCurrent.Value;
                                cwneMA.Value = modLibrary.CorrectInRange(ntbSetCurrent.Value, cwneMA.Minimum, cwneMA.Maximum);

                                //Rev20.00 追加 by長野 2015/02/16
                                if ((pnlDummy.Visible) && (pnlDummy.Enabled)) pnlDummy.Focus();

                                //2014/11/07hata キャストの修正
                                //cwsldMA.Value = (int)(ntbSetCurrent.Value / cwneMA.Increment);
                                //Rev20.00 test by長野 2015/02/09
                                //if (cwsldMA.Value >= cwsldMA.Maximum && cwsldMA.Value <= cwsldMA.Minimum)
                                //Rev23.40/Rev23.21 修正 by長野 2016/03/10
                                if (cwsldMA.Value <= cwsldMA.Maximum && cwsldMA.Value >= cwsldMA.Minimum)
                                {
                                    cwsldMA.Value = Convert.ToInt32(ntbSetCurrent.Value / cwneMA.Increment);
                                }

                                //Rev23.40/Rev23.21 ワッテージ制御がかかった状態でも正しく電流を表示するための対策 by長野 2016/03/10
                                //Rev25.03 コメントアウト by chouno 2017/01/05
                                //tmpMA = cwsldMA.Value;
                            }
                        }

                        cwsldMA.Enabled = true;
                        cwneMA.Enabled = true;      //v15.10追加 byやまおか 2009/12/01

                        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                        //                'FineFocusの場合は管電圧も無効にする    'v17.44追加 byやまおか 2011/02/16
                        //                If (XrayType = XrayTypeFeinFocus) Then
                        //                    cwsldKV.Enabled = True
                        //                    cwsldKV.ActivePointer.FILLCOLOR = vbHighlight
                        //                    cwneKV.Enabled = True
                        //                End If
                        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''

                        break;
                }
                //v15.10追加(ここまで) byやまおか 2009/10/15

                //Focusをスライダー以外にするpnlDummy
                //変更2015/01/24hata
                //if ((cwneMA.Visible) && (cwneMA.Enabled)) cwneMA.Focus();
                if ((pnlDummy.Visible) && (pnlDummy.Enabled)) pnlDummy.Focus();
            }
        }

        //*******************************************************************************
        //機　　能： 焦点ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v6.0  02/08/29 (SI4)間々田      新規作成
        //*******************************************************************************
        public void cmdFocus_ClickEx()
        {
            cmdFocus_Click(cmdFocus, EventArgs.Empty);
        }

        //*******************************************************************************
        //機　　能： 焦点ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v6.0  02/08/29 (SI4)間々田      新規作成
        //*******************************************************************************
        internal void cmdFocus_Click(object sender, EventArgs e) //Rev25.03/Rev25.02 change by chouno 2017/02/14
        //private void cmdFocus_Click(object sender, EventArgs e)
        {
            if (sender as Button == null) return;

			int Index = Array.IndexOf(cmdFocus, sender);
			if (Index < 0) return;

            //Rev20.01 LMHボタンと焦点切り替え処理の2重呼び出しはさせない 追加 by長野 2015/06/03 
            if (myLMHChangeBusy == true || myFocusChangeBusy == true)
            {
                return;
            }

            //Rev20.01 LMHボタンを押したときと焦点を押したときのみ、TempSetCurrentとTempSetVoltをクリアする by長野 2015/06/03
            modXrayControl.TempSetCurrent = -1;
            modXrayControl.TempSetVolt = -1;

            //Rev20.01 追加 by長野 2015/06/03
            try
            {
                //マウスポインタを砂時計にする
                Cursor.Current = Cursors.WaitCursor;

                //Rev20.01 追加 by長野 2015/06/03
                myFocusChangeBusy = true;

                //Dim Val1    As FeinFocus.UserValueSet
                clsTActiveX.UserValueSet Val1 = new clsTActiveX.UserValueSet();	//v16.20変更 byやまおか 2010/04/21


                //変更2014/10/07hata_v19.51反映
                //Val1.m_XrayModeSet = modXrayControl.XrayControl.Up_Xcont_Mode;
                //Val1.m_XrayTimeSet = modXrayControl.XrayControl.Up_Xtimer;
                //Val1.m_XrayFocusSet = Index;                    //v9.5 追加 by 間々田 2004/09/13
                //modXrayControl.XrayControl.UserValue_Set(Val1);
                switch (modXrayControl.XrayType)
                {
                    case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:
                    case modXrayControl.XrayTypeConstants.XrayTypeSpellman:  //add Rev25.03/Rev25.02 2017/02/05 by chouno 
                        //焦点切替実行(大焦点1、小焦点0)

//#if !DebugOn
//Rev23.10 変更 by長野 2015/10/02
#if !XrayDebugOn

                        //if ((modTitan.Ti_SetFocusSize(Index - 1) == 0))
                        //Rev23.20 変更 by長野 2015/12/23
                        if ((modTitan.Ti_SetFocusSize(Index) == 0))
                        {
#endif

                            //Rev23.40 前回と異なる場合のみ更新 by長野 2016/06/19
                            if (CTSettings.mecainf.Data.xfocus != Index)
                            {
                                //mecainf（コモン）取得
                                //GetMecainf(mecainf);
                                CTSettings.mecainf.Load();

                                CTSettings.mecainf.Data.xfocus = Index;

                                //mecainf（コモン）更新
                                //PutMecainf(mecainf);
                                CTSettings.mecainf.Write();

                                //Rev24.00 追加 by長野 2016/06/01
                                //オートセンタリングをありにする
                                frmScanControl.Instance.chkInhibit[3].CheckState = System.Windows.Forms.CheckState.Unchecked;
                            }

//#if !DebugOn
//Rev23.10 変更 by長野 2015/10/02
#if !XrayDebugOn
                        }
#endif
                        break;

                    default:

                        //Rev23.10 フォーカスを記憶させる by長野 2015/11/04
                        //mecainf（コモン）取得
                        //GetMecainf(mecainf);
                        CTSettings.mecainf.Load();

                        //mecainfの焦点を変更
                        CTSettings.mecainf.Data.xfocus = Index;

                        //mecainf（コモン）更新
                        //PutMecainf(mecainf);
                        CTSettings.mecainf.Write();

                        Val1.m_XrayModeSet = modXrayControl.XrayControl.Up_Xcont_Mode;
                        Val1.m_XrayTimeSet = modXrayControl.XrayControl.Up_Xtimer;
                        //Val1.m_XrayFocusSet = Index;                    //v9.5 追加 by 間々田 2004/09/13
                        Val1.m_XrayFocusSet = Index + 1;                  //Rev20.01 変更 by長野 2015/05/20
                        modXrayControl.XrayControl.UserValue_Set(Val1);

                        //Rev20.01 変更 by長野 2015/06/03
                        Thread.Sleep(1000);

                        break;

                }
            }
            finally
            {
                //マウスポインタを砂時計にする
                Cursor.Current = Cursors.Default;
            }

            //Rev20.01 追加 by長野 2015/06/03
            myFocusChangeBusy = false;

        }

        //*******************************************************************************
        //機　　能： 「詳細...」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*******************************************************************************
        private void cmdDetail_Click(object sender, EventArgs e)
        {
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //東芝EXM2-150の場合
            //    If (XrayType = XrayTypeToshibaEXM2_150) Then
            //
            //        'Ｘ線ツール画面の表示
            //        frmXrayTool.Show vbModal
            //
            //    Else
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //「詳細...」ボタンを使用不可にする     'v11.5追加 by 間々田 2006/05/26
            cmdDetail.Enabled = false;

            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //アライメント調整を表示
            //        If XrayType = XrayTypeViscom Then   'v11.5追加 by 間々田 2006/04/10
            //            frmViscomAlignment.Show , frmCTMenu 'Viscom用アライメント調整フォームを表示
            //        Else
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            frmAdjAlignment frmAdjAlignment = frmAdjAlignment.Instance;
            frmCTMenu frmCTMenu = frmCTMenu.Instance;

            //frmAdjAlignment.Show vbModal
            //修正 by 間々田 2004/09/24 透視画像を見ながら操作するためモーダル表示しない。
            frmAdjAlignment.Show(frmCTMenu);

            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //        End If
            //
            //
            //    End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
        }


        //*******************************************************************************
        //機　　能： 「Ｘ線情報」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v9.5  04/09/08 (SI4)間々田      新規作成
        //*******************************************************************************
        private void cmdXrayInfo_Click(object sender, EventArgs e)
        {
            //frmXrayInformation frmXrayInformation = frmXrayInformation.Instance;
            frmCTMenu frmCTMenu = frmCTMenu.Instance;

            //変更2014/10/07hata_v19.51反映
            ////Ｘ線情報フォームを表示
            ////frmXrayInformation.Show vbModal
            //frmXrayInformation.Instance.Show(frmCTMenu);   //v16.01/v17.00変更 非Modalとする byやまおか 2010/02/22
            //v19.50 Titanを追加 by長野 2014/02/03
            //if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan)) {
            //Rev25.03/Rev25.02 change by chouno 2017/02/14
            if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) || (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman))
            {
                //TitanのX線詳細画面の表示
                frmTitanXrayInfomation.Instance.ShowDialog();

            }
            else
            {
                //Ｘ線情報フォームを表示
                //frmXrayInformation.Show vbModal
                if (!modLibrary.IsExistForm("frmXrayInformation"))	//追加2015/01/30hata_if文追加
                {
                    frmXrayInformation.Instance.Show(frmCTMenu);   //v16.01/v17.00変更 非Modalとする byやまおか 2010/02/22
                }
                else
                {
                    frmXrayInformation.Instance.WindowState = FormWindowState.Normal;
                    frmXrayInformation.Instance.Visible = true;
                }

            }
        }


        //*******************************************************************************
        //機　　能： ステータスラベル・変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.2  99/XX/XX   ????????      新規作成
        //           v15.0  09/07/31   (SI1)間々田   廃止。代わりにXrayStatusプロパティを使用する
        //*******************************************************************************
        //Private Sub lblXray_Change()
        //
        //    'ﾌｨﾗﾒﾝﾄ調整中、I.I.電源をOFFしていたら、ここで I.I.電源をONする 'v11.5追加 by 間々田 2006/04/25
        //    If IIPowerOffForFilamentAdjust Then
        //        SeqBitWrite "IIPowerOn", True
        //        IIPowerOffForFilamentAdjust = False
        //    End If
        //
        //    With lblXray
        //
        //        ''Ｘ線ラベルが「接続中...」の場合、Ｘ線制御画面は操作できない        'v11.5追加 2006/05/30
        //        'Me.Enabled = (.Caption <> LoadResString(12910))                    'v15.0削除 by 間々田 2009/05/09
        //
        //        Select Case .Caption
        //            Case GC_STS_STANDBY_OK:     .BackColor = vbGreen    '準備完了
        //            Case GC_STS_CPU_BUSY:       .BackColor = vbRed      '処理中
        //            Case GC_STS_Scan:           .BackColor = vbRed      '動作中
        //            Case GC_STS_STANDBY_NG:     .BackColor = vbYellow   '準備未完了
        //            Case GC_STS_BUSY:           .BackColor = vbRed      '動作中
        //            Case GC_Xray_WarmUp:        .BackColor = vbRed      'ｳｫｰﾑｱｯﾌﾟ中
        //            Case GC_Xray_On:            .BackColor = vbRed      'Ｘ線ＯＮ中
        //            Case LoadResString(12754):  .BackColor = vbRed      '過負荷
        //            Case LoadResString(12755):  .BackColor = vbYellow   'プリヒート
        //            'Case LoadResString(12756):  .BackColor = vbYellow   '扉開          'v15.0削除 by 間々田 2009/01/18 扉開ステータスをＸ線のステータスに含めない
        //            Case LoadResString(12757):  .BackColor = vbYellow   'スタンバイ
        //            'Case LoadResString(12758):  .BackColor = vbYellow   '運転準備未完  'v15.0削除 by 間々田 2009/03/17 運転準備ステータスをＸ線のステータスに含めない
        //            Case LoadResString(12909):  .BackColor = vbRed      '接続エラー
        //            Case GC_Xray_WarmUp_NG:     .BackColor = vbYellow   'ｳｫｰﾑｱｯﾌﾟ未完了
        //            Case GC_Xray_Error:         .BackColor = vbRed      '異常
        //            Case LoadResString(12910):  .BackColor = vbYellow   '接続中...
        //            'Case LoadResString(12760):  .BackColor = vbYellow   '電磁ロック開  'v15.0削除 by 間々田 2009/03/17 電磁ロック開をＸ線のステータスに含めない
        //            Case GC_STS_FLM_RUNNING:    .BackColor = vbRed      'ﾌｨﾗﾒﾝﾄ調整中   'v11.5追加 by 間々田 2006/04/04
        //                                        'ﾌｨﾗﾒﾝﾄ調整中、I.I.電源をOFFする 'v11.5追加 by 間々田 2006/04/25
        //                                        If byEvent Then
        //                                            IIPowerOffForFilamentAdjust = True
        //                                            SeqBitWrite "IIPowerOff", True
        //                                        End If
        //            Case GC_STS_CNT_RUNNING:    .BackColor = vbRed      'ｾﾝﾀﾘﾝｸﾞ中      'v11.5追加 by 間々田 2006/04/04
        //            Case GC_STS_CNT_FAILED:     .BackColor = vbRed      'ｾﾝﾀﾘﾝｸﾞ失敗    'added by 山本 2006-12-14
        //            Case GC_STS_WUP_FAILED:     .BackColor = vbRed      'ｳｫｰﾑｱｯﾌﾟ失敗   'v11.5追加 by 間々田 2006/04/26
        //            Case GC_STS_FLM_FAILED:     .BackColor = vbRed      'ﾌｨﾗﾒﾝﾄ調整失敗 'v11.5追加 by 間々田 2006/04/26
        //            Case GC_STS_WUP_NOTREADY:   .BackColor = vbYellow   'ｳｫｰﾑｱｯﾌﾟ未完   'v11.5追加 by 間々田 2006/04/26
        //            Case GC_STS_FLM_NOTREADY:   .BackColor = vbYellow   'ﾌｨﾗﾒﾝﾄ調整未完 'v11.5追加 by 間々田 2006/04/26
        //
        //            Case GC_STS_INT:            .BackColor = vbRed      '中断           'v12.01追加 by 間々田 2006/12/14
        //            Case GC_STS_STOPPED:        .BackColor = vbRed      '停止           'v12.01追加 by 間々田 2006/12/14
        //            Case GC_STS_VAC_NOTREADY:   .BackColor = vbYellow   '真空未完       'v12.01追加 by 間々田 2006/12/14
        //
        //
        //        End Select
        //
        //        '文字長が長い場合フォントサイズを小さくする
        //        .fontSize = IIf(lstrlen(Trim$(.Caption)) > 12, 10, 12)
        //
        //        .Refresh
        //
        //    End With
        //
        //End Sub

        private void cwneWarmupSetVolt_ValueChanged(object sender, EventArgs e)
        {
            //ウォームアップ用管電圧のセット     'added by 山本 2006-12-13
            //2014/11/07hata キャストの修正
            //modXrayControl.XrayVoltAtWarmingup = (int)cwneWarmupSetVolt.Value;
            modXrayControl.XrayVoltAtWarmingup = Convert.ToInt32(cwneWarmupSetVolt.Value);
        }


        //*******************************************************************************
        //機　　能： Stepチェックボックスクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足：
        //
        //履　　歴： v17.72/v19.02  2012/05/16  やまおか    新規作成
        //*******************************************************************************
        private void chkStepWU_Click(object sender, EventArgs e)
        {
            //表示上わかりやすいように、チェックしたときに変更する。
            //その後、低い電圧に修正して[開始]しても、
            //実際に段階ウォームアップするときは勝手に変更する。

            //チェックありのときは最終ステップの電圧値にする
            if (chkStepWU.CheckState == CheckState.Checked)
            {
                //変更2015/02/02hata_Max/Min範囲のチェック
                //cwneWarmupSetVolt.Value = CTSettings.iniValue.STEPWU_KV[CTSettings.iniValue.STEPWU_NUM];
                cwneWarmupSetVolt.Value = modLibrary.CorrectInRange(CTSettings.iniValue.STEPWU_KV[CTSettings.iniValue.STEPWU_NUM], cwneWarmupSetVolt.Minimum, cwneWarmupSetVolt.Maximum);
            }
        }


        //*******************************************************************************
        //機　　能： Viscom用ウォームアップ開始関数
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足：
        //
        //履　　歴： v17.72/v19.02  2012/05/14  やまおか    新規作成
        //*******************************************************************************
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //Private Sub ViscomWarmupStart(ByVal WUkV As Long)
        //
        //    'ウォームアップ用管電圧のセット
        //    XrayVoltAtWarmingup = WUkV
        //
        //    'ViscomBusyをセット v12.01追加 by 間々田 2006/12/13
        //    ViscomBusy = True
        //
        //    'ウォームアップ開始
        //    SendCode CODE_XRAY_WCON
        //
        //    'ちょっと待つ   'v17.72追加 byやまおか 2012/05/16
        //    Sleep 1000
        //
        //    'ウォームアップ中断フラグをリセット
        //    XrayWarmUpCanceled = False
        //
        //    'ウォームアップ中の最大出力管電圧をクリア
        //    XrayMaxFeedBackVolt = 0
        //
        //End Sub
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''


        //*******************************************************************************
        //機　　能： ウォームアップ「開始」「停止」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.5  2006/04/10 (WEB)間々田   新規作成
        //*******************************************************************************
        //Private Sub cmdWarmupStart_Click()
        public void cmdWarmupStart_Click(object sender, EventArgs e)      //v16.01/v17.00変更 byやまおか 2010/02/18
        {
            int StepWUkV;       //v19.02下から移動してきた byやまおか 2012/07/05

            //Rev25.03/Rev25.02 add by chouno 2017/03/07
            if (isWUPPreparation == true)
            {
                return;
            }

            //Rev25.03/Rev25.02 add by chouno 2017/03/07
            isWUPPreparation = true;

            //ウォームアップ「停止」ボタンをクリックした場合
            if (cmdWarmupStart.Text == CTResources.LoadResString(StringTable.IDS_btnStop))
            {
                cmdWarmupStop_Click();
                //Rev25.03/Rev25.02 add by chouno 2017/03/07
                isWUPPreparation = false;
                return;
            }

            //X線ON中に開始を押すとWUできないため、一旦OFFしてもらう v17.65 by長野 2011/11/25
            if (XrayStatus == StringTable.GC_Xray_On)
            {
                //X線をOFFした後、再度、開始ボタンをクリックしてください
                //MessageBox.Show(CTResources.LoadResString(20187), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //Rev20.00 変更 by長野 2015/02/25
                MessageBox.Show(this,CTResources.LoadResString(20187), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //Rev25.03/Rev25.02 add by chouno 2017/03/07
                isWUPPreparation = false;

                return;
            }

            //Rev25.03/Rev25.02 上に移動 by chouno 2017/02/11
            frmCTMenu frmCTMenu = frmCTMenu.Instance;

            //段階ウォームアップのための処理 'v17.72/v19.02追加 byやまおか 2012/05/16
            if (chkStepWU.CheckState == CheckState.Checked)
            {
                //段階ウォームアップは最終ステップの電圧値にする
                //変更2015/02/02hata_Max/Min範囲のチェック
                //cwneWarmupSetVolt.Value = CTSettings.iniValue.STEPWU_KV[CTSettings.iniValue.STEPWU_NUM];
                cwneWarmupSetVolt.Value = modLibrary.CorrectInRange(CTSettings.iniValue.STEPWU_KV[CTSettings.iniValue.STEPWU_NUM], cwneWarmupSetVolt.Minimum, cwneWarmupSetVolt.Maximum);
            }

            //メッセージ表示：
            //   ～までウォームアップを実行します。
            //   よろしいですか？
            //変更2014/10/07hata_v19.51反映
            //if (FirstDone)
            //if (FirstDone & !Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan))    //v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //Rev25.03/Rev25.02 add by chouno 2017/02/05
            if (!mod2ndXray.PackageWUPFlg && FirstDone & (!Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan)) && (!Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman)))
            {
                modXrayControl.WUP_Start = false;   //フラグ初期化   'v16.01/v17.00追加 byやまおか
                //If MsgBox(GetResString(9609, cwneWarmupSetVolt.Text), vbExclamation Or vbYesNo Or vbDefaultButton2) = vbNo Then Exit Sub
                //いいえを押しても開始してしまう不具合対策   'v16.01/v17.00変更 byやまおか
                //Rev20.00 変更 by長野 2015/02/25
                DialogResult result = MessageBox.Show(this,StringTable.GetResString(9609, cwneWarmupSetVolt.Text), Application.ProductName,
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                //DialogResult result = MessageBox.Show(StringTable.GetResString(9609, cwneWarmupSetVolt.Text), Application.ProductName,
                //                                     MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                if (result == DialogResult.No)
                {
                    modXrayControl.WUP_Start = false;       //いいえを押したら抜ける
                    //Rev25.03/Rev25.02 add by chouno 2017/03/07
                    isWUPPreparation = false;
                    return;
                }
            }

            //運転準備が未完了の場合                             '追加 by 間々田 2009/08/24
            if (!modSeqComm.MySeq.stsRunReadySW)
            {
                //MsgBox "運転準備が未完了です。", vbCritical
                //v17.60 ストリングテーブル化 by長野 2011/05/25
                MessageBox.Show(CTResources.LoadResString(20109), Application.ProductName);
                //Rev25.03/Rev25.02 add by chouno 2017/03/07
                isWUPPreparation = false;
                return;
            }

            //Rev25.03/Rev25.02 上に移動 by chouno 2017/02/11
            //frmCTMenu frmCTMenu = frmCTMenu.Instance;

            //追加2014/10/07hata_v19.51反映
            //産業用CTモードのときは見ない   'v18.00条件追加 byやまおか 2011/03/15
            if ((CTSettings.scaninh.Data.avmode != 0))
            {

                //電磁ロックが開の場合、自動的に電磁ロックを閉とする '追加 by 間々田 2009/08/24
                if (frmCTMenu.DoorStatus == frmCTMenu.DoorStatusConstants.DoorClosed)
                {
                    modSeqComm.SeqBitWrite("DoorLockOn", true);
                    modCT30K.PauseForDoEvents(2);
                }
            }
            //インターロックチェック                             '追加 by 間々田 2009/08/24
            if (!modXrayControl.IsXrayInterLock())
            {
                //メッセージ表示：
                //MsgBox "電磁ロックが開なので、ウォームアップを開始できません。", vbCritical
                //v15.03変更 リソース化&メッセージ変更　by やまおか 2009/11/17
                if (!modSeqComm.MySeq.stsDoorLock && CTSettings.scaninh.Data.door_lock == 0)
                {
                    //電磁ロックが開のため、ウォームアップ開始できません。
                    MessageBox.Show(StringTable.GetResString(StringTable.IDS_CannotDo, CTResources.LoadResString(StringTable.IDS_MagLock),
                                                             CTResources.LoadResString(StringTable.IDS_OpenOnly), CTResources.LoadResString(StringTable.IDS_WarmupStart)),
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //扉が開のため、ウォームアップ開始できません。
                    MessageBox.Show(StringTable.GetResString(StringTable.IDS_CannotDo, CTResources.LoadResString(StringTable.IDS_Door),
                                                             CTResources.LoadResString(StringTable.IDS_OpenOnly), CTResources.LoadResString(StringTable.IDS_WarmupStart)),
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //Rev25.03/Rev25.02 add by chouno 2017/03/07
                isWUPPreparation = false;

                return;
            }

            //産業用CTモードの場合   'v18.00追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //Rev23.10 操作パネルONの機能を分離 by長野 2015/10
            //if ((CTSettings.scaninh.Data.avmode == 0))
            if ((CTSettings.scaninh.Data.avmode == 0) || (CTSettings.scaninh.Data.op_panel == 0))
            {
                //メッセージ表示：
                //操作パネルがONなら
                if (modSeqComm.MySeq.PcInhibit)
                {
                    //操作パネルがONのため動作しません。
                    MessageBox.Show(CTResources.LoadResString(17513), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //Rev25.03/Rev25.02 add by chouno 2017/03/07
                    isWUPPreparation = false;
                    return;
                }
            }

            //Rev26.40 add by chouno 2019/02/17
            if (CTSettings.scaninh.Data.high_speed_camera == 0 && CTSettings.iniValue.HSCSettingType == 1)
            {
                //メッセージ表示：
                //動作許可が不許可なら
                if (modSeqComm.MySeq.PcInhibit)
                {
                    //動作許可がOFFのため動作しません。
                    MessageBox.Show(CTResources.LoadResString(27000), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    isWUPPreparation = false;
                    return;
                }
            }
            //スタンバイモードチェック   'v17.72/v19.02追加 byやまおか 2012/05/16
            if (!modXrayControl.IsXrayReady())
            {
                //メッセージ表示：Ｘ線がスタンバイモードのため、Ｘ線をオンできません。
                MessageBox.Show(CTResources.LoadResString(9370), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Rev25.03/Rev25.02 add by chouno 2017/03/07
                isWUPPreparation = false;
                return;
            }

            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //ウォームアップ完了後のフィラメント調整を実施するためのフラグをセット
            //If chkFilament.Value = vbChecked Then FilamentAdjustAfterWarmup = True
            //    If XrayType = XrayTypeViscom Then           'v15.10変更 byやまおか 2009/10/13
            //        If chkFilament.Value = vbChecked Then FilamentAdjustAfterWarmup = True
            //    End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //ウォームアップが手動で開始された
            modXrayControl.WarmupStartAuto = false;

            //v15.10下記に変更 byやまおか 2009/10/30
            //'ウォームアップ用管電圧のセット
            //XrayVoltAtWarmingup = cwneWarmupSetVolt.Value
            //
            //'ViscomBusyをセット v12.01追加 by 間々田 2006/12/13
            //ViscomBusy = True
            //
            //SendCode CODE_XRAY_WCON
            //
            //'ウォームアップ中断フラグをリセット
            //XrayWarmUpCanceled = False
            //
            //'ウォームアップ中の最大出力管電圧をクリア
            //XrayMaxFeedBackVolt = 0

            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //ビープ音を鳴らす(500ms間隔で5回)   'v17.00追加 byやまおか 2010/01/19
            //If scaninh.xrayon_beep = 0 Then SoundBeep 5, 500
            //    If scaninh.xrayon_beep = 0 Then PlayXrayOnWarningSound  'v17.00変更 byやまおか 2010/03/12
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //Rev23.00 ビープ音.NET対応 by長野 2015/09/24
            if (CTSettings.scaninh.Data.xrayon_beep == 0)
            {
                modSound.PlayXrayOnWarningSound();
            }

            //I.I.電源のチェック（FPDはオフしない）  'v16.01/v17.00追加 byやまおか 2010/02/25
            //電源がすでにオンの場合はオフにする
            //v17.20 検出器切替用に条件を変更
            //変更2014/10/07hata_v19.51反映
            //If MySeq.stsIIPower And Not Use_FlatPanel Then SeqBitWrite "IIPowerOff", True
            if (CTSettings.SecondDetOn)
            {
                if (mod2ndDetctor.IsDet1mode)
                {
                    if (modSeqComm.MySeq.stsIIPower & !CTSettings.detectorParam.Use_FlatPanel)
                    {
                        modSeqComm.SeqBitWrite("IIPowerOff", true);
                    }
                }
                else if (mod2ndDetctor.IsDet2mode)
                {
                    if (modSeqComm.MySeq.stsTVIIPower & !CTSettings.detectorParam.Use_FlatPanel)
                    {
                        modSeqComm.SeqBitWrite("TVIIPowerOff", true);
                    }
                }
            }
            else
            {
                if (modSeqComm.MySeq.stsIIPower & !CTSettings.detectorParam.Use_FlatPanel)
                {
                    modSeqComm.SeqBitWrite("IIPowerOff", true);
                }
            }

            //追加2014/10/07hata_v19.51反映
            int Val1 = 0;
            int filterNum = 0;
            int ret = 0;
            int kv = 0;
            modTitan.WarmupConstants wu = default(modTitan.WarmupConstants);
            int focusNum = 0;                   //v15.10追加 byやまおか 2009/10/30
            float current = 0;            //v18.00追加 byやまおか 2011/09/05

            //v15.10追加 byやまおか 2009/10/30
            switch (modXrayControl.XrayType)
            {
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                //        Case XrayTypeViscom
                //
                //            ''ウォームアップ用管電圧のセット
                //            'XrayVoltAtWarmingup = cwneWarmupSetVolt.Value
                //            '
                //            ''ViscomBusyをセット v12.01追加 by 間々田 2006/12/13
                //            'ViscomBusy = True
                //            '
                //            'SendCode CODE_XRAY_WCON
                //            '
                //            ''ウォームアップ中断フラグをリセット
                //            'XrayWarmUpCanceled = False
                //            '
                //            ''ウォームアップ中の最大出力管電圧をクリア
                //            'XrayMaxFeedBackVolt = 0
                //            '
                //            ''上記を関数化   'v17.72/v19.02変更 byやまおか 2012/05/14
                //            'ViscomWarmupStart(cwneWarmupSetVolt.Value)
                //
                //            '段階ウォームアップ対応 'v17.72/v19.02変更 byやまおか 2012/05/14
                //            'ウォームアップ用管電圧のセット
                //            '段階ウォームアップのときは
                //            'Dim StepWUkV    As Long    'v19.02上へ移動 byやまおか 2012/07/05
                //            'myStepWU_Num = 0
                //            '段階ウォームアップ用変数の初期化
                //            StepWU_CurrentNum = 0   'v19.02変更 Property化 byやまおか 2012/07/20
                //            If (chkStepWU.Value = vbChecked) Then
                //                '第1段階をセット
                //                'myStepWU_Num = 1
                //                StepWU_CurrentNum = 1   'v19.02変更 Property化 byやまおか 2012/07/20
                //                StepWUkV = STEPWU_KV(1)
                //            '通常は
                //            Else
                //                '第1段階をセット
                //                StepWU_CurrentNum = 1   'v19.02追加 byやまおか 2012/07/20
                //                '設定値をセット
                //                StepWUkV = cwneWarmupSetVolt.Value
                //            End If
                //
                //            'ウォームアップ開始
                //            ViscomWarmupStart (StepWUkV)
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL12721://Rev23.10 追加 by長野 2015/10/01
                    
                    //Rev25.03/Rev25.02 add by chouno 2017/03/07
                    isWUPPreparation = false;
                    
                    //'I.I.（またはFPD）電源のチェック
                    //'電源がすでにオンの場合
                    //If MySeq.stsIIPower Then SeqBitWrite "IIPowerOff", True    'v16.01/v17.00上に移動 byやまおか 2010/02/23

                    //'ウォームアップ用管電圧のセット
                    //XrayVoltAtWarmingup = cwneWarmupSetVolt.Value
                    //'Call UC_Feinfocus.XrayCMV_Set(CInt(XrayVoltAtWarmingup))
                    //Call UC_XrayCtrl.XrayCMV_Set(CInt(XrayVoltAtWarmingup))    'v16.20変更 byやまおか 2010/04/21
                    //
                    //'Up_XrayStatusSMVプロパティがValueに変化するまで待つ    'v15.11追加 byやまおか 2010/02/09
                    //'WaitXrayCMV_Ready CInt(XrayVoltAtWarmingup)
                    //If Not WaitXrayCMV_Ready(CInt(XrayVoltAtWarmingup)) Then Exit Sub   'v16.01変更 byやまおか 2010/02/25
                    //
                    //段階ウォームアップ対応 'v19.02変更 byやまおか 2012/07/05
                    //ウォームアップ用管電圧のセット
                    //段階ウォームアップのときは
                    //myStepWU_Num = 0
                    StepWU_CurrentNum = 0;          //v19.02変更 Property化 byやまおか 2012/07/20
                    if (chkStepWU.CheckState == CheckState.Checked)
                    {
                        //第1段階をセット、1回目は1:WUP
                        //myStepWU_Num = 1
                        //StepWUkV = STEPWU_KV(myStepWU_Num)
                        StepWU_CurrentNum = 1;                                      //v19.02変更 Property化 byやまおか 2012/07/20
                        StepWUkV = CTSettings.iniValue.STEPWU_KV[StepWU_CurrentNum];     //v19.02変更 Property化 byやまおか 2012/07/20
                    }
                    //通常は
                    else
                    {
                        //第1段階をセット
                        StepWU_CurrentNum = 1;      //v19.02追加 byやまおか 2012/07/20
                        //設定値をセット
                        StepWUkV = modXrayControl.XrayVoltAtWarmingup;
                    }

                    //ウォームアップ用管電圧のセット
                    UC_XrayCtrl.XrayCMV_Set(Convert.ToInt32(StepWUkV));

                    //Up_XrayStatusSMVプロパティがValueに変化するまで待つ
                    if (!modXrayControl.WaitXrayCMV_Ready(Convert.ToInt32(StepWUkV))) return;

                    //ウォームアップ完了しても、ステータスが未完になってしまう対策   'v16.01追加 byやまおか 2010/02/25
                    //Sleep (3000)    '3秒待つ(確実に設定されてからWUPを開始する)
                    //Sleep (2000)    '3秒→2秒   'v19.02変更 byやまおか 2012/07/20
                    System.Threading.Thread.Sleep(2500);    //2秒→2.5秒 'v19.02変更 byやまおか 2012/07/23

                    //'ウォームアップを実行する
                    //'Val1 = 3    '1:WUP 2:WUP1 3:WUP2
                    //Val1 = IIf(WUP_No = -1, 3, WUP_No)  '1:WUP 2:WUP1 3:WUP2    'v16.01/v17.00変更 byやまおか 2010/02/18
                    //
                    //段階ウォームアップ対応 'v19.02変更 byやまおか 2012/07/05
                    //ウォームアップの種類をセット
                    //段階ウォームアップのときは
                    if (chkStepWU.CheckState == CheckState.Checked)
                    {
                        //第1段階はWUPを実行する
                        Val1 = 1;       //1:WUP 2:WUP1 3:WUP2
                    }
                    //通常は
                    else
                    {
                        //ウォームアップの種類をセット
                        Val1 = ((modXrayControl.WUP_No == -1) ? 3 : modXrayControl.WUP_No);  //1:WUP 2:WUP1 3:WUP2
                    }

                    //ここから　//Rev22.00 Rev21.01の反映 by長野 2015/07/23
                    //WUP前の管電圧・管電流をもっておく
                    //bakWUPkV = (int)ntbSetVolt.Value;
                    //Rev23.20 変更 by長野 2016/01/25
                    bakWUPkV = modLibrary.CorrectInRange((int)ntbSetVolt.Value, (int)cwneKV.Minimum, (int)cwneKV.Maximum);
                    
                    bakWUPmA = (float)ntbSetCurrent.Value;
                    //WUP後は、ソフトを介さず制御器自身が設定値を変えるため、
                    //内部で持っている前回設定値は初期化しておく
                    modXrayControl.TempSetCurrent = -1;
                    modXrayControl.TempSetVolt = -1;
                    //ここまで　//Rev22.00 Rev21.01の反映 by長野 2015/07/23

                    //Call UC_Feinfocus.XrayWarmUp_Set(Val1)
                    UC_XrayCtrl.XrayWarmUp_Set(Val1);   //v16.20変更 byやまおか 2010/04/21

                    break;

                //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9191:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181_02:     //追加2014/10/07hata_v19.51反映
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10711://Rev23.10 追加 by長野 2015/11/08

                    //Rev25.03/Rev25.02 add by chouno 2017/03/07
                    isWUPPreparation = false;

                    Val1 = 1;       //1:実行
                    //Call UC_Feinfocus.XrayWarmUp_Set(Val1)
                    UC_XrayCtrl.XrayWarmUp_Set(Val1);   //v16.20変更 byやまおか 2010/04/21
                    break;


                //v18.00追加 byやまおか 2011/03/02

                //v19.50 v19.41とv18.02の統合 by長野 2013/11/07 ここから
                //Titanの場合はウォームアップが完了するまでStartWarmUp()で止まる。
                //CT30Kへはコールバック関数で反応する。
                case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:
                case modXrayControl.XrayTypeConstants.XrayTypeSpellman:  //add Rev25.03/Rev25.02 2017/02/05 by chouno

                    kv = Convert.ToInt32(cwneKV.Value);

                    //ウォームアップモードは？
                    wu = modTitan.Ti_CheckWarmUpStatus(kv);
                
                    //ウォームアップ完了でなければ
                    if ((wu != modTitan.WarmupConstants.WU_READY))
                    {

                        //タッチパネル操作禁止   'v18.00追加 byやまおか 2011/03/14
                        modSeqComm.SeqBitWrite("PanelInhibit", true);

                        //焦点を記憶する 'v18.00追加 byやまおか 2011/09/05
                        focusNum = modTitan.Ti_GetFocusSize() + 1;

                        //焦点を記憶する 'v18.00追加 byやまおか 2011/09/05
                        current = (float)cwneMA.Value;

                        if ((CTSettings.scaninh.Data.shutterfilter == 0))
                        {
                            //現在のX線フィルタを記憶する
                            filterNum = modSeqComm.GetFilterIndex();

                            //Rev25.03/Rev25.02 ここでフィルタを取得できていないと、WUP完了後に元のフィルタへ戻せないので
                            //エラー処理追加 by chouno 2017/03/07
                            if (filterNum == -1)
                            {
                                MessageBox.Show(CTResources.LoadResString(8097), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //Rev25.02 add by chouno 2017/03/07
                                isWUPPreparation = false;
                                return;
                            }
                        }

                        ////シャッターを閉める
                        //if (!(modSeqComm.SeqBitWrite("Shutter", true)))
                        //{
                        //    return;
                        //}
                        //Rev25.03/Rev25.02 shutter位置の確認を行う by chouno 2017/03/07
                        bool bret = false;
                        bret = modSeqComm.ChangeFilter(5);//5==shutter

                        if (bret != true)
                        {
                            MessageBox.Show(CTResources.LoadResString(6011), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //Rev25.02 add by chouno 2017/03/07
                            isWUPPreparation = false;
                            return;
                        }

                        //ウォームアップ状態をウォームアップ中にする
                        myXrayWarmUp = modXrayControl.XrayWarmUpConstants.XrayWarmUpNow;
                        modTitan.Flg_TiWarmingUp = true;

                        //ボタンを「中止」にする
                        cmdWarmupStart.Text = CTResources.LoadResString(StringTable.IDS_btnStop);
                        cmdWarmupStart.Refresh();

                        modTitan.TitanWuCallbackDelegate wuCallback = new modTitan.TitanWuCallbackDelegate(modTitan.WuCallback);

                        //Rev25.02 add by chouno 2017/03/07
                        isWUPPreparation = false;

                        //ウォームアップスタート
                        ret = modTitan.Ti_StartWarmUp(kv, wu, wuCallback);

                        //If (ret <> 0) Then
                        //    'ウォームアップ未完
                        //    myXrayWarmUp = XrayWarmUpNotComplete
                        //    'ウォームアップ終了
                        //    Flg_TiWarmingUp = False
                        //End If
                        //

                        //Rev23.20 ウォームアップ成功なら、ここでフラグを落としてOK by長野 2016/01/21
                        if (ret == 0)
                        {
                            modTitan.Flg_TiWarmingUp = false;

                            //Rev23.20 追加 2世代・3世代兼用の場合は、シーケンサに400以上or430以上のX線が出せるかどうかを通知する by長野 2016/01/18
                            if (CTSettings.scaninh.Data.ct_gene2and3 == 0)
                            {
                                modTitan.SendSeq_Ti_XoffVolt_ByValue(kv);
                            }
                        }

                        //'ウォームアップ完了
                        //myXrayWarmUp = XrayWarmUpComplete
                        myXrayWarmUp = (ret != 0 ? modXrayControl.XrayWarmUpConstants.XrayWarmUpNotComplete : modXrayControl.XrayWarmUpConstants.XrayWarmUpComplete);
                        //v18.00修正 byやまおか 2011/03/26

                        //ボタンを「開始」にする
                        cmdWarmupStart.Text = CTResources.LoadResString(StringTable.IDS_btnStart);

                        //Rev23.20 処理の順番が悪いので見直し by長野 2016/01/21
                        ////フィルタを戻す前にX線ステータスがWU中ではなくなるのを待つ
                        //modCT30K.PauseForDoEvents(5);
                        ////v18.00追加 byやまおか 2011/07/09

                        ////フィルタを元に戻す
                        //if ((CTSettings.scaninh.Data.shutterfilter == 0))
                        //{
                        //    modSeqComm.SeqBitWrite("Filter" + Convert.ToString(filterNum), true);
                        //}

                        ////焦点を元に戻す     'v18.00追加 byやまおか 2011/09/05
                        //cmdFocus_Click(cmdFocus[focusNum], new System.EventArgs());

                        ////管電流を元に戻す   'v18.00追加 byやまおか 2011/09/05
                        //modXrayControl.SetCurrent((current));

                        ////タッチパネル操作禁止   'v18.00追加 byやまおか 2011/03/14
                        //modSeqComm.SeqBitWrite("PanelInhibit", false);

                        ////今の管電圧を設定する
                        //modXrayControl.SetVolt((float)cwneKV.Value);

                        ////Rev23.20 追加 by長野 2016/01/21
                        //MyUpdate();

                        ////ウォームアップ終了
                        //modTitan.Flg_TiWarmingUp = false;

                        //フィルタを戻す前にX線ステータスがWU中ではなくなるのを待つ
                        //管電流を元に戻す   'v18.00追加 byやまおか 2011/09/05
                        modXrayControl.SetCurrent((current), true);

                        modCT30K.PauseForDoEvents(1);

                        //今の管電圧を設定する
                        modXrayControl.SetVolt((float)cwneKV.Value, true);

                        modCT30K.PauseForDoEvents(5);
                        //v18.00追加 byやまおか 2011/07/09

                        //フィルタを元に戻す
                        if ((CTSettings.scaninh.Data.shutterfilter == 0))
                        {
                            modSeqComm.SeqBitWrite("Filter" + Convert.ToString(filterNum), true);
                        }

                        //焦点を元に戻す     'v18.00追加 byやまおか 2011/09/05
                        cmdFocus_Click(cmdFocus[focusNum], new System.EventArgs());

                        //Rev23.20 追加 by長野 2016/01/21
                        MyUpdate();

                        //ウォームアップ終了
                        modTitan.Flg_TiWarmingUp = false;

                        //タッチパネル操作禁止   'v18.00追加 byやまおか 2011/03/14
                        modSeqComm.SeqBitWrite("PanelInhibit", false);

                    }
                    else
                    {
                        //Rev25.03/Rev25.02 add by chouno 2017/03/07
                        isWUPPreparation = false;                
                    }

                    //Titanの場合はここで抜ける
                    return;     //v19.50 v19.41とv18.02の統合 by長野 2013/11/07 ここまで
            
            }
            
            //Rev20.00 移動 by長野 2015/04/14
            WUstsEvntLock = false;

            //ボタンを「中止」にする
            cmdWarmupStart.Text = CTResources.LoadResString(StringTable.IDS_btnStop);
        }

        //*******************************************************************************
        //機　　能： ウォームアップ「中止」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.5  2006/04/10 (WEB)間々田   新規作成
        //*******************************************************************************
        //private void cmdWarmupStop_Click()
        public void cmdWarmupStop_Click()
        {
//            int Val1;

            switch (modXrayControl.XrayType)
            {
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                //        Case XrayTypeViscom
                //
                //            'X線OFF
                //            SetXrayStop
                //
                //            'ちょっと待つ   'v17.72/v19.02追加 byやまおか 2012/05/16
                //            Sleep 1000
                //
                //            'ウォームアップ中断フラグをセット
                //            XrayWarmUpCanceled = True
                //
                //            'Ｘ線ウォームアップ中の場合、Ｘ線ウォームアップ強制中断フラグオン       'changed by 山本 2007-2-20  ウォームアップ中のみ中断フラグをオンにする
                //            If XrayWarmUp = XrayWarmUpNow Then XrayWarmUpCanceled = True
                //
                //            'フィラメント調整を実施するためのフラグをリセット   'v12.01追加 by 間々田 2006/12/04
                //            FilamentAdjustAfterWarmup = False
                //
                //            'ウォームアップの段階をクリア   'v17.72/v19.02追加 byやまおか 2012/05/16
                //            'myStepWU_Num = 0
                //            StepWU_CurrentNum = 0   'v19.02変更 Property化 byやまおか 2012/07/20
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL12721://Rev23.10 追加 by長野 2015/10/01

                    //X線OFF
                    modXrayControl.XrayOff();

                    //ウォームアップ中断フラグをセット
                    modXrayControl.XrayWarmUpCanceled = true;

                    //Ｘ線ウォームアップ中の場合、Ｘ線ウォームアップ強制中断フラグオン       'changed by 山本 2007-2-20  ウォームアップ中のみ中断フラグをオンにする
                    if (modXrayControl.XrayWarmUp() == modXrayControl.XrayWarmUpConstants.XrayWarmUpNow) modXrayControl.XrayWarmUpCanceled = true;

                    //ウォームアップの段階をクリア   'v19.02追加 byやまおか 2012/07/05
                    //myStepWU_Num = 0
                    StepWU_CurrentNum = 0;      //v19.02変更 Property化 byやまおか 2012/07/20

                    //ウォームアップ用管電圧のセット     'v19.02追加 byやまおか 2012/07/09
                    //段階ウォームアップの段階切り替わり時に停止するとステータスが完了となってしまうため
                    //管電圧セットコマンドを送ってステータスを未完にする
                    if (chkStepWU.CheckState == CheckState.Checked)
                    {
                        UC_XrayCtrl.XrayCMV_Set(Convert.ToInt32(modXrayControl.XrayVoltAtWarmingup));
                    }

                    //Rev22.00 //Rev21.01の反映 ウォームアップ実行前の設定に戻す by長野 2015/07/23
                    bakWUPXrayCondition();

                    break;

                //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9191:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:    //v16.30追加 byやまおか 2010/05/21
                case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:          //追加2014/10/07hata_v19.51反映
                case modXrayControl.XrayTypeConstants.XrayTypeSpellman:  //add Rev25.03/Rev25.02 2017/02/05 by chouno
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181_02:     //追加2014/10/07hata_v19.51反映
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10711:       //Rev23.10 追加 by長野 2015/10/01      
                    //X線OFF
                    modXrayControl.XrayOff();
                    break;
            }

            //ボタンを「開始」にする
            cmdWarmupStart.Text = CTResources.LoadResString(StringTable.IDS_btnStart);
        }


        //*************************************************************************************************
        //機　　能： 更新用タイマー処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************
        private void tmrUpdate_Timer(object sender, EventArgs e)
        {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
            Static Done As Boolean
            Static Count As Integer
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            if (modCT30K.RequestExit) return;

            //以下の項目は初回時のみ実行する。
            //   ※通常はFeinFocus.exeのイベントで取得する。
            //     ただし東芝 EXM2-150, ViscomはFeinFocus.exeのイベントを使用しないので常に実行する。
            if (!tmrUpdate_Timer_Done)
            {
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                //Viscom対応：各種状態取得 v11.5追加 by 間々田 2006/04/10
                //        If XrayType = XrayTypeViscom Then
                //
                //            GetViscom
                //
                //        End If
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

                //設定管電圧・設定管電流
                //lblkVmA.Caption = CStr(XrayVoltSet) & "kV / " & CStr(XrayCurrentSet) & CurrentUni

                //更新
                //this.Update();
                MyUpdate();


                switch (modXrayControl.XrayType)
                {
                    //変更2014/10/07hata_v19.51反映
                    case modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150:
				    case modXrayControl.XrayTypeConstants.XrayTypeViscom:
				    case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:  //v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    case modXrayControl.XrayTypeConstants.XrayTypeSpellman:  //add Rev25.03/Rev25.02 2017/02/05 by chouno
                       break;

                    default:
                        tmrUpdate_Timer_Done = true;
                        break;
                }
            }

            //Rev23.10 全フォーム起動終了後に統一するため ｺﾒﾝﾄｱｳﾄ by長野 2015/10/29
            ////変更2014/10/07hata_v19.51反映
            ////v17.71 追加 by長野 2012-03-28
            ////QueryWarmup();
            ////v19.19 条件式を追加する by長野 2013/09/13
            ////Viscomの時、フォームが全表示された後に処理するように変更
            ////v19.191 浜ホト追加 by長野 2013/10/02
            //if (!((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeViscom) |
            //     (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL12721) | //Rev23.10 追加 by長野 2015/10/01
            //      (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801) |
            //      (CTSettings.scaninh.Data.multi_tube == 0))) //Rev23.10 追加 by長野 2015/10/06
            //{
            //    QueryWarmup();
            //}

            //追加2014/10/07hata_v19.51反映
            //v19.50 Updateからに移動 by長野 2013/11/13
            //管電圧と管電流とフィルタ厚の組み合わせが同じなら   'v18.00追加 byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //If (XrayCondVolt = ntbSetVolt.Value) And (XrayCondCurrent = ntbSetCurrent.Value) And _
            //'   (ntbFilter.value = infdef.filter(XrayCondIndex, GetIINo())) Then
            //v18.00変更 byやまおか 2011/08/09
            //v19.50 産業用の場合のみフィルタを条件に加える by長野 2013/11/13
            //    If (XrayCondVolt = ntbSetVolt.Value) And (XrayCondCurrent = ntbSetCurrent.Value) And _
            //'       (StrComp(ntbFilter.Text, GetFilterTableStr(XrayCondIndex, GetIINo())) = 0) Then
            if ((XrayCondVolt == (float)ntbSetVolt.Value) & (XrayCondCurrent == (float)ntbSetCurrent.Value))
            {
                if ((CTSettings.scaninh.Data.avmode == 0))
                {
                    //変更2015/01/24hata
                    //if ((Strings.StrComp(ntbFilter.Text, modCT30K.GetFilterTableStr(XrayCondIndex, modSeqComm.GetIINo())) == 0))
                    //if (string.Compare(ntbFilter.Text, modCT30K.GetFilterTableStr(XrayCondIndex, modSeqComm.GetIINo()), true) != 0)
                    if (string.Compare(ntbFilter.Value.ToString(), modCT30K.GetFilterTableStr(XrayCondIndex, modSeqComm.GetIINo()), true) != 0)
                    {
                        //X線条件ボタンをセット
                        modLibrary.SetCmdButton(cmdCondition, XrayCondIndex);
                    }
                    else
                    {
                         modLibrary.SetCmdButton(cmdCondition, -1);
                    }
                }
                else
                {
                    //X線条件ボタンをセット
                    modLibrary.SetCmdButton(cmdCondition, XrayCondIndex);
                }
            }
            else
            {
                //X線条件ボタンをクリア
                modLibrary.SetCmdButton(cmdCondition, -1);
            }


            //Ｘ線ステータスの更新

            int m_XR_Status;    //X線異常（0:異常、1:正常）
            bool IsXrayOk;
            string strStatus;

            //初期値は正常
            IsXrayOk = true;
            strStatus = string.Empty;
            string strTiError = null;		//v19.50 追加 Titan用エラーコード by長野 2014/02/08
            string strTiCommError = null;   //v23.20 追加 Titan用Commエラーコード by長野 2016/01/19

            //Ｘ線スタンバイ
            bool IsStandby;
            IsStandby = !modXrayControl.IsXrayReady();

            //v11.5追加ここから by 間々田 2006/04/10 Viscom対応
            if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeViscom)
            {
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''

                //        'If ViscomError And Not (XER_WUP_FAILED Or XER_FLM_FAILED Or XER_CNT_FAILED Or XER_VAC Or XER_INT_KNF Or XER_INT_SCH Or XER_INT_KEY) Then
                //        'If ViscomError And Not (XER_WUP_FAILED Or XER_FLM_FAILED Or XER_CNT_FAILED Or XER_INT_KNF Or XER_INT_SCH Or XER_INT_KEY) Then 'v11.5変更 by 間々田 2006/07/12 真空未完を異常としない
                //'        If ViscomError And Not (XER_WUP_FAILED Or XER_FLM_FAILED Or XER_CNT_FAILED Or XER_VAC Or XER_PUMP Or XER_INT_KNF Or XER_INT_SCH Or XER_INT_KEY) Then  'v11.5変更 by 間々田 2006/07/12 真空未完,真空ポンプエラーを異常としない
                //'        If ViscomError And Not (XER_WUP_FAILED Or XER_FLM_FAILED Or XER_CNT_FAILED Or XER_VAC Or XER_INT_KNF Or XER_INT_SCH Or XER_INT_KEY) Then   'v11.5変更 by 山本 2006/09/21 真空ポンプエラーは異常とする
                //        If ViscomError And Not (XER_WUP_FAILED Or XER_FLM_FAILED Or XER_CNT_FAILED Or XER_VAC Or XER_INT_KNF Or XER_INT_SCH Or XER_INT_KEY Or XER_TARGET Or XER_CSHT Or XER_CFC Or XER_FUA Or XER_FKV Or XER_GUA Or XER_GKV Or XER_GIU Or XER_GII Or XER_SPSDEF) Then   'v11.5変更 by 山本 2006-12-13 異常としないエラーを追加 informativeは停止とする
                //            strStatus = GC_Xray_Error                           '異常
                //            If ViscomState1 And XST1_WUP_RUNNING Then strStatus = GC_STS_INT    'ウォームアップ中に異常となった場合は「中断」とする 'v11.5追加 by 間々田 2006/07/28
                //        ElseIf ViscomError And (XER_TARGET Or XER_CSHT Or XER_CFC Or XER_FUA Or XER_FKV Or XER_GUA Or XER_GKV Or XER_GIU Or XER_GII Or XER_SPSDEF) Then      'v11.5変更 by 山本 2006-12-13 異常としないエラーを追加 informativeは停止とする
                //             strStatus = GC_STS_STOPPED                                 '停止   'added by 山本　2006-12-13
                //
                //        'ElseIf (MySeq.stsDoorInterlock And (scaninh.door_lock = 1)) Or _                                   'v15.0削除 by 間々田 2009/01/18 扉インターロックステータスをＸ線のステータスに含めない
                //        '       ((Not MySeq.stsDoorKey) And (scaninh.door_lock = 0) And (scaninh.door_keyinput = 0)) Then   'v15.0削除 by 間々田 2009/01/18 扉インターロックステータスをＸ線のステータスに含めない
                //        '    strStatus = LoadResString(12756)                    '扉開                                      'v15.0削除 by 間々田 2009/01/18 扉インターロックステータスをＸ線のステータスに含めない
                //
                //        'ElseIf Not MySeq.stsRunReadySW Then                                    'v15.0削除 by 間々田 2009/01/18 運転準備ステータスをＸ線のステータスに含めない
                //        '    strStatus = LoadResString(12758)                    '運転準備未完  'v15.0削除 by 間々田 2009/01/18 運転準備ステータスをＸ線のステータスに含めない
                //
                //        ElseIf Not CBool(ViscomState1 And XST1_VAC_OK) Then
                //            strStatus = GC_STS_VAC_NOTREADY                     '真空未完   'v11.5追加 by 間々田 2006/07/12
                //
                //        ElseIf IsStandby Then
                //            strStatus = LoadResString(12757)                    'スタンバイ
                //
                //        ElseIf ViscomState1 And XST1_WUP_FAILED Then
                //            strStatus = GC_STS_WUP_FAILED                       'ｳｫｰﾑｱｯﾌﾟ失敗
                //
                //        ElseIf ViscomState1 And XST1_FLM_FAILED Then
                //            strStatus = GC_STS_FLM_FAILED                       'ﾌｨﾗﾒﾝﾄ調整失敗
                //
                //        ElseIf ViscomState1 And XST1_CNT_FAILED Then
                //            strStatus = GC_STS_CNT_FAILED                       'ｾﾝﾀﾘﾝｸﾞ失敗   ’added by 山本 2006-12-14
                //
                //        ElseIf ViscomState1 And XST1_WUP_RUNNING Then
                //            strStatus = GC_Xray_WarmUp                          'ｳｫｰﾑｱｯﾌﾟ中
                //
                //        ElseIf ViscomState1 And XST1_FLM_RUNNING Then
                //            strStatus = GC_STS_FLM_RUNNING                      'ﾌｨﾗﾒﾝﾄ調整中
                //
                //        ElseIf CBool(ViscomState1 And XST1_CNT_RUNNING) Or XrayCenteringManual Then
                //            strStatus = GC_STS_CNT_RUNNING                      'ｾﾝﾀﾘﾝｸﾞ中
                //
                //        ElseIf ViscomState2 And XST2_XON Then
                //            strStatus = GC_Xray_On                              'Ｘ線ＯＮ中
                //
                //        ElseIf (Not CBool(ViscomState1 And XST1_WUP_READY)) And (Not XrayWarmUpCanceled) Then
                //            strStatus = GC_STS_WUP_NOTREADY                     'ｳｫｰﾑｱｯﾌﾟ未完
                //
                //        ElseIf Not CBool(ViscomState1 And XST1_FLM_READY) Then
                //            strStatus = GC_STS_FLM_NOTREADY                     'ﾌｨﾗﾒﾝﾄ調整未完
                //
                //        Else
                //            strStatus = GC_STS_STANDBY_OK                       '準備完了
                //        End If

                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            }
            else
            //v11.5追加ここまで by 間々田 2006/04/10
            {
                //    '注意：シーケンサのインターロックはTrueが開、Falseが閉

                //If XrayType = XrayTypeToshibaEXM2_150 Then  'v11.3追加 by 間々田 2006/02/17
                //    IsStandby = Not IsXrayStandby           'v11.3追加 by 間々田 2006/02/17
                //Else                                        'v11.3追加 by 間々田 2006/02/17
                //    'スタンバイ：シーケンサのインターロック閉かつＸ線コントロールのインターロック開のときをスタンバイとみなす←苦肉の策 2005/11/29
                //    IsStandby = IsInterLock And (XrayControl.Up_InterLock = 0)
                //End If                                      'v11.3追加 by 間々田 2006/02/17

                //ウォームアップ中
                //If myXrayWarmUp = XrayWarmUpNow Then
                //If (myXrayWarmUp = XrayWarmUpNow) Or (myStepWU_Num <> 0) Then   'v19.02変更 段階WUP対応 byやまおか 2012/07/09
                if ((myXrayWarmUp == modXrayControl.XrayWarmUpConstants.XrayWarmUpNow) || (StepWU_CurrentNum != 0))  //v19.02変更 Property化 byやまおか 2012/07/20
                {
                    strStatus = StringTable.GC_Xray_WarmUp;         //ｳｫｰﾑｱｯﾌﾟ中
                }
                //Ｘ線ＯＮ中
                //ElseIf IsXrayOn Then
                else if (myXrayOn == modCT30K.OnOffStatusConstants.OnStatus)        //v11.5変更 by 間々田 2006/06/21
                {
                    strStatus = StringTable.GC_Xray_On;             //Ｘ線ＯＮ中
                }
                else
                {
                    switch (modXrayControl.XrayType)
                    {
                        //浜ホト対応 by 間々田 2004/02/18
                        //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421
                        //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL10801    'v15.10変更 byやまおか 2009/10/17
                        //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL10801, XrayTypeHamaL8601 'v16.03/v16.20変更 byやまおか 2010/03/03
                        //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL10801, XrayTypeHamaL8601, XrayTypeHamaL9421_02T  'v16.30 02T追加 byやまおか 2010/05/21
                        case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
                        case modXrayControl.XrayTypeConstants.XrayTypeHamaL9191:
                        case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
                        case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:
                        case modXrayControl.XrayTypeConstants.XrayTypeHamaL8601:
                        case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:
                        case modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02:         //v17.71 追加 b長野 2012/03/14
                        case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181_02: //追加2014/11/05hata L9181-02に対応
                        case modXrayControl.XrayTypeConstants.XrayTypeHamaL10711://Rev23.10 追加 by長野 2015/10/01
                        case modXrayControl.XrayTypeConstants.XrayTypeHamaL12721://Rev23.10 追加 by長野 2015/10/01

                            m_XR_Status = modXrayControl.XrayControl.Up_XR_Status;

                            if (m_XR_Status == 0)
                            {
                                if (tryConnectCount < 60)
                                {
                                    tryConnectCount = tryConnectCount + 1;
                                    //strStatus = lblXray.Caption
                                    strStatus = myXrayStatus;     //変更 by 間々田 2009/07/31
                                }
                                else
                                {
                                    strStatus = CTResources.LoadResString(12909);         //接続エラー
                                    IsXrayOk = false;
                                }
                            }
                            else if (m_XR_Status == 7)
                            {
                                strStatus = CTResources.LoadResString(12754);             //過負荷
                                IsXrayOk = false;
                            }
                            else if (m_XR_Status == 1)
                            {
                                strStatus = CTResources.LoadResString(12755);             //プリヒート
                            }
                            else if ((m_XR_Status == 2) && (!modSeqComm.MySeq.stsDoorInterlock) && modSeqComm.MySeq.stsRunReadySW)
                            {
                                //strStatus = GC_Xray_Error                  '異常
                                strStatus = StringTable.GC_STS_STANDBY_NG;              //準備未完了
                                IsXrayOk = false;
                            }

                            //冷却水異常をここで表示する     'v17.10追加 byやまおか 2010/08/25
                            if (CTSettings.mecainf.Data.coolant_err == 1)
                            {
                                strStatus = StringTable.GC_STS_COOLANT_ERROR;           //冷却水異常
                                IsXrayOk = false;       //v17.20追加 byやまおか 2010/09/21
                            }

                            //Rev23.10 追加 by長野 2015/10/06
                            if (modXrayControl.XrayControl.Up_XrayStatusFLM == 3)
                            {
                                strStatus = StringTable.GC_STS_FLM_NG;
                                IsXrayOk = false;
                            }

                            break;

                        //追加2014/10/07hata_v19.51反映
                        //Titanの場合    'v18.00追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                        case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:
                        case modXrayControl.XrayTypeConstants.XrayTypeSpellman:  //add Rev25.03/Rev25.02 2017/02/05 by chouno
                            if (modSeqComm.MySeq.stsDoorInterlock)
                            {
                                strStatus = StringTable.GC_STS_STANDBY_NG;  //準備未完了
                                //Rev23.40 変更 by長野 2016/06/19
                                ////Rev23.20 準備未完了でもX線のステータスは見る by長野 2016/01/20
                                //if ((modXrayControl.IsXrayError() == true) || (modTitan.Ti_GetCommErrorCode() != 0))
                                //{
                                //    IsXrayOk = false;
                                //}
                            }
                            else if (modSeqComm.MySeq.PcInhibit)
                            {
                                strStatus = StringTable.GC_STS_PANEL_ON;    //パネルON
                                ////Rev23.20 準備未完了でもX線のステータスは見る by長野 2016/01/20
                                //if ((modXrayControl.IsXrayError() == true) || (modTitan.Ti_GetCommErrorCode() != 0))
                                //{
                                //    IsXrayOk = false;
                                //}
                            }
                            else if (modXrayControl.IsXrayError() == true)
                            {
                                //Rev23.40 変更 by長野 2016/06/19
                                ////Rev23.20 通信系のエラー追加(通信していないと制御器のエラーは不明なため優先)
                                //if (modTitan.Ti_GetCommErrorCode() != 0)
                                //{
                                //    strStatus = CTResources.LoadResString(12909);       //接続エラー
                                //}
                                //else
                                //{
                                    strStatus = StringTable.GC_Xray_Error;         //異常
                                //}
                                IsXrayOk = false;
                            }

                            //Rev23.40 変更 by長野 2016/06/19
                            if (modTitan.Ti_GetCommErrorCode() != 0)
                            {
                                strStatus = CTResources.LoadResString(12909);       //接続エラー
                                IsXrayOk = false;
                            }
                            break;

                        //浜ホト以外の場合                                
                        default:

                            if (modXrayControl.IsXrayError())        //v11.3変更 by 間々田 2006/02/24
                            {
                                strStatus = StringTable.GC_Xray_Error;                  //異常
                                IsXrayOk = false;
                            }
                            break;
                    }
                }
            }

            if (string.Empty.Equals(strStatus))
            {
                //v11.5 表示の優先度の変更：「運転準備未完」は「電磁ロック開」のあとにした 2006/04/24
                //                           また、「ｳｫｰﾑｱｯﾌﾟ未完了」は「スタンバイ」のあとにした

                //If Not Myseq.stsRunReadySW Then
                //    strStatus = LoadResString(12758)                    '運転準備未完

                //'If (Myseq.stsDoorInterlock And (scaninh.door_lock = 1)) Or ((Not Myseq.stsDoorKey) And (scaninh.door_lock = 0)) Then
                //If (MySeq.stsDoorInterlock And (scaninh.door_lock = 1)) Or _                                                                       'v15.0削除 by 間々田 2009/01/18 扉インターロックステータスをＸ線のステータスに含めない
                //   ((Not MySeq.stsDoorKey) And (scaninh.door_lock = 0) And (scaninh.door_keyinput = 0)) Then    'v11.43変更 by 間々田 2006/05/08   'v15.0削除 by 間々田 2009/01/18 扉インターロックステータスをＸ線のステータスに含めない
                //    strStatus = LoadResString(12756)                    '扉開                                                                      'v15.0削除 by 間々田 2009/01/18 扉インターロックステータスをＸ線のステータスに含めない

                //ElseIf Not MySeq.stsRunReadySW Then                                    'v15.0削除 by 間々田 2009/01/18 運転準備ステータスをＸ線のステータスに含めない
                //    strStatus = LoadResString(12758)                    '運転準備未完  'v15.0削除 by 間々田 2009/01/18 運転準備ステータスをＸ線のステータスに含めない

                if (IsStandby)
                {
                    strStatus = CTResources.LoadResString(12757);                 //スタンバイ
                }
                else if (myXrayWarmUp == modXrayControl.XrayWarmUpConstants.XrayWarmUpNotComplete)
                {
                    strStatus = StringTable.GC_Xray_WarmUp_NG;                  //ｳｫｰﾑｱｯﾌﾟ未完了
                }
                else if (myXrayWarmUp == modXrayControl.XrayWarmUpConstants.XrayWarmUpFailed)   //v16.01/v17.00追加 byやまおか 2010/02/18
                {
                    strStatus = StringTable.GC_STS_WUP_FAILED;                  //ｳｫｰﾑｱｯﾌﾟ失敗   'v16.01/v17.00追加 byやまおか 2010/02/18
                }
                else if (myXrayWarmUp == modXrayControl.XrayWarmUpConstants.XrayWarmUpNotComplete) // Rev23.20 未完を追加 by長野 2016/01/23
                {
                    strStatus = StringTable.GC_STS_WUP_NOTREADY;
                }
                else
                {
                    strStatus = StringTable.GC_STS_STANDBY_OK;                  //準備完了
                }
            }

            //With lblXray                   '削除 by 間々田 2009/07/31

            //Ｘ線に異常はないか？
            if (IsXrayOk)
            {
                CountXrayError = 0;
                XrayErrorFlag = 0;
                //.Caption = strStatus
                //XrayStatus = strStatus  '変更 by 間々田 2009/07/31 'v17.20削除 ifの外で更新する byやまおか 2010/09/21
            }
            //エラーが５秒未満の場合
            else if (CountXrayError < 5)
            {
                CountXrayError = CountXrayError + 1;        //エラーカウントをカウントアップする

                //スキャンスタートしている場合は即エラーにする 'v17.20追加 byやまおか 2010/09/21
                if ((modCTBusy.CTBusy & modCTBusy.CTScanStart) != 0)
                {
                    //スキャン中に停止していたタイマーを再開する
                    frmMechaControl frmMechaControl = frmMechaControl.Instance;
                    //変更2014/10/07hata_v19.51反映
                    //v19.50 タイマー統合 by長野 2013/12/17
                    //frmMechaControl.tmrMecainf.Enabled = true;
                    modMechaControl.Flg_MechaControlUpdate = true;
                    frmMechaControl.tmrPIOCheck.Enabled = true;

                    //'スキャンストップ指令
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

                    modCT30K.PauseForDoEvents(2);           //2秒待つ        'v17.20追加 byやまおか 2010/09/21
                    //冷却水異常の場合はここでも表示する 'v17.20追加 byやまおか 2010/09/21
                    if (CTSettings.mecainf.Data.coolant_err == 1)
                    {
                        strStatus = StringTable.GC_STS_COOLANT_ERROR;       //冷却水異常
                        IsXrayOk = false;
                    }

                    //エラーメッセージ表示フラグを立てる
                    XrayErrorFlag = 1;
                }
            }
            //エラーが5秒以上の場合  'v17.20追加 byやまおか 2010/09/21
            else
            {
                //エラーメッセージを表示済みでなければフラグを立てる
                //if (XrayErrorFlag != -1)
                //Rev26.10 CT30K起動中は無視 by chouno 2018/01/10
                if (XrayErrorFlag != -1 && modCT30K.CT30KSetup == true)
                {
                    XrayErrorFlag = 1;
                }
            }

            XrayStatus = strStatus;         //v17.20追加 byやまおか 2010/09/21

            //段階ウォームアップ用ステータス確認     'v19.02追加 byやまおか 2012/07/23
            //ステータス未完が5秒続いたとき（段階ウォームアップ中に停止したのを想定）
            if (StepWU_StsCnt > 10)
            {
                //ウォームアップの段階をクリア
                StepWU_CurrentNum = 0;
                //ステータスカウントをクリア
                StepWU_StsCnt = 0;
            }
            //ウォームアップ中に
            else if (StepWU_CurrentNum != 0)
            {
                //ステータスが「ウォームアップ中」「完了」のとき
                if (lblWarmupStatus.Text == StringTable.GC_Xray_WarmUp || lblWarmupStatus.Text == CTResources.LoadResString(12060))
                {
                    //ステータスカウントをクリア
                    StepWU_StsCnt = 0;
                }
                //それ以外（たとえば未完）
                else
                {
                    //ステータスカウントをカウントアップ
                    StepWU_StsCnt = StepWU_StsCnt + 1;
                }
            }
            //通常は
            else
            {
                //ステータスカウントをクリア
                StepWU_StsCnt = 0;
            }

            //'エラーが５秒続いた時、エラーメッセージを表示
            //ElseIf XrayErrorFlag = 0 Then
            //エラーが５秒続いた時、エラーメッセージを表示   'v17.20変更 byやまおか　2010/09/21
            //スキャン中は即エラーを表示
            if (XrayErrorFlag > 0)
            {
                //追加2015/02/09hata
                XrayErrorFlag = -1;                 //表示済み   'v17.20変更 byやまおか 2010/09/21

                //.Caption = strStatus     '異常/過負荷/接続エラー
                //XrayStatus = strStatus  '変更 by 間々田 2009/07/31 'v17.20削除 ifの外で更新する byやまおか 2010/09/21

                //'エラーメッセージ表示：
                //'   X線装置に異常が発生しました。
                //'   X線制御器の表示器に表示されている内容を確認し対処してください
                //'   内容を確認したら、OKボタンをクリックしてください。
                //MsgBox LoadResString(9379), vbExclamation

                //変更2014/10/07hata_v19.51反映
                if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeFeinFocus)  //v15.03変更 byやまおか 2009/11/17
                {
                    //   X線装置に異常が発生しました。
                    //   X線制御器の表示器に表示されている内容を確認し対処してください
                    //   内容を確認したら、OKボタンをクリックしてください。
                    //Interaction.MsgBox(CT30K.My.Resources.str9379, MsgBoxStyle.Exclamation);
                    MessageBox.Show(CTResources.LoadResString(9379), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                //Titanの場合はErrorCodeも表示する   'v18.02追加 byやまおか 2013/05/28 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                }
                      //Rev25.03/Rev25.02 change by chouno 2017/02/05
                else if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) || (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman))
                {
                    //Rev25.03/Rev25.02 WUPフラグはOFF add by chouno 2017/02/15
                    modTitan.Ti_SetWarmingupFlag(0);

                    //通信のエラーと制御のエラーとで分岐させる by長野 2016/01/19
                    int TiErrorNo = 0;
                    int TiCommError = 0;
                    TiErrorNo = modTitan.Ti_GetErrorCode();
                    TiCommError = modTitan.Ti_GetCommErrorCode();
                    if (TiErrorNo != 0)//制御エラー優先で出力
                    {
                        switch (TiErrorNo)
                        {
                            //エラーなし
                            case 0:
                                break;
                            //異常にしないエラーコード
                            case 63:
                            case 64:
                            case 65:
                            case 70:
                            case 106:
                            case 109:
                            case 119:
                            case 121:
                                break;
                            //それ以外のエラーコード
                            default:

                                strTiError = Convert.ToString(TiErrorNo);
                                modSeqComm.MySeq.BitWrite("ScanPCErr", true);
                                MessageBox.Show(CTResources.LoadResString(9374) + "\r\n" + "Error Code " + strTiError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                modSeqComm.MySeq.BitWrite("ScanPCErr", false);
                                modCT30K.UserTiLogDel(524288);
                                //512kB(0.5MB)
                                modCT30K.UserTiLogOut(";" + DateTime.Now.ToString() + "," + strTiError + ",");
                                modCT30K.UserTiLogOut("\r\n");

                            break;
                        }

                    }
                    else
                    {
                        strTiCommError = Convert.ToString(TiCommError);

                        if (modSeqComm.MySeq.stsFDSystemPos == true)
                        {
                            modSeqComm.MySeq.BitWrite("ScanPCErr", true);
                            MessageBox.Show(CTResources.LoadResString(9374) + "\r\n" + "Connection Error Code " + strTiCommError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            modSeqComm.MySeq.BitWrite("ScanPCErr", false);
                        }

                        modCT30K.UserTiLogDel(524288);
                        //512kB(0.5MB)
                        modCT30K.UserTiLogOut(";" + DateTime.Now.ToString() + "," + strTiCommError + ",");
                        modCT30K.UserTiLogOut("\r\n");
                    }
                }
                else
                {
                    //X線装置に異常が発生しました｡
                    //異常の内容を確認してください｡
                    MessageBox.Show(CTResources.LoadResString(9374), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }


                //Call_Feinfocus_Start False
                modXrayControl.XrayControlStart(false);         //v11.3変更 by 間々田 2006/02/20

                //XrayErrorFlag = 1
                //先頭に移動_2015/02/09hata
                //XrayErrorFlag = -1;                 //表示済み   'v17.20変更 byやまおか 2010/09/21
            }

            //ウォームアップ中の場合、Ｘ線ステータスラベルを点滅させる
            //.Visible = IIf(.Caption = GC_Xray_WarmUp, Not .Visible, True)  '削除 by 間々田 2009/07/31

            //End With    
        }


        //*******************************************************************************
        //機　　能： メカ画面からのI.I.動作停止時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void myMechaControl_IIMovingStopped(object sender, EventArgs e)
        {
            //Ｘ線条件設定処理
            //SetXrayCondition GetCmdButton(cmdCondition), GetIINo(), myMechaControl.FIDWithOffset

            //v17.47/v14.53以下に変更 by 間々田 2011/03/09
            int condIndex;
            condIndex = modLibrary.GetCmdButton(cmdCondition);

            if (condIndex == -1)
            {
                condIndex = LastCondIndex;
            }

            //Ｘ線条件設定処理
            SetXrayCondition(condIndex, modSeqComm.GetIINo(), myMechaControl.FIDWithOffset);
        }


        //*******************************************************************************
        //機　　能： ウォームアップ中に操作を禁止する処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Value           [I/ ]           True:有効化 / False:無効化
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V15.10  10/01/21    やまおか    新規作成
        //*******************************************************************************
        private void SetWUControlEnabled(bool Value)
        {
            //Ｘ線条件ボタンを無効化(X線条件を変えさせない)
            cmdCondition0.Enabled = Value;
            cmdCondition1.Enabled = Value;
            cmdCondition2.Enabled = Value;

            frmScanControl frmScanControl = frmScanControl.Instance;
            frmAdjAlignment frmAdjAlignment = frmAdjAlignment.Instance;
            frmTransImage frmTransImage = frmTransImage.Instance;
            frmCTMenu frmCTMenu = frmCTMenu.Instance;
            frmMechaControl frmMechaControl = frmMechaControl.Instance;

            //スキャン条件のプリセット設定を無効化(X線条件を変えさせない)
            frmScanControl.cmdPresetRef.Enabled = Value;

            //Rev20.00 trueの場合、スキャンスタート開始状態かどうかも条件に入れる by長野 2015/03/06
            if (Value == true)
            {
                if ((modCTBusy.CTBusy & modCTBusy.CTScanStart) == 0)
                {
                    //スキャンスタートボタンを無効化
                    frmScanControl.ctbtnScanStart.Enabled = Value;
                }
            }
            else
            {
                frmScanControl.ctbtnScanStart.Enabled = Value;
            }

            //全自動校正ボタンを無効化   'v16.01/v17.00追加 byやまおか 2010/02/10
            frmScanControl.cmdCorrect8.Enabled = Value;

            //校正－詳細ボタンを無効化   'v16.01/v17.00追加 byやまおか 2010/02/10
            frmScanControl.cmdCorrectDetails.Enabled = Value;

            //アライメント調整を無効化   'v16.01/v17.00追加 byやまおか 2010/02/10
            frmAdjAlignment.Enabled = Value;

            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //ライブ画像処理ボタンを無効化
            //    If IsCTmode Then    'v16.01 条件追加 by 山影 10-02-10
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //禁止するときに、ライブ画像処理中なら停止       'v15.11変更 byやまおか 2010/02/04
            if ((Value == false) && (frmTransImage.CaptureOn)) frmTransImage.CaptureOn = false;

            //ライブ画像処理ボタンを無効化
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//          frmCTMenu.Toolbar1.Buttons("LiveImage").Enabled = Value
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
            frmCTMenu.tsbtnLiveImage.Enabled = Value;
            frmScanControl.cmdLive.Enabled = Value;         //v16.30/v17.00追加 byやまおか 2010/02/10

            //動画保存ボタンを無効化     'v17.10追加 byやまおか 2010/08/26
            frmScanControl.cmdSaveMovie.Enabled = Value;

            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //    End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //視野切替ボタンを無効(X線条件L/M/Hが選ばれているとkV/uAを変更してしまうから)
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//          frmCTMenu.Toolbar1.Buttons("I.I.Field").Enabled = Value     'v16.01追加 byやまおか 2010/02/25
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
            frmCTMenu.tsbtnIIField.Enabled = Value;     //v16.01追加 byやまおか 2010/02/25

            //詳細(X線)ボタンを無効化
            cmdDetail.Enabled = Value;          //v16.01/v17.00追加 byやまおか 2010/02/18

            //アライメント調整を無効化
            frmAdjAlignment.Enabled = Value;    //v16.30/v17.00追加 byやまおか 2010/02/18

            //自動スキャン位置指定-透視を無効化
            //Rev20.00 modCTBusyで制御しているため、ここではやならい by長野 2015/02/06
            //frmMechaControl.cmdFromTrans.Enabled = Value;   //v17.10追加 byやまおか 2010/08/10

            //透視画像処理-画像積算ボタンを無効化
            frmScanControl.cmdInteg.Enabled = Value;        //v17.30追加 byやまおか 2010/09/24

            //追加2014/10/07hata_v19.51反映
            //管電圧・管電流の設定           'v18.00追加 byやまおか 2011/03/16 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            cwneKV.Enabled = Value;
            cwsldKV.Enabled = Value;
            cwneMA.Enabled = Value;
            cwsldMA.Enabled = Value;

            //追加2014/10/07hata_v19.51反映
            //フィラメント調整ボタン     'v19.18 追加 byやまおか 2013/09/26
            if ((chkFilament.Visible))
                chkFilament.Enabled = Value;

            //追加2014/10/07hata_v19.51反映
            //ウォームアップ管電圧ボックス 'v19.18追加 byやまおか 2013/09/26
            if ((cwneWarmupSetVolt.Visible))
                cwneWarmupSetVolt.Enabled = Value;

        }


        //*************************************************************************************************
        //機　　能： Viscom用Liveタイマー処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 安全のためsi_xray.confのTIMEOUT_OFFを360000秒→7秒に変えた。
        //           そのため何もアクセスしないと7秒でX線がOFFしてしまう。
        //           対策として、常に管電圧を設定しにいく。インターバル3秒。
        //
        //履　　歴： v17.21  2010/10/06  (検S1)やまおか  新規作成
        //*************************************************************************************************
        //v29.99 内容は既にコメントアウトしてあった（処理のないタイマーがあることになっていた）ので、プロシージャ毎コメントアウト by長野 2013/04/08'''''ここから'''''
        //Private Sub tmrViscomLive_Timer()
        //
        //'    'Viscomのステータスが確実に変わるのを待つ
        //'    'WU完了後にフィラメント調整がある場合、一瞬表示される[ウォームアップ完了]を拾ってしまうことがある
        //'    Sleep (1500)    '1.5秒待つ（tmrUpdateがインターバル1000で動作しているため）
        //'
        //'    'ウォームアップ中は設定にいかない（完了するまで処理が待たされてしまうため）
        //'    If IsDrvReady Or IsWarmupAvailable Then
        //'
        //'        '管電圧を設定する
        //'        SetTargetU CLng(Format$(cwneKV.Value, "0"))
        //'
        //'    End If
        //
        //'    If frmXrayControl.MecaXrayOn = OnStatus Then
        //'        SendCode CODE_XRAY_ON, 1
        //'    End If
        //
        //End Sub
        //v29.99 内容は既にコメントアウトしてあった（処理のないタイマーがあることになっていた）ので、プロシージャ毎コメントアウト by長野 2013/04/08'''''ここから'''''


        //*******************************************************************************
        //機　　能： 英語用レイアウト
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v17.60  2011/05/25 (検S1)長野  新規作成
        //*******************************************************************************
        private void EnglishAdjustLayout()
        {
            int margin;
            margin = 3;

            //Rev20.01 追加 by長野 2015/05/19
            ntbFilter.Height = ntbFilter.Height + 3;
            ntbSetCurrent.Width = ntbSetCurrent.Width - 6;
            ntbActCurrent.Width = ntbActCurrent.Width - 4;
            cmdDetail.Width = 110;
            cmdXrayInfo.Width = 110;

            //ウォームアップフレーム
            //****************************************************************************************
            Label2.Visible = false;
            cwneWarmupSetVolt.Left = Label1.Width + margin;
            Label1.Visible = ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801) ||
                              (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL12721) || //Rev23.10 追加 by長野 2015/10/01
                              (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeFeinFocus) ||
                              (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeViscom));
            Label1.Top = Label2.Top;
            Label1.Left = margin * 2;
            Label2.Top = Label2.Top;
            Label2.Left = Label1.Width + cwneWarmupSetVolt.Width + margin * 3;
            //2014/11/07hata キャストの修正
            //Label2.Width = Label2.Width / 2;
            Label2.Width = Convert.ToInt32(Label2.Width / 2F);
            chkFilament.Height = chkFilament.Height * 2;

            //ステータスフレーム
            //****************************************************************************************
            //cwbtnXray.Width = 120;
            //cwbtnXray.Left = (fraStatus.Width - cwbtnXray.Width) / 2;
            ctLblXray.Width = 120;
            //2014/11/07hata キャストの修正
            //ctLblXray.Left = (fraStatus.Width - ctLblXray.Width) / 2;
            ctLblXray.Left = Convert.ToInt32((fraStatus.Width - ctLblXray.Width) / 2F);

            this.Refresh();
        }


        //*******************************************************************************
        //機　　能： XrayStatusプロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v19.02  2012/07/20  やまおか    新規作成
        //*******************************************************************************
        public int StepWU_CurrentNum
        {
            get
            {
                //現在の段階を返す
                return myStepWU_Num;
            }
            set
            {
                //変更時のみ設定する
                if (myStepWU_Num == value) return;

                //現在の段階を変更する
                myStepWU_Num = value;

                //ログファイル用変数
                //C:\CT\TEMP\WUP\yyyy\WUPyyyymmddhhmmss.log
                //WupLogDir_WUP = modFileIO.FSO.BuildPath(modFileIO.CTUSER, "WUPLOG");
                //WupLogDir_WUP_YYYYMM = modFileIO.FSO.BuildPath(WupLogDir_WUP, DateTime.Now.ToString("yyyyMM"));
                WupLogDir_WUP = Path.Combine(AppValue.CTUSER, "WUPLOG");
                WupLogDir_WUP_YYYYMM = Path.Combine(WupLogDir_WUP, DateTime.Now.ToString("yyyyMM"));

                switch (StepWU_CurrentNum)
                {
                    //終了時または初期状態のときは
                    case 0:

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
                        'ログファイルを閉じる
                        If (WupLogfileNo <> 0) Then
                            '一発書いてから閉じる
                            XrayStatusLogOut_withoutOpen
                            Close WupLogfileNo
                        End If 
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                        //ログファイルを閉じる
                        if (WupLogfileNo != null)
                        {
                            //一発書いてから閉じる
                            XrayStatusLogOut_withoutOpen();
                            WupLogfileNo.Close();
                            WupLogfileNo = null;
                        }
                        break;

                    //1段階または段階ウォームアップじゃないときは
                    case 1:

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
                        'ログファイル用変数
                        WupLogfileNo = FreeFile()
                        WupLogfile = "WUP" & Format$(Now, "yyyymmdd_hhmmss") & ".log"
                        WupLogPath = FSO.BuildPath(WupLogDir_WUP_YYYYMM, WupLogfile)
            
                        'ログフォルダを作成
                        If (Not FSO.FileExists(WupLogPath)) Then
                            'C:\CT\TEMP\WUPを作成
                            If (Not FSO.FolderExists(WupLogDir_WUP)) Then
                                FSO.CreateFolder (WupLogDir_WUP)
                            End If
                            'C:\CT\TEMP\WUP\yyyymmを作成
                            If (Not FSO.FolderExists(WupLogDir_WUP_YYYYMM)) Then
                                FSO.CreateFolder (WupLogDir_WUP_YYYYMM)
                            End If
                        End If
            
                        'ログファイルを作成
                        Open WupLogPath For Append As WupLogfileNo
            
                        'ヘッダを書き込む
                        '経過時間(秒),電圧(kV),電流(uA),ターゲット電流(uA),真空度*10
                        Print #WupLogfileNo, "Time(s),Vol(kV),Cur(uA),TCur(uA),Vac*10"
                        WupLogFirstCount = GetTickCount()
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                        //ログファイル用変数
                        WupLogfile = "WUP" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".log";
                        //WupLogPath = modFileIO.FSO.BuildPath(WupLogDir_WUP_YYYYMM, WupLogfile);
                        WupLogPath = Path.Combine(WupLogDir_WUP_YYYYMM, WupLogfile);

                        //ログフォルダを作成
                        //if (File.Exists(WupLogPath))
                        //Rev20.00 条件を修正 by長野 2014/12/15
                        if (!File.Exists(WupLogPath))
                        {
                            //C:\CT\TEMP\WUPを作成
                            if (!Directory.Exists(WupLogDir_WUP))
                            {
                                Directory.CreateDirectory(WupLogDir_WUP);
                            }
                            //C:\CT\TEMP\WUP\yyyymmを作成
                            if (!Directory.Exists(WupLogDir_WUP_YYYYMM))
                            {
                                Directory.CreateDirectory(WupLogDir_WUP_YYYYMM);
                            }
                        }

                        //ログファイルを作成
                        //変更2015/01/22hata
                        //WupLogfileNo = new StreamWriter(WupLogPath, true);
                        WupLogfileNo = new StreamWriter(WupLogPath, true, Encoding.GetEncoding("shift-jis"));
                        WupLogfileNo.AutoFlush = true; //Rev20.00 追加 by長野 2015/01/29

                        //ヘッダを書き込む
                        //経過時間(秒),電圧(kV),電流(uA),ターゲット電流(uA),真空度*10
                        WupLogfileNo.WriteLine("Time(s),Vol(kV),Cur(uA),TCur(uA),Vac*10");
                        WupLogFirstCount = Winapi.GetTickCount();

                        break;

                    //その他のとき
                    default:
                        //何もしない        
                        break;
                }
            }
        }


        //*******************************************************************************
        //機　　能： X線ステータスをログファイルへ出力する関数
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 別でログファイルをOpenする必要がある
        //
        //履　　歴： v19.02  2012/07/20  やまおか    新規作成
        //*******************************************************************************
        private int XrayStatusLogOut_withoutOpen()
        {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
            'ウォームアップ中だけ
            If ((StepWU_CurrentNum <> 0) And (WupLogfileNo <> 0)) Then
                'ログファイルに書き込む
                '経過時間,電圧,電流,ターゲット電流,真空度*10
                '(真空度はプロットしたときに縦軸が見やすくするために10倍する)
                Print #WupLogfileNo, Format$((GetTickCount() - WupLogFirstCount) * 0.001, "0.00") & "," & ntbActVolt.Value & "," & ntbActCurrent.Value & "," & ntbTargetCurrent.Value & "," & ntbVacSVV.Value * CVar(10)
            End If
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            //ウォームアップ中だけ
            if ((StepWU_CurrentNum != 0) && (WupLogfileNo != null))
            {
                //ログファイルに書き込む
                //経過時間,電圧,電流,ターゲット電流,真空度*10
                //(真空度はプロットしたときに縦軸が見やすくするために10倍する)
                WupLogfileNo.WriteLine(((Winapi.GetTickCount() - WupLogFirstCount) * 0.001).ToString("0.00") + ","
                                        + ntbActVolt.Value.ToString() + ","
                                        + ntbActCurrent.Value.ToString() + ","
                                        + ntbTargetCurrent.Value.ToString() + ","
                                        + (ntbVacSVV.Value * 10).ToString());
            }

            return 0;
        }



        //X線MechData更新
        private void XrayMechDataUpdate()
        {
            if (InvokeRequired)
            {
                //Invoke(new XrayMechDataUpdateDelegate(XrayMechDataUpdate));
                BeginInvoke(new XrayMechDataUpdateDelegate(XrayMechDataUpdate));
                return;
            }

            try
            {
                //logOut "UC_Feinfocus_MechDataDisp"
                modCT30K.logOut("UC_XrayCtrl_MechDataDisp");    //v16.20変更 byやまおか 2010/04/21


                //管電圧／管電流（実測値）
                switch (modXrayControl.XrayType)
                {
                    //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421
                    //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL10801    'v15.10変更 byやまおか 2009/10/07
                    //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL10801, XrayTypeHamaL8601 'v16.03/v16.20変更 byやまおか 2010/03/03
                    //Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL10801, XrayTypeHamaL8601, XrayTypeHamaL9421_02T  'v16.30 02T追加 byやまおか 2010/05/21
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9191:
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL8601:
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02:     //v17.71 追加 by長野 2012/03/14
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181_02: //追加2014/11/05hata L9181-02に対応
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL12721://Rev23.10 追加 by長野 2015/10/01
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL10711://Rev23.10 追加 by長野 2015/10/01

                        ntbActVolt.Value = (decimal)myXrayMechData.m_Voltage;              //管電圧（実測値）
                        ntbActCurrent.Value = (decimal)myXrayMechData.m_Curent;            //管電流（実測値）
                        break;
                }

                //Ｘ線オン・オフ状態
                MecaXrayOn = (modCT30K.OnOffStatusConstants)myXrayMechData.m_XrayOnSet;

                //アベイラブル
                MecaXrayAvailable = (modCT30K.OnOffStatusConstants)myXrayMechData.m_XAvail;

                //浜ホト160kV,230kVの時                                            v9.5 追加 by 間々田 2004/09/13
                //If XrayType = XrayTypeHamaL9191 Then
                if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL9191) ||
                    //(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801))       //v15.10変更 byやまおか 2009/10/07
                    (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10711) ||//Rev23.10 追加 by長野 2015/10/01
                    (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL12721) ||//Rev23.10 追加 by長野 2015/10/01
                    (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801))
                {
                    //真空度
                    UpdateVacuumSVC(myXrayMechData.m_XrayVacuumSVC);
                    ntbVacSVV.Value = (decimal)modXrayControl.XrayControl.Up_XrayStatusSVV;     //v19.02追加 byやまおか 2012/07/20

                    //ターゲット電流
                    ntbTargetCurrent.Value = (decimal)myXrayMechData.m_XrayTargetInfSTG;                                       //小数点以下１桁表示
                    //ntbTargetCurrent.BackColor = IIf((.m_XrayTargetLimit = 1) And (.m_XrayOnSet = 1), SunsetOrange, vbWhite) '背景色
                    ntbTargetCurrent.BackColor = (((myXrayMechData.m_XrayTargetLimit == 1) && (myXrayMechData.m_XrayOnSet == 1)) ? modCT30K.SunsetOrange : SystemColors.Control);    //v15.10変更 byやまおか 2009/10/15
                }


                //焦点：ウォームアップが完了かつX線OFFの時だけ焦点ボタンを使用可にする
                switch (modXrayControl.XrayType)
                {
                    //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421
                    //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL10801   'v15.10変更 byやまおか 2009/10/07
                    //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL10801, XrayTypeHamaL9421_02T  'v16.30 02T追加 byやまおか 2010/05/21
                    case modXrayControl.XrayTypeConstants.XrayTypeKevex:
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9191:
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02:       //v17.71 追加 by長野 2012/03/14
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181_02: //追加2014/11/05hata L9181-02に対応
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL12721://Rev23.10 追加 by長野 2015/10/01
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL10711://Rev23.10 追加 by長野 2015/10/01

                        fraFocus.Enabled = (modXrayControl.XrayWarmUp() == modXrayControl.XrayWarmUpConstants.XrayWarmUpComplete) && !(myXrayMechData.m_XrayOnSet == 1);

                        int xfocus = modXrayControl.XrayControl.Up_Focussize - 1;
                        if (xfocus >= 0)
                            modLibrary.SetCmdButton(cmdFocus, Convert.ToInt32(xfocus), ControlEnabled: true);
                        break;
                }

                //X線ステータスをログファイルに書き込む   'v19.02追加 byやまおか 2012/07/20
                XrayStatusLogOut_withoutOpen();
            }
            catch
            {
            }

        }

        //X線StatusValue更新
        private void XrayStatusValueUpdate()
        {
            if (InvokeRequired)
            {
                //Invoke(new XrayStatusValueUpdateDelegate(XrayStatusValueUpdate));
                BeginInvoke(new XrayStatusValueUpdateDelegate(XrayStatusValueUpdate));
                return;
            }

            try
            {
                 //logOut "UC_Feinfocus_StatusValueDisp"
                modCT30K.logOut("UC_XrayCtrl_StatusValueDisp");     //v16.20変更 byやまおか 2010/04/21

                //ウォームアップ情報の更新
                if (modXrayControl.XrayType != modXrayControl.XrayTypeConstants.XrayTypeFeinFocus)
                {
                    myXrayWarmUp = modXrayControl.XrayWarmUp();
                    UpdateWarmUp();
                }

            }
            catch
            {
            }

        }

        //X線Valueデータ更新
        private void XrayValueUpdate()
        {
            //string msg = "";

            if (InvokeRequired)
            {
                //Invoke(new XrayValueUpdateDelegate(XrayValueUpdate));
                BeginInvoke(new XrayValueUpdateDelegate(XrayValueUpdate));
                return;
            }

            try
            {
                Debug.Print("XrayType = " + modXrayControl.XrayType);
                Debug.Print("cwsldKV.Minimum = " + cwsldKV.Minimum.ToString());
                Debug.Print("cwsldKV.Maximum = " + cwsldKV.Maximum.ToString());

                //logOut "UC_Feinfocus_XrayValueDisp"
                modCT30K.logOut("UC_XrayCtrl_XrayValueDisp");   //v16.20変更 byやまおか 2010/04/21

                //v17.71 L8121_02で確実に管電流、管電圧を変更するための対策  by長野 2012-03-24
                //設定管電圧・設定管電流
                if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02)
                {
                    myXrayValue.m_kVSet = modXrayControl.XrayControl.Up_XR_VoltSet;
                    myXrayValue.m_mASet = modXrayControl.XrayControl.Up_XR_CurrentSet;
                }

                ntbSetVolt.Value = (decimal)myXrayValue.m_kVSet;
                ntbSetCurrent.Value = (decimal)myXrayValue.m_mASet;

                //Rev20.01 追加 by長野 2015/06/10
                //設定しようとした値と設定後の制御器がもつ設定値が異なる場合はmyTempSetCurrentとmyTempSetVoltを初期化する。
                if (ntbSetVolt.Value != (decimal)myXrayValue.m_kVSet)
                {
                    modXrayControl.TempSetVolt = -1;
                }
                if (ntbSetCurrent.Value != (decimal)myXrayValue.m_mASet)
                {
                    modXrayControl.TempSetCurrent = -1;
                }

                //使っていないので削除2015/02/02hata
                //byEvent = false;  


                //電圧スライダ設定
                //2014/11/07hata キャストの修正
                //int kVval = (int)((decimal)myXrayValue.m_kVSet / cwneKV.Increment);
                int kVval = Convert.ToInt32((decimal)myXrayValue.m_kVSet / cwneKV.Increment);
                //cwsldKV.Value = val;
                if (kVval < cwsldKV.Minimum)
                {
                    cwsldKV.Value = cwsldKV.Minimum;
                }
                //変更2014/12/22hata_dNet
                //else if (kVval > cwsldKV.Maximum)
                //{
                //    cwsldKV.Value = cwsldKV.Maximum;
                //}
                else if (kVval > (cwsldKV.Maximum + cwsldKV.LargeChange - 1))
                {
                    cwsldKV.Value = cwsldKV.Maximum + cwsldKV.LargeChange - 1;
                }
                else
                {
                    cwsldKV.Value = kVval;
                }
                //if (cwneKV.Value != cwsldKV.Value * cwneKV.Increment) cwneKV.Value = cwsldKV.Value * cwneKV.Increment;  //テキストボックス側にも確実に入れる 'v17.47/v17.53追加 2011/03/09 by 間々田
                if (cwneKV.Value != cwsldKV.Value * cwneKV.Increment)  //テキストボックス側にも確実に入れる 'v17.47/v17.53追加 2011/03/09 by 間々田
                {
                    if ((cwsldKV.Value >= cwneKV.Minimum) && (cwsldKV.Value <= cwneKV.Maximum))
                    {
                        cwneKV.Value = cwsldKV.Value;
                    }
                }    
   
                //電流スライダ設定
                //cwsldMA.Value = (int)((decimal)Val1.m_mASet / cwneMA.Increment);
                //2014/11/07hata キャストの修正
                //int MAval = (int)((decimal)myXrayValue.m_mASet / cwneMA.Increment);
                int MAval = Convert.ToInt32((decimal)myXrayValue.m_mASet / cwneMA.Increment);
                if (MAval < cwsldMA.Minimum)
                {
                    cwsldMA.Value = cwsldMA.Minimum;
                }
                //変更2014/12/22hata_dNet
                //else if (MAval > cwsldMA.Maximum)
                //{
                //    cwsldMA.Value = cwsldMA.Maximum;
                //}
                else if (MAval > (cwsldMA.Maximum + cwsldMA.LargeChange - 1))
                {
                    cwsldMA.Value = cwsldMA.Maximum + cwsldMA.LargeChange - 1;
                }
                else
                {
                    cwsldMA.Value = MAval;
                }
                //if (cwneMA.Value != cwsldMA.Value * cwneMA.Increment) cwneMA.Value = cwsldMA.Value * cwneMA.Increment;  //テキストボックス側にも確実に入れる 'v17.47/v17.53追加 2011/03/09 by 間々田
                if (cwneMA.Value != cwsldMA.Value * cwneMA.Increment)   //テキストボックス側にも確実に入れる 'v17.47/v17.53追加 2011/03/09 by 間々田
                {
                    if ((cwsldMA.Value >= cwneMA.Minimum) && (cwsldMA.Value <= cwneMA.Maximum))
                    {
                        cwneMA.Value = cwsldMA.Value;
                    }
                }

                //使っていないので削除2015/02/02hata
                //byEvent = true; 
 
            }
            catch
            {
            }

        }


        //X線UserValueデータ更新
        private void XrayUserValueUpdate()
        {
            if (InvokeRequired)
            {
                //Invoke(new XrayUserValueUpdateDelegate(XrayValueUpdate));
                //BeginInvoke(new XrayUserValueUpdateDelegate(XrayValueUpdate));
                //Rev20.01 修正 by長野 2015/05/29
                BeginInvoke(new XrayUserValueUpdateDelegate(XrayUserValueUpdate));
                return;
            }

            try
            {
                //logOut "UC_Feinfocus_UserValueDisp"
                modCT30K.logOut("UC_XrayCtrl_UserValueDisp");   //v16.20変更 byやまおか 2010/04/21

                //焦点情報を表示
                switch (modXrayControl.XrayType)
                {
                    //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421
                    //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL10801   'v15.10変更 byやまおか 2009/10/13
                    //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421     'v16.03/v16.20変更 byやまおか 2010/03/03
                    //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL9421_02T  'v16.30 02T追加 byやまおか 2010/05/21
                    case modXrayControl.XrayTypeConstants.XrayTypeKevex:
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9191:
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02:     //v17.71 追加 by長野 2012/03/14
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181_02: //追加2014/11/05hata L9181-02に対応
                    case modXrayControl.XrayTypeConstants.XrayTypeHamaL10711:   //Rev23.10 追加 by長野 2015/10/01
                        //SetCmdButton cmdFocus, CLng(Val1.m_XrayFocusSize)
                       
                        int xfocus = myXrayUserValue.m_XrayFocusSize - 1;
                        if (xfocus >= 0)
                            modLibrary.SetCmdButton(cmdFocus, Convert.ToInt32(xfocus), ControlEnabled: true);     //v11.41変更 by 間々田 2006/03/29
                        break;
                }

            }
            catch
            {
            }

        }

        //X線Error更新
        private void XrayErrSetUpdate()
        {
            if (InvokeRequired)
            {
                //Invoke(new XrayErrSetUpdateDelegate(XrayErrSetUpdate));
                BeginInvoke(new XrayErrSetUpdateDelegate(XrayErrSetUpdate));
                return;
            }

            try
            {
                //logOut "UC_Feinfocus_ErrSetDisp"
                modCT30K.logOut("UC_XrayCtrl_ErrSetDisp");      //v16.20変更 byやまおか 2010/04/21


                int idat1;

                if (XrayCtrl_Error) return;
                XrayCtrl_Error = true;

                //ActiveX制御ｴﾗｰの内容を表示
                idat1 = myXrayErrSet.m_ErrNO;



                switch (idat1)
                {
                    //正常の場合、抜ける
                    case 0:
                        return;

                    //ｷｰｽｲｯﾁの切り替えに反応してしまう為、このｴﾗｰは無視する  'V3.0 append by 鈴山 2000/09/28
                    case 20:
                        break;

                    //フィラメント断線が出てしまうため、このエラーは無視する 'v16.10/v17.00追加 byやまおか 2010/03/17
                    case 24:
                        break;

                    //定義されているエラーの場合
                    //Case 4, 7, 12, 21 To 25
                    case 4:
                    case 7:
                    case 12:
                    case 21:
                    case 22:
                    case 23:
                    case 25:    //v16.10/v17.00変更 byやまおか 2010/03/17
                    case 26:    //Rev20.00 追加 by長野 2015/04/10
                    case 27:    //Rev20.00 追加 by長野 2015/04/10
                    case 31:    //Rev20.00 追加 by長野 2015/04/10
                    case 41:    //Rev20.00 追加 by長野 2015/04/10
                    case 42:    //Rev20.00 追加 by長野 2015/04/10
                    case 43:    //Rev20.00 追加 by長野 2015/04/10
                    case 44:    //Rev20.00 追加 by長野 2015/04/10
                    case 45:    //Rev20.00 追加 by長野 2015/04/10
                    case 46:    //Rev20.00 追加 by長野 2015/04/10
                    case 47:    //Rev20.00 追加 by長野 2015/04/10
                    case 48:    //Rev20.00 追加 by長野 2015/04/10




//#if !(DEBUG || DebugOn)   //変更2015/02/09hata
#if (!DEBUG || (DEBUG && !XrayErrorDispOff))                        
                    
                        //追加2015/03/11hata
                        //スキャン中はエラーメッセージを表示しない
                        if (!Convert.ToBoolean(modCTBusy.CTBusy & modCTBusy.CTScanStart))
                        {
                            //Rev20.00 オーナーフォームをfrmCTMenuにする by長野 2015/03/11
                            MessageBox.Show(frmCTMenu.Instance,CTResources.LoadResString(StringTable.IDS_ErrorNum) + idat1.ToString() + "\r\n" + "\r\n" +
                                                CTResources.LoadResString(9700 + idat1), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

#endif
                        break;

                    case 51://X線通信ライン異常     //追加2015/04/03hata
                        
                        if(!Convert.ToBoolean(modCTBusy.CTBusy & modCTBusy.CTScanStart) & (idat1 == 51))    //X線通信ライン異常
                        {
                            modCT30K.ErrMessage(9700 + idat1, Icon: MessageBoxIcon.Error);
                            Environment.Exit(0);
                        }
                        break;

                    //定義されていないエラーの場合
                    default:

                        MessageBox.Show(CTResources.LoadResString(StringTable.IDS_ErrorNum) + idat1.ToString() + "\r\n" + "\r\n" +
                                        CTResources.LoadResString(StringTable.IDS_UnkownError), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                        break;
                }



                //エラーステータスに対する確認応答
                idat1 = 1;                               //v9.5 追加 by 間々田 2004/09/16
                modXrayControl.XrayControl.MessageOk_Set(idat1);
            }
            catch
            {
            }
            finally
            {
                XrayCtrl_Error = false;
            }

        }

        //追加2014/10/07hata_v19.51反映
        //*************************************************************************************************
        //機　　能： Titan用x_off.csv更新タイマー処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： x_off.csvは基本的にX線OFF時に更新する。
        //           しかし、OFF命令せずにOFFしてしまった場合に更新できないため、
        //           X線ON中は定期的にx_off.csvを更新する。
        //
        //履　　歴： v18.00  2011/03/26  (検S1)やまおか  新規作成
        //*************************************************************************************************
        private void tmrTitanUpdate_Tick(System.Object eventSender, System.EventArgs eventArgs)
        {
            //v19.50 ウォームアップ中は、VCの中でも書き込むためバッティングしないようにする by長野 2014/02/05
            if ((modTitan.Flg_TiWarmingUp == false) && !modXrayControl.IsXrayError() && (modTitan.Ti_GetCommErrorCode() == 0) && modSeqComm.MySeq.stsFDSystemPos == true)
            {
                if ((myXrayAvailable == modCT30K.OnOffStatusConstants.OnStatus))
                //modTitan.Ti_UpdateXoffcsv();
                //Rev23.20 追加 2世代・3世代兼用の場合は、シーケンサに400以上or430以上のX線が出せるかどうかを通知する by長野 2016/01/18
                {
                    modTitan.Ti_UpdateXoffcsv();
                    //if (CTSettings.scaninh.Data.ct_gene2and3 == 0)
                    //{
                    //    modTitan.SendSeq_Ti_XoffVolt();
                    //}
                }
            }

        }

        //Rev23.40 追加 by長野 2016/06/19
        //frmXrayControlのX線制御系コントロールを更新するだけ
        internal void UpdateGeCwneKVMA(float volt, float anpere)
        {
            //Rev24.00 追加 by長野 2016/06/13
            //追加2014/10/07hata_v19.51反映
            //v18.00追加 byやまおか 2011/06/24 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //Rev25.03/Rev25.02 change by chouno 2017/02/05
            if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) || (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman))
            {

                //変更2015/02/02hata_Max/Min範囲のチェック
                //cwneKV.Value = (decimal)theVolt;
                ////2014/11/07hata キャストの修正
                ////cwsldKV.Value = (int)theVolt;
                //cwsldKV.Value = Convert.ToInt32(theVolt);
                //cwneMA.Value = (decimal)theCurrent;
                ////2014/11/07hata キャストの修正
                ////cwsldMA.Value = (int)theCurrent;
                //cwsldMA.Value = Convert.ToInt32(theCurrent);
                cwneKV.Value = modLibrary.CorrectInRange((decimal)volt, cwneKV.Minimum, cwneKV.Maximum);
                cwsldKV.Value = Convert.ToInt32(cwneKV.Value);
                cwneMA.Value = modLibrary.CorrectInRange((decimal)anpere, cwneMA.Minimum, cwneMA.Maximum);
                //cwsldMA.Value = Convert.ToInt32(cwneMA.Value);
                //Rev23.20 変更 by長野 2015/12/23 by長野
                cwsldMA.Value = Convert.ToInt32(cwneMA.Value / cwneMA.Increment);
            }
        }

        //*******************************************************************************
        //機　　能： Ｘ線ウォームアップ前の管電圧・管電流に戻す(浜ホトL10801用)
        //
        //           変数名          [I/O] 型        内容C:\CT30Kv23.20\CT30K\Modules\modXrayControl.cs
        //引　　数： なし
        //戻 り 値： なし            [I/O]
        //
        //補　　足： なし
        //
        //履　　歴： v21.01 2015/07/23  (検S1)長野  新規作成
        //           
        //*******************************************************************************
        private void bakWUPXrayCondition()
        {
            //WUP後、X線制御ソフトを介さずに制御器の設定値が変わる。
            //ソフト内部で持っている前回設定値は、WUP前のままの状態。
            //そのため、WUP後に設定される20kV、0μAを一度送信し、
            //その後に、WUP前の設定値を入れる。

            modXrayControl.SetVolt(20);
            modCT30K.PauseForDoEvents(1);
            modXrayControl.SetCurrent(0);
            modCT30K.PauseForDoEvents(1);


            if (modXrayControl.XrayControl.Up_XrayStatusSMV <= bakWUPkV)
            {
                bakWUPkV = (int)modXrayControl.XrayControl.Up_XrayStatusSMV;
            }
            else if (modXrayControl.XrayControl.Up_XR_Min_kV >= bakWUPkV)
            {
                bakWUPkV = (int)modXrayControl.XrayControl.Up_XR_Min_mA;
            }
            modXrayControl.SetVolt(bakWUPkV);
            modCT30K.PauseForDoEvents(1);
            if (modXrayControl.XrayControl.Up_XR_Max_mA <= bakWUPmA)
            {
                bakWUPmA = modXrayControl.XrayControl.Up_XR_Max_mA;
            }
            else if (modXrayControl.XrayControl.Up_XR_Min_mA >= bakWUPmA)
            {
                bakWUPmA = modXrayControl.XrayControl.Up_XR_Min_mA;
            }
            modXrayControl.SetCurrent(bakWUPmA);

            modCT30K.PauseForDoEvents(1);
        }
    }
}