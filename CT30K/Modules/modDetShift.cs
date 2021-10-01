using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
 // ERROR: Not supported in C#: OptionDeclaration

using System.Threading; //Rev25.00 追加 by長野 2016/08/10

using CT30K.Common;
using CTAPI;
using TransImage;

namespace CT30K
{
	static class modDetShift
	{
        //* ************************************************************************** */
        //* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT                      */
        //* 客先　　　　： ?????? 殿                                                   */
        //* プログラム名： modDetShift.bas                                             */
        //* 処理概要　　： 検出器シフト機能関連モジュール                              */
        //* 注意事項　　： なし                                                        */
        //* -------------------------------------------------------------------------- */
        //* 適用計算機　： DOS/V PC                                                    */
        //* ＯＳ　　　　： Windows XP (SP6)                                            */
        //* コンパイラ　： VB 6.0                                                      */
        //* -------------------------------------------------------------------------- */
        //* VERSION     DATE        BY                  CHANGE/COMMENT                 */
        //*                                                                            */
        //* v18.00      11/01/26    (検S1)やまおか      新規作成                       */
        //*                                                                            */
        //* -------------------------------------------------------------------------- */
        //* ご注意：                                                                   */
        //* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
        //*                                                                            */
        //* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2011                 */
        //* ************************************************************************** */


        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************

        //'検出器切替の場合，非常停止が発生したことを示すmsgboxでＯＫをクリックしたことを示すフラグを用意
        //'通常はTrueにしておき，非常停止ボタンが押されたときだけFalseに変更
        //Public MsgBoxOK As Boolean

        //検出器の状態定数
		public enum DetShiftConstants
		{
			DetShift_origin = 0,    //基準位置		
			DetShift_forward = 1,   //正方向に移動(試料扉から見て奥)
			DetShift_backward = 2,  //負方向に移動(試料扉から見て手前)
			DetShift_none = 3       //いずれでもない
		}

		private static DetShiftConstants myDetShift;

        //検出器シフト調整用パラメータ   'v18.00追加 byやまおか 2011/03/07
		public static int Roi_Xs;   //始点 X座標
		public static int Roi_Xe;   //終点 X座標
		public static int Roi_Ys;   //始点 Y座標
		public static int Roi_Ye;   //終点 Y座標

        //ゲインデータをセットするか(検出器シフト時) 'v18.00追加 byやまおか 2011/07/04
		public const bool SET_GAIN = true;
		public const bool UNSET_GAIN = false;


        //********************************************************************************
        //  構造体宣言
        //********************************************************************************

        //探索情報構造体     'v18.xx追加 byやまかげ 2011/02/04
		public struct Search_Info
		{			
			public double Base;     //基準位置		
			public double pitch;    //探索ピッチ
			public int Range;       //探索範囲
		}


        //********************************************************************************
        //  外部関数宣言
        //********************************************************************************
        //v18.xx追加 byやまかげ 2011/02/04
        //[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int CalculateDetectorShiftAmount(ref short baseImage, ref short shiftImage, ref Size imageSize, ref modAutoPos.RECTROI roiRect, ref Search_Info searchH, ref Search_Info searchV, ref double minCorPosH, ref double minCorPosV);
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int CalculateDetectorShiftAmount(ref ushort baseImage, ref short shiftImage, ref Size imageSize, ref modAutoPos.RECTROI roiRect, ref Search_Info searchH, ref Search_Info searchV, ref double minCorPosH, ref double minCorPosV);
        public static extern int CalculateDetectorShiftAmount(ref ushort baseImage, ref ushort shiftImage, ref modAutoPos.SIZE imageSize, ref modAutoPos.RECTROI roiRect, ref Search_Info searchH, ref Search_Info searchV, ref double minCorPosH, ref double minCorPosV);

        //シーケンサ動作停止要求フラグ           'v15.0追加 by 間々田 2009/05/14 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
		static bool myRequestForStop;


        //*******************************************************************************
        //機　　能： DetShiftプロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v18.00  2011/01/31  やまおか    新規作成
        //*******************************************************************************

