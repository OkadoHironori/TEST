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
    public partial class frmSaveMovie : Form
    {
        ///* ************************************************************************** */
        ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
        ///* 客先　　　　： ?????? 殿                                                   */
        ///* プログラム名： frmSaveMovie.frm                                            */
        ///* 処理概要　　： ??????????????????????????????                              */
        ///* 注意事項　　： なし                                                        */
        ///* -------------------------------------------------------------------------- */
        ///* 適用計算機　： DOS/V PC                                                    */
        ///* ＯＳ　　　　： Windows 2000  (SP4)                                         */
        ///* コンパイラ　： VB 6.0                                                      */
        ///* -------------------------------------------------------------------------- */
        ///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
        ///*                                                                            */
        ///* V1.00       99/XX/XX    (TOSFEC) ????????   新規作成                       */
        ///* V17.30      01/09/24    (検S1)やまおか      MIL9対応(ActiveMILを使わない)  */
        ///*                                                                            */
        ///* -------------------------------------------------------------------------- */
        ///* ご注意：                                                                   */
        ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
        ///*                                                                            */
        ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
        ///* ************************************************************************** */
        

        #region メンバ変数
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmSaveMovie myForm = null;
        #endregion

        //使っていない
        //動画保存用配列   
        //object[] UserArry;

        //動画保存関連定数                           'v11.3追加 by 間々田 2006/01/27
        public enum MovieConstants
        {
            IsMovieStoping,
            //停止状態
            IsMovieReady,
            //メモリ確保状態
            IsMovieSaveing,
            //動画取込み状態
            IsMovieSaved,
            //動画取込み終了
            IsMovieToFile,
            //動画保存状態
            IsFirstDone
            //ロード時用 //Rev20.00 追加 by長野 2014/12/04
        }

        //動画保存ステータス
        MovieConstants myMovieStatus;

        //Rev22.00 追加 by長野 2015/07/02
        double dTime2 { get; set; }
        bool interruptFlg = true;

        //動画取り込み枚数
        int MovieCount;
        //取り込み予定枚数                   'v17.30追加 byやまおか
        int MovieSaveNum;
        //動画保存開始時間                   'v17.30追加 byやまおか
        //object StartTime;
        DateTime StartTime;
        
        //Rev22.00 追加 by長野 2015/07/02
        int modX = 0;
        int modY = 0;
  
        //動画作成に一時使用する8bit画像配列 'v17.30追加 byやまおか
        byte[] tmpWordImage;


        public RadioButton[] optFrameRate = null;
       
        ////透視画像フォームへの参照
        private frmTransImage withEventsField_myTransImage;
        public frmTransImage myTransImage
        {
            get { return withEventsField_myTransImage; }
            set
            {
                if (withEventsField_myTransImage != null)
                {
                    withEventsField_myTransImage.CaptureOnOffChanged -= myTransImage_CaptureOnOffChanged;
                    withEventsField_myTransImage.TransImageChanged -= myTransImage_TransImageChanged;
                }
                withEventsField_myTransImage = value;
                if (withEventsField_myTransImage != null)
                {
                    withEventsField_myTransImage.CaptureOnOffChanged += myTransImage_CaptureOnOffChanged;
                    withEventsField_myTransImage.TransImageChanged += myTransImage_TransImageChanged;
                }
            }
        }
        

        #region サポートしているイベント
        /// <summary>
        /// サポートしているイベント
        /// </summary>
        public event UnloadedEventHandler Unloaded;
        public delegate void UnloadedEventHandler();        //アンロードされた
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmSaveMovie()
        {
            InitializeComponent();
            optFrameRate = new RadioButton[] {null, _optFrameRate_1, _optFrameRate_2 };

        }
        #endregion


        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmSaveMovie Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmSaveMovie();
                }

                return myForm;
            }
        }
        #endregion    

        //*******************************************************************************
        //機　　能： MovieStatus プロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  06/01/31   (SI3)間々田    新規作成
        //*******************************************************************************

        private MovieConstants MovieStatus
        {


            get { return myMovieStatus; }
            set
            {

                if (myMovieStatus == value)
                    return;

                //値を保持
                myMovieStatus = value;

                switch (value)
                {

                    //メモリ確保状態
                    case MovieConstants.IsMovieReady:
                        //(StatusBar1.Panels("Status").Text = "メモリ確保中"
                        tslblStatus.Text = CTResources.LoadResString(20062);                        //ストリングテーブル化 'v17.60 by長野 2011/05/22
                        tspgbSaveMovie.Value = 0;                           //プログレスバー：リセット
                        //pgbSaveMovie.Max = UBound(UserArry)               'プログレスバー：取り込み用配列確保数をカウント
                        tspgbSaveMovie.Maximum = MovieSaveNum;              //プログレスバー：取り込み用配列確保数をカウント 'v17.30変更 MIL9対応 byやまおか 2010/09/24
                        tspgbSaveMovie.Visible = true;                      //プログレスバーを表示

                        Cursor = System.Windows.Forms.Cursors.AppStarting;  //マウスポインタを矢印つき砂時計にする
                        cmdStart.Text = CTResources.LoadResString(StringTable.IDS_btnStop); //キャプションを「中止」にする
                        cmdClose.Enabled = false;                           //「閉じる」ボタンを使用不可にする
                        optFrameRate[1].Enabled = false;                    //フレームレートオプションボタン(30Hz)を使用不可にする
                        optFrameRate[2].Enabled = false;                    //フレームレートオプションボタン(15Hz)を使用不可にする
                        cwneSaveTime.Enabled = false;                       //動画保存時間欄を使用不可にする
                        break;

                    //画像（動画）取り込み状態
                    case MovieConstants.IsMovieSaveing:
                        //StatusBar1.Panels("Status").Text = "取込み中"
                        tslblStatus.Text =CTResources.LoadResString(20063); //ストリングテーブル化 'v17.60 by長野 2011/05/22
                        tspgbSaveMovie.Value = 0;                                               //プログレスバー：リセット
                        tspgbSaveMovie.Maximum = Convert.ToInt32(cwneSaveTime.Value);           //プログレスバー：秒数をカウント
                        break;

                    //画像（動画）取り込み完了状態
                    case MovieConstants.IsMovieSaved:
                        tslblStatus.Text = "";          //メッセージラベルを消去
                        tspgbSaveMovie.Visible = false;                     //プログレスバーを消去
                        cmdStart.Enabled = false;                           //「中止」ボタンを使用不可にする
                        break;

                    //画像（動画）ファイル保存状態
                    case MovieConstants.IsMovieToFile:
                        //StatusBar1.Panels("Status").Text = "ファイルに保存中..."
                        tslblStatus.Text = CTResources.LoadResString(20064);        //ストリングテーブル化 'v17.60 by長野 2011/5/22
                        tspgbSaveMovie.Value = 0;                           //プログレスバー：リセット       'v17.30追加 byやまおか 2010/09/24
                        tspgbSaveMovie.Maximum = MovieCount;                //プログレスバー：枚数をカウント 'v17.30追加 byやまおか 2010/09/24
                        tspgbSaveMovie.Visible = true;                      //プログレスバーを表示           'v17.30追加 byやまおか 2010/09/24
                        break;

                    //停止状態
                    case MovieConstants.IsMovieStoping:
                        tslblStatus.Text = "";                              //メッセージラベルを消去
                        tspgbSaveMovie.Visible = false;                                         //プログレスバーを消去

                        Cursor = System.Windows.Forms.Cursors.Default;                          //マウスポインタを元に戻す
                        cmdStart.Text = CTResources.LoadResString(StringTable.IDS_btnStart);    //キャプションを「開始」にする
                        cmdStart.Enabled = true;                                                //「開始」ボタンを使用可にする
                        cmdClose.Enabled = true;                                                //「閉じる」ボタンを使用可にする
                        optFrameRate[1].Enabled = !(CTSettings.scanParam.AverageOn | CTSettings.scanParam.SharpOn); //フレームレートオプションボタン(30Hz)を使用可にする
                        optFrameRate[2].Enabled = true;     //フレームレートオプションボタン(15Hz)を使用可にする
                        cwneSaveTime.Enabled = true;        //動画保存時間欄を使用可にする
                        break;
                    //Rev20.00 初回起動用を追加 by長野 2014/12/04
                    case MovieConstants.IsFirstDone:
                        break;
                }
            }
        }

        //使っていない
        ////*******************************************************************************
        ////機　　能： 動画保存使用時のバッファの開放
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： なし
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        ////*******************************************************************************
        //private void ClearUserArry(int StartIndex = 0)
        //{
            
        //    int i = 0;
        //    for (i = StartIndex; i <=  UserArry.GetUpperBound(0); i++) {
        //        if ((UserArry[i] != null)) {
        //            //if (UserArry[i].IsAllocated)
        //            //	UserArry[i].Free();
        //            UserArry[i] = null;
        //        }
        //    }

        //    if (StartIndex > 0) {
        //        Array.Resize(ref UserArry, StartIndex);
        //    }
            
        //}

        //*******************************************************************************
        //機　　能： 取り込み完了時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************
		private void myTransImage_CaptureOnOffChanged(bool IsOn)
		{
            
			//動画保存オンの場合
			string FileName = null;
			if ((!IsOn) & ((myMovieStatus == MovieConstants.IsMovieSaveing) | (myMovieStatus == MovieConstants.IsMovieSaved))) {

				//取り込み完了
				MovieStatus = MovieConstants.IsMovieSaved;

				//動画が取り込まれている場合 v11.3追加 by 間々田 2006/01/27
				if (MovieCount > 0) {

					if (AppValue.IsTestMode) {
						//'StatusBar1.Panels("Status").Text = "取り込み枚数 " & CStr(MovieCount) & "/" & CStr(UBound(UserArry) + 1)
						//StatusBar1.Panels("Status").Text = "取り込み枚数 " & CStr(MovieCount) & "/" & CStr(MovieSaveNum + 1)    'v17.30変更 MIL9対応 byやまおか 2010/09/24
                        tslblStatus.Text = CTResources.LoadResString(20065) + Convert.ToString(MovieCount) + "/" + Convert.ToString(MovieSaveNum + 1);	//ストリングテーブル化 'v17.60 2011/05/22
					}

					//値を保持したままReDimする
					//ClearUserArry MovieCount   'v17.30削除 MIL9対応 byやまおか 2010/09/24

					//メカの動作を強制的に止める
					frmMechaControl.Instance.SendOffToSeq();					//v14.14追加 by 間々田 2008/02/20
                    frmMechaControl.Instance.SendOffToMecha();					//v14.14追加 by 間々田 2008/02/20


					//保存ダイアログ表示
					//FileName = GetFileName(IDS_Save, "AVI動画ファイル", ".avi")
					//FileName = modFileIO.GetFileName(StringTable.IDS_Save, CTResources.LoadResString(20066), ".avi");                    //ストリングテーブル化 'v17.60 by長野 2011/05/22
                    //Rev22.00 変更 by長野 2015/07/02
                    FileName = modFileIO.GetFileName(StringTable.IDS_Save, CTResources.LoadResString(12597), ".avi");                    //ストリングテーブル化 'v17.60 by長野 2011/05/22
                    int DlgFilterIndex = modFileIO.DlgFilterIndex;
					if (!string.IsNullOrEmpty(FileName)) {

						//マウスポインタを砂時計にする
						System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

						//ファイルに保存中の表示
						MovieStatus = MovieConstants.IsMovieToFile;

						//これまでの処理を確実に反映させる
						Application.DoEvents();

						//保存処理
						//SaveMovie(FileName, MovieCount / CTSettings.scanParam.MovieSaveTime);
                        //Rev20.00 計算を変更 by長野 2014/10/04
                        //SaveMovie(FileName, (float)MovieCount / (float)CTSettings.scanParam.MovieSaveTime);
                        //SaveMovie(FileName, (float)MovieCount / (float)CTSettings.scanParam.MovieSaveTime, DlgFilterIndex);

                        //Rev22.00 変更 by長野 2015/08/18
                        if (interruptFlg == true)
                        {
                            SaveMovie(FileName, (float)MovieCount / dTime2, DlgFilterIndex);
                        }
                        else
                        {
                            SaveMovie(FileName, (float)MovieCount / (float)CTSettings.scanParam.MovieSaveTime, DlgFilterIndex);
                        }
						//マウスポインタを元に戻す
						System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

					}

					//確保した領域を開放する     'v17.30追加 MIL9対応 byやまおか 2010/09/24
					ScanCorrect.Mil9ClearUserArry();

				}

				//動画保存状態を停止状態にする
				MovieStatus = MovieConstants.IsMovieStoping;

				#if !NoCamera

				//同期に戻す
				//MilSetGrabMode frmTransImage.hMil, M_SYNCHRONOUS
				//If DetType = (DetTypeII Or DetTypeHama) Then MilSetGrabMode frmTransImage.hMil, M_SYNCHRONOUS   'v17.00変更 byやまおか 2010/02/04
                    if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypeII) | (CTSettings.detectorParam.DetType == DetectorConstants.DetTypeHama))
                        Pulsar.MilSetGrabMode(Pulsar.hMil, Pulsar.M_SYNCHRONOUS);				//v17.00修正 byやまおか 2010/03/04
				
                myTransImage.FrameRate = 15;

				#endif

			}
            
		}

        //*************************************************************************************************
        //機　　能： 透視画像変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void myTransImage_TransImageChanged()
		{
            
			//動画取込み状態の時
			//object dTime = null;
            double dTime;

			if (myMovieStatus == MovieConstants.IsMovieSaveing) {

				//'初めて動画を取り込むときタイマーをリセットする
				//If MovieCount = 0 Then     'added by 山本　2005-8-29
				//    ApplicationDefaults1.Timer.Reset
				//End If
				//
				//'経過時間
				//Dim dTime   As Double
				//dTime = ApplicationDefaults1.Timer.Read

                //Rev22.00 一枚目をメモリに保存する直前でスタートの時間を記憶するように変更 by長野 2015/08/18
                if (MovieCount == 0)
                {
                    StartTime = DateTime.Now;
                }

				//経過時間   'v17.30変更 MIL9対応 byやまおか 2010/09/24
				//dTime = DateAndTime.DateDiff(Microsoft.VisualBasic.DateInterval.Second, StartTime, DateAndTime.Now);
                dTime =(DateTime.Now - StartTime).TotalSeconds;
                
				//経過時間が保存時間を超えた
				if (dTime > CTSettings.scanParam.MovieSaveTime) {

                    //Rev22.00 追加 by長野 2015/08/17
                    interruptFlg = false;

					//取り込み完了
					//MovieStatus = IsMovieSaved
                    frmTransImage.Instance.CaptureOn = false;

					//規定の枚数を超えた場合（フレーム抜けの無い場合）
					//ElseIf MovieCount > UBound(UserArry) Then
					//ElseIf MovieCount > MovieSaveNum Then   'v17.30変更 MIL9対応 byやまおか 2010/09/24
				//v17.50変更 by 間々田 2011/02/04 １個多かった
				} else if (MovieCount >= MovieSaveNum) {

                    //Rev22.00 追加 by長野 2015/08/17
                    interruptFlg = false;

					//取り込み完了
					//MovieStatus = IsMovieSaved
                    frmTransImage.Instance.CaptureOn = false;

				} else {

					//配列にコピー
					//UserArry(MovieCount).Picture = myTransImage.ctlTransImage.GetPicture					//v17.30変更 MIL9対応 byやまおか 2010/09/24
					var _with2 = myTransImage.ctlTransImage;
                    tmpWordImage = CTSettings.transImageControl.GetByteImage();

                    if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
                    {
                        byte[] tmpWordImage2 = new byte[(_with2.SizeX - (2 * modX)) * (_with2.SizeY - (2 * modY))];

                        unsafe
                        {
                            fixed (byte* p1 = tmpWordImage)
                            {
                                fixed (byte* p2 = tmpWordImage2)
                                {
                                    long cnt = 0;
                                    long index = 0;
                                    long cntX = 0;
                                    long cntY = 0;
                                    for (cntY = modY; cntY < _with2.SizeY - modY; cntY++)
                                    {
                                        index = cntY * _with2.SizeX;
                                        for (cntX = modX; cntX < _with2.SizeX - modX; cntX++)
                                        {
                                            *(p2 + cnt) = *(p1 + index + cntX);
                                            cnt++;
                                        }
                                    }
                                }
                            }
                        }
                        Pulsar.Mil9CopyUserArry(MovieCount, tmpWordImage2, Pulsar.hMil, _with2.SizeX - (2 * modX), modCT30K.hScale, _with2.SizeY - (2 * modY), modCT30K.vScale, frmTransImage.Instance.ZoomScale); //'v18.00変更 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

                    }
                    else
                    {
                        Pulsar.Mil9CopyUserArry(MovieCount, tmpWordImage, Pulsar.hMil, _with2.SizeX - (2 * modX), modCT30K.hScale, _with2.SizeY - (2 * modY), modCT30K.vScale, frmTransImage.Instance.ZoomScale); //'v18.00変更 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    }


                    //変更2014/10/07hata_v19.51反映
                    ////Call Mil9CopyUserArry(MovieCount, tmpWordImage(0), frmTransImage.hMil, .SizeX, fphm, .SizeY, fpvm)
                    //Pulsar.Mil9CopyUserArry(MovieCount, tmpWordImage, Pulsar.hMil, _with2.SizeX, CTSettings.detectorParam.fphm, _with2.SizeY, CTSettings.detectorParam.fpvm, frmTransImage.Instance.ZoomScale); //v17.65 縮小動画対応 by長野 2011.11.30
                    //Pulsar.Mil9CopyUserArry(MovieCount, tmpWordImage, Pulsar.hMil, _with2.SizeX, modCT30K.hScale, _with2.SizeY, modCT30K.vScale, frmTransImage.Instance.ZoomScale); //'v18.00変更 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    
                    //Rev22.00 変更 by長野 2015/07/02
                    //Pulsar.Mil9CopyUserArry(MovieCount, tmpWordImage, Pulsar.hMil, _with2.SizeX, modCT30K.hScale, _with2.SizeY, modCT30K.vScale, frmTransImage.Instance.ZoomScale); //'v18.00変更 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    //Pulsar.Mil9CopyUserArry(MovieCount, tmpWordImage2, Pulsar.hMil, _with2.SizeX - (2*modX), modCT30K.hScale, _with2.SizeY - (2*modY), modCT30K.vScale, frmTransImage.Instance.ZoomScale); //'v18.00変更 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

					MovieCount = MovieCount + 1;
					
                    //Rev22.0 test by長野
                    //途中停止時に、最後のフレーム保存と時間をそろえるためにここでもdTimeを更新する。
                    dTime2 = (DateTime.Now - StartTime).TotalSeconds;
                    
                    tspgbSaveMovie.Value = Convert.ToInt32(dTime);					//プログレスバーを更新

				}

			}
            
		}
   
        //*******************************************************************************
        //機　　能： 動画保存用にメモリ確保する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  2005-8-10   山本      新規作成
        //*******************************************************************************
        private bool MovieSaveInit(int theSaveTime, float theFrameRate)
        {
            bool functionReturnValue = false;

            //int i = 0;
            int ret = 0;            //v17.30追加 MIL9対応 byやまおか 2010/09/24
            string Errtext = "";

            //戻り値初期化
            functionReturnValue = false;

            var _with3 = myTransImage.ctlTransImage;

            //Rev22.00 追加 by長野 2015/07/02
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
            {
                if (_with3.SizeX == 2048)
                {
                    modX = 24;
                    modY = 24;
                }
                else if (_with3.SizeX == 1024)
                {
                    modX = 12;
                    modY = 12;
                }
            }

            //エラー時の扱い
            try
            {
                //動画保存時間
                CTSettings.scanParam.MovieSaveTime = theSaveTime;

                //'取り込み予定枚数分配列を準備
                //Dim MovieSaveNum As Long   'v17.30削除 MIL9対応 byやまおか 2010/09/24
                MovieSaveNum = Convert.ToInt32(theSaveTime * theFrameRate);

                //'取り込み予定枚数分配列を準備
                //ClearUserArry MovieSaveNum 'v17.30削除 MIL9対応 byやまおか 2010/09/24

                //動画保存メモリ確保状態へ
                MovieStatus = MovieConstants.IsMovieReady;

                //For i = LBound(UserArry) To UBound(UserArry)
                //
                //    'プログレスバーの更新
                //    pgbSaveMovie.Value = i
                //    DoEvents
                //
                //    '途中で中断された場合
                //    If myMovieStatus = IsMovieStoping Then Exit Function
                //
                //    'コントロールオブジェクトが存在しない
                //    If UserArry(i) Is Nothing Then
                //
                //        'ユーザー用配列を用意
                //        Set UserArry(i) = ApplicationDefaults1.CreateObject("MIL.Image", False)
                //
                //        With UserArry(i)
                //            '.OwnerSystem = System1
                //            .CanProcess = True
                //            .CanGrab = False
                //            .CanDisplay = False
                //            .Location = imOffBoard 'Host側に確保
                //            .Allocate
                //        End With
                //
                //    End If
                //
                //Next

                //v17.30変更 MIL9対応(ここから) byやまおか 2010/09/24
                //プログレスバーを更新(以下の関数では進捗がわからないため、少し進める。)
                //2014/11/07hata キャストの修正
                //tspgbSaveMovie.Value = MovieSaveNum / 4;  //1/4進める
                tspgbSaveMovie.Value = Convert.ToInt32(MovieSaveNum / 4F);  //1/4進める

                //Rev22.00 移動 コメント追加に使用するので動画初期化のタイミングではなくなったため by長野 2015/07/03
                ////ホストメモリをオープンする
                //if (Pulsar.hMil == IntPtr.Zero)
                //{
                //    //frmTransImage.hMil = MilHostOpen(IMAGE_BIT8)
                //    Pulsar.hMil = Pulsar.MilHostOpen(_with3.SizeX, _with3.SizeY);
                //    //v17.50変更 by 間々田 2011/01/20
                //}
            }
            catch (Exception exp)
            {
                Errtext = exp.Message;
                goto ErrorHandler;
            }

            try
            {
                //取り込み予定枚数分の配列を準備する

                //変更2014/10/07hata_v19.51反映
                ////ret = Mil9AllocUserArry(MovieSaveNum, frmTransImage.hMil, .SizeX, fphm, .SizeY, fpvm)
                //ret = Pulsar.Mil9AllocUserArry(MovieSaveNum, Pulsar.hMil, _with3.SizeX, CTSettings.detectorParam.fphm, _with3.SizeY, CTSettings.detectorParam.fpvm, frmTransImage.Instance.ZoomScale);      //v17.65 縮小動画対応 by 長野 2011.11.30
                //ret = Pulsar.Mil9AllocUserArry(MovieSaveNum, Pulsar.hMil, _with3.SizeX, modCT30K.hScale, _with3.SizeY, modCT30K.vScale, frmTransImage.Instance.ZoomScale);  //'v18.00変更 byやまおか 2011/07/09  'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                //Rev22.00 変更 by長野 2015/07/02
                ret = Pulsar.Mil9AllocUserArry(MovieSaveNum, Pulsar.hMil, _with3.SizeX - (2*modX), modCT30K.hScale, _with3.SizeY - (2*modY), modCT30K.vScale, frmTransImage.Instance.ZoomScale);  //'v18.00変更 byやまおか 2011/07/09  'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

            }
            catch (Exception exp)
            {
                Errtext = exp.Message;            
            }

            //予定枚数分確保できなかった場合
            if ((ret != MovieSaveNum))
            {

                //MsgBox CStr(theSaveTime) & "秒間撮影するメモリを確保できませんでした。" & vbCrLf & _
                //CStr(theSaveTime * theFrameRate) & "秒間だけ動画撮影します。", vbInformation
                //            MsgBox CStr(theSaveTime) & "秒間撮影するメモリを確保できませんでした。" & vbCrLf & _
                //'                   CStr(Format(ret / theFrameRate, 0)) & "秒間だけ動画撮影します。", vbInformation     'v17.40修正 byやまおか 2010/10/26
                //ストリングテーブル化 'v17.60 by長野 2011/5/22
                //Interaction.MsgBox(StringTable.GetResString(Convert.ToInt32(CT30K.My.Resources.str20067), Convert.ToString(theSaveTime)) + 
                //                                            Constants.vbCrLf + 
                //                                            StringTable.GetResString(Convert.ToInt32(CT30K.My.Resources.str20068),Convert.ToString(Microsoft.VisualBasic.Compatibility.VB6.Support.Format(ret / theFrameRate, Convert.ToString(0)))), 
                //                                            MsgBoxStyle.Information);
                MessageBox.Show(StringTable.GetResString(20067, Convert.ToString(theSaveTime)) + "\r\n" + 
                                StringTable.GetResString(20068 ,(ret / theFrameRate).ToString("0")),
                                Application.ProductName, 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Information);


                //保存時間を更新する
                theSaveTime = Convert.ToInt32(ret / theFrameRate);  //v17.40追加 byやまおか 2010/10/26

            }
            else if ((ret <= 0))
            {
                //配列準備に失敗したら

                //領域を開放する
                Pulsar.Mil9ClearUserArry();

                //プログレスバーを更新(クリアーする)
                tspgbSaveMovie.Value = 0;

                goto ErrorHandler;

            }

            //プログレスバーを更新(全部進める)
            tspgbSaveMovie.Value = MovieSaveNum;            //v17.30変更 MIL9対応(ここまで) byやまおか 2010/09/24

            //動画保存カウントクリア
            MovieCount = 0;

            //戻り値セット
            functionReturnValue = true;
            return functionReturnValue;
 
ErrorHandler:

            //メッセージの表示
            //MsgBox "動画保存用のメモリ確保中にエラーが発生しました。" & vbCr & vbCr & Err.Description, vbCritical
            //Interaction.MsgBox(CT30K.My.Resources.str20069 + Constants.vbCr + Constants.vbCr + Err().Description, MsgBoxStyle.Critical);

            MessageBox.Show(CTResources.LoadResString(20069) + "\r\n" + "\r\n" + Errtext,
                            Application.ProductName,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

            
            return functionReturnValue;
            //ストリングテーブル化 'v17.60 by長野 2011/05/22

        }

        //*******************************************************************************
        //機　　能： 動画をファイルに保存する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  06/02/23   (SI3)間々田    新規作成
        //*******************************************************************************
        //private bool SaveMovie(string FileName, double FrameRate)
        private bool SaveMovie(string FileName, double FrameRate, int Index) //Rev22.00 引数追加 by長野 2015/07/02
        {
            bool functionReturnValue = false;

            //int ret = 0;    //v17.30追加 MIL9対応 byやまおか 2010/09/24

            //戻り値初期化
            functionReturnValue = false;

            //AVIHandlerの生成
            //Dim AVIHandler As New AVIFileHandler       'v17.50削除 未使用 2010/12/28 by 間々田

            //エラー時の扱い
            try
            {

                //'作業用ピクチャオブジェクトを生成
                //Dim thePicture As Picture
                //With frmTransImage.ctlTransImage
                //    Set thePicture = CreatePicture(Me.hdc, .width, .Height)
                //End With
                //
                //'取り込んだPictureを縦方向に拡大する
                //Dim i As Integer
                //For i = LBound(UserArry) To UBound(UserArry)
                //    CopyPicture UserArry(i).Picture, thePicture
                //    UserArry(i).Picture = thePicture
                //    DoEvents
                //Next
                //
                //With AVIHandler
                //    .Open FileName, aviFileFormatDefault, aviCreate
                //    .Write UserArry, FrameRate
                //    .Close
                //End With

                //v17.30変更 MIL9対応(ここから) byやまおか 2010/09/24
                //プログレスバーを更新
                //(今の関数では進捗がわからないため、見た目でごまかす。)
                //(時間があるときにコールバックで進められるように改造する。)
                //2014/11/07hata キャストの修正
                //tspgbSaveMovie.Value = MovieCount / 4;    //1/4進める
                tspgbSaveMovie.Value = Convert.ToInt32(MovieCount / 4F);    //1/4進める

                //取り込んだデータを動画ファイルに保存する
                //ScanCorrect.Mil9SaveMovie(MovieCount, FileName, FrameRate);
                ScanCorrect.Mil9SaveMovie(MovieCount, FileName,Index,FrameRate); //Rev22.00 引数追加 by長野 2015/07/02

                //プログレスバーを更新(全部進める。)
                //2014/11/07hata キャストの修正
                //tspgbSaveMovie.Value = MovieCount * 4 / 5;    //4/5進める
                tspgbSaveMovie.Value = Convert.ToInt32(MovieCount * 4F / 5F);    //4/5進める
                modCT30K.PauseForDoEvents(1);
                tspgbSaveMovie.Value = MovieCount;    //全部進める
                //v17.30変更 MIL9対応(ここまで) byやまおか 2010/09/24

                //戻り値セット（成功）
                functionReturnValue = true;
         
            }
            catch
            {
                //（失敗）
            }
    
            //ExitHandler:

            //Set thePicture = Nothing

            //Rev22.00 コメントアウト by長野 2015/08/17
            //Pulsar.Mil9ClearUserArry();
            
            return functionReturnValue;            //v17.30変更 MIL9対応 byやまおか 2010/09/24

        }

        //*******************************************************************************
        //機　　能： 「閉じる」ボタンクリック処理
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
            //フォームのアンロード
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
        private void frmSaveMovie_Load(object sender, EventArgs e)
        {
            //実行時はフラグをセット
            modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTSaveMovie;

            //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);

            //アベレージング・シャープ実行中は動画保存の30Hzは選択できない
            if (CTSettings.scanParam.AverageOn | CTSettings.scanParam.SharpOn)
            {
                CTSettings.scanParam.gFrameRateIndex = 2;
            }
            //Rev20.00 PkeFPDの場合はインデックス1にする。
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
            {
                CTSettings.scanParam.gFrameRateIndex = 1;
            }

            //'フレームレートオプションボタンのキャプションの設定     'v11.3追加 by 間々田 2006/01/31
            //optFrameRate(1).Caption = Format$(FR(0), "0Hz")         '元の値
            //optFrameRate(2).Caption = Format$(FR(0) / 2, "0Hz")     '1/2の値
            //SetOption optFrameRate, gFrameRateIndex

            //フレームレートオプションボタンのキャプションの設定   'v17.30変更 byやまおか 2010/09/24
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
            {
                //PkeFPDの場合は、FPD積分時間に依存する
                optFrameRate[1].Text = frmTransImage.Instance.GetCurrentFR().ToString("0.0Hz");
                optFrameRate[1].Checked = true;
                optFrameRate[2].Visible = false;
                //2014/11/07hata キャストの修正
                //optFrameRate[1].Top = fraFrameRate.Parent.Height / 4;                //均等配置のための調整
                optFrameRate[1].Top = Convert.ToInt32(fraFrameRate.Parent.Height / 4F);                //均等配置のための調整

            }
            else
            {
                //谷口CT305Bの場合は、非同期30Hz/同期15Hzの切り替えを可能にする
                //optFrameRate[1 - 1].Text = Microsoft.VisualBasic.Compatibility.VB6.Support.Format(modCT30K.FR[0], (modCT30K.FR[0] > 10 ? "0Hz" : "0.0Hz"));   //元の値
                //optFrameRate[2 - 1].Text = Microsoft.VisualBasic.Compatibility.VB6.Support.Format(modCT30K.FR[0] / 2, (modCT30K.FR[0] / 2 > 10 ? "0Hz" : "0.0Hz")); //1/2の値
                optFrameRate[1].Text = CTSettings.detectorParam.FR[0].ToString(CTSettings.detectorParam.FR[0] > 10 ? "0Hz" : "0.0Hz");  //元の値
                optFrameRate[2].Text = (CTSettings.detectorParam.FR[0] / 2).ToString(CTSettings.detectorParam.FR[0] / 2 > 10 ? "0Hz" : "0.0Hz"); //1/2の値
            }
            modLibrary.SetOption(optFrameRate, CTSettings.scanParam.gFrameRateIndex);

            //動画保存時間
            //変更2015/02/02hata_Max/Min範囲のチェック
            //cwneSaveTime.Value = CTSettings.scanParam.MovieSaveTime;
            cwneSaveTime.Value = modLibrary.CorrectInRange(CTSettings.scanParam.MovieSaveTime, cwneSaveTime.Minimum, cwneSaveTime.Maximum);
            
            //アベレージング・シャープ実行中は動画保存の30Hzは選択できない       'v11.3追加 by 間々田 2006/02/21
            optFrameRate[1].Enabled = (!CTSettings.scanParam.AverageOn) & (!CTSettings.scanParam.SharpOn);

            //動画保存時のバッファの初期化
            //ReDim UserArry(0)  'v17.30削除 MIL9対応 byやまおか 2010/09/24

            //透視画像フォームへの参照
            myTransImage = frmTransImage.Instance;

            //キャプチャ画像用の一時配列を準備(動画保存用)
            tmpWordImage = new byte[frmTransImage.Instance.ctlTransImage.SizeX * frmTransImage.Instance.ctlTransImage.SizeY];

            //動画保存ステータスの初期化
            //myMovieStatus = MovieConstants.IsMovieStoping;
            //Rev20.00 追加 by長野 2014/12/04
            //this.myMovieStatus = MovieConstants.IsMovieStoping;
            //myMovieStatus = MovieConstants.IsMovieStoping;
            MovieStatus = MovieConstants.IsFirstDone;
            MovieStatus = MovieConstants.IsMovieStoping;
            int optFrameIndex = modLibrary.GetOption(optFrameRate);
            optFrameRate[optFrameIndex].Checked = true;
            optFrameRate_CheckedChanged(optFrameRate, new System.EventArgs());
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
        private void frmSaveMovie_FormClosed(object sender, FormClosedEventArgs e)
        {
            //動画保存使用時のバッファの開放
            //ClearUserArry
            //'v17.30変更 MIL9対応 byやまおか 2010/09/24 'v17.40削除 以前に解放している byやまおか 2010/10/26
            //With myTransImage.ctlTransImage
            //    Call Mil9ClearUserArry
            //End With
            tmpWordImage = new byte[1];

            //透視画像フォームへの参照
            myTransImage = null;

            //終了時はフラグをリセット
            //modCTBusy.CTBusy = modCTBusy.CTBusy & (!modCTBusy.CTSaveMovie);
            modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTSaveMovie);

            //イベント生成（アンロードされた）
            if (Unloaded != null)
            {
                Unloaded();
            }
        }

        //*******************************************************************************
        //機　　能： フォームリサイズ時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void frmSaveMovie_Resize(object sender, EventArgs e)
        {
            //不要
            ////プログレスバーの位置・サイズ調整
            //var _with4 = pgbSaveMovie;
             //_with4.SetBounds(StatusBar1.Items["Progress"].Left, StatusBar1.Top + StatusBar1.Height / 2 - _with4.Height / 2, Microsoft.VisualBasic.Compatibility.VB6.Support.PixelsToTwipsX(StatusBar1.Items["Progress"].Width) - 2, 0, System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y | System.Windows.Forms.BoundsSpecified.Width);
            //_with4.BringToFront();

        }

        //*******************************************************************************
        //機　　能： 動画保存フレーム内「開始」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  06/01/31   (SI3)間々田    新規作成
        //*******************************************************************************
        private void cmdStart_Click(object sender, EventArgs e)
        {
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            //    'RAMディスクが構築されているかどうか  'v17.40追加 byやまおか 2010/10/26
            //    'If UseRamDisk And (Not RamDiskIsReady) Then Exit Sub
            //    If UseRamDisk Then      'v17.42修正 byやまおか 2010/11/04
            //        If (Not RamDiskIsReady) Then Exit Sub
            //    End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //ラジオボタンから"Hz"を取った数字を取得する
            int iPos = optFrameRate[CTSettings.scanParam.gFrameRateIndex].Text.IndexOf("Hz");
            string strRate = optFrameRate[CTSettings.scanParam.gFrameRateIndex].Text.Remove(iPos);

            float fRate = Convert.ToSingle(strRate); ;
            
            //動画保存停止状態でなければ中止処理
            if (MovieStatus != MovieConstants.IsMovieStoping)
            {

                //領域を開放する     'v17.30追加 MIL9対応 byやまおか 2010/09/24
                var _with5 = myTransImage.ctlTransImage;
                
                //Rev22.00 test by長野 2015/07/02
                //動画が取り込まれている場合 v11.3追加 by 間々田 2006/01/27
                if (MovieCount > 0)
                {
                    //Rev22.00 //カメラ取り込み停止
                    if (frmTransImage.Instance.CaptureOn == true)
                    {
                        frmTransImage.Instance.CaptureOn = false;
                    }
                }

                //    //動画保存停止状態へ
                //    MovieStatus = MovieConstants.IsMovieStoping;

                //    //メカの動作を強制的に止める
                //    frmMechaControl.Instance.SendOffToSeq();					//v14.14追加 by 間々田 2008/02/20
                //    frmMechaControl.Instance.SendOffToMecha();					//v14.14追加 by 間々田 2008/02/20


                //    //保存ダイアログ表示
                //    string FileName;
                //    //FileName = GetFileName(IDS_Save, "AVI動画ファイル", ".avi")
                //    //FileName = modFileIO.GetFileName(StringTable.IDS_Save, CTResources.LoadResString(20066), ".avi");                    //ストリングテーブル化 'v17.60 by長野 2011/05/22
                //    //Rev22.00 変更 by長野 2015/07/02
                //    FileName = modFileIO.GetFileName(StringTable.IDS_Save, CTResources.LoadResString(12597), ".avi");                    //ストリングテーブル化 'v17.60 by長野 2011/05/22
                //    int DlgFilterIndex = modFileIO.DlgFilterIndex; //Rev22.00 追加 by長野 2015/07/02

                //    if (!string.IsNullOrEmpty(FileName))
                //    {

                //        //マウスポインタを砂時計にする
                //        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                //        //ファイルに保存中の表示
                //        MovieStatus = MovieConstants.IsMovieToFile;

                //        //これまでの処理を確実に反映させる
                //        Application.DoEvents();

                //        //保存処理
                //        //SaveMovie(FileName, MovieCount / CTSettings.scanParam.MovieSaveTime);
                //        //Rev20.00 計算を変更 by長野 2014/10/04
                //        //SaveMovie(FileName, (float)MovieCount / (float)dTime2,Index);
                //        //Rev22.00 引数追加 by長野 2015/07/02
                //        SaveMovie(FileName, (float)MovieCount / (float)dTime2, DlgFilterIndex);

                //        //マウスポインタを元に戻す
                //        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                //    }
                //}
                
                //Pulsar.Mil9ClearUserArry();

                //動画保存停止状態へ
                MovieStatus = MovieConstants.IsMovieStoping;
                
            //メモリ確保
            }
            //else if (MovieSaveInit(cwneSaveTime.Value, Conversion.Val(optFrameRate[CTSettings.scanParam.gFrameRateIndex - 1].Text)))
            else if (MovieSaveInit(Convert.ToInt32(cwneSaveTime.Value), fRate))
            {
                //フレームレートの設定
                myTransImage.FrameRate = Convert.ToInt32(fRate);

                //動画保存開始状態へ
                //MovieStatus = MovieConstants.IsMovieSaveing;

                //Rev22.00 追加 by長野 2015/08/18
                interruptFlg = true;

                //カメラ取り込み開始
                //Rev20.00 追加 by長野 2014/12/04
                if (frmTransImage.Instance.CaptureOn == false)
                {
                    frmTransImage.Instance.CaptureOn = true;
                }

                //Rev20.00 他の処理に時間を与える。
                //すぐ動画保存開始すると、最初の数十フレームに抜けが発生してしまう by長野 2014/12/04
                modCT30K.PauseForDoEvents(5);

                //Rev20.00 移動 by長野 2014/12/04
                //動画保存開始状態へ
                MovieStatus = MovieConstants.IsMovieSaveing;

                //開始時間を記録 'v17.30追加 MIL9対応 byやまおか 2010/09/24
                //StartTime = DateTime.Now;

                ////Rev20.00 移動 by長野 2014/12/04
                ////動画保存開始状態へ
                //MovieStatus = MovieConstants.IsMovieSaveing;


            }

        }
        //*******************************************************************************
        //機　　能： 動画保存フレーム内「フレームレート」オプションボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  06/01/31   (SI3)間々田    新規作成
        //*******************************************************************************
        private void optFrameRate_CheckedChanged(object sender, EventArgs e)
        {
            //Rev20.00 修正 by長野 2014/12/04
            //if (sender as CWButton == null) return;
            if (sender as RadioButton == null) return;

            int Index = Array.IndexOf(optFrameRate, sender);
            if (Index <= 0) return;
  
            //動画保存に費やすメモリの最大値（MB）
            //Const AvailableMemorySize As Long = 622
            //const int AvailableMemorySize = 1024;            //[MB]    'v17.30変更 byやまおか 2010/09/25
            long AvailableMemorySize = (long)CTSettings.iniValue.MovieMemSize;             //[MB]    'v22.00変更 by長野 2015/07/02

            //最大保存時間（秒）の計算
            //With frmFPulsar.ImageBuffer
            var _with6 = frmTransImage.Instance.ctlTransImage;

            //ラジオボタンから"Hz"を取った数字を取得する
            int iPos = optFrameRate[Index].Text.IndexOf("Hz");
            string strRate = optFrameRate[Index].Text.Remove(iPos);
            float fRate = Convert.ToSingle(strRate); ;

            ////cwneSaveTime.Maximum = Fix(AvailableMemorySize * CLng(1024) * CLng(1024) / (.SizeX * .SizeY * Val(optFrameRate(Index).Caption)))
            //cwneSaveTime.Maximum = Conversion.Fix(AvailableMemorySize * 1024 * 1024 / (_with6.Width * _with6.Height *　fRate));
            cwneSaveTime.Maximum = Convert.ToInt32(Math.Truncate(AvailableMemorySize * 1024 * 1024 / (_with6.Width * _with6.Height * fRate)));
                
            lblMaxSaveTime.Text = Convert.ToString(cwneSaveTime.Maximum);

            //    'デジタイザの設定：同期/非同期モードをフレームレートによって変える
            //    With frmFPulsar.Digitizer1
            //        .GrabMode = IIf(Index = 1, digAsynchronous, digSynchronous)  'Index=1 元のフレームレート Index=2 1/2のフレームレート
            //    End With

            //30Hzの場合、アベレージングとシャープ処理は選択できないようにする   'v15.0削除 ここで制御しない by 間々田 2009/06/16
            //frmScanControl.chkSharp.Enabled = (Index = 2)
            //frmScanControl.chkAverage.Enabled = (Index = 2)

            //透視画像動画保存時のフレームレートのインデックス値を記憶
            CTSettings.scanParam.gFrameRateIndex = Index;

        }

    }
}
