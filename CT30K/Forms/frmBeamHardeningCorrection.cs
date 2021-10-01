using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
//
using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;
using System.Diagnostics;


namespace CT30K
{

	///* ************************************************************************** */
	///* システム　　： 産業用ＣＴスキャナ TOSCANER-20000AV Ver8.0                  */
	///* 客先　　　　： ?????? 殿                                                   */
	///* プログラム名： frmBeamHardeningCorrection.frm                              */
	///* 処理概要　　： マルチスキャノ                                              */
	///* 注意事項　　： なし                                                        */
	///* -------------------------------------------------------------------------- */
	///* 適用計算機　： DOS/V PC                                                    */
	///* ＯＳ　　　　： WindowsXP (SP2)                                             */
	///* コンパイラ　： VB 6.0 (SP5)                                                */
	///* -------------------------------------------------------------------------- */
	///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
	///*                                                                            */
	///* V8.00     2006/12/27   (CATS)Ohkado         新規作成：BHC補正テーブル対応  */
	///* V19.00    2012/03/14    H.Nagai             マイクロCTに合わせる           */
	///* -------------------------------------------------------------------------- */
	///* ご注意：                                                                   */
	///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
	///*                                                                            */
	///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2004                 */
	///* ************************************************************************** */
	public partial class frmBeamHardeningCorrection : Form
	{
		//********************************************************************************
		//  共通データ宣言　　　by Ohkado 2007/01/09
		//********************************************************************************

		private const int BHC_DIMENSION = 6;					//BHCグラフからフィッテイングを行うときの次数の数
		private const int DIAMETER_LABEL = 12;					//P、P'のグラフに表示するラベル数
		private const int PDP_LABEL = 2;						//ΔP=P-P'のグラフに表示するラベルの数
		private const int COVE_PARA = 100;						//フィッテイング曲線の滑らかさを決定
		private const int BHC_LBL_COUNT = 5;					//ビームハードニング補正カーブ用グラフのラベル数 追加 by 村田 2007/01/19
		private const int MAX_RETURN_COUNT = 100;				//戻るボタン使用回数 by 村田 2007/01/24
		private const int MAX_DATA_COUNT = 100;					//入力枚数の最大値
		private const int GRAPH_DRAW_NEED_DATA = 10;			//グラフを作成するときの必要画像枚数
		private const int DIAXLBL_MODIFY_SIZE = 80;				//直径スケールラベルを少しだけずらす             追加 by 村田 2007/03/06
		//Const THRESHOLD_VALUE          As Single = 1.6                    '真円度の閾値                                  'v8.1 削除 by Ohkado 2007/04/16
		private const int MAX_DIAMETER = 32767;					//BHCテーブルに入力できる直径の最大値           'v8.1 追加 :ｵ-ﾊﾞｰﾌﾛｰ防止：by Ohkado 2007/04/12
		private const int MAX_RAWDATA = 0xFFFF4;				//BHCテーブルに入力できる生データ値の最大値     'v8.1 追加 :ｵｰﾊﾞｰﾌﾛｰ防止by Ohkado 2007/04/12
		private const int DIAMETER_LABEL_PITCH = 10;			//直径のラベルピッチの最小値                    'v8.1 追加 by Ohkado 2007/04/19

		//v19.00
		private const int AIR_ADD_VALUE = 0x7FFF;

		private bool Changed = false;											//BHCテーブル変更フラグ
		private bool byEvent = false;											//イベントフラグ(cwneA操作を行う:True,行わない：False)
		private bool PamitDrawGraph = false;									//グラフ作成許可フラグ
		//private bool InvalidOperation = false;									//不正操作フラグ
		private bool IsChangeBHCTValue = false;									//BHCテーブルに値を入力したかどうか？
		private float[] IncBHCValue = new float[BHC_DIMENSION + 1];				//BHCインクリメントバリュー V8.1追加

		private struct structDataofBHC				//BHCファイル用データ
		{
			public string FileName;				//ファイル名(パス名も含む)
			public float diaData;				//直径(mm)
			public float rawdata;				//生データ
			public float pData;					//X線減衰指数
		}

		private structDataofBHC[] DataofBHC = new structDataofBHC[MAX_DATA_COUNT + 1];		//BHCファイル構造体
		private int DataOfBHCCount = 0;														//BHCデータのファイル数
		private double[] bhc_a = new double[BHC_DIMENSION + 1];								//BHC用のパラメータ変数格納用


		private int OldRow = 0;											//編集した行を記録するv8.0 追加 by 村田 2007/01/19
		private int OldCol = 0;											//編集した列を記録するv8.0 追加 by 村田 2007/01/19
		private float oldValue = 0;										//編集した値を記録するv8.0 追加 by 村田 2007/01/19
		//private double OldBHCValue = 0;									//編集したBHCﾊﾟﾗﾒｰﾀ値を記録するv8.1 追加 by Ohkado 2007/04/02

		//v19.00
		private int voltPrev = 0;										//前回読み込み管電圧
		private float bhc_p0 = 0;										//P0
		//
		//**************************「元に戻す」機能用の変数の宣言****************************************************

		private enum ChangedType
		{
			BHCAdd = 0,					//データを加える
			BHCDelete = 1,				//データを削除する
			BHCChange = 2,				//データを変更する
			achange = 3,				//aのデータを変更する
		}

		private struct ImgUnDoData							//v8.0 追加 by 村田 2007/01/24
		{
			public short ChangedRows;						//変更が行われた行番号
			public ChangedType Changed;						//変更の種類
			public string FileName;							//ファイル名(パス名も含む)
			public float DIAMETER;							//直径(mm)
			public float rawdata;							//生データ
			public float p;									//X線減衰指数
			public double[] a;								//BHCパラメータa1～a6 v8.1 Single→Double パラメータ変更時のｴﾗｰ対策 by Ohkado 2007/04/02
			public short Count;								//一括削除、一括入力に対応
			public short ChangAIndex;						//a1～a6のどれを変更したか？

			public static ImgUnDoData Initialize()
			{
				ImgUnDoData ImgUnDoData = new ImgUnDoData();

				ImgUnDoData.a = new double[BHC_DIMENSION + 1];

				return ImgUnDoData;
			}
		}

		private ImgUnDoData[] ImgUnDo = new ImgUnDoData[MAX_RETURN_COUNT + 1];		//「元に戻す」用構造体　v8.0 追加 by 村田 2007/01/24
		private int ReturnIndex = 0;						//元に戻せる回数 0→1→・・MAX_RETURN_COUNT→0　v8.0 追加 by 村田 2007/01/24
		private int ReturnCount = 0;						//何回目戻したか？

		//private bool IsAParaLongKeyDown = false;			//a1～a6を長押しによる変更を行ったか？          'v8.1 追加 by Ohkado 2007/04/22
		private float AParaPreviousValue = 0;				//長押しによって何回変更が行われたか？          'v8.1 追加 by Ohkado 2007/04/22
		//************************************************************************************************************

		//Public BHCContinueOK            As Integer                         'frmBHCMessageから値を受け取るための変数            v8.0追加 by 村田 v8.1削除 by Ohkado 2007/04/18


		///【C#コントロールで代用】
		/// 
		/// PictureBox のスケール変更に使用する変数
		/// 
		private float picBHCfittingX = 1;
		private float picBHCfittingY = 1;
        private float picBHCdirectionX = 1;
        private float picBHCdirectionY = 1;

        private float picBHCfittingOffsetX = 0;
		private float picBHCfittingOffsetY = 0;
		private float picHBHCfittingX = 1;
		private float picHBHCfittingY = 1;
        private float picHBHCdirectionX = 1;
        private float picHBHCdirectionY = 1;

        private float picHBHCfittingOffsetX = 0;
		private float picHBHCfittingOffsetY = 0;

		private List<Label> lblBHCYscale = null;
		private List<Label> lblBHCXscale = null;
		private List<Label> lblHBHCXscale = null;
		private List<Label> lblHBHCYscale = null;
		private List<Label> lblDiaXscale = null;
		private List<Label> lblAJog = null;
		private List<NumericUpDown> ntbApara = null;
		private List<decimal> ntbAparaPreviousValue = null;		// 【C#コントロールで代用】ntbAparaの前回値保存用List
        

        //追加2014/10/07hata_v19.51反映
        private List<Microsoft.VisualBasic.PowerPacks.LineShape> linMemBHC = null;
        private List<Microsoft.VisualBasic.PowerPacks.ShapeContainer> linMemBHCContener = null;


        //追加2014/10/07hata_v19.51反映
        private Bitmap imgBHCfitting = null;
        private Bitmap imgHBHCfitting = null;


		private static frmBeamHardeningCorrection _Instance = null;

		public frmBeamHardeningCorrection()
		{
			InitializeComponent();

			lblBHCYscale = new List<Label>();
			lblBHCYscale.Add(lblBHCYscale0);
			lblBHCXscale = new List<Label>();
			lblBHCXscale.Add(lblBHCXscale0);
			lblHBHCXscale = new List<Label>();
			lblHBHCXscale.Add(lblHBHCXscale0);
			lblHBHCYscale = new List<Label>();
			lblHBHCYscale.Add(lblHBHCYscale0);
			lblDiaXscale = new List<Label>();
			lblDiaXscale.Add(lblDiaXscale0);

             //追加2014/10/07hata_v19.51反映
            linMemBHC = new List<Microsoft.VisualBasic.PowerPacks.LineShape>();
            linMemBHC.Add(linMemBHC0);
            linMemBHCContener = new List<Microsoft.VisualBasic.PowerPacks.ShapeContainer>();
            linMemBHCContener.Add(linMemBHC0.Parent);
            linMemBHC[0].Parent = linMemBHCContener[0];
            linMemBHCContener[0].Parent = this.fraTableandGraph;

			lblAJog = new List<Label>();
			lblAJog.Add(null);
			lblAJog.Add(lblAJog1);

			ntbApara = new List<NumericUpDown>();
			ntbApara.Add(null);
			ntbApara.Add(ntbApara1);

			ntbAparaPreviousValue = new List<decimal>();
			ntbAparaPreviousValue.Add(0);
			ntbAparaPreviousValue.Add(ntbApara1.Value);

			msgImgFile.Rows.Add(3);

			for (int i = 0; i < ImgUnDo.Length; i++)
			{
				ImgUnDo[i] = ImgUnDoData.Initialize();
			}
		}

		public static frmBeamHardeningCorrection Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmBeamHardeningCorrection();
				}