		public static DetShiftConstants DetShift {


			get { return myDetShift; }
			set {

				//検出器切替機能が無効の時は、常に検出器１
                //Rev25.00 Wスキャン追加 by長野 2016/06/19
                //if (!CTSettings.DetShiftOn) {
                if (!(CTSettings.DetShiftOn || CTSettings.W_ScanOn))
                {
                    myDetShift = DetShiftConstants.DetShift_origin;
					return;
				}

				myDetShift = value;

				//modMecainf.GetMecainf(ref modMecainf.mecainf);				//v18.00追加 byやまおか 2011/02/12
                CTSettings.mecainf.Load();

				var _with1 = frmMechaControl.Instance;

				MoveDetSharp(); //図を移動

                switch (value) {

					case DetShiftConstants.DetShift_origin:
						_with1.cwbtnDetShift.OnColor = System.Drawing.Color.Lime;
						//_with1.cwbtnDetShift.OffColor = System.Drawing.ColorTranslator.FromOle(modCT30K.DarkGreen);
						_with1.cwbtnDetShift.BackColor = modCT30K.DarkGreen;
                        
                        _with1.cwbtnDetShift.Value = false;
						//ボタンを消灯
						CTSettings.mecainf.Data.detsftpos_org = 1;						//基準位置にいる     'v18.00追加 byやまおか 2011/02/12
						CTSettings.mecainf.Data.detsftpos_sft = 0;						//シフト位置にいない 'v18.00追加 byやまおか 2011/02/12
						break;

					case DetShiftConstants.DetShift_forward:
						_with1.cwbtnDetShift.OnColor = System.Drawing.Color.Lime;
						//_with1.cwbtnDetShift.OffColor = System.Drawing.ColorTranslator.FromOle(modCT30K.DarkGreen);
						_with1.cwbtnDetShift.BackColor =  modCT30K.DarkGreen;
						_with1.cwbtnDetShift.Value = true;
						//ボタンを点灯
						CTSettings.mecainf.Data.detsftpos_org = 0;						//基準位置にいない   'v18.00追加 byやまおか 2011/02/12
						CTSettings.mecainf.Data.detsftpos_sft = 1;						//シフト位置にいる   'v18.00追加 byやまおか 2011/02/12
						break;

					case DetShiftConstants.DetShift_backward:
						_with1.cwbtnDetShift.OnColor = System.Drawing.Color.Lime;
						//_with1.cwbtnDetShift.OffColor = System.Drawing.ColorTranslator.FromOle(modCT30K.DarkGreen);
						_with1.cwbtnDetShift.BackColor =  modCT30K.DarkGreen;
						_with1.cwbtnDetShift.Value = true;
						//ボタンを点灯
						CTSettings.mecainf.Data.detsftpos_org = 0;						//基準位置にいない   'v18.00追加 byやまおか 2011/02/12
						CTSettings.mecainf.Data.detsftpos_sft = 1;						//シフト位置にいる   'v18.00追加 byやまおか 2011/02/12
						break;

					case DetShiftConstants.DetShift_none:
						_with1.cwbtnDetShift.OnColor = System.Drawing.Color.Yellow;
						//_with1.cwbtnDetShift.OffColor = System.Drawing.Color.Yellow;
						_with1.cwbtnDetShift.BackColor = System.Drawing.Color.Yellow;
						CTSettings.mecainf.Data.detsftpos_org = 0;						//基準位置にいない   'v18.00追加 byやまおか 2011/02/12
						CTSettings.mecainf.Data.detsftpos_sft = 0;						//シフト位置にいない 'v18.00追加 byやまおか 2011/02/12
						break;

				}

				//modMecainf.PutMecainf(ref modMecainf.mecainf);
                CTSettings.mecainf.Write();

				//透視画像上のスライスラインを更新する   'v18.00追加 byやまおか 2011/02/07
				if (modLibrary.IsExistForm(frmTransImage.Instance)) {
					frmTransImage.Instance.SetLine();
				}

				//内部変数に記憶
				myDetShift = value;

			}
		}


