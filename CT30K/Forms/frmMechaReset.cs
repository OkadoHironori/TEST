using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//
using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;

namespace CT30K
{
	///* ************************************************************************** */
	///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver15.0              */
	///* 客先　　　　： ?????? 殿                                                   */
	///* プログラム名： frmMechaReset.frm                                           */
	///* 処理概要　　： 「メカ－詳細」画面                                          */
	///* 注意事項　　： なし                                                        */
	///* -------------------------------------------------------------------------- */
	///* 適用計算機　： DOS/V PC                                                    */
	///* ＯＳ　　　　： Windows XP  (SP2)                                           */
	///* コンパイラ　： VB 6.0                                                      */
	///* -------------------------------------------------------------------------- */
	///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
	///*                                                                            */
	///* v15.0       09/04/10    (SS1) 間々田        新規作成                       */
	///*                                                                            */
	///* -------------------------------------------------------------------------- */
	///* ご注意：                                                                   */
	///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
	///*                                                                            */
	///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2009                 */
	///* ************************************************************************** */
	public partial class frmMechaReset : Form
	{
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
		//Ｘ線管回転ステータス   v9.0 added by 間々田 2004/02/12
		//Enum XrayRotStatus
		//    XrayRotNotBusy  '停止中
		//    XrayRotBusy     '動作中
		//    XrayCWLimit     '正転限
		//    XrayCCWlimit    '逆転限
		//    XrayRotError    '異常
		//End Enum
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

		//private Label lblStatus;

		//タイマーカウント
		private int TimerCount;

        //追加2014/10/07hata_v19.51反映
        //リセット中の軸 by長野 2013/12/17(0:X軸、1:Y軸)
        //private int ResetAxis;
        //Rev23.40 変更 by長野 2016/06/19
        private bool ResetXAxis = false;
        private bool ResetYAxis = false;

		//メカコントロールフォーム参照用
		private frmMechaControl myMechaControl;

        //Rev23.20 追加
        private bool mechaAllResetExFlg = false;

		private Button[] cmdFilter = null;
		private Button[] cmdTilt = null;
		private Button[] cmdTableReset = null;
		private Button[] cmdFxyReset = null;
		private CTStatus[] stsFineTable = null;
		private CWButton[] cwbtnFineTableX = null;
		private CWButton[] cwbtnFineTableY = null;
		private CWButton[] cwbtnIIMove = null;

        private Button[] cmdTiltAndRot_Reset = null;//Rev22.00 回転傾斜テーブル 追加 by長野 2015/08/20
        private CTStatus[] stsTiltAndRot = null;//Rev22.00 回転傾斜テーブル 追加 by長野 2015/08/20

        private static frmMechaReset _Instance = null;

		//
		// cwbtnFineTableX_ValueChanged イベントで使用する static フィールド
		//
		//２重起動をさせないためのフラグ
		private static bool cwbtnFineTableX_ValueChanged_IsBusy;

		//
		// cwbtnFineTableY_ValueChanged イベントで使用する static フィールド
		//
		//２重起動をさせないためのフラグ
		private static bool cwbtnFineTableY_ValueChanged_IsBusy;

        //Rev26.40 X線・高速度カメラ昇降軸用 by chouno
        private int stsUpDownPos = 0;     //UpDownｽﾗｲﾀﾞｰの現在位置ステータス
        private string PreUpDownValText = "";   //UpDownTextを変更した前回の値
        private char presskye = (char)0;  //Keypressの値       
        private bool mousecapture = false;

        //Rev26.40 add by chouno 2019/02/17
        private modHighSpeedCamera.IIModeConstants myIImode = modHighSpeedCamera.IIModeConstants.IIMode_None;

		public frmMechaReset()
		{
			InitializeComponent();

			cmdFilter = new Button[] { cmdFilter0, cmdFilter1, cmdFilter2, cmdFilter3, cmdFilter4, cmdFilter5 };
			cmdTilt = new Button[] { cmdTilt0, cmdTilt1, cmdTilt2 };
			cmdTableReset = new Button[] { cmdTableReset0, cmdTableReset1 };
			cmdFxyReset = new Button[] { cmdFxyReset0, cmdFxyReset1 };
			stsFineTable = new CTStatus[] { stsFineTable0, stsFineTable1 };

            cwbtnFineTableX = new CWButton[] { cwbtnFineTableX0, cwbtnFineTableX1 };
			cwbtnFineTableY = new CWButton[] { cwbtnFineTableY0, cwbtnFineTableY1 };
			cwbtnIIMove = new CWButton[] { cwbtnIIMove0, cwbtnIIMove1 };

            cmdTiltAndRot_Reset = new Button[] { cmdTiltAndRot_Reset0, cmdTiltAndRot_Reset1 }; //Rev22.00 回転傾斜テーブル 追加 by長野 2015/08/20
            stsTiltAndRot = new CTStatus[] { stsTiltAndRot0, stsTiltAndRot1 }; //Rev22.00 回転傾斜テーブル 追加 by長野 2015/08/20

            //透明色の設定を行う
            ((Bitmap)cmdTilt0.BackgroundImage).MakeTransparent(Color.White);
            ((Bitmap)cmdTilt2.BackgroundImage).MakeTransparent(Color.White);
            
            stsDetChangeReset.Font = new Font(stsDetChangeReset.Font.Name, 11);
			stsFineTable0.Font = new Font(stsFineTable0.Font.Name, 11);
			stsFineTable1.Font = new Font(stsFineTable1.Font.Name, 11);
			stsIIChangeReset.Font = new Font(stsIIChangeReset.Font.Name, 11);
			stsPhantom.Font = new Font(stsPhantom.Font.Name, 11);
			stsRotate.Font = new Font(stsRotate.Font.Name, 11);
			stsUpDown.Font = new Font(stsUpDown.Font.Name, 11);
			stsXrayRot.Font = new Font(stsXrayRot.Font.Name, 11);
            stsTiltAndRot0.Font = new Font(stsTiltAndRot0.Name, 11);
            stsTiltAndRot1.Font = new Font(stsTiltAndRot1.Name, 11);
            stsXrayChangeReset.Font = new Font(stsXrayChangeReset.Name, 11); //Rev23.10 追加 by長野 2015/09/20
            stsCTgene2and3.Font = new Font(stsCTgene2and3.Name, 11); //Rev23.20 追加 by長野 2015/12/17
        }

		public static frmMechaReset Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmMechaReset();
				}

