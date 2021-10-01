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

	public partial class frmSpeed : Form
	{
		//イベント宣言
		public class frmSpeedEventArgs : EventArgs
		{
			private modMechaControl.MechaConstants _theMecha;
			private double _Value;

			public modMechaControl.MechaConstants theMecha
			{
				get { return _theMecha; }
				set { _theMecha = value; }
			}			
			public double Value
							{
				get { return _Value; }
				set { _Value = value; }
			}

			public frmSpeedEventArgs()
			{
 				// Nothing
			}
			public frmSpeedEventArgs(modMechaControl.MechaConstants theMecha, double Value)
			{
				_theMecha = theMecha;
				_Value = Value;
			}
		}
		public delegate void frmSpeedEventHandler(object sender, frmSpeedEventArgs e);
		public event frmSpeedEventHandler ValueChanged;

		//対象メカ
		modMechaControl.MechaConstants myTarget;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
		Dim theForm  As Form            'v15.10追加 byやまおか 2009/11/09
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private bool FlgPanelOff_OK = false;	//v15.10追加 byやまおか 2009/11/09

		// cwknobSpeed の増減の刻み量
		private static decimal cwknobSpeedInterval = 0.1M;		// 【C#コントロールで代用】

		private static frmSpeed _Instance = null;

		public frmSpeed()
		{
			InitializeComponent();
		}

		public static frmSpeed Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmSpeed();
				}

				return _Instance;
			}
		}


		//*******************************************************************************
		//機　　能： ダイアログ処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*******************************************************************************
		public void Dialog(modMechaControl.MechaConstants theTarget, string Title, double Value, double MinValue, double MaxValue, string unitString = "mm/s")
		{
			//対象メカ
			myTarget = theTarget;

			//タイトル
			this.Text = Title;

#region 【C#コントロールで代用】
/*
			'ノブコントロールに対する設定
			With cwknobSpeed

				'最小値・最大値の設定
				.Axis.SetMinMax MinValue, MaxValue

				'現在値
				.Value = Value

			End With
*/
#endregion 【C#コントロールで代用】

			//ノブコントロールに対する設定
			//最小値・最大値の設定
			//現在値
            //変更2014/11/28hata_v19.51_dnet
            //cwknobSpeed_SetValues((decimal)MinValue, (decimal)MaxValue, (decimal)Value);
            cwknobSpeed_SetValues(MinValue, MaxValue, Value);

			//単位
			lblSpeedUni.Text = unitString;

			var _frmCTMenu = frmCTMenu.Instance;
			var _frmMechaControl = frmMechaControl.Instance; 

			////表示位置調整(コンボボックスのすぐ横に表示) 'v15.10追加 byやまおか 2009/11/04
			//this.SetBounds(x,y,w,height);
            this.Location = new Point(_frmCTMenu.Left + _frmMechaControl.cboSpeed[(int)theTarget].Left + _frmMechaControl.cboSpeed[(int)theTarget].Width + 33,
                                      _frmCTMenu.Top + _frmMechaControl.cboSpeed[(int)theTarget].Top + 28);

			//'モーダル表示              'v15.10削除 byやまおか 2009/11/02
			//Me.Show vbModal
			//モーダルをやめる(複数表示) 'v15.10追加 byやまおか 2009/11/04
            this.Show(_frmCTMenu);
        }

        //追加2014/11/28hata_v19.51_dnet
        //*******************************************************************************
        //機　　能： 速度ノブのCapture変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*******************************************************************************
        private void cwknobSpeed_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (!Capture)
            {
                //captureが開放されている時に値更新
                cwknobSpeed_ValueChanged(cwknobSpeed, EventArgs.Empty);
            }
        }
        
		//*******************************************************************************
		//機　　能： 速度ノブ変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*******************************************************************************
		private void cwknobSpeed_ValueChanged(object sender, EventArgs e)		// 【C#コントロールで代用】
		{
			//ノブコントロールで設定した値を表示
            //Rev22.00 変更 by長野 2015/09/04
            if (myTarget == modMechaControl.MechaConstants.MechaTiltAndRot_Tilt)
            {
                lblSpeed.Text = cwknobSpeed_GetValue().ToString("0.00");
            }
            else
            {
                lblSpeed.Text = cwknobSpeed_GetValue().ToString("0.0");
            }

            //追加2014/11/28hata_v19.51_dnet
            if (cwknobSpeed.Capture)
            {
                //capture中は値のみ更新
                return;
            }
            
            //表示を変更する           'v15.10追加 byやまおか 2009/11/04
            switch (myTarget)
            {
                case modMechaControl.MechaConstants.MechaTableRotate:		//回転
                    float.TryParse(lblSpeed.Text, out CTSettings.mechapara.Data.rot_speed[(int)frmMechaControl.SpeedConstants.speedmanual]);
                    break;
                case modMechaControl.MechaConstants.MechaTableUpDown:		//高さ
                    float.TryParse(lblSpeed.Text, out CTSettings.mechapara.Data.ud_speed[(int)frmMechaControl.SpeedConstants.speedmanual]);
                    break;
                case modMechaControl.MechaConstants.MechaTableX:			//FCD
                    float.TryParse(lblSpeed.Text, out CTSettings.mechapara.Data.fcd_speed[(int)frmMechaControl.SpeedConstants.speedmanual]);
                    break;
                case modMechaControl.MechaConstants.MechaII:				//FDD
                    float.TryParse(lblSpeed.Text, out CTSettings.mechapara.Data.fdd_speed[(int)frmMechaControl.SpeedConstants.speedmanual]);
                    break;
                case modMechaControl.MechaConstants.MechaTableY:			//Ｙ軸（従来のＸ軸）
                    float.TryParse(lblSpeed.Text, out CTSettings.mechapara.Data.tbl_y_speed[(int)frmMechaControl.SpeedConstants.speedmanual]);
                    break;
                case modMechaControl.MechaConstants.MechaFTableX:			//微調テーブル
                    float.TryParse(lblSpeed.Text, out CTSettings.mechapara.Data.fine_tbl_speed[(int)frmMechaControl.SpeedConstants.speedmanual]);
                    break;
                case modMechaControl.MechaConstants.MechaXrayRotate:		//Ｘ線管回転速度
                    float.TryParse(lblSpeed.Text, out CTSettings.mechapara.Data.xray_rot_speed[(int)frmMechaControl.SpeedConstants.speedmanual]);
                    break;
                case modMechaControl.MechaConstants.MechaXrayX:				//Ｘ線管Ｘ軸
                    float.TryParse(lblSpeed.Text, out CTSettings.mechapara.Data.xray_x_speed[(int)frmMechaControl.SpeedConstants.speedmanual]);
                    break;
                case modMechaControl.MechaConstants.MechaXrayY:				//Ｘ線管Ｙ軸
                    float.TryParse(lblSpeed.Text, out CTSettings.mechapara.Data.xray_y_speed[(int)frmMechaControl.SpeedConstants.speedmanual]);
                    break;
                case modMechaControl.MechaConstants.MechaTiltAndRot_Rot:	//Rev22.00 回転傾斜テーブル 回転 by長野 2015/09/02
                    float.TryParse(lblSpeed.Text, out CTSettings.mechapara.Data.tilt_and_rot_rot_speed[(int)frmMechaControl.SpeedConstants.speedmanual]);
                    break;
                case modMechaControl.MechaConstants.MechaTiltAndRot_Tilt:				//Rev22.00 回転傾斜テーブル 傾斜 by長野 2015/09/02
                    float.TryParse(lblSpeed.Text, out CTSettings.mechapara.Data.tilt_and_rot_tilt_speed[(int)frmMechaControl.SpeedConstants.speedmanual]);
                    break;
            }

            //値を変更する           'v15.10追加 byやまおか 2009/11/09
			frmMechaControl.Instance.mySpeedForm_ValueChanged(myTarget, (float)cwknobSpeed_GetValue());

            
            //mechaparaを書き換える  'v15.10追加 byやまおか 2009/11/04
            //modMechapara.PutMechapara(ref modMechapara.mechapara);
            CTSettings.mechapara.Put(CTSettings.mechapara.Data);

			//値が変更されたことを通知するためにイベント生成
			if(ValueChanged != null)
			{
				ValueChanged(this, new frmSpeedEventArgs(myTarget, (double)cwknobSpeed_GetValue()));
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
		private void frmSpeed_Load(object sender, EventArgs e)
		{
			//タッチパネル操作禁止
			//SeqBitWrite "PanelInhibit", True   'v15.10削除 byやまおか 2009/12/03

			//v17.60 SetCaptionを追加　by長野 2011/05/30
			SetCaption();
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
		private void frmSpeed_FormClosing(object sender, FormClosingEventArgs e)
		{
			//v15.10削除(cwknobSpeed_PointerValueChangedへ移動) byやまおか 2009/11/02
			//'mechaparaを書き換える
			//With mechapara
			//    Select Case myTarget
			//        Case MechaTableRotate:  .rot_speed(SpeedManual) = Val(lblSpeed.Caption)         '回転
			//        Case MechaTableUpDown:  .ud_speed(SpeedManual) = Val(lblSpeed.Caption)          '高さ
			//        Case MechaTableX:       .fcd_speed(SpeedManual) = Val(lblSpeed.Caption)         'FCD
			//        Case MechaII:           .fdd_speed(SpeedManual) = Val(lblSpeed.Caption)         'FDD
			//        Case MechaTableY:       .tbl_y_speed(SpeedManual) = Val(lblSpeed.Caption)       'Ｙ軸（従来のＸ軸）
			//        Case MechaFTableX:      .fine_tbl_speed(SpeedManual) = Val(lblSpeed.Caption)    '微調テーブル
			//        Case MechaXrayRotate:   .xray_rot_speed(SpeedManual) = Val(lblSpeed.Caption)    'Ｘ線管回転速度
			//        Case MechaXrayX:        .xray_x_speed(SpeedManual) = Val(lblSpeed.Caption)      'Ｘ線管Ｘ軸
			//        Case MechaXrayY:        .xray_y_speed(SpeedManual) = Val(lblSpeed.Caption)      'Ｘ線管Ｙ軸
			//    End Select
			//End With
			//
			//PutMechapara mechapara
			//

			//v15.10下記変更 byやまおか 2009/11/09
			//'タッチパネル操作禁止解除
			//SeqBitWrite "PanelInhibit", False

			//フォーム非表示     'v15.10追加 byやまおか 2009/11/09
			//変更2015/1/17hata_非表示のときにちらつくため
            //this.Hide();
            modCT30K.FormHide(this);

			//frmSpeedダイアログが他に表示されているかチェック   'v15.10追加 byやまおか 2009/11/09
			foreach (Form theForm in Application.OpenForms)
			{
				//表示されていれば、操作禁止解除しない
				if ((theForm.Name == this.Name) && theForm.Visible)
				{
					FlgPanelOff_OK = false;
				}
				//表示されていなければ、操作禁止解除する
				else
				{
					FlgPanelOff_OK = true;
				}
			}

			//操作禁止解除してよければ   'v15.10追加 byやまおか 2009/11/09
			if (FlgPanelOff_OK)
			{
				//タッチパネル操作禁止解除
				modSeqComm.SeqBitWrite("PanelInhibit", false);
			}
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
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		private void SetCaption()
		{
			lblHeader.Text = StringTable.LoadResStringWithColon(12179);		//速度：
		}



#region 【C#コントロールで代用】

		// TrackBar の値を実数に戻して取得する
		public decimal cwknobSpeed_GetValue()
		{
			return (cwknobSpeed.Value * cwknobSpeedInterval);
		}
		public decimal cwknobSpeed_GetMin()
		{
			return (cwknobSpeed.Minimum * cwknobSpeedInterval);
		}
		public decimal cwknobSpeed_GetMax()
		{
			return (cwknobSpeed.Maximum * cwknobSpeedInterval);
		}

		// TrackBar への設定値を整数に変換して設定する。
		//private void cwknobSpeed_SetValues(decimal? trackBarMin = null, decimal? trackBarMax = null, decimal? trackBarValue = null)
        private void cwknobSpeed_SetValues(double? trackBarMin = null, double? trackBarMax = null, double? trackBarValue = null)
		{
			// 設定する値
            //変更2014/11/28hata_v19.51_dnet
            //decimal min = (trackBarMin ?? cwknobSpeed_GetMin());
            //decimal max = (trackBarMax ?? cwknobSpeed_GetMax());
            //decimal val = (trackBarValue ?? 0.00M);
            double min = (trackBarMin ?? (double)cwknobSpeed_GetMin());
            double max = (trackBarMax ?? (double)cwknobSpeed_GetMax());
            double value = (trackBarValue ?? 0.00D);

            //// 小数点以下の桁数を取得する
            //int length = Math.Max(Math.Max(GetScale(min), GetScale(max)), GetScale(value));

            //// 小数点以下の桁数から増減の刻みを取得。刻みは最も粗くて 0.1
            //cwknobSpeedInterval = (decimal)(length > 0 ? Math.Pow(0.1, length) : 0.1);

            //有効は1桁目なので、2桁で丸める
            min = Math.Round(min, 2);
            max = Math.Round(max, 2);
            value = Math.Round(value, 2);

            //Rev22.300 変更 by長野 2015/09/04
            if (myTarget == modMechaControl.MechaConstants.MechaTiltAndRot_Rot || myTarget == modMechaControl.MechaConstants.MechaTiltAndRot_Tilt)
            {
                cwknobSpeedInterval = 0.01M;
            }
            else
            {
                cwknobSpeedInterval = 0.1M;
            }

			// 値を整数に変換して TrackBar に設定
            //変更2015/01/21hata_少数点以下が切られるため
            //cwknobSpeed.Minimum = (int)(min / (double)cwknobSpeedInterval);
            //cwknobSpeed.Maximum = (int)(max / (double)cwknobSpeedInterval);
            cwknobSpeed.Minimum = (int)Math.Round(min / (double)cwknobSpeedInterval, MidpointRounding.AwayFromZero);
            cwknobSpeed.Maximum = (int)Math.Round(max / (double)cwknobSpeedInterval, MidpointRounding.AwayFromZero);
            //変更2014/11/28hata_v19.51_dnet
            //if (min < value && value < max)		// TrackBar の有効範囲を超える値は設定しない
            //{
            //    cwknobSpeed.Value = (int)(value / cwknobSpeedInterval);
            //}
            if (value > 0D)
            {
                if (min > value)		// TrackBar の有効範囲を超える値は設定しない
                {
                    cwknobSpeed.Value = cwknobSpeed.Minimum;
                }
                else if (max < value)
                {
                    cwknobSpeed.Value = cwknobSpeed.Maximum;
                }
                else
                {   // TrackBar の有効範囲を超える値は設定しない
                    //変更2015/01/21hata_少数点以下が切られるため
                    //cwknobSpeed.Value = (int)(value / (double)cwknobSpeedInterval);
                    cwknobSpeed.Value = (int)Math.Round(value / (double)cwknobSpeedInterval, MidpointRounding.AwayFromZero);
                }
                cwknobSpeed_ValueChanged(cwknobSpeed, EventArgs.Empty);
            }
			// TrackBar の最大・最小値 Label を更新
            //Rev22.00 変更 by長野 2015/09/04
            //cwknobSpeedMinLabel.Text = min.ToString("0.0");
            //cwknobSpeedMaxLabel.Text = max.ToString("0.0");
            if (myTarget == modMechaControl.MechaConstants.MechaTiltAndRot_Tilt)
            {
                cwknobSpeedMinLabel.Text = min.ToString("0.00");
                cwknobSpeedMaxLabel.Text = max.ToString("0.00");
            }
            else
            {
                cwknobSpeedMinLabel.Text = min.ToString("0.0");
                cwknobSpeedMaxLabel.Text = max.ToString("0.0");
            }
		}

		private int GetScale(decimal value)
		{
			string separator = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
			string decimalString = value.ToString().TrimEnd('0');

			int decimalIndex = decimalString.IndexOf(separator);

			if (decimalIndex == -1) return 0;

			return decimalString.Replace(separator, string.Empty).Substring(decimalIndex).Length;
		}

#endregion 【C#コントロールで代用】
        
    }
}
