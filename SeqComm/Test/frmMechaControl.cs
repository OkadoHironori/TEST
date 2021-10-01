//using Microsoft.VisualBasic;
//using Microsoft.VisualBasic.Compatibility;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;

namespace Test
{
    public partial class frmMechaControl : Form
	{
        public List<Button> cmdSwitch = null;
        public List<TextBox> txtSpeed = null;
        public List<HScrollBar> hsbSpeed = null;


        private void Init()
        {
            cmdSwitch = new List<Button>();
            Control cFindControl= null;

            string basename = "_cmdSwitch_";
            string cname;
            int i = 0;
            int end = 150;

            for (i = 0; i <= end; i++)
            {
                // 現在のフォーム内から、textBox1 という名前のコントロールを探す
                cFindControl = null;
                cname = basename + i.ToString();
                cFindControl = FindControl(this, cname);
                 // 見つかった場合は Text プロパティを表示する
                if (cFindControl != null) {
                    AddButtonItem((Button)cFindControl);
                }
            }

            txtSpeed = new List<TextBox>();
            basename = "_txtSpeed_";
            i = 0;
            end = 9;

            for (i = 0; i <= end; i++)
            {
                // 現在のフォーム内から、textBox1 という名前のコントロールを探す
                cFindControl = null;
                cname = basename + i.ToString();
                cFindControl = FindControl(this, cname);
                // 見つかった場合は Text プロパティを表示する
                if (cFindControl != null)
                {
                    AddTextBoxItem((TextBox)cFindControl);
                }
            }

            hsbSpeed = new List<HScrollBar>();
            basename = "_hsbSpeed_";
            i = 0;
            end = 9;

            for (i = 0; i <= end; i++)
            {
                // 現在のフォーム内から、textBox1 という名前のコントロールを探す
                cFindControl = null;
                cname = basename + i.ToString();
                cFindControl = FindControl(this, cname);
                // 見つかった場合は Text プロパティを表示する
                if (cFindControl != null)
                {
                    AddHScrollBarItem((HScrollBar)cFindControl);
                }
            }

        }

        private int AddButtonItem(Button cmd)
        {
            int Index = 0;
            Index = cmdSwitch.Count - 1;
            Index = Index + 1;

            cmd.MouseDown += cmdSwitch_MouseDown;
            cmd.MouseUp += cmdSwitch_MouseUp;

            cmdSwitch.Add(cmd);
            //cmdSwitch[Index].Parent = this;

            //戻り値セット
            return Index;
        }

        private int AddTextBoxItem(TextBox txtb)
        {
            int Index = 0;
            Index = cmdSwitch.Count - 1;
            Index = Index + 1;

            txtb.TextChanged += txtSpeed_TextChanged;
 
            txtSpeed.Add(txtb);
            //txtSpeed[Index].Parent = this;

            //戻り値セット
            return Index;
        }

        private int AddHScrollBarItem(HScrollBar hscr)
        {
            int Index = 0;
            Index = cmdSwitch.Count - 1;
            Index = Index + 1;

            //hscr.Scroll += hsbSpeed_Scroll;
            hscr.ValueChanged += hsbSpeed_ValueChanged;
            hscr.MouseCaptureChanged += hsbSpeed_MouseCaptureChanged;

            hsbSpeed.Add(hscr);
            //hsbSpeed[Index].Parent = this;

            //戻り値セット
            return Index;
        }

        /// ---------------------------------------------------------------------------------------
        /// <summary>
        ///     指定したコントロール内に含まれるコントロールを指定した名前で検索します。</summary>
        /// <param name="hParent">
        ///     検索対象となる親コントロール。</param>
        /// <param name="stName">
        ///     検索するコントロールの名前。</param>
        /// <returns>
        ///     取得したコントロールのインスタンス。取得できなかった場合は null。</returns>
        /// ---------------------------------------------------------------------------------------
        public static Control FindControl(Control hParent, string stName)
        {
            // hParent 内のすべてのコントロールを列挙する
            foreach (Control cControl in hParent.Controls)
            {
                // 列挙したコントロールにコントロールが含まれている場合は再帰呼び出しする
                if (cControl.HasChildren)
                {
                    Control cFindControl = FindControl(cControl, stName);

                    // 再帰呼び出し先でコントロールが見つかった場合はそのまま返す
                    if (cFindControl != null)
                    {
                        return cFindControl;
                    }
                }

                // コントロール名が合致した場合はそのコントロールのインスタンスを返す
                if (cControl.Name == stName)
                {
                    return cControl;
                }
            }

            return null;
        }


        private void frmMechaControl_Load(object sender, EventArgs e)
        {
            InitializeComponent();
            Init();
        }


