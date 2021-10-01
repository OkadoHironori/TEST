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
	public partial class frmGainImageForMaint : Form
	{

		//private TransImageControl transImageCtrl;
        private BitmapImageControl BmpICtrl; 

		private RadioButton[] optScale = null;

		private static frmGainImageForMaint _Instance = null;

		public frmGainImageForMaint()
		{
			InitializeComponent();

			optScale = new RadioButton[] { optScale0, optScale1, optScale2 };

            this.SuspendLayout();

            BmpICtrl = new BitmapImageControl();
            BmpICtrl.MirrorOn = false;
            BmpICtrl.SetLTSize(LookupTableSize.LT12Bit);
            BmpICtrl.WindowLevel = 2048;
            BmpICtrl.WindowWidth = 4096;

            this.ctlTransImage.Size = new Size(1392, 1040);
            this.ctlTransImage.SizeX = 1392;
            this.ctlTransImage.SizeY = 520;
            this.ctlTransImage.Location = new Point(0, 0);
            this.ctlTransImage.MirrorOn = BmpICtrl.MirrorOn;

			this.ResumeLayout(false);

		}

		public static frmGainImageForMaint Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmGainImageForMaint();
				}

				return _Instance;
			}
		}

#region 【C#コントロールで代用】

		/// 
		/// TrackBar 最大値変更に伴い、最大値表示 Label を変更する
		/// 
		internal int cwsldWidthMaximum
		{
			set
			{
				cwsldWidth.Maximum = value;
                //削除2014/12/15hata
				//WDMax.Text = Convert.ToString(value);
			}
		}
		internal int cwsldLevelMaximum
		{
			set
			{
				cwsldLevel.Maximum = value;
                //削除2014/12/15hata
				//WLMax.Text = Convert.ToString(value);
			}
		} 

#endregion


		//*******************************************************************************
		//機　　能： ウィンドウレベルスライダー変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
#region 【C#コントロールで代用】
/*
		Private Sub cwsldLevel_PointerValueChanged(ByVal Pointer As Long, Value As Variant)
*/
#endregion
		private void cwsldLevel_ValueChanged(object sender, EventArgs e)
		{
			//値をラベルに表示
			lblLevel.Text = Convert.ToString(cwsldLevel.Value);

            //階調変換を実行
            //ctlTransImage.WindowLevel = cwsldLevel.Value;
            BmpICtrl.WindowLevel = cwsldLevel.Value;

            ctlTransImage.Picture = BmpICtrl.Picture;
            //ctlTransImage.Invalidate();
            ctlTransImage.Refresh();

        }


		//*******************************************************************************
		//機　　能： ウィンドウ幅スライダー変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