        //*************************************************************************************************
        //機　　能： 検出器が基準位置かシフト位置にあるかチェック
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値：                 Boolean             True:撮影位置にある   False:それ以外
        //
        //補　　足： なし
        //
        //履　　歴： v18.00 2011/02/04   やまおか        新規作成
        //*************************************************************************************************
		public static bool IsOKShiftPos {
			get {
				bool functionReturnValue = false;
				var _with2 = modSeqComm.MySeq;

                //if (CTSettings.DetShiftOn) 
                //{
                //    functionReturnValue = ((DetShift == DetShiftConstants.DetShift_origin) & (_with2.stsCTIIPos)) | 
                //                          ((DetShift == DetShiftConstants.DetShift_forward) & (_with2.stsTVIIPos)) | 
                //                          ((DetShift == DetShiftConstants.DetShift_backward) & (_with2.stsTVIIPos));

                //} 
                //else {
                //    functionReturnValue = true;
                //}
                //Rev23.10 変更 by長野 2015/10/18
                //if (!CTSettings.ChangeXrayOn && CTSettings.DetShiftOn)
                //Rev23.20 追加 by長野 2015/12/21
                //Rev25.00 Wスキャン追加 by長野 2016/06/19
                //if (!CTSettings.ChangeXrayOn && CTSettings.DetShiftOn && CTSettings.scaninh.Data.lr_sft == 1)
                if (!CTSettings.ChangeXrayOn && ((CTSettings.DetShiftOn && CTSettings.scaninh.Data.lr_sft == 1) && !CTSettings.W_ScanOn))
                {
                    functionReturnValue = ((DetShift == DetShiftConstants.DetShift_origin) & (_with2.stsCTIIPos)) |
                                          ((DetShift == DetShiftConstants.DetShift_forward) & (_with2.stsTVIIPos)) |
                                          ((DetShift == DetShiftConstants.DetShift_backward) & (_with2.stsTVIIPos));
                }
                //Rev23.20 追加 by長野 2015/12/21
                //Rev25.00 Wスキャン追加 by長野 2016/06/19
                //else if (!CTSettings.ChangeXrayOn && CTSettings.DetShiftOn && CTSettings.scaninh.Data.lr_sft == 0)
                else if (!CTSettings.ChangeXrayOn && (CTSettings.DetShiftOn && (CTSettings.scaninh.Data.lr_sft == 0)) || CTSettings.W_ScanOn)
                {
                    functionReturnValue = ((DetShift == DetShiftConstants.DetShift_origin) & (_with2.stsCTIIPos)) |
                                          ((DetShift == DetShiftConstants.DetShift_forward) & (_with2.stsTVIIPos)) |
                                          ((DetShift == DetShiftConstants.DetShift_backward) & (_with2.stsFPDLShiftPos));
                }
                //Rev25.00 Wスキャン追加 by長野 2016/06/19
                //else if (CTSettings.ChangeXrayOn && CTSettings.DetShiftOn)
                else if (CTSettings.ChangeXrayOn && (CTSettings.DetShiftOn && !CTSettings.W_ScanOn))
                {
                    functionReturnValue = ((DetShift == DetShiftConstants.DetShift_origin) & ((_with2.stsMicroFPDPos) || (_with2.stsNanoFPDPos))) |
                                          ((DetShift == DetShiftConstants.DetShift_forward) & ((_with2.stsMicroFPDShiftPos)) || (_with2.stsNanoFPDShiftPos)) |
                                          ((DetShift == DetShiftConstants.DetShift_backward) & (_with2.stsMicroFPDShiftPos));
                }
                else
                {
                    functionReturnValue = true;
                }
				return functionReturnValue;
			}
		}