				return _Instance;
			}
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
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void frmMechaReset_Load(object sender, EventArgs e)
		{
			//キャプションのセット
			SetCaption();
            
			//コントロールの初期設定
			InitControls();

			//表示
			myMechaControl_SeqCommChanged(this, EventArgs.Empty);
			myMechaControl_MecainfChanged(this, EventArgs.Empty);

            //変更2014/10/07hata_v19.51反映
			//v17.20 CTメニュー画面にロックをかける by 長野 2010/09/09
            //frmCTMenu.Instance.Enabled = false;			//v18.00追加 byやまおか 2011/02/12
            //スキャンコントロール画面にロックをかける
            frmScanControl.Instance.Enabled = false;     //v18.00追加 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

			//メカ制御画面への参照
			myMechaControl = frmMechaControl.Instance;
			myMechaControl.MecainfChanged += new EventHandler(myMechaControl_MecainfChanged);
			myMechaControl.SeqCommChanged += new EventHandler(myMechaControl_SeqCommChanged);
		
        }


		//*************************************************************************************************
		//機　　能： フォームアンロード時処理（イベント処理）
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Cancel          [ /O] Integer   True（0以外）: アンロードをキャンセル
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void frmMechaReset_FormClosed(object sender, FormClosedEventArgs e)
		{
            //frmCTMenu frmCTMenu = frmCTMenu.Instance; //'ロックしない   'v18.00削除 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            ////v17.20 CTメニューのロックを解除 2010/09/09
            //frmCTMenu.Enabled = true;         //'ロックしない   'v18.00削除 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //frmCTMenu.InitControls();         //'ロックしない   'v18.00削除 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //frmCTMenu.Toolbar1.Refresh();     //'ロックしない   'v18.00削除 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //追加2014/10/07hata_v19.51反映
            //スキャンコントロール画面のロックを解除する
            frmScanControl.Instance.Enabled = true;     //v18.00追加 byやまおか 2011/02/12

			//メカ制御画面への参照破棄
            if (myMechaControl != null) //追加201501/26hata_if文追加
            {
                myMechaControl.MecainfChanged -= myMechaControl_MecainfChanged;
                myMechaControl.SeqCommChanged -= myMechaControl_SeqCommChanged;
                myMechaControl = null;
            }
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

           
            //foreach(CTStatus Ctl in this.Controls)
            //{
            //    Ctl.Text = Ctl.Caption;          
            //}
            //foreach (CTButton Ctl in this.Controls)
            //{
            //    Ctl.Text = Ctl.Caption;
            //}

			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);

			this.Text = StringTable.BuildResStr(StringTable.IDS_Details, 10507);							//メカ準備－詳細

			//テーブル回転フレーム
            // Mod Start 2018/08/24 M.Oyama 中国語対応
            //fraTableRotate.Text = StringTable.BuildResStr(StringTable.IDS_Rotate, StringTable.IDS_Table);	//テーブル回転
            fraTableRotate.Text = StringTable.BuildResStr(StringTable.IDS_Rotate, StringTable.IDS_SampleTable);	//テーブル回転
            // Mod End 2018/08/24

			//テーブル昇降フレーム
            //変更2014/10/07hata_v19.51反映
			//fraTableUpDown.Text = StringTable.BuildResStr(StringTable.IDS_UpDown, StringTable.IDS_Table);	//テーブル昇降
            //v19.51 X線・検出器昇降とテーブル昇降の場合で場合分け
            if ((CTSettings.t20kinf.Data.ud_type == 0))
            {
                // Mod Start 2018/08/24 M.Oyama 中国語対応
                //fraTableUpDown.Text = StringTable.BuildResStr(StringTable.IDS_UpDown, StringTable.IDS_Table);           //テーブル昇降
                fraTableUpDown.Text = StringTable.BuildResStr(StringTable.IDS_UpDown, StringTable.IDS_SampleTable);       //テーブル昇降
                // Mod End 2018/08/24
            }
            else
            {
                fraTableUpDown.Text = StringTable.BuildResStr(StringTable.IDS_UpDown, StringTable.IDS_XrayDetector);    //X線・検出器
            }

			//テーブルＸ軸フレーム
			fraTableX.Text = StringTable.GetResString(StringTable.IDS_TableAxis,CTSettings.AxisName[0]);	//テーブルX軸
			cmdTableReset0.Text = CTResources.LoadResString(StringTable.IDS_btnReset);						//リセット

			//テーブルＹ軸フレーム
            fraTableY.Text = StringTable.GetResString(StringTable.IDS_TableAxis, CTSettings.AxisName[1]);	//テーブルY軸
			cmdTableReset1.Text = CTResources.LoadResString(StringTable.IDS_btnReset);						//リセット

            //Rev23.20 追加 by長野 2015/12/21
            cmdDetSystemReset.Text = CTResources.LoadResString(StringTable.IDS_btnReset);						//リセット

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//Ｘ線管回転フレーム
			//    fraXrayRotate.Caption = BuildResStr(IDS_Rotate, IDS_XrayTube)       'Ｘ線管回転
			//
			//    'Ｘ線管Ｘ軸フレーム
			//    fraXrayX.Caption = LoadResString(IDS_XrayTube) & " " & AxisName(0)  'Ｘ線管 Ｘ軸
			//    cmdXrayPosReset(0).Caption = LoadResString(IDS_btnReset)            'リセット
			//
			//    'Ｘ線管Ｙ軸フレーム
			//    fraXrayY.Caption = LoadResString(IDS_XrayTube) & " " & AxisName(1)  'Ｘ線管 Ｙ軸
			//    cmdXrayPosReset(1).Caption = LoadResString(IDS_btnReset)            'リセット
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			//微調テーブルＸ軸フレーム
            fraFineX.Text = StringTable.GetResString(12123, CTSettings.AxisName[0]);		//微調テーブルＸ軸

			//微調テーブルＹ軸フレーム
            fraFineY.Text = StringTable.GetResString(12123, CTSettings.AxisName[1]);		//微調テーブルＹ軸

            //回転傾斜テーブル(回転) フレーム
            fraTiltAndRot_Tilt.Text = CTResources.LoadResString(22020); //Rev22.00 追加 by長野 2015/08/20

            //回転傾斜テーブル(傾斜) フレーム
            fraTiltAndRot_Rot.Text = CTResources.LoadResString(22021); //Rev22.00 追加 by長野 2015/08/20

			//フィルタフレーム
            //変更2014/10/07hata_v19.51反映
            //cmdFilter0.Text = CTResources.LoadResString(StringTable.IDS_OFF);			//なし
            //ボタン名称をinfdefから取得する     'v18.00変更 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            int i = 0;
   		    for (i = CTSettings.infdef.Data.xfilter_c.GetLowerBound(0); i <=  CTSettings.infdef.Data.xfilter_c.GetUpperBound(0); i++)
            {
                cmdFilter[i].Text = CTSettings.infdef.Data.xfilter_c[i].GetString();
            }

			//チルトフレーム
			cmdTilt1.Text = CTResources.LoadResString(12136);							//原点

			//I.I.移動（またはFPD移動）フレーム                                  'v17.10追加 byやまおか 2010/08/27
            fraIIMove.Text = StringTable.GetResString(StringTable.IDS_Move, CTSettings.GStrIIOrFPD);							//I.I.移動（またはFPD移動）
            toolTip.SetToolTip(cwbtnIIMove0, StringTable.GetResString(StringTable.IDS_Forward, CTSettings.GStrIIOrFPD));	//I.I.前進   'v17.60修正 byやまおか 2011/06/07
            toolTip.SetToolTip(cwbtnIIMove1, StringTable.GetResString(StringTable.IDS_Back, CTSettings.GStrIIOrFPD));		//I.I.後退   'v17.60修正 byやまおか 2011/06/07

			//I.I.電源（またはFPD電源）                                          '追加 by 間々田 2009/07/21
            ctbtnIIPower.Caption = StringTable.GetResString(StringTable.IDS_PowerSupply, CTSettings.GStrIIOrFPD);			//I.I.電源（またはFPD電源）

			//cwbtnIIMove(0).ToolTipText = GetResString(IDS_Forward, "I.I.")      'I.I.前進  '追加 by 間々田 2009/07/21 メカ準備画面から移動
			//cwbtnIIMove(1).ToolTipText = GetResString(IDS_Back, "I.I.")         'I.I.後退  '追加 by 間々田 2009/07/21 メカ準備画面から移動

            //追加2014/10/07hata_v19.51反映
            //検出器切替、検出器シフト   'v18.00追加 byやまおか 2011/02/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            fraDetChangeReset.Text = CTResources.LoadResString((CTSettings.SecondDetOn ? StringTable.IDS_DetChange : StringTable.IDS_DetShift));

            //Rev26.40 add by chouno 2019/02/24
            lblMax.Text = CTSettings.iniValue.XrayAndHSCMaxPos.ToString();
            lblMin.Text = CTSettings.iniValue.XrayAndHSCMinPos.ToString();
        
        }


		//*************************************************************************************************
		//機　　能： 各コントロールの初期化
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void InitControls()
		{
			//scaninh.xray_rotate=1の時はＹ軸リセットボタンは非表示
			//fraTableY.Visible = (.xray_rotate = 0)
			fraTableX.Visible = (CTSettings.scaninh.Data.xray_rotate == 0);

			//v29.99 今のところ不要のためfalseにしておく by長野 2013/04/08'''''ここから'''''
			//        'Ｘ線管回転フレーム
			//        fraXrayRotate.Visible = (.xray_rotate = 0)
			//
			//        'Ｘ線管Ｘ軸フレーム
			//        fraXrayX.Visible = (.xray_rotate = 0)
			//
			//        'Ｘ線管Ｙ軸フレーム
			//        fraXrayY.Visible = (.xray_rotate = 0)
			//Ｘ線管回転フレーム
			fraXrayRotate.Visible = false;

			//Ｘ線管Ｘ軸フレーム
			fraXrayX.Visible = false;

			//Ｘ線管Ｙ軸フレーム
			fraXrayY.Visible = false;
			//v29.99 上記は、今のところ不要のためfalseにしておく by長野 201304/08'''''ここまで'''''

			//微調テーブルＸ軸フレーム
			fraFineX.Visible = (CTSettings.scaninh.Data.fine_table == 0) && (CTSettings.scaninh.Data.fine_table_x == 0);

			//微調テーブルＹ軸フレーム
			fraFineY.Visible = (CTSettings.scaninh.Data.fine_table == 0) && (CTSettings.scaninh.Data.fine_table_y == 0);

            //回転傾斜テーブル(傾斜)
            fraTiltAndRot_Tilt.Visible = (CTSettings.scaninh.Data.tilt_and_rot == 0); //Rev22.00 追加 by長野 2015/08/20

            //回転傾斜テーブル(回転)
            fraTiltAndRot_Rot.Visible = (CTSettings.scaninh.Data.tilt_and_rot == 0); //Rev22.00 追加 by長野 2015/08/20

            //ファントムフレーム
            fraPhantom.Visible = !CTSettings.detectorParam.Use_FlatPanel;

			//フィルタフレーム
            //変更2014/10/07hata_v19.51反映
            //fraFilter.Visible = (CTSettings.scaninh.Data.filter == 0);
            //v19.50 産業用CT用の電動フィルタをshutterfilter、マイクロCT用の電動フィルタをfilterで管理する。 by長野 2013/11/08
            if (CTSettings.scaninh.Data.shutterfilter == 0 | CTSettings.scaninh.Data.filter == 0)
            {
                frashutterfilter.Visible = true;

                //名称がないボタンは非表示にする
                for (int i = CTSettings.infdef.Data.xfilter_c.GetLowerBound(0); i <= CTSettings.infdef.Data.xfilter_c.GetUpperBound(0); i++)
                {
                    cmdFilter[i].Visible = (!string.IsNullOrEmpty(modLibrary.RemoveNull(CTSettings.infdef.Data.xfilter_c[i].GetString())) ? true : false);
                }
            }
            else
            {
                frashutterfilter.Visible = false;
            }

			//チルトフレーム
			fraTilt.Visible = (CTSettings.scaninh.Data.tilt == 0);

			//Ｘ線管フレーム
			//fraMultiTube.Visible = (.multi_tube = 0)
			//Rev23.10 条件を変更 by長野 2015/09/20
            fraMultiTube.Visible = (CTSettings.scaninh.Data.multi_tube == 0 && CTSettings.scaninh.Data.xray_remote == 1);

            //X線切替フレーム(外部制御可能な場合) //Rev23.10 追加 by長野 2015/09/20
            fraXrayChangeReset.Visible = (CTSettings.scaninh.Data.multi_tube == 0 && CTSettings.scaninh.Data.xray_remote == 0);

			//スライスライトボタン
			ctbtnSlight.Visible = (CTSettings.scaninh.Data.slice_light == 0);

			//I.I.移動フレーム               '追加 by 間々田 2009/07/21 メカ準備画面から移動
			fraIIMove.Visible = (CTSettings.scaninh.Data.ii_move == 0);

            //Rev23.20 検出器システム切り替えフレーム by長野 2015/12/21
            fra2and3CTgene.Visible = (CTSettings.scaninh.Data.ct_gene2and3 == 0);

            //変更2014/10/07hata_v19.51反映
            //I.I.リセットフレーム   'v16.01 追加 by 山影 10-02-12
            fraIIChangeReset.SetBounds(fraXrayRotate.Left, fraXrayRotate.Top, 0, 0, System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);
            
            //X線管回転フレームと同じ位置へ
            //fraIIChangeReset.Visible = CTSettings.HscOn;
            //Rev26.40 条件追加 by chouno 2019/02/12
            fraIIChangeReset.Visible = CTSettings.HscOn && CTSettings.iniValue.HSCSettingType == 0;

            //変更2014/10/07hata_v19.51反映
            //v17.20 追加 検出器リセットフレーム by 長野 2010/09/06
            fraDetChangeReset.SetBounds(fraXrayRotate.Left, fraXrayRotate.Top, 0, 0, System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);
            //X線管回転フレームと同じ位置へ
            //fraDetChangeReset.Visible = SecondDetOn
            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            //fraDetChangeReset.Visible = CTSettings.SecondDetOn | CTSettings.DetShiftOn; //検出器シフトのときもこれを使う 'v18.00変更 byやまおか 2011/02/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            fraDetChangeReset.Visible = CTSettings.SecondDetOn | CTSettings.DetShiftOn | CTSettings.W_ScanOn;
            //PkeFPDの場合は検出器電源ボタンを表示しない 'v17.44追加 byやまおか 2011/02/18
            ctbtnIIPower.Visible = !(CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);

            //Rev26.40 add by chouno 2019/02/24
            fraXrayAndHSC.Visible = CTSettings.iniValue.HSCModeType == 1;
            ctSliderVScroll1.Minimum = CTSettings.iniValue.XrayAndHSCMinPos + CTSettings.iniValue.XrayAndHSCMaxPos;//最大値分、オフセット
            ctSliderVScroll1.Maximum = CTSettings.iniValue.XrayAndHSCMaxPos + CTSettings.iniValue.XrayAndHSCMaxPos;//最大値分、オフセット
            cwnePos.Minimum = CTSettings.iniValue.XrayAndHSCMinPos;
            cwnePos.Maximum = CTSettings.iniValue.XrayAndHSCMaxPos;
            cwnePos.Value = modLibrary.CorrectInRange(modSeqComm.MySeq.stsXrayCameraUDPosition, cwnePos.Minimum, cwnePos.Maximum);
            if (txtUpDownPos.Visible) txtUpDownPos.BringToFront();
            txtUpDownPos.Text = modSeqComm.MySeq.stsXrayCameraUDPosition.ToString("0");
            ctSliderVScroll1.Value = modLibrary.CorrectInRange(cwnePos.Value + CTSettings.iniValue.XrayAndHSCMaxPos, ctSliderVScroll1.Minimum, ctSliderVScroll1.Maximum);
            ctSliderVScroll1.ArrowValue = ctSliderVScroll1.Value;

            if (modLibrary.InRange(modSeqComm.MySeq.stsXrayCameraUDPosition, cwnePos.Minimum, cwnePos.Maximum))
            {
                DispUpDownPointer((int)ctSliderVScroll1.Value);
            }

            //Rev26.40 dd by chouno 2019/02/17
            LimitControl();
		}


		//*************************************************************************************************
		//機　　能： Ｘ線管回転ステータスを返す関数
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v9.0  04/02/12 (SI4)間々田     新規作成
		//*************************************************************************************************
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
		//Private Function GetXrayRotStatus() As XrayRotStatus
		//
		//    With MySeq
		//        If .stsXrayCW Or .stsXrayCCW Then
		//            GetXrayRotStatus = XrayRotBusy      '動作中
		//        ElseIf .stsXrayRotErr Then
		//            GetXrayRotStatus = XrayRotError     '異常
		//        ElseIf .stsXrayCWLimit Then
		//            GetXrayRotStatus = XrayCWLimit      '正転限
		//        ElseIf .stsXrayCCWLimit Then
		//            GetXrayRotStatus = XrayCCWlimit     '逆転限
		//        Else
		//            GetXrayRotStatus = XrayRotNotBusy   '停止中
		//        End If
		//    End With
		//
		//End Function
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''


		//*************************************************************************************************
		//機　　能： シーケンサ関連更新処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void myMechaControl_SeqCommChanged(object sender, EventArgs e)
		{
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//        If IsHSCmode Or IsDet2mode Then 'v16.01 追加 by 山影 10-02-18　→　v17.20 条件に検出器切替を追加
			//            '高速I.I.電源
			//            ctbtnIIPower.Value = .stsTVIIPower
			//        Else
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
			//I.I.電源                                               '追加 by 間々田 2009/07/21
			ctbtnIIPower.Value = modSeqComm.MySeq.stsIIPower;
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//        End If
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			//FPDの場合、透視画像表示中はFPD電源をOFFできなくする    '追加 by 間々田 2009/07/21
            if (CTSettings.detectorParam.Use_FlatPanel)
			{
				if (ctbtnIIPower.Value) ctbtnIIPower.Enabled = !frmTransImage.Instance.CaptureOn;
			}

			frmMechaControl.Instance.SetLimitStatus(cwbtnIIMove0, "Up", modSeqComm.MySeq.stsIIFLimit);		//I.I.移動前進限 '追加 by 間々田 2009/07/21 メカ準備画面から移動
			frmMechaControl.Instance.SetLimitStatus(cwbtnIIMove1, "Down", modSeqComm.MySeq.stsIIBLimit);	//I.I.移動後退限 '追加 by 間々田 2009/07/21 メカ準備画面から移動

			//スライスライト
			if (CTSettings.scaninh.Data.slice_light == 0)
			{
				ctbtnSlight.Value = modSeqComm.MySeq.stsSLight;
			}

            //微調テーブル add Rev26.00 ラージテーブルの着脱有無を優先 by chouno 2017/10/16
            //微調リミットならボタン背景色を変える
            //frmMechaControl.Instance.SetLimitStatus(cwbtnFineTableX0, "Right", modSeqComm.MySeq.stsRotLargeTable == true);
            //frmMechaControl.Instance.SetLimitStatus(cwbtnFineTableX1, "Left", modSeqComm.MySeq.stsRotLargeTable == true);
            //frmMechaControl.Instance.SetLimitStatus(cwbtnFineTableY0, "Down", modSeqComm.MySeq.stsRotLargeTable == true);
            //frmMechaControl.Instance.SetLimitStatus(cwbtnFineTableY1, "Up", modSeqComm.MySeq.stsRotLargeTable == true);
            //微調テーブルタイプを見るように変更 by chouno 2019/02/06
            //Rev26.40 別の場所に移動 by chouno 2019/02/17
            //frmMechaControl.Instance.SetLimitStatus(cwbtnFineTableX0, "Right", (modSeqComm.MySeq.stsRotLargeTable == true && CTSettings.t20kinf.Data.ftable_type == 0));
            //frmMechaControl.Instance.SetLimitStatus(cwbtnFineTableX1, "Left", (modSeqComm.MySeq.stsRotLargeTable == true && CTSettings.t20kinf.Data.ftable_type == 0));
            //frmMechaControl.Instance.SetLimitStatus(cwbtnFineTableY0, "Down", (modSeqComm.MySeq.stsRotLargeTable == true && CTSettings.t20kinf.Data.ftable_type == 0));
            //frmMechaControl.Instance.SetLimitStatus(cwbtnFineTableY1, "Up", (modSeqComm.MySeq.stsRotLargeTable == true && CTSettings.t20kinf.Data.ftable_type == 0));

			//フィルタ
			//v19.50 条件変更 by長野 2013/11/08
            //変更2014/10/07hata_v19.51反映
			//if (CTSettings.scaninh.Data.filter == 0)
			if (CTSettings.scaninh.Data.shutterfilter == 0 | CTSettings.scaninh.Data.filter == 0)
			{
				modLibrary.SetCmdButton(cmdFilter, modSeqComm.GetFilterIndex(), ControlEnabled: true);
			}

			//チルト
			if (CTSettings.scaninh.Data.tilt == 0)
			{
				cmdTilt0.BackColor = (modSeqComm.MySeq.stsTiltCwLimit ? Color.Lime : SystemColors.Control);		//ＣＷ限
				cmdTilt1.BackColor = (modSeqComm.MySeq.stsTiltOrigin ? Color.Lime : SystemColors.Control);		//原点
				cmdTilt2.BackColor = (modSeqComm.MySeq.stsTiltCCwLimit ? Color.Lime : SystemColors.Control);	//ＣＣＷ限
			}

            #region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//        If scaninh.xray_rotate = 0 Then
			//
			//            'Ｘ線管回転ステータス
			//            Select Case GetXrayRotStatus()
			//                Case XrayRotNotBusy
			//                    stsXrayRot.Status = GC_STS_STOP                                     '停止中
			//                Case XrayRotBusy
			//                    stsXrayRot.Status = GC_STS_BUSY                                     '動作中
			//                Case XrayCWLimit
			//                    stsXrayRot.Status = LoadResString(12363)                            '正転限
			//                Case XrayCCWlimit
			//                    stsXrayRot.Status = LoadResString(12364)                            '逆転限
			//                Case XrayRotError
			//                    stsXrayRot.Status = GC_Xray_Error                                   '異常
			//            End Select
			//
			//        End If
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
	        #endregion 

            //Rev26.40 add by chouno 2019/02/24
            txtUpDownPos.Text = modSeqComm.MySeq.stsXrayCameraUDPosition.ToString("0");
            if (modLibrary.InRange(modSeqComm.MySeq.stsXrayCameraUDPosition, cwnePos.Minimum, cwnePos.Maximum))
            {
                DispUpDownPointer();
            }

            LimitControl();

            if (modHighSpeedCamera.IsIIChanging)
            {
                stsIIChangeReset.Status = StringTable.GC_STS_BUSY;					//動作中
            }
            else if (modHighSpeedCamera.IsOKIIPos)
            {
                stsIIChangeReset.Status = StringTable.GC_STS_STANDBY_OK;			//準備完
            }
            else
            {
                stsIIChangeReset.Status = StringTable.GC_STS_STANDBY_NG2;			//準備未完
            }

            //変更2014/10/07hata_v19.51反映
			//v17.20 追加 検出器リセットステータス by 長野 10-09-06
			//If SecondDetOn Then
			//検出器シフトのときもこれを使う 'v18.00変更 byやまおか 2011/02/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            //if (CTSettings.SecondDetOn |CTSettings.DetShiftOn) 
            if (CTSettings.SecondDetOn | CTSettings.DetShiftOn | CTSettings.W_ScanOn)
            {
				if (mod2ndDetctor.IsSwtichingDet) 
                {
					stsDetChangeReset.Status = StringTable.GC_STS_BUSY;                 //動作中
				} 
                else if (mod2ndDetctor.IsOKDetPos) 
                {
					stsDetChangeReset.Status = StringTable.GC_STS_STANDBY_OK;			//準備完了
				} 
                else 
                {
					stsDetChangeReset.Status = StringTable.GC_STS_STANDBY_NG2;			//準備未完
				}
			}

            //Rev23.10 追加 by長野 2015/09/20
            if (CTSettings.ChangeXrayOn)
            {
                if (mod2ndXray.IsChangingXray)
                {
                    stsXrayChangeReset.Status = StringTable.GC_STS_BUSY;                 //動作中
                }
                else if (mod2ndXray.IsOKXrayPos)
                {
                    stsXrayChangeReset.Status = StringTable.GC_STS_STANDBY_OK;			//準備完了
                }
                else
                {
                    stsXrayChangeReset.Status = StringTable.GC_STS_STANDBY_NG2;			//準備未完
                }
            }
            //Rev23.20 追加 by長野 2015/12/17
            if (CTSettings.scaninh.Data.ct_gene2and3 == 0)
            {
                if (modSeqComm.MySeq.stsFDSystemBusy)
                {
                    stsCTgene2and3.Status = StringTable.GC_STS_BUSY;                 //動作中
                }
                else if (modSeqComm.MySeq.stsFDSystemPos)
                {
                    stsCTgene2and3.Status = StringTable.GC_STS_STANDBY_OK;			//準備完了
                }
                else
                {
                    stsCTgene2and3.Status = StringTable.GC_STS_STANDBY_NG2;			//準備未完
                }
            }
        }

        private void LimitControl()
        {
            if (myIImode == modHighSpeedCamera.IIMode)
            {
                return;
            }

            myIImode = modHighSpeedCamera.IIMode;

            //I.I.リセットステータス v16.01 追加 by 山影 10-02-10
            if (CTSettings.HscOn)
            {
                //Rev26.40 CT、高速透視、落下試験器で動作許可変更 by chouno 2019/02/12
                if (modHighSpeedCamera.IIMode == modHighSpeedCamera.IIModeConstants.IIMode_CT || modHighSpeedCamera.IIMode == modHighSpeedCamera.IIModeConstants.IIMode_HSC)
                {
                    foreach (Control control in Controls)
                    {
                        if (control is GroupBox)
                        {
                            control.Enabled = true;
                        }
                    }
                    cmdAllReset.Enabled = true;
                    ctbtnIIPower.Enabled = true;
                    ctbtnPhmOn.Enabled = true;
                    ctbtnSlight.Enabled = true;
                    fraXrayAndHSC.Enabled = false;//Rev26.40 add by chouno 2019/02/24
                }
                else if (modHighSpeedCamera.IIMode == modHighSpeedCamera.IIModeConstants.IIMode_DROP_TEST)
                {
                    foreach (Control control in Controls)
                    {
                        if (control is GroupBox)
                        {
                            control.Enabled = false;
                        }
                    }
                    //FDDだけ許可
                    fraIIMove.Enabled = true;
                    cmdAllReset.Enabled = false;
                    ctbtnIIPower.Enabled = false;
                    ctbtnPhmOn.Enabled = false;
                    ctbtnSlight.Enabled = false;
                    fraXrayAndHSC.Enabled = true;//Rev26.40 add by chouno 2019/02/24
                }
                else//位置不定は全て禁止
                {
                    foreach (Control control in Controls)
                    {
                        if (control is GroupBox)
                        {
                            control.Enabled = false;
                        }
                    }
                    cmdAllReset.Enabled = false;
                    ctbtnIIPower.Enabled = false;
                    ctbtnPhmOn.Enabled = false;
                    ctbtnSlight.Enabled = false;
                    fraXrayAndHSC.Enabled = false;//Rev26.40 add by chouno 2019/02/24
                }
            }

        }

		//*************************************************************************************************
		//機　　能： メカ関連更新処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void myMechaControl_MecainfChanged(object sender, EventArgs e)
		{
			//テーブル回転ステータス
			//if (CTSettings.mecainf.Data.rot_ready == 1)     //'v18.00変更 byやまおか 2011/03/08
			if ((CTSettings.mecainf.Data.rot_ready == 1) & (modMechaControl.Flg_StartupRotReset))
			{
				stsRotate.Status = StringTable.GC_STS_STANDBY_OK;	//準備完了
			}
			else if (CTSettings.mecainf.Data.rot_busy == 1)
			{
				stsRotate.Status = StringTable.GC_STS_BUSY;			//動作中
			}
			else if (CTSettings.mecainf.Data.rot_error == 0)
			{
				stsRotate.Status = StringTable.GC_STS_STANDBY_NG2;	//準備未完
			}
			else
			{
				stsRotate.Status = StringTable.GC_Xray_Error;		//異常
			}


			//テーブル昇降ステータス
			//v17.20 上下限のコモンを追加したのでコメントアウト by 長野 2010/09/20
			//        If .ud_limit = 1 Then
			if (CTSettings.mecainf.Data.ud_u_limit == 1 || CTSettings.mecainf.Data.ud_l_limit == 1)
			{
				stsUpDown.Status = StringTable.GC_STS_MovementLimit;	//動作限
			}
            //変更2014/10/07hata_v19.51反映
            //v19.51 条件変更 by長野 2014/02/27
			//else if (CTSettings.mecainf.Data.ud_ready == 1)
			else if ((CTSettings.mecainf.Data.ud_ready == 1) & (modMechaControl.Flg_StartupUpDownReset))
			{
				stsUpDown.Status = StringTable.GC_STS_STANDBY_OK;		//準備完了
			}
			else if (CTSettings.mecainf.Data.ud_busy == 1)
			{
				stsUpDown.Status = StringTable.GC_STS_BUSY;				//動作中
			}
			else if (CTSettings.mecainf.Data.ud_error == 0)
			{
				stsUpDown.Status = StringTable.GC_STS_STANDBY_NG2;		//準備未完
			}
			else
			{
				stsUpDown.Status = StringTable.GC_Xray_Error;			//異常
			}


			//ファントムステータス
			if (CTSettings.mecainf.Data.phm_busy == 1)
			{
				stsPhantom.Status = StringTable.GC_STS_BUSY;			//動作中
			}
			else if (CTSettings.mecainf.Data.phm_ready == 1)
			{
				stsPhantom.Status = StringTable.GC_STS_STANDBY_OK;		//準備完了
			}
			else if (CTSettings.mecainf.Data.phm_error == 1)
			{
				stsPhantom.Status = StringTable.GC_Xray_Error;			//異常
			}
			else
			{
				stsPhantom.Status = StringTable.GC_STS_STANDBY_NG2;		//準備未完
			}

			//ファントム有無
			ctbtnPhmOn.Value = (CTSettings.mecainf.Data.phm_onoff == 1);


			//微調Ｘ軸ステータス：従来のＹ軸ステータス
            if (CTSettings.mecainf.Data.ystg_limit == 1)
			{
				stsFineTable0.Status = StringTable.GC_STS_MovementLimit;	//動作限
			}
			else if (CTSettings.mecainf.Data.ystg_ready == 1)
			{
				stsFineTable0.Status = StringTable.GC_STS_STANDBY_OK;		//準備完了
			}
			else if (CTSettings.mecainf.Data.ystg_busy == 1)
			{
				stsFineTable0.Status = StringTable.GC_STS_BUSY;				//動作中
			}
			else if (CTSettings.mecainf.Data.ystg_error == 0)
			{
				stsFineTable0.Status = StringTable.GC_STS_STANDBY_NG2;		//準備未完
			}
			else
			{
				stsFineTable0.Status = StringTable.GC_Xray_Error;			//異常
			}

			//'微調リミットならボタン背景色を変える
			//frmMechaControl.SetLimitStatus cwbtnFineTableY(0), "Down", (.ystg_pos < 0) And (.ystg_limit = 1)    'v15.10追加 byやまおか 2009/12/01
			//frmMechaControl.SetLimitStatus cwbtnFineTableY(1), "Up", (.ystg_pos > 0) And (.ystg_limit = 1)      'v15.10追加 byやまおか 2009/12/01

            //Rev26.00 ラージテーブルの着脱有無を優先 by chouno 2017/10/16
            //if (modSeqComm.MySeq.stsRotLargeTable == false)
            //Rev26.20 微調テーブルタイプも見るように変更 by chouno 2019/02/06
            if (modSeqComm.MySeq.stsRotLargeTable == false || CTSettings.t20kinf.Data.ftable_type == 1)
            {
                //微調リミットならボタン背景色を変える                   'v17.47/v17.53変更 2011/03/09 by 間々田 上下限動作限専用のコモン追加による
                frmMechaControl.Instance.SetLimitStatus(cwbtnFineTableY0, "Down", (CTSettings.mecainf.Data.ystg_l_limit == 1));
                frmMechaControl.Instance.SetLimitStatus(cwbtnFineTableY1, "Up", (CTSettings.mecainf.Data.ystg_u_limit == 1));
            }
            else
            {
                //Rev26.40 change by chouno 2019/02/17
                frmMechaControl.Instance.SetLimitStatus(cwbtnFineTableY0, "Down", (modSeqComm.MySeq.stsRotLargeTable == true && CTSettings.t20kinf.Data.ftable_type == 0) ||(CTSettings.mecainf.Data.ystg_l_limit == 1));
                frmMechaControl.Instance.SetLimitStatus(cwbtnFineTableY1, "Up", (modSeqComm.MySeq.stsRotLargeTable == true && CTSettings.t20kinf.Data.ftable_type == 0) || (CTSettings.mecainf.Data.ystg_l_limit == 1));
            }

			//微調Ｙ軸ステータス：従来のＸ軸ステータス
            if (CTSettings.mecainf.Data.xstg_limit == 1)
			{
				stsFineTable1.Status = StringTable.GC_STS_MovementLimit;	//動作限
			}
			else if (CTSettings.mecainf.Data.xstg_ready == 1)
			{
				stsFineTable1.Status = StringTable.GC_STS_STANDBY_OK;		//準備完了
			}
			else if (CTSettings.mecainf.Data.xstg_busy == 1)
			{
				stsFineTable1.Status = StringTable.GC_STS_BUSY;				//動作中
			}
			else if (CTSettings.mecainf.Data.xstg_error == 0)
			{
				stsFineTable1.Status = StringTable.GC_STS_STANDBY_NG2;		//準備未完
			}
			else
			{
				stsFineTable1.Status = StringTable.GC_Xray_Error;			//異常
			}

			//'微調リミットならボタン背景色を変える
			//frmMechaControl.SetLimitStatus cwbtnFineTableX(0), "Right", (.xstg_pos > 0) And (.xstg_limit = 1)   'v15.10追加 byやまおか 2009/12/01
			//frmMechaControl.SetLimitStatus cwbtnFineTableX(1), "Left", (.xstg_pos < 0) And (.xstg_limit = 1)    'v15.10追加 byやまおか 2009/12/01
            
            //Rev26.00 ラージテーブルの着脱有無を優先 by chouno 2017/10/16
            //if (modSeqComm.MySeq.stsRotLargeTable == false)
            //Rev26.20 微調テーブルタイプも見るようにする by chouno 2019/02/06
            if (modSeqComm.MySeq.stsRotLargeTable == false || CTSettings.t20kinf.Data.ftable_type == 1)
            {
                //微調リミットならボタン背景色を変える
                frmMechaControl.Instance.SetLimitStatus(cwbtnFineTableX0, "Right", (CTSettings.mecainf.Data.xstg_u_limit == 1));
                frmMechaControl.Instance.SetLimitStatus(cwbtnFineTableX1, "Left", (CTSettings.mecainf.Data.xstg_l_limit == 1));
            }
            else
            {
                //Rev26.40 change by chouno 2019/02/17
                frmMechaControl.Instance.SetLimitStatus(cwbtnFineTableX0, "Right", (modSeqComm.MySeq.stsRotLargeTable == true && CTSettings.t20kinf.Data.ftable_type == 0) ||(CTSettings.mecainf.Data.xstg_u_limit == 1));
                frmMechaControl.Instance.SetLimitStatus(cwbtnFineTableX1, "Left", (modSeqComm.MySeq.stsRotLargeTable == true && CTSettings.t20kinf.Data.ftable_type == 0) || (CTSettings.mecainf.Data.xstg_l_limit == 1));
            }

            //回転傾斜テーブル(傾斜)のステータス
            if (CTSettings.scaninh.Data.tilt_and_rot == 0 && CTSettings.mecainf.Data.tiltrot_table == 0)
            {
                stsTiltAndRot0.Status = StringTable.GC_STS_STANDBY_NG2;
            }
            else if (CTSettings.mecainf.Data.tilt_limit == 1)
            //if (CTSettings.mecainf.Data.tilt_limit == 1)
            {
                stsTiltAndRot0.Status = StringTable.GC_STS_MovementLimit;	//動作限
            }
            else if (CTSettings.mecainf.Data.tilt_ready == 1)
            {
                stsTiltAndRot0.Status = StringTable.GC_STS_STANDBY_OK;		//準備完了
            }
            else if (CTSettings.mecainf.Data.tilt_busy == 1)
            {
                stsTiltAndRot0.Status = StringTable.GC_STS_BUSY;				//動作中
            }
            else if (CTSettings.mecainf.Data.tilt_error == 0)
            {
                stsTiltAndRot0.Status = StringTable.GC_STS_STANDBY_NG2;		//準備未完
            }
            else
            {
                stsTiltAndRot0.Status = StringTable.GC_Xray_Error;			//異常
            }

            //回転傾斜テーブル(傾斜)のステータス
            if (CTSettings.scaninh.Data.tilt_and_rot == 0 && CTSettings.mecainf.Data.tiltrot_table == 0)
            {
                stsTiltAndRot1.Status = StringTable.GC_STS_STANDBY_NG2;
            }
            else if (CTSettings.mecainf.Data.tiltrot_limit == 1)
            {
                stsTiltAndRot1.Status = StringTable.GC_STS_MovementLimit;	//動作限
            }
            else if (CTSettings.mecainf.Data.tiltrot_ready == 1)
            {
                stsTiltAndRot1.Status = StringTable.GC_STS_STANDBY_OK;		//準備完了
            }
            else if (CTSettings.mecainf.Data.tiltrot_busy == 1)
            {
                stsTiltAndRot1.Status = StringTable.GC_STS_BUSY;				//動作中
            }
            else if (CTSettings.mecainf.Data.tiltrot_error == 0)
            {
                stsTiltAndRot1.Status = StringTable.GC_STS_STANDBY_NG2;		//準備未完
            }
            else
            {
                stsTiltAndRot1.Status = StringTable.GC_Xray_Error;			//異常
            }
        }
		//*************************************************************************************************
		//機　　能： I.I.切替フレーム：「リセット」ボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v16.01 2010/02/02 (検SS)山影   新規作成
		//*************************************************************************************************
        private void cmdIIChangeReset_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            //int error_sts = 0;

            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if (!modMechaControl.IsOkMechaMove())   
                return;
            
            //動作可否判定
            if (!modHighSpeedCamera.IsOKIIMove())
                return;

            //画面操作禁止
            this.Enabled = false;
             System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            //タッチパネル操作禁止
            modSeqComm.SeqBitWrite("PanelInhibit", true);

            //高速カメラ電源オフ
            modHighSpeedCamera.HSCameraPower(modCT30K.OnOffStatusConstants.OffStatus);

            //高速I.I.電源オフ
            modHighSpeedCamera.HSIIPower(modCT30K.OnOffStatusConstants.OffStatus);

            //原点復帰運転
            modSeqComm.SeqBitWrite("IIChangeReset", true);

            //監視タイマー起動
            TimerCount = 0;
            tmrWait.Enabled = true;

        }
        
		//*************************************************************************************************
		//機　　能： 検出器切替フレーム：「リセット」ボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v17.20 2010/09/06 (検S1)長野   新規作成
		//*************************************************************************************************
        private void cmdDetChangeReset_Click(System.Object eventSender, System.EventArgs eventArgs)
        {

            //変更2014/10/07hata_v19.51反映
            //    '運転準備ボタンが押されていなければ無効
            //    If Not MySeq.stsRunReadySW Then
            //
            //        'MsgBox "運転準備が未完のため検出器切替軸がリセットできません。" & vbCrLf & "運転準備スイッチを押して運転準備完了にしてください。", vbCritical
            //        MsgBox LoadResString(20055) & vbCrLf & LoadResString(8059), vbCritical 'ストリングテーブル化 'v17.60 by長野 2011/05/22
            //
            //        Exit Sub
            //    End If
            //
            //    'v17.40 メンテナンスのときは検査室扉が閉まっていることをチェックしないように変更 by 長野 2010/10/21
            //    'v17.40 稲葉さんの改造待ち
            //    If Not MySeq.stsDoorPermit Then
            //
            //        'v17.20 検査室の扉が閉じていなければ無効 by 長野 2010/09/20
            //
            //        If (frmCTMenu.DoorStatus = DoorOpened) Then
            //
            //        'MsgBox "Ｘ線検査室の扉が開いているため検出器切替軸がリセットができません。" & vbCrLf & "Ｘ線検査室の扉を閉めてから、再度操作を行なって下さい。", vbCritical
            //        MsgBox LoadResString(20056) & vbCrLf & LoadResString(8022), vbCritical 'ストリングテーブル化 'v17.60 by長野 2011/05/22
            //
            //        Exit Sub
            //
            //        End If
            //
            //    End If

            //機構部動作が可能かチェック（上記を関数化）
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if ((!modMechaControl.IsOkMechaMove()))
                return;

            //v19.50 コメントアウト IsOkMechaMoveに含まれている 長野 2014/02/15
            //If Not MySeq.stsRunReadySW Then
            //    Exit Sub
            //End If

            //動作可否判定
            //If Not IsSwitchDet() Then Exit Sub
            //v19.50 エラーメッセージを表示するようにした by長野 2014/02/24
            if (!mod2ndDetctor.IsSwitchDet())
            {
                //Interaction.MsgBox(CT30K.My.Resources.str17502, MsgBoxStyle.OkOnly);
                MessageBox.Show(CTResources.LoadResString(17502), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.None);
            }

            //v19.50 条件追加 シフトスキャン時はやらなくていいチェック by長野　2014/02/15
            //If Not IsAllCloseFrm() Then Exit Sub
            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            //if (!CTSettings.DetShiftOn)
            if (!(CTSettings.DetShiftOn || CTSettings.W_ScanOn))
            {
                if (!mod2ndDetctor.IsAllCloseFrm)
                    return;
            }

            //画面操作禁止
            this.Enabled = false;
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            //タッチパネル操作禁止
            modSeqComm.SeqBitWrite("PanelInhibit", true);

            //Rev23.10 X線切替ONの場合を追加 2015/10/23 by長野
            if (CTSettings.ChangeXrayOn)
            {
                if (mod2ndXray.XrayMode == mod2ndXray.XrayModeConstants.XrayMode_Xray1)
                {
                    if (modSeqComm.MySeq.stsMicroFPDShiftPos == true)
                    {
                        //原点復帰運転
                        modSeqComm.SeqBitWrite("MicroFPDSet", true);
                    }
                }
                else if (mod2ndXray.XrayMode == mod2ndXray.XrayModeConstants.XrayMode_Xray2)
                {
                    if (modSeqComm.MySeq.stsNanoFPDShiftPos == true)
                    {
                        //原点復帰運転
                        modSeqComm.SeqBitWrite("NanoFPDSet", true);
                    }
                }
            }
            else
            {
                //原点復帰運転
                modSeqComm.SeqBitWrite("IIChangeReset", true);
            }
            //監視タイマー起動
            TimerCount = 0;
            tmrWaitDet.Enabled = true;

        }

		//*************************************************************************************************
		//機　　能： テーブル回転フレーム：「リセット」ボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void cmdRotReset_Click(object sender, EventArgs e)
		{
			int error_sts = 0;

            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if ((!modMechaControl.IsOkMechaMove()))
                return;
            
            //FCD のチェック  'v9.7追加 by added 間々田 2004/12/10
			if (!modSeqComm.CheckFCD(ScanCorrect.GVal_Fcd)) return;

			//動作が禁止されている場合 v9.6 追加 by 間々田 2004/10/12
			if ((CTSettings.scaninh.Data.seqcomm == 0) && (CTSettings.scaninh.Data.table_restriction == 0))
			{
				if (!modSeqComm.MySeq.stsMechaPermit) return;
			}

            //Rev26.00 add by chouno 2017/03/13
            if (modMechaControl.IsOkMechaMoveWithLargeTable() == false)
            {
                return;
            }

            //Rev20.00 干渉領域内ではメッセージを出す by長野 2015/02/06
            //if (ScanCorrect.GVal_Fcd < CTSettings.GVal_FcdLimit)
            //Rev25.10 change by chouno 2017/09/11
            if (ScanCorrect.GVal_Fcd < modSeqComm.GetFCDLimit())
            {
                //確認メッセージ表示：
                //   テーブルの回転をリセットしても、ワークが干渉しないことを確認して下さい。
                //   よろしければＯＫをクリックして下さい。
                DialogResult result = MessageBox.Show(CTResources.LoadResString(8070) + "\r" + "\r"
                                                    + CTResources.LoadResString(StringTable.IDS_ClickOK),
                Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                
                if (result == DialogResult.Cancel) return;
            }

			//エラー時の扱い
			try
			{
				//画面操作禁止
				this.Enabled = false;
				cmdRotReset.Enabled = false;
				Cursor.Current = Cursors.WaitCursor;

				//メカエラーリセット
				error_sts = modMechaControl.Mechaerror_reset(modDeclare.hDevID1);
                if (error_sts != 0) throw new Exception();

				//回転軸初期化
				error_sts = modMechaControl.RotateInit(modDeclare.hDevID1);
                if (error_sts != 0) throw new Exception();

				//回転軸原点復帰
				error_sts = modMechaControl.MecaRotateOrigin(true);
                if (error_sts != 0) throw new Exception();


				//エンコーダの０リセットを行う                    'v7.0 append by 間々田 2003/09/09
				modSeqComm.SeqBitWrite("RotOriginOK", true);

            }
            catch
            {
            }

            finally
			{
                //画面操作禁止解除
				this.Enabled = true;
				cmdRotReset.Enabled = true;
				Cursor.Current = Cursors.Default;

				//エラーメッセージ
				if (error_sts != 0) modCT30K.ErrMessage(error_sts, Icon: MessageBoxIcon.Error);
			}
       
        }


		//*************************************************************************************************
		//機　　能： テーブル昇降フレーム：リセットボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void cmdUpdReset_Click(object sender, EventArgs e)
		{
			int error_sts = 0;

            //Rev25.10 PLCステータスを明示的に更新 //2017/09/11
            //干渉にかかわる処理なので、念のためseqcommのステータス更新
            modSeqComm.MySeq.StatusRead();
            modCT30K.PauseForDoEvents(0.5f);

            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if ((!modMechaControl.IsOkMechaMove()))
                return;
            
            //Rev26.00 add by chouno 2017/03/13
            if (modMechaControl.IsOkMechaMoveWithLargeTable() == false)
            {
                return;
            }

            //変更2014/10/07hata_v19.51反映
            //v19.51 条件追加 X線・検出器昇降の場合は上に原点があるためfcd制限でとめない by長野 2014/02/27
			if (CTSettings.t20kinf.Data.ud_type == 0) {

                //if (frmMechaControl.Instance.FCDWithOffset < CTSettings.GVal_FcdLimit)
                //Rev25.10 change by chouno 2017/09/11
                if (frmMechaControl.Instance.FCDWithOffset < modSeqComm.GetFCDLimit())

                {
				    //メッセージ表示：テーブルがＸ線管に近過ぎるため、リセットを中止します。
				    MessageBox.Show(CTResources.LoadResString(9470), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				    return;
			    }

            }

            //変更2014/10/07hata_v19.51反映
            //確認メッセージ表示：（コモンによってメッセージを切り替える）
			//v18.00追加 byやまおか 2011/03/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev23.10 計測CTモードの場合は、昇降原点は下のためメッセージはなし by長野 2015/10/22
			//if ((CTSettings.scaninh.Data.avmode == 0))
            if ((CTSettings.scaninh.Data.avmode == 0) || (CTSettings.scaninh.Data.cm_mode == 0)) 
            {
				//産業用CTモードの場合はメッセージなし
			} 
            else 
            {

                //確認メッセージ表示：（コモンによってメッセージを切り替える）
                //Rev21.00 リソース変更 9472→22010
                //   リソース22010:X線管・検出器が上昇しても、サンプル等にぶつからないことを確認してください。
			    //   リソース9510:試料テーブルが上昇しても、X線管にぶつからない位置にいることを確認して下さい。
			    //   リソース9905:よろしければＯＫをクリックしてください。

                //DialogResult result = MessageBox.Show(CTResources.LoadResString(CTSettings.t20kinf.Data.ud_type == 1 ? 9472 : 9510) + "\r" + "\r"
                //Rev21.00 変更 by長野 2015/03/18
                DialogResult result = MessageBox.Show(CTResources.LoadResString(CTSettings.t20kinf.Data.ud_type == 1 ? 22010 : 9510) + "\r" + "\r"
                                                    + CTResources.LoadResString(StringTable.IDS_ClickOK),
                                                    Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Cancel) return;

                //変更2014/10/07hata_v19.51反映
                //v19.51 X線・検出器昇降タイプは原点上なのでここで止めなくてよい by長野 2014/02/27
                if (CTSettings.t20kinf.Data.ud_type == 0)
                {
                    //動作が禁止されている場合 v9.6 追加 by 間々田 2004/10/12
                    if ((CTSettings.scaninh.Data.seqcomm == 0) && (CTSettings.scaninh.Data.table_restriction == 0))
                    {
                        if (!modSeqComm.MySeq.stsMechaPermit) return;
                    }
                }
            }

			//画面操作禁止
			this.Enabled = false;
			cmdUpdReset.Enabled = false;
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				//メカエラーリセット
				error_sts = modMechaControl.Mechaerror_reset(modDeclare.hDevID1);
				if (error_sts != 0) throw new Exception();

				//昇降軸原点復帰
				error_sts = modMechaControl.MecaUdOrigin();
			}
            catch
            {
            }
			finally
			{
				//画面操作禁止解除
				this.Enabled = true;
				cmdUpdReset.Enabled = true;
				Cursor.Current = Cursors.Default;

				//エラーメッセージ
				if (error_sts != 0) modCT30K.ErrMessage(error_sts, Icon: MessageBoxIcon.Error);
			}
		}


		//*************************************************************************************************
		//機　　能： テーブルＸ軸・Ｙ軸リセットボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 各コマンドボタンのタグに動作指令があらかじめ埋め込まれている
		//           cmdTableReset(0):YOrigin   Ｘ軸原点復帰
		//           cmdTableReset(1):XOrigin   Ｙ軸原点復帰
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void cmdTableReset_Click(object sender, EventArgs e)
		{
			if (sender as Button == null) return;
			int Index = Array.IndexOf(cmdTableReset, sender);
			if (Index < 0) return;

            //Rev26.00 add by chouno 2017/03/13
            if (modMechaControl.IsOkMechaMoveWithLargeTable() == false)
            {
                return;
            }

            //変更2014/10/07hata_v19.51反映
            ////運転準備ボタンが押されていなければ無効
            //if (!modSeqComm.MySeq.stsRunReadySW)
            //{
            //    //MsgBox "運転準備が未完のため試料テーブルY軸がリセットできません。" & vbCrLf & "運転準備スイッチを押して運転準備完了にしてください。", vbCritical
            //    MessageBox.Show(CTResources.LoadResString(20057) + "\r\n" + CTResources.LoadResString(8059),
            //                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);		//v17.60 ストリングテーブル化 by長野 2011/05/25
            //    return;
            //}

            ////v17.61 検査室扉が開いている場合の処理を追加 by 長野 2011/09/12
            //if (!modSeqComm.MySeq.stsDoorPermit)
            //{
            //    //    v17.20 検査室の扉が閉じていなければ無効 by 長野 2010/09/20
            //    //インターロック用
            //    if (frmCTMenu.Instance.DoorStatus == frmCTMenu.DoorStatusConstants.DoorOpened)
            //    {
            //        //MsgBox "X線検査室の扉が開いているため試料テーブルを移動することができません｡" & vbCrLf & "X線検査室の扉を閉めてから､再度操作を行なって下さい｡", vbCritical
            //        MessageBox.Show(CTResources.LoadResString(20037) + "\r\n" + CTResources.LoadResString(20034),
            //                        Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);		//ストリングテーブル化 'v17.60 by 長野 2011/05/22
            //        return;
            //    }
            //}

			//v17.20 検査室の扉が閉じていなければ無効 by 長野 2010/09/20
			//If Not (frmCTMenu.DoorStatus = Doorclosed) Then 'インターロック用
			//    If Not (frmCTMenu.DoorStatus = DoorLocked) Then '電磁ロック用
			//
			//            MsgBox LoadResString(17503), vbExclamation
			//            Exit Sub
			//    End If

            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック（上記を関数化）
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if ((!modMechaControl.IsOkMechaMove()))
                return;

            //Rev20.00 追加 by長野 2015/02/06
            //FCD のチェック  'v9.7追加 by added 間々田 2004/12/10
            if (!modSeqComm.CheckFCD(ScanCorrect.GVal_Fcd)) return;

            //マウスポインタを砂時計にする
            Cursor.Current = Cursors.WaitCursor;

            //フォームを使用不可にする
            this.Enabled = false;

            //タッチパネル操作禁止
            modSeqComm.SeqBitWrite("PanelInhibit", true);

            //原点復帰
            modSeqComm.SeqBitWrite(Convert.ToString(cmdTableReset[Index].Tag), true);

            //追加2014/10/07hata_v19.51反映
            //v19.50 XとYどちらの軸をリセットするか記憶させる by長野 2013/12/17
            //ResetAxis = Index;

            //Rev23.40 変更 by長野 2016/06/19
            switch (Index)
            {
                case 0:
                    ResetXAxis = true;
                    break;

                case 1:
                    ResetYAxis = true;
                    break;
            }

            modCT30K.PauseForDoEvents(0.5F);            //v18.00追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //監視タイマー起動
            TimerCount = 0;
            tmrWait.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： Ｘ線管回転フレーム内「リセット」ボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v9.0  2004/02/10 (SI4)間々田      新規作成
		//*************************************************************************************************
        //変更2014/10/07hata_v19.51反映
        private void cmdXrayRotReset_Click(System.Object eventSender, System.EventArgs eventArgs)
        {

            int error_sts = 0;

            //機構部動作が可能かチェック
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if ((!modMechaControl.IsOkMechaMove()))
                return;

            try
            {

                //画面操作禁止
                this.Enabled = false;
                cmdXrayRotReset.Enabled = false;
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                //ステータス表示：動作中
                stsXrayRot.Status = StringTable.GC_STS_BUSY;

                //メカエラーリセット
                error_sts = modMechaControl.Mechaerror_reset(modDeclare.hDevID1);
                if (error_sts != 0)
                    goto ExitHandler;

                //原点復帰
                error_sts = modMechaControl.PioOutBit("XRAYROTORG", 1);
                modCT30K.PauseForDoEvents(0.2F);                //added by 山本　2004-6-2
                error_sts = modMechaControl.PioOutBit("XRAYROTORG", 0); //added by 山本　2004-6-2

            }
            finally
            { 
            }
    
        ExitHandler:
            
            //画面操作禁止解除
            this.Enabled = true;
            cmdXrayRotReset.Enabled = true;
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

            //エラーメッセージ
            if (error_sts != 0) 
                modCT30K.ErrMessage(error_sts, Icon: MessageBoxIcon.Error);
            

        }

		//*************************************************************************************************
		//機　　能： Ｘ線管操作タブ：
		//               Ｘ軸フレーム内「リセット」ボタン
		//               Ｙ軸フレーム内「リセット」ボタン
		//               マウスクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 各コマンドボタンのタグに動作指令があらかじめ埋め込まれている
		//           cmdXrayPosReset(0):XrayXOrg    Ｘ軸リセット
		//           cmdXrayPosReset(1):XrayYOrg    Ｙ軸リセット
		//
		//履　　歴： v9.0  2004/02/10 (SI4)間々田      新規作成
		//*************************************************************************************************
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //private void cmdXrayPosReset_Click(object sender, EventArgs e)
        //{
        //    //short Index = cmdXrayPosReset.GetIndex(eventSender);

        //    if (sender as Button == null) return;
        //    int Index = Array.IndexOf(cmdXrayPosReset, sender);
        //    if (Index < 0) return;

        //    int error_sts = 0;

        //    //機構部動作が可能かチェック
        //    if ((!modMechaControl.IsOkMechaMove()))
        //        return;
        //    //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

        //    //運転指令
        //    modSeqComm.SeqBitWrite(cmdXrayPosReset[Index].Tag, true);

        //}
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

		//*************************************************************************************************
		//機　　能： 微調テーブル：リセットボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void cmdFxyReset_Click(object sender, EventArgs e)
		{
			if (sender as Button == null) return;
			int Index = Array.IndexOf(cmdFxyReset, sender);
			if (Index < 0) return;

			int error_sts = 0;

            //Rev26.00 add by chouno 2017/10/16 
            frmMechaControl.Instance.tmrMecainfSeqCommEx();
                
            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if ((!modMechaControl.IsOkMechaMove()))
                return;

            //v19.51 回転大テーブルが装着されている場合は、操作不可にする by長野 2014/03/03
            //if ((modSeqComm.GetLargeRotTableSts() == 1))
            //Rev26.20 微調テーブルタイプも見る by chouno 2019/02/11 
            if ((modSeqComm.GetLargeRotTableSts() == 1) && CTSettings.t20kinf.Data.ftable_type == 0)
            {
                MessageBox.Show(CTResources.LoadResString(21365), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

			try
			{
				//上限信号と下限信号が同時ONの場合は、微調テーブルが取り外されているものと判断し、何もしない v17.47/v17.53追加 2011/03/09 by 間々田
				//  ＊表示上ではＸ、Ｙが逆転していることに注意
				if (Index == 0 && CTSettings.mecainf.Data.ystg_u_limit == 1 && CTSettings.mecainf.Data.ystg_l_limit == 1) return;
				if (Index == 1 && CTSettings.mecainf.Data.xstg_u_limit == 1 && CTSettings.mecainf.Data.xstg_l_limit == 1) return;

				//FCD のチェック  'v9.7追加 by added 間々田 2004/12/10
				if (!modSeqComm.CheckFCD(ScanCorrect.GVal_Fcd)) return;

				//確認メッセージ表示：
				//   微調テーブルがリセットしても、X線管にぶつからない位置にいることを確認して下さい。
				//   よろしければＯＫをクリックして下さい。
				DialogResult result = MessageBox.Show(CTResources.LoadResString(9568) + "\r" + "\r" 
													+ CTResources.LoadResString(StringTable.IDS_ClickOK), 
													Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
				if (result == DialogResult.Cancel) return;

				//動作が禁止されている場合 v9.6 追加 by 間々田 2004/10/12
				if ((CTSettings.scaninh.Data.seqcomm == 0) && (CTSettings.scaninh.Data.table_restriction == 0))
				{
					if (!modSeqComm.MySeq.stsMechaPermit) return;
				}

				//画面操作禁止
				this.Enabled = false;
				cmdFxyReset[Index].Enabled = false;

				//マウスポインタを砂時計にする
				Cursor.Current = Cursors.WaitCursor;

				//ステータス表示：動作中
				stsFineTable[Index].Status = StringTable.GC_STS_BUSY;

				//メカエラーリセット
				error_sts = modMechaControl.Mechaerror_reset(modDeclare.hDevID1);
				if (error_sts != 0) throw new Exception();

				//初期化&原点復帰
				switch (Index)
				{
					case 0:
						error_sts = modMechaControl.MecaYStgOrigin();
						break;

					case 1:
						error_sts = modMechaControl.MecaXStgOrigin();
						break;
				}
			}
            catch
            {
            }
			finally
			{
				//画面操作禁止解除
				this.Enabled = true;
				cmdFxyReset[Index].Enabled = true;
				Cursor.Current = Cursors.Default;

				//エラーメッセージ
				if (error_sts != 0) modCT30K.ErrMessage(error_sts, Icon: MessageBoxIcon.Error);
			}
		}

        //*************************************************************************************************
        //機　　能： 回転傾斜テーブル：リセットボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V22.00  15/08/20   (検S1)長野            新規作成
        //*************************************************************************************************
        private void cmdTiltAndRot_Reset_Click(object sender, EventArgs e)
        {
            if (sender as Button == null) return;
            int Index = Array.IndexOf(cmdTiltAndRot_Reset, sender);
            if (Index < 0) return;

            int error_sts = 0;

            //回転傾斜テーブル未装着の場合は、そのまま抜ける by長野 2015/08/20
            CTSettings.mecainf.Load();
            if (CTSettings.mecainf.Data.tiltrot_table == 0)
            {
                return;
            }

            //機構部動作が可能かチェック
            if ((!modMechaControl.IsOkMechaMove()))
                return;

            try
            {
                //上限信号と下限信号が同時ONの場合は、チルト回転テーブルが取り外されているものと判断し、何もしない
                if (Index == 0 && CTSettings.mecainf.Data.tilt_l_limit == 1 && CTSettings.mecainf.Data.tilt_u_limit == 1) return;
                if (Index == 1 && CTSettings.mecainf.Data.tiltrot_l_limit == 1 && CTSettings.mecainf.Data.tiltrot_u_limit == 1) return;

                //FCD のチェック  'v9.7追加 by added 間々田 2004/12/10
                if (!modSeqComm.CheckFCD(ScanCorrect.GVal_Fcd)) return;

                if (Index == 0)
                {
                    //確認メッセージ表示：
                    //   回転傾斜テーブル(傾斜)がリセットしても、X線管にぶつからない位置にいることを確認して下さい。 
                    //   よろしければＯＫをクリックして下さい。
                    DialogResult result = MessageBox.Show(CTResources.LoadResString(22023) + "\r" + "\r"
                                                        + CTResources.LoadResString(StringTable.IDS_ClickOK),
                                                        Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                    if (result == DialogResult.Cancel) return;
                }
                else if (Index == 1)
                {
                    //確認メッセージ表示：
                    //   回転傾斜テーブル(回転)がリセットしても、X線管にぶつからない位置にいることを確認して下さい。 
                    //   よろしければＯＫをクリックして下さい。
                    DialogResult result = MessageBox.Show(CTResources.LoadResString(22022) + "\r" + "\r"
                                                        + CTResources.LoadResString(StringTable.IDS_ClickOK),
                                                        Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                    if (result == DialogResult.Cancel) return;
                }
                

                //動作が禁止されている場合 v9.6 追加 by 間々田 2004/10/12
                if ((CTSettings.scaninh.Data.seqcomm == 0) && (CTSettings.scaninh.Data.table_restriction == 0))
                {
                    if (!modSeqComm.MySeq.stsMechaPermit) return;
                }

                //画面操作禁止
                this.Enabled = false;
                cmdTiltAndRot_Reset[Index].Enabled = false;

                //マウスポインタを砂時計にする
                Cursor.Current = Cursors.WaitCursor;

                //ステータス表示：動作中
                stsTiltAndRot[Index].Status = StringTable.GC_STS_BUSY;

                //メカエラーリセット
                error_sts = modMechaControl.Mechaerror_reset(modDeclare.hDevID1);
                if (error_sts != 0) throw new Exception();

                //初期化&原点復帰
                switch (Index)
                {
                    case 0:
                        error_sts = modMechaControl.MecaTilt_TiltOrigin();
                        break;

                    case 1:
                        error_sts = modMechaControl.MecaTilt_RotOrigin();
                        break;
                }
            }
            catch
            {
            }
            finally
            {
                //画面操作禁止解除
                this.Enabled = true;
                cmdTiltAndRot_Reset[Index].Enabled = true;
                Cursor.Current = Cursors.Default;

                //エラーメッセージ
                if (error_sts != 0) modCT30K.ErrMessage(error_sts, Icon: MessageBoxIcon.Error);
            }
        }

		//*************************************************************************************************
		//機　　能： ファントム「あり」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void ctbtnPhmOn_Click(object sender, EventArgs e)
		{
			int error_sts = 0;

            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if ((!modMechaControl.IsOkMechaMove()))
                return;

			//画面操作禁止
			this.Enabled = false;

			//マウスポインタを砂時計にする
			Cursor.Current = Cursors.WaitCursor;
			frmMechaControl.Instance.tmrPIOCheck.Enabled = false;

			//エラー時の扱い
			try								//v11.4追加 by 間々田 2006/03/02
			{
				if (ctbtnPhmOn.Value)
				{
					//メカエラーリセット
					error_sts = modMechaControl.Mechaerror_reset(modDeclare.hDevID1);		//v11.4追加 by 間々田 2006/03/02
					if (error_sts != 0) throw new Exception();								//v11.4追加 by 間々田 2006/03/02

					//なし
					error_sts = modMechaControl.MecaPhmOff();
				}
				else
				{
					//あり
					error_sts = modMechaControl.MecaPhmOn();
				}
			}
            catch
            {
            }

			finally							//v11.4追加 by 間々田 2006/03/02
			{
				//画面操作禁止解除
				this.Enabled = true;

				//マウスポインタを元に戻す
				Cursor.Current = Cursors.Default;
				frmMechaControl.Instance.tmrPIOCheck.Enabled = true;

				//エラーメッセージ
				if (error_sts != 0) modCT30K.ErrMessage(error_sts, Icon: MessageBoxIcon.Error);
			}
		}


		//*************************************************************************************************
		//機　　能： フィルタボタンマウスダウン処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 各コマンドボタンのタグに動作指令があらかじめ埋め込まれている
		//           cmdFilter (0): Filter0 無し
		//           cmdFilter (1): Filter1 フィルタ1
		//           cmdFilter (2): Filter2 フィルタ2
		//           cmdFilter (3): Filter3 フィルタ3
		//           cmdFilter (4): Filter4 フィルタ4
		//           cmdFilter (5): Filter5 フィルタ5
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
        //private void cmdFilter_MouseDown(object sender, MouseEventArgs e)
        //{
        //    if (sender as Button == null) return;
        //    int Index = Array.IndexOf(cmdFilter, sender);
        //    if (Index < 0) return;

        //    //フィルタを設定
        //    modSeqComm.SeqBitWrite(Convert.ToString(cmdFilter[Index].Tag), true);
        //}
        //v18.02 MouseDownをClickに変更 byやまおか 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
        private void cmdFilter_Click(object sender, EventArgs e)
        {
            if (sender as Button == null) return;
            int Index = Array.IndexOf(cmdFilter, sender);
            if (Index < 0) return;

            //変更2014/10/07hata_v19.51反映
            //フィルタを設定
            //modSeqComm.SeqBitWrite(Convert.ToString(cmdFilter[Index].Tag), true);

            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック
            //v18.00追加 byやまおか 2011/02/19
            if ((!modMechaControl.IsOkMechaMove()))
                return;

            ////フィルタを設定
            ////SeqBitWrite cmdFilter(Index).tag, True
            ////tagを使わないようにした    'v18.00変更 byやまおか 2011/02/12
            //string command_Renamed = null;
            //command_Renamed = "Filter" + Convert.ToString(Index);
            ////Titanの場合はフィルタの5番目をシャッターにする 'v18.00変更 byやまおか 2011/02/20
            //if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) & (Index == 5))
            //    command_Renamed = "Shutter";

            //modSeqComm.SeqBitWrite(command_Renamed, true);

            //Rev23.40 処理を見直した by長野 2016/06/19
            //マウスポインタを砂時計にする
            Cursor.Current = Cursors.WaitCursor;

            bool bret = false;

            //フォームを使用不可にする
            this.Enabled = false;

            //タッチパネル操作禁止
            modSeqComm.SeqBitWrite("PanelInhibit", true);

            bret = modSeqComm.ChangeFilter(Index);

            if (mechaAllResetExFlg == false)
            {
                //マウスポインタを元に戻す。
                Cursor.Current = Cursors.Default;

                //フォームを使用可にする
                this.Enabled = true;

                //タッチパネル操作可
                modSeqComm.SeqBitWrite("PanelInhibit", false);
            }

            if (bret != true)
            {
                MessageBox.Show(CTResources.LoadResString(6011), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

		//*************************************************************************************************
		//機　　能： チルトボタンマウスダウン時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 各コマンドボタンのタグに動作指令があらかじめ埋め込まれている
		//           cmdTilt(0):TiltCw      ＣＷ
		//           cmdTilt(1):TiltOrigin  原点復帰
		//           cmdTilt(2):TiltCcw     ＣＣＷ
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void cmdTilt_MouseDown(object sender, MouseEventArgs e)
		{
			if (sender as Button == null) return;
			int Index = Array.IndexOf(cmdTilt, sender);
			if (Index < 0) return;


			if (Index == 1)
			{
				//チルト原点復帰
				modSeqComm.SeqBitWrite(Convert.ToString(cmdTilt[Index].Tag), true);
			}
			else
			{
				//シーケンサに動作オン指令を送る
				frmMechaControl.Instance.SendOnToSeq(Convert.ToString(cmdTilt[Index].Tag));
			}
		}


		//*************************************************************************************************
		//機　　能： チルトボタンマウスアップ時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 各コマンドボタンのタグに動作指令があらかじめ埋め込まれている
		//           cmdTilt(0):TiltCw      ＣＷ
		//           cmdTilt(1):TiltOrigin  原点復帰
		//           cmdTilt(2):TiltCcw     ＣＣＷ
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void cmdTilt_MouseUp(object sender, MouseEventArgs e)
		{
			if (sender as Button == null) return;
			int Index = Array.IndexOf(cmdTilt, sender);
			if (Index < 0) return;

            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック
            //v18.00追加 byやまおか 2011/02/19
            if ((!modMechaControl.IsOkMechaMove()))
                return;

			//原点復帰の場合は抜ける
			if (Index == 1) return;

			//シーケンサに送信した動作オン指令を解除する
			frmMechaControl.Instance.SendOffToSeq();			//v14.14追加 by 間々田 2008/02/20
		}


		//*************************************************************************************************
		//機　　能： Ｘ線管切替ボタンマウスダウン処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 各コマンドボタンのタグに動作指令があらかじめ埋め込まれている
		//           cmdMultiTube(0):Xray160    １６０ｋＶ
		//           cmdMultiTube(1):Xray225    ２２５ｋＶ
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //private void cmdMultiTube_MouseDown(object sender, MouseEventArgs e)
        //{
        //    //追加2014/10/07hata_v19.51反映
        //    //機構部動作が可能かチェック
        //    //v18.00追加 byやまおか 2011/02/19
        //    if ((!modMechaControl.IsOkMechaMove()))
        //        return;

        //    //Ｘ線管を設定
        //    modSeqComm.SeqBitWrite(cmdMultiTube[Index].Tag, true);
        //}
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''


		//*************************************************************************************************
		//機　　能： スライスライトクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*************************************************************************************************
		private void ctbtnSlight_Click(object sender, EventArgs e)
		{
            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック
            //v18.00追加 byやまおか 2011/02/19
            if ((!modMechaControl.IsOkMechaMove()))
                return;

            //シーケンサにオン・オフを設定
			//変更2015/02/26hata
            //modSeqComm.SeqBitWrite((ctbtnSlight.Value ? "SlightOff" : "SlightOn"), true);
            modSeqComm.SeqBitWrite((ctbtnSlight.Value ? "SLightOff" : "SLightOn"), true);
        }


		//*************************************************************************************************
		//機　　能： I.I.電源ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2009/07/21 (SS1)間々田   リニューアル
		//*************************************************************************************************
		private void ctbtnIIPower_Click(object sender, EventArgs e)
		{
			bool err_sts = false;

            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック
            //v18.00追加 byやまおか 2011/02/19
            if ((!modMechaControl.IsOkMechaMove()))
                return;

            //変更2014/10/07hata_v19.51反映
            //シーケンサにオン・オフを設定：高速撮影の場合
            modCT30K.OnOffStatusConstants theTVIIPower = default(modCT30K.OnOffStatusConstants);
            theTVIIPower = (ctbtnIIPower.Value ? modCT30K.OnOffStatusConstants.OffStatus : modCT30K.OnOffStatusConstants.OnStatus);

            //if (modHighSpeedCamera.IsHSCmode)
            //change by chouno 2019/02/12
            if (modHighSpeedCamera.IsHSCmode || modHighSpeedCamera.IsDropTestmode)
            {
                err_sts = modHighSpeedCamera.HSIIPower(theTVIIPower);
                //SeqBitWrite IIf(ctbtnIIPower.Value, "TVIIPowerOff", "TVIIPowerOn"), True
                //v17.20 検出器切替用の条件を追加 by 長野 2010-09-03
            }
            else if (CTSettings.SecondDetOn & mod2ndDetctor.IsDet2mode)
            {
                modSeqComm.SeqBitWrite((ctbtnIIPower.Value ? "TVIIPowerOff" : "TVIIPowerOn"), true);
            }
            else
            {
                //シーケンサにオン・オフを設定
                modSeqComm.SeqBitWrite((ctbtnIIPower.Value ? "IIPowerOff" : "IIPowerOn"), true);
            }
       
        }


		//*************************************************************************************************
		//機　　能： 「オールリセット」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void cmdAllReset_Click(object sender, EventArgs e)
		{
			int error_sts = 0;

			//If Not (frmCTMenu.DoorStatus = Doorclosed) Then 'インターロック用

			//If Not (frmCTMenu.DoorStatus = DoorLocked) Then '電磁ロック用

            //変更2014/10/07hata_v19.51反映
            ////v29.99 電磁ロックがない装置に対応するため変更 by長野 2013/04/18
            //if ((CTSettings.scaninh.Data.door_lock == 0))
            //{
            //    //電磁ロック用
            //    if (!(frmCTMenu.Instance.DoorStatus == frmCTMenu.DoorStatusConstants.DoorLocked))
            //    {
            //        MessageBox.Show(CTResources.LoadResString(17505), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            //}
            //if (!modSeqComm.MySeq.stsDoorPermit)
            //{
            //    if (frmCTMenu.Instance.DoorStatus == frmCTMenu.DoorStatusConstants.DoorOpened)
            //    {
            //        //MsgBox LoadResString(20058) & vbCrLf & LoadResString(8022), vbCritical
            //        MessageBox.Show(CTResources.LoadResString(20058) + "\r\n" + CTResources.LoadResString(8022), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            //}

            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック(上記を関数化)
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if ((!modMechaControl.IsOkMechaMove()))
                return;

			//エラー時の処理
            try
            {
                //Rev23.20 フラグON by長野 2016/01/16
                mechaAllResetExFlg = true;

                //変更2014/10/07hata_v19.51反映
                //v19.51 条件追加 X線・検出器昇降の場合は上に原点があるため、fcd制限でとめない by長野 2014/02/27
                //Rev23.10 計測CTモードの場合、昇降原点下のため出さない by長野 2015/10/22
                if (CTSettings.t20kinf.Data.ud_type == 0 || CTSettings.scaninh.Data.cm_mode == 1)
                {

                    //FCD(オフセット込み)がfcd_limitより小さな時
                    //if (frmMechaControl.Instance.FCDWithOffset < CTSettings.GVal_FcdLimit)
                    if (frmMechaControl.Instance.FCDWithOffset < modSeqComm.GetFCDLimit()) //Rev26.10 change by chouno 2018/01/05
                    {
                        //メッセージ表示：テーブルがＸ線管に近過ぎるため、リセットを中止します。
                        MessageBox.Show(CTResources.LoadResString(9470), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    //v9.5 追加ここまで by 間々田 2004/09/28
                }

                //確認ダイアログ表示：
                //変更2014/10/07hata_v19.51反映
                //v18.00追加 byやまおか 2011/03/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                //Rev23.10 計測CTモードの場合、昇降原点下のため出さない by長野 2015/10/22
                if (CTSettings.scaninh.Data.cm_mode == 1)
                {
                    if ((CTSettings.scaninh.Data.avmode == 0))
                    {
                        //産業用CTモードの場合
                        //産業用CTモードの場合
                        //   試料テーブルが動作しますので、試料の固定を確認してください。 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                        //   よろしければＯＫをクリックして下さい。
                        if (MessageBox.Show(CTResources.LoadResString(9479) + "\r" + "\r" + CTResources.LoadResString(StringTable.IDS_ClickOK), Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                            return;
                    }
                    else
                    {
                        //   試料テーブルが上昇しても、X線管にぶつからない位置にいることを確認して下さい。
                        //   よろしければＯＫをクリックして下さい。
                        //   追加 コモンによってメッセージを切り替える by 間々田 2003/10/24
                        //DialogResult result = MessageBox.Show(CTResources.LoadResString(CTSettings.t20kinf.Data.ud_type == 1 ? 9472 : 9510) + "\r" + "\r" 
                        //Rev21.00 変更 by長野 2015/03/18
                        DialogResult result = MessageBox.Show(CTResources.LoadResString(CTSettings.t20kinf.Data.ud_type == 1 ? 22010 : 9510) + "\r" + "\r"
                                                            + CTResources.LoadResString(StringTable.IDS_ClickOK),
                                                            Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                        if (result == DialogResult.Cancel)
                            return;
                    }
                }

                //画面操作禁止
                this.Enabled = false;
                //    cmdAllReset.Enabled = False
                Cursor.Current = Cursors.WaitCursor;
                //    Me.Refresh                                      'V4.0 append by 鈴山 2001/03/22

                //Rev20.00 コモン初期化処理の前はファイルに落としておく。 by長野 2015/01/24
                ComLib.SaveSharedCTCommon();

                //コモンの初期化処理(ＶＢ用)
                //error_sts = modCommon.ctcominit(0);
                error_sts = ComLib.ctcominit(0);
                if (error_sts != 0) throw new Exception();
                Application.DoEvents();			//added by 山本　2004-8-5
                this.Refresh();					//added by 山本　2004-8-5

                //実行時はフラグをセット                         'V4.0 append by 鈴山 2001/03/23
                //Call StatusFlag_Set(AllResetFlag, True)
                //frmMechaControl.tmrPIOCheck.Enabled = False                     'v11.5変更 by 間々田 2006/06/22

                //タッチパネル操作禁止
                modSeqComm.SeqBitWrite("PanelInhibit", true);

                //監視時間の設定
                //modDeclare.Sleep(100);
                System.Threading.Thread.Sleep(100);
                error_sts = modMechaControl.PioChkEnd();

                //一度、すべてのデバイスをクローズする
                //制御スイッチクローズ
                error_sts = modMechaControl.SwOpeEnd();
                if (error_sts != 0) throw new Exception();

                //メカクローズ
                modMechaControl.MechaClose();

                //再度、すべてのデバイスをオープンする
                modMechaControl.MechaOpen();

                //制御スイッチの初期化
                error_sts = modMechaControl.SwOpeStart();
                if (error_sts != 0) throw new Exception();

                //frmStatus.tmrPIOCheck_Timer    'V5.0 append by 山本 2001/08/04
                modMechaControl.PioChkStart(20);

                //回転軸初期化
                error_sts = modMechaControl.RotateInit(modDeclare.hDevID1);
                if (error_sts != 0) throw new Exception();

                //回転軸原点復帰
                error_sts = modMechaControl.MecaRotateOrigin();
                if (error_sts != 0) throw new Exception();

                //昇降軸原点復帰
                error_sts = modMechaControl.MecaUdOrigin();
                if (error_sts != 0) throw new Exception();

                // V5.0 deleted by 山本 2001/07/31 START
                ///    'ファントム初期化
                ///    error_sts = PhmInit(hDevID1)
                ///    If error_sts <> 0 Then GoTo ExitHandler
                ///
                ///    'ファントムリセット
                ///    error_sts = PhmOrigin(hDevID1)
                ///    If error_sts <> 0 Then GoTo ExitHandler
                // V5.0 deleted by 山本 2001/07/31 END

                //Rev20.00 FPDの場合は、ファントムなし by長野 2015/02/16
                if (CTSettings.detectorParam.Use_FlatPanel == false)
                {
                    // V5.0 append by 山本 2001/07/31 START
                    //ファントムなし位置に移動
                    error_sts = modMechaControl.MecaPhmOff();
                    if (error_sts != 0) throw new Exception();
                }

                if (CTSettings.scaninh.Data.fine_table == 0)
                {
                    //If scaninh.fine_table_x = 0 Then
                    //Rev26.00  回転大テーブル装着時は微調テーブルリセットはしない add by chouno 2017/03/13
                    //if (modSeqComm.GetLargeRotTableSts() == 0)
                    //Rev26.20 微調テーブルタイプを見るように変更 by chouno 2019/02/11
                    if (modSeqComm.GetLargeRotTableSts() == 0 || CTSettings.t20kinf.Data.ftable_type == 1)
                    {
                        //上限信号と下限信号が同時ONの場合は、微調テーブルが取り外されているものと判断し、何もしない v17.47/v17.53変更 2011/03/09 by 間々田
                        if ((CTSettings.scaninh.Data.fine_table_x == 0) &&
                            (CTSettings.mecainf.Data.xstg_u_limit == 0 || CTSettings.mecainf.Data.xstg_l_limit == 0))
                        {
                            //微調X軸原点復帰
                            error_sts = modMechaControl.MecaXStgOrigin();
                            if (error_sts != 0) throw new Exception();
                        }

                        //If scaninh.fine_table_y = 0 Then

                        //上限信号と下限信号が同時ONの場合は、微調テーブルが取り外されているものと判断し、何もしない v17.47/v17.53変更 2011/03/09 by 間々田
                        if ((CTSettings.scaninh.Data.fine_table_y == 0) &&
                            (CTSettings.mecainf.Data.ystg_u_limit == 0 || CTSettings.mecainf.Data.ystg_l_limit == 0))
                        {
                            //微調Y軸原点復帰
                            error_sts = modMechaControl.MecaYStgOrigin();
                            if (error_sts != 0) throw new Exception();
                        }
                    }
                }
                // V5.0 append by 山本 2001/07/31 END

                //Rev22.00 回転傾斜テーブル有りで、かつ、装着状態の場合はリセットする by長野 2015/09/04
                if (CTSettings.scaninh.Data.tilt_and_rot == 0 && CTSettings.mecainf.Data.tiltrot_table == 1)
                {
                    //回転傾斜テーブル 傾斜原点復帰
                    error_sts = modMechaControl.MecaTilt_TiltOrigin();
                    if (error_sts != 0) throw new Exception();

                    //回転傾斜テーブル 回転原点復帰
                    error_sts = modMechaControl.MecaTilt_RotOrigin();
                    if (error_sts != 0) throw new Exception();
                }

                //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                //Ｘ線管回転軸の原点復帰 v9.0 added by 間々田 2004/02/09
                //    If scaninh.xray_rotate = 0 Then
                //        '原点復帰
                //        error_sts = PioOutBit("XRAYROTORG", 1)
                //        PauseForDoEvents 0.2                        'added by 山本　2004-6-2
                //        error_sts = PioOutBit("XRAYROTORG", 0)      'added by 山本　2004-6-2
                //        If error_sts <> 0 Then GoTo ExitHandler
                //    End If
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''


                //変更2014/10/07hata_v19.51反映
                ////テーブルＹ軸のリセット
                //cmdTableReset_Click(cmdTableReset1, EventArgs.Empty);			//v15.0追加 by 間々田 2009/05/12
                //v19.18 条件追加 電動フィルタがあるときはリセットしない byやまおか　2013/09/25
                //If (scaninh.filter <> 0) Then
                //v19.50 条件変更 by長野 2014/01/07
                //if ((CTSettings.scaninh.Data.filter != 0) | (CTSettings.scaninh.Data.avmode == 0))
                //Rev23.10 条件変更 by長野 2015/11/14
                //if ((CTSettings.scaninh.Data.filter != 0) | (CTSettings.scaninh.Data.avmode == 0))
                if (((CTSettings.scaninh.Data.filter != 0) | (CTSettings.scaninh.Data.avmode == 0)) && (CTSettings.scaninh.Data.multi_tube == 1))
                {
                    //テーブルＹ軸のリセット
                    //cmdTableReset_Click 1       'v15.0追加 by 間々田 2009/05/12
                    //テーブルＹ軸(旧X軸)のリセット
                    //cmdTableReset_Click 1       'v15.0追加 by 間々田 2009/05/12
                    if ((CTSettings.scaninh.Data.table_x == 0))
                        cmdTableReset_Click(cmdTableReset[1], new System.EventArgs());  //v18.00変更 byやまおか 2011/03/14 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

                }

                //産業用CTモードの場合は、すべての移動軸をリセットする   'v18.00追加 byやまおか 2011/03/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                if ((CTSettings.scaninh.Data.avmode == 0))
                {
                    modCT30K.PauseForDoEvents(0.5F);

                    //テーブルＸ軸(FCD,旧Y軸)のリセット
                    if ((CTSettings.scaninh.Data.table_y == 0))
                        cmdTableReset_Click(cmdTableReset[0], new System.EventArgs());

                    //検出器移動(FDD)のリセット
                    if ((CTSettings.scaninh.Data.ii_move == 0))
                        modSeqComm.SeqBitWrite("IIOrigin", true);

                    //v19.51 標準のオプションとして使用できるように外に出す by長野 2014/02/27
                    //検出器シフトのリセット
                    //If DetShiftOn Then cmdDetChangeReset_Click

                    ////シャッターフィルターのリセット
                    //if ((CTSettings.scaninh.Data.shutterfilter == 0))
                    //    modSeqComm.SeqBitWrite("Shutter", true);
                    //Rev23.40 変更 by長野 2016/06/19
                      cmdFilter_Click(cmdFilter[5], new System.EventArgs());

                }

                //v19.51 標準のオプションとして使用できるように外に出す by長野 2014/02/27
                //検出器シフトのリセット
                //Rev25.00 Wスキャン追加 by長野 2016/06/19
                //if (CTSettings.DetShiftOn)
                if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
                    cmdDetChangeReset_Click(cmdDetChangeReset, new System.EventArgs());

                //v19.50 追加 最後のY軸リセットの関数を通らない場合、ロックがかかったままになる。 2013/11/08
                //If Not (scaninh.shutterfilter <> 0 Or scaninh.avmode = 0) Then
                if (!(CTSettings.scaninh.Data.filter != 0 | CTSettings.scaninh.Data.avmode == 0))
                {

                    //タッチパネル操作禁止解除
                    modSeqComm.SeqBitWrite("PanelInhibit", false);

                    //画面操作禁止解除
                    this.Enabled = true;
                    //cmdAllReset.Enabled = True
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                    //Me.Refresh                                      'V4.0 append by 鈴山 2001/03/22

                }

                //終了時はフラグをリセット   V4.0 append by 鈴山 2001/03/23
                //Call StatusFlag_Set(AllResetFlag, False)
                //frmMechaControl.tmrPIOCheck.Enabled = True                      'v11.5変更 by 間々田 2006/06/22
                return;

            }
            catch (Exception)
            {
                //タッチパネル操作禁止解除
                modSeqComm.SeqBitWrite("PanelInhibit", false);

                //画面操作禁止解除
                this.Enabled = true;
                //cmdAllReset.Enabled = True
                Cursor.Current = Cursors.Default;
                //Me.Refresh                                      'V4.0 append by 鈴山 2001/03/22

                //エラーメッセージ
                if (error_sts != 0) modCT30K.ErrMessage(error_sts, Icon: MessageBoxIcon.Error);
            }
            finally //Rev23.20 filnally追加 by長野 2016/01/16
            {
                long cnt = 0;
                //Rev23.20 
                while((tmrWait.Enabled == true || tmrWaitDet.Enabled == true || tmrWaitXray.Enabled == true) && (cnt < 60))
                {
                    modCT30K.PauseForDoEvents(1.0f);
                    cnt++;
                }

                //タッチパネル操作禁止解除
                modSeqComm.SeqBitWrite("PanelInhibit", false);

                //画面操作禁止解除
                this.Enabled = true;
                //cmdAllReset.Enabled = True
                Cursor.Current = Cursors.Default;
                //Me.Refresh                                      'V4.0 append by 鈴山 2001/03/22

                mechaAllResetExFlg = false;
            }
		}


		//*************************************************************************************************
		//機　　能： コモン初期化ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void cmdComInit_Click(object sender, EventArgs e)
		{
			//コモン初期化ダイアログ表示
			frmComInit.Instance.ShowDialog();
		}


		//*************************************************************************************************
		//機　　能： 「閉じる」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void cmdClose_Click(object sender, EventArgs e)
		{
			//メカ準備－詳細フォームをアンロード
			this.Close();
		}


		//*************************************************************************************************
		//機　　能： 微調テーブル（Ｙ軸）移動ボタンの処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   0:右へ，1:左へ
		//戻 り 値： なし
		//
		//補　　足： 従来のＸ軸移動
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*************************************************************************************************
		private void cwbtnFineTableX_ValueChanged(object sender, EventArgs e)
		{
            //変更2014/11/20hata
            //if (sender as Button == null) return;
            if (sender as CWButton == null) return;
			int Index = Array.IndexOf(cwbtnFineTableX, sender);
			if (Index < 0) return;

			int rc = 0;

            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック(上記を関数化)
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if (cwbtnFineTableX[Index].Value == true)
            {
                if (!modMechaControl.IsOkMechaMove())
                    return;

                //Rev26.00 add by chouno 2017/03/13
                //if (modSeqComm.GetLargeRotTableSts() == 1)
                //微調テーブルタイプを見るように変更 by chouno 2019/02/11
                if (modSeqComm.GetLargeRotTableSts() == 1 && CTSettings.t20kinf.Data.ftable_type == 0)
                {
                    MessageBox.Show(CTResources.LoadResString(21365), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

			if (cwbtnFineTableX_ValueChanged_IsBusy) return;
			cwbtnFineTableX_ValueChanged_IsBusy = true;

			//エラー時の扱い
			try 
			{
				//Valueで分岐
				if (cwbtnFineTableX[Index].Value)
				{
					//動作が禁止されている場合 v9.6 追加 by 間々田 2004/10/12
					if ((CTSettings.scaninh.Data.seqcomm == 0) && (CTSettings.scaninh.Data.table_restriction == 0))
					{
						//if (!modSeqComm.MySeq.stsMechaPermit) throw new Exception();
                        //Rev20.00 追加 メッセージを出すようにした by長野 2015/02/06
                        if (!modSeqComm.MySeq.stsMechaPermit)
                        {
                            MessageBox.Show(CTResources.LoadResString(20038), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //ストリングテーブル化 'v17.60 by長野 2011/05/25

                            throw new Exception();
                        }
                    }

					//移動
					rc = modMechaControl.XStgManual(modDeclare.hDevID1, Index, modMechaControl.GVal_FineTableSpeed);
				}
				else
				{
					//微調Ｘ軸停止
					//rc = XStgSlowStop(hDevID1)
					rc = modMechaControl.MechaXStgStop();		//v15.0変更 2009/06/18
				}
			}
            catch
            {
            }
			finally
			{
				//マウスカーソルを切り替える
				Cursor.Current = (cwbtnFineTableX[Index].Value && (rc == 0) ? Cursors.AppStarting : Cursors.Default);

				//エラーが発生している場合：メッセージ表示
				if (rc != 0) modCT30K.ErrMessage(rc, Icon: MessageBoxIcon.Error);

				//フラグリセット
				cwbtnFineTableX_ValueChanged_IsBusy = false;
			}
		}


		//*************************************************************************************************
		//機　　能： 微調テーブル（Ｙ軸）移動ボタンマウスアップ処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   0:右へ，1:左へ
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*************************************************************************************************
		private void cwbtnFineTableX_MouseUp(object sender, MouseEventArgs e)
		{
            //変更2014/11/20hata
            //if (sender as Button == null) return;
            if (sender as CWButton == null) return;
            int Index = Array.IndexOf(cwbtnFineTableX, sender);
			if (Index < 0) return;

			//ボタンのValueプロパティを確実にオフする
			//（これがないとボタンを連打した場合，ボタンのValueプロパティがTrueのままになってしまうので）
			cwbtnFineTableX[Index].Value = false;
		}


		//*************************************************************************************************
		//機　　能： 微調テーブル（Ｘ軸）移動ボタンの処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   0:縮小方向へ，1:拡大方向へ
		//戻 り 値： なし
		//
		//補　　足： 従来のＹ軸移動
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*************************************************************************************************
		private void cwbtnFineTableY_ValueChanged(object sender, EventArgs e)
		{
            //変更2014/11/20hata
            //if (sender as Button == null) return;
            if (sender as CWButton == null) return;
			int Index = Array.IndexOf(cwbtnFineTableY, sender);
			if (Index < 0) return;

			int rc = 0;

            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック(上記を関数化)
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if (cwbtnFineTableY[Index].Value == true)
            {
                if (!modMechaControl.IsOkMechaMove())
                    return;

                //Rev26.00 add by chouno 2017/03/13
                //if (modSeqComm.GetLargeRotTableSts() == 1)
                //Rev26.20 微調テーブルタイプを見るように変更 by chouno 2019/02/11
                if (modSeqComm.GetLargeRotTableSts() == 1 && CTSettings.t20kinf.Data.ftable_type == 0)
                {
                    MessageBox.Show(CTResources.LoadResString(21365), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (cwbtnFineTableY_ValueChanged_IsBusy) return;
			cwbtnFineTableY_ValueChanged_IsBusy = true;

			//エラー時の扱い
			try
			{
				//Valueで分岐
				if (cwbtnFineTableY[Index].Value)
				{
					//動作が禁止されている場合 v9.6 追加 by 間々田 2004/10/12
					if ((CTSettings.scaninh.Data.seqcomm == 0) && (CTSettings.scaninh.Data.table_restriction == 0))
					{
                        //if (!modSeqComm.MySeq.stsMechaPermit)
                        // return
                        //Rev20.00 追加 メッセージを出すようにした by長野 2015/02/06
                        if (!modSeqComm.MySeq.stsMechaPermit)
                        {
                            MessageBox.Show(CTResources.LoadResString(20038), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //ストリングテーブル化 'v17.60 by長野 2011/05/25

                            return;
                        }

					}

					rc = modMechaControl.YStgManual(modDeclare.hDevID1, Index, modMechaControl.GVal_FineTableSpeed);
				}
				else
				{
					//微調Ｙ軸停止
					//rc = YStgSlowStop(hDevID1)
					rc = modMechaControl.MechaYStgStop();		//v15.0変更 2009/06/18
				}
			}
			finally
			{
				//マウスカーソルを切り替える
				Cursor.Current = (cwbtnFineTableY[Index].Value && (rc == 0) ? Cursors.AppStarting : Cursors.Default);

				//エラーが発生している場合：メッセージ表示
				if (rc != 0) modCT30K.ErrMessage(rc, Icon: MessageBoxIcon.Error);

				//フラグリセット
				cwbtnFineTableY_ValueChanged_IsBusy = false;
			}
		}


		//*************************************************************************************************
		//機　　能： 微調テーブル（Ｘ軸）移動ボタンマウスアップ処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   0:縮小方向へ，1:拡大方向へ
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*************************************************************************************************
		private void cwbtnFineTableY_MouseUp(object sender, MouseEventArgs e)
		{
            //変更2014/11/20hata
            //if (sender as Button == null) return;
            if (sender as CWButton == null) return;
            int Index = Array.IndexOf(cwbtnFineTableY, sender);
			if (Index < 0) return;

			//ボタンのValueプロパティを確実にオフする
			//（これがないとボタンを連打した場合，ボタンのValueプロパティがTrueのままになってしまうので）
			cwbtnFineTableY[Index].Value = false;
		}


		//*************************************************************************************************
		//機　　能： メカに送信した動作オン指令を解除する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v14.14  2008/02/20 (WEB)間々田  新規作成
		//*************************************************************************************************
		public void SendOffToMecha()
		{
			//微調テーブル動作解除
			cwbtnFineTableX0.Value = false;
			cwbtnFineTableX1.Value = false;
			cwbtnFineTableY0.Value = false;
			cwbtnFineTableY1.Value = false;

		}

		//*************************************************************************************************
		//機　　能： Ｘ線管回転ステータス変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 表示色を設定する
		//
		//履　　歴： v11.3 2006/02/20  (SI3)間々田   新規作成
		//*************************************************************************************************
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
		//Private Sub stsXrayRot_Changed()
		//
		//    Select Case stsXrayRot.Status
		//
		//        '動作中
		//        Case GC_STS_BUSY
		//
		//            'rot_busyをセットする
		//            putcommon_long "mecainf", "rot_busy", 1
		//
		//        '異常
		//        Case GC_Xray_Error
		//
		//            'rot_errorをセットする
		//            putcommon_long "mecainf", "rot_error", 1
		//
		//
		//        '正転限/逆転限
		//        Case LoadResString(12363), _
		//'             LoadResString(12364)
		//
		//            'rot_readyをリセットする
		//            putcommon_long "mecainf", "rot_ready", 0
		//
		//    End Select
		//
		//End Sub
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''


		private void tmrWait_Timer(object sender, EventArgs e)
		{
            //変更2014/10/07hata_v19.51反映
            ////v29.99 上記のコメントアウトした内容を、下記の内容に変更(機能をいくつか削除しているため) by長野 2013/04/08'''''ここから'''''
            //if ((frmMechaControl.Instance.TableYPos != 0 || modSeqComm.MySeq.stsXLeft || modSeqComm.MySeq.stsXRight) && 
            //    (TimerCount < 30))
            //{
            //    TimerCount = TimerCount + 1;
            //    return;
            //}

            //検出器切替原点復帰 v17.20 by 長野 2010/09/20
            if (mod2ndDetctor.IsSwtichingDet & (TimerCount < 30))
            {
                TimerCount = TimerCount + 1;
                return;   
             
            //Y軸リセット
            //ElseIf ((frmMechaControl.TableYPos <> 0) Or MySeq.stsXLeft Or MySeq.stsXRight) And (TimerCount < 30) Then
            //v19.50 条件式変更 by長野 2013/12/17               
            }
            //else if (((frmMechaControl.Instance.TableYPos != 0) | modSeqComm.MySeq.stsXLeft | modSeqComm.MySeq.stsXRight) & (TimerCount < 30) & (ResetAxis == 1))
            //Rev23.40 変更 by長野 2016/06/19
            else if ((modSeqComm.MySeq.stsXLeft | modSeqComm.MySeq.stsXRight | modSeqComm.MySeq.stsXRLimit | modSeqComm.MySeq.stsXLLimit) & (TimerCount < 60) & (ResetYAxis == true))
            {
                TimerCount = TimerCount + 1;
                return;
            
            //X軸(旧Y軸)リセット 'v18.00追加 byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //ElseIf ((MySeq.stsYForward Or MySeq.stsYBackward)) And (TimerCount < 30) Then
            //v19.50 条件式変更 by長野 2013/12/17
            }
            //else if (((modSeqComm.MySeq.stsYForward | modSeqComm.MySeq.stsYBackward)) & (TimerCount < 30) & (ResetAxis == 0))
            //Rev23.40 変更 by長野 2016/06/19 
            else if (((modSeqComm.MySeq.stsYForward | modSeqComm.MySeq.stsYBackward)) & (TimerCount < 60) & (ResetXAxis == true))
            {
                TimerCount = TimerCount + 1;
                return; 
               
            //I.I.切替原点復帰 v16.01 追加 by 山影 10-02-23
            }
            //else if (modHighSpeedCamera.IsIIChanging & (TimerCount < 30))
            //Rev23.40 変更 by長野 2016/06/19
            else if (modHighSpeedCamera.IsIIChanging & (TimerCount < 60))
            {
                TimerCount = TimerCount + 1;
                return;
            }
            //Rev23.20 追加 by長野 2015/12/17
            //else if (((modSeqComm.MySeq.stsFDSystemBusy)) & (TimerCount < 30))
            //Rev23.40 変更 by長野 2016/06/19
            else if (((modSeqComm.MySeq.stsFDSystemBusy)) & (TimerCount < 60))
            {
                TimerCount = TimerCount + 1;
                return;
            }

            //Rev23.40 上記チェックを抜けた理由がX,Y軸のリセット完了かどうかチェック
            if (!(((modSeqComm.MySeq.stsYForward | modSeqComm.MySeq.stsYBackward)) & (TimerCount < 60) & (ResetXAxis == true)))
            {
                ResetXAxis = false;
            }
            if (!((modSeqComm.MySeq.stsXLeft | modSeqComm.MySeq.stsXRight | modSeqComm.MySeq.stsXRLimit | modSeqComm.MySeq.stsXLLimit) & (TimerCount < 60) & (ResetYAxis == true)))
            {
                ResetYAxis = false;
            }

			//タイマー解除
			tmrWait.Enabled = false;

            //Rev23.20 メカオールリセット中は、タイマーを抜けてフォームのenableは戻さない by長野 2016/01/16
            if (mechaAllResetExFlg == false)
            {
                //マウスポインタを元に戻す
                Cursor.Current = Cursors.Default;

                //フォームを使用可にする
                this.Enabled = true;

                //タッチパネル操作禁止
                modSeqComm.SeqBitWrite("PanelInhibit", false);
            }
		}


		//*************************************************************************************************
		//機　　能： I.I.移動ボタンの処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer
		//戻 り 値： なし
		//
		//補　　足： 各コマンドボタンのタグに動作指令があらかじめ埋め込まれている
		//           cwbtnIIMove(0):IIForward      II前進
		//           cwbtnIIMove(1):IIBackward     II後退
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*************************************************************************************************
		private void cwbtnIIMove_ValueChanged(object sender, EventArgs e)
		{
            //変更2014/11/20hata
            //if (sender as Button == null) return;
            if (sender as CWButton == null) return;
			int Index = Array.IndexOf(cwbtnIIMove, sender);
			if (Index < 0) return;


            ////v17.40 メンテナンスのときは検査室扉が閉まっていることをチェックしないように変更 by 長野 2010/10/21
            ////v17.40 稲葉さんの改造待ちのためコメントアウト
            //if (!modSeqComm.MySeq.stsDoorPermit)
            //{
            //    //v17.20 検査室の扉が閉じていなければ無効 by 長野 2010/09/20
            //    //インターロック用
            //    if ((frmCTMenu.Instance.DoorStatus == frmCTMenu.DoorStatusConstants.DoorOpened) && (cwbtnIIMove[Index].Value == true))
            //    {
            //        //MsgBox "Ｘ線検査室の扉が開いているため検出器を移動することができません。" & vbCrLf & "X線検査室の扉を閉めてから､再度操作を行なって下さい｡", vbCritical
            //        //MsgBox LoadResString(20056) & vbCrLf & LoadResString(8022), vbCritical 'ストリングテーブル化 'v17.60 by長野 2011/05/22
            //        //ストリングテーブルの番号を修正 v17.61 by長野　2011/09/12
            //        MessageBox.Show(CTResources.LoadResString(20058) + "\r\n" + CTResources.LoadResString(8022), 
            //                        Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            //}

            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック(上記を関数化)
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if (cwbtnIIMove[Index].Value == true)
            {
                if (!modMechaControl.IsOkMechaMove())
                    return;

                //Rev26.00 add by chouno 2017/03/13
                if (modMechaControl.IsOkMechaMoveWithLargeTable() == false)
                {
                    return;
                }
            }

			//エラー時の扱い
			try
			{
				//Valueで分岐
				if (cwbtnIIMove[Index].Value)
				{
					//シーケンサに動作オン指令を送る
					frmMechaControl.Instance.SendOnToSeq(Convert.ToString(cwbtnIIMove[Index].Tag));
				}
				else
				{
					//シーケンサに送信した動作オン指令を解除する
					frmMechaControl.Instance.SendOffToSeq();
				}

				//マウスカーソルの制御
				Cursor.Current = (cwbtnIIMove[Index].Value ? Cursors.AppStarting : Cursors.Default);

				return;
			}
			catch (Exception ex)
			{
				//マウスカーソルを元に戻す
				Cursor.Current = Cursors.Default;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				'エラーメッセージ
				ErrMessage Err.Description, vbCritical
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				//エラーメッセージ
				//modCT30K.ErrMessage(ex.Message, Icon: MessageBoxIcon.Error);
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                
			}
        }


		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //Rev23.10 必要になったため復活 by長野 2015/09/28
        private void tmrWaitDet_Tick(object sender, EventArgs e)
        {

            //検出器切替原点復帰 v17.20 by 長野 2010/09/20
            if (mod2ndDetctor.IsSwtichingDet & (TimerCount < 30))
            {
                TimerCount = TimerCount + 1;
                return;
            }
            //タイマー解除
            tmrWaitDet.Enabled = false;

            //Rev23.20 メカオールリセット中は、タイマーを抜けてフォームのenableは戻さない by長野 2016/01/16
            if (mechaAllResetExFlg == false)
            {
                //マウスポインタを元に戻す
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                //フォームを使用可にする
                this.Enabled = true;

                //タッチパネル操作禁止
                modSeqComm.SeqBitWrite("PanelInhibit", false);
            }

        }
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
        
        
        //追加2015/02/10hata
        private void cwbtnFineTableY_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (sender as CWButton == null) return;
            int Index = Array.IndexOf(cwbtnFineTableY, sender);
            if (Index < 0) return;

            if (!cwbtnFineTableY[Index].Capture)
            {
                cwbtnFineTableY[Index].Value = false;
            }
        }

        //追加2015/02/10hata
        private void cwbtnFineTableX_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (sender as CWButton == null) return;
            int Index = Array.IndexOf(cwbtnFineTableX, sender);
            if (Index < 0) return;

            if (!cwbtnFineTableX[Index].Capture)
            {
                cwbtnFineTableX[Index].Value = false;
            }
        }

        //追加2015/02/10hata
        private void cwbtnIIMove_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (sender as CWButton == null) return;
            int Index = Array.IndexOf(cwbtnIIMove, sender);
            if (Index < 0) return;

            if (!cwbtnIIMove[Index].Capture)
            {
                cwbtnIIMove[Index].Value = false;
            }
        }

        //Rev23.10 追加 by長野 2015/09/18 X線切替リセット
        private void cmdXrayChangeReset_Click(object sender, EventArgs e)
        {
            string theCommand1;
            string theCommand2;

            if (!modSeqComm.CheckFCD2(ScanCorrect.GVal_Fcd))
            {
                return;
            }

            //機構部動作が可能かチェック（上記を関数化）
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if ((!modMechaControl.IsOkMechaMove()))
                return;

            //動作可否判定
            //If Not IsSwitchDet() Then Exit Sub
            //v19.50 エラーメッセージを表示するようにした by長野 2014/02/24
            if (!mod2ndXray.IsChangeXray())
            {
                //Interaction.MsgBox(CT30K.My.Resources.str17502, MsgBoxStyle.OkOnly);
                MessageBox.Show(CTResources.LoadResString(17502), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.None);
            }

            if (!mod2ndXray.IsAllCloseFrm)
 
            //画面操作禁止
            this.Enabled = false;
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            //タッチパネル操作禁止
            modSeqComm.SeqBitWrite("PanelInhibit", true);

            theCommand1 = "MicroFPDSet";
            theCommand2 = "MicroTableYSet";

            //原点復帰運転
            //modSeqComm.SeqBitWrite("MicroFPDSet", true);
            if (!modSeqComm.SeqBitWrite(theCommand1, true, false))
                return ;
            Application.DoEvents();

            if (!modSeqComm.SeqBitWrite(theCommand2, true, false))
                return ;
            Application.DoEvents();


            //監視タイマー起動
            TimerCount = 0;
            tmrWaitXray.Enabled = true;

        }

        private void tmrWaitXray_Tick(object sender, EventArgs e)
        {
            //X線切替原点復帰 v23.10 by 長野 2015/09/28
            if (mod2ndXray.IsChangingXray && (TimerCount < 30))
            {
                TimerCount = TimerCount + 1;
                return;
            }
            //タイマー解除
            tmrWaitXray.Enabled = false;

            //Rev23.20 メカオールリセット中は、タイマーを抜けてフォームのenableは戻さない by長野 2016/01/16
            if (mechaAllResetExFlg == false)
            {
                //マウスポインタを元に戻す
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                //フォームを使用可にする
                this.Enabled = true;

                //タッチパネル操作禁止
                modSeqComm.SeqBitWrite("PanelInhibit", false);
            }
        }
        //*************************************************************************************************
        //機　　能： 検出器システムリセットボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 各コマンドボタンのタグに動作指令があらかじめ埋め込まれている
        //
        //履　　歴： V23.20  15/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void cmdDetSystemReset_Click(object sender, EventArgs e)
        {
            if (sender as Button == null) return;

            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック（上記を関数化）
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if ((!modMechaControl.IsOkMechaMove(true)))
                return;

            //マウスポインタを砂時計にする
            Cursor.Current = Cursors.WaitCursor;

            //フォームを使用不可にする
            this.Enabled = false;

            //原点復帰
            modSeqComm.SeqBitWrite(Convert.ToString(cmdDetSystemReset.Tag), true);

            //タッチパネル操作禁止
            modSeqComm.SeqBitWrite("PanelInhibit", true);

            modCT30K.PauseForDoEvents(0.5F);            //v18.00追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //監視タイマー起動
            TimerCount = 0;
            tmrWait.Enabled = true;
        }
 
        private void cmdPosExec_Click(object sender, EventArgs e)
        {
            bool error_sts = false;
    
            //追加2014/10/07hata_v19.51反映
            if (!modMechaControl.IsOkMechaMove())
                goto ExitHandler;            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //エラー時の扱い
            try
            {
                //「実行」ボタンを使用不可にする
                cmdPosExec.Enabled = false;
                this.Enabled = false;
                //ctSliderV1.ControlLock = true;

                //マウスを砂時計にする
                Cursor.Current = Cursors.WaitCursor;

                //メカ動作
                error_sts = modSeqComm.MoveXrayAndHSC((int)cwnePos.Value);
                modCT30K.PauseForDoEvents(1);
            }
            catch
            {
                //そのまま抜ける
            }

        ExitHandler:    //追加2014/10/07hata_v19.51反映

            //状態を元に戻す
            this.Enabled = true;
            cmdPosExec.Enabled = true;
            // Add Start 2018/10/29 M.Oyama V26.40 Windows10対応
            cmdPosExec.UseVisualStyleBackColor = true;
            // Add End 2018/10/29
            Cursor.Current = Cursors.Default;

            cwnePos.Value = modLibrary.CorrectInRange(modSeqComm.MySeq.stsXrayCameraUDPosition, cwnePos.Minimum, cwnePos.Maximum);

            //エラーメッセージ
            if (error_sts != true)
            {
                MessageBox.Show(CTResources.LoadResString(27001), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 		
        }

        private void DispUpDownPointer(int? pos = null)
        {
            //pos現在位置
            if (pos == null) pos = modSeqComm.MySeq.stsXrayCameraUDPosition;
            ctSliderVScroll1.ArrowValue = (int)pos + CTSettings.iniValue.XrayAndHSCMaxPos;

            if (!modCT30K.CT30KSetup)
            {
                ctSliderVScroll1.ArrowVisible = false;
                return;
            }

            //ｽﾗｲﾀﾞが現在位置と違うとき/移動中のときのときにポインターを表示
            if ((ctSliderVScroll1.Value != (pos + CTSettings.iniValue.XrayAndHSCMaxPos)) || (modSeqComm.MySeq.stsXrayCameraUDBusy != true))
            {
                //設定中　現在位置がMaxかMinの外にあるとき
                ctSliderVScroll1.ArrowVisible = true;
            }
            else
            {
                //設定も移動も無し
                ctSliderVScroll1.ArrowVisible = false;
                mousecapture = ctSliderVScroll1.ArrowVisible;
            }
        }


        //追加2014/12/22hata_三角のポインターを描画
        private void picPointer_Paint(object sender, PaintEventArgs e)
        {
            //変更2015/02/12hata_スライダー新規
            ////三角のポインターを描画
            //Point[] points = new Point[3];
            //int midH = Convert.ToInt32(picPointer.Height / 2F) - 1;
            //points[0] = new Point(0, 0);
            //points[1] = new Point(picPointer.Width - 1, midH);
            //points[2] = new Point(0, picPointer.Height - 1);

            //e.Graphics.FillPolygon(Brushes.Yellow, points);
            //e.Graphics.DrawPolygon(Pens.Goldenrod, points);
        }

        private void cwnePos_ValueChanged(object sender, EventArgs e)
        {
            //2014/11/07hata キャストの修正
            //cwsldUpDown[0].Value = (int)cwnePos.Value;

            //変更2014/12/22hata_ｽﾗｲﾀﾞを1つにする           
            //cwsldUpDown[0].Value = Convert.ToInt32(cwnePos.Value);
            //インクリメント値で増減させる
            decimal rem = cwnePos.Value % cwnePos.Increment;

            if (cwnePos.Increment < (decimal)1)
            {
                cwnePos.Value = modLibrary.CorrectInRange(modSeqComm.MySeq.stsXrayCameraUDPosition, cwnePos.Minimum, cwnePos.Maximum);
            }
            else
            {
                if (rem > 0)
                {
                    //変更2015/02/02hata_Max/Min範囲のチェック
                    //cwnePos.Value = cwnePos.Value - rem + cwnePos.Increment;
                    cwnePos.Value = modLibrary.CorrectInRange(cwnePos.Value - rem + cwnePos.Increment, cwnePos.Minimum, cwnePos.Maximum);
                    //Rev21.00 incrementが半端な数値の場合も対応する by長野 2015/03/13
                    //return;
                }
            }

            if ((modCT30K.CT30KSetup) & (modSeqComm.MySeq.stsXrayCameraUDBusy != true))
            {
                //変更2015/01/30hata
                //if (ntbUpDown.Value != cwsldUpDown[0].Value)
                if (modSeqComm.MySeq.stsXrayCameraUDPosition != cwnePos.Value)
                {
                    //変更2015/02/12hata_スライダー新規
                    ctSliderVScroll1.ArrowVisible = true;

                }
                else
                {
                    //変更2015/02/12hata_スライダー新規
                    ctSliderVScroll1.ArrowVisible = false;
                }
            }

            ctSliderVScroll1.Value = cwnePos.Value + CTSettings.iniValue.XrayAndHSCMaxPos;
        }

        private void ctSliderVScroll1_ValueChanged(object sender, EventArgs e)
        {
            if (sender as CTSliderVScroll == null) return;

            if (this.ActiveControl == ctSliderVScroll1)
            {
                cwnePos.Value = modLibrary.CorrectInRange((decimal)ctSliderVScroll1.Value - CTSettings.iniValue.XrayAndHSCMaxPos, cwnePos.Minimum, cwnePos.Maximum);
            }
        }
    }
}