#region 【C#コントロールで代用】
/*
		Private Sub cwsldWidth_PointerValueChanged(ByVal Pointer As Long, Value As Variant)
*/
#endregion
		private void cwsldWidth_ValueChanged(object sender, EventArgs e)
		{
			//値をラベルに表示
			lblWidth.Text = Convert.ToString(cwsldWidth.Value);

            //階調変換を実行
            //ctlTransImage.WindowWidth = cwsldWidth.Value;
            BmpICtrl.WindowWidth = cwsldWidth.Value;

            //描画
            ctlTransImage.Picture = BmpICtrl.Picture;
            //ctlTransImage.Invalidate();
            ctlTransImage.Refresh();

        }


		//*******************************************************************************
		//機　　能： 倍率オプションボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] 型        1:１倍 2:４倍 3:16倍
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void optScale_CheckedChanged(object sender, EventArgs e)
		{
            //if (sender as RadioButton == null || ((RadioButton)sender).Checked == false) return;
            //int Index = Array.IndexOf(optScale, sender);
            //if (Index < 0) return;

            ////スクロールバーの最大値の調整
            ////cwsldWidth.Axis.Maximum = Choose(Index + 1, 256, 1024, 4096)   'v17.02削除 byやまおか 2010/07/05
            //switch (CTSettings.detectorParam.DetType)		//v17.02追加(ここから) byやまおか 2010/07/28
            //{
            //    case DetectorConstants.DetTypeII:
            //    case DetectorConstants.DetTypeHama:
            //        cwsldWidthMaximum = new int[]{ 256, 1024, 4096 }[Index];
            //        break;

            //    case DetectorConstants.DetTypePke:
            //        cwsldWidthMaximum = new int[]{ 4096, 16384, 65536 }[Index];
            //        break;
            //}						//v17.02追加(ここまで) byやまおか 2010/07/28

            //cwsldLevelMaximum = cwsldWidth.Maximum - 1;

            ////Index値を記憶
            //modCT30K.FimageBitIndex = Index;

            int Index = -1; // 選択したラジオボタンのインデックス番号
            for (int i = 0; i < optScale.Length; i++)
            {
                if (sender.Equals(optScale[i]))
                {
                    Index = i;
                    break;
                }
            }

            //スクロールバーの最大値の調整
            //cwsldWidth.Axis.Maximum = Choose(Index + 1, 256, 1024, 4096)   'v17.00削除 byやまおか 2010/01/20
            switch (CTSettings.detectorParam.DetType)
            {
                //v17.00追加(ここから) byやまおか 2010/01/20
                case DetectorConstants.DetTypeII:
                case DetectorConstants.DetTypeHama:
                    if (Index + 1 == 1)
                    {
                        cwsldWidth.Maximum = 256;
                    }
                    else if (Index + 1 == 2)
                    {
                        cwsldWidth.Maximum = 1024;
                    }
                    else if (Index + 1 == 3)
                    {
                        cwsldWidth.Maximum = 4096;
                    }
                    break;
                case DetectorConstants.DetTypePke:
                    //cwsldWidth.Axis.Maximum = Choose(Index + 1, 2048, 8192, 16384)  '変更　山本　2009-10-09
                    if (Index + 1 == 1)
                    {
                        cwsldWidth.Maximum = 4096;
                    }
                    else if (Index + 1 == 2)
                    {
                        cwsldWidth.Maximum = 16384;
                    }
                    else if (Index + 1 == 3)
                    {
                        cwsldWidth.Maximum = 65536;//v17.02変更 byやまおか 2010-06-14
                    }
                    break;
                default:
                    break;
            }
            //v17.00追加(ここまで) byやまおか 2010/01/20
            cwsldLevel.Maximum = cwsldWidth.Maximum - 1;

            //削除2014/12/15hata
            //WLMin.Text = Convert.ToString(cwsldLevel.Minimum);
            //WLMax.Text = Convert.ToString(cwsldLevel.Maximum);
            //WDMin.Text = Convert.ToString(cwsldWidth.Minimum);
            //WDMax.Text = Convert.ToString(cwsldWidth.Maximum);

            //Index値を記憶
            modCT30K.FimageBitIndex = Index;
            
        }


		//*******************************************************************************
		//機　　能： 「閉じる」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdClose_Click(object sender, EventArgs e)
		{
			//フォームをアンロード
			this.Close();
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
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void frmGainImageForMaint_Load(object sender, EventArgs e)
		{
			//リソースから文字列を取得し、各コントロールのキャプションにセット
			SetCaption();

			//各コントロールのサイズ・位置の設定
			InitControls();

			//倍率の設定
			modLibrary.SetOption(optScale, modCT30K.FimageBitIndex);

			//ウィンドウレベル・ウィンドウ幅のセット
			cwsldLevel.Value = frmScanControl.Instance.WindowLevel;
			cwsldWidth.Value = frmScanControl.Instance.WindowWidth;

#region 【C#コントロールで代用】    //上記の処理で ValueChanged イベントが発生するため コメントアウト
/*
			'上記の処理ではPointerValueChangedイベントは発生しないので以下の処理を行なう
			cwsldLevel_PointerValueChanged 0, cwsldLevel.Value
			cwsldWidth_PointerValueChanged 0, cwsldWidth.Value
*/
#endregion

			//変換対象となる画像の配列と制御するコントロールを登録
			BmpICtrl.SetImage(ScanCorrect.GAIN_IMAGE);
            ctlTransImage.Picture = BmpICtrl.Picture;
            //ctlTransImage.Invalidate();
            ctlTransImage.Refresh();

        
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
		//履　　歴： V7.00  03/08/25 (SI4)間々田     新規作成
		//*******************************************************************************
		private void SetCaption()
		{
			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);

			//optScale(0).Caption = GetResString(IDS_Times, " 1")     ' 4 倍 → 1 倍に変更 by 間々田 2005/01/07
			//optScale(1).Caption = GetResString(IDS_Times, " 4")     ' 8 倍 → 4 倍に変更 by 間々田 2005/01/07
			//optScale(2).Caption = GetResString(IDS_Times, "16")     '16 倍
			optScale0.Text = " 1 / 16";		//v17.10変更 byやまおか 2010/08/26
			optScale1.Text = " 1 / 4";		//v17.10変更 byやまおか 2010/08/26
			optScale2.Text = " 1 / 1";		//v17.10変更 byやまおか 2010/08/26
		}


		//*******************************************************************************
		//機　　能： 各コントロールのサイズ・位置の設定
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V7.00  03/08/25 (SI4)間々田     新規作成
		//*******************************************************************************
		private void InitControls()
		{
			//透視画像表示コントロール
            ctlTransImage.SizeX = CTSettings.detectorParam.h_size;
            ctlTransImage.SizeY = CTSettings.detectorParam.v_size;
            //2014/11/06hata キャストの修正
            ctlTransImage.Width = Convert.ToInt32(CTSettings.detectorParam.h_size / CTSettings.detectorParam.fphm);
            ctlTransImage.Height = Convert.ToInt32(CTSettings.detectorParam.v_size / CTSettings.detectorParam.fpvm);

			//フォーム
			this.Width = (ctlTransImage.Width + fraChange.Width + 4);
			this.Height = (ctlTransImage.Height + 25);
			this.Location = new Point(27, 27);

			//階調変更コントロールフレームの位置
			fraChange.Left = ctlTransImage.Width;
			fraChange.Height = ctlTransImage.Height;

			//「閉じる」ボタンの位置
			cmdClose.Top = (fraChange.Height - cmdClose.Height - 27);


			//倍率のフレームの表示・非表示
            fraScale.Visible = (CTSettings.scancondpar.Data.fimage_bit == 2) && CTSettings.detectorParam.Use_FlatPanel;
		}


	}
}