        private void cmdSwitch_MouseDown(object sender, MouseEventArgs e)
        {
            Button bt = (Button)sender;
            int Index = cmdSwitch.IndexOf(bt);
            string device;
            //string data;
            bool bdata;
            int sdata;

#if AllTest
            int Ans = 0;
#endif

            //ﾘﾓｰﾄ操作の許可
            if (! Module1.MySeq.PcInhibit)
            {
                switch (Index)
                {
                    case 0:
                        //ｴﾗｰ表示、ﾌﾞｻﾞｰOFFのOFF
                        //Module1.PlcBitWrite(ref "ErrOff", ref true);
                        device = "ErrOff";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 1:
                        //ﾃｰﾌﾞﾙX左移動ON
                        cmdSwitch[1].BackColor = System.Drawing.Color.Red;
                        sdata = 0;
                        SpeedSet(ref sdata);
                        
                        //運転速度設定
                        //Module1.PlcBitWrite(ref "XLeft", ref true);
                        device = "XLeft";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;

                    case 2:
                        //ﾃｰﾌﾞﾙX右移動ON
                        cmdSwitch[2].BackColor = System.Drawing.Color.Red;
                        //SpeedSet(ref 0);
                        sdata = 0;
                        SpeedSet(ref sdata);
                        
                        //運転速度設定
                        //Module1.PlcBitWrite(ref "XRight", ref true);
                        device = "XRight";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);

                        break;
                    case 3:
                        //ﾃｰﾌﾞﾙY前進ON
                        cmdSwitch[3].BackColor = System.Drawing.Color.Red;
                        //SpeedSet(ref 1);
                        sdata = 1;
                        SpeedSet(ref sdata);

                        //運転速度設定
                        //Module1.PlcBitWrite(ref "YForward", ref true);
                        device = "YForward";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 4:
                        //ﾃｰﾌﾞﾙY後退ON
                        cmdSwitch[4].BackColor = System.Drawing.Color.Red;
                        //SpeedSet(ref 1);
                        sdata = 1;
                        SpeedSet(ref sdata);
                    
                        //運転速度設定
                        //Module1.PlcBitWrite(ref "YBackward", ref true);
                        device = "YBackward";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        
                        break;
                    case 5:
                        //I.I.ｲﾝﾃﾞｯｸｽ停止
                        //Module1.PlcBitWrite(ref "IIIndexStop", ref true);
                        device = "IIIndexStop";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 6:
                        //I.I.ｲﾝﾃﾞｯｸｽ動作
                        //Module1.PlcBitWrite(ref "IIIndex", ref true);
                        device = "IIIndex";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 7:
                        //I.I.前進ON
                        cmdSwitch[7].BackColor = System.Drawing.Color.Red;
                        //SpeedSet(ref 4);
                        sdata = 4;
                        SpeedSet(ref sdata);

                        //運転速度設定
                        //Module1.PlcBitWrite(ref "IIForward", ref true);
                        device = "IIForward";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 8:
                        //I.I.後退ON
                        cmdSwitch[8].BackColor = System.Drawing.Color.Red;
                        //SpeedSet(ref 4);
                        sdata = 4;
                        SpeedSet(ref sdata);

                        //運転速度設定
                        //Module1.PlcBitWrite(ref "IIBackward", ref true);
                        device = "IIBackward";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        
                        break;

                    case 9:
                        //傾斜CCW ON
                        //Module1.PlcBitWrite(ref "TiltCcw", ref true);
                        device = "TiltCcw";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 10:
                        //傾斜CW ON
                        //Module1.PlcBitWrite(ref "TiltCw", ref true);
                        device = "TiltCw";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 11:
                        //傾斜原点復帰ON
                        //Module1.PlcBitWrite(ref "TiltOrigin", ref true);
                        device = "TiltOrigin";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 12:
                        //ﾌｨﾙﾀ無しON
                        //Module1.PlcBitWrite(ref "Filter0", ref true);
                        device = "Filter0";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 13:
                        //ﾌｨﾙﾀ1 ON
                        //Module1.PlcBitWrite(ref "Filter1", ref true);
                        device = "Filter1";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 14:
                        //ﾌｨﾙﾀ2 ON
                        //Module1.PlcBitWrite(ref "Filter2", ref true);
                        device = "Filter2";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;

                    case 15:
                        //ﾌｨﾙﾀ3 ON
                        //Module1.PlcBitWrite(ref "Filter3", ref true);
                        device = "Filter3";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 16:
                        //ﾌｨﾙﾀ4 ON
                        //Module1.PlcBitWrite(ref "Filter4", ref true);
                        device = "Filter4";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 17:
                        //ﾌｨﾙﾀ5 ON
                        //Module1.PlcBitWrite(ref "Filter5", ref true);
                        device = "Filter5";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 18:
                        //ｺﾘﾒｰﾀ上開ON
                        //Module1.PlcBitWrite(ref "ColliUOpen", ref true);
                        device = "ColliUOpen";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 19:
                        //ｺﾘﾒｰﾀ上閉ON
                        //Module1.PlcBitWrite(ref "ColliUClose", ref true);
                        device = "ColliUClose";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;

                    case 20:
                        //ｺﾘﾒｰﾀ下開ON
                        //Module1.PlcBitWrite(ref "ColliDOpen", ref true);
                        device = "ColliDOpen";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 21:
                        //ｺﾘﾒｰﾀ下閉ON
                        //Module1.PlcBitWrite(ref "ColliDClose", ref true);
                        device = "ColliDClose";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 22:
                        //ｺﾘﾒｰﾀ左開ON
                        //Module1.PlcBitWrite(ref "ColliLOpen", ref true);
                        device = "ColliLOpen";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 23:
                        //ｺﾘﾒｰﾀ左閉ON
                        //Module1.PlcBitWrite(ref "ColliLClose", ref true);
                        device = "ColliLClose";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 24:
                        //ｺﾘﾒｰﾀ右開ON
                        //Module1.PlcBitWrite(ref "ColliROpen", ref true);
                        device = "ColliROpen";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;

                    case 25:
                        //ｺﾘﾒｰﾀ右閉ON
                        //Module1.PlcBitWrite(ref "ColliRClose", ref true);
                        device = "ColliRClose";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 26:
                        //I.I.視野9" ON
                        //Module1.PlcBitWrite(ref "II9", ref true);
                        device = "II9";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 27:
                        //I.I.視野6" ON
                        //Module1.PlcBitWrite(ref "II6", ref true);
                        device = "II6";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 28:
                        //I.I.視野4.5" ON
                        //Module1.PlcBitWrite(ref "II4", ref true);
                        device = "II4";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;

#if AllTest

                    case 29:
                        //微調X軸左
                        if (Module1.XstgFlg)    //追加2015/03/14hata
                            Ans = Module1.XStgManual(Module1.hDevID, 1, Convert.ToSingle(txtSpeed[5].Text));
                        break;
                    case 30:
                        //微調X軸原点復帰
                        if (Module1.XstgFlg)    //追加2015/03/14hata
                        {
                            _cmdSwitch_30.Enabled = false;  //追加2015/03/14hata
                            Ans = Module1.XStgOrigin(Module1.hDevID, 0);
                            _cmdSwitch_30.Enabled = true;   //追加2015/03/14hata
                        }

                        break;
                    case 31:
                        //回転逆転
                        Ans = Module1.RotateManual(Module1.hDevID, 1, Convert.ToSingle(txtSpeed[2].Text), 0);
                        break;
                    case 32:
                        //回転正転
                        Ans = Module1.RotateManual(Module1.hDevID, 0, Convert.ToSingle(txtSpeed[2].Text), 0);
                        break;

                    case 33:
                        //回転原点復帰
                        Ans = Module1.RotateOrigin(Module1.hDevID, 0);
                        if (Ans == 0)
                        {
                            //回転原点復帰完了
                            //Module1.PlcBitWrite(ref "RotOriginOK", ref true);
                            device = "RotOriginOK";
                            bdata = true;
                            Module1.PlcBitWrite(ref device, ref bdata);
                        }
                        break;
                    case 34:
                        //昇降上昇
                        Ans = Module1.UdManual(Module1.hDevID, 1, Convert.ToSingle(txtSpeed[3].Text));
                        break;
                    case 35:
                        //昇降下降
                        Ans = Module1.UdManual(Module1.hDevID, 0, Convert.ToSingle(txtSpeed[3].Text));
                        break;
                    case 36:
                        //昇降原点復帰
                        Ans = Module1.UdOrigin(Module1.hDevID, 0);
                        break;

                    //追加2014/10/06hata_v1951反映
                    case 150:
                        //昇降位置決め    
                        Ans = Module1.UdIndex(Module1.hDevID, 0, 500, 0, 0);
                        break;                                     

#endif
                   
                    case 37:
                        //I.I.電源ON
                        device = "IIPowerOn";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 38:
                        //I.I.電源OFF
                        device = "IIPowerOff";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 39:
                        //ﾀｯﾁﾊﾟﾈﾙ操作禁止
                        //Module1.PlcBitWrite(ref "PanelInhibit", ref true);
                        device = "PanelInhibit";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 40:
                        //ﾀｯﾁﾊﾟﾈﾙ操作許可
                        //Module1.PlcBitWrite(ref "PanelInhibit", ref false);
                        device = "PanelInhibit";
                        bdata = false;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;

                    case 41:
                        //X線外部制御禁止
                        //Module1.PlcBitWrite(ref "XrayInhibit", ref false);
                        device = "XrayInhibit";
                        bdata = false;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 42:
                        //X線外部制御許可
                        //Module1.PlcBitWrite(ref "XrayInhibit", ref true);
                        device = "XrayInhibit";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;

                    case 43:
                        //ﾃｰﾌﾞﾙYｲﾝﾃﾞｯｸｽ動作
                        //Module1.PlcBitWrite(ref "YIndex", ref true);
                        device = "YIndex";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);

                        break;
                    case 44:
                        //ﾃｰﾌﾞﾙYｲﾝﾃﾞｯｸｽ停止
                        //Module1.PlcBitWrite(ref "YIndexStop", ref true);
                        device = "YIndexStop";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 45:
                        //ﾃｰﾌﾞﾙXｲﾝﾃﾞｯｸｽ動作
                        //Module1.PlcBitWrite(ref "XIndex", ref true);
                        device = "XIndex";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;

                    case 46:
                        //ﾃｰﾌﾞﾙXｲﾝﾃﾞｯｸｽ停止
                        //Module1.PlcBitWrite(ref "XIndexStop", ref true);
                        device = "XIndexStop";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;

                    case 47:
                        //回転中心校正ﾃｰﾌﾞﾙX移動有りﾘｾｯﾄ
                        //Module1.PlcBitWrite(ref "RotXChangeReset", ref true);
                        device = "RotXChangeReset";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 48:
                        //寸法校正ﾃｰﾌﾞﾙX移動有りﾘｾｯﾄ
                        //Module1.PlcBitWrite(ref "DisXChangeReset", ref true);
                        device = "DisXChangeReset";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 49:
                        //回転中心校正ﾃｰﾌﾞﾙY移動有りﾘｾｯﾄ
                        //Module1.PlcBitWrite(ref "RotYChangeReset", ref true);
                        device = "RotYChangeReset";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 50:
                        //寸法校正ﾃｰﾌﾞﾙY移動有りﾘｾｯﾄ
                        //Module1.PlcBitWrite(ref "DisYChangeReset", ref true);
                        device = "DisYChangeReset";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;

                    case 51:
                        //幾何歪校正ﾃｰﾌﾞﾙI.I.移動有りﾘｾｯﾄ
                        //Module1.PlcBitWrite(ref "VerIIChangeReset", ref true);
                        device = "VerIIChangeReset";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 52:
                        //回転中心校正ﾃｰﾌﾞﾙI.I.移動有りﾘｾｯﾄ
                        //Module1.PlcBitWrite(ref "RotIIChangeReset", ref true);
                        device = "RotIIChangeReset";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 53:
                        //ｹﾞｲﾝ校正ﾃｰﾌﾞﾙI.I.移動有りﾘｾｯﾄ
                        //Module1.PlcBitWrite(ref "GainIIChangeReset", ref true);
                        device = "GainIIChangeReset";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 54:
                        //寸法校正ﾃｰﾌﾞﾙI.I.移動有りﾘｾｯﾄ
                        //Module1.PlcBitWrite(ref "DisIIChangeReset", ref true);
                        device = "DisIIChangeReset";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;

                    case 55:
                        //ﾃｰﾌﾞﾙX原点復帰ON
                        //Module1.PlcBitWrite(ref "XOrigin", ref true);
                        device = "XOrigin";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;

                    //ここからコメント解除_2014/10/07----->
                    case 56:
                        //ｽﾗｲｽﾗｲﾄON
                        //Module1.PlcBitWrite(ref "SLightOn", ref true);
                        device = "SLightOn";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 57:
                        //ｽﾗｲｽﾗｲﾄOFF
                        //Module1.PlcBitWrite(ref "SLightOff", ref true);
                        device = "SLightOff";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;

#if AllTest
                    case 58:
                        //微調X軸右
                        if (Module1.XstgFlg)    //追加2015/03/14hata
                            Ans = Module1.XStgManual(Module1.hDevID, 0, Convert.ToSingle(txtSpeed[5].Text));
                        break;
                    case 59:
                        //微調Y軸原点復帰
                        if (Module1.YstgFlg)    //追加2015/03/14hata
                        {
                            _cmdSwitch_59.Enabled = false;  //追加2015/03/14hata
                            Ans = Module1.YStgOrigin(Module1.hDevID, 0);
                            _cmdSwitch_59.Enabled = true;  //追加2015/03/14hata
                        }
                        break;
                    case 60:
                        //微調Y軸前進
                        if (Module1.YstgFlg)    //追加2015/03/14hata
                            Ans = Module1.YStgManual(Module1.hDevID, 1, Convert.ToSingle(txtSpeed[6].Text));
                        break;
                    case 61:
                        //微調Y軸後退
                        if (Module1.YstgFlg)    //追加2015/03/14hata
                            Ans = Module1.YStgManual(Module1.hDevID, 0, Convert.ToSingle(txtSpeed[6].Text));
                        break;
#endif

                    case 62:
                        //ﾄﾘｶﾞ出力開始
                        //Module1.PlcBitWrite(ref "TrgReq", ref true);
                        device = "TrgReq";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 64:
                        //検出器I.I.
                        //Module1.PlcBitWrite(ref "II", ref true);
                        device = "II";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 65:
                        //検出器FPD
                        //Module1.PlcBitWrite(ref "FPD", ref true);
                        device = "FPD";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 66:
                        //X軸機構ﾃｰﾌﾞﾙ
                        //Module1.PlcBitWrite(ref "XTableMove", ref true);
                        device = "XTableMove";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;

                    case 67:
                        //X軸機構X線管
                        //Module1.PlcBitWrite(ref "TubeMove", ref true);
                        device = "TubeMove";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 68:
                        //微調X軸無効
                        //Module1.PlcBitWrite(ref "FXTableOff", ref true);
                        device = "FXTableOff";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 69:
                        //微調X軸有効
                        //Module1.PlcBitWrite(ref "FXTableOn", ref true);
                        device = "FXTableOn";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 70:
                        //微調Y軸有効
                        //Module1.PlcBitWrite(ref "FYTableOn", ref true);
                        device = "FYTableOn";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;

                    case 71:
                        //微調Y軸無効
                        //Module1.PlcBitWrite(ref "FYTableOff", ref true);
                        device = "FYTableOff";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    //ここまでコメント解除_2014/10/07----->
                   
                    case 72:
                        //ﾃｰﾌﾞﾙY原点復帰ON
                        //Module1.PlcBitWrite(ref "YOrigin", ref true);
                        device = "YOrigin";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;


                    //ここからコメント解除_2014/10/07----->
                    case 73:
                        //X線管X原点復帰ON
                        //Module1.PlcBitWrite(ref "XrayXOrg", ref true);
                        device = "XrayXOrg";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 74:
                        //X線管X左移動ON
                        cmdSwitch[74].BackColor = System.Drawing.Color.Red;
                        //SpeedSet(ref 7);
                        sdata = 7;
                        SpeedSet(ref sdata);

                        //運転速度設定
                        //Module1.PlcBitWrite(ref "XrayXLeft", ref true);
                        device = "XrayXLeft";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 75:
                        //X線管X右移動ON
                        cmdSwitch[75].BackColor = System.Drawing.Color.Red;
                        //SpeedSet(ref 7);
                        sdata = 7;
                        SpeedSet(ref sdata);

                        //運転速度設定
                        //Module1.PlcBitWrite(ref "XrayXRight", ref true);
                        device = "XrayXRight";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 76:
                        //X線管Xｲﾝﾃﾞｯｸｽ動作
                        //Module1.PlcBitWrite(ref "XrayXIndex", ref true);
                        device = "XrayXIndex";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 77:
                        //X線管X停止
                        //Module1.PlcBitWrite(ref "XrayXStop", ref true);
                        device = "XrayXStop";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 78:
                        //X線管Y原点復帰ON
                        //Module1.PlcBitWrite(ref "XrayYOrg", ref true);
                        device = "XrayYOrg";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 79:
                        //X線管Y前進ON
                        cmdSwitch[79].BackColor = System.Drawing.Color.Red;
                        //SpeedSet(ref 8);
                        sdata = 8;
                        SpeedSet(ref sdata);

                        //運転速度設定
                        //Module1.PlcBitWrite(ref "XrayYForward", ref true);
                        device = "XrayYForward";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 80:
                        //X線管Y後退ON
                        cmdSwitch[80].BackColor = System.Drawing.Color.Red;
                        //SpeedSet(ref 8);
                        sdata = 8;
                        SpeedSet(ref sdata);

                        //運転速度設定
                        //Module1.PlcBitWrite(ref "XrayYBackward", ref true);
                        device = "XrayYBackward";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 81:
                        //X線管Yｲﾝﾃﾞｯｸｽ動作
                        //Module1.PlcBitWrite(ref "XrayYIndex", ref true);
                        device = "XrayYIndex";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 82:
                        //X線管Y停止
                        //Module1.PlcBitWrite(ref "XrayYStop", ref true);
                        device = "XrayYStop";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;

#if AllTest                   
                    case 83:
                        //X線管回転停止
                        //PlcBitWrite "XrayRotStop", True
                        Module1.PioOutBit("XRAYROTSTOP", 1);
                        break;
                    case 84:
                        //X線管回転ｲﾝﾃﾞｯｸｽ動作
                        //PlcBitWrite "XrayRotIndex", True
                        Module1.PioOutBit("XRAYROTINDEX", 1);
                        break;
                    case 85:
                        //X線管正転ON
                        cmdSwitch[85].BackColor = System.Drawing.Color.Red;
                        //SpeedSet(ref 9);
                        sdata = 9;
                        SpeedSet(ref sdata);

                        //運転速度設定
                        //PlcBitWrite "XrayRotCW", True
                        Module1.PioOutBit("XRAYROTCW", 1);
                        break;
                    case 86:
                        //X線管逆転ON
                        cmdSwitch[86].BackColor = System.Drawing.Color.Red;
                        //SpeedSet(ref 9);
                        sdata = 9;
                        SpeedSet(ref sdata);

                        //運転速度設定
                        //PlcBitWrite "XrayRotCCW", True
                        Module1.PioOutBit("XRAYROTCCW", 1);
                        break;
                    case 87:
                        //X線管回転原点復帰ON
                        //PlcBitWrite "XrayRotOrg", True
                        Module1.PioOutBit("XRAYROTORG", 1);
                        break;
#endif

                    case 88:
                        //回転中心校正X線管X移動有りﾘｾｯﾄ
                        //Module1.PlcBitWrite(ref "RotXrayXChReset", ref true);
                        device = "RotXrayXChReset";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 89:
                        //寸法校正X線管X移動有りﾘｾｯﾄ
                        //Module1.PlcBitWrite(ref "DisXrayXChReset", ref true);
                        device = "DisXrayXChReset";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 90:
                        //回転中心校正X線管Y移動有りﾘｾｯﾄ
                        //Module1.PlcBitWrite(ref "RotXrayYChReset", ref true);
                        device = "RotXrayYChReset";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 91:
                        //寸法校正X線管Y移動有りﾘｾｯﾄ
                        //Module1.PlcBitWrite(ref "DisXrayYChReset", ref true);
                        device = "DisXrayYChReset";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 92:
                        //ﾃｰﾌﾞﾙｲﾝﾀｰﾛｯｸ有効
                        //Module1.PlcBitWrite(ref "TableMoveRestrict", ref true);
                        device = "TableMoveRestrict";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 93:
                        //ﾃｰﾌﾞﾙｲﾝﾀｰﾛｯｸ解除
                        //Module1.PlcBitWrite(ref "TableMovePermit", ref true);
                        device = "TableMovePermit";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;

#if AllTest
                    case 94:
                        //ﾌｧﾝﾄﾑOFF
                        Ans = Module1.PhmOff(0);
                        break;
                    case 95:
                        //ﾌｧﾝﾄﾑON
                        Ans = Module1.PhmOn(0);
                        break;
#endif

                    case 96:
                        //X線EXMﾘﾓｰﾄ要求
                        //Module1.PlcBitWrite(ref "EXMRemote", ref true);
                        device = "EXMRemote";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 97:
                        //X線EXMﾛｰｶﾙ要求
                        //Module1.PlcBitWrite(ref "EXMLocal", ref true);
                        device = "EXMLocal";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 98:
                        //X線ON要求
                        //Module1.PlcBitWrite(ref "EXMOn", ref true);
                        device = "EXMOn";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 99:
                        //X線OFF要求
                        //Module1.PlcBitWrite(ref "EXMOff", ref true);
                        device = "EXMOff";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 100:
                        //X線EXM異常ﾘｾｯﾄ要求
                        //Module1.PlcBitWrite(ref "EXMReset", ref true);
                        device = "EXMReset";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 101:
                        //X線EXMｳｫｰﾑｱｯﾌﾟ短期要求
                        //Module1.PlcBitWrite(ref "EXMWUShort", ref true);
                        device = "EXMWUShort";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 102:
                        //X線EXMｳｫｰﾑｱｯﾌﾟ長期要求
                        //Module1.PlcBitWrite(ref "EXMWULong", ref true);
                        device = "EXMWULong";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 103:
                        //X線EXMｳｫｰﾑｱｯﾌﾟ解除要求
                        //Module1.PlcBitWrite(ref "EXMWUNone", ref true);
                        device = "EXMWUNone";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 104:
                        //ｽｷｬﾝ位置校正ﾃｰﾌﾞﾙI.I.移動有りﾘｾｯﾄ
                        //Module1.PlcBitWrite(ref "SPIIChangeReset", ref true);
                        device = "SPIIChangeReset";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 105:
                        //ｽｷｬﾝ位置校正ﾃｰﾌﾞﾙI.I.移動有ｾｯﾄ
                        //Module1.PlcBitWrite(ref "SPIIChangeSet", ref true);
                        device = "SPIIChangeSet";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 106:
                        //扉電磁ﾛｯｸOFF
                        //Module1.PlcBitWrite(ref "DoorLockOff", ref true);
                        device = "DoorLockOff";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 107:
                        //扉電磁ﾛｯｸON
                        //Module1.PlcBitWrite(ref "DoorLockOn", ref true);
                        device = "DoorLockOn";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 108:
                        //ﾃｰﾌﾞﾙXY直線補間動作
                        //Module1.PlcBitWrite(ref "XYIndex", ref true);
                        device = "XYIndex";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 110:
                        //寸法校正ﾃｰﾌﾞﾙI.I.移動有ｾｯﾄ
                        //Module1.PlcBitWrite(ref "DisIIChangeSet", ref true);
                        device = "DisIIChangeSet";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 111:
                        //ｹﾞｲﾝ校正ﾃｰﾌﾞﾙI.I.移動有ｾｯﾄ
                        //Module1.PlcBitWrite(ref "GainIIChangeSet", ref true);
                        device = "GainIIChangeSet";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 112:
                        //回転中心校正ﾃｰﾌﾞﾙI.I.移動有ｾｯﾄ
                        //Module1.PlcBitWrite(ref "RotIIChangeSet", ref true);
                        device = "RotIIChangeSet";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 113:
                        //幾何歪校正ﾃｰﾌﾞﾙI.I.移動有ｾｯﾄ
                        //Module1.PlcBitWrite(ref "VerIIChangeSet", ref true);
                        device = "VerIIChangeSet";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 114:
                        //回転中心校正ﾃｰﾌﾞﾙX移動有ｾｯﾄ
                        //Module1.PlcBitWrite(ref "RotXChangeSet", ref true);
                        device = "RotXChangeSet";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 115:
                        //寸法校正ﾃｰﾌﾞﾙX移動有ｾｯﾄ
                        //Module1.PlcBitWrite(ref "DisXChangeSet", ref true);
                        device = "DisXChangeSet";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 116:
                        //回転中心校正ﾃｰﾌﾞﾙY移動有ｾｯﾄ
                        //Module1.PlcBitWrite(ref "RotYChangeSet", ref true);
                        device = "RotYChangeSet";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 117:
                        //寸法校正ﾃｰﾌﾞﾙY移動有ｾｯﾄ
                        //Module1.PlcBitWrite(ref "DisYChangeSet", ref true);
                        device = "DisYChangeSet";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 109:
                        //I.I.絞り左開ON
                        //Module1.PlcBitWrite(ref "IrisLOpen", ref true);
                        device = "IrisLOpen";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 118:
                        //I.I.絞り左閉ON
                        //Module1.PlcBitWrite(ref "IrisLClose", ref true);
                        device = "IrisLClose";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 119:
                        //I.I.絞り右開ON
                        //Module1.PlcBitWrite(ref "IrisROpen", ref true);
                        device = "IrisROpen";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 120:
                        //I.I.絞り右閉ON
                        //Module1.PlcBitWrite(ref "IrisRClose", ref true);
                        device = "IrisRClose";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 121:
                        //I.I.絞り上開ON
                        //Module1.PlcBitWrite(ref "IrisUOpen", ref true);
                        device = "IrisUOpen";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 122:
                        //I.I.絞り上閉ON
                        //Module1.PlcBitWrite(ref "IrisUClose", ref true);
                        device = "IrisUClose";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 123:
                        //I.I.絞り下開ON
                        //Module1.PlcBitWrite(ref "IrisDOpen", ref true);
                        device = "IrisDOpen";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 124:
                        //I.I.絞り下閉ON
                        //Module1.PlcBitWrite(ref "IrisDClose", ref true);
                        device = "IrisDClose";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 125:
                        //高速I.I.視野4.5" ON
                        //Module1.PlcBitWrite(ref "TVII4", ref true);
                        device = "TVII4";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 126:
                        //高速I.I.視野6" ON
                        //Module1.PlcBitWrite(ref "TVII6", ref true);
                        device = "TVII6";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 127:
                        //高速I.I.視野9" ON
                        //Module1.PlcBitWrite(ref "TVII9", ref true);
                        device = "TVII9";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 128:
                        //高速I.I.電源ON
                        //Module1.PlcBitWrite(ref "TVIIPowerOn", ref true);
                        device = "TVIIPowerOn";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 129:
                        //高速I.I.電源OFF
                        //Module1.PlcBitWrite(ref "TVIIPowerOff", ref true);
                        device = "TVIIPowerOff";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 130:
                        //ｶﾒﾗ電源ON
                        //Module1.PlcBitWrite(ref "CameraPowerOn", ref true);
                        device = "CameraPowerOn";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 131:
                        //ｶﾒﾗ電源OFF
                        //Module1.PlcBitWrite(ref "CameraPowerOff", ref true);
                        device = "CameraPowerOff";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 132:
                        //ｳｫｰﾑｱｯﾌﾟON中
                        //Module1.PlcBitWrite(ref "stsWarmUpOn", ref true);
                        device = "stsWarmUpOn";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 133:
                        //ｳｫｰﾑｱｯﾌﾟOFF中
                        //Module1.PlcBitWrite(ref "stsWarmUpOn", ref false);
                        device = "stsWarmUpOn";
                        bdata = false;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 134:
                        //I.I.切替停止
                        //Module1.PlcBitWrite(ref "IIChangeStop", ref true);
                        device = "IIChangeStop";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 135:
                        //CT用I.I.切替
                        //Module1.PlcBitWrite(ref "CTIISet", ref true);
                        device = "CTIISet";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;

                    case 136:
                        //TV用I.I.切替
                        //Module1.PlcBitWrite(ref "TVIISet", ref true);
                        device = "TVIISet";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 137:
                        //I.I.切替ﾘｾｯﾄ
                        //Module1.PlcBitWrite(ref "IIChangeReset", ref true);
                        device = "IIChangeReset";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 138:
                        //CT切替許可
                        //Module1.PlcBitWrite(ref "CTChangeNO", ref false);
                        device = "CTChangeNO";
                        bdata = false;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 139:
                        //高速切替許可
                        //Module1.PlcBitWrite(ref "TVChangeNO", ref false);
                        device = "TVChangeNO";
                        bdata = false;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 140:
                        //CT切替禁止
                        //Module1.PlcBitWrite(ref "CTChangeNO", ref true);
                        device = "CTChangeNO";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 141:
                        //高速切替禁止
                        //Module1.PlcBitWrite(ref "TVChangeNO", ref true);
                        device = "TVChangeNO";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    //ここまでコメント解除_2014/10/07----->

                    case 142:
                        //I.IY原点復帰ON
                        //Module1.PlcBitWrite(ref "IIOrigin", ref true);
                        device = "IIOrigin";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 143:
                        //ﾒｶﾘｾｯﾄ停止要求
                        //Module1.PlcBitWrite(ref "MechaResetStop", ref true);
                        device = "MechaResetStop";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 144:
                        //ﾒｶﾘｾｯﾄ要求
                        //Module1.PlcBitWrite(ref "MechaReset", ref true);
                        device = "MechaReset";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 145:
                        //動作制限自動復帰無効
                        //Module1.PlcBitWrite(ref "AutoRestrictOff", ref true);
                        device = "IIOrigin";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 146:
                        //動作制限自動復帰有効
                        //Module1.PlcBitWrite(ref "AutoRestrictOn", ref true);
                        device = "AutoRestrictOn";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 147:
                        //ﾃｰﾌﾞﾙYｲﾝﾃﾞｯｸｽ減速無効
                        //Module1.PlcBitWrite(ref "YIndexSlowOff", ref true);
                        device = "YIndexSlowOff";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                    case 148:
                        //ﾃｰﾌﾞﾙYｲﾝﾃﾞｯｸｽ減速有効
                        //Module1.PlcBitWrite(ref "YIndexSlowOn", ref true);
                        device = "YIndexSlowOn";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                
                    //追加2014/10/06hata_v1951反映
                    case 149:      //'ｼｬｯﾀ
                        //PlcBitWrite "Shutter", True
                        device = "Shutter";
                        bdata = true;
                        Module1.PlcBitWrite(ref device, ref bdata);
                        break;
                
                }
            }

        }

        private void cmdSwitch_MouseUp(object sender, MouseEventArgs e)
        {

#if AllTest
            int Ans = 0;
#endif

            string device;
            //string data;
            bool bdata;

            Button bt = (Button)sender;
            int Index = cmdSwitch.IndexOf(bt);

            switch (Index)
            {
                case 0:
                    //ｴﾗｰ表示、ﾌﾞｻﾞｰOFFのON
                    //Module1.PlcBitWrite(ref "ErrOff", ref false);
                    device = "ErrOff";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    
                    break;
                case 1:
                    //ﾃｰﾌﾞﾙX右移動OFF
                    cmdSwitch[1].BackColor = System.Drawing.Color.Lime;
                    //Module1.PlcBitWrite(ref "XLeft", ref false);
                    device = "XLeft";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    
                    break;
                case 2:
                    //ﾃｰﾌﾞﾙX左移動OFF
                    cmdSwitch[2].BackColor = System.Drawing.Color.Lime;
                    //Module1.PlcBitWrite(ref "XRight", ref false);
                    device = "XRight";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 3:
                    //ﾃｰﾌﾞﾙY前進OFF
                    cmdSwitch[3].BackColor = System.Drawing.Color.Lime;
                    //Module1.PlcBitWrite(ref "YForward", ref false);
                    device = "YForward";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 4:
                    //ﾃｰﾌﾞﾙY後退OFF
                    cmdSwitch[4].BackColor = System.Drawing.Color.Lime;
                    //Module1.PlcBitWrite(ref "YBackward", ref false);
                    device = "YBackward";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 5:
                    //I.I.ｲﾝﾃﾞｯｸｽ停止
                    //Module1.PlcBitWrite(ref "IIIndexStop", ref false);
                    device = "IIIndexStop";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 6:
                    //I.I.ｲﾝﾃﾞｯｸｽ動作
                    //Module1.PlcBitWrite(ref "IIIndex", ref false);
                    device = "IIIndex";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 7:
                    //I.I.前進OFF
                    cmdSwitch[7].BackColor = System.Drawing.Color.Lime;
                    //Module1.PlcBitWrite(ref "IIForward", ref false);
                    device = "IIForward";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 8:
                    //I.I.後退OFF
                    cmdSwitch[8].BackColor = System.Drawing.Color.Lime;
                    //Module1.PlcBitWrite(ref "IIBackward", ref false);
                    device = "IIBackward";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;


                //ここからコメント解除_2014/10/07
                case 9:
                    //傾斜CCW OFF
                    //Module1.PlcBitWrite(ref "TiltCcw", ref false);
                    device = "TiltCcw";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 10:
                    //傾斜CW OFF
                    //Module1.PlcBitWrite(ref "TiltCw", ref false);
                    device = "TiltCw";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 11:
                    //傾斜原点復帰OFF
                    //Module1.PlcBitWrite(ref "TiltOrigin", ref false);
                    device = "TiltOrigin";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 12:
                    //ﾌｨﾙﾀ無しOFF
                    //Module1.PlcBitWrite(ref "Filter0", ref false);
                    device = "Filter0";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 13:
                    //ﾌｨﾙﾀ1 OFF
                    //Module1.PlcBitWrite(ref "Filter1", ref false);
                    device = "Filter1";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 14:
                    //ﾌｨﾙﾀ2 OFF
                    //Module1.PlcBitWrite(ref "Filter2", ref false);
                    device = "Filter2";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 15:
                    //ﾌｨﾙﾀ3 OFF
                    //Module1.PlcBitWrite(ref "Filter3", ref false);
                    device = "Filter3";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 16:
                    //ﾌｨﾙﾀ4 OFF
                    //Module1.PlcBitWrite(ref "Filter4", ref false);
                    device = "Filter4";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 17:
                    //ﾌｨﾙﾀ5 OFF
                    //Module1.PlcBitWrite(ref "Filter5", ref false);
                    device = "Filter5";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 18:
                    //ｺﾘﾒｰﾀ上開OFF
                    //Module1.PlcBitWrite(ref "ColliUOpen", ref false);
                    device = "ColliUOpen";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 19:
                    //ｺﾘﾒｰﾀ上閉OFF
                    //Module1.PlcBitWrite(ref "ColliUClose", ref false);
                    device = "ColliUClose";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 20:
                    //ｺﾘﾒｰﾀ下開OFF
                    //Module1.PlcBitWrite(ref "ColliDOpen", ref false);
                    device = "ColliDOpen";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 21:
                    //ｺﾘﾒｰﾀ下閉OFF
                    //Module1.PlcBitWrite(ref "ColliDClose", ref false);
                    device = "ColliDClose";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 22:
                    //ｺﾘﾒｰﾀ左開OFF
                    //Module1.PlcBitWrite(ref "ColliLOpen", ref false);
                    device = "ColliLOpen";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 23:
                    //ｺﾘﾒｰﾀ左閉OFF
                    //Module1.PlcBitWrite(ref "ColliLClose", ref false);
                    device = "ColliLClose";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 24:
                    //ｺﾘﾒｰﾀ右開OFF
                    //Module1.PlcBitWrite(ref "ColliROpen", ref false);
                    device = "ColliROpen";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 25:
                    //ｺﾘﾒｰﾀ右閉OFF
                    //Module1.PlcBitWrite(ref "ColliRClose", ref false);
                    device = "ColliRClose";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 26:
                    //I.I.視野9" OFF
                    //Module1.PlcBitWrite(ref "II9", ref false);
                    device = "II9";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 27:
                    //I.I.視野6" OFF
                    //Module1.PlcBitWrite(ref "II6", ref false);
                    device = "II6";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 28:
                    //I.I.視野4.5" OFF
                    //Module1.PlcBitWrite(ref "II4", ref false);
                    device = "II4";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;                

#if AllTest
                case 29:
                case 58:
                    //微調X停止
                    Ans = Module1.XStgFastStop(Module1.hDevID, 0);
                    break;
                case 31:
                case 32:
                    //回転停止
                    Ans = Module1.RotateFastStop(Module1.hDevID, 0);
                    break;
                case 34:
                case 35:
                    //昇降停止
                    Ans = Module1.UdFastStop(Module1.hDevID, 0);
                    break;
#endif

                case 37:
                    //I.I.電源ON
                    //Module1.PlcBitWrite(ref "IIPowerOn", ref false);
                    device = "IIPowerOn";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 38:
                    //I.I.電源OFF
                    //Module1.PlcBitWrite(ref "IIPowerOff", ref false);
                    device = "IIPowerOff";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                //ここまでコメント解除_2014/10/07


                case 43:
                    //ﾃｰﾌﾞﾙYｲﾝﾃﾞｯｸｽ動作
                    //Module1.PlcBitWrite(ref "YIndex", ref false);
                    device = "YIndex";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 44:
                    //ﾃｰﾌﾞﾙYｲﾝﾃﾞｯｸｽ停止
                    //Module1.PlcBitWrite(ref "YIndexStop", ref false);
                    device = "YIndexStop";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 45:
                    //ﾃｰﾌﾞﾙXｲﾝﾃﾞｯｸｽ動作
                    //Module1.PlcBitWrite(ref "XIndex", ref false);
                    device = "XIndex";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 46:
                    //ﾃｰﾌﾞﾙXｲﾝﾃﾞｯｸｽ停止
                    //Module1.PlcBitWrite(ref "XIndexStop", ref false);
                    device = "XIndexStop";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;


                //ここからコメント解除_2014/10/07
                case 47:
                    //回転中心校正ﾃｰﾌﾞﾙX移動有りﾘｾｯﾄ
                    //Module1.PlcBitWrite(ref "RotXChangeReset", ref false);
                    device = "RotXChangeReset";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 48:
                    //寸法校正ﾃｰﾌﾞﾙX移動有りﾘｾｯﾄ
                    //Module1.PlcBitWrite(ref "DisXChangeReset", ref false);
                    device = "DisXChangeReset";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 49:
                    //回転中心校正ﾃｰﾌﾞﾙY移動有りﾘｾｯﾄ
                    //Module1.PlcBitWrite(ref "RotYChangeReset", ref false);
                    device = "RotYChangeReset";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 50:
                    //寸法校正ﾃｰﾌﾞﾙY移動有りﾘｾｯﾄ
                    //Module1.PlcBitWrite(ref "DisYChangeReset", ref false);
                    device = "DisYChangeReset";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 51:
                    //幾何歪校正ﾃｰﾌﾞﾙI.I.移動有りﾘｾｯﾄ
                    //Module1.PlcBitWrite(ref "VerIIChangeReset", ref false);
                    device = "VerIIChangeReset";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 52:
                    //回転中心校正ﾃｰﾌﾞﾙI.I.移動有りﾘｾｯﾄ
                    //Module1.PlcBitWrite(ref "RotIIChangeReset", ref false);
                    device = "RotIIChangeReset";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 53:
                    //ｹﾞｲﾝ校正ﾃｰﾌﾞﾙI.I.移動有りﾘｾｯﾄ
                    //Module1.PlcBitWrite(ref "GainIIChangeReset", ref false);
                    device = "GainIIChangeReset";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 54:
                    //寸法校正ﾃｰﾌﾞﾙI.I.移動有りﾘｾｯﾄ
                    //Module1.PlcBitWrite(ref "DisIIChangeReset", ref false);
                    device = "DisIIChangeReset";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                //ここまでコメント解除_2014/10/07
                
                case 55:
                    //ﾃｰﾌﾞﾙX原点復帰OFF
                    //Module1.PlcBitWrite(ref "XOrigin", ref false);
                    device = "XOrigin";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;


                //ここからコメント解除_2014/10/07
                case 56:
                    //ｽﾗｲｽﾗｲﾄON
                    //Module1.PlcBitWrite(ref "SLightOn", ref false);
                    device = "SLightOn";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 57:
                    //ｽﾗｲｽﾗｲﾄOFF
                    //Module1.PlcBitWrite(ref "SLightOff", ref false);
                    device = "SLightOff";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;

#if AllTest
                case 60:
                case 61:
                    //微調Y停止
                    Ans = Module1.YStgFastStop(Module1.hDevID, 0);
                    break;
#endif

                case 63:
                    //ﾄﾘｶﾞ出力開始
                    //Module1.PlcBitWrite(ref "TrgReq", ref false);
                    device = "TrgReq";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                //ここまでコメント解除_2014/10/07


                case 72:
                    //ﾃｰﾌﾞﾙY原点復帰OFF
                    //Module1.PlcBitWrite(ref "YOrigin", ref false);
                    device = "YOrigin";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;

                //ここからコメント解除_2014/10/07
                case 73:
                    //X線管X原点復帰OFF
                    //Module1.PlcBitWrite(ref "XrayXOrg", ref false);
                    device = "XrayXOrg";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 74:
                    //X線管X右移動OFF
                    cmdSwitch[74].BackColor = System.Drawing.Color.Lime;
                    //Module1.PlcBitWrite(ref "XrayXLeft", ref false);
                    device = "XrayXLeft";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 75:
                    //X線管X左移動OFF
                    cmdSwitch[75].BackColor = System.Drawing.Color.Lime;
                    //Module1.PlcBitWrite(ref "XrayXRight", ref false);
                    device = "XrayXRight";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 76:
                    //X線管Xｲﾝﾃﾞｯｸｽ動作
                    //Module1.PlcBitWrite(ref "XrayXIndex", ref false);
                    device = "XrayXIndex";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 77:
                    //X線管X停止
                    //Module1.PlcBitWrite(ref "XrayXStop", ref false);
                    device = "XrayXStop";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 78:
                    //X線管Y原点復帰OFF
                    //Module1.PlcBitWrite(ref "XrayYOrg", ref false);
                    device = "XrayYOrg";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 79:
                    //X線管Y右移動OFF
                    cmdSwitch[79].BackColor = System.Drawing.Color.Lime;
                    //Module1.PlcBitWrite(ref "XrayYForward", ref false);
                    device = "XrayYForward";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 80:
                    //X線管Y左移動OFF
                    cmdSwitch[80].BackColor = System.Drawing.Color.Lime;
                    //Module1.PlcBitWrite(ref "XrayYBackward", ref false);
                    device = "XrayYBackward";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 81:
                    //X線管Yｲﾝﾃﾞｯｸｽ動作
                    //Module1.PlcBitWrite(ref "XrayYIndex", ref false);
                    device = "XrayYIndex";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 82:
                    //X線管Y停止
                    //Module1.PlcBitWrite(ref "XrayYStop", ref false);
                    device = "XrayYStop";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;

#if AllTest
                case 83:
                    //X線管回転停止
                    //            PlcBitWrite "XrayRotStop", False
                    Module1.PioOutBit("XRAYROTSTOP", 0);
                    break;
                case 84:
                    //X線管回転ｲﾝﾃﾞｯｸｽ動作
                    //            PlcBitWrite "XrayRotIndex", False
                    Module1.PioOutBit("XRAYROTINDEX", 0);
                    break;
                case 85:
                    //X線管正転OFF
                    cmdSwitch[85].BackColor = System.Drawing.Color.Lime;
                    //            PlcBitWrite "XrayRotCW", False
                    Module1.PioOutBit("XRAYROTCW", 0);
                    break;
                case 86:
                    //X線管逆転OFF
                    cmdSwitch[86].BackColor = System.Drawing.Color.Lime;
                    //            PlcBitWrite "XrayRotCCW", False
                    Module1.PioOutBit("XRAYROTCCW", 0);
                    break;
                case 87:
                    //X線管回転原点復帰OFF
                    //            PlcBitWrite "XrayRotOrg", False
                    Module1.PioOutBit("XRAYROTORG", 0);
                    break;
#endif

                case 88:
                    //回転中心校正X線管X移動有りﾘｾｯﾄ
                    //Module1.PlcBitWrite(ref "RotXrayXChReset", ref false);
                    device = "RotXrayXChReset";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 89:
                    //寸法校正X線管X移動有りﾘｾｯﾄ
                    //Module1.PlcBitWrite(ref "DisXrayXChReset", ref false);
                    device = "DisXrayXChReset";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 90:
                    //回転中心校正X線管Y移動有りﾘｾｯﾄ
                    //Module1.PlcBitWrite(ref "RotXrayYChReset", ref false);
                    device = "RotXrayYChReset";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 91:
                    //寸法校正X線管Y移動有りﾘｾｯﾄ
                    //Module1.PlcBitWrite(ref "DisXrayYChReset", ref false);
                    device = "DisXrayYChReset";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 92:
                    //ﾃｰﾌﾞﾙｲﾝﾀｰﾛｯｸ有効
                    //Module1.PlcBitWrite(ref "TableMoveRestrict", ref false);
                    device = "TableMoveRestrict";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 93:
                    //ﾃｰﾌﾞﾙｲﾝﾀｰﾛｯｸ解除
                    //Module1.PlcBitWrite(ref "TableMovePermit", ref false);
                    device = "TableMovePermit";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 104:
                    //ｽｷｬﾝ位置校正ﾃｰﾌﾞﾙI.I.移動有りﾘｾｯﾄ
                    //Module1.PlcBitWrite(ref "SPIIChangeReset", ref false);
                    device = "SPIIChangeReset";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 105:
                    //ｽｷｬﾝ位置校正ﾃｰﾌﾞﾙI.I.移動有ｾｯﾄ
                    //Module1.PlcBitWrite(ref "SPIIChangeSet", ref false);
                    device = "SPIIChangeSet";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 106:
                    //扉電磁ﾛｯｸOFF
                    //Module1.PlcBitWrite(ref "DoorLockOff", ref false);
                    device = "DoorLockOff";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 107:
                    //扉電磁ﾛｯｸON
                    //Module1.PlcBitWrite(ref "DoorLockOn", ref false);
                    device = "DoorLockOn";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 110:
                    //寸法校正ﾃｰﾌﾞﾙI.I.移動有ｾｯﾄ
                    //Module1.PlcBitWrite(ref "DisIIChangeSet", ref false);
                    device = "DisIIChangeSet";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 111:
                    //ｹﾞｲﾝ校正ﾃｰﾌﾞﾙI.I.移動有ｾｯﾄ
                    //Module1.PlcBitWrite(ref "GainIIChangeSet", ref false);
                    device = "GainIIChangeSet";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 112:
                    //回転中心校正ﾃｰﾌﾞﾙI.I.移動有ｾｯﾄ
                    //Module1.PlcBitWrite(ref "RotIIChangeSet", ref false);
                    device = "RotIIChangeSet";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 113:
                    //幾何歪校正ﾃｰﾌﾞﾙI.I.移動有ｾｯﾄ
                    //Module1.PlcBitWrite(ref "VerIIChangeSet", ref false);
                    device = "VerIIChangeSet";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 114:
                    //回転中心校正ﾃｰﾌﾞﾙX移動有ｾｯﾄ
                    //Module1.PlcBitWrite(ref "RotXChangeSet", ref false);
                    device = "RotXChangeSet";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 115:
                    //寸法校正ﾃｰﾌﾞﾙX移動有ｾｯﾄ
                    //Module1.PlcBitWrite(ref "DisXChangeSet", ref false);
                    device = "DisXChangeSet";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 116:
                    //回転中心校正ﾃｰﾌﾞﾙY移動有ｾｯﾄ
                    //Module1.PlcBitWrite(ref "RotYChangeSet", ref false);
                    device = "RotYChangeSet";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 117:
                    //寸法校正ﾃｰﾌﾞﾙY移動有ｾｯﾄ
                    //Module1.PlcBitWrite(ref "DisYChangeSet", ref false);
                    device = "DisYChangeSet";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 109:
                    //I.I.絞り左開OFF
                    //Module1.PlcBitWrite(ref "IrisLOpen", ref false);
                    device = "IrisLOpen";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 118:
                    //I.I.絞り左閉OFF
                    //Module1.PlcBitWrite(ref "IrisLClose", ref false);
                    device = "IrisLClose";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 119:
                    //I.I.絞り右開OFF
                    //Module1.PlcBitWrite(ref "IrisROpen", ref false);
                    device = "IrisROpen";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 120:
                    //I.I.絞り右閉OFF
                    //Module1.PlcBitWrite(ref "IrisRClose", ref false);
                    device = "IrisRClose";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 121:
                    //I.I.絞り上開OFF
                    //Module1.PlcBitWrite(ref "IrisUOpen", ref false);
                    device = "IrisUOpen";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 122:
                    //I.I.絞り上閉OFF
                    //Module1.PlcBitWrite(ref "IrisUClose", ref false);
                    device = "IrisUClose";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 123:
                    //I.I.絞り下開OFF
                    //Module1.PlcBitWrite(ref "IrisDOpen", ref false);
                    device = "IrisDOpen";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;
                case 124:
                    //I.I.絞り下閉OFF
                    //Module1.PlcBitWrite(ref "IrisDClose", ref false);
                    device = "IrisDClose";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;  
                //ここまでコメント解除_2014/10/07

                case 143:
                    //ﾒｶﾘｾｯﾄ停止要求
                    //Module1.PlcBitWrite(ref "MechaResetStop", ref false);
                    device = "MechaResetStop";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;

                case 144:
                    //ﾒｶﾘｾｯﾄ要求
                    //Module1.PlcBitWrite(ref "MechaReset", ref false);
                    device = "MechaReset";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;

                case 149:
                    //ｼｬｯﾀ
                    //PlcBitWrite "Shutter", False
                    device = "MechaReset";
                    bdata = false;
                    Module1.PlcBitWrite(ref device, ref bdata);
                    break;

            }

        }

        private void hsbSpeed_Scroll(object sender, ScrollEventArgs e)
        {
            //try
            //{
            //    HScrollBar hsb = (HScrollBar)sender;
            //    int Index = hsbSpeed.IndexOf(hsb);

            //    //運転速度変更
            //    if (Index < 7)
            //    {
            //        txtSpeed[Index].Text = Convert.ToString(Convert.ToDouble(hsb.Value / 10));
            //    }
            //    else if ((Index >= 7 & Index <= 8))
            //    {
            //        txtSpeed[Index].Text = Convert.ToString(Convert.ToDouble(hsbSpeed[Index].Value / 100));
            //    }
            //    else if (Index == 9)
            //    {
            //        txtSpeed[Index].Text = Convert.ToString(Convert.ToDouble(hsbSpeed[Index].Value / 10000));
            //    }
            //    SpeedSet(ref Index);
            //}
            //catch
            //{
            //}

            //switch (e.Type)
            //{
            //    case System.Windows.Forms.ScrollEventType.ThumbTrack:
            //        //hsbImage_Scroll_Renamed(eventArgs.NewValue);
            //        break;
            //    case System.Windows.Forms.ScrollEventType.EndScroll:
            //        //hsbImage_Change(eventArgs.NewValue);
            //        break;
            //}

        }

        private void hsbSpeed_MouseCaptureChanged(object sender, EventArgs e)
        {
            HScrollBar hsb = (HScrollBar)sender;
            int Index = hsbSpeed.IndexOf(hsb);
            if (!hsb.Capture)
            {
                SpeedSet(ref Index);
            }     

        }


        private void hsbSpeed_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                HScrollBar hsb = (HScrollBar)sender;
                int Index = hsbSpeed.IndexOf(hsb);

                //運転速度変更
                if (Index < 7)
                {
                    txtSpeed[Index].Text = Convert.ToString(Convert.ToDouble(hsb.Value) / 10f);
                }
                else if ((Index >= 7 & Index <= 8))
                {
                    txtSpeed[Index].Text = Convert.ToString(Convert.ToDouble(hsbSpeed[Index].Value) / 100f);
                }
                else if (Index == 9)
                {
                    txtSpeed[Index].Text = Convert.ToString(Convert.ToDouble(hsbSpeed[Index].Value) / 10000f);
                }
                //SpeedSet(ref Index);
            }
            catch
            {
            }

        }


        //運転速度設定
        private void SpeedSet(ref int Index)
        {
            //    Dim SpeedData(6) As Integer

            if (Index < 7)
            {
                Module1.SpeedData[Index] = Convert.ToInt32(Convert.ToSingle(txtSpeed[Index].Text) * 10);
                if (Module1.SpeedData[Index] > (hsbSpeed[Index].Maximum - hsbSpeed[Index].LargeChange + 1))
                {
                    Module1.SpeedData[Index] = (hsbSpeed[Index].Maximum - hsbSpeed[Index].LargeChange + 1);
                    txtSpeed[Index].Text = Convert.ToString(Convert.ToDouble(hsbSpeed[Index].Maximum - hsbSpeed[Index].LargeChange + 1) / 10f);
                }
                else if (Module1.SpeedData[Index] < hsbSpeed[Index].Minimum)
                {
                    Module1.SpeedData[Index] = hsbSpeed[Index].Minimum;
                    //SpeedData(Index) = hsbSpeed(Index).Min + 0.0001
                    txtSpeed[Index].Text = Convert.ToString(Convert.ToDouble(hsbSpeed[Index].Minimum) / 10f);
                    //txtSpeed(Index).Text = CStr(hsbSpeed(Index).Min / 10 + 0.0001)
                }

                ////光学系X運転速度設定
                //string device = "";
                //if (Index == 0)
                //{
                //    device = "XSpeed";
                //}
                //if (Index == 1)
                //{
                //    device = "YSpeed";
                //}
                //if (Index == 4)
                //{
                //    device = "IISpeed";
                //}

                //string data = Convert.ToString(Module1.SpeedData[Index]);
                //if (Module1.PlcWordWrite(ref device, ref data) != 0)
                //    return;
             }
            else if ((Index >= 7 & Index <= 8))
            {
                Module1.SpeedData[Index] = Convert.ToInt32(Convert.ToSingle(txtSpeed[Index].Text) * 100);
                if (Module1.SpeedData[Index] > (hsbSpeed[Index].Maximum - hsbSpeed[Index].LargeChange + 1))
                {
                    Module1.SpeedData[Index] = (hsbSpeed[Index].Maximum - hsbSpeed[Index].LargeChange + 1);
                    txtSpeed[Index].Text = Convert.ToString(Convert.ToDouble(hsbSpeed[Index].Maximum - hsbSpeed[Index].LargeChange + 1) / 100f);
                    
              
                }
                else if (Module1.SpeedData[Index] < hsbSpeed[Index].Minimum)
                {
                    Module1.SpeedData[Index] = hsbSpeed[Index].Minimum;
                    txtSpeed[Index].Text = Convert.ToString(Convert.ToDouble(hsbSpeed[Index].Minimum) / 100f);
                }


            }
            else if (Index == 9)
            {
                Module1.SpeedData[Index] = Convert.ToInt32(Convert.ToSingle(txtSpeed[Index].Text) * 10000);
                if (Module1.SpeedData[Index] > (hsbSpeed[Index].Maximum - hsbSpeed[Index].LargeChange + 1))
                {
                    Module1.SpeedData[Index] = (hsbSpeed[Index].Maximum - hsbSpeed[Index].LargeChange + 1);
                    txtSpeed[Index].Text = Convert.ToString(Convert.ToDouble(hsbSpeed[Index].Maximum - hsbSpeed[Index].LargeChange + 1) / 10000f);
                }
                else if (Module1.SpeedData[Index] < hsbSpeed[Index].Minimum)
                {
                    Module1.SpeedData[Index] = hsbSpeed[Index].Minimum;
                    txtSpeed[Index].Text = Convert.ToString(Convert.ToDouble(hsbSpeed[Index].Minimum) / 10000f);
                }
            }



        }

        //ﾃｰﾌﾞﾙ-X線管ｲﾝﾀｰﾛｯｸ設定
        private void txtFCDLimit_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
            string device;
            string data;
            device = "FCDLimit";
            data = Convert.ToString(Convert.ToInt32(txtFCDLimit.Text));
            
            try
            {
                //Module1.PlcWordWrite(ref "FCDLimit", ref Convert.ToString(Convert.ToInt32(txtFCDLimit.Text)));
                Module1.PlcWordWrite(ref device, ref data);
            }
            catch
            {
            }
		}

        //微調ﾃｰﾌﾞﾙ-X線管ｲﾝﾀｰﾛｯｸ設定
        private void txtFCDFineLimit_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
            string device;
            string data;
            device = "FCDFineLimit";
            data = Convert.ToString(Convert.ToInt32(txtFCDFineLimit.Text));

            try
            {
                //
			    //Module1.PlcWordWrite(ref "FCDFineLimit", ref Convert.ToString(Convert.ToInt32(txtFCDFineLimit.Text)));
                Module1.PlcWordWrite(ref device, ref data);
            }
            catch
            {
            }
		}

        //微調ﾃｰﾌﾞﾙ-検出器ｲﾝﾀｰﾛｯｸ設定
         private void txtFIDFineLimit_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
            string device;
            string data;
            device = "FIDFineLimit";
            data = Convert.ToString(Convert.ToInt32(txtFIDFineLimit.Text));
            
            try
            {
                //
			    //Module1.PlcWordWrite(ref "FIDFineLimit", ref Convert.ToString(Convert.ToInt32(txtFIDFineLimit.Text)));
                Module1.PlcWordWrite(ref device, ref data);
            }
            catch
            {
            }
		}

        //ｲﾝﾃﾞｯｸｽ位置入力時に書込みを行うように追加 by 稲葉 02-03-04
        private void txtIIIndexPosition_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
            string device;
            string data;
            device = "IIIndexPosition";
            data = Convert.ToString(Convert.ToInt32(Convert.ToSingle(txtIIIndexPosition.Text) * 10));

            try
            {
                //
			    //ﾃｰﾌﾞﾙX指定位置
			    //Module1.PlcWordWrite(ref "IIIndexPosition", ref Convert.ToString(Convert.ToInt32(Convert.ToSingle(txtIIIndexPosition.Text) * 10)));
                Module1.PlcWordWrite(ref device, ref data);
            }
            catch
            {
            }
		}

        //ｽｷｬﾝ開始角度設定
        private void txtScanStartPos_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
            string device;
            string data;
            device = "ScanStartPos";
            data = Convert.ToString(Convert.ToInt32(Convert.ToSingle(txtScanStartPos.Text) * 10));

            try
            {
                //
			    //Module1.PlcWordWrite(ref "ScanStartPos", ref Convert.ToString(Convert.ToInt32(Convert.ToSingle(txtScanStartPos.Text) * 10)));
                Module1.PlcWordWrite(ref device, ref data);
            }
            catch
            {
            }
		}

        //光学系回転速度設定
        private void txtSpeed_TextChanged(object sender, EventArgs e)
		{
            try
            {
                //
                TextBox txtb = (TextBox)sender;
                int Index = txtSpeed.IndexOf(txtb);

                if (Index == 9) {
				    //運転速度設定
					SpeedSet(ref Index);
			        
                    //光学系回転運転速度設定
 				    //Module1.PlcWordWrite(ref "XrayRotSpeed", ref Convert.ToString(Module1.SpeedData[9]));
                    string device = "XrayRotSpeed";
                    string data = Convert.ToString(Module1.SpeedData[9]);
                    Module1.PlcWordWrite(ref device, ref data);
                } 
            
            }
            catch
            {
            }
		}
 

        //ﾄﾘｶﾞ出力周期時間設定
        private void txtTrgCycleTime_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
            string device;
            string data;
            device = "TrgCycleTime";
            data = Convert.ToString(Convert.ToInt32(txtTrgCycleTime.Text));
            
            try
            {
                //
			    //Module1.PlcWordWrite(ref "TrgCycleTime", ref Convert.ToString(Convert.ToInt32(txtTrgCycleTime.Text)));
                Module1.PlcWordWrite(ref device, ref data);
            }
            catch
            {
            }
		}

        //ﾄﾘｶﾞ出力ON時間設定
        private void txtTrgTime_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
            string device;
            string data;
            device = "TrgTime";
            data = Convert.ToString(Convert.ToInt32(txtTrgTime.Text));
            
            try
            {
                //
                //Module1.PlcWordWrite(ref "TrgTime", ref Convert.ToString(Convert.ToInt32(txtTrgTime.Text)));
                Module1.PlcWordWrite(ref device, ref data);
            }
            catch
            {
            }
		}

        //ｲﾝﾃﾞｯｸｽ位置入力時に書込みを行うように追加 by 稲葉 02-03-04
        private void txtXIndexPosition_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
            string device;
            string data;
            device = "XIndexPosition";
            data = Convert.ToString(Convert.ToInt32(Convert.ToSingle(txtXIndexPosition.Text) * 100));

            try
            {
                //
 			    //ﾃｰﾌﾞﾙX指定位置
			    //Module1.PlcWordWrite(ref "XIndexPosition", ref Convert.ToString(Convert.ToInt32(Convert.ToSingle(txtXIndexPosition.Text) * 100)));
                Module1.PlcWordWrite(ref device, ref data);
            }
            catch
            {
            }

        }

        //ｲﾝﾃﾞｯｸｽ位置入力時に書込みを行う
        private void txtXrayRotIndexPosition_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
            string device;
            string data;
            device = "XrayRotPos";
            data = Convert.ToString(Convert.ToInt32(Convert.ToSingle(txtXrayRotIndexPosition.Text) * 100));

            try
            {
                //
			    //X線管回転指定位置
                //Module1.PlcWordWrite(ref "XrayRotPos", ref Convert.ToString(Convert.ToInt32(Convert.ToSingle(txtXrayRotIndexPosition.Text) * 100)));
                Module1.PlcWordWrite(ref device, ref data);
            }
            catch
            {
            }
            
		}

        //ｲﾝﾃﾞｯｸｽ位置入力時に書込みを行う
        private void txtXrayXIndexPosition_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
            string device;
            string data;
            device = "XrayXPos";
            data = Convert.ToString(Convert.ToInt32(Convert.ToSingle(txtXrayXIndexPosition.Text) * 100));
            
            try
            {
                //
                //X線管X指定位置
                //Module1.PlcWordWrite(ref "XrayXPos", ref Convert.ToString(Convert.ToInt32(Convert.ToSingle(txtXrayXIndexPosition.Text) * 100)));
                Module1.PlcWordWrite(ref device, ref data);
            }
            catch
            {
            }

 		}

        //ｲﾝﾃﾞｯｸｽ位置入力時に書込みを行う
        private void txtXrayYIndexPosition_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
            string device;
            string data;
            device = "XrayYPos";
            data = Convert.ToString(Convert.ToInt32(Convert.ToSingle(txtXrayYIndexPosition.Text) * 100));

            try
            {
                //
               //X線管Y指定位置
			    //Module1.PlcWordWrite(ref "XrayYPos", ref Convert.ToString(Convert.ToInt32(Convert.ToSingle(txtXrayYIndexPosition.Text) * 100)));
                Module1.PlcWordWrite(ref device, ref data);
            }
            catch
            {
            }
		} 

        //ｲﾝﾃﾞｯｸｽ位置入力時に書込みを行うように追加 by 稲葉 02-03-04
        private void txtYIndexPosition_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
            string device;
            string data;
            device = "YIndexPosition";
            data = Convert.ToString(Convert.ToInt32(Convert.ToSingle(txtYIndexPosition.Text) * 10));
            try
            {
                //
    			//ﾃｰﾌﾞﾙX指定位置
			    //Module1.PlcWordWrite(ref "YIndexPosition", ref Convert.ToString(Convert.ToInt32(Convert.ToSingle(txtYIndexPosition.Text) * 10)));
                Module1.PlcWordWrite(ref device, ref data);
            }
            catch
            {
            }			
            
		}



        #region   EXM管電圧/管電流設定イベントは　numericUpDown1_ValueChanged、numericUpDown2_ValueChangedに変更
        ////管電圧下降設定
        ////イベント 
        //private void UpDown1_DownClick()
        //{
        //    try
        //    {
        //        //
        //        if (Convert.ToSingle(txtEXMTVSet.Text) > Convert.ToSingle(Module1.MyfMSts.lblEXMMinTV.Text))
        //        {
        //            txtEXMTVSet.Text = Convert.ToString(Convert.ToDouble(txtEXMTVSet.Text) - 1);
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}

        ////管電圧上昇設定
        ////イベント 
        //private void UpDown1_UpClick()
        //{
        //    try
        //    {
        //        //
        //        if (Convert.ToSingle(txtEXMTVSet.Text) < Convert.ToSingle(Module1.MyfMSts.lblEXMMaxTV.Text))
        //        {
        //            txtEXMTVSet.Text = Convert.ToString(Convert.ToDouble(txtEXMTVSet.Text) + 1);
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}

        ////管電流下降設定
        ////イベント 
        //private void UpDown2_DownClick()
        //{
        //    try
        //    {
        //        //
        //        if (Convert.ToSingle(txtEXMTCSet.Text) > Convert.ToSingle(Module1.MyfMSts.lblEXMMinTC.Text))
        //        {
        //            txtEXMTCSet.Text = Convert.ToString(Convert.ToDouble(txtEXMTCSet.Text) - 0.01);
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}

        ////管電流上昇設定
        ////イベント 
        //private void UpDown2_UpClick()
        //{
        //    try
        //    {
        //        //
        //        if (Convert.ToSingle(txtEXMTCSet.Text) < Convert.ToSingle(Module1.MyfMSts.lblEXMMaxTC.Text))
        //        {
        //            txtEXMTCSet.Text = Convert.ToString(Convert.ToDouble(txtEXMTCSet.Text) + 0.01);
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}
        #endregion
        
        //追加2014/10/07
        //管電圧上昇/下降設定
        //イベント 
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value == Convert.ToDecimal(txtEXMTVSet.Text)) return;
            
            try
            {
                //
                if (Convert.ToSingle(numericUpDown1.Value) > Convert.ToSingle(Module1.MyfMSts.lblEXMMaxTV.Text))
                {
                    numericUpDown1.Value = numericUpDown1.Value - numericUpDown1.Increment;
                }
                //
                if (Convert.ToSingle(numericUpDown1.Value) < Convert.ToSingle(Module1.MyfMSts.lblEXMMinTV.Text))
                {
                    numericUpDown1.Value = numericUpDown1.Value + numericUpDown1.Increment;
                }
                txtEXMTVSet.Text = numericUpDown1.Value.ToString();

            }
            catch
            {
            }
        }

        //追加2014/10/07
        //管電流上昇/下降設定
        //イベント 
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown2.Value == Convert.ToDecimal(txtEXMTCSet.Text)) return;
            
            try
            {
                if (Convert.ToSingle(numericUpDown2.Value) > Convert.ToSingle(Module1.MyfMSts.lblEXMMaxTC.Text))
                {
                    numericUpDown2.Value = numericUpDown2.Value - numericUpDown2.Increment;
                
                }
                else if (Convert.ToSingle(numericUpDown2.Value)  < Convert.ToSingle(Module1.MyfMSts.lblEXMMinTC.Text))
                {
                    numericUpDown2.Value = numericUpDown2.Value + numericUpDown2.Increment;
                }
                txtEXMTCSet.Text = numericUpDown2.Value.ToString();

            }
            catch
            {
            }
        }

        //追加2014/10/07
        private void txtEXMTVSet_TextChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value != Convert.ToDecimal(txtEXMTVSet.Text))
                numericUpDown1.Value = Convert.ToDecimal(txtEXMTVSet.Text);
        }

        //追加2014/10/07
        private void txtEXMTCSet_TextChanged(object sender, EventArgs e)
        {
            if (numericUpDown2.Value != Convert.ToDecimal(txtEXMTCSet.Text))
                numericUpDown2.Value = Convert.ToDecimal(txtEXMTCSet.Text);
        }

	}
}
