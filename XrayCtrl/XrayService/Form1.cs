using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XrayCtrl;

namespace XrayService
{
    public partial class Form1 : Form
    {
        public static XrayCtrl.clsTActiveX UC_XrayCtrl;     //X線制御            'V9.5 追加 by 間々田 2004/09/17 modCT30Kから移動
        clsTActiveX.XrayValueSet XrayVal = new clsTActiveX.XrayValueSet();     //v16.20変更 byやまおか 2010/04/21

        delegate void XrayMechDataUpdateDelegate();
        clsTActiveX.MechData XrayMechData;

        delegate void XrayStatusValueUpdateDelegate();
        clsTActiveX.StatusValue XrayStatusValue;
        
        delegate void XrayValueUpdateDelegate();
        clsTActiveX.XrayValue XrayValueData;

        delegate void XrayErrorDelegate();
        clsTActiveX.ErrSet XrayErrorData;

        delegate void UserValueUpdateDelegate();
        clsTActiveX.UserValue UserValueData;


        //Rev test
        delegate void XrayUserValueSetDlegate();
        clsTActiveX.UserValueSet UserValue;

        int XrayPtr = -1;
        int XrayProc = -1;


        delegate void XrayStatus3ValueDispDlegate();
        clsTActiveX.udtXrayStatus3ValueDisp Status3Value;

        public Form1()
        {
            InitializeComponent();

            UC_XrayCtrl = new XrayCtrl.clsTActiveX();
            //イベント設定
            UC_XrayCtrl.XrayValueDisp += new clsTActiveX.XrayValueDispEventHandler(UC_XrayCtrl_XrayValueDisp);
            //クラスオブジェクトを参照
            UC_XrayCtrl.ErrSetDisp += new clsTActiveX.ErrSetDispEventHandler(UC_XrayCtrl_ErrSetDisp);
            UC_XrayCtrl.MechDataDisp += new clsTActiveX.MechDataDispEventHandler(UC_XrayCtrl_MechDataDisp);
            UC_XrayCtrl.StatusValueDisp += new clsTActiveX.StatusValueDispEventHandler(UC_XrayCtrl_StatusValueDisp);
            UC_XrayCtrl.UserValueDisp += new clsTActiveX.UserValueDispEventHandler(UC_XrayCtrl_UserValueDisp);
            //Rev23.10 連続して発生してしまう 不要では? by長野 2015/09/29
            //UC_XrayCtrl.XrayValueDisp += new clsTActiveX.XrayValueDispEventHandler(UC_XrayCtrl_XrayValueDisp);

            UC_XrayCtrl.XrayStatus3ValueDisp +=new clsTActiveX.XrayStatus3ValueDispEventHandler(UC_XrayCtrl_XrayStatus3ValueDisp);
        }

        //Ｘ線イベントを受け取る（エラー情報）
        private void UC_XrayCtrl_ErrSetDisp(object sender, clsTActiveX.ErrSetEventArgs e)
        {
            XrayErrorData = e.ErrSet;
            try
            {
                //スレッド間で操作できないため、カウントアップ処理を移す
                XrayErrorDataUpdate();
            }
            catch
            {
            }
            finally
            {
            }         
            

        }

         //Ｘ線イベントを受け取る
        private void UC_XrayCtrl_MechDataDisp(object sender, clsTActiveX.MechDataEventArgs e)
        {
            //clsTActiveX.MechData Val1 = e.MechData;
            XrayMechData = e.MechData;
           
            try
            {
                //スレッド間で操作できないため、カウントアップ処理を移す
                XrayMechDataUpdate();
            }
            catch
            {
            }
            finally
            {
            }          
        }

        //Ｘ線イベントを受け取る（ウォームアップ情報）
        private void UC_XrayCtrl_StatusValueDisp(object sender, clsTActiveX.StatusValueEventArgs e)
        {
            XrayStatusValue = e.StatusValue;

            try
            {
                //スレッド間で操作できないため、カウントアップ処理を移す
                XrayStatusValueUpdate();
            }
            catch
            {
            }
            finally
            {
            }        


        }


        //Ｘ線イベントを受け取る（設定管電圧・設定管電流情報）
        private void UC_XrayCtrl_XrayValueDisp(object sender, clsTActiveX.XrayValueEventArgs e)
        {
            XrayValueData = e.XrayValue;
             try
            {
                //スレッド間で操作できないため、カウントアップ処理を移す
                XrayValueUpdate();
            }
            catch
            {
            }
            finally
            {
            }      

          
        }