        //*************************************************************************************************
        //機　　能： シフト位置のどこにいるかチェック
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値：                 DetShiftConstants   シフト位置
        //
        //補　　足： なし
        //
        //履　　歴： v18.00 2011/02/02 (検S1)やまおか    新規作成
        //*************************************************************************************************
		public static DetShiftConstants IsDetShiftPos {
			get {

                DetShiftConstants functionReturnValue = default(DetShiftConstants);
				var _with3 = modSeqComm.MySeq;

				//検出器シフトありの場合
                //if (CTSettings.DetShiftOn)
                //Rev23.10 条件追加 by長野 2015/09/28
                //Rev25.00 Wスキャン追加 by長野 2016/06/19
                //if (CTSettings.DetShiftOn && !CTSettings.ChangeXrayOn)
                if ((CTSettings.DetShiftOn || CTSettings.W_ScanOn) && !CTSettings.ChangeXrayOn)
                {
                    //Rev23.20 条件追加 by長野 2015/12/21
                    //if (CTSettings.scaninh.Data.lr_sft == 0)
                    //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                    if (CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn)
                    {
                        //基準位置なら
                        if (_with3.stsCTIIPos)
                        {
                            functionReturnValue = DetShiftConstants.DetShift_origin;

                        //右シフト位置なら
                        }
                        else if (_with3.stsTVIIPos)
                        {
                            functionReturnValue = DetShiftConstants.DetShift_forward;

                            //その他の位置なら
                        }
                        else if (_with3.stsFPDLShiftPos)
                        {
                            functionReturnValue = DetShiftConstants.DetShift_backward;
                        }
                        else
                        {
                            functionReturnValue = DetShiftConstants.DetShift_none;
                        }
                    }
                    else
                    {
                        //基準位置なら
                        if (_with3.stsCTIIPos)
                        {
                            functionReturnValue = DetShiftConstants.DetShift_origin;

                            //シフト位置なら
                        }
                        else if (_with3.stsTVIIPos)
                        {
                            if ((CTSettings.scancondpar.Data.det_sft_pix > 0))
                            {
                                functionReturnValue = DetShiftConstants.DetShift_forward;
                            }
                            else if ((CTSettings.scancondpar.Data.det_sft_pix < 0))
                            {
                                functionReturnValue = DetShiftConstants.DetShift_backward;
                            }
                            else
                            {
                                functionReturnValue = DetShiftConstants.DetShift_origin;
                            }

                            //その他の位置なら
                        }
                        else
                        {
                            functionReturnValue = DetShiftConstants.DetShift_none;
                        }
                    }

				//stsCTIIPos,stsTVIIPosがない場合(シーケンサソフトが古いなど)は常に基準位置
				}
                //Rev25.00 Wスキャン追加 by長野 2016/06/19
                //else if (CTSettings.DetShiftOn && CTSettings.ChangeXrayOn)//Rev23.10 条件追加 by長野 2015/09/28
                else if ((CTSettings.DetShiftOn || CTSettings.W_ScanOn) && CTSettings.ChangeXrayOn)
                {
                    //基準位置なら
                    if (_with3.stsMicroFPDPos || _with3.stsNanoFPDPos)
                    {
                        functionReturnValue = DetShiftConstants.DetShift_origin;

                        //シフト位置なら
                    }
                    else if (_with3.stsMicroFPDShiftPos || _with3.stsNanoFPDShiftPos)
                    {
                        if ((CTSettings.scancondpar.Data.det_sft_pix > 0))
                        {
                            functionReturnValue = DetShiftConstants.DetShift_forward;
                        }
                        else if ((CTSettings.scancondpar.Data.det_sft_pix < 0))
                        {
                            functionReturnValue = DetShiftConstants.DetShift_backward;
                        }
                        else
                        {
                            functionReturnValue = DetShiftConstants.DetShift_origin;
                        }

                        //その他の位置なら
                    }
                    else
                    {
                        functionReturnValue = DetShiftConstants.DetShift_none;
                    }
                }
                else
                {
                    functionReturnValue = DetShiftConstants.DetShift_origin;
                }
				return functionReturnValue;

			}
		}


