using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using CTAPI;
namespace CT30K
{
    public partial class frmXrayWarning : Form
    {
        ///* ************************************************************************** */
        ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000 Ver23.21                */
        ///* 客先　　　　： ?????? 殿                                                   */
        ///* プログラム名： frmXwarWarning                                              */
        ///* 処理概要　　：                                                             */
        ///* 注意事項　　： デバッグ時のみ使用                                          */
        ///* -------------------------------------------------------------------------- */
        ///* 適用計算機　： DOS/V PC                                                    */
        ///* ＯＳ　　　　： Windows7 (SP1) 64bit                                        */
        ///* コンパイラ　： VS2010(SP1)                                                 */
        ///* -------------------------------------------------------------------------- */
        ///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
        ///*                                                                            */
        ///* V23.21      16/02/18   (検S1)長野           32bitソフトを移植              */
        ///*                                                                            */
        ///* -------------------------------------------------------------------------- */
        ///* ご注意：                                                                   */
        ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
        ///*                                                                            */
        ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2016                 */
        ///* ************************************************************************** */

        //警告音間隔(msec)
        private int XBeepInterval = 1000;
        //監視間隔(msec)
        private int SupervisoryInterval = 100;
        //X線開始時間(msec)
        private int XStartTime = 0;
        //フォーム表示時間(msec)
        private int frmShowTime = 0;
        //X線警告時間(msec)
        private int XWarnTime = 0;
        //X線警告返答制限時間(msec)
        private int XWarnResTime = 0;

        private modCT30K.OnOffStatusConstants myXWarningOn;

        private frmXrayControl myfrmXrayControl;

        private bool XrayForcedStopMessageFlg = false;
        private bool KeepXrayFlg = false;

        public frmXrayWarning()
        {
            InitializeComponent();
        }

        private static frmXrayWarning _Instance = null;

        /// <summary>
        /// 
        /// </summary>
        public static frmXrayWarning Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmXrayWarning();
                }

