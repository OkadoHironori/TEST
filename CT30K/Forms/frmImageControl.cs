using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
//
using CT30K.Common;
using CTAPI;
using TransImage;
//using CT30K.Controls;
//using CT30K.Modules;
using CT30K.Properties;

namespace CT30K
{
    public partial class frmImageControl : FixedForm
    //public partial class frmImageControl : Form 
	{
		//階調変換の「元に戻す」用記憶領域
		private List<string> colContrast = new List<string>();

		private struct CONTRAST
		{
			public int WindowLevel;
			public int WindowWidth;
			public float GAMMA;		//v19.00 追加 by長野 2012/02/22		
		}

		private int ContrastCount;
		private CONTRAST[] ContrastList = new CONTRAST[11];		//TODO (1 TO 10)
		private CONTRAST LastContrast;

		//イベント制御フラグ
		private bool byEvent;

		//
		// cwneGamma_ValueChanged イベントで使用する static フィールド
		//
		private static bool cwneGamma_ValueChanged_BUSYNOW = false;			//状態(True:実行中,False:停止中)

		//
		// cwneWindowLevel_ValueChanged イベントで使用する static フィールド
		//
		private static bool cwneWindowLevel_ValueChanged_BUSYNOW = false;	//状態(True:実行中,False:停止中)

		//
		// cwneWindowWidth_ValueChanged イベントで使用する static フィールド
		//
		private static bool cwneWindowWidth_ValueChanged_BUSYNOW = false;	//状態(True:実行中,False:停止中)

		/// <summary>
		/// filSliceName ListBox に表示するファイル一覧のディレクトリパス
		/// </summary>
		private static string _filSliceNamePath = Application.StartupPath;			// 【C#コントロールで代用】

		private Button[] cmdSliceNextBack = null;

		private static frmImageControl _Instance = null;

        private char presskye = (char)0;  //Keypressの値          //追加2014/12/19hata


		public frmImageControl()
		{
			InitializeComponent();

			cmdSliceNextBack = new Button[] { cmdSliceNextBack1, cmdSliceNextBack2 };

            //FilSliceNamePath = Application.StartupPath;		// 【C#コントロールで代用】
            //削除2014/12/15hata
            //SetTrackBarMaxMinLabel();						// 【C#コントロールで代用】
		}

        // staticを抜いておく
		//internal static string FilSliceNamePath					// 【C#コントロールで代用】
		internal string FilSliceNamePath					// 【C#コントロールで代用】
		{
			get { return frmImageControl._filSliceNamePath; }
			set
			{
                int TargetNum = 0;
                int i = 0;

				if (!Directory.Exists(value)) return;

				frmImageControl._filSliceNamePath = value;

                //Rev20.00 追加 by長野 2014/12/04
                filSliceName.Items.Clear();

               //FileInfo[] finfo = new DirectoryInfo(Application.StartupPath).GetFiles("*.img");
                FileInfo[] finfo = new DirectoryInfo(_filSliceNamePath).GetFiles("*.img");
                foreach (FileInfo fileInfo in finfo)
                {
					if ((fileInfo.Attributes & FileAttributes.System) == FileAttributes.System) continue;
                    //frmImageControl.Instance.filSliceName.Items.Add(fileInfo.ToString());
                    filSliceName.Items.Add(fileInfo.ToString());
                }

                //Rev20.00 追加 by長野 2015/03/11
                //スライス変更時処理
                for (i = 0; i <= filSliceName.Items.Count - 1; i++)
                {
                    if (Path.Combine(FilSliceNamePath, Convert.ToString(filSliceName.Items[i])).ToLowerInvariant() == frmScanImage.Instance.Target.ToLowerInvariant())
                    {
                        TargetNum = i + 1;
                        break;
                    }
                }

                //フォルダ移動前と移動先の枚数によってはMaximumと同時にValueも代わるため、再セット処理を行う by長野 2015/03/10
                if (sldImageSearch.Maximum > filSliceName.Items.Count && sldImageSearch.Value > filSliceName.Items.Count && TargetNum != -1)
                {
                    sldImageSearch.Value = TargetNum;
                    sldImageSearch.Maximum = filSliceName.Items.Count;
                }
                //frmImageControl.Instance.filSliceName_PathChange();
                filSliceName_PathChange();
               
			}
		}

		public static frmImageControl Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmImageControl();
				}