        //Ｘ線イベントを受け取る（焦点情報）
        private void UC_XrayCtrl_UserValueDisp(object sender, clsTActiveX.UserValueEventArgs e)
        {
            //clsTActiveX.UserValue Val1 = e.UserValue;


            UserValueData = e.UserValue;
            try
            {
                //スレッド間で操作できないため、カウントアップ処理を移す
                UserValueUpdate();
            }
            catch
            {
            }
            finally
            {
            }      

        }

        //Ｘ線イベントを受け取る(アライメントetc)
        private void UC_XrayCtrl_XrayStatus3ValueDisp(object sender, clsTActiveX.UdtXrayStatus3EventArgs e)
        {
            Status3Value = e.udtXrayStatus3ValueDisp;
            try
            {
                //スレッド間で操作できないため、カウントアップ処理を移す
                Status3ValueDispUpdate();
            
            
            }
            catch
            {
            }
            finally
            {
            }


        }

        private void cmdEventSet_Click(object sender, EventArgs e)
        {
            //0:処理無し
            //1:FeinFoucus用ｲﾍﾞﾝﾄ開始
            //2:ｲﾍﾞﾝﾄ停止
            //3:Kvex用ｲﾍﾞﾝﾄ開始
            //4:浜ﾎﾄ（90kV L9421）のｲﾍﾞﾝﾄ開始
            //5:浜ﾎﾄ（130kV L9181）のｲﾍﾞﾝﾄ開始
            //6:浜ﾎﾄ（130kV L9191）のｲﾍﾞﾝﾄ開始
            //7:浜ﾎﾄ（230kV L10801）のｲﾍﾞﾝﾄ開始
            //8:浜ﾎﾄ（90kV L8601）のｲﾍﾞﾝﾄ開始
            //9:浜ﾎﾄ（90kV L9421-02）のｲﾍﾞﾝﾄ開始
            //10:450kV用
            //11:浜ﾎﾄ（150kV L8121-02）のｲﾍﾞﾝﾄ開始
            //12:浜ﾎﾄ（130kV L9181-02）のｲﾍﾞﾝﾄ開始

            //if ((XrayPtr == -1) || (XrayProc != (int)nupEventSet.Value))
            //{
                
            //    int  sts = UC_XrayCtrl.EventValue_Set((int)nupEventSet.Value);
            //    if (sts == 0)
            //    {
            //        if (nupEventSet.Value == 2)
            //        {
            //            XrayProc = -1;
            //            XrayPtr = -1;
            //            return;
            //        }

            //        XrayProc = (int)nupEventSet.Value;
            //        XrayPtr = sts;
            //        XrayVal.m_kVSet = Convert.ToSingle(nudVolt.Value);
            //        XrayVal.m_mASet = Convert.ToSingle(nudAmper.Value);
                
            //        UC_XrayCtrl.XrayValue_Set(XrayVal);
            //    }
            //}
            if ((XrayPtr == -1) || (XrayProc != comboBox1.SelectedIndex))
            {

                int sts = UC_XrayCtrl.EventValue_Set(comboBox1.SelectedIndex);
                if (sts == 0)
                {
                    if (comboBox1.SelectedIndex == 2)
                    {
                        XrayProc = -1;
                        XrayPtr = -1;
                        return;
                    }

                    XrayProc = (int)comboBox1.SelectedIndex;
                    XrayPtr = sts;
                    XrayVal.m_kVSet = Convert.ToSingle(nudVolt.Value);
                    XrayVal.m_mASet = Convert.ToSingle(nudAmper.Value);

                    UC_XrayCtrl.XrayValue_Set(XrayVal);
                }
            }
        }

        private void cmdXon_Click(object sender, EventArgs e)
        {
            UC_XrayCtrl.Xrayonoff_Set(1);
            //UC_XrayCtrl.XrayWarmUp_Set(1);

        }