				return frmBeamHardeningCorrection._Instance;
			}
		}


		//*******************************************************************************
		//機　　能： 表示ラベルの設定
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v8.00 2006/12/28 Ohkado    新規作成
		//*******************************************************************************
		private void FormLabeling()
		{
			int i = 0;
			int lblScalewide = 0;			//ラベルの幅
			//int buf = 0;

			//BHC補正グラフのラベル位置
			lblBHCYscale0.Top = picBHCfitting.Top + picBHCfitting.Height - 7;
			lblBHCYscale0.Left = picBHCfitting.Left - 57;							//変更 by 村田 2007/03/06
			lblBHCXscale0.Top = picBHCfitting.Top + picBHCfitting.Height + 8;
			lblBHCXscale0.Left = picBHCfitting.Left - lblBHCXscale0.Width / 2;		//変更 by 村田 2007/03/06
			lblBHCXscale0.TextAlign = ContentAlignment.TopCenter;					//追加 by 村田 2007/03/06
			lblBHCYscale0.TextAlign = ContentAlignment.TopRight;

			//BHC補正グラフのY軸ラベルの設定
            lblScalewide = Convert.ToInt32(picBHCfitting.Height / (float)BHC_LBL_COUNT);    //変更 by 村田 2007/01/19
			
            
            for (i = 1; i<= BHC_LBL_COUNT; i++)
			{
#region 【C#コントロールで代用】
/*
				Load lblBHCYscale(i)
*/
#endregion
				lblBHCYscale.Add(new Label());
				lblBHCYscale[i].Font = lblBHCYscale0.Font;
				lblBHCYscale[i].Location = lblBHCYscale0.Location;
				lblBHCYscale[i].Size = lblBHCYscale0.Size;
				lblBHCYscale[i].TextAlign = lblBHCYscale0.TextAlign;
				lblBHCYscale[i].Visible = lblBHCYscale0.Visible;
				this.fraTableandGraph.Controls.Add(lblBHCYscale[i]);

				lblBHCYscale[i].Top = lblBHCYscale[i - 1].Top - lblScalewide;
				lblBHCYscale[i].Text = i.ToString();
			}

			//BHC補正グラフのX軸ラベルの設定
            lblScalewide = Convert.ToInt32(picBHCfitting.Width / (float)BHC_LBL_COUNT);     //変更 by 村田 2007/01/19
			for (i = 1; i <= BHC_LBL_COUNT; i++)
			{
#region 【C#コントロールで代用】
/*
				Load lblBHCXscale(i)
*/
#endregion
				lblBHCXscale.Insert(i, new Label());
				lblBHCXscale[i].Font = lblBHCXscale0.Font;
				lblBHCXscale[i].Location = lblBHCXscale0.Location;
				lblBHCXscale[i].Size = lblBHCXscale0.Size;
				lblBHCXscale[i].TextAlign = lblBHCXscale0.TextAlign;
				lblBHCXscale[i].Visible = lblBHCXscale0.Visible;
				this.fraTableandGraph.Controls.Add(lblBHCXscale[i]);

				lblBHCXscale[i].Left = lblBHCXscale[i - 1].Left + lblScalewide;
				lblBHCXscale[i].Text = i.ToString();
			}

			//BHC補正補助グラフのラベルの位置
			lblHBHCXscale0.Top = picHBHCfitting.Top + picHBHCfitting.Height + 13;
			lblHBHCXscale0.Left = picHBHCfitting.Left - lblHBHCXscale0.Width / 2;			//変更 by 村田 2007/03/06
			lblHBHCXscale0.TextAlign = ContentAlignment.TopCenter;							//追加 by 村田 2007/03/06
			lblHBHCYscale0.Top = picHBHCfitting.Top + picHBHCfitting.Height - 7;
			lblHBHCYscale0.Left = lblBHCYscale0.Left;
			lblHBHCYscale0.TextAlign = ContentAlignment.TopRight;							//追加 by 村田 2007/03/06
			lblHBHCYscale0.Width = lblBHCYscale0.Width;

			//BHC補正補助X軸ラベルの設定
            lblScalewide = Convert.ToInt32(picHBHCfitting.Width / (float)BHC_LBL_COUNT);
			for (i = 1; i <= BHC_LBL_COUNT; i++)
			{
#region 【C#コントロールで代用】
/*
				Load lblHBHCXscale(i)
*/
#endregion
				lblHBHCXscale.Add(new Label());
				lblHBHCXscale[i].Font = lblHBHCXscale0.Font;
				lblHBHCXscale[i].Location = lblHBHCXscale0.Location;
				lblHBHCXscale[i].Size = lblHBHCXscale0.Size;
				lblHBHCXscale[i].TextAlign = lblHBHCXscale0.TextAlign;
				lblHBHCXscale[i].Visible = lblHBHCXscale0.Visible;
				this.fraTableandGraph.Controls.Add(lblHBHCXscale[i]);

				lblHBHCXscale[i].Left = lblHBHCXscale[i - 1].Left + lblScalewide;
				lblHBHCXscale[i].Text = i.ToString();
			}

			//BHC補正補助Y軸ラベルの設定
            lblScalewide = Convert.ToInt32(picHBHCfitting.Height / (float)PDP_LABEL);
			for (i = 1; i <= PDP_LABEL; i++)
			{
#region 【C#コントロールで代用】
/*
				Load lblHBHCYscale(i)
*/
#endregion
				lblHBHCYscale.Add(new Label());
				lblHBHCYscale[i].Font = lblHBHCYscale0.Font;
				lblHBHCYscale[i].Location = lblHBHCYscale0.Location;
				lblHBHCYscale[i].Size = lblHBHCYscale0.Size;
				lblHBHCYscale[i].TextAlign = lblHBHCYscale0.TextAlign;
				lblHBHCYscale[i].Visible = lblHBHCYscale0.Visible;
				this.fraTableandGraph.Controls.Add(lblHBHCYscale[i]);
				
				lblHBHCYscale[i].Top = lblHBHCYscale[i - 1].Top - lblScalewide;
			}

			//BHC円柱ファントム直径のラベルの位置
            lblDiaXscale0.Top = linDiameter.Y1 + 10;
            lblDiaXscale0.Left = linDiameter.X1 - lblDiaXscale0.Width / 2;				//変更 by 村田 2007/03/06
            lblDiaXscale0.TextAlign = ContentAlignment.TopCenter;							//追加 by 村田 2007/03/06

            //BHC円柱ファントム直径のラベルメモリの位置
            linMemBHC0.X1 = linDiameter.X1;
            linMemBHC0.Y1 = linDiameter.Y1;
            linMemBHC0.X2 = linDiameter.X1;
            linMemBHC0.Y2 = linDiameter.Y1 - 7;

			//BHC円柱ファントム直径の補助ラベルの設定
            lblScalewide = (linDiameter.X2 - linDiameter.X1) / DIAMETER_LABEL;

            
            for (i = 1; i <= DIAMETER_LABEL; i++)
			{
				lblDiaXscale.Add(new Label());
				lblDiaXscale[i].Font = lblDiaXscale0.Font;
				lblDiaXscale[i].Location = lblDiaXscale0.Location;
				lblDiaXscale[i].Size = lblDiaXscale0.Size;
				lblDiaXscale[i].TextAlign = lblDiaXscale0.TextAlign;
				lblDiaXscale[i].Visible = lblDiaXscale0.Visible;
				this.fraTableandGraph.Controls.Add(lblDiaXscale[i]);
 				lblDiaXscale[i].Left = lblDiaXscale[i - 1].Left + lblScalewide;
				lblDiaXscale[i].Text = i.ToString();

				//BHC円柱ファントム直径の補助ラベルメモリの設定
                linMemBHCContener.Add(new Microsoft.VisualBasic.PowerPacks.ShapeContainer());
                linMemBHC.Add(new Microsoft.VisualBasic.PowerPacks.LineShape());
                linMemBHC[i].BorderStyle = linMemBHC0.BorderStyle;
                linMemBHC[i].Visible = linMemBHC0.Visible;
                linMemBHC[i].Parent = linMemBHCContener[i];
                linMemBHCContener[i].Parent = this.fraTableandGraph;
                linMemBHC[i].X1 = linMemBHC[i - 1].X1 + lblScalewide;
                linMemBHC[i].X2 = linMemBHC[i].X1;
                linMemBHC[i].Y1 = linMemBHC[0].Y1;
                linMemBHC[i].Y2 = linMemBHC[0].Y2;
            }

			//a2～a6JOGの設定
			for (i = 2; i <= BHC_DIMENSION; i++)
			{
				//JOGのラベル位置設定
				lblAJog.Add(new Label());
				lblAJog[i].Font = lblAJog1.Font;
				lblAJog[i].Location = lblAJog1.Location;
				lblAJog[i].Size = lblAJog1.Size;
				lblAJog[i].TextAlign = lblAJog1.TextAlign;
				lblAJog[i].Visible = lblAJog1.Visible;
				this.fraBHCcontrol.Controls.Add(lblAJog[i]);

				lblAJog[i].Top = lblAJog[i - 1].Top + ntbApara1.Height + 7;				//v8.1変更cwneA→ntbApara by Ohkado 2007/04/12
				lblAJog[i].Text = "a" + Convert.ToString(i);
				//JOGの設定

				//With cwneA(i)                                                          'v8.1削除 by Ohkado 2007/04/12
				//    .Top = cwneA(i - 1).Top + ntbApara(1).Height + 100
				//    '.TabIndex = ntbApara1.TabIndex + i - 1                          '追加 by 村田 2007/02/28
				//End With
				ntbApara.Add(new NumericUpDown());										//v8.1追加 by Ohkado 2007/04/12
				ntbApara[i].DecimalPlaces = ntbApara1.DecimalPlaces;
				ntbApara[i].Font = ntbApara1.Font;
				ntbApara[i].Location = ntbApara1.Location;
				ntbApara[i].Maximum = ntbApara1.Maximum;
				ntbApara[i].Minimum = ntbApara1.Minimum;
				ntbApara[i].Size = ntbApara1.Size;
				ntbApara[i].TabStop = ntbApara1.TabStop;
				ntbApara[i].TextAlign = ntbApara1.TextAlign;
				ntbApara[i].ValueChanged += new EventHandler(this.ntbApara_ValueChanged);
				ntbApara[i].MouseDown += new MouseEventHandler(this.ntbApara_MouseDown);
				ntbApara[i].MouseUp += new MouseEventHandler(this.ntbApara_MouseUp);
				ntbAparaPreviousValue.Add(ntbApara[i].Value);
				this.fraBHCcontrol.Controls.Add(this.ntbApara[i]);

				ntbApara[i].Top = ntbApara[i - 1].Top + ntbApara1.Height + 7;
			}

			//JOG1クリック時の増加量の設定                                               'v8.1削除 by Ohkado 2007/04/12
			//For i = 1 To 6
				//With cwneA(i)
					//.RangeChecking = False
					//.Discrete = False
					//'scancondparよりBHC_biのｲﾝｸﾘﾒﾝﾄ係数を求める
					//.DiscreteInterval = GetCommonFloat("scancondpar", "bhc_bi_increment") / (GetCommonFloat("scancondpar", "bhc_p0") ^ i)
					//'最大値、最小値の設定
					//.Minimum = -300#
					//.Maximum = 300#
					//'初期値の設定
					//.Value = 0
					//.IncDecValue = .DiscreteInterval
					//'矢印を押し続けたときに変化する割合
					//.AccelInc = .DiscreteInterval * 5
					//.AccelTime = 1
					//.RangeChecking = True
					//.Discrete = True
				//End With
			//Next i
			for (i = 1; i <= 6; i++)															//v8.1追加 by Ohkado 2007/04/12
			{

				ntbApara[i].DecimalPlaces = 7;
				//scancondparよりBHC_biのｲﾝｸﾘﾒﾝﾄ係数を求める
				//v19.00
				//初期値を入れる
                CTSettings.scancondpar.Data.mbhc_p0 = 1.0F;
                IncBHCValue[i] = (float)(CTSettings.scancondpar.Data.mbhc_bi_increment / Math.Pow(CTSettings.scancondpar.Data.mbhc_p0, i));
				//            IncBHCValue(i) = GetCommonFloat("scancondpar", "bhc_bi_increment") _
				//                         / (GetCommonFloat("scancondpar", "bhc_p0") ^ i)        'v8.1変更 by Ohkado 2007/04/12
				ntbApara[i].Increment = (decimal)IncBHCValue[i];
				//.Minimum = -300# '削除 by Ohkado 2007/04/16
				//.Maximum = 300#  '削除 by Ohkado 2007/04/16
				//初期値の設定
				ntbApara[i].Value = 0;
			}

			//ΔPスケール変化用cwne                                                      'v8.1削除 by Ohkado 2007/04/18
			//With cwneScaleDP
			//    .RangeChecking = False
			//    .Discrete = False
			//    .DiscreteInterval = 0.01
			//    '初期値の設定
			//    .Value = 0.01
			//    .IncDecValue = .DiscreteInterval
			//    '矢印を押し続けたときに変化する割合
			//    .AccelInc = 0.05
			//    .AccelTime = 1
			//
			//    .RangeChecking = True
			//    .Discrete = True
			//
			//End With

			ntbPDMax.DecimalPlaces = 2;											//v8.1追加 by Ohkado 2007/04/18
			ntbPDMax.Increment = 0.01M;

		}


		//*******************************************************************************
		//機　　能： JOGを表示する
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//補　　足： '
		//履　　歴： v8.00 2006/12/28 Ohkado    新規作成
		//*******************************************************************************
		public void JOG_SET()
		{
			int i = 0;

			for (i = 1; i <= BHC_DIMENSION; i++)
			{
				lblAJog[i].Visible = true;
				//cwneA(i).Visible = True                'v8.1削除 by Ohkado 2007/04/12
				ntbApara[i].Visible = true;				//v8.1追加 by Ohkado 2007/04/12
			}
		}


		//*******************************************************************************
		//機　　能： 各コントロールを初期設定します
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v8.00 2006/12/28 Ohkado    新規作成
		//*******************************************************************************
		private void InitControls()
		{
			int i = 0;
			//int lblScalewide = 0;						//ラベルの幅
			//int buf = 0;

			const int FmBHCLeft = 0;					//フォームのＸ座標
			const int FmBHCTop = 0;						//フォームのＹ座標

			//フォームの表示位置設定
			this.Location = new Point(FmBHCLeft, FmBHCTop);
			this.Height = 1048;					//フォームの高さ v8.1変更 by Ohkado 2007/04/02
			this.Width = 1267;					//フォームの幅

			//「ステータス」フレームの調整
			fraBHCStatus.Location = new Point(FmBHCLeft, 0);			//位置の調整
			fraBHCStatus.BorderStyle = BorderStyle.None;				//外枠を消す
			//BHCテーブルとグラフのフレーム調整
			fraTableandGraph.Location = new Point(FmBHCLeft, 100);		//位置の調整
			fraTableandGraph.BorderStyle = BorderStyle.None;			//外枠を消す
			//コントロールボタンの位置調整
			fraBHCcontrol.Location = new Point(FmBHCLeft, 633);			//位置の調整
			fraBHCcontrol.BorderStyle = BorderStyle.None;				//外枠を消す

			//BHCテーブルの調整
			msgImgFile.Top = 53;
			msgImgFile.Left = 33;
			msgImgFile.Height = 467;

			//BHCテーブルのパラメータ表示位置間隔を設定
			int defaultColWidth = 60;
            defaultColWidth = 50;
 
            msgImgFile.RowHeadersWidth = defaultColWidth;														//No
			msgImgFile.Columns[0].Width = msgImgFile.Width - (msgImgFile.Columns.Count) * defaultColWidth;		//パスとファイル名
			msgImgFile.Columns[1].Width = ((msgImgFile.Width - msgImgFile.Columns[0].Width - msgImgFile.RowHeadersWidth) / 3) - 1 * 7;
			msgImgFile.Columns[2].Width = ((msgImgFile.Width - msgImgFile.Columns[0].Width - msgImgFile.RowHeadersWidth) / 3) - 1 * 7;
			msgImgFile.Columns[3].Width = ((msgImgFile.Width - msgImgFile.Columns[0].Width - msgImgFile.RowHeadersWidth) / 3) - 1 * 7;

            msgImgFile.Columns[1].Width = ((msgImgFile.Width - msgImgFile.Columns[0].Width - msgImgFile.RowHeadersWidth) / 3) - 1 * 7 + 10;
            msgImgFile.Columns[2].Width = ((msgImgFile.Width - msgImgFile.Columns[0].Width - msgImgFile.RowHeadersWidth) / 3) - 1 * 7 + 10;
            msgImgFile.Columns[3].Width = ((msgImgFile.Width - msgImgFile.Columns[0].Width - msgImgFile.RowHeadersWidth) / 3) - 1 * 7 + 10;

            // Mod Start 2018/10/29 M.Oyama V26.40 Windows10対応
            //msgImgFile.ColumnHeadersHeight = 18;
            msgImgFile.ColumnHeadersHeight = 20;
            // Mod End 2018/10/29
			for (i = 0; i <= msgImgFile.Rows.Count - 1; i++)
			{
				msgImgFile.Rows[i].Height = 18;
			}

			msgImgFile.TopLeftHeaderCell.Value = "No.";
			msgImgFile.Columns[0].HeaderText = CTResources.LoadResString(StringTable.IDS_SliceName); //スライス名
			msgImgFile.Columns[1].HeaderText = CTResources.LoadResString(17199);
			msgImgFile.Columns[2].HeaderText = CTResources.LoadResString(17200);
			msgImgFile.Columns[3].HeaderText = CTResources.LoadResString(17201);

			msgImgFile.Rows[0].HeaderCell.Value = "1";

			//スクロールバーをつける
			//.ScrollBars = flexScrollBarVertical
			msgImgFile.ScrollBars = ScrollBars.Both;
#region 【C#コントロールで代用】	flexSelectionFreeに相当する設定値はないため、RowHeaderSelect(行単位／セル単位の選択)に設定
/*
			.SelectionMode = flexSelectionFree                                 '追加 by 村田 2007/01/19
*/
#endregion
			msgImgFile.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            // Mod Start 2018/10/29 M.Oyama V26.40 Windows10対応
            //msgImgFile.Height = msgImgFile.ColumnHeadersHeight * 25;			//追加 by 村田 2007/02/28
            msgImgFile.Height = msgImgFile.ColumnHeadersHeight * 23;
            // Mod End 2018/10/29

         	//BHC補正グラフの位置調整
			picBHCfitting.Top = msgImgFile.Top;
			picBHCfitting.Left = msgImgFile.Left + msgImgFile.Width + 100;
			picBHCfitting.Height = msgImgFile.Height - 133;
			picBHCfitting.Width = 533;

			//BHC補正補助グラフの位置設定
			picHBHCfitting.Top = picBHCfitting.Top + picBHCfitting.Height + 133;
			picHBHCfitting.Left = picBHCfitting.Left;
			picHBHCfitting.Height = picBHCfitting.Height;
			picHBHCfitting.Width = picBHCfitting.Width;

			//円柱ファントム直径用のメモリの位置設定
            linDiameter.X1 = picBHCfitting.Left;
            linDiameter.X2 = picBHCfitting.Left + picBHCfitting.Width;
            linDiameter.Y1 = picBHCfitting.Top + picBHCfitting.Height + 60;
            linDiameter.Y2 = picBHCfitting.Top + picBHCfitting.Height + 60;


			//ビームハードニング補正カーブラベルの位置決定
			lblBHCGraphe.Left = picBHCfitting.Left + (picBHCfitting.Width) / 2 - lblBHCGraphe.Width / 2;
			lblBHCGraphe.Top = lblNumTitle.Top;			
			//直線と補正カーブの差分の位置決定
			//        .Left = picHBHCfitting.Left + (picHBHCfitting.width) / 2 - .width / 2  '英語版で文字がかぶるのを修正　ver8.30本間　2008/3/4
			lblBHCSubGraphe.Top = picHBHCfitting.Top - 30;
			//直径(mm)の位置
			//.Left = linDiameter.x2
            lblDiameter_mm.Left = linDiameter.X2 + 17;				//変更 by 村田 2007/03/06
            lblDiameter_mm.Top = linDiameter.Y2;
           
            //P'ラベルの位置
            lblPD1.Left = lblDiameter_mm.Left + 20;
            lblPD1.Top = picBHCfitting.Top + picBHCfitting.Height - 20;
			//P'2ラベルの位置
            lblPD2.Left = lblDiameter_mm.Left +20;
			lblPD2.Top = picHBHCfitting.Top + (picBHCfitting.Height / 2) - 20;
			//P、ΔPの位置
			lblP.Top = lblNumTitle.Top;
			lblP.Left = picBHCfitting.Left - 33;
			lblDP.Left = lblP.Left;

			lblDP.Top = lblBHCSubGraphe.Top;
			ntbPDMax.Top = lblDP.Top;										//v8.1変更cwneScaleDP→ntbPDMax by Ohkado 2007/04/18
			lblScaleDP.Left = lblDP.Left + lblDP.Width + 13;
			ntbPDMax.Left = lblScaleDP.Left + lblScaleDP.Width + 7;			//v8.1変更cwneScaleDP→ntbPDMax by Ohkado 2007/04/18
			lblScaleDP.Top = lblDP.Top;

			lblBHCSubGraphe.Left = ntbPDMax.Left + ntbPDMax.Width + 7;		//英語版で文字がかぶるのを修正　ver8.30本間　2008/3/4

			//元に戻すボタンを使えなくする
			cmdBack.Enabled = false;									//追加 by 村田 2007/01/26

			//｢材質｣の位置                                           '追加 by 村田 2007/03/06
			lblMaterial.Top = lblBHCTableName.Top;
			txtMaterial.Top = lblBHCTableName.Top;
			//｢コメント｣の位置
			txtComment.Top = lblComment.Top;

			//v19.00 削除
			//Rev8.4 相対パス対応 by YAMAKAGE 08-05-28
			//    CommonDialog1.InitDir = DEF_IMGSAVEDIR
			//    dlgBHCTable.InitDir = DEF_BHCTBLDIR
		}


		//*******************************************************************************
		//機　　能： BHCテーブルリストからファイルを削除する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： theIndex        [I/ ] String    削除する行
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2006/12/27   Ohkado      新規作成
		//*******************************************************************************
		private void RemoveBHCTable(int theIndex)
		{

			if (msgImgFile.Rows.Count + 1 > 2)
			{
				msgImgFile.Rows.RemoveAt(theIndex - 1);
				//登録数表示
				lblInpNum.Text = msgImgFile.Rows.Count.ToString();
			}
			else if (theIndex == 1)
			{
				foreach (DataGridViewCell col in msgImgFile.Rows[0].Cells)
				{
					col.Value = "";
				}

				//登録数表示
				lblInpNum.Text = "0";
			}

			//テーブル内容に変更有り
			Changed = true;
		}


		//*******************************************************************************
		//機　　能： BHCテーブルリストにファイルを追加する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   追加する行数
		//        ： fileName 　　 　[I/ ] String    追加するファイル名
		//        ： diameter        [I/ ] Single    直径(mm)
		//        ： max_raw         [I/ ] Single    生データ値
		//        ： p               [I/ ] Single    X線減衰指数:生データ値より計算
		//戻 り 値： なし
		//
		//補　　足： BHCテーブルmsgImgFileに値を入力していく
		//
		//履　　歴： V8.00  2006/12/27   Ohkado      新規作成
		//          V19.00  2013/03/14   H.Nagai     直径1mm単位表示(小数点以下四捨五入)
		//*******************************************************************************
		private void AddToBHCTable(int Index, string FileName, float DIAMETER, float max_raw, float p)		//ファイル名
		{
			//計算した円柱ファントムの直径(mm)とX線減衰指数の配列のデータをBHCテーブルに記入
			msgImgFile.CurrentCell = msgImgFile[0, 0];
			if (string.IsNullOrEmpty(Convert.ToString(msgImgFile.CurrentCell.Value)))
			{
				msgImgFile.CurrentRow.HeaderCell.Value = Convert.ToString(msgImgFile.CurrentRow.Index + 1);
				msgImgFile.CurrentRow.Cells[0].Value = FileName;
				//.col = 2: .Text = Format(RoundOff(DIAMETER, 1), "0")    '半径(diameter)は、10mm単位で四捨五入する
				//.col = 2: .Text = Format(RoundOff(DIAMETER, 0), "0")    '半径(diameter)は、10mm単位で四捨五入する
				//v19.00 直径は小数点以下切捨て
				msgImgFile.CurrentRow.Cells[1].Value = RoundOff(DIAMETER, 0).ToString("0");		//半径(diameter)は、1mm単位で四捨五入する
				msgImgFile.CurrentRow.Cells[2].Value = max_raw.ToString("00.00");
				msgImgFile.CurrentRow.Cells[3].Value = p.ToString("0.0000");
			}
			else
			{
				//            .AddItem CStr(Index) & vbTab & _
				//                       FileName & vbTab & _
				//                        Format(RoundOff(DIAMETER, 1), "0") & vbTab & _
				//                        Format(max_raw, "00.00") & vbTab & _
				//                        Format(p, "0.0000") _
				//                        , Int(Index)
				//            .RowHeight(.Rows - 1) = .RowHeight(1)
				//v19.00 直径1mm単位
				//            .AddItem CStr(Index) & vbTab & _
				//                       FileName & vbTab & _
				//                        Format(DIAMETER, "0") & vbTab & _
				//                        Format(max_raw, "00.00") & vbTab & _
				//                        Format(p, "0.0000") _
				//                        , Int(Index)
				msgImgFile.Rows.Insert((Index - 1), 
									   FileName, 
									   RoundOff(DIAMETER, 0).ToString("0"), 
									   max_raw.ToString("00.00"), 
									   p.ToString("0.0000"));
				msgImgFile.Rows[Index - 1].HeaderCell.Value = Convert.ToString(Index);
				msgImgFile.Rows[msgImgFile.Rows.Count - 1].Height = msgImgFile.Rows[0].Height;
			}

			//表示位置を変更:左側に持ってくる来る処理
			msgImgFile.CurrentCell = msgImgFile[0, msgImgFile.Rows.Count - 1];
			msgImgFile.CurrentRow.Selected = true;
			msgImgFile.CurrentRow.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

			//登録数表示
			lblInpNum.Text = msgImgFile.Rows.Count.ToString();

			//追加する行が表示しきれないばあい、表示範囲を切り替える                 '追加 by 村田 2007/02/28
			if (msgImgFile.Rows.Count + 1 > 23)
			{
				msgImgFile.FirstDisplayedScrollingRowIndex = msgImgFile.Rows.Count - 23;
			}
		}


		//*******************************************************************************
		//機　　能： BHC用グラフの初期化
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2006/1/12   Ohkado      新規作成
		//*******************************************************************************
        private void InitGraph()
        {
            //BHCグラフ、BHC補助グラフの初期化
            if (picBHCfitting.Image != null)
            {
                //picBHCfitting.Image.Dispose();
                picBHCfitting.Image = null;
             }

            if (picHBHCfitting.Image != null) 
            {   
                //picHBHCfitting.Image.Dispose();
                picHBHCfitting.Image = null;
            }
        }

		//*******************************************************************************
		//機　　能： BHCテーブルからグラフのラベルとJOGと目盛の設定
		//           変数名          [I/O] 型        内容
		//引　　数： DataNum         [I/ ] Integer   データ数
		//           theValueA       [I/ ] Single    Aの値:a1～a6の値から計算してくる
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2006/1/12   Ohkado      新規作成
		//*******************************************************************************
		private void Init_Label_Jog(int DataNum, float theValueA)
		{
			//int i = 0;
			int PScaleMax = 0;
			int PDScaleMax = 0;
			float PDPScaleMax = 0;
			int ScaleMax = 0;

			//グラフを描画しないルーチン：入力データの数が10以下の場合、ラベルはすべて消す
			if (DataNum < GRAPH_DRAW_NEED_DATA)
			{
				//すべてのラベルを非表示にする
				Set_All_Label_False();
				//グラフ作成許可フラグを下ろす
				PamitDrawGraph = false;
				return;
			}
			//最初にグラフを描画するルーチン：入力データの数が10以上で、グラフ作成許可フラグが降りている場合はグラフを設定する
			else if (PamitDrawGraph == false)
			{
				//グラフ作成許可フラグを上げる
				PamitDrawGraph = true;
				//JOGを設定する
				JOG_SET();
				//BHCグラフのラベル最大値を求める BHC_LBL_COUNTはラベルの数
				PDScaleMax = PD_SetScaleMax(BHC_LBL_COUNT, theValueA);
				//P'とPのラベル最大値を設定                                  'v8.1変更 by Ohkado 2007/04/18
				PScaleMax = P_SetScaleMax(BHC_LBL_COUNT);
				//PDScaleMaxかPScaleMaxかいずれか大きいほうをラベルに設定するようにした'v8.1追加 by Ohkado 2007/04/18
				ScaleMax = (PScaleMax < PDScaleMax) ? PDScaleMax : PScaleMax;
				//P'とPのラベルを貼り付ける
				Labeling_PD(ScaleMax);
				Labeling_P(ScaleMax);
				//ΔP=P'-Pのラベル最大値を求める
				PDPScaleMax = PDP_SetScaleMax(theValueA);
				Labeling_PDP(PDPScaleMax);
				//上記より求めたΔPのラベル最大値をcwneScaleDPに値を入れる
				byEvent = false;
				ntbPDMax.Maximum = 10;											//v8.1追加 by Ohkado 2007/04/18
                //変更2015/02/02hata_Max/Min範囲のチェック
				//ntbPDMax.Value = (decimal)PDPScaleMax;							//v8.1変更cwneScaleDP→ntbPDMax by Ohkado 2007/04/18
                ntbPDMax.Value = modLibrary.CorrectInRange((decimal)PDPScaleMax, ntbPDMax.Minimum, ntbPDMax.Maximum);
                
                byEvent = true;
				//円柱ファントムの直径用のラベルを表示
				Labeling_diameter(ScaleMax / theValueA, theValueA);
				//グラフタイトルなどのその他のラベルを表示
				Labeling_ALL();
			}
			//入力データの数が10以上で、グラフ作成許可グラフが上がっている場合の処理：ダイヤルやコモンラベルの2重張りを防ぐ
			else
			{
				//BHCグラフのラベル最大値を求める BHC_LBL_COUNTはラベルの数
				PDScaleMax = PD_SetScaleMax(BHC_LBL_COUNT, theValueA);
				//P'とPのラベル最大値を設定                                  'v8.1変更 by Ohkado 2007/04/18
				PScaleMax = P_SetScaleMax(BHC_LBL_COUNT);
				//PDScaleMaxかPScaleMaxかいずれか大きいほうをラベルに設定するようにした'v8.1追加 by Ohkado 2007/04/18
				ScaleMax = (PScaleMax < PDScaleMax) ? PDScaleMax : PScaleMax;
				//P'とPのラベルを貼り付ける
				Labeling_PD(ScaleMax);
				Labeling_P(ScaleMax);
				//直径のラベルも更新
				Labeling_diameter(ScaleMax / theValueA, theValueA);
				//BHCグラフの距離目盛を決定する
				byEvent = false;
				ntbPDMax.Maximum = 10;											//v8.1追加 by Ohkado 2007/04/18
				PDPScaleMax = (float)ntbPDMax.Value;							//v8.1変更cwneScaleDP→ntbPDMax by Ohkado 2007/04/18
				byEvent = true;
				Labeling_PDP(PDPScaleMax);
			}

			//BHCグラフ、BHC補助グラフのスケールを決定する
            //picBHCfittingX = (float)picBHCfitting.Width / (float)(ScaleMax - 0);
            //picBHCfittingY = (float)picBHCfitting.Height / (float)(0 - ScaleMax);
            //picBHCfittingOffsetX = (ScaleMax - 0) < 0 ? (picBHCfitting.ClientSize.Width - 1) * Math.Abs(0 / (float)(ScaleMax - 0)) : 0;
            //picBHCfittingOffsetY = (0 - ScaleMax) < 0 ? (picBHCfitting.ClientSize.Height - 1) * Math.Abs(ScaleMax / (float)(0 - ScaleMax)) : 0;
            picBHCdirectionX = (ScaleMax - 0) < 0 ? -1 : 1;
            picBHCdirectionY = (0 - ScaleMax) < 0 ? -1 : 1;
            picBHCfittingX = (float)picBHCfitting.Width / (float)(ScaleMax - 0) * picBHCdirectionX;
            picBHCfittingY = (float)picBHCfitting.Height / (float)(0 - ScaleMax) * picBHCdirectionY;
            //picBHCfittingOffsetX = (picBHCfitting.ClientSize.Width - 1) * Math.Abs(0 / (float)(ScaleMax - 0));
            //picBHCfittingOffsetY = (picBHCfitting.ClientSize.Height - 1) * Math.Abs(ScaleMax / (float)(0 - ScaleMax));
            picBHCfittingOffsetX = (ScaleMax - 0) < 0 ? (picBHCfitting.ClientSize.Width - 1) * Math.Abs(0 / (ScaleMax - 0)) : 0;
            picBHCfittingOffsetY = (0 - ScaleMax) < 0 ? (picBHCfitting.ClientSize.Height - 1) * Math.Abs(ScaleMax / (0 - ScaleMax)) : 0;

			//picHBHCfittingX = (float)picHBHCfitting.Width / (ScaleMax - 0);
			//picHBHCfittingY = (float)picHBHCfitting.Height / (-PDPScaleMax - PDPScaleMax);
            //picHBHCfittingOffsetX = (ScaleMax - 0) < 0 ? (picHBHCfitting.ClientSize.Width - 1) * Math.Abs(0 / (float)(ScaleMax - 0)) : 0;
            //picHBHCfittingOffsetY = (-PDPScaleMax - PDPScaleMax) < 0 ? (picHBHCfitting.ClientSize.Height - 1) * Math.Abs(PDPScaleMax / (float)(-PDPScaleMax - PDPScaleMax)) : 0;
            picHBHCdirectionX = (ScaleMax - 0) < 0 ? -1 : 1;
            picHBHCdirectionY = (-PDPScaleMax - PDPScaleMax) < 0 ? -1 : 1;
            picHBHCfittingX = (float)picHBHCfitting.Width / (ScaleMax - 0) * picHBHCdirectionX;
            picHBHCfittingY = (float)picHBHCfitting.Height / (-PDPScaleMax - PDPScaleMax) * picHBHCdirectionY;
            //picHBHCfittingOffsetX = (picHBHCfitting.ClientSize.Width - 1) * Math.Abs(0 / (float)(ScaleMax - 0));
            //picHBHCfittingOffsetY = (picHBHCfitting.ClientSize.Height - 1) * Math.Abs(PDPScaleMax / (float)(-PDPScaleMax - PDPScaleMax));
            picHBHCfittingOffsetX = (ScaleMax - 0) < 0 ? (picHBHCfitting.ClientSize.Width - 1) * Math.Abs(0 / (ScaleMax - 0)) : 0;
            picHBHCfittingOffsetY = (-PDPScaleMax - PDPScaleMax) < 0 ? (picHBHCfitting.ClientSize.Height - 1) * Math.Abs(PDPScaleMax /(-PDPScaleMax - PDPScaleMax)) : 0;
            

			//BHCグラフの距離目盛を決定する
			Draw_BHC_Mem_Line(BHC_LBL_COUNT, BHC_LBL_COUNT);

            ////BHC補助グラフの距離目盛を決定する
            Draw_HBHC_Mem_Line(BHC_LBL_COUNT, BHC_DIMENSION);

            //近似グラフを描画する
            DrawFittingCave(ScaleMax, theValueA);


		}


		//*******************************************************************************
		//機　　能： ラベルとJOGの表示をすべて消す
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2006/1/12   Ohkado      新規作成
		//*******************************************************************************
		private void Set_All_Label_False()
		{
			int i = 0;

			//直径(ｍｍ)用の横スケールバー
            linDiameter.Visible = false;

			for (i = 0; i <= DIAMETER_LABEL; i++)
			{
				//直径(ｍｍ)用ラベル
				lblDiaXscale[i].Visible = false;
				//直径(ｍｍ)用の目盛
                linMemBHC[i].Visible = false;
			}
			//ΔP用のラベルの表示
			for (i = 0; i <= PDP_LABEL; i++)
			{
				lblHBHCYscale[i].Visible = false;
			}
			//P'・Pのラベル表示
			for (i = 0; i <= BHC_LBL_COUNT; i++)
			{
				lblBHCXscale[i].Visible = false;
				lblBHCYscale[i].Visible = false;
				lblHBHCXscale[i].Visible = false;
			}

			//JOGを非表示にする
			for (i = 1; i <= BHC_DIMENSION; i++)
			{
				lblAJog[i].Visible = false;
				//cwneA(i).Visible = False       'v8.1削除 by Ohkado 2007/04/12
				ntbApara[i].Visible = false;		//v8.1追加 by Ohkado 2007/04/12
			}
			//ΔPのラベルを非表示
			ntbPDMax.Visible = false;				//v8.1変更cwneScaleDP→ntbPDMax by Ohkado 2007/04/18
			//ΔPの表示
			lblDP.Visible = false;
			//Pの表示
			lblP.Visible = false;
			//P’の表示
			lblPD1.Visible = false;
			//P’の表示
			lblPD2.Visible = false;
			//直径(mm)の表示
			lblDiameter_mm.Visible = false;
			//縦軸倍率
			lblScaleDP.Visible = false;
		}


		//*******************************************************************************
		//機　　能： グラフ作成時のラベル表示
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2006/1/12   Ohkado      新規作成
		//*******************************************************************************
		private void Labeling_ALL()
		{
			//ΔPのラベルの表示
			ntbPDMax.Visible = true;			//v8.1変更cwneScaleDP→ntbPDMax by Ohkado 2007/04/18
			//ΔPの表示
			lblDP.Visible = true;
			//Pの表示
			lblP.Visible = true;
			//P’の表示
			lblPD1.Visible = true;
			//P’の表示
			lblPD2.Visible = true;
			//直径(mm)の表示
			lblDiameter_mm.Visible = true;
			//縦軸倍率
			lblScaleDP.Visible = true;
		}


		//*******************************************************************************
		//機　　能： PとP’の関係をプロットする
		//           変数名          [I/O] 型        内容
		//引　　数： theValueA       [I/ ] Single    Aの値:a1～a6の値より計算
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2006/1/12   Ohkado      新規作成
		//*******************************************************************************
		private void PlotBHC(float theValueA)
		{
			int i = 0;

			//グラフ作成許可フラグが偽の時はプロットしない
			if (PamitDrawGraph == false) return;

			//BHC補正グラフのスケール決定
			Pen penBHCfitting = new Pen(Color.Blue, 5);

            //imgBHCfitting = (Bitmap)picBHCfitting.Image.Clone();
            //imgBHCfitting = ((picBHCfitting.Image != null) ? (Bitmap)picBHCfitting.Image.Clone() : new Bitmap(picBHCfitting.Width, picBHCfitting.Height));
            imgBHCfitting = (Bitmap)((picBHCfitting.Image) ?? new Bitmap(picBHCfitting.Width, picBHCfitting.Height));

            Graphics gBHCfitting = Graphics.FromImage(imgBHCfitting);

            //座標変換をリセットする（初期化）
            gBHCfitting.ResetTransform();

            //Graphics上で座標平行移動（先に移動させておく）
            gBHCfitting.TranslateTransform(picBHCfittingOffsetX, picBHCfittingOffsetY);

            //Graphics上でスケーリング(倍率で設定)
            gBHCfitting.ScaleTransform(picBHCdirectionX, picBHCdirectionY);

			//BHCテーブルからデータの読み出しAを計算してP,P'の関係をプロットする
			for (i = 0; i <= DataOfBHCCount - 1; i++)
			{
                gBHCfitting.FillEllipse(penBHCfitting.Brush,
                                        picBHCfittingX * DataofBHC[i].diaData * theValueA - penBHCfitting.Width / 2,
                                        picBHCfittingY * DataofBHC[i].pData - penBHCfitting.Width / 2,
                                        penBHCfitting.Width, penBHCfitting.Width);
            
            }

            picBHCfitting.Image = (Bitmap)imgBHCfitting.Clone();

            //座標変換をリセットする（初期化）
            gBHCfitting.ResetTransform();

            gBHCfitting.Dispose();
		}


		//*******************************************************************************
		//機　　能： フィッテイングカーブを描画・メモリも描画する
		//           変数名          [I/O] 型        内容
		//引　　数： PScaleMax       [I/ ]Integer    ラベルP'の最大値
		//           theValueA       [I/ ]Single     Aの値:a1～a6の値より計算
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2006/1/12   Ohkado      新規作成
		//*******************************************************************************
		private void DrawFittingCave(int PScaleMax, float theValueA)
		{
			int i = 0;
			float buf = 0;			//メモリの間隔設定用
			float t = 0;
			float pd = 0;

			//BHCグラフの曲線の色と大きさを決定
			Pen penBHCfitting = new Pen(Color.Lime, 1);			//緑
			Pen penHBHCfitting = new Pen(Color.Lime, 1);		//緑

			//X軸方向に移動する距離を計算
            buf = (float)PScaleMax / (float)COVE_PARA;

            //Bitmap imgBHCfitting = ((picBHCfitting.Image != null) ? (Bitmap)picBHCfitting.Image : new Bitmap(picBHCfitting.Width, picBHCfitting.Height));
            //Bitmap imgHBHCfitting = ((picHBHCfitting.Image != null) ? (Bitmap)picHBHCfitting.Image : new Bitmap(picHBHCfitting.Width, picHBHCfitting.Height));
            imgBHCfitting = (Bitmap)((picBHCfitting.Image) ?? new Bitmap(picBHCfitting.Width, picBHCfitting.Height));
            imgHBHCfitting = (Bitmap)((picHBHCfitting.Image) ?? new Bitmap(picHBHCfitting.Width, picHBHCfitting.Height));
			
            Graphics gBHCfitting = Graphics.FromImage(imgBHCfitting);
			Graphics gHBHCfitting = Graphics.FromImage(imgHBHCfitting);

            //座標変換をリセットする（初期化）
            gBHCfitting.ResetTransform();
            gHBHCfitting.ResetTransform();

            //Graphics上で座標平行移動（先に座標を移動させておく）
            gBHCfitting.TranslateTransform(picBHCfittingOffsetX, picBHCfittingOffsetY);
            gHBHCfitting.TranslateTransform(picHBHCfittingOffsetX, picHBHCfittingOffsetY);
            
            //Graphics上でスケーリング(倍率で設定)
            gBHCfitting.ScaleTransform(picBHCdirectionX, picBHCdirectionY);
            gHBHCfitting.ScaleTransform(picHBHCdirectionX, picHBHCdirectionY);
 
			List<PointF> BHCfittingPoints = new List<PointF>();
			List<PointF> HBHCfittingPoints = new List<PointF>();

			for (i = 0; i <= COVE_PARA + 50; i++)			//線が途中で切れるのを防止するために＋50とした
			{
				//P'とPに関するfitting曲線を計算する
				t = (float)(bhc_a[1] * (i * buf) + 
							bhc_a[2] * Math.Pow((i * buf), 2) + 
							bhc_a[3] * Math.Pow((i * buf), 3) + 
							bhc_a[4] * Math.Pow((i * buf), 4) + 
							bhc_a[5] * Math.Pow((i * buf), 5) + 
							bhc_a[6] * Math.Pow((i * buf), 6));
				//Pdの計算
				pd = theValueA * t;
				//pdのオーバーフロー防止
				if (pd > 7000)
				{
					pd = 7000;
				}
				else if (pd < -7000)
				{
					pd = -7000;
				}

				//P'とPとΔPの曲線を描画
                //if (i == 0)
                //{
                //    BHCfittingPoints.Add(new PointF(picBHCfittingX * pd, picBHCfittingY * (i * buf)));
                //    HBHCfittingPoints.Add(new PointF(picHBHCfittingX * pd, picHBHCfittingY * ((i * buf) - pd)));
                //}
                //else
                //{
                //    BHCfittingPoints.Add(new PointF(picBHCfittingX * pd, picBHCfittingY * (i * buf)));
                //    HBHCfittingPoints.Add(new PointF(picHBHCfittingX * pd, picHBHCfittingY * ((i * buf) - pd)));
                //}
                BHCfittingPoints.Add(new PointF(picBHCfittingX * pd, picBHCfittingY * (i * buf)));
                HBHCfittingPoints.Add(new PointF(picHBHCfittingX * pd, picHBHCfittingY * ((i * buf) - pd)));

            }
			gBHCfitting.DrawLines(penBHCfitting, BHCfittingPoints.ToArray());
			gHBHCfitting.DrawLines(penHBHCfitting, HBHCfittingPoints.ToArray());
            //picBHCfitting.Image = (Bitmap)imgBHCfitting.Clone();
            //picHBHCfitting.Image = (Bitmap)imgHBHCfitting.Clone();
            picBHCfitting.Image = (Bitmap)imgBHCfitting.Clone();
            picHBHCfitting.Image = (Bitmap)imgHBHCfitting.Clone();

            //座標変換をリセットする（初期化）
            gBHCfitting.ResetTransform();
            gHBHCfitting.ResetTransform();

            //imgBHCfitting.Dispose();
            //imgHBHCfitting.Dispose();

            gBHCfitting.Dispose();
			gHBHCfitting.Dispose();
		}


		//*******************************************************************************
		//機　　能： BHCグラフの目盛を作成する.
		//           変数名          [I/O] 型        内容
		//引　　数：  X              [I/ ]Integer    Pのラベル用最大値
		//            Y              [I/ ]Integer    P'のラベル用最大値
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2006/1/12   Ohkado      新規作成
		//*******************************************************************************
		private void Draw_BHC_Mem_Line(int x, int y)
		{
			int i = 0;
			//int temp = 0;
            float temp = 0;
			float buf = 0;

#region 【C#コントロールで代用】
/*
			With picBHCfitting
				'目盛の色決定
				.ForeColor = vbBlack '黒
				'目盛の太さ決定
				.DrawWidth = 1
*/
#endregion
			//目盛の色決定
			//目盛の太さ決定
			Pen penBHCfitting = new Pen(Color.Black, 1);		//黒

			//Bitmap imgBHCfitting = new Bitmap(picBHCfitting.ClientSize.Width, picBHCfitting.ClientSize.Height);
            //imgBHCfitting = ((picBHCfitting.Image != null) ? (Bitmap)picBHCfitting.Image.Clone() : new Bitmap(picBHCfitting.Width, picBHCfitting.Height));
            //imgBHCfitting = new Bitmap(picBHCfitting.ClientSize.Width, picBHCfitting.ClientSize.Height);
            
            imgBHCfitting = (Bitmap)((picBHCfitting.Image) ?? new Bitmap(picBHCfitting.Width, picBHCfitting.Height));

            //if (picBHCfitting.Image == null)
            //    imgBHCfitting = new Bitmap(picBHCfitting.ClientSize.Width, picBHCfitting.ClientSize.Height);
            Graphics gBHCfitting = Graphics.FromImage(imgBHCfitting);
            
            //座標変換をリセットする（初期化）
            gBHCfitting.ResetTransform();

            //Graphics上で座標平行移動（先に座標を移動させておく）
            gBHCfitting.TranslateTransform(picBHCfittingOffsetX, picBHCfittingOffsetY);
            
            //Graphics上でスケーリング(倍率で設定)
            gBHCfitting.ScaleTransform(picBHCdirectionX, picBHCdirectionY);
            
			//P=P'の描画
			gBHCfitting.DrawLine(penBHCfitting, 0, 0, picBHCfitting.ClientSize.Width, picBHCfitting.ClientSize.Height);
			
            //P'の目盛の描画
            buf = picBHCfitting.ClientSize.Height / 30f;
            for (i = 1; i <= x - 1; i++)
			{
				temp = picBHCfitting.ClientSize.Width / (float)x * i;
                gBHCfitting.DrawLine(penBHCfitting, temp, 0, temp, buf);
			}

            //Pの目盛の描画
            //buf = (picBHCfitting.ClientSize.Height / 30f) * ((float)picBHCfitting.Height / (float)picBHCfitting.Width);
            buf = (picBHCfitting.ClientSize.Width / 30f) * ((float)picBHCfitting.Height / (float)picBHCfitting.Width);
            for (i = 1; i <= y - 1; i++)
            {
                temp = picBHCfitting.ClientSize.Height / (float)y * i;
                gBHCfitting.DrawLine(penBHCfitting, 0, temp, buf, temp);
            }

            picBHCfitting.Image = (Bitmap)imgBHCfitting.Clone();

            //座標変換をリセットする（初期化）
            gBHCfitting.ResetTransform();

            //imgBHCfitting.Dispose();
			gBHCfitting.Dispose();
		}


		//*******************************************************************************
		//機　　能： BHC補助グラフの目盛を作成する.
		//
		//           変数名          [I/O] 型        内容
		//引　　数：  X              [I/ ]Integer    P'軸のラベル用最大値
		//            Y              [I/ ]Integer    P'-P軸のラベル用最大値
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2006/1/12   Ohkado      新規作成
		//*******************************************************************************
		private void Draw_HBHC_Mem_Line(int x, int y)
		{
			int i = 0;
			float temp = 0;
			float buf = 0;

#region 【C#コントロールで代用】
/*
			With picBHCfitting
				'目盛の色決定
				.ForeColor = vbBlack '黒
				'目盛の太さ決定
				.DrawWidth = 1
*/
#endregion

			//目盛の色、サイズの設定
			Pen penHBHCfitting = new Pen(Color.Black, 1);

			//Bitmap iimgHBHCfitting = ((picHBHCfitting.Image != null) ? (Bitmap)picHBHCfitting.Image.Clone() : new Bitmap(picHBHCfitting.Width, picHBHCfitting.Height));
            //imgHBHCfitting = new Bitmap(picHBHCfitting.ClientSize.Width, picHBHCfitting.ClientSize.Height);
            imgHBHCfitting = (Bitmap)((picHBHCfitting.Image) ?? new Bitmap(picHBHCfitting.Width, picHBHCfitting.Height));
            
            Graphics gHBHCfitting = Graphics.FromImage(imgHBHCfitting);

            //座標変換をリセットする（初期化）
            gHBHCfitting.ResetTransform();

            //Graphics上で座標平行移動（先に座標を移動させておく）
            gHBHCfitting.TranslateTransform(picHBHCfittingOffsetX, picHBHCfittingOffsetY);
            
            //Graphics上でスケーリング(倍率で設定)
            gHBHCfitting.ScaleTransform(picHBHCdirectionX, picHBHCdirectionY);

			//中心線の描画
			gHBHCfitting.DrawLine(penHBHCfitting, 0, 0, picHBHCfitting.ClientSize.Width , 0);
			
            //目盛の描画　P'軸の目盛作成
			//buf = -picHBHCfitting.ClientSize.Height / 30f;
            buf = picHBHCfitting.ClientSize.Height / 30f;
            for (i = 1; i <= x - 1; i++)
			{
				temp = picHBHCfitting.ClientSize.Width / x * i;
                gHBHCfitting.DrawLine(penHBHCfitting, temp, 0, temp, buf);

            }
			//目盛の描画　P'-P軸の目盛作成
            buf = (picHBHCfitting.ClientSize.Width / 30f) * ((float)picHBHCfitting.Height / (float)picHBHCfitting.Width);
            for (i = 1; i <= y - 1; i++)
			{
				temp = (-picHBHCfitting.ClientSize.Height / y) * (i - y / 2);
                gHBHCfitting.DrawLine(penHBHCfitting, 0, temp, buf, temp);
			}
            picHBHCfitting.Image = (Bitmap)imgHBHCfitting.Clone();

            //座標変換をリセットする（初期化）
            gHBHCfitting.ResetTransform();

            //imgHBHCfitting.Dispose();
			gHBHCfitting.Dispose();
		}


		//*******************************************************************************
		//機　　能： P-P’とPの関係をプロットする
		//           変数名          [I/O] 型        内容
		//引　　数： theValueA       [I/ ] Single    Aの値:a1～a6の値より計算
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2006/1/12   Ohkado      新規作成
		//*******************************************************************************
		private void PlotHBHCDraw(float theValueA)
		{
			int i = 0;

			//グラフ作成許可フラグが偽の時はプロットしない
			if (PamitDrawGraph == false) return;

			//目盛の色、サイズの設定
            Pen penHBHCfitting = new Pen(Color.Blue, 5);
            
			//Bitmap imgHBHCfitting = (Bitmap)picHBHCfitting.Image.Clone();
            //imgHBHCfitting = ((picHBHCfitting.Image != null) ? (Bitmap)picHBHCfitting.Image.Clone() : new Bitmap(picHBHCfitting.Width, picHBHCfitting.Height));
            imgHBHCfitting = (Bitmap)((picHBHCfitting.Image) ?? new Bitmap(picHBHCfitting.Width, picHBHCfitting.Height));
            
            Graphics gHBHCfitting = Graphics.FromImage(imgHBHCfitting);

            //座標変換をリセットする（初期化）
            gHBHCfitting.ResetTransform();

            //Graphics上で座標平行移動（先に座標を移動させておく）
            gHBHCfitting.TranslateTransform(picHBHCfittingOffsetX, picHBHCfittingOffsetY);

            //Graphics上でスケーリング(倍率で設定)
            gHBHCfitting.ScaleTransform(picHBHCdirectionX, picHBHCdirectionY);


			//BHCテーブルからデータの読み出しAを計算してP’とP-P'の関係をプロットする
			for (i = 0; i <= DataOfBHCCount - 1; i++)
			{
                gHBHCfitting.FillEllipse(penHBHCfitting.Brush,
                                         picHBHCfittingX * theValueA * DataofBHC[i].diaData - penHBHCfitting.Width / 2,
                                         picHBHCfittingY * (DataofBHC[i].pData - (theValueA * DataofBHC[i].diaData)) - penHBHCfitting.Width / 2,
                                         penHBHCfitting.Width, penHBHCfitting.Width);

            }
            picHBHCfitting.Image = (Bitmap)imgHBHCfitting.Clone();

            //座標変換をリセットする（初期化）
            gHBHCfitting.ResetTransform();

            //imgHBHCfitting.Dispose();
            gHBHCfitting.Dispose();
		}


		//*******************************************************************************
		//機　　能： 円柱ファントムの直径最大値用のラベルキャプションを変更する.
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Single    直径(mm)の最大値ラベル
		//           theValueA       [I/ ] Single    Aの値
		//戻 り 値： なし
		//
		//補　　足： エラー防止のため直径(mm)のラベル最大値は300mmとする
		//
		//履　　歴： V8.00  2006/1/12   Ohkado      新規作成
		//*******************************************************************************
		private void Labeling_diameter(float Index, float theValueA)
		{
			int i = 0;
			int iMax = 0;
			int LabelPitch = 0;
			float buf = 0;

            linDiameter.Visible = true;

            //消去2014/11/04hata
            //ちらつくのでここでやらない
            ////ラベルを初期化
            //for (i = 0; i <= DIAMETER_LABEL; i++)
            //{
            //    lblDiaXscale[i].Visible = false;
            //    linMemBHC[i].Visible = false;

            //}
            //linDiameter.Refresh();
			
            //ラベルのピッチは10→20→150・・・1000と増加する
			LabelPitch = DIAMETER_LABEL_PITCH;
			//Indexの値の2桁目を四捨五入
			buf = RoundOff(Index, 1);
			//ピッチの計算
			//LabelPitch = ((int)(buf / (LabelPitch * DIAMETER_LABEL)) + 1) * LabelPitch;
            LabelPitch = ((int)Math.Floor(buf / (LabelPitch * DIAMETER_LABEL)) + 1) * LabelPitch;

			//ｴﾗｰ防止:ピッチが2700以上の場合はそれ以上は変化させない:ｵｰﾊﾞｰﾌﾛｰ防止 'v8.1変更 500→2700 by Ohkado 2007/04/19
			if (LabelPitch >= 2700)
			{
				LabelPitch = 2700;
				buf = 2700 * DIAMETER_LABEL;
				Index = buf;
			}

			if (Index > buf)
			{
				buf = buf + LabelPitch;
			}
			else if (buf == 0)
			{
				buf = LabelPitch;
			}

            iMax = Convert.ToInt32(buf / LabelPitch);

            for (i = 0; i <= DIAMETER_LABEL; i++)
			//for (i = 0; i <= iMax - 2; i++)
			{
                if (i > iMax - 2)
                {
                    //他のラベルは初期化(隠す)
                    lblDiaXscale[i].Visible = false;
                    linMemBHC[i].Visible = false;
                }
                else
                {
				    //ラベルはLabelPitchmm毎に貼り付ける
				    if (i == 0)
				    {
                        lblDiaXscale[i].Visible = true;
					    lblDiaXscale[i].Text = Convert.ToInt32(LabelPitch * i).ToString();
				    }
				    else
				    {
					    lblDiaXscale[i].Visible = true;
                        lblDiaXscale[i].Left = lblDiaXscale[i - 1].Left + Convert.ToInt32(LabelPitch * picBHCfitting.Width / Index);
                    
                        lblDiaXscale[i].Text = Convert.ToInt32(LabelPitch * i).ToString();
				    }


				    //ラベルメモリ
                    if (i == 0)
                    {
                        linMemBHC[i].Visible = true;
                    }
                    else
                    {
                        linMemBHC[i].Visible = true;
                        linMemBHC[i].X1 = linMemBHC[i - 1].X1 + Convert.ToInt32(LabelPitch * picBHCfitting.Width / Index);
                        linMemBHC[i].X2 = linMemBHC[i].X1;
                    }
                    //再描画
                    linMemBHC[i].Refresh();

                }
 			}
            //再描画
            linDiameter.Refresh();
 
        }


		//*******************************************************************************
		//機　　能： ΔP=P'-P用のラベル設定.
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Single    ΔPラベルの最大値
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2006/1/12   Ohkado      新規作成
		//*******************************************************************************
		private void Labeling_PDP(float Index)
		{
			int i = 0;
			float buf = 0;

			buf = Index / PDP_LABEL;

			for (i = 0; i <= PDP_LABEL; i++)
			{
				lblHBHCYscale[i].Visible = true;
				lblHBHCYscale[i].Text = (((buf * i) - ((buf * PDP_LABEL) / 2)) * 2).ToString("0.00");
			}
		}


		//*******************************************************************************
		//機　　能： BHCテーブルの値を利用してフィッテイング計算を行う.
		//           変数名          [I/O] 型        内容
		//引　　数： DataofBHC       [I/ ] DataofBHC BHCテーブルのデータ構造体     ：共通変数として上で定義
		//戻 り 値： bhc_a(6)        [ /O] Single    ﾌｨｯﾃｲﾝｸﾞを行った後のa1～a6の値：共通変数として上で定義
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2007/01/12   Ohkado      新規作成
		//*******************************************************************************
		private bool fittingCaliculation()
		{
			bool functionReturnValue = false;

			int i = 0;
			float[] pDataFit = new float[MAX_DATA_COUNT + 1];
			float[] diaDataFit = new float[MAX_DATA_COUNT + 1];
			float[] bhc_a_fit = new float[BHC_DIMENSION + 1];

			//内部データ構造体(DataOfBHC)からP値と直径(mm)を取り出してフィッテイング用データに格納する
			for (i = 0; i<= DataOfBHCCount - 1; i++)
			{
				float.TryParse(DataofBHC[i].pData.ToString("0.######0"), out pDataFit[i]);
				diaDataFit[i] = DataofBHC[i].diaData;
			}

			functionReturnValue = false;
			//フィッテイング(最小二乗法)計算の実行
			if (IICorrect.calculate_bhc_fitting(DataOfBHCCount, ref pDataFit[0], ref diaDataFit[0], ref bhc_a_fit[0]) != 0)
			{
				//メッセージ表示：
				//BHCテーブルに追加できません。
				MessageBox.Show(StringTable.GetResString(21300, CTResources.LoadResString(StringTable.IDS_BHCTable)), 
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			//イベントフラグを偽にすることで、cwneA()関数で2度処理がループされることを防ぐ
			byEvent = false;
			//フィッテイング計算した値をJOGに入力する
			for (i = 1; i<= 6; i++)
			{
				//cwneA(i).Value = Format(bhc_a_fit(i), "0.######0")     'v8.1削除 by Ohkado 2007/04/12
				//bhc_a(i) = cwneA(i).Value
				ntbApara[i].Value = (decimal)RoundOFFAPara(bhc_a_fit[i]);			//v8.1変更 by Ohkado 2007/04/24
				bhc_a[i] = (double)ntbApara[i].Value;
			}
			//イベントフラグを真にする
			byEvent = true;

			functionReturnValue = true;

			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： ｢元に戻す｣ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2007/1/17   Ohkado      新規作成
		//*******************************************************************************
		private void cmdBack_Click(object sender, EventArgs e)
		{
			if (ReturnCount > 0)
			{
				//マウスポインタを砂時計に変更
				Cursor.Current = Cursors.WaitCursor;
				//他のイベントを発生させない
				this.Enabled = false;
				//textBOXの編集取り消し
				InitEditTxtbox();
				//a1～a6用編集用テキストボックスを初期化     'v8.1 追加 by Ohkado 2007/04/02
				//InittxtGridBHCPara                         'v8.1 削除 by Ohkado 2007/04/12
				//「元に戻す」関数呼び出し
				UnDoImg();
				//マウスポインタを元に戻す
				Cursor.Current = Cursors.Default;
				//他のイベント発生を許可
				this.Enabled = true;
			}
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
		//履　　歴： v8.00 2007/01/24 (CATS)Murata    新規作成
		//           v19.00  12/02/27 H.Nagai         マイクロCT対応
		//*******************************************************************************
		private void cmdBHCTableExit_Click(object sender, EventArgs e)
		{
			//modScansel.scanselType theScansel = default(modScansel.scanselType);
            string CurrentBHCDir = null;
			string CurrentBHCName = null;

			//BHCテーブルが未保存で変更されている場合、処理を続行させるかどうかのチェック
			if (!DoContinue()) return;

			//スキャン条件を取得し、ＢＨＣテーブル名の比較を行う             '追加 by 村田 2007/01/24
            ScanSel theScansel = new ScanSel();
            theScansel.Data.Initialize();
			//modScansel.GetScansel(ref theScansel);
            theScansel.Load();

			//BHCテーブルが指定されている場合
			if (!string.IsNullOrEmpty(txtBHCTable.Text))
			{
				//テキストボックスのフルパスをディレクトリとファイル名に分離
				modFileIO.GetPathAndFileName(txtBHCTable.Text, ref CurrentBHCDir, ref CurrentBHCName);

				//現在設定されているＢＨＣテーブルと異なる場合
				//v19.00
				//            If UCase$(RemoveNull(.bhc_dir)) <> UCase$(CurrentBHCDir) Or _
				//                UCase$(RemoveNull(.bhc_name)) <> UCase$(CurrentBHCName) Then
				if (modLibrary.RemoveNull(theScansel.Data.mbhc_dir.GetString()).ToLower() != CurrentBHCDir.ToLower() ||
                    modLibrary.RemoveNull(theScansel.Data.mbhc_name.GetString()).ToLower() != CurrentBHCName.ToLower())
				{
					//メッセージ表示
					//　～を, 今後使用するBHCテーブルに設定しますか？
					//If MsgBox(GetResString(17206, txtBHCTable.text), vbExclamation + vbYesNo) = vbYes Then
					DialogResult result = MessageBox.Show(StringTable.GetResString(17197, txtBHCTable.Text, CTResources.LoadResString(StringTable.IDS_BHCTable)), 
															Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
					if (result == DialogResult.Yes)
					{
						//v19.00
						//                    .bhc_dir = AddNull(CurrentBHCDir)   'BHCテーブルパス名
						//                    .bhc_name = AddNull(CurrentBHCName) 'BHCテーブル名
						//                    .bhc_flag = 1                       'BHC可不可フラグ
						//theScansel.Data.mbhc_dir = modLibrary.AddNull(CurrentBHCDir);		//BHCテーブルパス名
                        theScansel.Data.mbhc_dir.SetString(modLibrary.AddNull(CurrentBHCDir));
                        
                        //theScansel.Data.mbhc_name = modLibrary.AddNull(CurrentBHCName);		//BHCテーブル名
						theScansel.Data.mbhc_name.SetString(modLibrary.AddNull(CurrentBHCName));
                        
                        theScansel.Data.mbhc_flag = 1;										//BHC可不可フラグ

						//modScansel.PutScansel(ref theScansel);				//スキャン条件入力
                        CTSettings.scansel.Put(theScansel.Data);

						//v19.00
						//modScansel.GetScansel(ref modScansel.scansel);
                        CTSettings.scansel.Load();


					}
				}
			}


			//スキャン条件でビームハードニング補正を行うになっていない場合, メッセージ表示後, スキャン条件を書き換える
			//v19.00
			//If theScansel.bhc_flag = 0 Then
			if (theScansel.Data.mbhc_flag == 0)
			{
				//メッセージ表示
				//ビームハードニング補正を行う設定にしますか?
				DialogResult result = MessageBox.Show(CTResources.LoadResString(17207), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

				if (result == DialogResult.Yes)
				{
					//v19.00
					//If UCase$(RemoveNull(theScansel.bhc_dir)) <> "" Or UCase$(RemoveNull(theScansel.bhc_name)) <> "" Then
                    if (string.IsNullOrEmpty(modLibrary.RemoveNull(theScansel.Data.mbhc_dir.GetString()).ToLower()) ||
                        string.IsNullOrEmpty(modLibrary.RemoveNull(theScansel.Data.mbhc_name.GetString()).ToLower()))
					{
						//BHC可不可フラグを真にする
						//theScansel.bhc_flag = 1 'v19.00
                        theScansel.Data.mbhc_flag = 1;	//v19.00

						//スキャン条件入力
						//modScansel.PutScansel(ref theScansel);
                        CTSettings.scansel.Put(theScansel.Data);
                    }
					else
					{
						//エラー時の扱い
						try
						{
							//コモンダイアログ設定
							dlgBHCTableOpen.Title = StringTable.GetResString(StringTable.IDS_Open, CTResources.LoadResString(StringTable.IDS_BHCTable));		//ＢＨＣテーブルを開く
                            dlgBHCTableOpen.Filter = modLibrary.MakeCommonDialogFilter(CTResources.LoadResString(StringTable.IDS_BHCTable), "-BHC.csv");
							
                            dlgBHCTableOpen.FileName = "";
							dlgBHCTableOpen.ShowReadOnly = false;							//[読み取り専用]チェックボックス非表示

							//v19.00
							dlgBHCTableOpen.InitialDirectory = AppValue.InitDir_BHCTable;

							//コモンダイアログを表示
							DialogResult openDialogResult = dlgBHCTableOpen.ShowDialog();

							//エラーの場合、抜ける
							if (openDialogResult == DialogResult.Cancel)
							{
								return;
							}

							//BHCテーブルのファイル名とパス名の分離
							modFileIO.GetPathAndFileName(dlgBHCTableOpen.FileName, ref CurrentBHCDir, ref CurrentBHCName);
                                
							//BHCテーブルのパス名をスキャン条件に入力
							//theScansel.bhc_dir = CurrentBHCDir 'v19.00
							//theScansel.Data.mbhc_dir = CurrentBHCDir;		//v19.00
                            theScansel.Data.mbhc_dir.SetString(CurrentBHCDir);

							//BHCテーブル名をスキャン条件に入力
							//theScansel.bhc_name = CurrentBHCName   'v19.00
                            //theScansel.Data.mbhc_name = CurrentBHCName;		//v19.00
                            theScansel.Data.mbhc_name.SetString(CurrentBHCName);

							//BHC可不可フラグを真にする
							//theScansel.bhc_flag = 1    'v19.00
                            theScansel.Data.mbhc_flag = 1;		//v19.00

							//スキャン条件入力
							//modScansel.PutScansel(ref theScansel);
                            CTSettings.scansel.Put(theScansel.Data);

                            //Rev20.00 Load追加 by長野 2014/12/15
                            CTSettings.scansel.Load();
						}
						catch (Exception ex)
						{
							//キャンセルボタン選択時以外のエラーの場合、エラーメッセージを表示
							MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}
					}
				}
			}

			//フォームのアンロード
			this.Close();
		}


		//*******************************************************************************
		//機　　能： ｢新規テーブル作成｣ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2007/1/17   Ohkado      新規作成
		//*******************************************************************************
		private void cmdBHCTableNew_Click(object sender, EventArgs e)
		{
			//BHCテーブルが未保存で変更されている場合、処理を続行させるかどうかのチェック
			if (!DoContinue()) return;

			//｢元に戻す｣をリセット
			ReturnCount = 0;
			ReturnIndex = 0;
			cmdBack.Enabled = false;

			//BHCテーブルを初期化
			InitBHCTable();
			//BHC内部データ初期化
			InitDataofBHC();
			//グラフを一回消去
			InitGraph();
			//すべてのラベルを非表示にする
			Set_All_Label_False();
			//グラフ作成許可フラグを下ろす
			PamitDrawGraph = false;
			//変更フラグクリア
			Changed = false;
			//削除ﾎﾞﾀﾝを使えなくする
			cmdImgDelete.Enabled = false;

			InitEditTxtbox();			//変更 by 村田 2007/01/26
		}


		//*******************************************************************************
		//機　　能： ビームハードニングテーブルを初期化
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v8.00 2006/12/28 (CATS)Ohkado    新規作成
		//*******************************************************************************
		private void InitBHCTable()
		{
			int i = 0;

			//BHCテーブル名をクリア
			txtBHCTable.Text = "";
			txtMaterial.Text = "";
			txtComment.Text = "";

			//a1～a6用編集用テキストボックスを初期化     'v8.1 追加 by Ohkado 2007/04/02
			//InittxtGridBHCPara                         'v8.1 削除 by Ohkado 2007/04/12

			//リストボックス内の項目を全て削除する
			for (i = msgImgFile.Rows.Count; i >= 1; i--)
			{
				RemoveBHCTable(i);
			}

			//総スライス数
			lblInpNum.Text = "";
		}


		//*******************************************************************************
		//機　　能： ビームハードニング内部データを初期化
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v8.00 2006/12/28 (CATS)Ohkado    新規作成
		//          v19.00 2012/03/14 H.Nagai         前回読み込み管電圧、P0初期化
		//*******************************************************************************
		private void InitDataofBHC()
		{
			int i = 0;

			//v19.00
			voltPrev = 0;
            bhc_p0 = CTSettings.scancondpar.Data.mbhc_p0_const;

			for (i = 0; i <= DataOfBHCCount - 1; i++)
			{
				DataofBHC[i].FileName = "0";
				DataofBHC[i].diaData = 0;
				DataofBHC[i].rawdata = 0;
				DataofBHC[i].pData = 0;
			}

			DataOfBHCCount = 0;
		}


		//*******************************************************************************
		//機　　能： 処理続行するかどうかの確認ダイアログを表示
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： BHCテーブルが未保存で変更されている場合のみ確認ダイアログを表示する
		//
		//履　　歴： V8.00  2006/12/28   Ohkado      新規作成
		//*******************************************************************************
		private bool DoContinue()
		{
			//戻り値初期化
			bool functionReturnValue = false;

			//BHCテーブルが未保存で変更されている場合
			if (Changed)
			{
				//ダイアログ表示：BHCテーブルは変更されています。保存しますか？
				DialogResult result = MessageBox.Show(StringTable.GetResString(9987, CTResources.LoadResString(StringTable.IDS_BHCTable)), 
													  Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

				switch (result)
				{
					case DialogResult.Yes:
						if (!SaveBHCTable(GetBHCTableName())) return functionReturnValue;
						break;
					case DialogResult.No:
                        break;
					case DialogResult.Cancel:
						return functionReturnValue;
				}
			}

			//戻り値セット
			functionReturnValue = true;

			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： 削除ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  2007/1/17   (CATS)Ohkado      新規作成
		//*******************************************************************************
		private void cmdImgDelete_Click(object sender, EventArgs e)
		{
			int i = 0;
			int RemovePointL = 0;
			int RemovePointU = 0;
			int RemoveCount = 0;		//削除するデータ数

			//a1～a6用編集用テキストボックスを初期化     'v8.1 追加 by Ohkado 2007/04/02
			//InittxtGridBHCPara                         'v8.1 削除 by Ohkado 2007/04/12

			//削除した項目を記憶させるためのリセット
			OldRow = 0;			//追加 by 村田 2007/01/19
			OldCol = 0;

			//編集用テキストを非表示にする
			txtGridEdit.Visible = false;

#region 【C#コントロールで代用】
/*
			With msgImgFile
    
				'ドラック時に選択していく方向によって、.RowSelと.Rowが異なるため、必ずRowSelの値が小さくするようにしている
				If .Row > .RowSel Then
					RemovePointL = .Row
					RemovePointU = .RowSel
				Else
					RemovePointL = .RowSel
					RemovePointU = .Row
				End If
*/
#endregion

			int rowIndex = msgImgFile.SelectedCells[0].RowIndex;
			int minIndex = rowIndex;
			int maxIndex = rowIndex;
			foreach (DataGridViewCell cell in msgImgFile.SelectedCells)
			{
				if (rowIndex == cell.RowIndex) continue;

				rowIndex = cell.RowIndex;

				if (rowIndex < minIndex)
				{
					minIndex = rowIndex;
				}
				else if (maxIndex < rowIndex)
				{
					maxIndex = rowIndex;
				}
			}

			RemovePointL = maxIndex + 1;
			RemovePointU = minIndex + 1;

			//削除するデータ数
			RemoveCount = RemovePointL - RemovePointU + 1;
			//選択した項目を全て削除する
			for (i = RemovePointL; i >= RemovePointU; i--)
			{
				//元に戻す処理    '「元に戻す」用直前のデータを覚えておく
				SetImgUnDoData(ChangedType.BHCDelete, i - 1, RemoveCount, 0, 0);
				//内部データの削除
				RemoveCalculateBHCTableData(i);
				//BHCテーブル表示の削除
				RemoveBHCTable(i);
				//変更あり
				Changed = true;
			}

			//番号を修正
			for (i = 1; i <= msgImgFile.Rows.Count; i++)
			{
				msgImgFile.Rows[i - 1].HeaderCell.Value = Convert.ToString(i);
			}


			//「削除」を使用不可にする
			cmdImgDelete.Enabled = false;

			if (msgImgFile.Rows.Count + 1 > GRAPH_DRAW_NEED_DATA)
			{
				//フィッテイング計算
				if (fittingCaliculation() == false) return;
				//グラフの描画
				DrawGraph();
			}
			else
			{
				//グラフ初期化
				InitGraph();
				//すべてのラベルを非表示にする
				Set_All_Label_False();
				//グラフ作成許可フラグを下ろす
				PamitDrawGraph = false;
			}
		}


		//*******************************************************************************
		//機　　能： 参照ボタンクリック時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2006/12/27   Ohkado      新規作成
		//           V8.10  2007/04/22   Ohkado      真円率による円柱ﾌｧﾝﾄﾑの評価を行わなくした：byThresholdAllを削除
		//*******************************************************************************
		private void cmdImgSelect_Click(object sender, EventArgs e)
		{
			string[] FileName = null;
			//int fileCount = 0;
			//int i = 0;
			//float DIAMETER = 0;
			//int StrWidth = 0;
			//Dim byThresholdAll      As Boolean         'BHCファントムの可能性でないデータの読み込み可否    'v8.1 削除 by Ohkado 2007/04/22
			string[] ImportErrorList = null;			//画像読み込み失敗リスト　追加 by 村田 2007/03/01
			int ImportErrorCount = 0;					//画像読み込み失敗数      追加 by 村田 2007/03/01

			//a1～a6用編集用テキストボックスを初期化     'v8.1 追加 by Ohkado 2007/04/02
			//InittxtGridBHCPara                         'v8.1 削除 by Ohkado 2007/04/12

			//真円率の閾値の有効可否初期化
			//byThresholdAll = False                     'v8.1 削除 by Ohkado 2007/04/22
			//画像読み込み失敗数初期化
			ImportErrorCount = 0;

			//テーブル追加前に，BHC用画像データの総数をチェック      'v8.1変更 by Ohkado 2007/04/16
			if (msgImgFile.Rows.Count >= MAX_DATA_COUNT)
			{
				//メッセージ表示：
				//   BHCテーブルに追加できません。
				//   これ以上は追加できません。
				MessageBox.Show(StringTable.GetResString(21300, CTResources.LoadResString(StringTable.IDS_BHCTable)) + "\r" + "\r" + CTResources.LoadResString(21301), 
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//ImageProを起動する:起動していない場合に備える 'V8.1 追加 by Ohkado 2007/04/11
			modCT30K.StartImagePro();

			//********************ファイル選択の場合********************
			if (optFile.Checked)
			{

				try 
				{
					//コモンダイアログ設定
					CommonDialog1.FileName = "";
					CommonDialog1.Title = CTResources.LoadResString(21304);							//BHCテーブルに追加する画像を選択"
					//            .Filter = (LoadResString(17403))                                'IMGファイル   'ver8.30本間　これではフィルターの意味をなさない
					CommonDialog1.Filter = modLibrary.MakeCommonDialogFilter(CTResources.LoadResString(StringTable.IDS_CTImage), ".img");		//画像ファイル(*.img)|*.img|すべてのファイル(*.*)|*.*|     'ver8.30本間変更　フィルタを有効にした
                    CommonDialog1.InitialDirectory = AppValue.InitDir_ImageOpen;					//ディレクトリの設定
					CommonDialog1.ShowReadOnly = false;			//[読み取り専用]チェックボックス非表示 + 複数選択可能 + 既存のファイルのみ選択
					CommonDialog1.Multiselect = true;
					//コモンダイアログを表示
					DialogResult result = CommonDialog1.ShowDialog();
					//エラーの場合、抜ける
					if (result == DialogResult.Cancel)
					{
						return;
					}

					//コモンダイアログで取得したファイルリストをパス名とファイル名のリストに分割
					modLibrary.GetFileList(CommonDialog1.FileNames, ref FileName);

				}
				catch (Exception ex)
				{
					//キャンセルボタン選択時以外のエラーの場合、エラーメッセージを表示
					MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				//ダイアログを消す
				this.Refresh();
				//マウスポインタを砂時計に変更
				Cursor.Current = Cursors.WaitCursor;
				//他のイベントを発生させない
				this.Enabled = false;
				//画像取り込み(ファイル指定)
				//Call SetListBoxFromFile(FileName(), byThresholdAll, ImportErrorCount, ImportErrorList()) 'v8.1変更 by Ohkado 2007/04/22
				SetListBoxFromFile(ref FileName, ref ImportErrorCount, ref ImportErrorList);
				//マウスポインタを元に戻す
				Cursor.Current = Cursors.Default;
				this.Enabled = true;
			}
			//********************フォルダ指定の場合***********************
			//v19.00 フォルダ参照ダイアログを使用
			//ElseIf frmDirSelect.GetPath(InitDir_ImageOpen, LoadResString(21303)) = False Then
			else if (GetBHCTableFolder(ref AppValue.InitDir_ImageOpen) == false)
			{
				return;
			}
			else
			{
				//コモンダイアログを消す
				this.Refresh();
				//マウスポインタを砂時計に変更
				Cursor.Current = Cursors.WaitCursor;
				//他のイベントを発生させない
				this.Enabled = false;
				//画像取り込み(フォルダ指定)
				//Call SetListBoxFromPath(InitDir_ImageOpen, byThresholdAll, ImportErrorCount, ImportErrorList()) 'v8.1変更 by Ohkado 2007/04/22
                SetListBoxFromPath(AppValue.InitDir_ImageOpen, ref ImportErrorCount, ref ImportErrorList);
				this.Enabled = true;
			}

			//追加できなかったファイルがある場合、別フォームに一覧表示                   'v8.0 追加 by 村田 2007/03/02
			if (ImportErrorCount != 0)
			{
				frmBHCImportError frmBHCImportError = new frmBHCImportError();
				//Call frmBHCImportError.AddList(ImportErrorList(), ImportErrorCount - 1, byThresholdAll) 'v8.1変更 by Ohkado 2007/04/22
				frmBHCImportError.AddList(ImportErrorList, ImportErrorCount - 1);
				//マウスポインタを元に戻す
				Cursor.Current = Cursors.Default;
				//BHCファントム画像参照時のエラー一覧表示
				frmBHCImportError.ShowDialog();
			}

			//v19.00 P0を中間測定点にする
			UpdateBHC_P0();

			//マウスポインタを元に戻す
			Cursor.Current = Cursors.Default;

			//BHCテーブルに画像ファイルが10個以上ある場合はBHCカーブを描画する
			if (msgImgFile.Rows.Count + 1 > GRAPH_DRAW_NEED_DATA)
			{
				//フィッテイング計算
				if (fittingCaliculation() == false) return;
				//グラフの描画
				DrawGraph();
			}

			//「削除」を使用不可にする                           'v8.1追加 by Ohkado 2007/04/18
			cmdImgDelete.Enabled = false;
		}


		//*******************************************************************************
		//機　　能： BHCテーブル内部データの追加
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ]Integer    追加するデータの行番号
		//        ： FileName        [I/ ]String     ファイル名
		//        ： diameter        [I/ ]Single     直径
		//        ： max_raw         [I/ ]Single     生データ
		//        ： p               [I/ ]Single     X線減衰指数
		//戻 り 値： なし
		//
		//補　　足： 1データ毎に追加する
		//
		//履　　歴： V8.0 2006/12/27  by Ohkado            新規作成
		//          v19.0 2012/03/15  H.Nagai              bhc_p0更新
		//*******************************************************************************
		private void AddToCalculateBHCTableData(int Index, string FileName, float DIAMETER, float max_raw, float p)
		{
			int i = 0;
			int AddIndex = 0;		//追加するデータ番号
			int EndIndex = 0;

			//追加するデータ番号計算
			AddIndex = Index;
			EndIndex = DataOfBHCCount - 1;

			//追加するデータのために、追加前にあったデータを一つずつずらす
			for (i = EndIndex; i >= AddIndex; i--)
			{
				DataofBHC[i + 1].FileName = DataofBHC[i].FileName;
				DataofBHC[i + 1].diaData = DataofBHC[i].diaData;
				DataofBHC[i + 1].rawdata = DataofBHC[i].rawdata;
				DataofBHC[i + 1].pData = DataofBHC[i].pData;
			}

			DataofBHC[AddIndex].FileName = FileName;
			DataofBHC[AddIndex].diaData = DIAMETER;
			DataofBHC[AddIndex].rawdata = max_raw;
			DataofBHC[AddIndex].pData = p;

			//DataofBHCCountのデクリメント:内部データのデータ数更新
			DataOfBHCCount = DataOfBHCCount + 1;

			//v19.00 bhc_p0更新
			UpdateBHC_P0();
		}


		//*******************************************************************************
		//機　　能： BHCテーブル内部データの削除
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ]Integer    削除するデータ列の番号
		//戻 り 値： なし
		//
		//補　　足： 1データ毎にしか削除できない
		//
		//履　　歴： V8.0 2006/12/27  by Ohkado            新規作成
		//          v19.0 2012/03/15  H.Nagai              bhc_p0更新
		//*******************************************************************************
		private void RemoveCalculateBHCTableData(int Index)
		{
			int i = 0;
			int RemoveIndex = 0;		//削除するデータ番号
			int EndIndex = 0;


			//削除する内部データ番号計算
			RemoveIndex = Index - 1;
			EndIndex = DataOfBHCCount - 1;

			//削除するデータのために、削除前のデータを一つずつずらす
			for (i = RemoveIndex; i <= EndIndex; i++)
			{
				DataofBHC[i].FileName = DataofBHC[i + 1].FileName;
				DataofBHC[i].diaData = DataofBHC[i + 1].diaData;
				DataofBHC[i].rawdata = DataofBHC[i + 1].rawdata;
				DataofBHC[i].pData = DataofBHC[i + 1].pData;
			}

			//DataofBHCCountのデクリメント:内部データのデータ数更新
			DataOfBHCCount = DataOfBHCCount - 1;

			//v19.00 bhc_p0更新
			UpdateBHC_P0();
		}


		//*******************************************************************************
		//機　　能： BHCテーブルに入力される値を計算
		//
		//           変数名          [I/O] 型        内容
		//引　　数： fileName        [I/ ]String     ファイル名
		//        ： byThresholdAll  [ /O]Boolean    真円率が閾値以下だが画像としてとりこむか？
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.0 2006/12/27  by Ohkado            新規作成
		//           V8.1     2007/04/22  by Ohakdo     真円率による画像評価を削除
		//           V19.00  2012/02/20  H.Nagai         マイクロCTにあわせる
		//*******************************************************************************
		//Private Function CalculateBHCTableData(ByVal FileName As String, ByRef byThresholdAll As Boolean) As Boolean
		private bool CalculateBHCTableData(string FileName)
		{
			string RawName = null;		//生データのファイルパス
			float raw_dia = 0;			//生データより求めた直径(画素)
			float Threshold = 0;		//P値のスレッショルド by長野 2012-03-24 v19.99
			
		    //'v19.50 産業用CTの場合は6にする by長野 2014/01/07
		    if (CTSettings.scaninh.Data.avmode == 0)
			{
			    Threshold = 6F;
			}
		    else
			{
			    Threshold = 3.5F;
			}

			//初期化
			bool functionReturnValue = false;
			//入力データ名を保存
			DataofBHC[DataOfBHCCount].FileName = FileName;

			//v19.00 マイクロCTにあわせる
			//生データファイルの拡張子に変更
			//RawName = ChangeExtension(FileName, ".img", ".raw")
			RawName = modFileIO.ChangeExtension(FileName, ".raw");

			//ズーミング画像の場合の処理         'v8.0 追加 by 村田 2007/03/08
			//%.imgが最小文字数なのでこれの場合絶対にズーミング画像ではない
			if (RawName.Length > 5)
			{
				//ズーミング画像かどうかをチェック、ズーミング画像ならば-Z000をはずす
				if (RawName.Substring(RawName.Length - "-Z000.raw".Length, 2) == "-Z")
				{
					RawName = RawName.Substring(0, RawName.Length - "-Z000.raw".Length) + ".raw";
				}
			}

			//円柱ファントムの直径(mm)を抽出
			//If Not GetColumnDiameter(FileName, DataofBHC(DataOfBHCCount).diaData, byThresholdAll) Then 'V8.1変更 by Ohakdo 2007/04/22
			if (!GetColumnDiameter(FileName, ref DataofBHC[DataOfBHCCount].diaData))		//V8.1変更 by Ohakdo 2007/04/22
			{
				return functionReturnValue;
			}

			//円柱ファントムの直径(mm)を小数第1位で四捨五入
			DataofBHC[DataOfBHCCount].diaData = RoundOff(DataofBHC[DataOfBHCCount].diaData, -1);

			//生データ値の最大値を算出
			//If Not GetMaxRawDataValue(RawName, raw_dia, DataofBHC(DataOfBHCCount).rawData) = 0 Then            'v8.1 削除 by Ohkado 2007/04/11
			if (IICorrect.GetMaxRawDataValue(FileName, RawName, ref raw_dia, ref DataofBHC[DataOfBHCCount].rawdata) != 0)		//v8.1 追加 by Ohkado 2007/04/11
			{
				//メッセージ表示：
				//FileNameは、(エラーコード)です
                modCT30K.ErrMessage(IICorrect.GetMaxRawDataValue(FileName, RawName, ref raw_dia, ref DataofBHC[DataOfBHCCount].rawdata), Icon: MessageBoxIcon.Error);
				//MsgBox FileName & "は、" & vbCr & vbCr & LoadResString(GetMaxRawDataValue(RawName, raw_dia, DataofBHC(DataOfBHCCount).rawData)), vbCritical
				return functionReturnValue;
			}

			//pの値を計算する
			DataofBHC[DataOfBHCCount].pData = CalculateXRay_damping_exponent(DataofBHC[DataOfBHCCount].rawdata, 0);
			//P値のスレッショルドを上回る場合は、追加を不可とする 'v19.00 by長野 2012-03-24
			if (DataofBHC[DataOfBHCCount].pData > Threshold)
			{
				return functionReturnValue;
			}

			//DataofBHCCountのインクリメント:内部データのデータ数更新
			DataOfBHCCount = DataOfBHCCount + 1;

			functionReturnValue = true;

			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： X線減衰指数を算出する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： theRValue       [I/ ] Single    生データ値
		//
		//戻 り 値：CalculateXRay_damping_exponent [ /O ] Single X線減衰指数
		//
		//補　　足： なし
		//
		//履　　歴： V8.0 2006/12/27  by Ohkado            新規作成
		//*******************************************************************************
		private float CalculateXRay_damping_exponent(float theRValue, int targetRow)
		{
			float functionReturnValue = 0;

			//v19.00 &H8080を定数、ビット数をscancondpar.fimage_bitから取得
			//       積分枚数を追加
			//CalculateXRay_damping_exponent = (Log(2 ^ 20 - 1) / (2 ^ 15 - 1)) * (theRValue - &H8080&)
			int bit_num = 0;
			double integ_num = 0;
            bit_num = 2 * CTSettings.scancondpar.Data.fimage_bit + 8;
            integ_num = CTSettings.scansel.Data.scan_integ_number;
			if (integ_num == 0) integ_num = 1;

			//CalculateXRay_damping_exponent = (Log(integ_num * (2 ^ bit_num - 1)) / (2 ^ 15 - 1)) * (theRValue - AIR_ADD_VALUE)

			//v19.00 追加 by長野
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypeII)
			{
				functionReturnValue = (float)((Math.Log(Math.Pow(2, bit_num - 1)) / Math.Pow(2, 15 - 1)) * (theRValue - AIR_ADD_VALUE));
			}
            else if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
			{
				string FileName;

                //if (targetRow == 0)
                //{
                //    FileName = DataofBHC[DataOfBHCCount].FileName;
                //}
                //else
                //{
                //    FileName = DataofBHC[targetRow - 1].FileName;
                //}

                FileName = DataofBHC[msgImgFile.CurrentCell.RowIndex].FileName;

				FileName = modFileIO.ChangeExtension(FileName, ".inf");

				 //付帯情報読み込み
				//modImageInfo.ImageInfoStruct ctinf2 = default(modImageInfo.ImageInfoStruct);			//付帯情報構造体
                ImageInfo ctinf2 = new ImageInfo();
                ctinf2.Data.Initialize();

                //if (modImageInfo.ReadImageInfo(ref ctinf2, modFileIO.RemoveExtensionEx(FileName)) == false)
                if (ImageInfo.ReadImageInfo(ref ctinf2.Data, modFileIO.RemoveExtensionEx(FileName)) == false)
				{
					//読み込みエラー
					return functionReturnValue;
				}

				//functionReturnValue = (float)((Math.Log(Math.Pow(2, bit_num - 1)) / Math.Pow(2, 15 - 1)) * (theRValue - ctinf2.Data.mbhc_AirLogValue));
                //Rev20.00 修正 by長野 2014/12/15
                //functionReturnValue = (float)((Math.Log(Math.Pow(2, bit_num) -1) / Math.Pow(2, 15) - 1) * (theRValue - ctinf2.Data.mbhc_AirLogValue));
                functionReturnValue = (float)((Math.Log(Math.Pow(2, bit_num) - 1) / (Math.Pow(2, 15) - 1)) * (theRValue - ctinf2.Data.mbhc_AirLogValue));

            }

			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： ファイル指定時のファイルの取得・リストボックスへの追加
		//
		//           変数名          [I/O] 型        内容
		//引　　数： FileName()      [I/ ] String    ファイル名
		//        ： byThresholdAll　[I/ ] Boolean   真円率の閾値
		//        ： ImportErrorCount[I/ ] Integer   読み込めなかったファイル数
		//        ： ImportErrorList [I/ ] String    読み込めなかったファイルのリスト
		//
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.0 2006/03/02  by Ohkado            新規作成
		//           V8.1 2007/04/22  by Ohakdo            真円率による画像評価を削除
		//*******************************************************************************
		//Private Sub SetListBoxFromFile(ByRef FileName() As String, ByRef byThresholdAll As Boolean, _
		//                                    ByRef ImportErrorCount As Integer, ByRef ImportErrorList() As String)
		private void SetListBoxFromFile(ref string[] FileName, ref int ImportErrorCount, ref string[] ImportErrorList)
		{
			int fileCount = 0;
			int i = 0;
			bool NotAddErrorList = false;

			//取得したファイルの数
			fileCount = (FileName.GetUpperBound(0) == 0) ? 1 : FileName.GetUpperBound(0);
			//エラーファイルを格納する配列の宣言
			ImportErrorList = new string[fileCount + 1];

			//パス名
			AppValue.InitDir_ImageOpen = FileName[0];

			//If msgImgFile.Rows - 2 + fileCount > MAX_DATA_COUNT Then
			if (DataOfBHCCount + fileCount > MAX_DATA_COUNT)
			{
				//マウスポインタを元に戻す
				this.Cursor = Cursors.Default;
				//メッセージ表示：
				//   BHCテーブルに追加できません。
				//   追加する枚数を減らして下さい。
				MessageBox.Show(StringTable.GetResString(21300, CTResources.LoadResString(StringTable.IDS_BHCTable)) + "\r" + "\r" + CTResources.LoadResString(21302), 
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//BHCテーブルに追加
			ImportErrorCount = 0;

			for (i = 1; i <= FileName.GetUpperBound(0); i++)
			{
				//追加チェックＯＫで円柱ファントムとして認識できた
				NotAddErrorList = false;
				if (IsOKAddBHCTable(FileName[i])) NotAddErrorList = CalculateBHCTableData(FileName[i]);		//V8.1, byThresholdAll)を削除 by Ohkado 2007/04/22

				//追加チェックＯＫで円柱ファントムとして認識できた場合
				if (NotAddErrorList)
				{
					//BHCテーブルの値をGUIに表示する
					AddToBHCTable(msgImgFile.Rows.Count + 1, DataofBHC[DataOfBHCCount - 1].FileName, DataofBHC[DataOfBHCCount - 1].diaData, DataofBHC[DataOfBHCCount - 1].rawdata, DataofBHC[DataOfBHCCount - 1].pData);
					//「元に戻す」処理　追加処理
					SetImgUnDoData(ChangedType.BHCAdd, DataOfBHCCount - 1, i - ImportErrorCount, 0, 0);
					//変更あり
					Changed = true;
				}
				//追加チェックが非で円柱ファントムとして認識できない場合
				else
				{
					ImportErrorList[ImportErrorCount] = FileName[i];
					ImportErrorCount = ImportErrorCount + 1;
				}
				//Me.Enable=Falseを有効にしておく
				Application.DoEvents();
			}
		}


		//*******************************************************************************
		//機　　能： フォルダ指定時のファイルの取得・リストボックスへの追加
		//
		//           変数名          [I/O] 型        内容
		//引　　数： strPath      　 [I/ ] String    指定フォルダのパス
		//        ： byThresholdAll　[I/ ] Boolean   真円率の閾値
		//        ： ImportErrorCount[I/ ] Integer   読み込めなかったファイル数
		//        ： ImportErrorList [I/ ] String    読み込めなかったファイルのリスト
		//
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.0 2006/03/02  by Ohkado            新規作成
		//           V8.1 2007/04/22  by Ohakdo            真円率による画像評価を削除
		//*******************************************************************************
		//Private Sub SetListBoxFromPath(ByVal strPath As String, ByRef byThresholdAll As Boolean, _
		//                                            ByRef ImportErrorCount As Integer, ByRef ImportErrorList() As String)
		private void SetListBoxFromPath(string strPath, ref int ImportErrorCount, ref string[] ImportErrorList)
		{
			string FileName = null;
			string FullFileName = null;
			string strPathWithYen = null;
			int i = 0;
			bool NotAddErrorList = false;

			//参照したフォルダにあるデータ数を検索
			strPathWithYen = (strPath.EndsWith("\\")) ? strPath : strPath + "\\";		//パス末尾に \ がついている場合を考慮

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			FileName = Dir(strPathWithYen & "*.img", vbNormal)
			Do While FileName <> ""
				FileName = Dir()
				i = i + 1
			Loop
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
			i = Directory.GetFiles(strPathWithYen, "*.img").Length;

			//エラーファイルを格納する配列の宣言
			ImportErrorList = new string[i + 1];


			//パス末尾に \ がついている場合を考慮
			strPathWithYen = (strPath.EndsWith("\\")) ? strPath : strPath + "\\";

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			'拡張子が .img のファイルを検索する
			FileName = Dir(strPathWithYen & "*.img", vbNormal)
    
			'iの初期化
			i = 1
			Do While FileName <> ""
    
				FullFileName = strPathWithYen & FileName
				'入力データ名を保存
				DataofBHC(DataOfBHCCount).FileName = FullFileName
        
				'テーブル追加前に，BHC用画像データの総数をチェック
				If msgImgFile.Rows - 1 >= MAX_DATA_COUNT Then
					'マウスポインタを元に戻す
					Me.MousePointer = vbDefault
					'メッセージ表示：
					'   BHCテーブルに追加できません。
					'   入力画像枚数を減らして追加して下さい。
					MsgBox GetResString(21300, LoadResString(IDS_BHCTable)) & vbCr & vbCr & LoadResString(21302), vbCritical
					Exit Sub
				End If
           
				'スキャノ画像でない、付帯情報ファイルある、生データがある、
				'       BHCファントム画像である、名前が同じでない、ファイルのみリストボックスに追加
				NotAddErrorList = False
				If IsOKAddBHCTable(FullFileName) Then NotAddErrorList = CalculateBHCTableData(FullFileName) 'V8.1, byThresholdAll)を削除 by Ohkado 2007/04/22
        
				If NotAddErrorList Then
					'BHCテーブルの値をGUIに表示する
					AddToBHCTable msgImgFile.Rows, DataofBHC(DataOfBHCCount - 1).FileName, DataofBHC(DataOfBHCCount - 1).diaData, DataofBHC(DataOfBHCCount - 1).rawdata, DataofBHC(DataOfBHCCount - 1).pData
					'「元に戻す」処理　追加処理
					Call SetImgUnDoData(BHCAdd, DataOfBHCCount - 1, i, 0, 0)
					'｢元に戻す｣処理用インクリメント
					i = i + 1
					'変更あり
					Changed = True
				Else
					ImportErrorList(ImportErrorCount) = FullFileName
					ImportErrorCount = ImportErrorCount + 1
				End If

				'Me.Enable=Falseを有効にしておく
				DoEvents
				'次の候補
				FileName = Dir()
				'Me.Enable=Falseを有効にしておく
				DoEvents
			Loop
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//拡張子が .img のファイルを検索する
			string[] FileNames = Directory.GetFiles(strPathWithYen, "*.img");
			if (FileNames.Length == 0) return;

			//iの初期化
			i = 1;
			foreach(string fileName in FileNames)
			{
				FullFileName = strPathWithYen + Path.GetFileName(fileName);
				//入力データ名を保存
				DataofBHC[DataOfBHCCount].FileName = FullFileName;

				//テーブル追加前に，BHC用画像データの総数をチェック
				if (msgImgFile.Rows.Count >= MAX_DATA_COUNT)
				{
					//マウスポインタを元に戻す
					this.Cursor = Cursors.Default;
					//メッセージ表示：
					//   BHCテーブルに追加できません。
					//   入力画像枚数を減らして追加して下さい。
					MessageBox.Show(StringTable.GetResString(21300, CTResources.LoadResString(StringTable.IDS_BHCTable)) + "\r" + "\r" + CTResources.LoadResString(21302), 
									Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				//スキャノ画像でない、付帯情報ファイルある、生データがある、
				//       BHCファントム画像である、名前が同じでない、ファイルのみリストボックスに追加
				NotAddErrorList = false;
				if (IsOKAddBHCTable(FullFileName)) NotAddErrorList = CalculateBHCTableData(FullFileName);		//V8.1, byThresholdAll)を削除 by Ohkado 2007/04/22

				if (NotAddErrorList)
				{
					//BHCテーブルの値をGUIに表示する
					AddToBHCTable(msgImgFile.Rows.Count + 1, DataofBHC[DataOfBHCCount - 1].FileName, DataofBHC[DataOfBHCCount - 1].diaData, DataofBHC[DataOfBHCCount - 1].rawdata, DataofBHC[DataOfBHCCount - 1].pData);
					//「元に戻す」処理　追加処理
					SetImgUnDoData(ChangedType.BHCAdd, DataOfBHCCount - 1, i, 0, 0);
					//｢元に戻す｣処理用インクリメント
					i = i + 1;
					//変更あり
					Changed = true;
				}
				else
				{
					ImportErrorList[ImportErrorCount] = FullFileName;
					ImportErrorCount = ImportErrorCount + 1;
				}

				//Me.Enable=Falseを有効にしておく
				Application.DoEvents();
				//次の候補
				FileName = (i < FileNames.Length) ? Path.GetFileName(FileNames[i]) : string.Empty;
				//Me.Enable=Falseを有効にしておく
				Application.DoEvents();
			}
		}


		//*******************************************************************************
		//機　　能： [▲][▼]クリック時(マウスボタンを離した時)の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.10  2007/04/23   Ohkado     新規作成
		//*******************************************************************************
#region 【C#コントロールで代用】
/*
		Private Sub ntbApara_UpDownEnd(Index As Integer)
*/
#endregion
		private void ntbApara_MouseUp(object sender, MouseEventArgs e)
		{
			if (Cursor.Current == Cursors.IBeam) return;		// 【C#コントロールで代用】TextBox部分のクリックは無視する

			if (sender as NumericUpDown == null) return;
			int Index = ntbApara.IndexOf((NumericUpDown)sender);
			if (Index < 0) return;

			//[元に戻す]ために一回値を保存　処理処理は一回
			SetImgUnDoData(ChangedType.achange, DataOfBHCCount - 1, 1, AParaPreviousValue, Index);
			//変化させたJOGの値に対応するa(i)に入力する
			//bhc_a(Index) = Format(ntbApara(Index).Value, "0.######0")        'v8.1　変更 by Ohkado 2007/04/19
			ntbApara[Index].Value = (decimal)RoundOFFAPara((float)ntbApara[Index].Value);		//v8.1　追加 by Ohkado 2007/04/23
			bhc_a[Index] = (float)ntbApara[Index].Value;
			//変更あり
			Changed = true;
			//イベントフラグ
			byEvent = true;
			//GRAPH_DRAW_NEED_DATA以上の画像数があるとグラフを描画
			if (msgImgFile.Rows.Count + 1 > GRAPH_DRAW_NEED_DATA)
			{
				//グラフの描画
				DrawGraph();
			}
		}

		//*******************************************************************************
		//機　　能： [▲][▼]クリック時(マウスボタンを押した時)の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.10  2007/04/23   Ohkado     新規作成
		//*******************************************************************************
#region 【C#コントロールで代用】
/*
		Private Sub ntbApara_UpDownStart(Index As Integer)
*/
#endregion
		private void ntbApara_MouseDown(object sender, MouseEventArgs e)
		{
			if (Cursor.Current == Cursors.IBeam) return;		// 【C#コントロールで代用】TextBox部分のクリックは無視する
		
			if (sender as NumericUpDown == null) return;
			int Index = ntbApara.IndexOf((NumericUpDown)sender);
			if (Index < 0) return;

			AParaPreviousValue = Convert.ToSingle(ntbApara[Index].Value.ToString("0.######0"));
			//IncrementValueを変更する
			ntbApara[Index].Increment = (decimal)ComparIncrementValue(Index, (float)ntbApara[Index].Value);

			byEvent = false;
		}


		//*******************************************************************************
		//機　　能： [▲][▼]クリック時のインクリメント値比較の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.10  2007/04/23   Ohkado     新規作成
		//*******************************************************************************
		private float ComparIncrementValue(int Index, float theValue)
		{
			float functionReturnValue = 0;

			int i = 0;
			string buf = null;
			int K = 0;

			K = 0;
			for (i = 1; i <= theValue.ToString("0.######0").Length; i++)
			{
				buf = theValue.ToString("0.######0").Substring(i - 1, 1);
				if (K == 0 && buf == "0") break;
				if (buf == ".") break;
				K = K + 1;
				if (buf == "-")
				{
					K = K - 1;
				}
			}

			if (K == 0)
			{
				functionReturnValue = (IncBHCValue[Index] < 0.0000001) ? 0.0000001F : IncBHCValue[Index];
			}
			else if (K == 1)
			{
				functionReturnValue = (IncBHCValue[Index] < 0.000001) ? 0.000001F : IncBHCValue[Index];
			}
			else if (K == 2)
			{
				functionReturnValue = (IncBHCValue[Index] < 0.00001) ? 0.00001F : IncBHCValue[Index];
			}
			else if (K == 3)
			{
				functionReturnValue = (IncBHCValue[Index] < 0.0001) ? 0.0001F : IncBHCValue[Index];
			}
			else if (K == 4)
			{
				functionReturnValue = (IncBHCValue[Index] < 0.001) ? 0.001F : IncBHCValue[Index];
			}
			else if (K == 5)
			{
				functionReturnValue = (IncBHCValue[Index] < 0.01) ? 0.01F : IncBHCValue[Index];
			}
			else
			{
				functionReturnValue = (IncBHCValue[Index] < 0.1) ? 0.1F : IncBHCValue[Index];
			}

			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： フィッティングパラメータ変更時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.10  2007/04/12   Ohkado     新規作成
		//*******************************************************************************
		private void ntbApara_ValueChanged(object sender, EventArgs e)
		{
			if (sender as NumericUpDown == null) return;
			int Index = ntbApara.IndexOf((NumericUpDown)sender);
			if (Index < 0) return;

			//イベントフラグがFalseの場合
			if (byEvent == false)
			{
				ntbAparaPreviousValue[Index] = ntbApara[Index].Value;		// 【C#コントロールで代用】
				return;
			}

			//元に戻す処理 処理は一回
			SetImgUnDoData(ChangedType.achange, DataOfBHCCount - 1, 1, (double)ntbAparaPreviousValue[Index], Index);
			//変化させたJOGの値に対応するa(i)に入力する
			//ntbApara(Index).Value = CSng(ntbApara(Index).Value)
			//bhc_a(Index) = Format(CSng(ntbApara(Index).Value), "0.######0")        'v8.1　変更 by Ohkado 2007/04/19
			ntbApara[Index].Value = (decimal)RoundOFFAPara((float)ntbApara[Index].Value);			//v8.1　追加 by Ohkado 2007/04/23
			bhc_a[Index] = (float)ntbApara[Index].Value;
			//変更あり
			Changed = true;

			//GRAPH_DRAW_NEED_DATA以上の画像数があるとグラフを描画
			if (msgImgFile.Rows.Count + 1> GRAPH_DRAW_NEED_DATA)
			{
				//グラフの描画
				DrawGraph();
			}

			ntbAparaPreviousValue[Index] = ntbApara[Index].Value;			// 【C#コントロールで代用】
		}


		//*******************************************************************************
		//機　　能： コメント欄変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v8.1    2007/04/10 (CATS)Ohkado    新規作成
		//*******************************************************************************
		private void txtMaterial_TextChanged(object sender, EventArgs e)
		{
			//テキストボックスバイト数チェック（全角文字対応版）
			modLibrary.CheckTextBox(txtMaterial);

			//変更あり
			Changed = true;
		}


		//*******************************************************************************
		//機　　能： コメント欄変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v8.1    2007/04/10 (CATS)Ohkado    新規作成
		//*******************************************************************************
		private void txtComment_TextChanged(object sender, EventArgs e)
		{
			//テキストボックスバイト数チェック（全角文字対応版）
			modLibrary.CheckTextBox(txtComment);

			//変更あり
			Changed = true;
		}


		//v8.1　ここから削除 コンポーネントワークスのcwneからユーザーコントロールのntbAparaを使用することにともなう削除 by Ohkado 2007/04/12
		//'*******************************************************************************
		//'機　　能： a1～a6の編集用テキストボックスの内容が変化した時の処理
		//'
		//'           変数名          [I/O] 型        内容
		//'引　　数： なし
		//'戻 り 値： なし
		//'
		//'補　　足： なし
		//'
		//'履　　歴： v8.1  2007/04/03 (CATS)Ohkado      新規作成
		//'*******************************************************************************
		//Private Sub txtGridBHCPara_KeyDown(KeyCode As Integer, Shift As Integer)
		//
		//    'Enterが押された場合 (KeyCode=13)
		//    If KeyCode = vbKeyReturn Then
		//        'セルの内容を確認する
		//        'エラー処理
		//        If IsBHCErrorValue(txtGridBHCPara.text) Then
		//            '値が変わっていない場合、何もしない
		//            If Format(OldBHCValue, "0.######0") = txtGridBHCPara.text Then
		//                'a1～a6用編集用テキストボックスを初期化
		//                InittxtGridBHCPara
		//                Exit Sub
		//            Else
		//                'セルに値を代入
		//                Call SetBHCValue(txtGridBHCPara.text, txtGridBHCPara.tag)
		//            End If
		//
		//        End If
		//        'a1～a6用編集用テキストボックスを初期化
		//        InittxtGridBHCPara
		//    End If
		//
		//End Sub
		//'*******************************************************************************
		//'機　　能：　a1～a6の編集用テキストボックスの値が適切がどうかの処理
		//'           変数名          [I/O] 型        内容
		//'引　　数： なし
		//'戻 り 値：
		//'
		//'補　　足： なし
		//'
		//'履　　歴： v8.1  2007/04/11 (CATS)Ohkado      新規作成
		//'*******************************************************************************
		//Public Sub txtGridBHCPara_Validate(Cancel As Boolean)
		//
		//    '[▲][▼]をクリックした場合はなにもしない
		//    If txtGridBHCPara.text = "" Then
		//        InittxtGridBHCPara
		//        '関数をぬける
		//        Exit Sub
		//    End If
		//
		//    'エラー処理
		//    If IsBHCErrorValue(txtGridBHCPara.text) Then
		//        '値が変わっていない場合、何もしない
		//        If Format(OldBHCValue, "0.######0") = txtGridBHCPara.text Then
		//            InittxtGridBHCPara
		//            Exit Sub
		//        End If
		//        'セルに値を代入
		//        Call SetBHCValue(txtGridBHCPara.text, txtGridBHCPara.tag)
		//    End If
		//    'a1～a6用編集用テキストボックスを初期化
		//    InittxtGridBHCPara
		//
		//End Sub
		//'*******************************************************************************
		//'機　　能：　a1～a6の編集用テキストボックスの初期化
		//'           変数名          [I/O] 型        内容
		//'引　　数： なし
		//'戻 り 値：
		//'
		//'補　　足： なし
		//'
		//'履　　歴： v8.1  2007/04/02 (CATS)Ohkado      新規作成
		//'*******************************************************************************
		//Private Sub InittxtGridBHCPara()
		//
		//    With txtGridBHCPara
		//        .Visible = False
		//        .text = ""
		//    End With
		//
		//End Sub
		//'*******************************************************************************
		//'機　　能：a1～a6に直接値を入力する
		//'
		//'           変数名          [I/O] 型        内容
		//'引　　数： theText         [I/ ] Variant   入力される値a1～a6
		//'           Index           [I/ ] Integer   a1～a6のどれを変化させたか？
		//'戻 り 値： なし
		//'
		//'補　　足： なし
		//'
		//'履　　歴： v8.1  2007/04/02 (CATS)Ohkado      新規作成
		//'*******************************************************************************
		//Private Sub SetBHCValue(ByVal theText As String, ByVal Index As Integer)
		//
		//    '操作前の情報を保存しておく
		//    Call SetImgUnDoData(achange, DataOfBHCCount - 1, 1, 0, 0)
		//
		//    '変更あり
		//    Changed = True
		//
		//    'a1～a6の表示部分を変更
		//    bhc_a(Index) = CDbl(txtGridBHCPara.text)
		//
		//    byEvent = False
		//    cwneA(Index).Value = bhc_a(Index)
		//    byEvent = True
		//
		//    'GRAPH_DRAW_NEED_DATA以上の画像数があるとグラフを描画
		//    If msgImgFile.Rows > GRAPH_DRAW_NEED_DATA Then
		//        'グラフの描画
		//        DrawGraph
		//    End If
		//
		//End Sub
		//'*******************************************************************************
		//'機　　能： a1～a6のテキストボックスの内容が適切かどうかの処理
		//'
		//'           変数名          [I/O] 型        内容
		//'引　　数： txtVale         [I/ ] Variant   入力される値
		//'戻 り 値： なし
		//'
		//'補　　足： なし
		//'
		//'履　　歴： v8.1  2007/04/02 (CATS)Ohkado      新規作成
		//'*******************************************************************************
		//Private Function IsBHCErrorValue(ByVal txtValue As Variant)
		//
		//    IsBHCErrorValue = True
		//    'テキストボックスの値が数値として評価できない、300～300の範囲外の場合は元に戻す
		//    If IsNumeric(txtValue) = False Then
		//        IsBHCErrorValue = False
		//    ElseIf -300 > CSng(txtValue) Or 300 < CSng(txtValue) Then
		//        IsBHCErrorValue = False
		//    End If
		//    '入力された値が不正ならばメッセージ表示
		//    If Not IsBHCErrorValue Then
		//        'メッセージ:
		//        '不正なパラメータ値となるので処理を元に戻します。
		//        MsgBox LoadResString(17218), vbCritical
		//    End If
		//
		//End Function
		//'*******************************************************************************
		//'機　　能： フィッティングパラメータフォーカス取得時の処理
		//'
		//'           変数名          [I/O] 型        内容
		//'引　　数： なし
		//'戻 り 値： なし
		//'
		//'補　　足： なし
		//'
		//'履　　歴： V8.00  2007/04/02   Ohkado     新規作成
		//'*******************************************************************************
		//Private Sub cwneA_GotFocus(Index As Integer)
		//
		//    '編集用テキストボックスを移動
		//    With cwneA(Index)
		//        txtGridBHCPara.Move .Left, .Top, .width - 270, .Height '270は[▲][▼]の幅分
		//    End With
		//    '編集用テキストボックスの設定
		//    With txtGridBHCPara
		//        .Visible = True
		//        .text = Format(cwneA(Index).Value, "0.######0")
		//        OldBHCValue = cwneA(Index).Value
		//        .tag = Index
		//        .SetFocus
		//    End With
		//
		//End Sub
		//'*******************************************************************************
		//'機　　能： フィッティングパラメータ変更時の処理
		//'
		//'           変数名          [I/O] 型        内容
		//'引　　数： なし
		//'戻 り 値： なし
		//'
		//'補　　足： なし
		//'
		//'履　　歴： V8.00  2006/12/21   Ohkado     新規作成
		//'*******************************************************************************
		//Private Sub cwneA_ValueChanged(Index As Integer, Value As Variant, PreviousValue As Variant, ByVal OutOfRange As Boolean)
		//
		//    'イベントフラグがFalseの場合
		//    If byEvent = False Then Exit Sub
		//    '元に戻す処理 処理は一回
		//    Call SetImgUnDoData(achange, DataOfBHCCount - 1, 1, PreviousValue, Index)
		//    '変化させたJOGの値に対応するa(i)に入力する
		//    bhc_a(Index) = cwneA(Index).Value
		//    '変更あり
		//    Changed = True
		//    'GRAPH_DRAW_NEED_DATA以上の画像数があるとグラフを描画
		//    If msgImgFile.Rows > GRAPH_DRAW_NEED_DATA Then
		//        'グラフの描画
		//        DrawGraph
		//    End If
		//
		//    'a1～a6用編集用テキストボックスを初期化     'v8.1 追加 by Ohkado 2007/04/02
		//    InittxtGridBHCPara
		//
		//End Sub
		//v8.1　ここまで削除 コンポーネントワークスのcwneからユーザーコントロールのntbAparaを使用することにともなう削除 by Ohkado 2007/04/12
		//v8.1　ここから削除 コンポーネントワークスのcwneの使用をやめた。 by Ohkado 2007/04/18
		//'*******************************************************************************
		//'機　　能： ΔPのスケールパラメータ変更時の処理
		//'
		//'           変数名          [I/O] 型        内容
		//'引　　数： なし
		//'戻 り 値： なし
		//'
		//'補　　足： なし
		//'
		//'履　　歴： V8.00  2006/12/21   Ohkado     新規作成
		//'*******************************************************************************
		//Private Sub cwneScaleDP_ValueChanged(Value As Variant, PreviousValue As Variant, ByVal OutOfRange As Boolean)
		//
		//    'イベントフラグがFalseの場合はぬける
		//    If byEvent = False Then Exit Sub
		//    'BHCテーブルに10枚以上画像があればグラフ描画
		//    If msgImgFile.Rows > GRAPH_DRAW_NEED_DATA Then
		//        'グラフの描画
		//        DrawGraph
		//    End If
		//
		//End Sub
		//v8.1　ここまで削除 コンポーネントワークスのcwneの使用をやめた。 by Ohkado 2007/04/18


		//*******************************************************************************
		//機　　能： ΔPのスケールパラメータ変更時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.1  2007/04/18   Ohkado     新規作成
		//*******************************************************************************
		private void ntbPDMax_ValueChanged(object sender, EventArgs e)
		{
			//イベントフラグがFalseの場合はぬける
			if (byEvent == false) return;
			//BHCテーブルに10枚以上画像があればグラフ描画
			if (msgImgFile.Rows.Count + 1 > GRAPH_DRAW_NEED_DATA)
			{
				//グラフの描画
				DrawGraph();
			}
		}


		//*******************************************************************************
		//機　　能： グリッドコントロール内でのクリック処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2007/01/19  (CATS)村田      新規作成
		//*******************************************************************************
		private void msgImgFile_Click(object sender, EventArgs e)
		{
			//a1～a6用編集用テキストボックスを初期化     'v8.1 追加 by Ohkado 2007/04/02
			//InittxtGridBHCPara                         'v8.1 削除 by Ohkado 2007/04/12

			//ファイル名かＰの列を選択した場合何もしない
            //if ((msgImgFile.CurrentCell.ColumnIndex + 1 == 1) ||
            //    (msgImgFile.CurrentCell.ColumnIndex + 1 == 4) ||
            //    string.IsNullOrEmpty(Convert.ToString(msgImgFile.CurrentCell.Value)) ||
            //    (msgImgFile.FirstDisplayedScrollingRowIndex + 1 == 1 && msgImgFile.CurrentCell.RowIndex + 1 == 1))
            //Rev20.00 修正 最後の条件式が入っていると最初の行の変更が効かない。
            if ((msgImgFile.CurrentCell.ColumnIndex + 1 == 1) ||
               (msgImgFile.CurrentCell.ColumnIndex + 1 == 4) ||
                string.IsNullOrEmpty(Convert.ToString(msgImgFile.CurrentCell.Value)))
            {
				//グリッドコントロール編集用テキストボックスを初期化'
				InitEditTxtbox();          //変更 by 村田 2007/01/26
				//欄固定を解除
#region 【C#コントロールで代用】
/*
				msgImgFile.AllowUserResizing = flexResizeColumns
*/
#endregion
				msgImgFile.AllowUserToResizeColumns = true;
				//スクロールバーをつける
				//msgImgFile.ScrollBars = flexScrollBarVertical
				//msgImgFile.ScrollBars = flexScrollBarBoth
				return;
			}
			
			//int tmpCol = 0;
			//int tmpRow = 0;
			//欄が動かない用に固定
#region 【C#コントロールで代用】
/*
			msgImgFile.AllowUserResizing = flexResizeNone
*/
#endregion
			msgImgFile.AllowUserToResizeColumns = false;
			msgImgFile.AllowUserToResizeRows = false;
			//左スクロールバーを消す
			//msgImgFile.ScrollBars = flexScrollBarNone

			OldRow = msgImgFile.CurrentCell.RowIndex + 1;
			OldCol = msgImgFile.CurrentCell.ColumnIndex + 1;
			//半径(mm)の場合
			if (OldCol == 2)
			{
				oldValue = DataofBHC[OldRow - 1].diaData;
			}
			//直径の場合
			else if (OldCol == 3)
			{
				oldValue = DataofBHC[OldRow - 1].rawdata;
			}

			//編集用テキストボックスを移動
#region 【C#コントロールで代用】
/*
			With msgImgFile
				txtGridEdit.Move .Left + .CellLeft, .Top + .CellTop, .CellWidth, .CellHeight
			End With
*/
#endregion
			Rectangle cell = msgImgFile.GetCellDisplayRectangle(msgImgFile.CurrentCell.ColumnIndex, msgImgFile.CurrentCell.RowIndex, true);
			txtGridEdit.SetBounds(msgImgFile.Left + cell.Left, msgImgFile.Top + cell.Top, cell.Width, cell.Height);

			//編集用テキストボックスの設定
			txtGridEdit.Visible = true;
			txtGridEdit.Text = Convert.ToString(msgImgFile.CurrentCell.Value);
			txtGridEdit.Focus();
		}


		//*******************************************************************************
		//機　　能： グリッドコントロール内でのクリック処理
		//           長いファイル名を表示するテキストボックスを表示する
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2007/01/19  (CATS)村田      新規作成
		//*******************************************************************************
		private void msgImgFile_DoubleClick(object sender, EventArgs e)
		{
			int StrWidth = 0;

			//欄が動かない用に固定
#region 【C#コントロールで代用】
/*
			msgImgFile.AllowUserResizing = flexResizeNone
*/
#endregion
			msgImgFile.AllowUserToResizeColumns = false;
			msgImgFile.AllowUserToResizeRows = false;

			//左スクロールバーを消す
			//msgImgFile.ScrollBars = flexScrollBarNone

#region 【C#コントロールで代用】
/*
			With msgImgFile
				'ダブルクリックした列がファイル名でない場合何もしない
				If Not .col = 1 Then Exit Sub
				StrWidth = TextWidth(.TextMatrix(.Row, .col))
				'デフォルトで表示できている場合は何もしない
				If .ColWidth(.col) > StrWidth + 100 Then Exit Sub
				txtGridFileName.Move .Left + .CellLeft, .Top + .CellTop, .CellWidth, .CellHeight
				txtGridFileName.Width = StrWidth + 100
				txtGridFileName.Text = .TextMatrix(.Row, .col)
				txtGridFileName.Visible = True
				txtGridFileName.SetFocus
				'文字列を選択状態にする
				txtGridFileName.SelStart = 0
				txtGridFileName.SelLength = Len(txtGridFileName.Text)
				txtGridFileName.Locked = True
			End With
*/
#endregion

			//ダブルクリックした列がファイル名でない場合何もしない
            //Rev20.00 P値をダブルクリックされた場合にも編集不可にするため、変更 by長野 2014/12/15
			//if (msgImgFile.CurrentCell.ColumnIndex != 0) return;
            //Rev20.00 ４行目も編集不可にする by長野 2015/02/24
            if (msgImgFile.CurrentCell.ColumnIndex != 0 && msgImgFile.CurrentCell.ColumnIndex != 3 && msgImgFile.CurrentCell.ColumnIndex != 4) return;

            //Rev20.00 P値用に条件式追加 by長野 2014/12/15
            if (msgImgFile.CurrentCell.ColumnIndex == 3)
            {
                msgImgFile.CurrentCell.ReadOnly = true;
            }
            else
            {
                Graphics g = msgImgFile.CreateGraphics();
                StrWidth = (int)Math.Round(g.MeasureString(Convert.ToString(msgImgFile.CurrentCell.Value), msgImgFile.CurrentCell.InheritedStyle.Font).Width, MidpointRounding.AwayFromZero);
                g.Dispose();

                //デフォルトで表示できている場合は何もしない
                if (msgImgFile.CurrentCell.Size.Width > StrWidth + 7) return;
                Rectangle cell = msgImgFile.GetCellDisplayRectangle(msgImgFile.CurrentCell.ColumnIndex, msgImgFile.CurrentCell.RowIndex, true);
                txtGridFileName.SetBounds(msgImgFile.Left + cell.Left, msgImgFile.Top + cell.Top, cell.Width, cell.Height);
                txtGridFileName.Width = StrWidth + 7;
                txtGridFileName.Text = Convert.ToString(msgImgFile.CurrentCell.Value);
                txtGridFileName.Visible = true;
                txtGridFileName.Focus();
                //文字列を選択状態にする
                txtGridFileName.SelectionStart = 0;
                txtGridFileName.SelectionLength = (txtGridFileName.Text).Length;
                txtGridFileName.ReadOnly = true;
            }
		}


		//*******************************************************************************
		//機　　能： リストボックス内でのキーダウン処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2007/01/10   Ohkado      新規作成
		//*******************************************************************************
		private void msgImgFile_KeyDown(object sender, KeyEventArgs e)
		{
			//キーを判定
			switch (e.KeyCode)
			{
				//[Del]キーで削除
				case Keys.Delete:
					//[削除]ボタンを表示していたら実行
					if (cmdImgDelete.Enabled)
					{
						cmdImgDelete_Click(cmdImgDelete, EventArgs.Empty);
					}
					break;
			}
		}


		//*******************************************************************************
		//機　　能： リストボックス内でのスクロールバーを動かした時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2007/01/10   Ohkado      新規作成
		//*******************************************************************************
		private void msgImgFile_Scroll(object sender, ScrollEventArgs e)
		{
			txtGridEdit.Visible = false;
			txtGridFileName.Visible = false;
		}


		//*******************************************************************************
		//機　　能： リストボックス内でのクリック処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2007/01/19  (CATS)村田      新規作成
		//*******************************************************************************
		private void msgImgFile_SelectionChanged(object sender, EventArgs e)
		{
			cmdImgDelete.Enabled = !string.IsNullOrEmpty(Convert.ToString(msgImgFile.CurrentCell.Value)) && (DataOfBHCCount != 0);		//v8.1:And～追加 by Ohkado 2007/04/16
		}


		//*******************************************************************************
		//機　　能： 「テーブル読み込み」をクリックする
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdBHCTableOpen_Click(object sender, EventArgs e)
		{
			//BHCﾃｰﾌﾞﾙが未保存で変更されている場合、処理を続行させるかどうかのチェック
			if (!DoContinue()) return;
            try
			{
				//BHCテーブル作成ダイアログ

				//コモンダイアログ設定
				dlgBHCTableOpen.Title = StringTable.GetResString(StringTable.IDS_Open, CTResources.LoadResString(StringTable.IDS_BHCTable));			//ＢＨＣテーブルを開く
                dlgBHCTableOpen.Filter = modLibrary.MakeCommonDialogFilter(CTResources.LoadResString(StringTable.IDS_BHCTable), "-BHC.csv");
                
                dlgBHCTableOpen.FileName = "";
				dlgBHCTableOpen.ShowReadOnly = false;							//[読み取り専用]チェックボックス非表示

				//v19.00
				dlgBHCTableOpen.InitialDirectory = AppValue.InitDir_BHCTable;

				//コモンダイアログを表示
				DialogResult result = dlgBHCTableOpen.ShowDialog();

				//エラーの場合、抜ける
				if (result == DialogResult.Cancel)
				{
					return;
				}
			}
			catch (Exception ex)
			{
				//キャンセルボタン選択時以外のエラーの場合、エラーメッセージを表示
				MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//コモンダイアログのリフレッシュ
			this.Refresh();

			//BHCテーブルの初期化
			Changed = false;
			cmdBHCTableNew_Click(cmdBHCTableNew, EventArgs.Empty);

			//BHCテーブルの読み込み処理
			if (LoadBHCTable(dlgBHCTableOpen.FileName))
			{
				//BHCテーブル名
				txtBHCTable.Text = dlgBHCTableOpen.FileName;

				//次にBHCテーブルを読み込むディレクトリを保存しておく
				dlgBHCTableOpen.InitialDirectory = dlgBHCTableOpen.FileName;

				//v19.00 BHCテーブル読み込みディレクトリ保存
				AppValue.InitDir_BHCTable = dlgBHCTableOpen.FileName;

				//マウスポインタを砂時計に変更
				this.Cursor = Cursors.WaitCursor;

				//マウスポインタを元に戻す
				this.Cursor = Cursors.Default;

				//変更フラグクリア
				Changed = false;
			}
			else
			{
				//メッセージ
				//BHCﾃｰﾌﾞﾙを開くことができませんでした。
				//下記の原因が考えられます：
				//　・テーブルが存在しない
				//  ・テーブルが壊れている
				//再度BHCﾃｰﾌﾞﾙを作り直してください。

				MessageBox.Show(CTResources.LoadResString(17221) + "\r" + 
								CTResources.LoadResString(17214) + "\r" + "\r" + 
								CTResources.LoadResString(17222) + "\r" + 
								CTResources.LoadResString(17223) + "\r" + "\r" + 
								CTResources.LoadResString(17211), 
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

				Changed = false;

				//新規テーブル作成処理
				cmdBHCTableNew_Click(cmdBHCTableNew, EventArgs.Empty);

				return;
			}


			//グラフの作成　※テーブル読み込み時はフィッテイングを行ってはいけない
			if (msgImgFile.Rows.Count + 1 > GRAPH_DRAW_NEED_DATA && msgImgFile.Rows.Count <= MAX_DATA_COUNT && CalculateA() > 0)
			{
				//グラフの描画
				DrawGraph();
			}
			else		//登録された画像が10以下の場合のエラー処理
			{
				//メッセージ
				//BHCﾃｰﾌﾞﾙを開くことができませんでした。
				//下記の原因が考えられます：
				//　・テーブルが存在しない
				//  ・テーブルが壊れている
				//再度BHCﾃｰﾌﾞﾙを作り直してください。

				MessageBox.Show(CTResources.LoadResString(17221) + "\r" + 
								CTResources.LoadResString(17214) + "\r" + "\r" + 
								CTResources.LoadResString(17222) + "\r" + 
								CTResources.LoadResString(17223) + "\r" + "\r" + 
								CTResources.LoadResString(17211), 
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

				Changed = false;

				//新規テーブル作成処理
				cmdBHCTableNew_Click(cmdBHCTableNew, EventArgs.Empty);

				return;
			}
		}


		//*******************************************************************************
		//機　　能： BHCテーブル読み込み処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： FileName        [I/ ] String    読み込み対象ファイル名
		//戻 り 値：                 [ /O] Boolean   True:読み込み成功　False:読み込み失敗
		//
		//補　　足：
		//
		//履　　歴： v8.00 2007/01/16 (CATS)Ohkado    新規作成
		//          v19.00 2012/03/15 H.Nagai         P0読み込み追加
		//*******************************************************************************
		private bool LoadBHCTable(string FileName)
		{
			string buf = null;
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Dim fileNo          As Integer
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
			string[] Cell = null;
			string Filebuf = null;
			string abuf = null;
			int DotPos = 0;
			int i = 0;
			int j = 0;
			bool IsErrorBHCTable = false;

            //追加2015/01/27hata
            // ソート対象のDictionary<string, float>
            Dictionary<string, float> dict = new Dictionary<string, float>();

			//削除ボタンを使用不可にする
			cmdImgDelete.Enabled = false;

			//戻り値初期化
			bool functionReturnValue = false;

			//BHCテーブルを初期化
			InitBHCTable();

			//DataOfBHCCountを初期化
			InitDataofBHC();

			//エラーフラグを初期化
			IsErrorBHCTable = true;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			'エラー時の扱い
				On Error GoTo ErrorHandler

				'ファイルオープン
				fileNo = FreeFile()
				Open FileName For Input As #fileNo
    
				i = 1: j = 1

				Do While Not EOF(fileNo)
        
					'ファイルから読み込む
					Line Input #fileNo, buf
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			StreamReader sr = null;

			try
			{	//ファイルオープン
                //変更2015/01/22hata
                //sr = new StreamReader(FileName, Encoding.Default);
                sr = new StreamReader(FileName, Encoding.GetEncoding("shift-jis"));
                
				i = 1;
				j = 1;

				//ファイルから読み込む
				while ((buf = sr.ReadLine()) != null)
				{
					if (!string.IsNullOrEmpty(buf))
					{
						Cell = buf.Split(',');

						//１列目の項目をチェック
						if (Cell[0].Trim() == CTResources.LoadResString(17204))	//材質(Material)
						{
							//Materialを取り出す
							DotPos = buf.IndexOf(",") + 1;
							txtMaterial.Text = (DotPos > 0) ? buf.Substring(DotPos) : "";
						}
						else if (Cell[0].Trim() == CTResources.LoadResString(17205))	//コメント
						{
							//commentを取り出す
							DotPos = buf.IndexOf(",") + 1;
							txtComment.Text = (DotPos > 0) ? buf.Substring(DotPos) : "";

						}
						else		//その他
						{
							//FileName[i]なら情報を取り出す
							Filebuf = "FileName" + Convert.ToString(j);
							if ((Cell[0] == Filebuf) && (Cell.GetUpperBound(0) == 4))
							{
								//BHC内部データに追加
								DataofBHC[DataOfBHCCount].FileName = Cell[1].Trim();

								//直径が適正かどうか？                                   'v8.1 ここから追加(Errorﾁｪｯｸ) by Ohkado 2007/04/16
								float TrimCell2 = 0;
								if (float.TryParse(Cell[2].Trim(), out TrimCell2))
								{
									if (MAX_DIAMETER > TrimCell2)
									{
										DataofBHC[DataOfBHCCount].diaData = TrimCell2;
									}
									else
									{
										IsErrorBHCTable = false;
									}
								}
								else
								{
									IsErrorBHCTable = false;
								}

								//生データ値が適正かどうか？
								float TrimCell3 = 0;
								if (float.TryParse(Cell[3].Trim(), out TrimCell3))
								{
									if (MAX_RAWDATA > TrimCell3)
									{
										DataofBHC[DataOfBHCCount].rawdata = TrimCell3;
									}
									else
									{
										IsErrorBHCTable = false;
									}
								}
								else
								{
									IsErrorBHCTable = false;
								}

								//P値は適正かどうか?
								float TrimCell4 = 0;
								if (float.TryParse(Cell[4].Trim(), out TrimCell4))
								{
									if (CalculateXRay_damping_exponent(MAX_RAWDATA, 0) > TrimCell4)
									{
										DataofBHC[DataOfBHCCount].pData = TrimCell4;
									}
									else
									{
										IsErrorBHCTable = false;
									}
								}
								else
								{
									IsErrorBHCTable = false;
								}													//v8.1 ここまで追加(Errorﾁｪｯｸ) by Ohkado 2007/04/16

								//v19.00 付帯情報チェック追加
								if (!IsOkAddImageInfo(DataofBHC[DataOfBHCCount].FileName))
								{
									IsErrorBHCTable = false;
								}

								DataOfBHCCount = DataOfBHCCount + 1;

								//100枚以上画像を読み込んだ場合
								if (j > MAX_DATA_COUNT) IsErrorBHCTable = false;

								//ｴﾗｰ時にループを抜ける
								if (IsErrorBHCTable == false) break;

                                //変更2015/01/27hata
                                //データ並び順昇順になるため、データセットは後にする。
								//BHCテーブルの値をGUIに表示する
								//AddToBHCTable(msgImgFile.Rows.Count + 1, DataofBHC[DataOfBHCCount - 1].FileName, DataofBHC[DataOfBHCCount - 1].diaData, DataofBHC[DataOfBHCCount - 1].rawdata, DataofBHC[DataOfBHCCount - 1].pData);
                                //ソート用データにセットする
                                dict.Add(j.ToString(),  DataofBHC[DataOfBHCCount - 1].diaData);

								//jのｲﾝｸﾘﾒﾝﾄ
								j = j + 1;
							}


							//v19.00
							//p0なら情報を取り出す
							if ((Cell[0] == "p0") && (Cell.GetUpperBound(0) == 1))
							{
								//p0を取り出す
								DotPos = buf.IndexOf(",") + 1;
								//p0が数字なら情報を取り出す
								float p0 = 0;
								if (float.TryParse(buf.Substring(DotPos), out p0))
								{
									bhc_p0 = (DotPos > 0) ? p0 : 0;
								}
								else
								{
									IsErrorBHCTable = false;
									break;
								}
							}


							//a[i]なら情報を取り出す
							abuf = "a" + Convert.ToString(i);
							if ((Cell[0] == abuf) && (Cell.GetUpperBound(0) == 1))
							{
								//a[i]を取り出す
								//DotPos = buf.IndexOf(",") - 1;
                                DotPos = buf.IndexOf(",") + 1;
								
                                //a[i]が数字なら情報を取り出す
								float a = 0;
								if (float.TryParse(buf.Substring(DotPos), out a))
								{
									bhc_a[i] = (DotPos > 0) ? a : 0;
									//bhc_a(i) = CSng(bhc_a(i))

									byEvent = false;
									//cwneA(i).Value = bhc_a(i)              'v8.1削除 by Ohkado 2007/04/12
									ntbApara[i].Value = (decimal)RoundOFFAPara((float)bhc_a[i]);			//v8.1追加 by Ohkado 2007/04/12
									//ntbApara(i).Value = bhc_a(i)
									byEvent = true;
									//iのｲﾝｸﾘﾒﾝﾄ
									i = i + 1;
								}
								else
								{
									//v8.1 追加:BHCﾃｰﾌﾞﾙのaの値が読み込めなかった場合の処理 by Ohkado 2007/04/16
									IsErrorBHCTable = false;
									break;
								}
							}
						}
					}
				}

				//追加2015/01/27hata
                //ここでデータをセットする
                DataSortAndAddTable(dict);


                functionReturnValue = (IsErrorBHCTable == true);

                //ﾃｰﾌﾞﾙを開くに成功したら[削除ボタン]を使用可能にする
                if (IsErrorBHCTable) cmdImgDelete.Enabled = true;

            }
			catch (Exception ex)
			{
				//エラーメッセージの表示
				MessageBox.Show(CTResources.LoadResString(9965) + FileName + "\r" + "\r" + ex.Message, 
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			finally
			{
				//ファイルクローズ
				if (sr != null)
				{
					sr.Close();
					sr = null;
				}
			}



			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： 有効桁数８桁目を四捨五入して表示する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.10  2007/04/23   (CATS)Ohakdo      新規作成
		//*******************************************************************************
		private float RoundOFFAPara(float theValue)
		{
			float functionReturnValue = 0;

			int i = 0;
			string buf = null;
			int K = 0;
			bool IsMainas = false;

			K = 0;
			for (i = 1; i <= theValue.ToString("0.######0").Length; i++)
			{
				buf = (theValue.ToString("0.######0")).Substring(i - 1, 1);
				if (K == 0 && buf == "0") break;
				if (buf == ".") break;
				K = K + 1;
				if (buf == "-")
				{
					K = K - 1;
					IsMainas = true;
				}
			}

			if (IsMainas) theValue = theValue * -1;

			if (K == 0)
			{
				float.TryParse(RoundOff(theValue, K - 9).ToString("0.0000000") , out functionReturnValue);
			}
			else if (K == 1)
			{
				float.TryParse(RoundOff(theValue, K - 9).ToString("0.000000") , out functionReturnValue);
			}
			else if (K == 2)
			{
				float.TryParse(RoundOff(theValue, K - 9).ToString("0.00000") , out functionReturnValue);
			}
			else if (K == 3)
			{
				float.TryParse(RoundOff(theValue, K - 9).ToString("0.0000") , out functionReturnValue);
			}
			else if (K == 4)
			{
				float.TryParse(RoundOff(theValue, K - 9).ToString("0.000") , out functionReturnValue);
			}
			else if (K == 5)
			{
				float.TryParse(RoundOff(theValue, K - 9).ToString("0.00") , out functionReturnValue);
			}
			else
			{
				float.TryParse(RoundOff(theValue, K - 9).ToString("0.00") , out functionReturnValue);
			}

			if (IsMainas) functionReturnValue = functionReturnValue * -1;

			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： 「テーブル保存」ボタンクリック時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2006/12/28   (CATS)Ohakdo      新規作成
		//*******************************************************************************
		private void cmdBHCTableSave_Click(object sender, EventArgs e)
		{
            SaveBHCTable(GetBHCTableName());
		}


		//*******************************************************************************
		//機　　能： BHCテーブル名（ファイル名）の取得
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v8.00 2006/12/28 (CATS)Ohkado    新規作成
		//*******************************************************************************
		private string GetBHCTableName()
		{
			string KeyName = null;
			int DotPos = 0;

			//戻り値初期化
			string functionReturnValue = "";

			if (msgImgFile.Rows.Count + 1 <= GRAPH_DRAW_NEED_DATA)								//v8.1変更 by Ohado 2007/04/16
			{
				//メッセージ
				//BHCテーブルを作成するには、10枚以上のデータが必要です"
				MessageBox.Show(StringTable.GetResString(17208, CTResources.LoadResString(StringTable.IDS_BHCTable)), 
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);					//リソース:17208 BHCテーブル
				return functionReturnValue;
			}

			try
			{
				//ダイアログ設定
				//BHCテーブル(*-BHC.csv)|*-BHC.csv|すべてのファイル(*.*)|*.*
				dlgBHCTableSave.Title = StringTable.GetResString(StringTable.IDS_Save, CTResources.LoadResString(StringTable.IDS_BHCTable));		//ＢＨＣテーブルの保存
                dlgBHCTableSave.Filter = modLibrary.MakeCommonDialogFilter(CTResources.LoadResString(StringTable.IDS_BHCTable), "-BHC.csv");
                //dlgBHCTableSave.FileName = txtBHCTable.Text;
                if (string.IsNullOrEmpty(txtBHCTable.Text))
                {
                    dlgBHCTableSave.FileName = "";
                }
                else
                {
                    dlgBHCTableSave.FileName = Path.GetFileName(txtBHCTable.Text);
                }

				//v19.00
				dlgBHCTableSave.InitialDirectory = AppValue.InitDir_BHCTable;

				//ダイアログボックスを表示
				DialogResult result = dlgBHCTableSave.ShowDialog();

				//エラーの場合、抜ける
				if (result == DialogResult.Cancel)
				{
					return functionReturnValue;
				}

				KeyName = Path.GetFileName(dlgBHCTableSave.FileName);
				DotPos = KeyName.LastIndexOf(".");
				if (DotPos > 0) KeyName = KeyName.Substring(0, DotPos);

				//BHCテーブル名を設定
                //変更2014/11/05hata
                functionReturnValue = modLibrary.RemoveExtension(dlgBHCTableSave.FileName, Path.GetFileName(dlgBHCTableSave.FileName))
                                    + modLibrary.AddExtension(KeyName, "-BHC") + ".csv";
                
				//v19.00 BHCテーブル保存ディレクトリを保存
                AppValue.InitDir_BHCTable = Path.GetDirectoryName(dlgBHCTableSave.FileName);
			}
			catch (Exception ex)
			{
				//キャンセルボタン選択時以外のエラーの場合、エラーメッセージを表示
				MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： BHCテーブル保存処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v8.00 2007/1/16 (CATS)Ohkado    新規作成
		//*******************************************************************************
		private bool SaveBHCTable(string FileName)
		{
			string buf = null;
			int i = 0;

			//戻り値初期化
			bool functionReturnValue = false;

			//ファイル名が未指定の場合、抜ける
			if (string.IsNullOrEmpty(FileName)) return functionReturnValue;

			//v19.00 デフォルトのテーブルの上書きは禁止する by長野
			//v19.12 英語化対応 by長野 2013/02/20
			//If FileName = "C:\CTUSER\BHCﾃｰﾌﾞﾙ\DEFAULT_TABLE-BHC.csv" Then
			if (FileName == AppValue.InitDir_BHCTable + @"\DEFAULT_TABLE-BHC.csv")
			{
				MessageBox.Show(CTResources.LoadResString(21309), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return functionReturnValue;
			}

            //指定ファイルがすでに存在している場合
            if (File.Exists(FileName))
            {
                //確認メッセージ表示：～が存在します。上書きしますか？
                DialogResult result = MessageBox.Show(StringTable.GetResString(9915, FileName), CTResources.LoadResString(10515),
                                                      MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Cancel)
                {
                    return functionReturnValue;
                }
            }

			//ヘッダの生成
			buf = CTResources.LoadResString(17198);					//BHC Parameter
			buf = buf + "," + " ";									//
			buf = buf + "," + CTResources.LoadResString(17199);		//直径(mm)
			buf = buf + "," + CTResources.LoadResString(17200);		//生データ
			buf = buf + "," + CTResources.LoadResString(17201);		//P

			StreamWriter sw = null;

			//エラー時の設定
			try 
			{
				//ファイルオープン
                //変更2015/01/22hata
                //sw = new StreamWriter(FileName, false,Encoding.Default);
                sw = new StreamWriter(FileName, false, Encoding.GetEncoding("shift-jis"));

				//ヘッダの書き込み
				sw.WriteLine(buf);

				//v19.00 P0書き込み
				buf = "p0" + "," + bhc_p0.ToString("0.######0");
				sw.WriteLine(buf);

				//Dimension と 次数の数(BHC_DIMENSION)の生成
				buf = CTResources.LoadResString(17202);
				buf = buf + "," + Convert.ToString(BHC_DIMENSION);

				//Dimensionと次数の数の書き込み
				sw.WriteLine(buf);

				for (i = 1; i <= BHC_DIMENSION; i++)
				{
					//求めた次数の値の生成
					buf = "a" + Convert.ToString(i);					//a[i]
					buf = buf + "," + bhc_a[i].ToString("0.######0");	//次数の値

					//求めた次数の値の書き込み
					sw.WriteLine(buf);
				}

				//Number of Image と 入力画像枚数の設定
				buf = CTResources.LoadResString(17203);
				buf = buf + "," + Convert.ToString(msgImgFile.Rows.Count);

				//Number of Image と 入力画像枚数の書き込み
				sw.WriteLine(buf);
    
				for (i = 0; i <= DataOfBHCCount - 1; i++)
				{
					//FileName[i],画像ファイル名、直径、生データ、Pを生成
					buf = "FileName" + Convert.ToString(i + 1);
					buf = buf + "," + DataofBHC[i].FileName;
					buf = buf + "," + DataofBHC[i].diaData.ToString("0");
					buf = buf + "," + DataofBHC[i].rawdata.ToString("0.00");
					buf = buf + "," + DataofBHC[i].pData.ToString("0.000000");

					//FileName[i],画像ファイル名、直径、生データ、Pの書き込み
					sw.WriteLine(buf);
				}


				//フッタの書き込み
				sw.WriteLine(CTResources.LoadResString(17204) + "," + modLibrary.RemoveCRLF(txtMaterial.Text));				//Material(改行ｺｰﾄﾞ等はｽﾍﾟｰｽに置換)
				sw.WriteLine(CTResources.LoadResString(17205) + "," + modLibrary.RemoveCRLF(txtComment.Text));				//comment(改行ｺｰﾄﾞ等はｽﾍﾟｰｽに置換)


				//戻り値セット
				functionReturnValue = true;

				//変更フラグクリア
				Changed = false;

				//保存ファイル名を「BHCテーブル名」欄に表示する
				txtBHCTable.Text = FileName;

				//｢元に戻す｣機能クリア
				ReturnCount = 0;
				ReturnIndex = 0;
				cmdBack.Enabled = false;
			}
			catch (Exception ex)
			{
				//エラーメッセージの表示
				MessageBox.Show(CTResources.LoadResString(9968) + FileName + "\r" + "\r" + ex.Message, 
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			finally
			{
				//ファイルクローズ
				if (sw != null)
				{
					sw.Close();
					sw = null;
				}
			}

			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： フォームロード時の処理（イベント処理）
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2006/12/28   (CATS)Ohkado      新規作成
		//*******************************************************************************
		private void frmBeamHardeningCorrection_Load(object sender, EventArgs e)
		{
			//実行時はフラグをセット
			modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTBHCTable;

			//変数初期化
			Changed = false;						//変更フラグ
			byEvent = false;							//イベントフラグ
			PamitDrawGraph = false;					//グラフ作成許可フラグ

			//キャプションのセット
			SetCaption();

			//各コントロールを初期設定する
			InitControls();

			//ラベルの設定
			FormLabeling();

			//「新規ﾃｰﾌﾞﾙ作成」ボタンクリック時処理と同じ処理
			cmdBHCTableNew_Click(cmdBHCTableNew, EventArgs.Empty);

			//ImageProを起動する:起動していない場合に備える 'V8.1 追加 by Ohkado 2007/04/11
			modCT30K.StartImagePro();

			//フォームロード終了処理
			byEvent = true;
		}


		//*******************************************************************************
		//機　　能： フォームアンロード時処理（イベント処理）
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Cancel          [I/ ] Integer   True（0以外）: アンロードをキャンセル
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2007/01/17   (CATS)Ohkado      新規作成
		//*******************************************************************************
		private void frmBeamHardeningCorrection_FormClosed(object sender, FormClosedEventArgs e)
		{
			//終了時はフラグをリセット
			modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTBHCTable);
		}


		//*******************************************************************************
		//機　　能： 各コントロールのキャプションをリソースからセットします
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v8.0  2006/12/28 (CATS)Ohkado      未完成
		//           v8.30 2008/23/4  本間           英語化対応
		//*******************************************************************************
		private void SetCaption()
		{
			this.Text = CTResources.LoadResString(21004);												//ビームハードニング補正テーブルの編集

			lblBHCTableName.Text = StringTable.LoadResStringWithColon(17226);											//編集中のテーブル名：
			lblNumTitle.Text = StringTable.LoadResStringWithColon(StringTable.IDS_SelectedImageNum);					//入力枚数：
			lblMaxNum.Text = StringTable.GetResString(StringTable.IDS_FramesWithMax, MAX_DATA_COUNT.ToString());		//枚（最大100枚）
			lblMaterial.Text = StringTable.LoadResStringWithColon(17530);												//材質：
			lblComment.Text = StringTable.LoadResStringWithColon(StringTable.IDS_Comment);								//コメント：

			fraSpecify.Text = CTResources.LoadResString(StringTable.IDS_Specify);											//指定
			optFolder.Text = CTResources.LoadResString(StringTable.IDS_Folder);											//フォルダ
			optFile.Text = CTResources.LoadResString(StringTable.IDS_File);												//ファイル
			lblBHCGraphe.Text = CTResources.LoadResString(17531);															//ビームハードニング補正カーブ
			lblBHCSubGraphe.Text = CTResources.LoadResString(17532);														//直線と補正カーブの差分
			lblScaleDP.Text = StringTable.LoadResStringWithColon(17533);												//縦軸倍率：
			lblP.Text = CTResources.LoadResString(17534);																	//P
			lblDP.Text = CTResources.LoadResString(17535);																//ΔP
			lblPD1.Text = CTResources.LoadResString(17536);																//P´
			lblPD2.Text = CTResources.LoadResString(17536);																//P´
			lblDiameter_mm.Text = CTResources.LoadResString(17537);														//直径(mm)
			cmdImgSelect.Text = CTResources.LoadResString(17538);															//参　照
			cmdBHCTableNew.Text = CTResources.LoadResString(17539);														//新規テーブル作成
			cmdBHCTableOpen.Text = CTResources.LoadResString(17540);														//テーブル読み込み
			cmdBHCTableSave.Text = CTResources.LoadResString(17541);														//テーブル保存
			cmdImgDelete.Text = CTResources.LoadResString(StringTable.IDS_btnDel);										//削　除
			cmdBHCTableExit.Text = CTResources.LoadResString(StringTable.IDS_btnClose);									//閉じる
			cmdBack.Text = CTResources.LoadResString(StringTable.IDS_btnUndo);											//元に戻す

			//コモンダイアログのパラメータ設定
			//Rev8.4 相対パス対応 by YAMAKAGE 08-05-28
			//v19.00
			//dlgBHCTable.InitDir = CTPATH & LoadResString(IDS_BHCTableDir)                                              '***\ctuser\BHCﾃｰﾌﾞﾙ
			//    dlgBHCTable.InitDir = LoadResString(10250)                                              'c:\ctuser\BHCﾃｰﾌﾞﾙ

            dlgBHCTableOpen.Filter = modLibrary.MakeCommonDialogFilter(CTResources.LoadResString(StringTable.IDS_BHCTable), "-BHC.csv");		//BHCﾃｰﾌﾞﾙ(*-BHC.csv)|*-BHC.csv|全てのファイル(*.*)|*.*
            dlgBHCTableSave.Filter = modLibrary.MakeCommonDialogFilter(CTResources.LoadResString(StringTable.IDS_BHCTable), "-BHC.csv");		//BHCﾃｰﾌﾞﾙ(*-BHC.csv)|*-BHC.csv|全てのファイル(*.*)|*.*
            dlgBHCTableOpen.DefaultExt = "Csv";
			dlgBHCTableSave.DefaultExt = "Csv";

			//英語環境の場合、ラベルコントロールに使用するフォントをArialにする
			//    If IsEnglish Then SetLabelFont Me
			if (modCT30K.IsEnglish)			//ver8.30本間変更 2008/3/19
			{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				Dim theControl As Control
				For Each theControl In Me.Controls
					Select Case TypeName(theControl)
						Case "Label", "OptionButton", "ComboBox", "CheckBox", "Frame", "TextBox", "CommandButton", "SSTab", "CWNumEdit", "MSFlexGrid"
							theControl.Font.Name = "Arial"
							theControl.Font.SIZE = 10
					End Select
				Next
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				SetFont(this, new Font("Arial", 10));
			}
		}

		/// <summary>
		/// 指定したコントロールのフォントを、指定したフォントに変更する
		/// </summary>
		private void SetFont(Control Control, Font Font)
		{
			foreach (Control theControl in Control.Controls)
			{
				switch (theControl.GetType().Name)
				{
					case "Label":
					case "RadioButton":
					case "ComboBox":
					case "CheckBox":
					case "Panel":
					case "GroupBox":
					case "TextBox":
					case "Button":
					case "TabControl":
					case "NumericUpDown":
					case "DataGridView":
						theControl.Font = Font;
						break;
				}

				SetFont(theControl, Font);
			}
		}


		//*******************************************************************************
		//機　　能： 円柱の直径を求める
		//
		//           変数名          [I/O] 型        内容
		//引　　数： FileName        [I/ ] String    処理画像のファイル名
		//           DIAMETER        [I/ ] Single    円柱の直径
		//           byThresholdAll  [I/ ] Boolean   真円率の閾値を有効にするか可否
		//戻 り 値：                 [ /O] boolean   処理結果 TRUE:正常終了 FALSE:異常終了
		//
		//補　　足： なし
		//
		//履　　歴： V8.0       06/12/27  (CATS)村田    新規作成
		//           V8.1     2007/04/22  by Ohakdo     真円率による画像評価を削除
		//           V19.00     12/02/22  H.Nagai       frmDistanceCorrect.DoDistanceCorrectにあわせた
		//*******************************************************************************
		//Public Function GetColumnDiameter(ByVal FileName As String, ByRef diameter As Single, ByRef byThresholdAll As Boolean) As Boolean
		public bool GetColumnDiameter(string FileName, ref float DIAMETER)
		{
			int xSize = 0;
			int ySize = 0;
			ushort[] DiaImage;
			int MinValue = 0;
			int MaxValue = 0;
			int Threshold = 0;
			ushort[] BinImage;
			int rc = 0;
			//Ipc32v5.RECT DiaImgRect = default(Ipc32v5.RECT);
			string clptxt = null;
			//string swork = null;
			//short cnt = 0;
			int icount = 0;
			string InfFileName = null;
			float PixelSize = 0;
			float[] ShapeInfo = new float[2];

			bool functionReturnValue = false;

            //modImageInfo.ImageInfoStruct Inf = default(modImageInfo.ImageInfoStruct);
            ImageInfo Inf = new ImageInfo();

            Inf.Data.Initialize();



			//入力画像のサイズからレイアウトを判定する
			DiaImage = new ushort[(new FileInfo(FileName).Length / 2) + 1];
			xSize = Convert.ToInt32(Math.Sqrt(DiaImage.GetUpperBound(0)));
			ySize = xSize;

			if (ScanCorrect.ImageOpen(ref DiaImage[0], FileName, xSize, ySize) != 0)
			{
				return functionReturnValue;
			}

			//画像の最大値、最小値を求める
			ScanCorrect.GetMaxMin(ref DiaImage[0], xSize, ySize, ref MinValue, ref MaxValue);
			Threshold = (MaxValue + MinValue) / 2;
			//最大値と最小値の絶対差が20以下の場合はdiameter(直径)を0とする   'v8.1追加 by Ohkado 2007/04/02
			if (Math.Abs(MaxValue - MinValue) <= 20)
			{
				DIAMETER = 0;
				functionReturnValue = true;
				return functionReturnValue;
			}

			//画像の2値化
			BinImage = new ushort[(new FileInfo(FileName).Length / 2) + 1];
			ScanCorrect.BinarizeImage_signed(ref DiaImage[0], ref BinImage[0], xSize, ySize, Threshold, 1, 1);


            #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
            /*
            //開いている全ての画像ｳｨﾝﾄﾞを閉じる
			rc = Ipc32v5.IpAppCloseAll();

			//空の画像ウィンドウを生成（Gray Scale 16形式）
			rc = Ipc32v5.IpWsCreate((short)xSize, (short)ySize, 300, Ipc32v5.IMC_GRAY16);

			//直径を求める画像データをImage-Proの新しく作成した空ウィンドウに書き込む
			DiaImgRect.Left = 0;
			DiaImgRect.Top = 0;
			DiaImgRect.Right = xSize - 1;
			DiaImgRect.Bottom = ySize - 1;
			rc = Ipc32v5.IpDocPutArea(Ipc32v5.DOCSEL_ACTIVE, ref DiaImgRect, ref BinImage[0], Ipc32v5.CPROG);
			//画像ウィンドウの再描画
			rc = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);
			//Image-Proの(カウント/サイズ)コマンドに関連する設定
			//画像内のオブジェクトをImage-Proが自動抽出するように設定
			rc = Ipc32v5.IpBlbSetAttr(Ipc32v5.BLOB_AUTORANGE, 1);
			//明るい色のオブジェクトを自動抽出するように設定
			rc = Ipc32v5.IpBlbSetAttr(Ipc32v5.BLOB_BRIGHTOBJ, 1);
			//(カウント/サイズ)の実行に備えて面積を選択状態にする
			rc = Ipc32v5.IpBlbEnableMeas(Ipc32v5.BLBM_AREA, 1);

			//v19.00 コメント
			//(カウント/サイズ)の実行に備えて真円度を選択状態にする
			rc = Ipc32v5.IpBlbEnableMeas(Ipc32v5.BLBM_ROUNDNESS, 1);

			//オブジェクトの輪郭内で閾値以下の画素も面積としてカウントできるように設定
			rc = Ipc32v5.IpBlbSetAttr(Ipc32v5.BLOB_FILLHOLES, 1);

			//v19.00 範囲を変更
			//測定する範囲を制限
			//rc = IpBlbSetFilterRange(BLBM_AREA, 50#, 1000000#)
			rc = Ipc32v5.IpBlbSetFilterRange(Ipc32v5.BLBM_AREA, 200.0F, xSize * ySize);

			//"Count/Size"ダイアログを開く
			rc = Ipc32v5.IpBlbShow(1);
			//IpBlBEnableMeasで選択した項目を実際に計測する
			rc = Ipc32v5.IpBlbCount();
			//再描画
			rc = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);
			//計測結果をクリップボードにコピー
			rc = Ipc32v5.IpBlbShowData(1);
			rc = Ipc32v5.IpBlbSaveData("", Ipc32v5.S_CLIPBOARD + Ipc32v5.S_Y_AXIS);
			rc = Ipc32v5.IpBlbShowData(0);
			//"Count/Size"ダイアログを閉じる
			rc = Ipc32v5.IpBlbShow(0);
            */
            rc = CallImageProFunction.CallGetColumnDiameter(BinImage, ySize, xSize);
            #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//


			clptxt = Clipboard.GetText(TextDataFormat.Text);

            if (string.IsNullOrEmpty(clptxt))
			{
				DIAMETER = 0;
				functionReturnValue = true;
				return functionReturnValue;
			}

            //クリップボードから得た測定値から、真円度と面積を取り出す
			makeInfoFromImStr(clptxt, ref ShapeInfo);
			//
			//v8.1ここから削除:新円率による判定をやめた by Ohkado 2007/04/12
			//
			//    '真円度が閾値を下回った場合・ノイズが多い場合・BHCファントムと認識されない画像の取り込みを許可がない場合
			//    If (ShapeInfo(1) > THRESHOLD_VALUE Or (ShapeInfo(1) = 0 And ShapeInfo(1) = 0)) And byThresholdAll = False Then
			//        '砂時計を元に戻す
			//        Screen.MousePointer = vbDefault
			//        '確認ダイアログ表示：
			//        '   BHCファントムの画像として認識されない画像がありました。
			//        '   処理を継続しますか？。
			//        Call SetBHCMessage(FileName)      'v8.1追加 by Ohkado 2007/04/10
			//
			//        'BHCファントム画像参照時のエラー一覧表示
			//        frmBHCMessage.Show vbModal
			//        'BHCMessageFormを消す
			//        Me.Refresh
			//        '砂時計を表示
			//        Screen.MousePointer = vbHourglass
			//        '｢いいえ｣(BHCContinueOK = 2)の場合、BHCテーブルに追加しない
			//        If BHCContinueOK = 2 Then Exit Function
			//        '｢はい｣(BHCContinueOK = 3)の場合、データ追加分については真円率と無関係に追加する
			//        If BHCContinueOK = 3 Then byThresholdAll = True
			//
			//    End If
			//v8.1ここまで削除:新円率による判定をやめることになった by Ohkado 2007/04/12

			icount = Convert.ToInt32(ShapeInfo[0]);

			//付帯情報読み込み
			InfFileName = FileName.Substring(0, FileName.Length - 4);
			//if (!modImageInfo.ReadImageInfo(ref Inf, InfFileName))
            if (!ImageInfo.ReadImageInfo(ref Inf.Data, InfFileName))
            {
				return functionReturnValue;
			}

			//１画素サイズを求める
			//v19.00 inf.scaleは1000で割る
			//PixelSize = Val(Inf.scale) / Val(Trim(Inf.matsiz))
			float scale = 0;
			float matsiz = 0;
            float.TryParse(Inf.Data.scale.GetString(), out scale);
            float.TryParse(Inf.Data.matsiz.GetString().Trim(), out matsiz);
			PixelSize = scale / 1000F / matsiz;

			//面積から直径を求める
			DIAMETER = (float)(2.0 * Math.Sqrt(icount / ScanCorrect.Pai) * PixelSize);

			functionReturnValue = true;

			return functionReturnValue;
		}

		//*******************************************************************************
		//機　　能： 真円率が1.6を下回った時に表示するメッセージボックスの設定
		//
		//           変数名          [I/O] 型        内容
		//引　　数： FileName        [I/ ] String    読み込もうとするディレクトリ名＋ファイル名
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.1  2007/04/11   (CATS)Ohkado      新規作成
		//*******************************************************************************
		//Private Sub SetBHCMessage(ByVal FileName As String)
		//
		//        Load frmBHCMessage
		//        Call frmBHCMessage.setFileName(FileName)
		//        'BHCﾌｧﾝﾄﾑと認識されない時の警告メッセージの幅の設定
		//        If TextWidth(FileName) > frmBHCMessage.lblErrorFileMessage.width Then
		//            frmBHCMessage.width = frmBHCMessage.lblErrorFileMessage.Left + TextWidth(FileName) + 100 * 5 '100*2追加
		//            'ボタンの位置変更
		//            frmBHCMessage.cmdNo.Left = (frmBHCMessage.width / 2) - (frmBHCMessage.cmdNo.width / 2)
		//            frmBHCMessage.cmdYes.Left = frmBHCMessage.cmdNo.Left - frmBHCMessage.cmdYes.width - 200
		//            frmBHCMessage.cmdAllYes.Left = frmBHCMessage.cmdNo.Left + frmBHCMessage.cmdYes.width + 200
		//        End If
		//
		//End Sub

		//*******************************************************************************
		//機　　能： イメージプロから取得した文字列を区切り文字ごとにわけて、配列に格納
		//
		//           変数名          [I/O] 型        内容
		//引　　数：
		//
		//戻 り 値：
		//
		//補　　足： なし
		//
		//履　　歴： V8.0       07/22/23  (CATS)村田    新規作成
		//*******************************************************************************
		private void makeInfoFromImStr(string ImStr, ref float[] Cell)
		{
			string swork = null;
			int cnt = 0;
			int Count = 0;
			float[] tmp = new float[4];
			float[] tmpMaxDia = new float[4];

			Count = 0;
			swork = "";
			//二値化時にノイズが多い場合0を返す
			if (ImStr.Length > 100)				//追加 by Ohkado
			{
				Cell[0] = 0;
				Cell[1] = 0;
				return;
			}

			for (cnt = 1; cnt <= ImStr.Length; cnt++)
			{
				if ((int)Convert.ToChar(ImStr.Substring(cnt - 1, 1)) < 0x20)
				{
					//CRを検出したら、その直前のデータがデータ部
					if ((int)Convert.ToChar(ImStr.Substring(cnt - 1, 1)) == 13 || (int)Convert.ToChar(ImStr.Substring(cnt - 1, 1)) == 9)
					{
						float.TryParse(swork.Trim(), out tmp[Count]);
						Count = Count + 1;
						if (Count == 3)
						{
							if (tmpMaxDia[0] < tmp[1])
							{
								tmpMaxDia[0] = tmp[1];
								tmpMaxDia[1] = tmp[2];
							}
							Count = 0;
						}
					}
					swork = "";
				}
				else
				{
					//制御記号が出てくるまでは文字を加算する
					swork = swork + ImStr.Substring(cnt - 1, 1);
				}
			}
			Cell[0] = tmpMaxDia[0];
			Cell[1] = tmpMaxDia[1];
		}


		//*******************************************************************************
		//機　　能： グリッドコントロールテキストボックスの内容が適切かどうかの処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： txtValue        [I/ ] Variant   入力した値
		//        ： IsErrorValue    [ /O] Boolean   正常な値か？　OK→True NG→False
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v8.0  2007/01/19 (CATS)村田      新規作成
		//           v8.1　2007/04/23 (CATS)Ohkado   　IsErrorValueの値が逆だったのでFalseとTrueを入れ替え
		//*******************************************************************************
		private bool IsErrorValue(string txtValue)
		{
			bool functionReturnValue = false;

			//テキストボックスの値が数値として評価できない場合
			double IsNumeric = 0;
			if (double.TryParse(txtValue, out IsNumeric) == false)
			{
				functionReturnValue = true;
			}
			//生データ値の直接入力は0～MAX_RAWDATAの範囲で行う
			else if (OldCol == 3)
			{
				if (0 > Convert.ToSingle(txtValue) || MAX_RAWDATA < Convert.ToSingle(txtValue))
				{
					functionReturnValue = true;
				}
			}
			//半径の直接入力は0～MAX_DIAMETERの範囲で行う
			else if (OldCol == 2)
			{
				if (0 > Convert.ToSingle(txtValue) || MAX_DIAMETER < Convert.ToSingle(txtValue))
				{
					functionReturnValue = true;
				}
			}
			//入力された値が不正ならばメッセージ表示
			if (functionReturnValue)
			{
				//メッセージ:
				//不正なパラメータ値となるので処理を元に戻します。
				MessageBox.Show(CTResources.LoadResString(17218), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： グリッドコントロール編集用テキストボックスにフォーカスを当てた時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v8.0  2007/04/19 (CATS)Ohkado      新規作成
		//*******************************************************************************
		//Private Sub txtGridEdit_GotFocus()
		//    '「削除」を使用可にする
		//    cmdImgDelete.Enabled = True
		//End Sub

		//*******************************************************************************
		//機　　能： グリッドコントロール編集用テキストボックスの内容が変化した時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v8.0  2007/01/19 (CATS)村田      新規作成
		//*******************************************************************************
		private void txtGridEdit_KeyDown(object sender, KeyEventArgs e)
		{
			bool IsNumber = false;

			//シフトキー、Altキー、Ctrlキーが押された場合の処理             'V8.1 追加 by Ohakdo 2007/04/22
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			If Shift <> 0 Then Exit Sub
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
			if (e.Shift || e.Alt || e.Control) return;
			//初期化                                              by 村田 2007/01/26
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			If msgImgFile.Row = 0 Then Exit Sub
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
			if (msgImgFile.CurrentCell == null) return;
			IsNumber = true;

			//Enterが押された場合 (KeyCode=13)
			if (e.KeyCode == Keys.Return)
			{
				//セルの内容を確認する
				IsNumber = !IsErrorValue(txtGridEdit.Text);
				//元の値を入力する
				if (!IsNumber)
				{
					txtGridEdit.Text = oldValue.ToString();
					InitEditTxtbox();						//追加 by 村田 2007/01/26
				}
				//以前と値が変わっていない場合、何もしない
				else if (oldValue.ToString() == txtGridEdit.Text)
				{
					InitEditTxtbox();
					//Exit Sub                            '追加 by 村田 2007/01/26
				}
				else
				{
					//セルに値を代入
					SetValue(OldRow, OldCol, txtGridEdit.Text);
					//テキストボックスを初期化
					InitEditTxtbox();
				}
				//「削除」を使用不可にする                           'v8.1追加 by Ohkado 2007/04/18
				cmdImgDelete.Enabled = false;
			}
			//Enter以外が入力された場合、BHCﾃｰﾌﾞﾙを変更しようとしたので後でﾌｨｯﾃｲﾝｸﾞを行う
			else
			{
				//BHCテーブルに値を入力したかどうか？
				IsChangeBHCTValue = true;
			}

			//欄固定を解除
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			msgImgFile.AllowUserResizing = flexResizeColumns
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
			msgImgFile.AllowUserToResizeColumns = true;
			//スクロールバーをつける
			//msgImgFile.ScrollBars = flexScrollBarVertical
			//msgImgFile.ScrollBars = flexScrollBarBoth              'v8.1追加 by Ohkado 2007/04/23
		}


		//*******************************************************************************
		//機　　能：指定した列、行番号のセルに値を入力する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v8.0  2007/01/19 (CATS)村田      新規作成
		//          v19.0  2012/03/15 H.Nagai         bhc_p0更新
		//*******************************************************************************
		private void SetValue(int theRow, int theCol, string theText)
		{
			int OldCol = 0;
			int OldRow = 0;

			//操作前の情報を保存しておく
			SetImgUnDoData(ChangedType.BHCChange, theRow - 1, 1, 0, 0);

			//変更あり
			Changed = true;

#region 【C#コントロールで代用】
/*
			With msgImgFile
				OldCol = .col
				OldRow = .Row
				.col = theCol
				.Row = theRow
				'直径(mm)の変更処理
				If .col = 2 Then
					'BHCテーブルの書き込み 1桁目は四捨五入
					'v19.00 直径は1mm単位
					'.Text = RoundOff(Val(theText), 1)
					.Text = RoundOff(Val(theText), 0)
					'BHC内部データに書き込み
					DataofBHC(theRow - 1).diaData = RoundOff(CSng(theText), -1)

				'生データの変更処理
				ElseIf .col = 3 Then
					'BHCテーブル書き込み
					.Text = Format(RoundOff(Val(theText), -3), "0.00")
					.col = 4
					'Pの値を計算する
					'.Text = Format(CalculateXRay_damping_exponent((Val(theText)), "0.0000"), theRow)
					'v19.00 修正 by長野 2012/05/31
					.Text = Format(CalculateXRay_damping_exponent((Val(theText)), theRow), "0.0000")
					'内部データ書き込み 0.01単位で修正している
					DataofBHC(theRow - 1).rawdata = Val(theText) 'v8.1 小数第3位の四捨五入をしないように変更 2007/04/20 by ohkado
					DataofBHC(theRow - 1).pData = CalculateXRay_damping_exponent(DataofBHC(theRow - 1).rawdata, theRow - 1)
				End If
				.col = OldCol
				.Row = OldRow
			End With
*/
#endregion

			OldCol = msgImgFile.CurrentCell.ColumnIndex + 1;
			OldRow = msgImgFile.CurrentCell.RowIndex + 1;
			msgImgFile.CurrentCell = msgImgFile[theCol - 1, theRow - 1];

			//直径(mm)の変更処理
			if (msgImgFile.CurrentCell.ColumnIndex + 1 == 2)
			{
				//BHCテーブルの書き込み 1桁目は四捨五入
				//v19.00 直径は1mm単位
				//.Text = RoundOff(Val(theText), 1)
				float theTextValue = 0;
				float.TryParse(theText, out theTextValue);
				//msgImgFile.Text = RoundOff(theTextValue, 0).ToString();
				//Rev20.00 修正 by長野 2014/12/15
                msgImgFile.CurrentCell.Value = RoundOff(theTextValue, -1).ToString();

                //BHC内部データに書き込み
				DataofBHC[theRow - 1].diaData = RoundOff(Convert.ToSingle(theText), -1);
			}
			//生データの変更処理
			else if (msgImgFile.CurrentCell.ColumnIndex + 1 == 3)
			{
				//BHCテーブル書き込み
				float theTextValue = 0;
				float.TryParse(theText, out theTextValue);
				//msgImgFile.Text = RoundOff(theTextValue, -3).ToString("0.00");
                //Rev20.00 修正 by長野 2014/12/15
                msgImgFile.CurrentCell.Value = RoundOff(theTextValue, -3).ToString("0.00");

				msgImgFile.CurrentCell = msgImgFile[4 - 1, msgImgFile.CurrentCell.RowIndex];
				//Pの値を計算する
				//.Text = Format(CalculateXRay_damping_exponent((Val(theText)), "0.0000"), theRow)
				//v19.00 修正 by長野 2012/05/31
				//msgImgFile.Text = CalculateXRay_damping_exponent(theTextValue, theRow).ToString("0.0000");
                //Rev20.00 修正 by長野 2014/12/15
                msgImgFile.CurrentCell.Value = CalculateXRay_damping_exponent(theTextValue, theRow).ToString("0.0000");

                //内部データ書き込み 0.01単位で修正している
				DataofBHC[theRow - 1].rawdata = theTextValue;		//v8.1 小数第3位の四捨五入をしないように変更 2007/04/20 by ohkado
				DataofBHC[theRow - 1].pData = CalculateXRay_damping_exponent(DataofBHC[theRow - 1].rawdata, theRow - 1);
			}
			msgImgFile.CurrentCell = msgImgFile[OldCol - 1, OldRow - 1];


			//v19.00 bhc_p0更新
			UpdateBHC_P0();

			//値を変更したのでフィッテイングを再び行いグラフを描画する
			if (msgImgFile.Rows.Count + 1 > 10)
			{
				//フィッテイング計算
				if (fittingCaliculation() == false) return;
				//グラフの描画
				DrawGraph();
			}
		}


		//*******************************************************************************
		//機　　能：四捨五入する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Value           [I/ ] Sinlge    四捨五入する値
		//　　　　： digit           [I/ ] Integer　 四捨五入を行う桁数
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v8.0  2007/01/19 (CATS)村田      新規作成
		//          v19.00 2012/03/14 H.Nagai        digitが0の場合は小数点以下切捨て
		//*******************************************************************************
		private float RoundOff(float Value, int digit)
		{
			double buf = 0;
			float tmpDigit = 0;
			float tmpValue = 0;

			if (digit > 0)
			{
				tmpDigit = digit - 1;
				tmpValue = (float)Math.Truncate(Value);
                buf = tmpValue + 5 * Math.Pow(10, tmpDigit);
				buf = buf - buf % (Math.Pow(10, digit));
			}
			else if (digit < 0)			//v19.00
			{
				tmpDigit = digit * -1;

                //tmpValue = RoundOff((float)Math.Pow(Value * 10, tmpDigit), 1);
                float tmpV = Value * (float)Math.Pow(10, tmpDigit);
                tmpValue = RoundOff(tmpV, 1);
				buf = tmpValue / Math.Pow(10, tmpDigit);
			}
			else
			{
				buf = Math.Truncate(Value);
			}

			return Convert.ToSingle(buf);
		}


		//*******************************************************************************
		//機　　能：BHCグラフのP’のラベル値の最大値を返す
		//
		//           変数名          [I/O] 型        内容
		//引　　数： width           [I/ ] Integer   ラベルの分割数
		//           theValueA       [I/ ] Singel    Aの値
		//戻 り 値： PD_SetScaleMax  [ /O] Integer   ラベルの最大値
		//
		//補　　足： なし
		//
		//履　　歴： v8.0  2007/01/19 (CATS)Ohkado      新規作成
		//*******************************************************************************
		private int PD_SetScaleMax(int Width, float theValueA)
		{
			int MaxValue = 0;

			//msgImgFileの2列目のDiameter(直径)の最大値を計算後
			MaxValue = Convert.ToInt32((Convert.ToInt32(GetMaxVale(DataOfBHCCount)) * theValueA));
			//一番右側にプロットした点から最低1.5倍右に広げる必要がある
			MaxValue = Convert.ToInt32(MaxValue * 1.5);
			//p 'のラベル値の最大値を計算する
			return (MaxValue + (Width - MaxValue % Width));
		}


		//*******************************************************************************
		//機　　能：BHCグラフのPのラベル値の最大値を返す
		//
		//           変数名          [I/O] 型        内容
		//引　　数： width           [I/ ]Integer    ラベルの分割数
		//　　　　：
		//戻 り 値： P_SetScaleMax   [ /O]Integer    ラベルの最大値
		//
		//補　　足： なし
		//
		//履　　歴： v8.0  2007/01/19 (CATS)Ohkado      新規作成
		//*******************************************************************************
		private int P_SetScaleMax(int Width)
		{
			int MaxValue = 0;

			//msgImgFileの4列目のPの値の最大値を計算
			MaxValue = Convert.ToInt32(GetMaxPVale(DataOfBHCCount));
			//Pのラベル値の最大値を計算する
			return (MaxValue + (Width - MaxValue % Width));
		}


		//*******************************************************************************
		//機　　能： BHC補助グラフのP-P’を計算してラベル値の最大値を返す
		//           変数名          [I/O] 型        内容
		//引　　数： theValueA       [I/ ] Single    Aの値
		//戻 り 値： PDP_SetScaleMax [ /O] Single    P'ラベルの最大値
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2006/1/22   Ohkado      新規作成
		//*******************************************************************************
		private float PDP_SetScaleMax(float theValueA)
		{
			int i = 0;
			float PDP = 0;
			float PDPMAX = 0;

			float functionReturnValue = 0.0F;
			PDPMAX = 0.0F;
			//P-P'の最大値を求める
			for (i = 0; i <= DataOfBHCCount; i++)
			{
				//ΔP=P'-P
				PDP = Math.Abs(DataofBHC[i].diaData * theValueA - DataofBHC[i].pData);
				if (PDP > PDPMAX)
				{
					PDPMAX = PDP;
				}
			}

			//P-Pの最大値に応じたラベリングを行う。まだ、どのような値が入力されるのかわからないので保留 2007/01/22 by Ohkado
			if (PDPMAX < 0.2)
			{
				functionReturnValue = 0.2F;
			}
			else if (PDPMAX < 0.3)
			{
				functionReturnValue = 0.3F;
			}
			else if (PDPMAX < 0.5)
			{
				functionReturnValue = 0.5F;
			}
			else if (PDPMAX < 1.0)
			{
				functionReturnValue = 1.0F;
			}
			else if (PDPMAX < 2.0)
			{
				functionReturnValue = 5.0F;
			}
			else
			{
				functionReturnValue = 10.0F;
			}

			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能：ＢＨＣグラフを描画するＰ'ラベルをセット
		//           変数名          [I/O] 型        内容
		//引　　数： MaxValue        [I/ ]Integer    ラベルの間隔
		//戻 り 値：                 [ /O]Integer    ラベルの最大値
		//
		//補　　足： なし
		//
		//履　　歴： v8.0  2007/01/19 (CATS)村田      新規作成
		//*******************************************************************************
		private int Labeling_PD(int MaxValue)
		{
			int i = 0;
			int ScaleWidth = 0;
			//int lblMaxValue = 0;

			ScaleWidth =Convert.ToInt32(MaxValue / (float)BHC_LBL_COUNT);

			for (i = 0; i <= BHC_LBL_COUNT; i++)
			{
				lblBHCXscale[i].Text = (ScaleWidth * i).ToString();
				lblBHCXscale[i].Visible = true;
				lblHBHCXscale[i].Text = (ScaleWidth * i).ToString();
				lblHBHCXscale[i].Visible = true;
			}

			return 0;
		}


		//*******************************************************************************
		//機　　能：ＢＨＣグラフを描画するＰラベルをセット
		//           変数名          [I/O] 型        内容
		//引　　数： ラベルの間隔 As Integer [I]
		//戻 り 値： ラベルの最大値
		//
		//補　　足： なし
		//
		//履　　歴： v8.0  2007/01/19 (CATS)村田      新規作成
		//*******************************************************************************
		private int Labeling_P(int MaxValue)
		{
			int i = 0;
			int ScaleWidth = 0;
			//int lblMaxValue = 0;

            ScaleWidth = Convert.ToInt32(MaxValue / (float)BHC_LBL_COUNT);
            for (i = 0; i <= BHC_LBL_COUNT; i++)
			{
				lblBHCYscale[i].Text = (ScaleWidth * i).ToString();
				lblBHCYscale[i].Visible = true;
			}

			return 0;
		}


		//*******************************************************************************
		//機　　能：Pの最大値を取得する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   データ数
		//戻 り 値： GetMaxValue     [ /O] Single    データ配列の最大値
		//
		//補　　足： なし
		//
		//履　　歴： v8.0  2007/01/19 (CATS)村田      新規作成
		//*******************************************************************************
		private float GetMaxPVale(int Index)
		{
			float functionReturnValue = 0;

			int i = 0;
			float MaxValue = 0.0F;

			for (i = 0; i <= Index; i++)
			{
				if (MaxValue < DataofBHC[i].pData)
				{
					MaxValue = DataofBHC[i].pData;
				}
			}

			functionReturnValue = MaxValue;
			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能：配列の最大値を取得する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   データ数
		//戻 り 値： GetMaxValue     [ /O] Single    データ配列の最大値
		//
		//補　　足： なし
		//
		//履　　歴： v8.0  2007/01/19 (CATS)村田      新規作成
		//*******************************************************************************
		private float GetMaxVale(int Index)
		{
			float functionReturnValue = 0;

			int i = 0;
			float MaxValue = 0.0F;

			for (i = 0; i <= Index; i++)
			{
				if (MaxValue < DataofBHC[i].diaData)
				{
					MaxValue = DataofBHC[i].diaData;
				}
			}

			functionReturnValue = MaxValue;
			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： scanconpara.csvとtheInfoRecのa1～a6からAの値を計算
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： CalculateA      [ /O] Single    Aの値を求める
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2006/1/15   Ohkado      新規作成
		//          V19.00  2012/03/15  H.nagai     bhc_p0を使用する
		//*******************************************************************************
		public float CalculateA()
		{
			float functionReturnValue = 0;

			float P0 = 0;
			float theValue = 0;
			int i = 0;

			//P0をscancondpar.p0より読み取る
			//v19.00
			//P0 = CDbl(Format$(GetCommonFloat("scancondpar", "bhc_p0"), "0.0"))
			P0 = bhc_p0;
			//0割りを防止
			if (bhc_a[1] == 0.0 && bhc_a[2] == 0.0 && bhc_a[3] == 0.0 && bhc_a[4] == 0.0 && bhc_a[5] == 0.0 && bhc_a[6] == 0.0)
			{
				//不正入力なので、-1で返す
				for (i = 1; i <= 6; i++)
				{
					bhc_a[i] = 1;
				}
				functionReturnValue = -1;
				return functionReturnValue;
			}

			//A値を求める
            theValue = (float)(P0 / (bhc_a[1] * Math.Pow(P0, 1) +
                                     bhc_a[2] * Math.Pow(P0, 2) +
                                     bhc_a[3] * Math.Pow(P0, 3) +
                                     bhc_a[4] * Math.Pow(P0, 4) +
                                     bhc_a[5] * Math.Pow(P0, 5) +
                                     bhc_a[6] * Math.Pow(P0, 6)));
			//A値は5を超えない値とする:ｵｰﾊﾞｰﾌﾛｰ防止
			if (theValue > 15) theValue = 15;
			if (theValue < -15) theValue = -15;

			//P'に関するメモリを作成する
			functionReturnValue = theValue;

			return functionReturnValue;
		}


		//********************************************************************************
		//機    能  ：  同じ名前のファイルの有無をチェックする関数
		//              変数名           [I/O] 型        内容
		//引    数  ：  FileName         [I/ ] String    ファイル名
		//戻 り 値  ：                   [ /O] Boolean   検索結果  True:有り  False:無し
		//補    足  ：  なし
		//
		//履    歴  ：  V8.00  2007/01/30  (CATS)Ohkado       新規作成
		//********************************************************************************
		public bool SameFileExistCheck(string FullFileName)
		{
			int i = 0;

			//SameFileExistCheckの初期化
			bool functionReturnValue = true;

			//もともとファイルがない場合は何もしない
			if (DataOfBHCCount == 0)
			{
				functionReturnValue = true;
				return functionReturnValue;
			}

#region 【C#コントロールで代用】
/*
			With msgImgFile
				'BHCテーブルの2列目からファイル名を読む
				.col = 1
				'ファイル名が入っている場所
				For i = 1 To .Rows - 1
					'同じデータを見つけた場合はFalseを返す
					If FullFileName = DataofBHC(i - 1).FileName Then
						SameFileExistCheck = False
						Exit Function
					End If
				Next i
			End With
*/
#endregion

			//BHCテーブルの2列目からファイル名を読む
			msgImgFile.CurrentCell = msgImgFile[0, msgImgFile.CurrentCell.RowIndex];
			//ファイル名が入っている場所
			for (i = 1; i <= msgImgFile.Rows.Count; i++)
			{
				//同じデータを見つけた場合はFalseを返す
				if (FullFileName == DataofBHC[i - 1].FileName)
				{
					functionReturnValue = false;
					return functionReturnValue;
				}
			}

			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能：グリッドコントロールを操作する前にこの関数を呼び出して
		//           操作内容を保存しておく
		//           変数名          [I/O] 型            内容
		//引　　数： Action          [I/ ] ChangedType   操作内容
		//           CangeAllCount   [I/ ] Integer       元に戻すデータ数
		//           PreviousValue   [I/ ] CangeAllCount a1～a6の変更させる前の値
		//           ChangeIndex     [I/ ] ChangeIndex   a1～a6のどれを変更したか？
		//戻 り 値：
		//
		//補　　足： なし
		//
		//履　　歴： v8.0  2007/01/19 (CATS)村田      新規作成
		//*******************************************************************************
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
		Private Sub SetImgUnDoData(ByVal Action As ChangedType, ByVal theRow As Integer, ByVal CangeAllCount As Integer, _
		                                ByVal PreviousValue As Variant, ByVal ChangeIndex As Variant)
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
		private void SetImgUnDoData(ChangedType Action, int theRow, int CangeAllCount, double PreviousValue, int ChangeIndex)
		{
			int i = 0;

			//構造体ImgUnDoに変更できる値を入力しておく
			ImgUnDo[ReturnIndex].Changed = Action;							//どんな作業か？
			ImgUnDo[ReturnIndex].ChangedRows = (short)theRow;				//内部データの何列目を操作？
			ImgUnDo[ReturnIndex].FileName = DataofBHC[theRow].FileName;		//ファイル名
			ImgUnDo[ReturnIndex].DIAMETER = DataofBHC[theRow].diaData;		//直径
			ImgUnDo[ReturnIndex].rawdata = DataofBHC[theRow].rawdata;		//生データ
			ImgUnDo[ReturnIndex].p = DataofBHC[theRow].pData;				//X線減衰指数
			ImgUnDo[ReturnIndex].Count = (short)CangeAllCount;				//元に戻すデータ数
			ImgUnDo[ReturnIndex].ChangAIndex = (short)ChangeIndex;			//aのどれを変更したか？
        
			for (i = 1; i <= BHC_DIMENSION; i++)					//係数a1～a6
			{
				//.a(i) = Format(cwneA(i).Value, "0.######0")        'v8.1削除 by Ohkado 2007/04/12
				ImgUnDo[ReturnIndex].a[i] = Convert.ToDouble(ntbApara[i].Value.ToString("0.######0"));		//v8.1追加 by Ohkado 2007/04/12
			}

			//a(1)～a(6)を変更した時の処理
			if (ChangeIndex != 0) ImgUnDo[ReturnIndex].a[ChangeIndex] = Convert.ToDouble(PreviousValue.ToString("0.######0"));


			//次の構造体ImgUnDo用にインクリメント
			ReturnIndex = ReturnIndex + 1;
			//構造体ImgUnDoに復元操作可能範囲以上の値が入ると循環させるためにRetrunIndex=0とする
			if (ReturnIndex > MAX_RETURN_COUNT) ReturnIndex = 0;
			//「元に戻す」をクリックしたときのインクリメント
			ReturnCount = ReturnCount + 1;
			//「元に戻す」を復元操作以上行うと循環させるためにRetrunCountは「元に戻す」の最大回数をいれる
			if (ReturnCount > MAX_RETURN_COUNT) ReturnCount = MAX_RETURN_COUNT;
			//元に戻すボタンを使えるようにする
			cmdBack.Enabled = true;								//追加 by 村田 2007/01/26
		}


		//*******************************************************************************
		//機　　能：グリッドコントロールに行った操作を元に戻す
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値：
		//
		//補　　足： なし
		//
		//履　　歴： v8.0  2007/01/19 (CATS)村田      新規作成
		//*******************************************************************************
		private void UnDoImg()
		{
			//float theValueA = 0;
			int i = 0;
			int j = 0;
			int StartPoint = 0;
			int EndPoint = 0;
			int NextReturnIndex = 0;
			//int NextReturnAIndex = 0;			//特定のaのパラメータを変更したときよう V8.1追加 by Ohkado 2007/04/22

			//リターンインデックスをディクリメント
			ReturnIndex = ReturnIndex - 1;
			//リターンインデックスが負の場合の処理
			if (ReturnIndex < 0) ReturnIndex = MAX_RETURN_COUNT;
			//生データ値若しくは直径(mm)を変更させた場合
			if (ImgUnDo[ReturnIndex].Changed == ChangedType.BHCChange)
			{
				//内部データ書き換え
				DataofBHC[ImgUnDo[ReturnIndex].ChangedRows].diaData = ImgUnDo[ReturnIndex].DIAMETER;
				DataofBHC[ImgUnDo[ReturnIndex].ChangedRows].rawdata = ImgUnDo[ReturnIndex].rawdata;
				DataofBHC[ImgUnDo[ReturnIndex].ChangedRows].pData = ImgUnDo[ReturnIndex].p;
				//変更前のデータをBHCテーブルに書き込む
				WriteImgFromUnDoData();

				//bhc_a(1)～bhc_a(6)の値を元に戻す
				cwneAUnDoData();
				//「元に戻す」ことの出来る範囲をデクリメント
				ReturnCount = ReturnCount - 1;
			}
			//削除の場合
			else if (ImgUnDo[ReturnIndex].Changed == ChangedType.BHCDelete)
			{
				//｢元に戻す｣をできる回数制限を超える場合は制限分だけ元に戻すようにする
				if (ImgUnDo[ReturnIndex].Count > MAX_RETURN_COUNT)
				{
					ImgUnDo[ReturnIndex].Count = MAX_RETURN_COUNT;
				}
				//「削除」を使用不可にする                           'v8.1追加 by Ohkado 2007/04/18
				cmdImgDelete.Enabled = false;
				//｢元に戻す｣開始点と終了点を決定する
				StartPoint = ImgUnDo[ReturnIndex].ChangedRows;
				EndPoint = ImgUnDo[ReturnIndex].ChangedRows + ImgUnDo[ReturnIndex].Count - 1;

				//前に削除した行の内部データの一番下の行から順次｢元に戻す｣を繰り返す
				for (j = StartPoint; i <= EndPoint; i++)
				{
					//内部データ書き換え
					AddToCalculateBHCTableData(j, ImgUnDo[ReturnIndex].FileName, ImgUnDo[ReturnIndex].DIAMETER, ImgUnDo[ReturnIndex].rawdata, ImgUnDo[ReturnIndex].p);
					//BHCテーブル書き換え
					AddToBHCTable( j + 1, ImgUnDo[ReturnIndex].FileName, ImgUnDo[ReturnIndex].DIAMETER, ImgUnDo[ReturnIndex].rawdata, ImgUnDo[ReturnIndex].p);

#region 【C#コントロールで代用】
/*
				'BHCテーブルの表示を整理する処理
				With msgImgFile
					'表示位置を変更を左側に持ってくる来る処理
					.Row = j + 1
					.col = 1:
					.ColSel = 4
					.CellAlignment = flexAlignLeftCenter
					.RowHeight(.Row) = msgImgFile.RowHeight(0)
					'削除の場合行番号を再度付け直す
					.col = 0
					For i = 1 To .Rows - 1
						.Row = i: .Text = CStr(i)
					Next
				End With
*/
#endregion
					//BHCテーブルの表示を整理する処理
					//表示位置を変更を左側に持ってくる来る処理
					msgImgFile.CurrentCell = msgImgFile[0, j];
					msgImgFile.CurrentRow.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
					msgImgFile.CurrentRow.Height = msgImgFile.ColumnHeadersHeight;
					//削除の場合行番号を再度付け直す
					for (i = 1; i <= msgImgFile.Rows.Count; i++)
					{
						msgImgFile.Rows[i - 1].HeaderCell.Value = Convert.ToString(i);
					}

					//bhc_a(1)～bhc_a(6)の値を元に戻す
					cwneAUnDoData();
					//「元に戻す」ことの出来る範囲をデクリメント
					ReturnCount = ReturnCount - 1;
					//1つ追加の場合
					if (StartPoint == EndPoint) break;
					//i=EndPoint
					if (j == EndPoint) break;
					//ReturnCountが0の場合も戻せないので抜ける
					if (ReturnCount == 0)
					{
						break;
					}
					//リターンインデックスをディクリメント
					ReturnIndex = ReturnIndex - 1;
					//リターンインデックスが負の場合の処理
					if (ReturnIndex < 0) ReturnIndex = MAX_RETURN_COUNT;
					//Me.Enable=Falseを有効にしておく
					Application.DoEvents();
				}
			}
			//追加の場合:追加されたのは必ず最後の行にあるので、最後の行を削除する
			else if (ImgUnDo[ReturnIndex].Changed == ChangedType.BHCAdd)
			{
				//「削除」を使用不可にする                           'v8.1追加 by Ohkado 2007/04/18
				cmdImgDelete.Enabled = false;
				//削除するデータの始点と終点を指定
				StartPoint = msgImgFile.Rows.Count;
				EndPoint = msgImgFile.Rows.Count + 1 - ImgUnDo[ReturnIndex].Count;

				for (i = StartPoint; i >= EndPoint; i--)
				{
					//内部データの削除
					RemoveCalculateBHCTableData(i);
					//表の削除
					RemoveBHCTable(i);
					//bhc_a(1)～bhc_a(6)の値を元に戻す
					cwneAUnDoData();
					//「元に戻す」ことの出来る範囲をデクリメント
					ReturnCount = ReturnCount - 1;
					//1つ追加の場合
					if (StartPoint == EndPoint) break;
					//i=EndPointの場合
					if (i == EndPoint) break;
					//リターンインデックスをディクリメント
					ReturnIndex = ReturnIndex - 1;
					//リターンインデックスが負の場合の処理
					if (ReturnIndex < 0) ReturnIndex = MAX_RETURN_COUNT;
					//Me.Enable=Falseを有効にしておく
					Application.DoEvents();
				}
			}
			//係数a1～a6の値を元に戻す処理
			else if (ImgUnDo[ReturnIndex].Changed == ChangedType.achange)
			{
				//        '初期化
				//        IsAParaLongKeyDown = True
				//
				//        '4回以上｢元に戻す｣をクリックしたときに同じa()の変更を行なっていたかモニタリング
				//        For i = 1 To 4
				//            NextReturnAIndex = ReturnIndex - i
				//            If NextReturnAIndex < 0 Then NextReturnAIndex = MAX_RETURN_COUNT
				//
				//            If ImgUnDo(NextReturnAIndex).ChangAIndex <> ImgUnDo(ReturnIndex).ChangAIndex Then
				//              IsAParaLongKeyDown = False
				//              Exit For
				//            End If
				//        Next i
				//
				//        '特定のa()の変更を長押しによって行った場合の処理
				//        If IsAParaLongKeyDown = True Then
				//            For i = 1 To MAX_RETURN_COUNT
				//                '[元に戻す]を一回実行
				//                cwneAUnDoData
				//                '「元に戻す」ことの出来る範囲をデクリメント
				//                ReturnCount = ReturnCount - 1
				//                '次に[元に戻す]をした時にループを抜けれるかの評価
				//                NextReturnAIndex = ReturnIndex - 1
				//                If NextReturnAIndex < 0 Then NextReturnAIndex = MAX_RETURN_COUNT
				//
				//                If ImgUnDo(NextReturnAIndex).ChangAIndex <> ImgUnDo(ReturnIndex).ChangAIndex Then Exit For
				//                'ループを抜けれなかった場合
				//                ReturnIndex = ReturnIndex - 1
				//                If ReturnIndex < 0 Then ReturnIndex = MAX_RETURN_COUNT
				//                'Me.Enable=Falseを有効にしておく
				//                DoEvents
				//            Next i
				//        Else
				//bhc_a(1)～bhc_a(6)の値を元に戻す
				cwneAUnDoData();
				//「元に戻す」ことの出来る範囲をデクリメント
				ReturnCount = ReturnCount - 1;
				//End If
			}

			//｢元に戻す｣実行後、上記のパラメータでグラフを作成　　：パラメータに変更はないのでフィッテイングは行わない
			if (msgImgFile.Rows.Count + 1 > GRAPH_DRAW_NEED_DATA)
			{
				//グラフの描画
				DrawGraph();
			}
			else
			{
				//グラフを一回消去
				InitGraph();
				Set_All_Label_False();
				//グラフ作成許可グラフを下ろす
				PamitDrawGraph = false;
			}

			//次に｢元に戻す｣をクリックしたときに何回操作を行うかモニタリング
			NextReturnIndex = ReturnIndex - 1;
			if (NextReturnIndex < 0) NextReturnIndex = MAX_RETURN_COUNT;

			//ReturnCountが0の場合, 次に｢元に戻す｣をクリックするとReturnCount以上の操作を行う場合は, ｢元に戻す｣ボタンは使えなくする．
			if (ReturnCount < 1 || ImgUnDo[NextReturnIndex].Count > ReturnCount)		//MAX_RETURN_COUNT→ReturnCountに変更
			{
				ReturnCount = 0;
				ReturnIndex = 0;
				cmdBack.Enabled = false;
			}
		}


		//*******************************************************************************
		//機　　能：cwneA(1)～(6)の値を元に戻す処理
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値：
		//
		//補　　足： なし
		//
		//履　　歴： v8.0  2007/02/01 (CATS)大門      新規作成
		//*******************************************************************************
		private void cwneAUnDoData()
		{
			int i = 0;

			//cwneA操作においてグラフの作成などはここでは行わないのでイベントフラグは下ろす
			byEvent = false;

			for (i = 1; i <= BHC_DIMENSION; i++)
			{
				bhc_a[i] = ImgUnDo[ReturnIndex].a[i];
				//cwneA(i).Value = bhc_a(i)      'v8.1削除 by Ohkado 2007/04/12
				ntbApara[i].Value = (decimal)RoundOFFAPara((float)bhc_a[i]);		//v8.1追加 by Ohkado 2007/04/12
			}
			byEvent = true;
		}


		//*******************************************************************************
		//機　　能：グリッドコントロールに過去の履歴を書き込む
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値：
		//
		//補　　足： なし
		//
		//履　　歴： v8.0  2007/01/19 (CATS)村田      新規作成
		//*******************************************************************************
		private void WriteImgFromUnDoData()
		{
			int theRow = 0;				//追加 by 村田 2007/01/26

			theRow = ImgUnDo[ReturnIndex].ChangedRows + 1;

#region 【C#コントロールで代用】
/*
			With msgImgFile
				.TextMatrix(theRow, 1) = ImgUnDo(ReturnIndex).FileName
				'表示位置を変更を左側に持ってくる来る処理
				.col = 1:                                                   'v8.1追加 by Ohkado 2007/04/18
				.ColSel = 4                                                 'v8.1追加 by Ohkado 2007/04/18
				.CellAlignment = flexAlignLeftCenter
				.TextMatrix(theRow, 2) = Format(RoundOff(ImgUnDo(ReturnIndex).DIAMETER, 1), "0")
				.TextMatrix(theRow, 3) = Format(ImgUnDo(ReturnIndex).rawdata, "0.00")
				.TextMatrix(theRow, 4) = Format(ImgUnDo(ReturnIndex).p, "0.0000")
			End With
*/
#endregion

			//msgImgFile[theRow - 1, 0].Value = ImgUnDo[ReturnIndex].FileName;
			//Rev20.00 修正 by長野 2014/12/15
            msgImgFile[0, theRow - 1].Value = ImgUnDo[ReturnIndex].FileName;
            
            //表示位置を変更を左側に持ってくる来る処理
			msgImgFile.CurrentRow.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			
            //msgImgFile[theRow - 1, 1].Value = RoundOff(ImgUnDo[ReturnIndex].DIAMETER, 1).ToString("0");
			//msgImgFile[theRow - 1, 2].Value = ImgUnDo[ReturnIndex].rawdata.ToString("0.00");
			//msgImgFile[theRow - 1, 3].Value = ImgUnDo[ReturnIndex].p.ToString("0.0000");

            //Rev20.00 修正 by長野 2014/12/15
            msgImgFile[1,theRow - 1].Value = RoundOff(ImgUnDo[ReturnIndex].DIAMETER, -1).ToString("0");
            msgImgFile[2,theRow - 1].Value = ImgUnDo[ReturnIndex].rawdata.ToString("0.00");
            msgImgFile[3,theRow - 1].Value = ImgUnDo[ReturnIndex].p.ToString("0.0000");

        }


		//*******************************************************************************
		//機　　能：　グリッドコントロール編集用テキストボックスを初期化する
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値：
		//
		//補　　足： なし
		//
		//履　　歴： v8.0  2007/01/26 (CATS)村田      新規作成
		//*******************************************************************************
		private void InitEditTxtbox()
		{
			OldCol = 0;
			OldRow = 0;

			txtGridEdit.Visible = false;
			txtGridEdit.Text = "";
			//BHCﾃｰﾌﾞﾙに変更を加えたか？ 'v8.1追加 by Ohkado 2007/04/03
			IsChangeBHCTValue = false;
		}


		//*******************************************************************************
		//機　　能：　グリッドコントロール編集用テキストボックスがフォーカスを失った時の処理
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値：
		//
		//補　　足： なし
		//
		//履　　歴： v8.0  2007/01/26 (CATS)村田      新規作成
		//*******************************************************************************
		private void txtGridEdit_Leave(object sender, EventArgs e)
		{
			//bool IsNumber = false;

			if (OldCol == 0 || OldRow == 0) return;
			//エラー処理
			if (!IsErrorValue(txtGridEdit.Text))
			{
				//マウスによるコピーやペーストを考えて四捨五入の値以上変化があった時の処理：IsChangeBHCTValue=Trueとする.
				//If Abs(Fix(oldValue) - Fix(txtGridEdit.text)) >= 1 And OldCol = 2 Then IsChangeBHCTValue = True        'コピーは効かせない。
				//値が変わっていない場合、何もしない
				if (oldValue.ToString() != txtGridEdit.Text && IsChangeBHCTValue)
				{
					//セルに値を代入
					SetValue(OldRow, OldCol, txtGridEdit.Text);
				}
			}
			//グリッドコントロール編集用テキストボックスを初期化
			InitEditTxtbox();				//変更 by 村田 2007/01/26
		}


		//*******************************************************************************
		//機　　能：グラフを描画する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし'
		//戻 り 値： なし'
		//
		//補　　足： グラフの描画と目盛線の描画とラベルの表示を行う
		//
		//履　　歴： v8.0  2007/01/19 (CATS)Ohkado      新規作成
		//*******************************************************************************
		private void DrawGraph()
		{
			float theValueA = 0;

			//グラフを一回消去
			InitGraph();
			//a1～a6の値に依存して変化するAの値を取得:Plot時の再計算を防止
			theValueA = CalculateA();
			//フィッテイングの行えないデータかどうかチェック
			if (theValueA <= 0)
			{
				//InvalidOperation = False
				//メッセージ
				//不正なパラメータ値となるので処理を元に戻します。
				MessageBox.Show(CTResources.LoadResString(17218), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				//｢元に戻す｣
				cmdBack_Click(cmdBack, EventArgs.Empty);
				return;
			}
			//ラベルとJogの設定
			Init_Label_Jog(msgImgFile.Rows.Count + 1, theValueA);

            //BHCグラフにプロットを行う
            PlotBHC(theValueA);

            //BHC補助グラフにプロットを行う
            PlotHBHCDraw(theValueA);
		}


		//*******************************************************************************
		//機　　能：ファイル名表示用テキストボックスがフォーカスを失った時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし'
		//戻 り 値： なし'
		//
		//補　　足：
		//
		//履　　歴： v8.0  2007/02/23 (CATS)村田      新規作成
		//*******************************************************************************
		private void txtGridFileName_Leave(object sender, EventArgs e)
		{
			txtGridFileName.Visible = false;
			//欄固定を解除
#region 【C#コントロールで代用】
/*
			msgImgFile.AllowUserResizing = flexResizeColumns
*/
#endregion
			msgImgFile.AllowUserToResizeColumns = true;
			//スクロールバーを戻す
			//msgImgFile.ScrollBars = flexScrollBarVertical
			//msgImgFile.ScrollBars = flexScrollBarBoth
		}


		//*******************************************************************************
		//機　　能：BHCテーブルに追加できるかを判定する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし'
		//戻 り 値： IsOKAddBHCTable [I/ ] Boolean   BHCテーブルに追加してもよい値か？
		//
		//補　　足：
		//
		//履　　歴： v8.0  2007/03/1 (CATS)村田      新規作成
		//           v19.00  12/03/14    H.Nagai     マイクロCTにあわせる
		//                                           シングルスキャン、管電圧一致チェック追加
		//*******************************************************************************
		private bool IsOKAddBHCTable(string FileName)
		{
			bool functionReturnValue = false;

			//スキャノ画像か？
			if ((FileName.Substring(FileName.Length - 9).ToUpper()).StartsWith("-S")) return functionReturnValue;

			//付帯情報ファイルは存在するか？
			//If Not InfExistCheck(FileName) Then Exit Function
			//If Not FSO.FileExists(ChangeExtension(fileName, ".img", ".inf")) Then Exit Function 'v8.5変更 by 間々田 2008/08/01
			if (!File.Exists(modFileIO.ChangeExtension(FileName, ".inf"))) return functionReturnValue;	//v8.5変更 by 間々田 2008/08/01

			//生データファイルは存在するか？
			//If Not RawExistCheck(FileName) Then Exit Function
			if (!File.Exists(modCT30K.GetRawName(FileName))) return functionReturnValue;					//v8.5変更 by 間々田 2008/08/01

			//同じ名前のテーブルは存在するか
			if (!SameFileExistCheck(FileName)) return functionReturnValue;

			//2012/03/14 チェック追加
			if (!IsOkAddImageInfo(FileName)) return functionReturnValue;

			functionReturnValue = true;

			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能：付帯情報をチェックしてBHCテーブルに追加できるか調べる
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし'
		//戻 り 値： IsOkAddImageInfo[I/ ] Boolean  True:追加可 False:追加不可
		//
		//補　　足：
		//
		//履　　歴： v19.00  12/03/14    H.Nagai     新規
		//*******************************************************************************
		private bool IsOkAddImageInfo(string FileName)
		{
			bool functionReturnValue = false;

			//付帯情報読み込み
			//modImageInfo.ImageInfoStruct ctinf2 = default(modImageInfo.ImageInfoStruct);
            ImageInfo ctinf2 = new ImageInfo();
            ctinf2.Data.Initialize();

			//if (modImageInfo.ReadImageInfo(ref ctinf2, modFileIO.RemoveExtensionEx(FileName)) == false)
            if (ImageInfo.ReadImageInfo(ref ctinf2.Data, modFileIO.RemoveExtensionEx(FileName)) == false)
            {
				//読み込みエラー
				return functionReturnValue;
			}

            //v19.00
			//シングルスキャン
			if (ctinf2.Data.bhc != 0) return functionReturnValue;

			//管電圧
			int lVolt = 0;
            int.TryParse(ctinf2.Data.volt.GetString(), out lVolt);
			if (DataOfBHCCount == 0)
			{
				//最初の登録の場合、管電圧を保持
				voltPrev = lVolt;
			}
			else if (DataOfBHCCount > 0)
			{
				//BHCテーブルに1個以上登録されている場合

				//管電圧が一致していない
				if (voltPrev != lVolt) return functionReturnValue;
			}
			functionReturnValue = true;

			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能：BHCテーブルフォルダ選択
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし'
		//戻 り 値： GetBHCTableFolder[I/ ] Boolean  フォルダ選択 True:あり False:なし
		//
		//補　　足：
		//
		//履　　歴： v19.00  12/03/14    H.Nagai     マイクロCTにあわせる
		//*******************************************************************************
		private bool GetBHCTableFolder(ref string sFolder)
		{
			string FolderName = null;

			bool functionReturnValue = false;

			//フォルダ参照ダイアログ表示
			FolderName = modFileIO.GetFolderName(sFolder, CTResources.LoadResString(21303));

			//ＯＫ選択時：選択したフォルダをテキストボックスに設定
			if (!string.IsNullOrEmpty(FolderName))
			{
				sFolder = FolderName;

				functionReturnValue = true;
			}

			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能：BHC P0の更新
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足：
		//
		//履　　歴： v19.00  12/03/15    H.Nagai     新規
		//*******************************************************************************
		private void UpdateBHC_P0()
		{
			//中間測定点のPをP0とする
			if (DataOfBHCCount == 0)
			{
				//初期化
                bhc_p0 = CTSettings.scancondpar.Data.mbhc_p0_const;
			}
			else
			{
				//v19.99 P0を1.5で統一する by長野 2012-03/24
				//中間点
				//        Dim p0Index As Long
				//        p0Index = Fix(DataOfBHCCount / 2 + 0.5) - 1
				//
				//        bhc_p0 = DataofBHC(p0Index).pdata
                
                //変更2014/10/07hata_v19.51反映
                //bhc_p0 = 1.5F;
                //'v19.50 産業用の場合は3.5、マイクロの場合は1.5にする by長野　2014/02/15
                if (CTSettings.scaninh.Data.avmode == 0)
                {
                    bhc_p0 = 3.5F;
                }
                else
                {
                    bhc_p0 = 1.5F;
                }

			}
		}

        //追加2015/01/27hata
        //データのソート
        //直径を使ってインデックスを並び変える
        //並び変えたインデックスでデータを格納する
        private void DataSortAndAddTable(Dictionary<string, float> dict)
        {
            try
            {

                // キーと値を格納する配列を作成し、Dictionaryの内容をコピー
                string[] sortedKeys = new string[dict.Count];
                float[] sortedValues = new float[dict.Count];

                dict.Keys.CopyTo(sortedKeys, 0);
                dict.Values.CopyTo(sortedValues, 0);

                // Array.Sort(Array, Array)メソッドを使ってソート
                //直径を使ってインデックスを並び変える
                //Array.Sort(sortedKeys, sortedValues);
                Array.Sort(sortedValues, sortedKeys);

                //ソート用データ
                structDataofBHC[] sdata = new structDataofBHC[DataofBHC.Length];		//BHCファイル構造体
                int indx;
                int i = 0;
                for (i = 0; i < sortedValues.Length; i++)
                {
                    indx = Convert.ToInt32(sortedKeys[i]);
                    sdata[i].FileName = DataofBHC[indx - 1].FileName;
                    sdata[i].diaData = DataofBHC[indx - 1].diaData;
                    sdata[i].rawdata = DataofBHC[indx - 1].rawdata;
                    sdata[i].pData = DataofBHC[indx - 1].pData;

                    //BHCテーブルの値をGUIに表示する
                    AddToBHCTable(msgImgFile.Rows.Count + 1, sdata[i].FileName, sdata[i].diaData, sdata[i].rawdata, sdata[i].pData);

                }

                DataofBHC = sdata;
            }
            catch
            { 
            }

        }

    }

}
