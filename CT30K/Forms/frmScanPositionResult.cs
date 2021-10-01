using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TransImage;
//
using CT30K.Properties;
using CT30K.Common;
using CTAPI;

namespace CT30K
{
    ///* *************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver9.7                */
    ///* 客先　　　　： ?????? 殿                                                    */
    ///* プログラム名： frmScanPositionResult.frm                                    */
    ///* 処理概要　　： スキャン位置校正結果                                         */
    ///* 注意事項　　：                                                              */
    ///* --------------------------------------------------------------------------- */
    ///* ＯＳ　　　　： Windows XP Professional (SP1)                                */
    ///* コンパイラ　： VB 6.0 (SP5)                                                 */
    ///* --------------------------------------------------------------------------- */
    ///* VERSION     DATE        BY                  CHANGE/COMMENT                  */
    ///*                                                                             */
    ///* V1.00       99/XX/XX    (TOSFEC) ????????                                   */
    ///* V2.0        00/02/08    (TOSFEC) 鈴山　修   V1.00を改造                     */
    ///* V3.0        00/08/01    (TOSFEC) 鈴山　修   ｺｰﾝﾋﾞｰﾑCT対応                   */
    ///* V9.7        04/11/01    (SI4)間々田         階調変換オブジェクトを使用      */
    ///*                                                                             */
    ///* --------------------------------------------------------------------------- */
    ///* ご注意：                                                                    */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。    */
    ///*                                                                             */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2004                  */
    ///* *************************************************************************** */
    public partial class frmScanPositionResult : Form
    {
        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************
        bool OK;            //入力結果
        int drgline;        //1:始点側ドラッグ 2:終点側ドラッグ
        int LastY;          //線をクリックした位置
        float myA;          //傾き
        float myB;          //切片
        modImgProc.LineStruct myLine;
        
        private RadioButton[] optScale = new RadioButton[3];
        private Label[] lblColon = new Label[3];

        private BitmapImageControl BmpICtrl;
        //追加2014/12/05hata_v19.51_dnet
        private bool bLoadup = false;



        #region インスタンスを返すプロパティ

        // frmScanPositionResultのインスタンス
        private static frmScanPositionResult _Instance = null;

        /// <summary>
        /// frmScanPositionResultのインスタンスを返す
        /// </summary>
        public static frmScanPositionResult Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmScanPositionResult();
                }