        //*************************************************************************************************
        //機　　能： シフト位置にあわせて検出器の図をシフトさせる
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値：                 DetShiftConstants   シフト位置
        //
        //補　　足： なし
        //
        //履　　歴： v18.00 2011/02/02 (検S1)やまおか    新規作成
        //*************************************************************************************************
		public static void MoveDetSharp()
		{
			int posOrg = 0;
			int posRight = 0;
			int posRNone = 0;
			int posLeft = 0;
			int posLNone = 0;
            int posCNone = 0;

			var _with4 = frmMechaControl.Instance;

            //Rev23.20 左右シフトの場合の処理を追加 by長野 2015/12/17
            //if (CTSettings.scaninh.Data.lr_sft == 0)
            //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
            if (CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn)
            {
                posOrg = _with4.pnlDetShift.Left + _with4.pnlDetShift.Width / 2 - _with4.lblXrayII.Width / 2;                        //基準位置
                posRight = _with4.pnlDetShift.Left + _with4.pnlDetShift.Width - _with4.lblXrayII.Width;         //右へ移動
                //2014/11/13hata キャストの修正
                //posRNone = _with4.pnlDetShift.Left + (_with4.pnlDetShift.Width - _with4.lblXrayII.Width) / 2;   //位置不定(右)
                posRNone = _with4.pnlDetShift.Left + Convert.ToInt32((_with4.pnlDetShift.Width - _with4.lblXrayII.Width) / 2F);   //位置不定(右)

                posLeft = _with4.pnlDetShift.Left ;          //左へ移動
                //2014/11/13hata キャストの修正
                //posLNone = _with4.pnlDetShift.Left - (_with4.pnlDetShift.Width - _with4.lblXrayII.Width) / 2;  //位置不定(左)
                posLNone = _with4.pnlDetShift.Left - Convert.ToInt32((_with4.pnlDetShift.Width - _with4.lblXrayII.Width) / 2F);  //位置不定(左)
                posCNone = _with4.pnlDetShift.Left + _with4.pnlDetShift.Width / 2 - _with4.lblXrayII.Width / 2;                        //基準位置(不定)
            }
            else
            {
                posOrg = _with4.pnlDetShift.Left;                                                               //基準位置
                posRight = _with4.pnlDetShift.Left + _with4.pnlDetShift.Width - _with4.lblXrayII.Width;         //右へ移動
                //2014/11/13hata キャストの修正
                //posRNone = _with4.pnlDetShift.Left + (_with4.pnlDetShift.Width - _with4.lblXrayII.Width) / 2;   //位置不定(右)
                posRNone = _with4.pnlDetShift.Left + Convert.ToInt32((_with4.pnlDetShift.Width - _with4.lblXrayII.Width) / 2F);   //位置不定(右)

                posLeft = _with4.pnlDetShift.Left - _with4.pnlDetShift.Width - _with4.lblXrayII.Width;          //左へ移動
                //2014/11/13hata キャストの修正
                //posLNone = _with4.pnlDetShift.Left - (_with4.pnlDetShift.Width - _with4.lblXrayII.Width) / 2;  //位置不定(左)
                posLNone = _with4.pnlDetShift.Left - Convert.ToInt32((_with4.pnlDetShift.Width - _with4.lblXrayII.Width) / 2F);  //位置不定(左)
                posCNone = _with4.pnlDetShift.Left + _with4.pnlDetShift.Width / 2 - _with4.lblXrayII.Width / 2;                       //基準位置(不定)//Rev23.20 追加 by長野 2015/12/17
            }


			switch (IsDetShiftPos)
            {
				case DetShiftConstants.DetShift_origin:
					//_with4.ShpXrayII.Left =posOrg;
					_with4.lblXrayII.Left = posOrg;
					break;

				case DetShiftConstants.DetShift_forward:
					//_with4.ShpXrayII.Left = posRight;
					_with4.lblXrayII.Left = posRight;
					break;

				case DetShiftConstants.DetShift_backward:
					//_with4.ShpXrayII.Left = posLeft;
					_with4.lblXrayII.Left = posLeft;
					break;

				case DetShiftConstants.DetShift_none:

                    //if (CTSettings.scaninh.Data.lr_sft == 0)
                    //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                    if (CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn)
                    {
                        _with4.lblXrayII.Left = posCNone;
                    }
                    else
                    {
                        if ((CTSettings.scancondpar.Data.det_sft_pix > 0))
                        {
                            //_with4.ShpXrayII.Left = posRNone;
                            _with4.lblXrayII.Left = posRNone;
                        }
                        else
                        {
                            //_with4.ShpXrayII.Left = posLNone;
                            _with4.lblXrayII.Left = posLNone;
                        }
                    }
					break;
			}

        }