				return _Instance;
			}
		}


		//*******************************************************************************
		//機　　能： ガンマ補正値テキストボックス変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v19.00  2012/02/21 (検S1)長野  新規作成
		//*******************************************************************************
		private void cwneGamma_ValueChanged(object sender, EventArgs e)
		{
			if (cwneGamma_ValueChanged_BUSYNOW) return;
			cwneGamma_ValueChanged_BUSYNOW = true;

			//以降は２重呼び出しで処理させない
			if (CheckContrastAvailable())
			{
				//ガンマ変更処理
				frmScanImage.Instance.GAMMA = (float)cwneGamma.Value;
				if (this.ActiveControl == cwneGamma) RestoreContrast();
			}
			else
			{
				//階調変換できない場合元に戻す
				//sldWindowWidth.Value = PreviousValue
			}

			//元の状態に戻す
			cwneGamma_ValueChanged_BUSYNOW = false;
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
		private void frmImageControl_Load(object sender, EventArgs e)
		{
			//'フォームの表示位置：スキャン条件画面の真下    'v15.10削除 2009/07/29 byやまおか
			//With frmScanControl
			//    Me.Move .Left, .Top + .Height, FmControlWidth
			//End With

            //Instancを作成してから出ないとエラーするためLoadで行う　→ｺﾝｽﾄﾗｸﾀで実施
            //FilSliceNamePath = Application.StartupPath;		// 【C#コントロールで代用】

			//コントロールの初期化   'v15.10追加 2009/07/29 byやまおか
			InitControls();

			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);


			//v17.60　英語用レイアウト調整 by長野 2011/05/25
			if (modCT30K.IsEnglish == true)
			{
				EnglishAdjustLayout();
			}

			//イベントを許可
			byEvent = true;

			//modDispinf.GetDispinf(ref CTSettings.dispinf.Data);
            CTSettings.dispinf.Load();

            //初期化しておく
            optRangeBit0.Checked = true;    //追加2014/09/18(検S1)hata

            sldWindowLevel.Value = CTSettings.dispinf.Data.level;
            sldWindowWidth.Value = CTSettings.dispinf.Data.width;
            //変更2015/02/02hata_Max/Min範囲のチェック
			//cwneWindowLevel.Value = CTSettings.dispinf.Data.level;
            //cwneWindowWidth.Value = CTSettings.dispinf.Data.width;
			//cwneGamma.Value = (decimal)CTSettings.dispinf.Data.Gamma;		//v19.00 ガンマの初期値を追加(初期値は1.0にする) by長野 2012/02/22
            cwneWindowLevel.Value = modLibrary.CorrectInRange(CTSettings.dispinf.Data.level, cwneWindowLevel.Minimum, cwneWindowLevel.Maximum);
            cwneWindowWidth.Value = modLibrary.CorrectInRange(CTSettings.dispinf.Data.width, cwneWindowWidth.Minimum, cwneWindowWidth.Maximum);
            cwneGamma.Value = modLibrary.CorrectInRange((decimal)CTSettings.dispinf.Data.Gamma, cwneGamma.Minimum, cwneGamma.Maximum);		//v19.00 ガンマの初期値を追加(初期値は1.0にする) by長野 2012/02/22

            //追加2015/01/08hata_dNet
            txtWL.Text = cwneWindowLevel.Value.ToString();
            txtWW.Text = cwneWindowWidth.Value.ToString();


			//階調記憶数
			ContrastCount = 0;
			SaveLastContrast();

        }


		//*************************************************************************************************
		//機　　能： 各コントロールの位置・サイズ等の初期化
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.0  09/07/29   やまおか      新規作成
		//*************************************************************************************************
		private void InitControls()
		{
			//フォームの表示位置：スキャン条件画面の真下
			
			//メカ制御ありなら、そのまま表示
			if (CTSettings.scaninh.Data.mechacontrol == 0)
			{
				this.SetBounds(frmScanControl.Instance.Left, frmScanControl.Instance.Top + frmScanControl.Instance.Height, modCT30K.FmControlWidth, this.Height);
			}
			//メカ制御なし(2ndPC)なら、フォームを幅狭縦長にし、(上)階調変換(下)画像サーチ
			else
			{
				this.SetBounds(frmScanControl.Instance.Left, frmScanControl.Instance.Top + frmScanControl.Instance.Height, modCT30K.FmControlWidth2nd, this.Height * 2);
				this.fraImageSearch.Location = new Point(this.fraContrast.Left, this.fraContrast.Height + 13);
			}

            //追加2014/06/18(検S1)hata
            lblImageSearchMin.Visible = false;
            lblImageSearchMax.Visible = false;
            lblImageSearchMidL.Visible = false;
            lblImageSearchMidR.Visible = false;
            lineShapeMax.Visible = false;
            lineShapeMin.Visible = false;
            lineShapeMidL.Visible = false;
            lineShapeMidR.Visible = false;

            //追加2015/01/08hata_dNet
            //UpDownボタンをTextBoxの裏に隠す
            cwneWindowLevel.Location = txtWL.Location;
            cwneWindowWidth.Location = txtWW.Location;
  
        }


		//*******************************************************************************
		//機　　能： スライダーのレンジ幅の切り替えオプションボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
        //private void optRangeBit_Click(object sender, EventArgs e)
        //{
        //    if (sender as RadioButton == null) return;

        //    int bit = 0;
        //    bit = (sender == optRangeBit1) ? 14 : 12;

        //    //ウィンドウレベル・スライダコントロールの最小値・最大値の設定
        //    sldWindowLevel.Minimum = (int)(-Math.Pow(2, (bit - 1)));
        //    sldWindowLevel.Maximum = (int)(Math.Pow(2, (bit - 1)) - 1);

        //    //ウィンドウ幅・スライダコントロールの最大値の設定
        //    sldWindowWidth.Maximum = (int)(Math.Pow(2, bit));

        //    SetTrackBarMaxMinLabel();					// 【C#コントロールで代用】
        //}
        private void optRangeBit_CheckedChanged(object sender, EventArgs e)
        {
            //変更2015/01/21hata
            //if (sender as RadioButton == null) return;
            if (sender as RadioButton == null || ((RadioButton)sender).Checked == false) return;

            int bit = 0;
            bit = (sender == optRangeBit1) ? 14 : 12;

            ////Rev20.01 下から移動 by長野 2015/06/09
            //追加2015/01/21hata
            if (bit == 12)
            {
                sldWindowLevel.SmallChange = 1;
                sldWindowWidth.SmallChange = 1;
                sldWindowLevel.LargeChange = 10;
                sldWindowWidth.LargeChange = 10;
            }
            else
            {
                sldWindowLevel.SmallChange = 5;
                sldWindowWidth.SmallChange = 5;
                sldWindowLevel.LargeChange = 50;
                sldWindowWidth.LargeChange = 50;
            }

            //ウィンドウレベル・スライダコントロールの最小値・最大値の設定
            sldWindowLevel.Minimum = (int)(-Math.Pow(2, (bit - 1)));
            sldWindowLevel.Maximum = (int)(Math.Pow(2, (bit - 1)) - 1);
            //Rev20.01 変更 by長野 2015/06/09
            //sldWindowLevel.Maximum = (int)(Math.Pow(2, (bit - 1)) - 1) + sldWindowLevel.LargeChange - 1;

            //ウィンドウ幅・スライダコントロールの最大値の設定
            sldWindowWidth.Maximum = (int)(Math.Pow(2, bit));
            //sldWindowWidth.Maximum = (int)(Math.Pow(2, bit)) + sldWindowWidth.LargeChange - 1;

            //追加2014/09/18(検S1)hata
            //最大/最小値を合わせておく
            cwneWindowLevel.Minimum = sldWindowLevel.Minimum;
            cwneWindowLevel.Maximum = sldWindowLevel.Maximum;
            cwneWindowWidth.Minimum = sldWindowWidth.Minimum;
            cwneWindowWidth.Maximum = sldWindowWidth.Maximum;

            //Rev20.01 上に移動する by長野 2015/06/09
            //追加2015/01/21hata
            //if (bit == 12)
            //{
            //    sldWindowLevel.SmallChange = 1;
            //    sldWindowWidth.SmallChange = 1;
            //    sldWindowLevel.LargeChange = 10;
            //    sldWindowWidth.LargeChange = 10;
            //}
            //else
            //{
            //    sldWindowLevel.SmallChange = 5;
            //    sldWindowWidth.SmallChange = 5;
            //    sldWindowLevel.LargeChange = 50;
            //    sldWindowWidth.LargeChange = 50;
            //}

            //削除2014/12/15hata
            //SetTrackBarMaxMinLabel();					// 【C#コントロールで代用
        }

		//*******************************************************************************
		//機　　能： 変更前の階調値の保存
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		private void SaveLastContrast()
		{
			LastContrast.WindowLevel = (int)cwneWindowLevel.Value;
			LastContrast.WindowWidth = (int)cwneWindowWidth.Value;
			LastContrast.GAMMA = (float)cwneGamma.Value;
		}


		//*******************************************************************************
		//機　　能： ウィンドウレベルスライダー変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
        // 【C#コントロールで代用】
		private void sldWindowLevel_PointerValueChanged(object sender, EventArgs e)
		{
			cwneWindowLevel.Value = sldWindowLevel.Value;
		}
        //削除2014/12/15hata
        //private void sldWindowLevel_PointerValueCommitted(object sender, MouseEventArgs e)	//TODO MouseUpイベントにて実装
        //{
        //    if (this.ActiveControl == sldWindowLevel) RestoreContrast();
        //}
        //追加2014/12/15hata
        private void sldWindowLevel_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (!sldWindowLevel.Capture)
            {
                //変更2015/01/21hata
                //if (this.ActiveControl == sldWindowLevel) RestoreContrast();
                RestoreContrast();
            }
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
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
        // 【C#コントロールで代用】
		private void sldWindowWidth_PointerValueChanged(object sender, EventArgs e)
		{
            cwneWindowWidth.Value = sldWindowWidth.Value;
		}
        //private void sldWindowWidth_PointerValueCommitted(object sender, MouseEventArgs e)	//TODO MouseUpイベントにて実装
        //{
        //    if (this.ActiveControl == sldWindowWidth) RestoreContrast();
        //}
        //追加2014/12/15hata
        private void sldWindowWidth_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (!sldWindowWidth.Capture)
            {
                //変更2015/01/21hata
                //if (this.ActiveControl == sldWindowWidth) RestoreContrast();
                RestoreContrast();
            }
        }

		//*******************************************************************************
		//機　　能： ウィンドウレベルテキストボックス変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		private void cwneWindowLevel_ValueChanged(object sender, EventArgs e)
		{
            //追加2014/12/15hata
            txtWL.Text = cwneWindowLevel.Value.ToString();
            
            //スライダーコントロールに値を反映させる
			if (this.ActiveControl != sldWindowLevel) sldWindowLevel.Value = (int)cwneWindowLevel.Value;

			if (cwneWindowLevel_ValueChanged_BUSYNOW) return;
			cwneWindowLevel_ValueChanged_BUSYNOW = true;

			//以降は２重呼び出しで処理させない
			if (CheckContrastAvailable())
			{
				//階調変更処理
				frmScanImage.Instance.WindowLevel = (int)cwneWindowLevel.Value;
                //変更2015/01/21hata
				//if (this.ActiveControl == cwneWindowLevel) RestoreContrast();
                if((this.ActiveControl == cwneWindowLevel) | (this.ActiveControl ==  txtWL) ) RestoreContrast();
			}
			else
			{
				//階調変換できない場合元に戻す
				//sldWindowLevel.Value = PreviousValue
			}

			//元の状態に戻す
			cwneWindowLevel_ValueChanged_BUSYNOW = false;
		}


		//*******************************************************************************
		//機　　能： ウィンドウ幅テキストボックス変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		private void cwneWindowWidth_ValueChanged(object sender, EventArgs e)
		{
            //追加2014/12/15hata
            txtWW.Text = cwneWindowWidth.Value.ToString();

            //スライダーコントロールに値を反映させる
			if (this.ActiveControl != sldWindowWidth) sldWindowWidth.Value = (int)cwneWindowWidth.Value;

			if (cwneWindowWidth_ValueChanged_BUSYNOW) return;
			cwneWindowWidth_ValueChanged_BUSYNOW = true;

			//以降は２重呼び出しで処理させない
			if (CheckContrastAvailable())
			{
				//階調変更処理
				frmScanImage.Instance.WindowWidth = (int)cwneWindowWidth.Value;
                //変更2015/01/21hata
				//if (this.ActiveControl == cwneWindowWidth) RestoreContrast();
                if ((this.ActiveControl == cwneWindowWidth) | (this.ActiveControl == txtWW)) RestoreContrast();
            }
			else
			{
				//階調変換できない場合元に戻す
				//sldWindowWidth.Value = PreviousValue
			}

			//元の状態に戻す
			cwneWindowWidth_ValueChanged_BUSYNOW = false;
		}


		//*******************************************************************************
		//機　　能： 階調変換が可能か
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*******************************************************************************
		private bool CheckContrastAvailable()
		{
			//戻り値初期化
			bool functionReturnValue = false;

			if (this.Visible)
			{
				bool ByOperate = false;
				ByOperate = (this.ActiveControl == cwneWindowLevel) ||
							(this.ActiveControl == cwneWindowWidth) ||
							(this.ActiveControl == sldWindowLevel) ||
							(this.ActiveControl == sldWindowWidth) ||
							(this.ActiveControl == cwneGamma);				//v19.00 cwneGammaの条件を追加 by長野 2012/02/22

				//if (modLibrary.IsExistForm(frmColor.Instance))
                if (modLibrary.IsExistForm("frmColor"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
                {
					//メッセージ表示：ｶﾗｰ処理実行中は、階調変換は行えません。
					//メッセージ表示：ｶﾗｰ処理実行中は、階調変換およびガンマ変換は行えません v19.00 by長野 2012/02/22
					if (ByOperate) MessageBox.Show(CTResources.LoadResString(9391), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return functionReturnValue;
				}
				//画像ﾊﾟﾚｯﾄのﾓｰﾄﾞをﾁｪｯｸ
				else if (CTSettings.dispinf.Data.color_max != 8191)
				{
					//メッセージ表示：ｵﾘｼﾞﾅﾙｶﾗｰ(CT値)でﾊﾟﾚｯﾄ設定されているので、階調変換は行えません。
					//メッセージ表示：ｵﾘｼﾞﾅﾙｶﾗｰ(CT値)でﾊﾟﾚｯﾄ設定されているので、階調変換およびガンマ変換は行えません。v19.00 by長野 2012/02/22
					if (ByOperate) MessageBox.Show(CTResources.LoadResString(9390), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return functionReturnValue;
				}
			}

			//戻り値セット
			functionReturnValue = true;
			return functionReturnValue;
		}


		private void RestoreContrast()
		{
			int i = 0;

			if (ContrastCount == ContrastList.GetUpperBound(0))
			{
				//最初に記憶したものを削除
				for (i = 1; i <= ContrastCount - 1; i++)
				{
					ContrastList[i] = ContrastList[i + 1];
				}
			}
			else
			{
				ContrastCount = ContrastCount + 1;
			}

			ContrastList[ContrastCount] = LastContrast;
			cmdUndo.Enabled = true;

			//現在の階調値の保存
			SaveLastContrast();
		}


		private void ClearContrastList()
		{
			ContrastCount = 0;
			cmdUndo.Enabled = false;
		}


		//*******************************************************************************
		//機　　能： 「元に戻す」ボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		private void cmdUndo_Click(object sender, EventArgs e)
		{			
			//１個
			cwneWindowLevel.Value = ContrastList[ContrastCount].WindowLevel;
			cwneWindowWidth.Value = ContrastList[ContrastCount].WindowWidth;
			cwneGamma.Value = (decimal)ContrastList[ContrastCount].GAMMA;   //v19.00 追加 by長野 2012/05/02

			ContrastCount = ContrastCount - 1;
			cmdUndo.Enabled = (ContrastCount > 0);

			//現在の階調値を記憶
			SaveLastContrast();
		}


		//*******************************************************************************
		//機　　能： 「階調一括変換...」ボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		private void cmdAllContrast_Click(object sender, EventArgs e)
		{
			//階調一括変換画面表示
			frmAllwlww.Instance.ShowDialog();
		}


		//*******************************************************************************
		//機　　能： スライスフレーム内の「前に戻る」「次に進む」ボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		private void cmdSliceNextBack_Click(object sender, EventArgs e)
		{
			if (sender as Button == null) return;
			int Index = Array.IndexOf(cmdSliceNextBack, sender);

			int NextValue = 0;

			//マウスポインタを砂時計にする
			this.Cursor = Cursors.WaitCursor;

			//スライダーコントロールの値を変化させることにより制御する

			//次のインデックス番号候補
			NextValue = sldImageSearch.Value + new int[] { 1, -1 }[Index];

			if (NextValue > sldImageSearch.Maximum)
			{
				sldImageSearch.Value = sldImageSearch.Minimum;		//最初に戻る
			}
			else if (NextValue < sldImageSearch.Minimum)
			{
				sldImageSearch.Value = sldImageSearch.Maximum;		//最後に戻る
			}
			else
			{
				sldImageSearch.Value = NextValue;
			}

			//マウスポインタを元に戻す
			this.Cursor = Cursors.Default;
		}


		//*******************************************************************************
		//機　　能： スライダー動作時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		private void sldImageSearch_PointerValueChanged(object sender, EventArgs e)			// 【C#コントロールで代用】
        {
            int direction = 0;
            string FileName = null;

            Debug.Print(sldImageSearch.Maximum.ToString());

            //イベントによる呼び出しでない場合、実行しない
            if (!byEvent) return;

            //動かそうとする方向を取得
            direction = (sldImageSearch.Value > filSliceName.SelectedIndex + 1 ? 1 : -1);

            do
            {
                FileName = Path.Combine(FilSliceNamePath, Convert.ToString(filSliceName.Items[sldImageSearch.Value - 1]));

                //画像の有無をチェック
                if (!File.Exists(FileName))
                {
                    //画像がなければ次を捜す
                    //ダイアログ表示：画像ファイルがありません。次の画像をサーチしますか？
                    DialogResult result = MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_NotFound, StringTable.IDS_CTImage) + CTResources.LoadResString(9949),
                                                        Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.No)
                    {
                        byEvent = false;
                        sldImageSearch.Value = filSliceName.SelectedIndex + 1;		//元の値に戻す
                        byEvent = true;
                        return;
                    }
                }
                //付帯情報の有無をチェック
                else if (!File.Exists(modFileIO.ChangeExtension(FileName, ".inf")))
                {
                    //付帯情報がなければ次を捜す
                    //ダイアログ表示：
                    //   付帯情報ファイルが見つかりません。次の画像をサーチしますか？
                    DialogResult result = MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_NotFound, StringTable.IDS_InfoFile) + CTResources.LoadResString(9949),
                                    Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.No)
                    {
                        byEvent = false;
                        sldImageSearch.Value = filSliceName.SelectedIndex + 1;		//元の値に戻す
                        byEvent = true;
                        return;
                    }
                }
                else
                {
                    break;
                }

                if ((sldImageSearch.Value + direction) < sldImageSearch.Minimum)
                {
                    sldImageSearch.Value = sldImageSearch.Maximum;
                }
                else if ((sldImageSearch.Value + direction) > sldImageSearch.Maximum)
                {
                    sldImageSearch.Value = sldImageSearch.Minimum;
                }
                else
                {
                    sldImageSearch.Value = sldImageSearch.Value + direction;
                }

            } while (true);

            //表示対象のスライス名リストのインデックス値をセット（lstSliceName_Clickがコールされます）
            filSliceName.SelectedIndex = sldImageSearch.Value - 1;
		}


		//*******************************************************************************
		//機　　能： スライス名リストのパス変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void filSliceName_PathChange()			// 【C#コントロールで代用】
        {
			//int i = 0;

			//総スライス数が２未満の場合，スライスフレームを使用不可にする
			//SetEnabledInFrame fraSlice, (.ListCount > 1)
			modLibrary.SetEnabledInFrame(fraImageSearch, (filSliceName.Items.Count > 1));

			//総スライス数が２未満の場合，画像めくり用スライダを非表示にする
			sldImageSearch.Visible = (filSliceName.Items.Count > 1);
            //追加2014/06/18(検S1)hata
            lblImageSearchMin.Visible = sldImageSearch.Visible;
            lblImageSearchMax.Visible = sldImageSearch.Visible;
            lineShapeMin.Visible = sldImageSearch.Visible;
            lineShapeMax.Visible = sldImageSearch.Visible;
            lblImageSearchMidL.Visible = false;
            lblImageSearchMidR.Visible = false;
            lineShapeMidL.Visible = false;
            lineShapeMidR.Visible = false;

			//画像めくり用スライダの設定
			if (sldImageSearch.Visible)
			{
	
                sldImageSearch.Maximum = filSliceName.Items.Count;
    
                //追加2014/06/18(検S1)hata
                lblImageSearchMin.Text = sldImageSearch.Minimum.ToString();
                lblImageSearchMax.Text = sldImageSearch.Maximum.ToString();

                //追加2015/01/20hata_条件追加（100枚以上）
                if (sldImageSearch.Maximum > 100)
                {
                    //目盛り表示調整
                    if ((sldImageSearch.Maximum - 1) % 3 == 0)
                    {
                        //目盛りを4つ表示        
                        lineShapeMidL.X1 = (lineShapeMax.X1 + lineShapeMin.X1) / 3;
                        lineShapeMidL.X2 = lineShapeMidL.X1;
                        lineShapeMidR.X1 = (lineShapeMax.X1 + lineShapeMin.X1) / 3 * 2;
                        lineShapeMidR.X2 = lineShapeMidR.X1;

                        lblImageSearchMidL.Left = lineShapeMidL.X1 - lblImageSearchMidL.Width / 2 + lblImageSearchMidL.Margin.Left;
                        lblImageSearchMidR.Left = lineShapeMidR.X1 - lblImageSearchMidR.Width / 2 + lblImageSearchMidR.Margin.Left;
                        //変更2015/01/20hata                    
                        //lblImageSearchMidL.Text = Convert.ToString((sldImageSearch.Maximum + sldImageSearch.Minimum) / 3);
                        //lblImageSearchMidR.Text = Convert.ToString((sldImageSearch.Maximum + sldImageSearch.Minimum) / 3 * 2);
                        lblImageSearchMidL.Text = Convert.ToString(sldImageSearch.Minimum + (sldImageSearch.Maximum - sldImageSearch.Minimum) / 3);
                        lblImageSearchMidR.Text = Convert.ToString(sldImageSearch.Minimum + (sldImageSearch.Maximum - sldImageSearch.Minimum) / 3 * 2);

                        lblImageSearchMidL.Visible = true;
                        lineShapeMidL.Visible = true;
                        lblImageSearchMidR.Visible = true;
                        lineShapeMidR.Visible = true;
                    }
                    else if (sldImageSearch.Maximum % 2 == 0)
                    {
                        //目盛りを3つ表示
                        lblImageSearchMidR.Visible = false;
                        lineShapeMidR.Visible = false;
                        lineShapeMidL.X1 = (lineShapeMax.X1 + lineShapeMin.X1) / 2;
                        lineShapeMidL.X2 = lineShapeMidL.X1;
                        lblImageSearchMidL.Left = lineShapeMidL.X1 - lblImageSearchMidL.Width / 2 + lblImageSearchMidL.Margin.Left;
                        //変更2015/01/20hata                    
                        //lblImageSearchMidL.Text = Convert.ToString((sldImageSearch.Maximum + sldImageSearch.Minimum) / 2);
                        lblImageSearchMidL.Text = Convert.ToString((sldImageSearch.Minimum + sldImageSearch.Maximum - sldImageSearch.Minimum) / 2);

                        lblImageSearchMidL.Visible = true;
                        lineShapeMidL.Visible = true;
                    }
                }
                else
                {
                    //目盛りを2つ表示
                    lblImageSearchMidL.Visible = false;
                    lblImageSearchMidR.Visible = false;
                    lineShapeMidL.Visible = false;
                    lineShapeMidR.Visible = false;
                }

			}
 
		}


		//*******************************************************************************
		//機　　能： スライス名リストのインデックス値変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		//private void filSliceName_Click(object sender, EventArgs e)			// 【C#コントロールで代用】
        private void filSliceName_SelectedIndexChanged(object sender, EventArgs e)
        {
			//フレームにカレント画像の番号と総画像数を表示：スライス (m/n)
			//fraSlice.Caption = LoadResString(12119) & " (" & Str(.ListIndex + 1) & "/" & Str(.ListCount) & ")"
			fraImageSearch.Text = CTResources.LoadResString(10511)
								+ " (" + Convert.ToString(filSliceName.SelectedIndex + 1) + "/" + Convert.ToString(filSliceName.Items.Count) + ")";

			//画像めくり用スライダーを更新
			if (sldImageSearch.Visible)
			{
				byEvent = false;
				sldImageSearch.Value = filSliceName.SelectedIndex + 1;
				byEvent = true;
			}

			string FileName = null;

			//表示ファイル名取得
			FileName = Target;

			//すでに現在表示しているファイル名と一致していない場合のみ、表示画面を更新
			if (FileName.ToLowerInvariant() != frmScanImage.Instance.Target.ToLowerInvariant())
			{
				//画像付帯情報の表示
				frmScanImage.Instance.Target = FileName;
			}
		}


		//*******************************************************************************
		//機　　能： Targetプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 現在表示中の画像ファイル
		//
		//履　　歴： v10.2 2005/08/22 (SI3)間々田    新規作成
		//*******************************************************************************
		public string Target
		{
			get
			{
				string functionReturnValue = null;

				if (filSliceName.SelectedIndex < 0)
				{
					functionReturnValue = "";
				}
				else
				{
					functionReturnValue = Path.Combine(FilSliceNamePath, Convert.ToString(filSliceName.Items[filSliceName.SelectedIndex]));
				}

				return functionReturnValue;
			}
			set
			{
				int i = 0;

                if(string.IsNullOrEmpty(value)) return;

				string FolderName = null;
 				FolderName = Path.GetDirectoryName(value);

				//スライス名の一覧を作成
				if (string.IsNullOrEmpty(FolderName))
				{
					modLibrary.SetEnabledInFrame(fraImageSearch, false);
					sldImageSearch.Visible = false;
					fraImageSearch.Text = CTResources.LoadResString(10511);
				}
                //Rev20.00 変更 by長野 2014/12/04
                //else if (FilSliceNamePath != FolderName)
                //{
                //    FilSliceNamePath = FolderName;
                //} 
                //else
                //{
                //    filSliceName.Refresh();
                //    filSliceName_PathChange();
                //}
                else
                {
                    FilSliceNamePath = FolderName;
                }

				//スライス変更時処理
				for (i = 0; i <= filSliceName.Items.Count - 1; i++)
				{
					if (Path.Combine(FilSliceNamePath, Convert.ToString(filSliceName.Items[i])).ToLowerInvariant() == frmScanImage.Instance.Target.ToLowerInvariant())
					{
						filSliceName.SelectedIndex = i;
						break;
					}
				}

				//「元に戻す」用階調リストクリア
				ClearContrastList();
			}
		}


		//*******************************************************************************
		//機　　能： 英語用レイアウト
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v17.60 2011/05/25 (検S1)長野   新規作成
		//*******************************************************************************
		private void EnglishAdjustLayout()
		{
			//int margin = 0;

			Label1.Text = CTResources.LoadResString(12420);
			fraSliderRange.Text = "";
			Label1.Visible = true;
            //optRangeBit0.Top = optRangeBit0.Top + 10;
            //optRangeBit1.Top = optRangeBit1.Top + 6;
            //rev20.01 変更 by長野 2015/05/19
            optRangeBit0.Top = optRangeBit0.Top + 13;
            optRangeBit1.Top = optRangeBit1.Top + 6;
        }

        //削除2014/12/15hata
        ///// <summary>
        ///// TrackBarの最大・最小値を表示する目盛Labelに、現在の最大・最小値を設定する
        ///// </summary>
        //private void SetTrackBarMaxMinLabel()							// 【C#コントロールで代用】
        //{
        //    sldWindowLevelMaxLabel.Text = sldWindowLevel.Maximum.ToString();
        //    sldWindowLevelMinLabel.Text = sldWindowLevel.Minimum.ToString();
        //    sldWindowWidthMaxLabel.Text = sldWindowWidth.Maximum.ToString();
        //    sldWindowWidthMinLabel.Text = sldWindowWidth.Minimum.ToString();
        //}

        //追加2014/05hata
        private void frmImageControl_Activated(object sender, EventArgs e)
        {
            ////描画を強制する
            //if (this.Visible && this.Enabled) this.Refresh();
        }

        //追加2014/12/15hata
        private void txtWLWW_KeyPress(object sender, KeyPressEventArgs e)
        {
            presskye = e.KeyChar;
            switch (e.KeyChar)
            {
                //数字キーと削除キー
                case (char)Keys.D0:
                case (char)Keys.D1:
                case (char)Keys.D2:
                case (char)Keys.D3:
                case (char)Keys.D4:
                case (char)Keys.D5:
                case (char)Keys.D6:
                case (char)Keys.D7:
                case (char)Keys.D8:
                case (char)Keys.D9:
                case (char)Keys.Back:
                case (char)45:  //追加2015/01/21hata //(-)マイナスキー
                    break;
                case (char)Keys.Return:
                    txtWLWW_TextChanged(sender, EventArgs.Empty);
                    break;
                default:
                    e.KeyChar = (char)0;
                    //変更2015/01/08hata_dNet
                    e.Handled = true;
                    //if (sender.Equals(txtWW))
                    //{
                    //    e.KeyChar = (char)1;
                    //}
                    break;
            }

        }

        //追加2014/12/15hata
        private void txtWLWW_TextChanged(object sender, EventArgs e)
        {
            int dval = 0;
           
            //Keys.Returnのときだけ反映する
            if (sender.Equals(txtWL))
            {   
                dval = 0;
                if (! int.TryParse(txtWL.Text, out dval))
                {
                    //追加2015/01/21hata
                    //間に(-)がある場合は(-)を消す
                    int pos = txtWL.Text.LastIndexOf("-") ;
                    if (pos > 0)
                    {
                        //(-)を消す
                        string text = txtWL.Text.Remove(pos, 1);
                        if (string.IsNullOrEmpty(txtWL.Text)) text = cwneWindowLevel.Value.ToString();
                        txtWL.Text = text; 
                    }
                    presskye = (char)0;
                    return;
                }
                if ((int)cwneWindowLevel.Maximum < Convert.ToInt32(txtWL.Text))
                {
                    presskye = (char)0;
                    txtWL.Text = cwneWindowLevel.Maximum.ToString();
                    return;
                }
                if ((int)cwneWindowLevel.Minimum > Convert.ToInt32(txtWL.Text))
                {
                    presskye = (char)0;
                    txtWL.Text = cwneWindowLevel.Minimum.ToString();
                    return;
                }
                if (presskye == (char)Keys.Return)
                {
                    cwneWindowLevel.Value = dval;
                }
            }
            else if (sender.Equals(txtWW))
            {
                dval = 1;
                if (! int.TryParse(txtWW.Text, out dval))
                {
                    presskye = (char)0;
                    return;
                }
                if ((int)cwneWindowWidth.Maximum < Convert.ToInt32(txtWW.Text))
                {
                    presskye = (char)0;
                    txtWW.Text = cwneWindowWidth.Maximum.ToString();
                    return;
                }
                if ((int)cwneWindowWidth.Minimum > Convert.ToInt32(txtWW.Text))
                {
                    presskye = (char)0;
                    txtWW.Text = cwneWindowWidth.Minimum.ToString();
                    return;
                }
                if (presskye == (char)Keys.Return)
                {
                    cwneWindowWidth.Value = dval;
                }
            }
            presskye = (char)0;
        }

        private void txtWLWW_Leave(object sender, EventArgs e)
        {
            if (sender.Equals(txtWL))
            {
                //変更2015/01/08hata
                //if (txtWL.Text == "") txtWL.Text = cwneWindowLevel.Minimum.ToString();
                if (txtWL.Text == "") txtWL.Text = cwneWindowLevel.Value.ToString();
                //追加2015/01/21hata
                if ((txtWL.Text == "-0") | (txtWL.Text == "-")) txtWL.Text = "0";                    
            }
            else if (sender.Equals(txtWW))
            {
                //変更2015/01/08hata
                //if (txtWW.Text == "") txtWW.Text = cwneWindowWidth.Minimum.ToString();
                if (txtWW.Text == "") txtWW.Text = cwneWindowWidth.Value.ToString();
            }
            //追加2015/01/08hata
            presskye = (char)Keys.Return;
            txtWLWW_TextChanged(sender, EventArgs.Empty);
        }

        //追加2015/01/08hata
        private void txtWLWW_KeyDown(object sender, KeyEventArgs e)
        {
            presskye = (char)e.KeyCode;
            switch (e.KeyCode)
            {
                //UpDownキー
                case Keys.Up:
                    if (sender.Equals(txtWL))
                    {
                        cwneWindowLevel.UpButton();
                    }
                    else if (sender.Equals(txtWW))
                    {
                        cwneWindowWidth.UpButton();
                    }
                    break;
                case Keys.Down:
                    if (sender.Equals(txtWL))
                    {
                        cwneWindowLevel.DownButton();
                    }
                    else if (sender.Equals(txtWW))
                    {
                        cwneWindowWidth.DownButton();
                    }
                    break;
            }
        }

	}
}