                return _Instance;
            }
        }

        #endregion


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmScanPositionResult()
        {
            InitializeComponent();

            #region コントロールの貼り付け

            this.SuspendLayout();

            for (int i = 0; i < optScale.Length; i++)
            {
                this.optScale[i] = new RadioButton();
                this.optScale[i].AutoSize = true;
                this.optScale[i].Font = new Font("ＭＳ ゴシック", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(128)));
                this.optScale[i].Location = new Point(16, 20 + i * 20);
                this.optScale[i].Name = "optScale" + i.ToString();
                this.optScale[i].Size = new Size(60, 17);
                this.optScale[i].TabIndex = i + 1;
                this.optScale[i].TabStop = true;
                switch (i)
                {
                    case 0:
                        this.optScale[i].Text = " 1 倍";
                        break;
                    case 1:
                        this.optScale[i].Text = " 4 倍";
                        break;
                    case 2:
                        this.optScale[i].Text = " 16 倍";
                        break;
                    default:
                        break;
                }
                this.optScale[i].UseVisualStyleBackColor = true;
                this.optScale[i].CheckedChanged += new System.EventHandler(this.optScale_CheckedChanged);
                this.fraScale.Controls.Add(this.optScale[i]);
            }

            // lblColon
            for (int i = 0; i < lblColon.Length; i++)
            {
                this.lblColon[i] = new Label();
                this.lblColon[i].AutoSize = true;
                this.lblColon[i].BackColor = Color.Transparent;
                this.lblColon[i].Font = new Font("ＭＳ ゴシック", 11F);
                this.lblColon[i].ForeColor = Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(0)))), ((int)(((byte)(198)))));
                this.lblColon[i].Location = new Point(91, 17 + i * 24);
                this.lblColon[i].Name = "lblColon" + i.ToString();
                this.lblColon[i].Size = new Size(15, 15);
                this.lblColon[i].TabIndex = i;
                this.lblColon[i].Text = ":";
                this.lblColon[i].TextAlign = ContentAlignment.MiddleLeft;
                this.Controls.Add(this.lblColon[i]);
            }

            BmpICtrl = new BitmapImageControl();
            BmpICtrl.MirrorOn = false;
            BmpICtrl.SetLTSize(LookupTableSize.LT16Bit);
            BmpICtrl.WindowLevel = 2048;
            BmpICtrl.WindowWidth = 4096;
            BmpICtrl.ImageSize = new Size(CTSettings.detectorParam.h_size , CTSettings.detectorParam.v_size);
            myLine = new modImgProc.LineStruct();

            ctlTransImage.MirrorOn = BmpICtrl.MirrorOn;

            this.ResumeLayout(false);

            #endregion
        }

        #region ウィンドウレベルスライダー変更時処理

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
		 private void cwsldLevel_PointerValueChanged(object sender, EventArgs e)
        {
			//値をラベルに表示
			lblLevel.Text = cwsldLevel.Value.ToString();

			//階調変換を実行
			//ctlTransImage.WindowLevel = cwsldLevel.Value;       
            BmpICtrl.WindowLevel = cwsldLevel.Value;

            //描画
            ctlTransImage.Picture = BmpICtrl.Picture;
            //ctlTransImage.Invalidate();
            ctlTransImage.Refresh();
        }

        #endregion

        #region ウィンドウ幅スライダー変更時処理

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
        private void cwsldWidth_PointerValueChanged(object sender, EventArgs e)
        {
			//値をラベルに表示
			lblWidth.Text = cwsldWidth.Value.ToString();

			//階調変換を実行
			//ctlTransImage.WindowWidth = cwsldWidth.Value;
            BmpICtrl.WindowWidth = cwsldWidth.Value;

            //描画
            ctlTransImage.Picture = BmpICtrl.Picture;
            //ctlTransImage.Invalidate();
            ctlTransImage.Refresh();
        }

        #endregion

        #region Line 変更処理

        //   Line 変更処理
        //   （傾き・切片・上下差の表示）    'append by 間々田 2003/10/14
        //
		private void Update(bool LineChanged = false)
		{
			//自動の時は表示するだけで、自動スキャン位置校正で求めたパラメーターを書き換えない   'v11.2追加 2005/12/05 by 間々田
			//If Not MyDispOnly Then                                                              'v11.2追加 2005/12/05 by 間々田
			//    GVal_ScanPosiA(2) = (Line1.y2 - Line1.y1) / 15 / h_size * pvm
			//    GVal_ScanPosiB(2) = (Line1.y1 / 15 * pvm + GVal_ScanPosiA(2) * (h_size - 1) / 2 - (v_size - 1) / 2) 'changed by 山本　2005-12-6
			//End If                                                                             'v11.2追加 2005/12/05 by 間々田

            //2014/11/11hata Point型を変更？？
            PointF L1;
            PointF L2;
            //Point L1;
            //Point L2;
            //変更 2006/01/12 by 間々田
            if (LineChanged)
            {
                //傾き・切片を再計算
                //2014/11/07hata キャストの修正
                //myA = (myLine.y2 - myLine.y1) / CTSettings.detectorParam.h_size * CTSettings.detectorParam.pvm;
                //myB = (myLine.y1 * CTSettings.detectorParam.pvm + myA * (CTSettings.detectorParam.h_size - 1) / 2 - (CTSettings.detectorParam.v_size - 1) / 2);
                myA = (float)(myLine.y1 - myLine.y2) / (float)CTSettings.detectorParam.h_size * CTSettings.detectorParam.pvm;
                myB = (myLine.y2 * CTSettings.detectorParam.pvm + myA * (CTSettings.detectorParam.h_size - 1) / 2F - (CTSettings.detectorParam.v_size - 1) / 2F);

                //            'ドラッグ用のラベルの位置
                //            lbl1(1).Move .x1 - lbl1(1).Width / 2, .y1 - lbl1(1).Height / 2
                //            lbl1(2).Move .x2 - lbl1(2).Width / 2, .y2 - lbl1(2).Height / 2
            }

            //2014/11/11hata Point型を変更？？
            L1 = new PointF(myLine.x1, myLine.y1);
            L2 = new PointF(myLine.x2, myLine.y2);
            //L1 = new Point(myLine.x1, myLine.y1);
            //L2 = new Point(myLine.x2, myLine.y2);
            ctlTransImage.SetLinePoint(LineConstants.ScanLine, ref L1, ref L2);

            ctlTransImage.Refresh();


			//傾き・切片・上下差の表示
			//lblScanPosiA.Caption = Format$(GVal_ScanPosiA(2), "0.000000")               '傾き
			//lblScanPosiB.Caption = Format$(GVal_ScanPosiB(2), "0.000000")               '切片
			//lblPosiAB.Caption = Format$(Abs((GVal_ScanPosiA(2) * h_size / pvm)), "0")   '上下差

			//変更 2006/01/12 by 間々田
			lblScanPosiA.Text = myA.ToString("0.000000");//傾き
			lblScanPosiB.Text = myB.ToString("0.000000");//切片
            lblPosiAB.Text = Math.Abs(myA * CTSettings.detectorParam.h_size / CTSettings.detectorParam.pvm).ToString("0");//上下差

			//ラベルや線が時々消えてしまうことへの対策 added by 間々田 2004/03/22
			lblScanPosiA.Refresh();
			lblScanPosiB.Refresh();
			lblPosiAB.Refresh();
		}

        #endregion

        #region コメントアウト

        //Private Sub lbl1_MouseDown(Index As Integer, Button As Integer, Shift As Integer, x As Single, y As Single)
        //
        //    '自動スキャン位置校正時は動かさない
        //    If MyDispOnly Then Exit Sub 'v11.2追加 by 間々田 2005/12/05
        //
        //    lbl1(Index).ZOrder 0
        //    lbl1(Index).Drag vbBeginDrag
        //    drgline = Index
        //
        //End Sub
        //
        //Private Sub Display1_DragDrop(Source As Control, x As Single, y As Single)
        //
        //    '自動スキャン位置校正時は動かさない
        //    If MyDispOnly Then Exit Sub 'v11.2追加 by 間々田 2005/12/05
        //
        //    Select Case drgline
        //
        //        Case 1
        //            Line1.y1 = y   '始点側変更
        //
        //        Case 2
        //            Line1.y2 = y   '終点側変更
        //
        //    End Select
        //
        //    '傾き・切片・上下差の表示 change by 間々田 2003/10/14
        //    UpdateInfo
        //
        //End Sub
        //
        //Private Sub Display1_DragOver(Source As Control, x As Single, y As Single, State As Integer)
        //
        //    '自動スキャン位置校正時は動かさない
        //    If MyDispOnly Then Exit Sub 'v11.2追加 by 間々田 2005/12/05
        //
        //    Select Case drgline
        //
        //        Case 1
        //            Line1.y1 = y   '始点側変更
        //
        //        Case 2
        //            Line1.y2 = y   '終点側変更
        //
        //    End Select
        //
        //    '傾き・切片・上下差の表示 change by 間々田 2003/10/14
        //    UpdateInfo
        //
        //End Sub
        //
        //'Private Sub Display1_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
        //Private Sub Display1_MouseDown(Button As Integer, Shift As Integer, x As Long, y As Long)   'changed by 山本　2002-6-27　MIL LITE 7.0
        //
        //    '自動スキャン位置校正時は動かさない
        //    If MyDispOnly Then Exit Sub 'v11.2追加 by 間々田 2005/12/05
        //
        //    If Me.MousePointer = vbSizeNS Then
        //        LastY = y
        //    End If
        //
        //End Sub
        //
        //'Private Sub Display1_MouseMove(Button As Integer, Shift As Integer, X As Single, Y As Single)
        //Private Sub Display1_MouseMove(Button As Integer, Shift As Integer, x As Long, y As Long)   'changed by 山本　2002-6-27　MIL LITE 7.0
        //
        //    Dim moveY   As Integer
        //    Dim minY    As Integer
        //    Dim maxY    As Integer
        //
        //    '自動スキャン位置校正時は動かさない
        //    If MyDispOnly Then Exit Sub 'v11.2追加 by 間々田 2005/12/05
        //
        //    'マウスボタンが押されていない場合
        //    If Button = 0 Then
        //
        //        'マウスポインタを設定する
        //        Me.MousePointer = IIf(PointOnLine(x, y, ScaleX(Line1.x1, vbTwips, vbPixels), _
        //'                                                ScaleY(Line1.y1, vbTwips, vbPixels), _
        //'                                                ScaleX(Line1.x2, vbTwips, vbPixels), _
        //'                                                ScaleY(Line1.y2, vbTwips, vbPixels)), _
        //'                                                vbSizeNS, vbDefault)
        //
        //    'マウスボタンが押されていて、マウスポインタが上下方向の場合
        //    ElseIf Me.MousePointer = vbSizeNS Then
        //
        //        If Line1.y1 < Line1.y2 Then
        //            minY = Line1.y1
        //            maxY = Line1.y2
        //        Else
        //            minY = Line1.y2
        //            maxY = Line1.y1
        //        End If
        //
        //        '移動距離を求める（水平線が画面からはみ出さないようにする）
        //        moveY = CorrectInRange(y - LastY, -ScaleY(minY, vbTwips, vbPixels), _
        //'                                          Image1.SizeY - 1 - ScaleY(maxY, vbTwips, vbPixels))
        //        moveY = ScaleY(moveY, vbPixels, vbTwips)
        //
        //        '水平線の移動
        //        Line1.y1 = Line1.y1 + moveY
        //        Line1.y2 = Line1.y2 + moveY
        //        LastY = y
        //
        //        '傾き・切片・上下差の表示 change by 間々田 2003/10/14
        //        UpdateInfo
        //
        //    End If
        //
        //End Sub
        //
        //'Private Sub Display1_MouseUp(Button As Integer, Shift As Integer, X As Single, Y As Single)
        //Private Sub Display1_MouseUp(Button As Integer, Shift As Integer, x As Long, y As Long)   'changed by 山本　2002-6-27　MIL LITE 7.0
        //
        //    Me.MousePointer = vbDefault
        //
        //End Sub

        #endregion

        #region ctlTransImageの上でマウスボタンが押されたときに発生するイベント

        /// <summary>
        /// ctlTransImageの上でマウスボタンが押されたときに発生するイベント
        /// </summary>
        /// <param name="sender">イベントのソース</param>
        /// <param name="e">イベントデータを格納しているオブジェクト</param>
        private void ctlTransImage_MouseDown(object sender, MouseEventArgs e)
		{
            MouseButtons button = e.Button;
			int x = e.X;
            int y = e.Y;

			if (object.ReferenceEquals(this.Cursor, Cursors.SizeNS))
            {
				LastY = y;
			}
		}

        #endregion

        #region ctlTransImageの上でマウスポインターを移動させたときに発生するイベント

        /// <summary>
        /// ctlTransImageの上でマウスポインターを移動させたときに発生するイベント
        /// </summary>
        /// <param name="sender">イベントのソース</param>
        /// <param name="e">イベントデータを格納しているオブジェクト</param>
        private void ctlTransImage_MouseMove(object sender, MouseEventArgs e)
		{
            MouseButtons button = e.Button;
            int x = e.X;
            int y = e.Y;

            int moveY = 0;
            int minY = 0;
            int maxY = 0;

			//マウスボタンが押されていない場合
            if (button == MouseButtons.None)
            {
				//マウスポインタを設定する
                if (Figure.IsNear(x, y, myLine.x1, myLine.y1))
                {
					this.Cursor = Cursors.SizeAll;
					drgline = 1;
				}
                else if (Figure.IsNear(x, y, myLine.x2, myLine.y2))
                {
					this.Cursor = Cursors.SizeAll;
					drgline = 2;
				}
                else if (Figure.PointOnLine(x, y, myLine.x1, myLine.y1, myLine.x2, myLine.y2))
                {
					this.Cursor = Cursors.SizeNS;
				}
                else
                {
					this.Cursor = Cursors.Default;
				}
			}
            //マウスボタンが押されていて、マウスポインタが上下方向の場合
            else if (object.ReferenceEquals(this.Cursor, Cursors.SizeNS))
            {
                minY = modLibrary.MinVal(myLine.y1, myLine.y2);
                maxY = modLibrary.MaxVal(myLine.y1, myLine.y2);

				//移動距離を求める（水平線が画面からはみ出さないようにする）
				moveY = modLibrary.CorrectInRange(y - LastY, -minY, ctlTransImage.Height - 1 - maxY);

				//水平線の移動
                myLine.y1 = Convert.ToInt16(myLine.y1 + moveY);
                myLine.y2 = Convert.ToInt16(myLine.y2 + moveY);
				LastY = y;

				//傾き・切片・上下差の表示
				Update(true);
			}
            else if (object.ReferenceEquals(this.Cursor, Cursors.SizeAll))
            {
				switch (drgline)
                {
					case 1:
                        myLine.y1 =  Convert.ToInt16(modLibrary.CorrectInRange(y, 0, ctlTransImage.Height - 1));  //始点側変更
						break;
					case 2:
                        myLine.y2 = Convert.ToInt16( modLibrary.CorrectInRange(y, 0, ctlTransImage.Height - 1));  //終点側変更
						break;
                    default:
                        break;
				}

				//傾き・切片・上下差の表示
				Update(true);
			}
		}

        #endregion

        #region ctlTransImageの上でマウスボタンが離されたときに発生するイベント

        /// <summary>
        /// ctlTransImageの上でマウスボタンが離されたときに発生するイベント
        /// </summary>
        /// <param name="sender">イベントのソース</param>
        /// <param name="e">イベントデータを格納しているオブジェクト</param>
		private void ctlTransImage_MouseUp(object sender, MouseEventArgs e)
		{
			this.Cursor = Cursors.Default;
		}

        #endregion

        #region ctlTransImageを再描画する必要があるときに発生するイベント

        /// <summary>
        /// ctlTransImageを再描画する必要があるときに発生するイベント
        /// </summary>
        /// <param name="sender">イベントのソース</param>
        /// <param name="e">イベントデータを格納しているオブジェクト</param>
        private void ctlTransImage_Paint(object sender, PaintEventArgs e)
		{
            
            //ctlTransImage.DrawLine(myLine.x1, myLine.y1, myLine.x2, myLine.y2, ColorTranslator.ToOle(Color.Lime));

            //PointF L1 = new PointF(myLine.x1, myLine.y1);
            //PointF L2 = new PointF(myLine.x2, myLine.y2);
            //ctlTransImage.SetLinePoint(LineConstants.ScanLine, ref L1, ref L2);
		}

        #endregion

        #region 倍率オプションボタンクリック時処理

        //*******************************************************************************
        //機　　能： 倍率オプションボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Index           [I/ ] 型        0:１倍 1:４倍 2:16倍
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		private void optScale_CheckedChanged(object sender, EventArgs e)
        {
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
            //cwsldWidth.Axis.Maximum = Choose(Index + 1, 256, 1024, 4096)   
            //'v17.00追加(ここから) byやまおか 2010/02/08
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
                    //cwsldWidth.Axis.Maximum = Choose(Index + 1, 4096, 16384, 50000)  '変更　山本　2009-11-13
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
            //v17.00追加(ここまで) byやまおか 2010/02/08

            cwsldLevel.Maximum = cwsldWidth.Maximum - 1;

            //削除2014/12/15hata
            //WLMin.Text = Convert.ToString(cwsldLevel.Minimum);
            //WLMax.Text = Convert.ToString(cwsldLevel.Maximum);
            //WDMin.Text = Convert.ToString(cwsldWidth.Minimum);
            //WDMax.Text = Convert.ToString(cwsldWidth.Maximum);

            ////追加2014/11/28hata_v19.51_dnet
            //WDMax.Left = cwsldWidth.Right - WDMax.Width;
            //WLMax.Left = cwsldLevel.Right - WLMax.Width;

            //Index値を記憶
            modCT30K.FimageBitIndex = Index;
 
        }

        #endregion

        #region コメントアウト

        //'*******************************************************************************
        //'機　　能： 階調変換用スライダー・値変更処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： Index           [I/ ] 型        0:ウィンドウレベル 1:ウィンドウ幅
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //'*******************************************************************************
        //Private Sub sldImg1_Change(Index As Integer)
        //
        //    '値を表示
        //    lblValue(Index).Caption = CStr(sldImg1(Index).Value)
        //
        //    '階調変換を実行
        //    Select Case Index
        //        Case 0: ctlTransImage.WindowLevel = sldImg1(Index).Value 'ウィンドウレベル
        //        Case 1: ctlTransImage.WindowWidth = sldImg1(Index).Value 'ウィンドウ幅
        //    End Select
        //
        //End Sub
        //
        //'*******************************************************************************
        //'機　　能： 階調変換用スライダー・スクロール時処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： Index           [I/ ] 型        0:ウィンドウレベル 1:ウィンドウ幅
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //'*******************************************************************************
        //Private Sub sldImg1_Scroll(Index As Integer)
        //
        //    sldImg1_Change Index
        //
        //End Sub

        #endregion

        #region 「はい」ボタンクリック時処理

        //*******************************************************************************
        //機　　能： 「はい」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		private void cmdOK_Click(object sender, EventArgs e)
		{
			//押したボタンの種類を通知   'V4.0 append by 鈴山 2001/02/01
			OK = true;

			//メカの移動有無プロパティをリセットする
			modSeqComm.SeqBitWrite("SPIIChangeReset", true);

			//scancondpar（コモン）の更新
			UpdateScancondpar();

			//mecainf（コモン）の更新
			UpdateMecainf();

			//scan_posi.csv の更新
            modCT30K.UpdateCsv(AppValue.SCANPOSI_CSV, "scan_posi_a[2]", CTSettings.scancondpar.Data.scan_posi_a[2].ToString("0.0000"));
            modCT30K.UpdateCsv(AppValue.SCANPOSI_CSV, "scan_posi_b[2]", CTSettings.scancondpar.Data.scan_posi_b[2].ToString("0.0000"));//ｽｷｬﾝ位置の切片

			//フォームを消去
            //変更2015/1/17hata_非表示のときにちらつくため
            //Hide();
            modCT30K.FormHide(this);

		}

        #endregion

        #region 「いいえ」ボタンクリック時処理

        //*******************************************************************************
        //機　　能： 「いいえ」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		private void cmdEnd_Click(object sender, EventArgs e)
		{
			//フォームを消去
            //変更2015/1/17hata_非表示のときにちらつくため
            //Hide();
            modCT30K.FormHide(this);

		}

        #endregion

        #region フォームロード時の処理

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
		private void frmScanPositionResult_Load(object sender, EventArgs e)
		{
            //追加2014/12/05hata_v19.51_dnet
            if (bLoadup) return;
            
			int adv = 0;

			//キャプションのセット
			SetCaption();

            #region コメントアウト

			//v11.21削除ここから by 間々田 2006/02/14
			//'v11.2追加ここから by 間々田 2006/01/11
			//Dim Size As Long
			//Size = v_size * h_size
			//
			//'２次元幾何歪補正パラメータ計算用
			//Dim gi()        As Single
			//Dim git()       As Single
			//Dim gj()        As Single
			//Dim gjt()       As Single
			//Dim Qidjd()     As Long
			//Dim Qidp1jd()   As Long
			//Dim Qidjdp1()   As Long
			//Dim Qidp1jdp1() As Long
			//Dim WorkTemp()  As Integer
			//
			//ReDim gi(Size)
			//ReDim git(Size)
			//ReDim gj(Size)
			//ReDim gjt(Size)
			//ReDim Qidjd(Size)
			//ReDim Qidp1jd(Size)
			//ReDim Qidjdp1(Size)
			//ReDim Qidp1jdp1(Size)
			//ReDim WorkTemp(Size)
			//
			//'２次元幾何歪の場合
			//If scaninh.full_distortion = 0 Then
			//
			//    With scancondpar
			//
			//        '２次元幾何歪パラメータ計算を行なう
			//        make_2d_fullindiv_table h_size, v_size, .ist, .ied, .jst, .jed, CLng(vm / hm), .alk(0), .blk(0), _
			//'                                gi(0), git(0), gj(0), gjt(0), Qidjd(0), Qidp1jd(0), Qidjdp1(0), Qidp1jdp1(0)
			//
			//        '２次元幾何歪補正を行なう
			//        cone_fullindiv_crct h_size, .ist, .ied, .jst, .jed, _
			//'                             gi(0), git(0), gj(0), gjt(0), Qidjd(0), Qidp1jd(0), Qidjdp1(0), Qidp1jdp1(0), _
			//'                             POSITION_IMAGE(0), WorkTemp(0)
			//        POSITION_IMAGE = WorkTemp
			//
			//    End With
			//
			//End If
			//
			//'スキャン位置校正画像の保存   added by 山本　2005-12-7
			//Call ImageSave(POSITION_IMAGE(0), SCPOSI_CORRECT, h_size, v_size)
			//
			//'自動スキャン位置校正時
			//If MyDispOnly Then
			//
			//    'AcqScanPosition h_size, v_size, MyA, MyB, POSITION_IMAGE(0)
			//
			//    'v11.21変更 by 間々田 2006/02/14 エラーを返した場合、メッセージを表示
			//    If AcqScanPosition(h_size, v_size, MyA, MyB, POSITION_IMAGE(0)) = -1 Then
			//        MsgBox "スキャン位置の抽出に失敗しました。再度実行してもエラーする場合は手動で実行してください。", vbExclamation
			//    End If
			//
			//'手動時：２次元
			//ElseIf scaninh.full_distortion = 0 Then
			//    MyA = scancondpar.cone_scan_posi_a
			//    MyB = scancondpar.cone_scan_posi_b
			//'手動時：１次元
			//Else
			//    MyA = scancondpar.scan_posi_a(2)
			//    MyB = scancondpar.scan_posi_b(2)
			//End If
			//'v11.2追加ここまで by 間々田 2006/01/11
			//v11.21削除ここまで by 間々田 2006/02/14

            #endregion

			//コントロールの初期化
			InitControls();

			//v17.60 英語用レイアウト調整 by長野 2011/05/25
			if (modCT30K.IsEnglish)
            {
				EnglishAdjustLayout();
			}

			//Select Case WLWWMax
			//    Case 1024
			//        optScale(1).Value = True
			//    Case 2048
			//        optScale(2).Value = True
			//    Case 4096
			//        optScale(3).Value = True
			//End Select

			//下記に変更 by 間々田 2005/01/07
            modLibrary.SetOption(optScale, modCT30K.FimageBitIndex);



			//画像の最大最小値を求める added by 山本 2005-12-20 START ///////////////////////////////////////
			int rc = 0;
			//Ipc32v5.RECT tmpReg = default(Ipc32v5.RECT);
			
            //Ipc32v5.RECT roiRect = default(Ipc32v5.RECT);   //ヒストグラム用矩形
            Winapi.RECT roiRect = default(Winapi.RECT);		//ヒストグラム用矩形
            float[] HistData = new float[11];
            #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
            
			//■スキャン位置校正画像の表示
            //tmpReg.left = 0;
            //tmpReg.top = 0;
            //tmpReg.right = ScanCorrect.h_size - 1;
            //tmpReg.bottom = ScanCorrect.v_size - 1;
            //rc = Ipc32v5.IpAppCloseAll();                                                                                   //☆☆開いている全ての画像ｳｨﾝﾄﾞを閉じる
            //rc = Ipc32v5.IpWsCreate((short)ScanCorrect.h_size, (short)ScanCorrect.v_size, 300, (short)Ipc32v5.IMC_GRAY16);  //空の画像ウィンドウを生成（Gray Scale 16形式）
            //rc = Ipc32v5.IpDocPutArea((short)Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, ref ScanCorrect.POSITION_IMAGE[0], (short)Ipc32v5.CPROG);
            //rc = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);                                                             //☆☆画像ウィンドの再描画
			
            ////■ヒストグラム処理を行なうＲＯＩを設定する
            //if (modScaninh.scaninh.full_distortion == 0)
            //{
            //    //roiRect.Left = scancondpar.ist + 1
            //    //roiRect.Top = scancondpar.jst + 1
            //    //roiRect.Right = scancondpar.ied - 1
            //    //roiRect.Bottom = scancondpar.jed - 1
            //    //v17.00 2010-02-11 パーキンエルマーFPDの場合は余裕を多くとる　山本
            //    roiRect.left = modScancondpar.scancondpar.ist + (modGlobal.DetType == modGlobal.DetectorConstants.DetTypePke ? 40 : 0);
            //    roiRect.top = modScancondpar.scancondpar.jst + (modGlobal.DetType == modGlobal.DetectorConstants.DetTypePke ? 40 : 0);
            //    roiRect.right = modScancondpar.scancondpar.ied - 1 - (modGlobal.DetType == modGlobal.DetectorConstants.DetTypePke ? 40 : 0);
            //    roiRect.bottom = modScancondpar.scancondpar.jed - 1 - (modGlobal.DetType == modGlobal.DetectorConstants.DetTypePke ? 40 : 0);
            //}
            //else
            //{
            //    roiRect.left = ScanCorrect.FRMWIDTH;
            //    roiRect.top = ScanCorrect.FRMWIDTH;
            //    roiRect.right = ScanCorrect.h_size - ScanCorrect.FRMWIDTH;
            //    roiRect.bottom = ScanCorrect.v_size - ScanCorrect.FRMWIDTH;
            //}

            ////■ヒストグラム処理をし、画素値の最大・最小値を求める
            //rc = Ipc32v5.IpAoiCreateBox(ref roiRect);
            //rc = Ipc32v5.IpHstCreate();                                             //☆☆ヒストグラムｳｨﾝﾄﾞを開く
            //rc = Ipc32v5.IpHstGet((short)Ipc32v5.GETSTATS, 0, ref HistData[0]);     //☆☆輝度統計を取得　～Max：HistData(4)、Min：HistData(3)
            //rc = Ipc32v5.IpHstDestroy();                                            //☆☆ヒストグラムｳｨﾝﾄﾞを閉る
            ////画像の最大最小値を求める added by 山本 2005-12-20 END ///////////////////////////////////////
            
            //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
            int height = CTSettings.detectorParam.v_size;
            int width = CTSettings.detectorParam.h_size;
            //■ヒストグラム処理を行なうＲＯＩを設定する
            if (CTSettings.scaninh.Data.full_distortion == 0)
            {
                roiRect.left = CTSettings.scancondpar.Data.ist + (CTSettings.detectorParam.DetType == TransImage.DetectorConstants.DetTypePke ? 40 : 0);
                roiRect.top = CTSettings.scancondpar.Data.jst + (CTSettings.detectorParam.DetType == TransImage.DetectorConstants.DetTypePke ? 40 : 0);
                roiRect.right = CTSettings.scancondpar.Data.ied - 1 - (CTSettings.detectorParam.DetType == TransImage.DetectorConstants.DetTypePke ? 40 : 0);
                roiRect.bottom = CTSettings.scancondpar.Data.jed - 1 - (CTSettings.detectorParam.DetType == TransImage.DetectorConstants.DetTypePke ? 40 : 0);
            }
            else
            {
                roiRect.left = ScanCorrect.FRMWIDTH;
                roiRect.top = ScanCorrect.FRMWIDTH;
                roiRect.right = CTSettings.detectorParam.h_size - ScanCorrect.FRMWIDTH;
                roiRect.bottom = CTSettings.detectorParam.v_size - ScanCorrect.FRMWIDTH;
            }
            rc = CallImageProFunction.CallGetHistgramParam(ScanCorrect.POSITION_IMAGE, HistData, height, width, roiRect.left, roiRect.top, roiRect.right, roiRect.bottom);
            #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

            //スライダー値の設定
			//'    sldImg1(0).Value = MinVal(wlevel, sldImg1(0).Max) '値が最大値を超えないようにする
			//'    sldImg1(1).Value = MinVal(wwidth, sldImg1(1).Max) '値が最大値を超えないようにする
			//    sldImg1(0).Value = MinVal((HistData(4) - HistData(3)) / 2, sldImg1(0).Max)          'changed by 山本　2005-12-20　ウィンドウレベルを自動設定
			//    sldImg1(1).Value = MinVal(MaxVal((HistData(4) - HistData(3)), 1), sldImg1(1).Max)   'changed by 山本　2005-12-20　ウィンドウ幅を自動設定

			//ウィンドウレベル・ウィンドウ幅のセット
			//cwsldLevel.Value = (HistData(4) - HistData(3)) / 2
            //2014/11/07hata キャストの修正
            //cwsldLevel.Value = (int)(HistData[4] + HistData[3]) / 2;     //v17.00修正　山本 2009-10-07
            //cwsldWidth.Value = (int)(HistData[4] - HistData[3] + 1);
            cwsldLevel.Value = Convert.ToInt32((HistData[4] + HistData[3]) / 2F);     //v17.00修正　山本 2009-10-07
            cwsldWidth.Value = Convert.ToInt32(HistData[4] - HistData[3] + 1);

			//上記の処理ではPointerValueChangedイベントは発生しないので以下の処理を行なう
			//cwsldLevel_PointerValueChanged(cwsldLevel, new AxCWUIControlsLib._DCWSlideEvents_PointerValueChangedEvent(0, (cwsldLevel.Value)));
			//cwsldWidth_PointerValueChanged(cwsldWidth, new AxCWUIControlsLib._DCWSlideEvents_PointerValueChangedEvent(0, (cwsldWidth.Value)));
            cwsldLevel_PointerValueChanged(cwsldLevel, EventArgs.Empty);
            cwsldWidth_PointerValueChanged(cwsldWidth, EventArgs.Empty);
			

			//Ｘ線検出器がフラットパネルの場合           'V7.0 added by 間々田 03/09/25
            if (CTSettings.detectorParam.Use_FlatPanel)
            {
				//補正
				//        If GetGainImage() Then Call FpdGainCorrect(POSITION_IMAGE(0), GAIN_IMAGE(0), h_size, v_size, 0)
				//        If GetDefImage() Then Call FpdDefCorrect_short(POSITION_IMAGE(0), Def_IMAGE(0), h_size, v_size, 0, v_size - 1)
				//change by 間々田 2003/10/14
				if (ScanCorrect.GetDefGainOffset(ref adv))
                {
					//v17.00 if追加 byやまおか 2010/01/20
                    if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypeHama))
                    {
                        ScanCorrect.FpdGainCorrect(ref ScanCorrect.POSITION_IMAGE[0], ref ScanCorrect.GAIN_IMAGE[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, adv);
                        ScanCorrect.FpdDefCorrect_short(ref ScanCorrect.POSITION_IMAGE[0], ref ScanCorrect.Def_IMAGE[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, 0, CTSettings.detectorParam.v_size - 1);
                    }

                    //追加2014/12/15hata
                    //欠陥データとGainデータをtransImageCtrlへセットする
                    CTSettings.transImageControl.SetDefGain(ScanCorrect.Def_IMAGE, ScanCorrect.GAIN_IMAGE, adv);
				}
			}

			//階調変換の前に画像をコピーする。(POSITION_IMAGE→IMAGE_L)
			//    Call ChangeImageSize_HV(POSITION_IMAGE(0), Image_L(0), h_size, v_size, phm, pvm)   'v9.7削除 by 間々田 2004/11/09

			//    '階調変換オブジェクトの生成                                 'v9.7追加 by 間々田 2004/11/09
			//    Set MyLUT = New clsLUT
			//
			//    '変換対象となる画像の配列と制御するコントロールを登録       'v9.7追加 by 間々田 2004/11/09
			//    MyLUT.SetWorkImage POSITION_IMAGE(), h_size, v_size, Image1
			//
			//    '階調変換処理
			//    Call sldImg1_Change(0)

			//ドラッグ状態初期化
			//    drgline = 0

			//ctlTransImage.SetImage(ScanCorrect.POSITION_IMAGE);
            BmpICtrl.SetImage(ScanCorrect.POSITION_IMAGE);
            ctlTransImage.Picture = BmpICtrl.Picture;

			//CT30Kをアクティブにする
            frmCTMenu.Instance.Activate(); //v16.20追加 byやまおか 2010/04/21
        }

        #endregion

        #region コメントアウト

        //'*******************************************************************************
        //'機　　能： フォームアンロード時の処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //'*******************************************************************************
        //Private Sub Form_Unload(Cancel As Integer)
        //
        //    '階調変換オブジェクトを破棄                                 'v9.7追加 by 間々田 2004/11/09
        //    Set MyLUT = Nothing
        //
        //End Sub
        //
        //'*******************************************************************************
        //'機　　能： フォームリサイズ時の処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //'*******************************************************************************
        //Private Sub Form_Resize()
        //
        //    '表示位置の調整
        //    Me.Top = MinVal(Me.Height + Me.Top, frmCTMenu.SysInfo1.WorkAreaHeight) - Me.Height
        //
        //End Sub

        #endregion

        #region 各コントロールのキャプションに文字列をセットする

        //*******************************************************************************
        //機　　能： 各コントロールのキャプションに文字列をセットする
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		private void SetCaption()
		{
			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);

			//フォームのキャプション：スキャン位置校正結果
			this.Text = StringTable.BuildResStr(StringTable.IDS_Result, StringTable.IDS_CorScanPos);

			if (modCT30K.IsEnglish)
            {
                //lblSa.Top = lblSa.Top - 100;
                lblSa.Top = lblSa.Top - 7;
            }

			lblWL.Text = StringTable.LoadResStringWithColon(StringTable.IDS_WindowLevel);   //ウィンドウレベル：
			lblWW.Text = StringTable.LoadResStringWithColon(StringTable.IDS_WindowWidth);   //ウィンドウ幅：

			//optScale(0).Caption = GetResString(IDS_Times, " 1")         ' 4 倍 → 1 倍に変更 by 間々田 2005/01/07
			//optScale(1).Caption = GetResString(IDS_Times, " 4")         ' 8 倍 → 4 倍に変更 by 間々田 2005/01/07
			//optScale(2).Caption = GetResString(IDS_Times, "16")         '16 倍
			optScale[0].Text = " 1 / 16";       //v17.10変更 byやまおか 2010/08/26
			optScale[1].Text = " 1 / 4";        //v17.10変更 byやまおか 2010/08/26
			optScale[2].Text = " 1 / 1";        //v17.10変更 byやまおか 2010/08/26
		}

        #endregion

        #region ２値化画像のサイズ設定

        //********************************************************************************
        //機    能  ：  ２値化画像のサイズ設定
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  設定に応じて、各コントロールのサイズ・位置を調整する。
        //              基準となる項目は、下記の通り。
        //
        //                横サイズ → H_SIZE
        //                縦サイズ → V_SIZE
        //
        //履    歴  ：  V2.0   00/03/08  (SI1)鈴山       新規作成
        //********************************************************************************
		private void InitControls()
		{
			int mod_SizeX = 0;  //v17.00追加 byやまおか 2010/03/03
			int mod_SizeY = 0;  //v17.00追加 byやまおか 2010/03/03

			//透視画像表示コントロール
            ctlTransImage.SizeX = CTSettings.detectorParam.h_size;
			ctlTransImage.SizeY = CTSettings.detectorParam.v_size;
            ctlTransImage.Width = Convert.ToInt32(CTSettings.detectorParam.h_size / CTSettings.detectorParam.phm);
			ctlTransImage.Height = Convert.ToInt32(CTSettings.detectorParam.v_size / CTSettings.detectorParam.pvm);

            ctlTransImage.Enabled = true;

			//PkeFPDの場合は額縁を考慮する   'v17.02修正 byやまおか 2010/07/28
			//If (DetType = DetTypePke) Then
			//v17.22変更 byやまおか 2010/10/19
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) && (!CTSettings.detectorParam.Use_FpdAllpix))
            {
                //Rev20.00 追加 by長野 2014/12/04
                //Rev20.00 間違いなのでコメントアウト by長野 2015/02/28
                //BmpICtrl.MirrorOn = true;
                //ctlTransImage.MirrorOn = BmpICtrl.MirrorOn;

                //画像サイズの端数(例2048なら48)
                mod_SizeX = Convert.ToInt32((ctlTransImage.SizeX % 100) / CTSettings.detectorParam.phm);
                mod_SizeY = Convert.ToInt32((ctlTransImage.SizeY % 100) / CTSettings.detectorParam.pvm);
				//左上にずらす(額縁をはみ出させる)
				//.Left = .Left - mod_SizeY / 2
				//.Top = .Top - mod_SizeY / 2
                //2014/11/07hata キャストの修正
                //ctlTransImage.Left = -mod_SizeX / 2;    //v17.10変更 byやまおか 2010/08/19
                //ctlTransImage.Top = -mod_SizeY / 2;     //v17.10変更 byやまおか 2010/08/19
                ctlTransImage.Left = Convert.ToInt32(-mod_SizeX / 2F);    //v17.10変更 byやまおか 2010/08/19
                ctlTransImage.Top = Convert.ToInt32(-mod_SizeY / 2F);     //v17.10変更 byやまおか 2010/08/19
				
				//傾き、切片、上下差を下にずらす 'v17.10追加 byやまおか 2010/08/19
                //lblPosiaTitle.Top = lblPosiaTitle.Top + 120 / 15;
                //lblColon[0].Top = lblColon[0].Top + 120 / 15;
                //lblScanPosiA.Top = lblScanPosiA.Top + 120 / 15;
                //lblPosibtitle.Top = lblPosibtitle.Top + 120 / 15;
                //lblColon[1].Top = lblColon[1].Top + 120 / 15;
                //lblScanPosiB.Top = lblScanPosiB.Top + 120 / 15;
                //lblSa.Top = lblSa.Top + 120 / 15;
                //lblColon[2].Top = lblColon[2].Top + 120 / 15;
                //lblPosiAB.Top = lblPosiAB.Top + 120 / 15;

                //Rev20.00 コメントアウト by長野 2014/12/04
                //InitControlがLoadと本番の読み込みで２重に実行されていたため位置がずれていた。
                //・コメントアウトすると正しく動く。なぜ？
                //・Colonの位置が下記のソース通り動いていない。なぜ？

                //lblPosiaTitle.Top = lblPosiaTitle.Top + 120;
                //lblColon[0].Top = lblColon[0].Top + 120;
                //lblScanPosiA.Top = lblScanPosiA.Top + 120;
                //lblPosibtitle.Top = lblPosibtitle.Top + 120;
                //lblColon[1].Top = lblColon[1].Top + 120;
                //lblScanPosiB.Top = lblScanPosiB.Top + 120;
                //lblSa.Top = lblSa.Top + 120;
                //lblColon[2].Top = lblColon[2].Top + 120;
                //lblPosiAB.Top = lblPosiAB.Top + 120;
			}
            else
            {
                //Rev20.00 追加 by長野 2014/12/04
                BmpICtrl.MirrorOn = false;
                ctlTransImage.MirrorOn = BmpICtrl.MirrorOn;
                
                //画像サイズの端数は気にしない
				mod_SizeX = 0;
				mod_SizeY = 0;
                ctlTransImage.Left = 0; //v17.21追加 byやまおか 2010/10/06
                ctlTransImage.Top = 0;  //v17.21追加 byやまおか 2010/10/06			
			}

            //追加2014/06/03(検S1)hata
            lblSa.Height = lblPosiaTitle.Height;


			//フォーム
			//Me.width = Screen.TwipsPerPixelX * (MaxVal(.width, fraControl.width) + 4 + 16)
			//Me.Height = Screen.TwipsPerPixelY * (.Height + fraControl.Height + 25 + 8)
			//PkeFPDの場合は一回り小さくする 'v17.02修正 byやまおか 2010/07/28
            this.Width = modLibrary.MaxVal(ctlTransImage.Width, fraControl.Width) + 4 - mod_SizeX;
            this.Height = ctlTransImage.Height + fraControl.Height + 25 - mod_SizeY;

            //追加2014/11/28hata_v19.51_dnet
            //topの位置を設定する
            if ((this.Top + this.Height) > Screen.PrimaryScreen.Bounds.Height)
            {
                int mytop = Screen.PrimaryScreen.Bounds.Height - frmCTMenu.Instance.Height;
                int top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
                if (top - mytop <= 0) top = 0;
                this.Top = top;
            }

			//コントロールフレーム
			//fraControl.Move ScaleWidth / 2 - fraControl.width / 2, .Height
			//PkeFPDの場合はコントロールフレームでゲイン画像の下端を隠す     'v17.02修正 byやまおか 2010/07/28
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
            {
                //2014/11/07hata キャストの修正
                //fraControl.SetBounds(0, (ctlTransImage.Top) + (ctlTransImage.Height) - mod_SizeY / 2, (ctlTransImage.Width), 0, BoundsSpecified.X | BoundsSpecified.Y | BoundsSpecified.Width);
                fraControl.SetBounds(0, Convert.ToInt32((ctlTransImage.Top) + (ctlTransImage.Height) - mod_SizeY / 2F), (ctlTransImage.Width), 0, BoundsSpecified.X | BoundsSpecified.Y | BoundsSpecified.Width);
			}
            else
            {
                //2014/11/07hata キャストの修正
                //fraControl.SetBounds(ClientRectangle.Width / 2 - fraControl.Width / 2, ctlTransImage.Height, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
                fraControl.SetBounds(Convert.ToInt32(ClientRectangle.Width / 2F - fraControl.Width / 2F), ctlTransImage.Height, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
            }

            #region コメントアウト

			//    'フォーム
			//    Me.Width = Display1.Width + 360
			//    Me.Height = Display1.Height + fraControl.Height + 405 + 120
			//
			//    '階調コントロールフレームの位置
			//    fraControl.Move Me.Width / 2 - fraControl.Width / 2, Display1.Top + Display1.Height
			//
			//    'ドラッグ用のラベル：透明にし、見えないようにする
			//    lbl1(1).BackStyle = vbTransparent
			//    lbl1(2).BackStyle = vbTransparent
			//
			//    'Line1の設定
			//    'Line1.x1 = 0
			//    'Line1.y1 = Screen.TwipsPerPixelY * CorrectInRange((v_size / 2 - GVal_ScanPosiA(2) * h_size / 2 + GVal_ScanPosiB(2)) / pvm, 1, Image1.SizeY - 1)
			//    'Line1.x2 = Display1.Width
			//    'Line1.y2 = Screen.TwipsPerPixelY * CorrectInRange((v_size / 2 + GVal_ScanPosiA(2) * h_size / 2 + GVal_ScanPosiB(2)) / pvm, 1, Image1.SizeY - 1)
			//
			//    'v11.2以下に変更
			//    With Line1
			//        .x1 = 0
			//        .y1 = Screen.TwipsPerPixelY * CorrectInRange(((v_size - 1) / 2 - MyA * (h_size - 1) / 2 + MyB) / pvm, 0, Image1.SizeY - 1)  'changed by 山本　2005-12-6
			//        .x2 = Display1.Width - 1
			//        .y2 = Screen.TwipsPerPixelY * CorrectInRange(((v_size - 1) / 2 + MyA * (h_size - 1) / 2 + MyB) / pvm, 0, Image1.SizeY - 1) 'changed by 山本　2005-12-6
			//    End With

            #endregion

			//倍率のフレーム
            fraScale.Visible = ((CTSettings.scancondpar.Data.fimage_bit == 2) && CTSettings.detectorParam.Use_FlatPanel);	//v11.2変更 by 間々田 05/10/17
		}

        #endregion

        #region ダイアログ処理

        //*******************************************************************************
        //機　　能： ダイアログ処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [ /O] Boolean   True:「はい」・False:「いいえ」
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        //Public Function Dialog() As Boolean
		public bool Dialog(bool IsAuto = false) //v11.2変更 by 間々田 2005/12/05
		{
			bool functionReturnValue = false;

			//戻り値用変数の初期化
			OK = false;

            //先にFormをロードする
            frmScanPositionResult_Load(this, EventArgs.Empty);

            //追加2014/12/05hata_v19.51_dnet
            bLoadup = true;　

			//モーダルでフォームを表示
			//Show vbModal
			if (GetSlopeAndBias(IsAuto)) //v11.21変更 by 間々田 2006/02/14
            {
                //変更2014/12/22hata_dNet_オーナーフォームを指定する
                //ShowDialog();
                this.ShowDialog(frmCTMenu.Instance);          
            }  

			//戻り値のセット
			functionReturnValue = OK;

			//フォームをアンロード
			this.Close();

			return functionReturnValue;
		}

        #endregion

        #region scancondpar（コモン）の更新

        //*******************************************************************************
        //機　　能： scancondpar（コモン）の更新
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： スキャン位置校正関連のパラメータのみ更新
        //
        //履　　歴： v11.2  05/10/17  (SI3)間々田    新規作成・２次元幾何歪校正対応
        //*******************************************************************************
		private void UpdateScancondpar()
		{
			double FlatPanel_dpm = 0;
			float FlatPanel_e_dash = 0;
			float FlatPanel_Ils = 0;
			float FlatPanel_Ile = 0;
			float FlatPanel_kv = 0;
			float FlatPanel_bb0 = 0;
			float FlatPanel_Dpi = 0;
			float FlatPanel_FGD = 0;
			double a_0 = 0;
			double b_0 = 0;
			double a0_bar = 0;
			double b0_bar = 0;
			int xls = 0;
			int xle = 0;
			double a0 = 0;
			double A1 = 0;
			double a2 = 0;
			double a3 = 0;
			double a4 = 0;
			double a5 = 0;
			float kv = 0;
			float bb0 = 0;
			float FGD = 0;
			float jc = 0;
			float Dpi = 0;
			float e_dash = 0;
			float Delta_Ip = 0;
			float Ils = 0;
			float Ile = 0;
			double dpm = 0;

			//v17.00追加(ここから)　山本 2010-02-06
            if (CTSettings.detectorParam.Use_FlatPanel)
            {
                CTSettings.scancondpar.Data.scan_posi_a[2] = myA + CTSettings.scancondpar.Data.rc_slope_ft;   //傾き
                CTSettings.scancondpar.Data.scan_posi_b[2] = myB;	                                        //切片
                CTSettings.scancondpar.Data.cone_scan_posi_a = myA + CTSettings.scancondpar.Data.rc_slope_ft;
                CTSettings.scancondpar.Data.cone_scan_posi_b = myB;

				//v19.10　v19.20の反映 フラットパネルも傾きを使用してdpmを計算しているので、dpmの再計算が必要 2012/11/28 by長野
                ScanCorrect.Cal_ConeBeamParameter((CTSettings.scaninh.Data.full_distortion == 0 ? CTSettings.scancondpar.Data.alk[1] : ScanCorrect.B1),
                                                    CTSettings.scancondpar.Data.cone_scan_posi_a, 
                                                    ref FlatPanel_dpm);
				//v11.2変更 by 間々田 2005/10/06
                CTSettings.scancondpar.Data.dpm = Convert.ToSingle(FlatPanel_dpm);
				//v19.10 v19.20の反映 cone_max_mfanangleが更新されないとmcが正しく求まらないため、ここで更新 2012/11/28 by長野
                FlatPanel_FGD = ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[modCT30K.GetFcdOffsetIndex()];
				FlatPanel_bb0 = ScanCorrect.GVal_ScanPosiA[2];
                FlatPanel_kv = CTSettings.detectorParam.vm / CTSettings.detectorParam.hm;
				FlatPanel_Dpi = (float)(10 / ScanCorrect.B1);
				FlatPanel_e_dash = (float)(Math.Atan(FlatPanel_kv * FlatPanel_bb0));

                //変更2014/10/07hata_v19.51反映
                ////v19.12 ±8に変更 by長野 2013/03/06
                //FlatPanel_Ils = CTSettings.scancondpar.Data.ist + 8;
                //FlatPanel_Ile = CTSettings.scancondpar.Data.ied - 8;
                ////FlatPanel_Ils = .ist + 2
                ////FlatPanel_Ile = .ied - 2
                if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
                {
                    if (CTSettings.detectorParam.h_size == 1024)
                    {
                        FlatPanel_Ils = CTSettings.scancondpar.Data.ist + 12;
                        FlatPanel_Ile = CTSettings.scancondpar.Data.ied - 12;
                    }
                    else if (CTSettings.detectorParam.h_size == 2048)
                    {
                        FlatPanel_Ils = CTSettings.scancondpar.Data.ist + 24;
                        FlatPanel_Ile = CTSettings.scancondpar.Data.ied - 24;
                    }
                }
                else
                {
                    FlatPanel_Ils = CTSettings.scancondpar.Data.ist + 2;
                    FlatPanel_Ile = CTSettings.scancondpar.Data.ied - 2;
                }

                CTSettings.scancondpar.Data.cone_max_mfanangle =Convert.ToSingle(2 * Math.Atan((FlatPanel_Ile - FlatPanel_Ils) * FlatPanel_Dpi / Math.Cos(FlatPanel_e_dash) * 1.02 * 0.5 / FlatPanel_FGD));
            
            }
            else //v17.00追加(ここまで)　山本 2010-02-06
            {
				//２次元幾何歪補正の場合
                if (CTSettings.scaninh.Data.full_distortion == 0)
                {
					//Dim kmax    As Long    'deleted by 山本　2005-12-21 Public宣言に変更

					//hole_dataの読み込み
					ScanCorrect.kmax = ScanCorrect.ReadHole();

					//a_0 = GVal_ScanPosiB(2) - GVal_ScanPosiA(2) * (h_size - 1) / 2 + (v_size - 1) / 2   'changed by 山本　2005-12-6
					//b_0 = GVal_ScanPosiA(2)
                    //2014/11/07hata キャストの修正
                    //a_0 = myB - myA * (CTSettings.detectorParam.h_size - 1) / 2 + (CTSettings.detectorParam.v_size - 1) / 2;
                    a_0 = myB - myA * (CTSettings.detectorParam.h_size - 1) / 2D + (CTSettings.detectorParam.v_size - 1) / 2D;
					//changed by 山本　2005-12-6
					b_0 = myA;
                    ScanCorrect.Cal_1d_Distortion_Parameter(Convert.ToInt32(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm), CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, a_0, b_0, ref CTSettings.scancondpar.Data.alk[0], ref CTSettings.scancondpar.Data.blk[0], ScanCorrect.kmax, ref ScanCorrect.gx[0], ref ScanCorrect.gy[0],
					                                        ref ScanCorrect.gg[0], ref ScanCorrect.gh[0], ref a0_bar, ref b0_bar, ref xls, ref xle, ref a0, ref A1, ref a2, ref a3, ref a4, ref a5);

                    CTSettings.scancondpar.Data.scan_posi_a[2] = Convert.ToSingle(b0_bar);                                                       //changed by 山本　2005-12-7
                    //2014/11/07hata キャストの修正
                    //CTSettings.scancondpar.Data.scan_posi_b[2] = Convert.ToSingle(a0_bar + b0_bar * (CTSettings.detectorParam.h_size - 1) / 2 - (CTSettings.detectorParam.v_size - 1) / 2);  //changed by 山本　2005-12-6
                    CTSettings.scancondpar.Data.scan_posi_b[2] = Convert.ToSingle(a0_bar + b0_bar * (CTSettings.detectorParam.h_size - 1) / 2D - (CTSettings.detectorParam.v_size - 1) / 2D);  //changed by 山本　2005-12-6
					
					//端のデータを使用しないための処理　added by 山本　2005-12-13
					if (xls < 4)
                    {
						xls = 4;
                    }
                    if (xle > CTSettings.detectorParam.h_size - 5)
                    {
                        xle = CTSettings.detectorParam.h_size - 5;
                    }

                    //CTSettings.scancondpar.Data.a[0, 2] = a0;    //scancondpar.a2,0のこと
                    //CTSettings.scancondpar.Data.a[1, 2] = A1;    //scancondpar.a2,1のこと
                    //CTSettings.scancondpar.Data.a[2, 2] = a2;    //scancondpar.a2,2のこと
                    //CTSettings.scancondpar.Data.a[3, 2] = a3;    //scancondpar.a2,3のこと
                    //CTSettings.scancondpar.Data.a[4, 2] = a4;    //scancondpar.a2,4のこと
                    //CTSettings.scancondpar.Data.a[5, 2] = a5;    //scancondpar.a2,5のこと
                    CTSettings.scancondpar.Data.a[2 * 6 + 0] = Convert.ToSingle(a0);    //scancondpar.a2,0のこと
                    CTSettings.scancondpar.Data.a[2 * 6 + 1] = Convert.ToSingle(A1);    //scancondpar.a2,1のこと
                    CTSettings.scancondpar.Data.a[2 * 6 + 2] = Convert.ToSingle(a2);    //scancondpar.a2,2のこと
                    CTSettings.scancondpar.Data.a[2 * 6 + 3] = Convert.ToSingle(a3);    //scancondpar.a2,3のこと
                    CTSettings.scancondpar.Data.a[2 * 6 + 4] = Convert.ToSingle(a4);    //scancondpar.a2,4のこと
                    CTSettings.scancondpar.Data.a[2 * 6 + 5] = Convert.ToSingle(a5);    //scancondpar.a2,5のこと
                    
                    CTSettings.scancondpar.Data.xls[2] = xls;
                    CTSettings.scancondpar.Data.xle[2] = xle;
                    CTSettings.scancondpar.Data.mdtpitch[2] = Convert.ToSingle(10 / A1);

					//最大ファン角を求める為の変数

					//最大ファン角の計算
                    kv = CTSettings.detectorParam.vm / CTSettings.detectorParam.hm;
					//bb0 = GVal_ScanPosiA(2)
					bb0 = myA;

                    FGD = ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[modCT30K.GetFcdOffsetIndex()];

                    //2014/11/07hata キャストの修正
                    //jc = (CTSettings.detectorParam.v_size - 1) / 2;
                    jc = (float)(CTSettings.detectorParam.v_size - 1) / 2F;
					Dpi = (float)(10 / A1);
                    e_dash = (float)(Math.Atan(kv * bb0));
                    Delta_Ip = (float)(Math.Floor(2 + jc * kv * kv * Math.Abs(bb0)));

					Ils = Delta_Ip;
                    Ile = CTSettings.detectorParam.h_size - Delta_Ip - 1;
                    CTSettings.scancondpar.Data.max_mfanangle[2] = Convert.ToSingle(2 * Math.Atan((Ile - Ils) * Dpi / Math.Cos(e_dash) * 1.02 * 0.5 / FGD));

					//コーンビームスキャンが可能な場合
                    if (CTSettings.scaninh.Data.data_mode[2] == 0)
                    {
						//最大ファン角の計算
						Dpi = (float)(10 / ScanCorrect.B1);
                        Ils = CTSettings.scancondpar.Data.ist + 2;
                        Ile = CTSettings.scancondpar.Data.ied - 2;
                        CTSettings.scancondpar.Data.cone_max_mfanangle = Convert.ToSingle(2 * System.Math.Atan((Ile - Ils) * Dpi / System.Math.Cos(e_dash) * 1.02 * 0.5 / FGD));

						//Cal_ConeBeamParameter B1, GVal_ScanPosiA(2), dpm
						ScanCorrect.Cal_ConeBeamParameter(ScanCorrect.B1, myA, ref dpm);
                        CTSettings.scancondpar.Data.dpm =Convert.ToSingle(dpm);

						//.cone_scan_posi_a = GVal_ScanPosiA(2) + .rc_slope_ft
						//.cone_scan_posi_b = GVal_ScanPosiB(2)
                        CTSettings.scancondpar.Data.cone_scan_posi_a = myA + CTSettings.scancondpar.Data.rc_slope_ft;
                        CTSettings.scancondpar.Data.cone_scan_posi_b = myB;
					}
				}
                //１次元幾何歪補正の場合
                else
                {
					//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
					//.scan_posi_a(2) = GVal_ScanPosiA(2) + .rc_slope_ft  '傾き   rc_slope_ft:コーンの傾き調整パラメータ
					//.scan_posi_b(2) = GVal_ScanPosiB(2)                 '切片
					//                    .scan_posi_a(2) = myA + .rc_slope_ft  '傾き   rc_slope_ft:コーンの傾き調整パラメータ
					//                    .scan_posi_b(2) = myB                 '切片
					//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
				}
			}
			//v17.02 if追加 byやまおか 2010/07/28

			//Scancondpar（コモン）の書き込み
			//modScancondpar.CallPutScancondpar();
            CTSettings.scancondpar.Write();

			//透視画像上のスライスラインを更新する   'v15.0変更 by 間々田 2009/06/17
			//if (modLibrary.IsExistForm(frmTransImage.Instance))
            if (modLibrary.IsExistForm("frmTransImage"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
            {
				frmTransImage.Instance.SetLine();
			}
		}

        #endregion

        #region mecainf（コモン）の更新

        //*******************************************************************************
        //機　　能： mecainf（コモン）の更新
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： スキャン位置校正関連のパラメータのみ更新
        //
        //履　　歴： v11.2  05/10/17  (SI3)間々田    新規作成
        //*******************************************************************************
		private void UpdateMecainf()
		{
			//mecainfType theMecainf = default(mecainfType);
            MecaInf theMecainf = new MecaInf();
            theMecainf.Data.Initialize();

			//mecainf（コモン）取得
			//modMecainf.GetMecainf(ref theMecainf);
            theMecainf.Load();

			//I.I.視野
			theMecainf.Data.sp_iifield = modSeqComm.GetIINo();

			//Ｘ線管
            theMecainf.Data.sp_mt = CTSettings.scansel.Data.multi_tube;

			//ビニングモード
            theMecainf.Data.sp_bin = CTSettings.scansel.Data.binning;//0:１×１，1:２×２，2:４×４

			//スキャン位置校正を行った時の年月日   'v17.00追加 byやまおか 2010/03/02
            theMecainf.Data.sp_date = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));    //YYYYMMDD形式

			//スキャン位置校正を行った時の時間     'v17.00追加 byやまおか 2010/03/04
            theMecainf.Data.sp_time = Convert.ToInt32(DateTime.Now.ToString("HHmmss"));      //HHMMSS形式

			//mecainf（コモン）更新
			//modMecainf.PutMecainf(ref theMecainf);
            theMecainf.Write();

        }

        #endregion

        #region  傾きと切片を求める

        //*******************************************************************************
        //機　　能： 傾きと切片を求める
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [ /O] Boolean
        //
        //補　　足： なし
        //
        //履　　歴： v11.21  06/02/14  (SI3)間々田    新規作成
        //*******************************************************************************
		private bool GetSlopeAndBias(bool IsAuto = false)
		{
			//戻り値初期化
			bool functionReturnValue = false;

            #region コメントアウト

			//   ２次元幾何歪補正処理を関数化    by YAMAKAGE 09-07-10
			//    Dim Upper As Long
			//    Upper = v_size * h_size - 1
			//
			//    '２次元幾何歪補正パラメータ計算用
			//    Dim gi()        As Single
			//    Dim git()       As Single
			//    Dim gj()        As Single
			//    Dim gjt()       As Single
			//    Dim Qidjd()     As Long
			//    Dim Qidp1jd()   As Long
			//    Dim Qidjdp1()   As Long
			//    Dim Qidp1jdp1() As Long
			//    Dim WorkTemp()  As Integer
			//
			//    ReDim gi(Upper)
			//    ReDim git(Upper)
			//    ReDim gj(Upper)
			//    ReDim gjt(Upper)
			//    ReDim Qidjd(Upper)
			//    ReDim Qidp1jd(Upper)
			//    ReDim Qidjdp1(Upper)
			//    ReDim Qidp1jdp1(Upper)
			//    ReDim WorkTemp(Upper)

            #endregion

			//２次元幾何歪の場合
            if (CTSettings.scaninh.Data.full_distortion == 0)
            {
				//２次元幾何歪補正
				modScanCorrectNew.DistortionCorrect(ref ScanCorrect.POSITION_IMAGE);

                #region コメントアウト

				//   ２次元幾何歪補正処理を関数化    by YAMAKAGE 09-07-10
				//        With scancondpar
				//
				//            '２次元幾何歪パラメータ計算を行なう
				//            make_2d_fullindiv_table h_size, v_size, .ist, .ied, .jst, .jed, CLng(vm / hm), .alk(0), .blk(0), _
				//'                                    gi(0), git(0), gj(0), gjt(0), Qidjd(0), Qidp1jd(0), Qidjdp1(0), Qidp1jdp1(0)
				//
				//            '２次元幾何歪補正を行なう
				//            cone_fullindiv_crct h_size, .ist, .ied, .jst, .jed, _
				//'                                 gi(0), git(0), gj(0), gjt(0), Qidjd(0), Qidp1jd(0), Qidjdp1(0), Qidp1jdp1(0), _
				//'                                 POSITION_IMAGE(0), WorkTemp(0)
				//            POSITION_IMAGE = WorkTemp
				//
				//        End With

                #endregion
			}

			//スキャン位置校正画像の保存   added by 山本　2005-12-7
            ScanCorrect.ImageSave(ref ScanCorrect.POSITION_IMAGE[0], ScanCorrect.SCPOSI_CORRECT, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);

            //Rev22.00 Rev20.03の反映 by長野 2015/07/29
            BmpICtrl.SetImage(ScanCorrect.POSITION_IMAGE);
            ctlTransImage.Picture = BmpICtrl.Picture;

			//自動スキャン位置校正時
			if (IsAuto)
            {
				//AcqScanPosition h_size, v_size, MyA, MyB, POSITION_IMAGE(0)

				#if !NoCamera
				//v11.21変更 by 間々田 2006/02/14 エラーを返した場合、メッセージを表示
                if (ScanCorrect.AcqScanPosition(CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, ref myA, ref myB, ref ScanCorrect.POSITION_IMAGE[0]) == -1)
                {
					//            MsgBox "スキャン位置の抽出に失敗しました。再度実行してもエラーする場合は手動で実行してください。", vbCritical
                    MessageBox.Show(CTResources.LoadResString(20076) + System.Environment.NewLine + CTResources.LoadResString(20077), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);//ストリングテーブル化 '17.60 by長野 2011/05/22
                    return functionReturnValue;
				}
				#endif

				ctlTransImage.Enabled = false;
			}
            //手動時：２次元
            else if (CTSettings.scaninh.Data.full_distortion == 0)
            {
                myA = CTSettings.scancondpar.Data.cone_scan_posi_a;
                myB = CTSettings.scancondpar.Data.cone_scan_posi_b;
            }
            //手動時：１次元
            else
            {
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
				//        myA = scancondpar.scan_posi_a(2)
				//        myB = scancondpar.scan_posi_b(2)
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
			}

            //Rev26.00 他クラスからも参照できるように add by chouno 2017/01/07
            ScanCorrect.myA = myA;
            ScanCorrect.myB = myB;

            //線の設定
            myLine.x1 = 0;
            //2014/11/07hata キャストの修正
            //myLine.y1 = Convert.ToInt16(modLibrary.CorrectInRange(((CTSettings.detectorParam.v_size - 1) / 2 - myA * (CTSettings.detectorParam.h_size - 1) / 2 + myB) / CTSettings.detectorParam.pvm, 0, ctlTransImage.Height - 1));
            myLine.y1 = Convert.ToInt16(modLibrary.CorrectInRange(Convert.ToInt32(((CTSettings.detectorParam.v_size - 1) / 2F - myA * (CTSettings.detectorParam.h_size - 1) / 2F + myB) / CTSettings.detectorParam.pvm), 0, ctlTransImage.Height - 1));

            myLine.x2 = Convert.ToInt16(ctlTransImage.Width - 1);
            //2014/11/07hata キャストの修正
            //myLine.y2 = Convert.ToInt16(modLibrary.CorrectInRange(((CTSettings.detectorParam.v_size - 1) / 2 + myA * (CTSettings.detectorParam.h_size - 1) / 2 + myB) / CTSettings.detectorParam.pvm, 0, ctlTransImage.Height - 1));
            myLine.y2 = Convert.ToInt16(modLibrary.CorrectInRange(Convert.ToInt32(((CTSettings.detectorParam.v_size - 1) / 2F + myA * (CTSettings.detectorParam.h_size - 1) / 2F + myB) / CTSettings.detectorParam.pvm), 0, ctlTransImage.Height - 1));
          
			//表示
			Update();

			//戻り値セット（成功）
			functionReturnValue = true;
			return functionReturnValue;
		}

        #endregion

        #region 英語用レイアウト調整

        //*******************************************************************************
        //機　　能： 英語用レイアウト調整
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V17.60  11/05/25 (検S1)  長野   新規作成
        //*******************************************************************************
		private void EnglishAdjustLayout()
		{
			lblSa.Visible = false;
			Label1.Visible = true;
		}

        #endregion

       
    }
}