        //*************************************************************************************************
        //機　　能： 検出器シフト処理
        //
        //           変数名          [I/O] 型            内容
        //引　　数： theMode         DetShiftConstants   検出器シフト状態
        //           theSetGainFlg   Boolean             ゲインをセットする/しない
        //
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v18.00  2011/01/31  (検S1)やまおか  新規作成
        //           v18.00  2011/07/04  やまおか        ゲインセット可否
        //*************************************************************************************************
		public static bool ShiftDet(DetShiftConstants theMode, bool theSetGainFlg = UNSET_GAIN)
        {
			bool functionReturnValue = false;   //初期値設定

            bool sftOrigin = false;
			bool sftForward = false;
			bool sftBackward = false;
			///    Dim sftNone     As Boolean

			string theCommand = null;

			sftOrigin = Convert.ToBoolean(theMode == DetShiftConstants.DetShift_origin);			//基準位置にシフト
			sftForward = Convert.ToBoolean(theMode == DetShiftConstants.DetShift_forward);			//＋方向にシフト
			sftBackward = Convert.ToBoolean(theMode == DetShiftConstants.DetShift_backward);		//－方向にシフト
            ///    sftNone = CBool(theMode = DetShift_none)            'いずれでもない 'v18.00追加 byやまおか 2011/08/19

            //検出器シフト可否判定
			if (!mod2ndDetctor.IsSwitchDet())
				return functionReturnValue;

            //Rev23.10 X線切替用でのシフトを追加 by長野 2015/09/20
            if (CTSettings.ChangeXrayOn)
            {
                if (mod2ndXray.XrayMode == mod2ndXray.XrayModeConstants.XrayMode_Xray1)
                {
                    //シーケンサに送るコマンドをセット
                    if (sftOrigin)
                    {
                        theCommand = "MicroFPDSet";
                    }
                    else if (sftForward)
                    {
                        theCommand = "MicroFPDShiftSet";
                    }
                    else
                    {
                        theCommand = "MicroFPDSet";
                    }
                }
                else if (mod2ndXray.XrayMode == mod2ndXray.XrayModeConstants.XrayMode_Xray2)
                {
                    //シーケンサに送るコマンドをセット
                    if (sftOrigin)
                    {
                        theCommand = "NanoFPDSet";
                    }
                    else if (sftForward)
                    {
                        theCommand = "NanoFPDShiftSet";
                    }
                    else
                    {
                        theCommand = "NanoFPDSet";
                    }
                }
                else
                {
                    theCommand = "MicroFPDSet";
                }
            }
            else
            {
                //Rev23.20X ここに左右シフトバージョンを追加
                //シーケンサに送るコマンドをセット
                if (sftOrigin)
                {
                    theCommand = "CTIISet";
                }
                else if (sftForward)
                {
                    theCommand = "TVIISet";
                }
                else if (sftBackward)
                {
                    //theCommand = "TVIISet";
                    //Rev23.20 左シフト追加 by長野 2015/12/17
                    theCommand = "FPDLShiftSet";

                    //とりあえずForwardと同じ
                    ///    ElseIf sftNone Then     'v18.00追加 byやまおか 2011/08/19
                    ///        '不定の場合はシフトせずにTrueとして抜ける
                    ///        DetShift = DetShift_none
                    ///        ShiftDet = True
                    ///        Exit Function
                }
                else
                {
                    theCommand = "IIChangeReset";
                }
            }
			//検出器切替
			//If Not ChangeDet(theCommand) Then Exit Function
			//If Not ChangeDet(theCommand, theSetGainFlg) Then Exit Function   'ゲインをセットする 'v18.00変更 byやまおか 2011/07/04
			//v17.52でソフトが大きく変わっているため、独立させて作る。 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
			//ゲインデータのファイル名 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
			//string gaincor_fn = null;		//v18.00追加 byやまおか 2011/02/12
			int ret = 0;			        //v18.00追加 byやまおか 2011/02/12

			//シーケンサ動作停止要求フラグリセット   'v18.00追加 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
			myRequestForStop = false;

			//検出器移動可否判定
			if (!mod2ndDetctor.IsSwitchDet())
				return functionReturnValue;

			//検出器切替命令送信
            //Rev23.10 条件追加 by長野 2015/09/20
            //if (!CTSettings.ChangeXrayOn)
            //Rev23.20 変更 by長野 2015/12/21
            //if (!CTSettings.ChangeXrayOn && CTSettings.scaninh.Data.lr_sft == 1)
            //Rev25.00 変更 by長野 2016/08/03
            if (!CTSettings.ChangeXrayOn && CTSettings.scaninh.Data.lr_sft == 1 && CTSettings.scaninh.Data.w_scan == 1)
            {
                theCommand = ((int)theMode == (int)mod2ndDetctor.DetModeConstants.DetMode_Det1 ? "CTIISet" : "TVIISet");
            }
            if (!modSeqComm.SeqBitWrite(theCommand, true, false))
				return functionReturnValue;
			
            Application.DoEvents();

			//検出器切替移動タイムアウト時間(秒)
			//const int PauseTime = 30;
            //Rev26.10 change by chouno 2018/01/16
            const int PauseTime = 50;

			//開始時間
			DateTime StartTime = DateTime.Now;

			//シーケンサ動作停止要求フラグリセット
			myRequestForStop = false;

			//v19.50 シーケンサのステータス変更街のため1.5秒待つ by長野 2013/12/17
			modCT30K.PauseForDoEvents(1.5f);
            
			//指定位置に達しているまでループ
			//Do While (Not IsOKDetPos)
			//v19.50 検出器切り替えとの場合わけをする by長野 2013/11/20
			do {

				//v18.00変更 byやまおか 2011/02/04
				if (CTSettings.SecondDetOn) {
					if (mod2ndDetctor.IsOKDetPos)
						break;
                //Rev25.00 Wスキャン追加 by長野 2016/06/19
				//} else if (CTSettings.DetShiftOn) {
				}else if((CTSettings.DetShiftOn || CTSettings.W_ScanOn)){
                    if (IsOKShiftPos)
						break; 
				} else {
					return functionReturnValue;
				}

				//1秒おきにチェック
				modCT30K.PauseForDoEvents(1);

				//タイムアウト
				//if (DateAndTime.DateDiff(Microsoft.VisualBasic.DateInterval.Second, StartTime, DateAndTime.Now) > PauseTime)
				if ((DateTime.Now - StartTime).TotalSeconds >  PauseTime)
                    return functionReturnValue;

				//ストップ要求があった場合
				if (myRequestForStop) {

					//I.I.切替強制停止
                    if (!CTSettings.ChangeXrayOn)
                    {
                        modSeqComm.SeqBitWrite("IIChangeStop", true, false);
                    }
                    else
                    {
                        modSeqComm.SeqBitWrite("FPDMoveStop", true, false);
                    }
                    return functionReturnValue;

				}

			} while (true);

			//PkeFPDで検出器シフトの場合ゲインデータをセットする 'v18.00追加 byやまおか 2011/02/27
			//'ゲイン校正中とスキャン中はセットしない
			//If ((DetType = DetTypePke) And DetShiftOn And (Not Flg_GainCorrecting)) And (Not CBool(CTBusy And CTScanStart)) Then
			//関数化&条件変更    'v18.00変更 byやまおか 2011/07/04
			if (( modSeqComm.IsSetGainData() & theSetGainFlg)) {

#if (!NoCamera)

				//ゲインデータファイル名を取得
				switch (theCommand) {

					//基準位置に移動した場合
					case "CTIISet":
					case "IIChangeReset":
                    case "MicroFPDSet":
                    case "NanoFPDSet":
                        ret = Pulsar.PkeSetGainData(Pulsar.hPke, ScanCorrect.Gain_Image_L, 1, ScanCorrect.GAIN_CORRECT_L);

                        break;
					//シフト位置に移動した場合
                    //Rev23.20 従来のシフト位置は右シフト位置として扱う by長野 2015//11/19
					case "TVIISet":
                    case "MicroFPDShiftSet":
                    case "NanoFPDShiftSet":
						//ret = Pulsar.PkeSetGainData(Pulsar.hPke, ScanCorrect.Gain_Image_L_SFT, 1, ScanCorrect.GAIN_CORRECT_L_SFT);
                        //Rev23.20 左右シフト対応 by長野 2015/11/19
                        ret = Pulsar.PkeSetGainData(Pulsar.hPke, ScanCorrect.Gain_Image_L_SFT_R, 1, ScanCorrect.GAIN_CORRECT_L_SFT_R);
                        break;
                    ////Rev23.20X 左右シフト対応 by長野 2015/11/19
                    //case "":
                    //    ret = Pulsar.PkeSetGainData(Pulsar.hPke, ScanCorrect.Gain_Image_L_SFT_L, 1, ScanCorrect.GAIN_CORRECT_L_SFT_L);
                    //    break; 
                    //Rev23.20 左シフト追加 by長野 2016/01/05 --->
                    case "FPDLShiftSet":
                        ret = Pulsar.PkeSetGainData(Pulsar.hPke, ScanCorrect.Gain_Image_L_SFT_L, 1, ScanCorrect.GAIN_CORRECT_L_SFT_L);
                        break;
                    //<---
				}

				//セット失敗した場合
				if (ret == 1)
					Interaction.MsgBox("ゲイン校正データをセットできませんでした。", MsgBoxStyle.Critical);

#endif

			}

            if ((modSeqComm.IsSetGainData() & theSetGainFlg))
            {
                //Rev25.00 ゲインセットした後、少し待つ
                Thread.Sleep(5000);
            }
            //Rev23.10 スキャン中のときは、データ収集処理が移動待ちになっているためフラグを立てる。
            if(Convert.ToBoolean(modCTBusy.CTBusy & modCTBusy.CTScanStart))
            {
                TransImageControl.DetSftCompleteFlg = true;
            }

			//切替成功
			functionReturnValue = true;
			return functionReturnValue;

		}
	}
}