                return _Instance;
            }
        }

        //'*******************************************************************************
        //'機　　能： X線アベイラブルON時のイベント
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v16.01  10/02/25   (検SS)山影   新規作成
        //'*******************************************************************************
        public void myfrmXrayControl_XrayAvailableOn(object sender, EventArgs e)
        {
            //'機能ONの時
            //if (!modHighSpeedCamera.IsHSCmode)
            //change by chouno 2019/02/12 Rev26.20
            if (!modHighSpeedCamera.IsHSCmode && !modHighSpeedCamera.IsDropTestmode)
            {
                return;
            }
            if (myXWarningOn == modCT30K.OnOffStatusConstants.OnStatus)
            {
                if (KeepXrayFlg == false)
                {
                    XStartTime = Winapi.GetTickCount(); //'X線照射開始時間更新
                    tmrSuperVisoryTimer.Enabled = true;
                }
            }
        }

        //'*******************************************************************************
        //'機　　能： 機能ON/OFF
        //'
        //'           変数名          [I/O] 型                内容
        //'引　　数： theStatus       OnOffStatusConstants
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v16.01  10/02/03   (検SS)山影   新規作成
        //'*******************************************************************************
        public modCT30K.OnOffStatusConstants XWarningOn
        {
            get
            {
                return myXWarningOn;
            }
            set
            {
                //高速時のみ有効
                //if(!modHighSpeedCamera.IsHSCmode)
                //change by chouno 2019/02/12 Rev26.20
                if (!modHighSpeedCamera.IsHSCmode && !modHighSpeedCamera.IsDropTestmode)
                {
                    return;
                }
                //変更時のみ設定
                if(myXWarningOn == value)
                {
                    return;
                }
                //内部変数に記憶
                myXWarningOn = value;

                //Rev23.21 アベイラブルからカウントするので、ここではない。 by長野 2016/02/23
                //tmrSuperVisoryTimer.Enabled = (myXWarningOn == modCT30K.OnOffStatusConstants.OnStatus);

                ////X線アベイラブルONした時からの計測に変更したが、念のため残しておく
                ////X線照射開始時間更新
                //XStartTime = Winapi.GetTickCount();

                //警告音停止
                //modSound.PlayXrayOnWarningSoundLoopStop();

                if(myXWarningOn == modCT30K.OnOffStatusConstants.OffStatus)
                {
                    tmrSuperVisoryTimer.Enabled = false;
                    
                    //'警告音停止
                    StopSound();

                    KeepXrayFlg = false;

                    this.Hide();
                }
            }
        }
    
        //'*******************************************************************************
        //'機　　能： Yesクリック時
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v16.01  10/02/03   (検SS)山影   新規作成
        //'*******************************************************************************
        private void cmdYes_Click(object sender, EventArgs e)
        {
            KeepXray();
        }
        //'*******************************************************************************
        //'機　　能： Noクリック時
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v16.01  10/02/03   (検SS)山影   新規作成
        //'*******************************************************************************
        private void cmdNo_Click(object sender, EventArgs e)
        {
            StopXray();
        }
        //'*******************************************************************************
        //'機　　能： X線停止処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v16.01  10/02/03   (検SS)山影   新規作成
        //'*******************************************************************************
        private void KeepXray()
        {
            //'警告メッセージを隠す
            this.Hide();
    
            //'警告音停止
            StopSound();

            //Rev26.40(特) 開始時刻を加算 by chouno 2019/03/25
            //XStartTime = XStartTime + XWarnTime;
            //Rev26.40(特) ここから再開 by chouno 2019/03/25
            XStartTime = Winapi.GetTickCount();

            //Rev26.40 add X線警告表示モードの切替 by chouno 2019/03/25
            //ここでtmrSuperVisoryTimerをfalseにしなかった場合は、x_warn_time毎に警告が出る
            if (CTSettings.iniValue.HSCWarningMessageMethod == 1)
            {
                tmrSuperVisoryTimer.Enabled = false;
            }

            KeepXrayFlg = true;
        }
        //'*******************************************************************************
        //'機　　能： 警告音停止
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v16.01  10/02/15   山影      新規作成
        //'*******************************************************************************
        private void StopSound()
        {
            modSound.PlayXrayOnWarningSoundLoopStop();

            //tmrBeep.Enabled = false;
    
        }
        //'*******************************************************************************
        //'機　　能： X線停止処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v16.01  10/02/03   (検SS)山影   新規作成
        //'*******************************************************************************
        private void StopXray()
        {
            //'X線オフ処理
            modXrayControl.XrayOff();

            //'警告音停止
            StopSound();

            tmrSuperVisoryTimer.Enabled = false;

            KeepXrayFlg = false;

            this.Hide();
        }
        //'*******************************************************************************
        //'機　　能： 監視タイマー
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v16.01  10/02/03   (検SS)山影   新規作成
        //'*******************************************************************************
        private void tmrSuperVisoryTimer_Tick(object sender, EventArgs e)
        {
            if (XrayForcedStopMessageFlg == false)
            {
                int NowTime = 0;
                //現在時刻を取得
                NowTime = Winapi.GetTickCount();

                //'警告表示からX線警告返答時間を過ぎた場合、X線を強制停止

                if ((NowTime - frmShowTime > XWarnResTime) && this.Visible)
                {
                    XrayForcedStopMessageFlg = true;
                    StopXray();
                    MessageBox.Show(CTResources.LoadResString(StringTable.IDS_XrayStopMessage)); //'X線強制停止メッセージ
                    XrayForcedStopMessageFlg = false;
                    return;
                }

                //'X線警告時間を過ぎた場合、警告表示
                //if ((NowTime - XStartTime) > XWarnTime)
                //Rev26.40 既に表示中の場合は表示はしない
                if (((NowTime - XStartTime) > XWarnTime) && (this.Visible == false))
                {
                    //'警告表示時間の取得
                    frmShowTime = Winapi.GetTickCount();

                    //Rev26.40(特) ここで加算してはいけない。回答待ち中もタイマーを動いている。
                    //XStartTime = XStartTime + XWarnTime;

                    //'警告音アラーム開始
                    modSound.PlayXrayOnWarningSoundLoopStart();

                    //'フォーム表示
                    this.Show(frmCTMenu.Instance);
                }
            }
        }

        private void tmrBeep_Tick(object sender, EventArgs e)
        {
            modSound.Beep(500, 500);
        }

        //'*******************************************************************************
        //'機　　能： フォーム初期化
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v16.01  10/02/03   (検SS)山影   新規作成
        //'*******************************************************************************
        public void frmXrayWarning_Init()
        {
            Init();
        }
        public void Init()
        {
            this.Visible = false;

            //X線コントロールフォームへの参照
            myfrmXrayControl = frmXrayControl.Instance;

            myfrmXrayControl.XrayAvailableOn += new EventHandler(myfrmXrayControl_XrayAvailableOn);

            XWarnTime = CTSettings.hscpara.Data.x_warn_time * 1000; //X線警告時間(msec)
            XWarnResTime = CTSettings.hscpara.Data.x_warn_res_limit * 1000; //X線警告返答制限時間(msec)

            SetCaption();

            tmrSuperVisoryTimer.Enabled = false;
            tmrSuperVisoryTimer.Interval = SupervisoryInterval;

            tmrBeep.Enabled = false;
            tmrBeep.Interval = XBeepInterval;

            KeepXrayFlg = false;
        }

        //'*******************************************************************************
        //'機　　能： 各コントロールのキャプションに文字列をセットする
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v16.01  10/02/03   (検SS)山影   新規作成
        //'*******************************************************************************
        private void SetCaption()
        {
            //'Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);

            lblXWarnMessage.Text = StringTable.GetResString(StringTable.IDS_XrayWarningMessage,((int)(XWarnTime / 1000)).ToString());
        }
        //'*******************************************************************************
        //'機　　能： フォームアンロード
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v16.01  10/02/03   (検SS)山影   新規作成
        //'*******************************************************************************
        private void frmXrayWarning_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            myfrmXrayControl.XrayAvailableOn -= new EventHandler(myfrmXrayControl_XrayAvailableOn);

            //X線コントロールフォームへの参照破棄
            myfrmXrayControl = null;

            //'タイマーの停止
            tmrSuperVisoryTimer.Enabled = false;
    
            //'念のため
            modSound.PlayXrayOnWarningSoundLoopStop();       
        }

        private void frmXrayWarning_Load(object sender, EventArgs e)
        {

        }
    }
}