        private void cmdXoff_Click(object sender, EventArgs e)
        {
            UC_XrayCtrl.Xrayonoff_Set(2);
        
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //制御停止
            UC_XrayCtrl.EventValue_Set(0);
            //コントロール終了
            UC_XrayCtrl.Dispose();

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void XrayErrorDataUpdate()
        {
            if (InvokeRequired)
            {
                Invoke(new XrayErrorDelegate(XrayErrorDataUpdate));
                return;
            }

            try
            {
                txtError.Text =XrayErrorData.m_ErrNO.ToString();
               
            }
            catch
            {
            }

        }



        private void XrayMechDataUpdate()
        {
            if (InvokeRequired)
            {
                Invoke(new XrayMechDataUpdateDelegate(XrayMechDataUpdate));
                return;
            }

            try
            {
                txtVolt.Text = XrayMechData.m_Voltage.ToString();
                txtAmp.Text = XrayMechData.m_Curent.ToString();
                txtOnoff.Text = XrayMechData.m_XrayOnSet.ToString();

                if(XrayMechData.m_XrayOnSet == 1)
                    cmdXon.BackColor = Color.Lime;
                if (XrayMechData.m_XrayOnSet == 0)
                    cmdXon.BackColor = SystemColors.Control;

            }
            catch
            {
            }

        }

        private void XrayStatusValueUpdate()
        {
            if (InvokeRequired)
            {
                Invoke(new XrayStatusValueUpdateDelegate(XrayStatusValueUpdate));
                return;
            }

            try
            {
                txtStatus.Text = XrayStatusValue.m_XrayStatus.ToString();
                txtInterLock.Text = XrayStatusValue.m_InterLock.ToString();

                if (XrayStatusValue.m_WarmUp == 0)
                {
                    txtWarmupEnd.Text = "未";
                }
                else if (XrayStatusValue.m_WarmUp == 1)
                {
                    txtWarmupEnd.Text = "WarmUp中";
                }
                else if (XrayStatusValue.m_WarmUp == 2)
                {
                    txtWarmupEnd.Text = "完了";
                }

            }
            catch
            {
            }

        }

        private void XrayValueUpdate()
        {
            if (InvokeRequired)
            {
                Invoke(new XrayValueUpdateDelegate(XrayValueUpdate));
                return;
            }

            try
            {
                txtSetVolt.Text = XrayValueData.m_kVSet.ToString();
                txtSetAmp.Text = XrayValueData.m_mASet.ToString();

                nudAmper.Value = (decimal)XrayValueData.m_mASet;
                nudVolt.Value = (decimal)XrayValueData.m_kVSet;
             

            }
            catch
            {
            }

            this.Refresh();
        }


        private void UserValueUpdate()
        {
            if (InvokeRequired)
            {
                Invoke(new UserValueUpdateDelegate(UserValueUpdate));
                return;
            }

            try
            {
                txtFocus.Text = UserValueData.m_XrayFocusSize.ToString();

            }
            catch
            {
            }

        
        }


        private void Status3ValueDispUpdate()
        {
            if (InvokeRequired)
            {
                Invoke(new XrayStatus3ValueDispDlegate(Status3ValueDispUpdate));
                return;
            }

            try
            {
                //txtFocus.Text = UserValueData.m_XrayFocusSize.ToString();
                txtSAD.Text = Status3Value.m_XrayStatusSAD.ToString();



            }
            catch
            {
            }

        
        }



        private void nudAmper_ValueChanged(object sender, EventArgs e)
        {
            //Rev23.10 追加 by長野 2015/09/29
            if (this.ActiveControl == nudAmper)
            {
                pnldmy.Focus();
                XrayVal.m_kVSet = Convert.ToSingle(nudVolt.Value);
                XrayVal.m_mASet = Convert.ToSingle(nudAmper.Value);

                UC_XrayCtrl.XrayValue_Set(XrayVal);
            }
        }

        private void nudVolt_ValueChanged(object sender, EventArgs e)
        {
            //Rev23.10 追加 by長野 2015/09/29
            if (this.ActiveControl == nudVolt)
            {
                pnldmy.Focus();
                XrayVal.m_kVSet = Convert.ToSingle(nudVolt.Value);
                XrayVal.m_mASet = Convert.ToSingle(nudAmper.Value);

                UC_XrayCtrl.XrayValue_Set(XrayVal);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UC_XrayCtrl.MessageOk_Set(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //UserValue.m_XrayModeSet = 0;
            //UserValue.m_XrayTimeSet = 0;
            UserValue.m_XrayFocusSet = (int)nudFocus.Value;
            UC_XrayCtrl.UserValue_Set(UserValue);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UC_XrayCtrl.XrayADJ_Set(2);
        }

        private void btnALLADJ_Click(object sender, EventArgs e)
        {
            UC_XrayCtrl.XrayADJ_Set(2);
        }

        private void btnADA_Click(object sender, EventArgs e)
        {
            UC_XrayCtrl.XrayADA_Set(1);
        }

    }
}
