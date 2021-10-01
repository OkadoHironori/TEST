using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
//
using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;

using System.Text.RegularExpressions; //Rev21.00 追加 by長野 2015/03/08

namespace CT30K
{
	///* ************************************************************************** */
	///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
	///* 客先　　　　： ?????? 殿                                                   */
	///* プログラム名： frmFormatTransfer.frm                                       */
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
	///* V19.00    2012/04/06    (電S2)   永井　　   BHC追加                        */
	///*                                                                            */
	///* -------------------------------------------------------------------------- */
	///* ご注意：                                                                   */
	///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
	///*                                                                            */
	///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
	///* ************************************************************************** */
	public partial class frmFormatTransfer : Form
	{
		//********************************************************************************
		//  共通データ宣言
		//********************************************************************************
		private short[] ImgBuff;
        //private ushort[] ImgBuff;

		//フォームの高さ
		private const int NormalSize = 560;
		private const int DICOMSize = 732;

		private bool myCancel = false;
		private bool myBusy = false;

		#region プロパティ

		#region インスタンスを返すプロパティ 

		// frmFormatTransferのインスタンス
		private static frmFormatTransfer _Instance = null;

		/// <summary>
		/// frmFormatTransferのインスタンスを返す
		/// </summary>
		public static frmFormatTransfer Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmFormatTransfer();
				}

				return _Instance;
			}
		}

		#endregion

		//*************************************************************************************************
		//機　　能： IsBusyプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v10.2 2005/08/22 (SI3)間々田    新規作成
		//*************************************************************************************************
		public bool IsBusy
		{
			get
			{
				return myBusy;
			}
			set
			{
				//設定値を保存
				myBusy = value;

				//「実行」ボタンと「停止」ボタンの切り替え
				cmdGo.Text = CTResources.LoadResString(myBusy?StringTable.IDS_btnStop:StringTable.IDS_btnExe);

				//各コントロールのEnabledプロパティを制御
				cmdExit.Enabled = !myBusy;
				fraDICOM.Enabled = !myBusy;
				chkInformation.Enabled = !myBusy;
				chkScale.Enabled = !myBusy;
				cmbOutKind.Enabled = !myBusy;
				chkChangeContrast.Enabled = !myBusy;
				txtDirName.Enabled = !myBusy;
				lstImgFile.Enabled = !myBusy;
				cmdImgSelect.Enabled = !myBusy;
				fraSpecify.Enabled = !myBusy;
				cmdImgDelete.Enabled = !myBusy;
				cmdDirSelect.Enabled = !myBusy;

				//マウスポインタを元に戻す
				Cursor.Current = (myBusy ? Cursors.WaitCursor : Cursors.Default);
			}
		}

		#endregion

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public frmFormatTransfer()
		{
			InitializeComponent();
		}


        #region フォーマット変換後の画像サイズを算出

        //*******************************************************************************
        //機　　能： フォーマット変換後の画像サイズを算出
        //
        //           変数名          [I/O] 型        内容
        //引　　数： FileName        [I/ ] String    オリジナル画像のファイル名
        //           FormatType      [I/ ] String    フォーマットタイプ
        //           ImageInfo       [I/ ] Boolean   付帯情報付き？
        //戻 り 値：                 [ /O] Long      フォーマット変換後の画像サイズ（KB）／エラー時は -1を返す
        //
        //補　　足： 上のfnc_TransImgSizeと同じ機能
        //
        //履　　歴： v11.3  06/02/20  (SI3)間々田    新規作成
        //*******************************************************************************
        //Private Function GetTransImgSize(ByVal FileName As String, ByVal FormatType As String, Optional ImageInfo As Boolean = False) As Long
        private int GetTransImgSize(string FileName, modImgProc.FormatType theFormatType, bool ImageInfo = false)
        {
            int OriginalImgSize = 0;

            //戻り値の初期値
            int functionReturnValue = -1;

            try
            {
                //オリジナル画像のファイルサイズ算出（バイト）
                FileInfo fileInfo = new FileInfo(FileName);
                OriginalImgSize = (int)fileInfo.Length;

                //2014/11/06hata キャストの修正
                double imgSize = Math.Sqrt(OriginalImgSize / 2D);

                //条件により画像サイズを算出
                switch (theFormatType)
                {
                    #region JPEG形式

                    case modImgProc.FormatType.FormatJPG:
                        if (imgSize == 256)
                        {
                            functionReturnValue = (ImageInfo ? 60 : 26);
                        }
                        else if (imgSize == 512)
                        {
                            functionReturnValue = (ImageInfo ? 120 : 100);
                        }
                        else if (imgSize == 1024)
                        {
                            functionReturnValue = (ImageInfo ? 300 : 250);
                        }
                        else if (imgSize == 2048)
                        {
                            functionReturnValue = (ImageInfo ? 807 : 768);
                        }
                        else if (imgSize == 4096)
                        {
                            //v16.10 4096対応 画像によって圧縮したときにサイズにバラつきが出るため，複数枚変換した時の最大サイズを入れた by 長野 10/01/29
                            functionReturnValue = (ImageInfo ? 1700 : 1500);
                        }
                        break;

                    #endregion

                    #region BMP形式・PICT形式

                    case modImgProc.FormatType.FormatBMP:
                    case modImgProc.FormatType.FormatPCT:
                        if (imgSize == 256)
                        {
                            functionReturnValue = (ImageInfo ? 835 : 193);
                        }
                        else if (imgSize == 512)
                        {
                            functionReturnValue = (ImageInfo ? 1219 : 769);
                        }
                        else if (imgSize == 1024)
                        {
                            functionReturnValue = (ImageInfo ? 3973 : 3073);
                        }
                        else if (imgSize == 2048)
                        {
                            functionReturnValue = (ImageInfo ? 14089 : 12289);
                        }
                        else if (imgSize == 4096)
                        {
                            //v16.10 4096対応 by 長野 10/01/29
                            functionReturnValue = (ImageInfo ? 52753 : 41953);
                        }
                        break;

                    #endregion

                    #region TIFF16ﾋﾞｯﾄ形式

                    case modImgProc.FormatType.FormatTIF16:
                        if (imgSize == 256)
                        {
                            functionReturnValue = (ImageInfo ? 557 : 129);
                        }
                        else if (imgSize == 512)
                        {
                            functionReturnValue = (ImageInfo ? 813 : 513);
                        }
                        else if (imgSize == 1024)
                        {
                            functionReturnValue = (ImageInfo ? 2651 : 2050);
                        }
                        else if (imgSize == 2048)
                        {
                            functionReturnValue = (ImageInfo ? 9401 : 8198);
                        }
                        else if (imgSize == 4096)
                        {
                            //v16.10 4096対応のため追加 by 長野 10/01/29
                            functionReturnValue = (ImageInfo ? 35169 : 32769);
                        }
                        break;

                    #endregion

                    #region TIFF8ﾋﾞｯﾄ形式

                    case modImgProc.FormatType.FormatTIF8:
                        if (imgSize == 256)
                        {
                            functionReturnValue = (ImageInfo ? 279 : 65);
                        }
                        else if (imgSize == 512)
                        {
                            functionReturnValue = (ImageInfo ? 407 : 257);
                        }
                        else if (imgSize == 1024)
                        {
                            functionReturnValue = (ImageInfo ? 1326 : 1025);
                        }
                        else if (imgSize == 2048)
                        {
                            functionReturnValue = (ImageInfo ? 4700 : 4099);
                        }
                        else if (imgSize == 4096)
                        {
                            //v16.10 4096対応のため追加 by 長野 10/01/29
                            functionReturnValue = (ImageInfo ? 35169 : 32769);
                        }
                        break;

                    #endregion

                    #region RAW形式

                    case modImgProc.FormatType.FormatRAW:
                        functionReturnValue = Convert.ToInt32(OriginalImgSize / 1024D);
                        break;

                    #endregion

                    //v29.99 今のところ不要なのでfalse by長野 2013/04/08'''''ここから'''''
                    //        'DICOM形式
                    //        Case FormatDICOM
                    //
                    //            GetTransImgSize = OriginalImgSize / 1024 + 2
                    //v29.99 今のところ不要なのでfalse by長野 2013/04/08'''''ここまで'''''
                }
            }
            catch
            {
            }

            return functionReturnValue;
        }

        #endregion

        #region フォーマット変換を実行できる状態か確認する

        //********************************************************************************
        //機    能  ：  フォーマット変換を実行できる状態か確認する
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：                   [ /O] Long      結果(0:正常,-1:異常)
        //補    足  ：  なし
        //
        //履    歴  ：  V4.0   01/02/13  (SI1)鈴山       新規作成
        //********************************************************************************
        private bool fnc_ready()
        {
            //戻り値初期化
            bool functionReturnValue = false;

            //選択画像枚数チェック
            if (lstImgFile.ListCount < 1)
            {
                modCT30K.ErrMessage(2404, Icon: MessageBoxIcon.Exclamation);
                return functionReturnValue;
            }
            //保存先フォルダの入力チェック
            else if (string.IsNullOrEmpty(txtDirName.Text.Trim()))
            {
                modCT30K.ErrMessage(2405, Icon: MessageBoxIcon.Exclamation);
                return functionReturnValue;
            }
            //保存先フォルダが存在しない
            else if (!Directory.Exists(txtDirName.Text.Trim()))
            {
                modCT30K.ErrMessage(2402, Icon: MessageBoxIcon.Exclamation);
                txtDirName.Text = "";
                return functionReturnValue;
            }
            //DICOMの時  V6.0 append by 間々田 2002/08/29
            else if (cmbOutKind.Text == "DICOM")
            {
                if (string.IsNullOrEmpty(txtPatientName.Text.Trim()))
                {
                    //メッセージ表示：患者名は必ず入力してください。
                    //変更2014/11/18hata_MessageBox確認
                    //MessageBox.Show(StringTable.GetResString(9918, CTResources.LoadResString(12519)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MessageBox.Show(StringTable.GetResString(9918, CTResources.LoadResString(12519)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return functionReturnValue;
                }
            }

            //保存先ﾄﾞﾗｲﾌﾞの空き容量ﾁｪｯｸ
            //Rev22.01/Rev23.00 UNCのパスに対応 by長野 2015/09/14
            Uri pathUni = new Uri(txtDirName.Text);
            if (!(pathUni.IsUnc))
            {
                if (!Sub_DiskFreeSapceCheck(txtDirName.Text.Substring(0, 3))) return functionReturnValue;
            }
            else
            {
                if (!Sub_DiskFreeSapceCheck(txtDirName.Text)) return functionReturnValue;
            }


            //戻り値(正常)
            functionReturnValue = true;
            return functionReturnValue;
        }

        #endregion

        #region 画像フォーマット変換関数

        //********************************************************************************
        //機    能  ：  画像フォーマット変換関数
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  99/10/04  (CATE)山本      新規作成
        //********************************************************************************
        private int FormatConvert(string FileName, string SavePath, modImgProc.FormatType theFormat)
        {
            int functionReturnValue = 0;

            const int InfoWodth = 300;		//付帯情報の幅
            int Matrix = 0;					//マトリクス
            int rc = 0;
            int Max = 0;
            int Min = 0;
            int WindowLevel = 0;
            int WindowWidth = 0;
            //modImageInfo.ImageInfoStruct ImgInf = default(modImageInfo.ImageInfoStruct);	//付帯情報
            CTAPI.CTstr.IMAGEINFO ImgInf = default(CTAPI.CTstr.IMAGEINFO);

            //Rev21.00 スキャノかどうかのフラグ 2015/03/08
            bool scanoflg = false;
            string BaseName = null;

            short IPXsize = 0;
            short IPYsize = 0;
            //int sts = 0;
            //v19.00 ガンマ追加 by長野 2012/06/18
            float GAMMA = 0;

            //エラー時の扱い
            try
            {
                //ファイルが存在するか？
                if (!File.Exists(FileName))
                {
                    functionReturnValue = 2412;		//画像ファイルが無い、またはオープンできません。
                    throw new Exception();
                }

                //Rev21.00 スキャノ画像であるかの判定 by長野 2015/02/24
                //ベース名取得
                BaseName = Path.GetFileNameWithoutExtension(FileName);

                //スキャノの場合、スキャノ画像であることのフラグを立てる
                if (Regex.IsMatch(BaseName.ToLower(), @".+-s\d{4}$"))
                {
                    scanoflg = true;
                }

                //入力画像のサイズからマトリクスを判定する
                FileInfo fileInfo = new FileInfo(FileName);
                //2014/11/06hata キャストの修正
                Matrix = Convert.ToInt32(Math.Sqrt(fileInfo.Length / 2D));

                //画像ファイルの取り込み
                ImgBuff = new short[Matrix * Matrix * 2 + 1];
                rc = ScanCorrect.ImageOpen(ref ImgBuff[0], FileName, Matrix, Matrix);

                if (rc != 0)
                {
                    functionReturnValue = 2412;		//画像ファイルが無い、またはオープンできません。
                    throw new Exception();
                }

                //付帯情報ファイルからウィンドウレベル、ウィンドウ幅を取得する
                //if (modImageInfo.ReadImageInfo(ref ImgInf, modLibrary.RemoveExtension(FileName, ".img")))
			    if (ImageInfo.ReadImageInfo(ref ImgInf, modLibrary.RemoveExtension(FileName, ".img"))) 
                {
                    int.TryParse(ImgInf.wl.GetString(), out WindowLevel);
                    int.TryParse(ImgInf.ww.GetString(), out WindowWidth);
                    //v19.00 追加 by長野 2012/06/18
                    float.TryParse(ImgInf.gamma.GetString(), out GAMMA);
                    if (GAMMA == 0)
                    {
                        GAMMA = 1F;
                    }
                }
                else
                {
                    modCT30K.ErrMessage(2410, Icon: MessageBoxIcon.Error);
                    WindowWidth = 1000;
                }

                //保存画像サイズの設定
                if (chkInformation.CheckState == CheckState.Checked)
                {
                    IPXsize = (short)(Matrix + InfoWodth);
                    //IPYsize = (short)modLibrary.MaxVal(Matrix, 512);				//added by 山本　2004-3-20　256画素対応　付帯情報がはみ出るため
                    IPYsize = (short)modLibrary.MaxVal(Matrix, 550);				//Rev20.00 変更 by長野 2015/01/29 
                }
                else
                {
                    IPXsize = (short)Matrix;
                    IPYsize = (short)Matrix;
                }

                //画像のコントラスト調整 bmp,jpg,pct,tifの場合
                switch (theFormat)
                {
                    case modImgProc.FormatType.FormatBMP:
                    case modImgProc.FormatType.FormatJPG:
                    case modImgProc.FormatType.FormatPCT:
                    case modImgProc.FormatType.FormatTIF8:
                    case modImgProc.FormatType.FormatTIF16:

                        //added by 山本　2003-3-18　tif16ビットで階調変換を選択しなかった場合の処理				
                        if ((theFormat == modImgProc.FormatType.FormatTIF16) && (chkChangeContrast.CheckState != CheckState.Checked))	//v14.00 変更 by 山影 2007/07/24
                        {
                            Min = -32768;
                            Max = 32767;
                        }
                        else
                        {
                            //2014/11/06hata キャストの修正
                            Min = WindowLevel - Convert.ToInt32(WindowWidth / 2F);
                            Max = WindowLevel + Convert.ToInt32(WindowWidth / 2F);
                        }

                        //ChangeFullRange_Short ImgBuff(0), Matrix, Matrix, Min, Max
                        ScanCorrect.ChangeFullRange_Short(ref ImgBuff[0], Matrix, Matrix, Min, Max, GAMMA);		//v19.00 by長野 2012/06/18

                        #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                        /*
                        //コントラスト調整後のImagePro再表示
                        rc = Ipc32v5.IpAppCloseAll();											//☆☆開いている全ての画像ｳｨﾝﾄﾞを閉じる
                        //rc = IpWsCreate(IPXsize, IPYsize, 300, IMC_GRAY16)   '空の画像ウィンドウを生成（Gray Scale 16形式）
                        sts = Ipc32v5.IpWsCreate(IPXsize, IPYsize, 300, Ipc32v5.IMC_GRAY16);	//空の画像ウィンドウを生成（Gray Scale 16形式）
                        rc = Ipc32v5.IpWsFill(0, 3, 0);											//☆☆新規の画像ｳｨﾝﾄﾞを黒く塗りつぶす
                        */
                        //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                        rc = CallImageProFunction.CallFormatConvertStep1((int)IPYsize, (int)IPXsize, 1);
                        #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

                        //画像データをImage-Proの画像に書き込む
                        CTSettings.IPOBJ.DrawWordImage(ImgBuff, theWidth: Matrix, theHeight: Matrix);
                        break;
                }

                string strPixelSize = null;
                int x1 = 0;
                int x2 = 0;
                float info_ver = 0;

                //付帯情報を画像に書き込む
                if (chkInformation.CheckState == CheckState.Checked)
                {
                    //Rev20.00 移動 by長野 2015/01/29
                    info_ver = modImageInfo.GetImageInfoVerNumber(ImgInf.version.GetString());

                    //added by 山本　2003-3-18　TIF16ビットの場合は実行しない（８ビットになってしまうため）
                    if (theFormat != modImgProc.FormatType.FormatTIF16)		//V14.0 変更 by 山影
                    {
                        //フルカラーに変換
#if ImageProV3																							//v9.5 コンパイルオプションによる分岐を追加 by 間々田 2004/09/17
                        rc = Ipc32v5.IpWsConvertToRGB();													//ImagePro Ver3の場合

#else
                        #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                        //rc = Ipc32v5.IpWsConvertImage(Ipc32v5.IMC_RGB, Ipc32v5.CONV_SCALE, 0, 0, 0, 0);		//ImagePro Ver5の場合
                        //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                        rc = CallImageProFunction.CallFormatConvertStep2();
                        #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//


#endif

                        //v9.5 コンパイルオプションによる分岐を追加 by 間々田 2004/09/17
#if ImageProV3
                        //線色を白に設定
                        rc = Ipc32v5.IpPalSetRGBBrush(1, 255, 255, 255);
#endif
                    }

                    //v9.5 コンパイルオプションによる分岐を追加 by 間々田 2004/09/17
#if ImageProV3
                    rc = Ipc32v5.IpTextFont(CTResources.LoadResString(StringTable.IDS_FImageInfo0), 3);		//ＭＳ ゴシック
                    rc = Ipc32v5.IpTextSetAttr(Ipc32v5.TXT_BOLD, 0);
                    rc = Ipc32v5.IpTextSetAttr(Ipc32v5.TXT_UNDERLINE, 0);
                    rc = Ipc32v5.IpTextSetAttr(Ipc32v5.TXT_ITALIC, 0);
                    rc = Ipc32v5.IpTextSetAttr(Ipc32v5.TXT_STRIKEOUT, 0);
                    rc = Ipc32v5.IpTextSetAttr(Ipc32v5.TXT_ENCLOSED, 0);
                    rc = Ipc32v5.IpTextSetAttr(Ipc32v5.TXT_DROPSHADOW, 0);
                    rc = Ipc32v5.IpTextSetAttr(Ipc32v5.TXT_SPACING, 0);
#endif

                    #region v15.0削除ここから by 間々田 2009/02/04

                    //v15.0削除ここから by 間々田 2009/02/04
                    //Dim p   As POINTAPI
                    //p.X = ImgXsize + 10
                    //p.Y = 10
                    //
                    //With ImgInf
                    //
                    //    '画像サイズが512のときは行間を14ptにする    'v14.00追加 byやまおか 2007/06/28
                    //    vPit = IIf(CInt(.matsiz) = 512, 14, 16)     'v14.00 Vpit追加 byやまおか 2007/06/28
                    //
                    //    'v14.00 行間調整のためDoIpTextBurnの引数にVpit追加(↓↓↓ここから↓↓↓) byやまおか 2007/06/28
                    //    rc = DoIpTextBurn(LoadResString(IDS_ProductName), Left$(RemoveExtension(DDName, "\"), 30), p, vPit)   'プロダクト名   'v14.00 \があれば取り除く byやまおか 07/06/26
                    //    rc = DoIpTextBurn(LoadResString(IDS_SliceName), Left$(FFName & ".img", 30), p, vPit)                'スライス名
                    //    rc = DoIpTextBurn(LoadResString(IDS_ScanPos), Trim$(.d_tablepos), p, vPit)                        'スキャン位置(mm)
                    //    rc = DoIpTextBurn(LoadResString(IDS_ScanDate), Trim$(.d_date), p, vPit)                            'スキャン年月日
                    //    rc = DoIpTextBurn(LoadResString(IDS_ScanTime), Trim$(.start_time), p, vPit)                        'スキャン時刻
                    //    rc = DoIpTextBurn(LoadResString(12830), IIf(.data_acq_time <= 0, "", Format$(.data_acq_time, "0.0")), p, vPit)  'データ収集時間(秒) 'v14.00追加 byやまおか 2007/08/09
                    //    rc = DoIpTextBurn(LoadResString(12831), IIf(.recon_time <= 0, "", Format$(.recon_time, "0.0")), p, vPit)        '再構成時間(秒)     'v14.00追加 byやまおか 2007/08/09
                    //    rc = DoIpTextBurn(LoadResString(12806), Trim$(.volt), p, vPit)                              '管電圧(kV)
                    //    'rc = DoIpTextBurn(LoadResString(12807), Trim$(.anpere), p)                                 '管電流(μA)
                    //    rc = DoIpTextBurn(GetResString(12807, CurrentUni), Trim$(.anpere), p, vPit)                 '管電流(μA)    'v11.4変更 by 間々田 2006/03/03
                    //
                    //    rc = DoIpTextBurn(LoadResString(12808), Trim$(.scan_view), p, vPit)                         'ビュー数
                    //    rc = DoIpTextBurn(IIf(IsEnglish, "Integration num.", LoadResString(12809)), Trim$(.integ_number), p, vPit)   '積算枚数（英語の場合、リソースの文字が長いので）
                    //
                    //    If Use_FlatPanel Then
                    //        rc = DoIpTextBurn(LoadResString(IDS_BinningMode), Trim$(.iifield), p, vPit)                       'ビニングモード(FPDの場合)
                    //    Else
                    //        rc = DoIpTextBurn(LoadResString(IDS_IIField), Trim$(.iifield), p, vPit)                                 'II視野
                    //    End If
                    //
                    //    rc = DoIpTextBurn(IIf(IsEnglish, "Max Scan area", LoadResString(12811)), CStr(.max_mscan_area), p, vPit)    '最大ｽｷｬﾝｴﾘｱ(mm)（英語の場合、リソースの文字が長いので）
                    //    rc = DoIpTextBurn(LoadResString(12812), Trim$(.width), p, vPit)                                             'スライス厚(mm)
                    //    rc = DoIpTextBurn(LoadResString(IDS_SystemName), Trim$(.system_name), p, vPit)                              'システム名
                    //    rc = DoIpTextBurn(LoadResString(12814), Trim$(.matsiz), p, vPit)                                            '画像サイズ
                    //    rc = DoIpTextBurn(LoadResString(IDS_WorkShopName), Trim$(Left$(.workshop, 30)), p, vPit)                    '事業所名
                    //    rc = DoIpTextBurn(LoadResString(IDS_Comment), GetFirstItem(Trim$(Left$(.Comment, 30)), vbCrLf), p, vPit)    'コメント  '改行コード以降を削除
                    //
                    //    rc = DoIpTextBurn(LoadResString(IDS_ScanMode), Trim$(.full_mode), p, vPit)                                  'スキャンモード
                    //    rc = DoIpTextBurn(LoadResString(12818), Trim$(.fc), p, vPit)                                                'フィルタ関数
                    //
                    //    If scaninh.filter_process(0) = 0 And scaninh.filter_process(1) = 0 Then         'フィルタ処理   'v14.00追加 byやまおか 2007/06/26
                    //        rc = DoIpTextBurn(LoadResString(IDS_FilterProc), Trim$(.filter_process), p, vPit)    'フィルタ処理   'v14.00追加 byやまおか 2007/06/26
                    //    End If                                                                          'フィルタ処理   'v14.00追加 byやまおか 2007/06/26
                    //
                    //    If scaninh.rfc = 0 Then                                                         'ＲＦＣ         'v14.00追加 byやまおか 2007/06/26
                    //        rc = DoIpTextBurn(LoadResString(12832), Trim$(.rfc_char), p, vPit)          'ＲＦＣ         'v14.00追加 byやまおか 2007/06/26
                    //    End If                                                                          'ＲＦＣ         'v14.00追加 byやまおか 2007/06/26
                    //
                    //    rc = DoIpTextBurn(LoadResString(12819), CStr(.image_bias), p, vPit)                                 '画像バイアス
                    //    rc = DoIpTextBurn(LoadResString(12820), CStr(.image_slope), p, vPit)                                '画像スロープ
                    //    rc = DoIpTextBurn(LoadResString(12821), IIf(.bhc = 0, "", LoadResString(IDS_ConeBeam)), p, vPit)    'コーンビーム
                    //    rc = DoIpTextBurn(IIf(IsEnglish, "Direction", LoadResString(12822)), LoadResString(IIf(.image_direction = 0, IDS_DirectionTop, IDS_DirectionBottom)), p, vPit)       '断面像方向（英語の場合、リソースの文字が長いので）
                    //    'rc = DoIpTextBurn(LoadResString(12823), Format$(.FID - .fid_offset, "0.000"), p)           'ＦＩＤ
                    //    rc = DoIpTextBurn(gStrFidOrFdd, Format$(.Fid - .fid_offset, "0.000"), p, vPit)              'FID/FDD    'v9.6 change by 間々田 2004/10/13
                    //    rc = DoIpTextBurn(LoadResString(12824), Format$(.FCD - .fcd_offset, "0.000"), p, vPit)      'ＦＣＤ
                    //    rc = DoIpTextBurn(LoadResString(IDS_WindowLevel), Trim$(.WL), p, vPit)                                'ウィンドウレベル
                    //    rc = DoIpTextBurn(LoadResString(IDS_WindowWidth), Trim$(.ww), p, vPit)                                'ウィンドウ幅
                    //
                    //    '1画素サイズ(mm)を求めておく
                    //    Dim strPixelSize As String
                    //    If Val(.matsiz) = 0 Then
                    //        strPixelSize = ""
                    //    Else
                    //        strPixelSize = Format$(Val(.scale) / 1000 / Val(.matsiz), "##0.0000000000")
                    //    End If
                    //
                    //    '拡大倍率　'added by 間々田　2004/01/29
                    //    Dim strMagnify As String
                    //    Dim m As Integer
                    //    If Val(.scale) / 10000 = 0 Then
                    //        strMagnify = ""
                    //    Else
                    //        m = IIf(Val(.matsiz) = 2048, 2, 1)    'added by 間々田 2004/05/13 拡大率を2倍にする
                    //        strMagnify = Format$(m * scancondpar.magnify_para / (Val(.scale) / 10000), "##0.00000")
                    //    End If
                    //
                    //    rc = DoIpTextBurn(LoadResString(IDS_PixelSize), strPixelSize, p, vPit)                 '1画素サイズ(mm)
                    //    rc = DoIpTextBurn(LoadResString(IDS_Magnification), strMagnify, p, vPit)                   '拡大倍率
                    //    rc = DoIpTextBurn(LoadResString(IDS_Scale), CStr(Val(.scale) / 10000), p, vPit)    'スケール(mm)
                    //    'v14.00 行間調整のためDoIpTextBurnの引数にVpit追加(↑↑↑ここまで↑↑↑) byやまおか 2007/06/28
                    //
                    //End With
                    //v15.0削除ここまで by 間々田 2009/02/04

                    #endregion


                    //v15.0追加ここから by 間々田 2009/02/04

                    //オフセット位置セット
                    CTSettings.IPOBJ.SetCurrentPoint(Matrix + 10, 10);

                    //行ピッチ
                    CTSettings.IPOBJ.LinePitch = (Convert.ToInt16(ImgInf.matsiz.GetString()) == 512 ? 14 : 16);

                    //ヘッダ幅
                    //IPOBJ.HeaderWidth = 17
                    //v17.60 英語の場合、リソースの文字が長いためヘッダ幅を広げる
                    //CTSettings.IPOBJ.HeaderWidth = (modCT30K.IsEnglish == true ? 24 : 17);
                    //Rev20.01 上記復活 by長野 2015/05/19
                    CTSettings.IPOBJ.HeaderWidth = (modCT30K.IsEnglish == true ? 24 : 17);

                    //Rev20.00 狭いほうで統一 by長野 2015/01/29
                    CTSettings.IPOBJ.LinePitch = 14;

                    CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_ProductName), Path.GetDirectoryName(FileName));											//プロダクト名
                    CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_SliceName), Path.GetFileName(FileName));													//スライス名
                    CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_ScanPos) + "(mm)", ImgInf.d_tablepos.GetString());													//スキャン位置(mm)
                    CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_ScanDate), ImgInf.d_date.GetString());																//スキャン年月日
                    CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_ScanTime), ImgInf.start_time.GetString());															//スキャン時刻
                    CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_DataAcqTime), (ImgInf.data_acq_time <= 0 ? "" : ImgInf.data_acq_time.ToString("0.0")));	//データ収集時間(秒) 'v14.00追加 byやまおか 2007/08/09
                    //Rev21.00 条件式追加 by長野 2015/03/09
                    if (scanoflg == false)
                    {
                        CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_ReconTime), (ImgInf.recon_time <= 0 ? "" : ImgInf.recon_time.ToString("0.0")));			//再構成時間(秒)     'v14.00追加 byやまおか 2007/08/09
                    }
                    else
                    {
                        CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_ReconTime), "");
                    }
                    CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_TubeVoltage) + "(kV)", ImgInf.volt.GetString().Trim());												//管電圧(kV)
                    CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_TubeCurrent) + "(" + modXrayControl.CurrentUni + ")", ImgInf.anpere.GetString().Trim());				//管電流(μA)    'v11.4変更 by 間々田 2006/03/03

                    CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_ViewNum), ImgInf.scan_view.GetString().Trim());														//ビュー数
                    //Rev21.00 条件式追加 by長野 2015/03/09
                    if (scanoflg == false)
                    {
                        CTSettings.IPOBJ.DrawText((modCT30K.IsEnglish ? "Integration num." : CTResources.LoadResString(StringTable.IDS_IntegNum)), ImgInf.integ_number.GetString().Trim());		//積算枚数（英語の場合、リソースの文字が長いので）
                    }
                    else
                    {
                        CTSettings.IPOBJ.DrawText((modCT30K.IsEnglish ? "Integration num." : CTResources.LoadResString(StringTable.IDS_IntegNum)), ImgInf.mscano_integ_number.GetString().Trim());		//積算枚数（英語の場合、リソースの文字が長いので）
                    }


                    //Rev20.00 追加 by長野 2015/01/29
                    //Rev21.00 条件式追加 by長野 2015/03/09
                    if (info_ver >= 20.00 && scanoflg == false)
                    {
                        if (ImgInf.table_rotation == 0)
                        {
                            CTSettings.IPOBJ.DrawText(StringTable.BuildResStr(StringTable.IDS_Rotate, StringTable.IDS_Table), CTResources.LoadResString(12383));  //テーブル回転
                        }
                        else
                        {
                            CTSettings.IPOBJ.DrawText(StringTable.BuildResStr(StringTable.IDS_Rotate, StringTable.IDS_Table), CTResources.LoadResString(12386));  //テーブル回転
                        }
                    }
                    else if (info_ver < 20.00 || scanoflg == true)
                    {
                        CTSettings.IPOBJ.DrawText(StringTable.BuildResStr(StringTable.IDS_Rotate, StringTable.IDS_Table), "－");  //テーブル回転
                    }


                    //            If Use_FlatPanel Then
                    //                IPOBJ.DrawText LoadResString(IDS_BinningMode), Trim$(.iifield)                                  'ビニングモード(FPDの場合)
                    //            Else
                    //                IPOBJ.DrawText LoadResString(IDS_IIField), .iifield                                             'II視野
                    //            End If
                    //
                    //v17.30 付帯情報の検出器種類に変更 by 長野 2010-09-26
                    if (ImgInf.detector == 0)
                    {
                        //                IPOBJ.DrawText LoadResString(IDS_BinningMode), Trim$(.iifield)                                 'ビニングモード(FPDの場合)
                        //           Else
                        CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_IIField), ImgInf.iifield.GetString());					//II視野
                    }

                    //            If DetType = DetTypePke Then    'v17.00追加 byやまおか
                    if (ImgInf.detector == 2 && (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke))		//v17.30 付帯情報の検出器種類に変更 by 長野 2010-09-26
                    {
                        //ストリングテーブル化　'v17.60 by 長野 2011/05/22
                        //IPOBJ.DrawText "FPDゲイン(pF)", IIf(GetImageInfoVerNumber(.version) >= 17, GetFpdGainStr(.fpd_gain), "")        'FPDゲイン(pF)
                        //IPOBJ.DrawText "FPD積分時間(ms)", IIf(GetImageInfoVerNumber(.version) >= 17, GetFpdIntegStr(.fpd_integ), "")    'FPD積分時間(ms)
                        //CTSettings.IPOBJ.DrawText(CTResources.LoadResString(20015) + "(pF)", (modImageInfo.GetImageInfoVerNumber(ImgInf.version.GetString()) >= 17 ? modCT30K.GetFpdGainStr(ImgInf.fpd_gain) : ""));		//FPDゲイン(pF)
                        //CTSettings.IPOBJ.DrawText(CTResources.LoadResString(20016) + "(ms)", (modImageInfo.GetImageInfoVerNumber(ImgInf.version.GetString()) >= 17 ? modCT30K.GetFpdIntegStr(ImgInf.fpd_integ) : ""));	//FPD積分時間(ms)
                        //v19.50 以前のバージョンの画像も読めるように変更 by長野 2013/11/21   //変更2014/10/07hata_v19.51反映
                        //Rev20.00 上に移動 by長野 2015/01/29
                        //info_ver = modImageInfo.GetImageInfoVerNumber(ImgInf.version.GetString());
					    if (((info_ver >= 19.5) | (info_ver >= 18.0 | info_ver < 19.0)))
                        {
                            //CTSettings.IPOBJ.DrawText("FPDゲイン(pF)", modImageInfo.GetImageInfoFpdGain(ref ImgInf));      //FPDゲイン(pF)      'v18.00変更 byやまおか 2011/07/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                            //CTSettings.IPOBJ.DrawText("FPD積分時間(ms)", modImageInfo.GetImageInfoFpdInteg(ref ImgInf));    //FPD積分時間(ms)    'v18.00変更 byやまおか 2011/07/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                            //Rev20.01 変更 by長野　2015/05/19
                            CTSettings.IPOBJ.DrawText(CTResources.LoadResString(12109), modImageInfo.GetImageInfoFpdGain(ref ImgInf));      //FPDゲイン(pF)      'v18.00変更 byやまおか 2011/07/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
						    CTSettings.IPOBJ.DrawText(CTResources.LoadResString(12110), modImageInfo.GetImageInfoFpdInteg(ref ImgInf));    //FPD積分時間(ms)    'v18.00変更 byやまおか 2011/07/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                        }
                        else
                        {
                            CTSettings.IPOBJ.DrawText(CTResources.LoadResString(20015) + "(pF)", (modImageInfo.GetImageInfoVerNumber(ImgInf.version.GetString()) >= 17 ? modCT30K.GetFpdGainStr(ImgInf.fpd_gain, CTSettings.t20kinf.Data.pki_fpd_type) : ""));    //FPDゲイン(pF)
                            CTSettings.IPOBJ.DrawText(CTResources.LoadResString(20016) + "(ms)", (modImageInfo.GetImageInfoVerNumber(ImgInf.version.GetString()) >= 17 ? modCT30K.GetFpdIntegStr(ImgInf.fpd_integ) : ""));  //FPD積分時間(ms)
					    }                  
                    
                    }

                    //IPOBJ.DrawText LoadResString(IDS_MaxScanArea) & "(mm)", CStr(.max_mscan_area)                       '最大ｽｷｬﾝｴﾘｱ(mm)
                    CTSettings.IPOBJ.DrawText("FOV(mm)", Convert.ToString(ImgInf.mscan_area));												//FOV(mm)            v15.0変更 最大スキャンエリア→FOVに変更 2009/07/24 by 間々田
                    //Rev21.00 条件式追加 by長野 2015/03/09
                    if (scanoflg == false)
                    {
                        CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_SliceWidth) + "(mm)", ImgInf.width.GetString().Trim());			//スライス厚(mm)
                    }
                    else
                    {
                        CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_SliceWidth) + "(mm)", ImgInf.mscano_width.GetString().Trim());			//スライス厚(mm)
                    }
                    CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_SystemName), ImgInf.system_name.GetString().Trim());				//システム名
                    CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_Matrix), ImgInf.matsiz.GetString().Trim());						//マトリクスサイズ
                    //IPOBJ.DrawText LoadResString(IDS_WorkShopName), Trim$(Left$(.workshop, 30))                         '事業所名  'v15.0削除 表示しないことになった 2009/07/24 by 間々田


                    //CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_Comment), modLibrary.GetFirstItem(ImgInf.comment.GetString().Substring(0, 30).Trim(), "\r\n"));		//コメント  '改行コード以降を削除
                    string strComment = "";
                    strComment = ImgInf.comment.GetString();
                    if (!string.IsNullOrEmpty(strComment))
                    {
                        int stLen = strComment.Length;
                        if (stLen > 30) stLen = 30;
                        strComment = strComment.Substring(0, stLen).Trim();
                    }
                    CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_Comment), modLibrary.GetFirstItem(strComment, "\r\n"));		//コメント  '改行コード以降を削除

                    if (scanoflg == false)
                    {
                        //CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_ScanMode), ImgInf.full_mode.GetString().Trim());					//スキャンモード
                        //Rev25.00 Wスキャン追加 by長野 2016/07/05
                        //CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_W_Scan),(ImgInf.w_scan == 1 ? "OFF":"ON"));
                        //Rev26.01 修正 by chouno 2017/11/02
                        //if (CTSettings.W_ScanOn) //Rev26.10 add by chouno 2018/01/13
                        //Rev26.10 シフトスキャンの名称がWスキャンの場合は表示 by chouno 2018/01/16
                        //if (CTSettings.W_ScanOn) //Rev26.10 add by chouno 2018/01/13
                        if (CTSettings.W_ScanOn || CTSettings.infdef.Data.scan_mode[3].GetString() == CTResources.LoadResString(25009))
                        {
                            if (ImgInf.w_scan == 1 || ImgInf.full_mode.GetString() == CTSettings.ctinfdef.Data.full_mode[3].GetString())
                            {
                                //CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_W_Scan), ImgInf.w_scan == 1 ? "ON" : "OFF"));
                                CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_W_Scan), "ON");
                            }
                            else
                            {
                                CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_W_Scan), "OFF");
                            }
                        }

                        CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_ScanMode), ImgInf.full_mode.GetString().Trim());					//スキャンモード

                        CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_FilterFunc), ImgInf.fc.GetString().Trim());						//フィルタ関数

                        if (CTSettings.scaninh.Data.filter_process[0] == 0 & CTSettings.scaninh.Data.filter_process[1] == 0)
                        { 						//フィルタ処理   'v14.00追加 byやまおか 2007/06/26
                            CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_FilterProc), ImgInf.filter_process.GetString().Trim());		//フィルタ処理   'v14.00追加 byやまおか 2007/06/26
                        }																													//フィルタ処理   'v14.00追加 byやまおか 2007/06/26

                        if (CTSettings.scaninh.Data.rfc == 0)
                        {																					//ＲＦＣ         'v14.00追加 byやまおか 2007/06/26
                            CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_RFC), ImgInf.rfc_char.GetString().Trim());					//ＲＦＣ         'v14.00追加 byやまおか 2007/06/26
                        }																													//ＲＦＣ         'v14.00追加 byやまおか 2007/06/26

                        //v19.00 BHCファイル(電S2)永井
                        if (CTSettings.scaninh.Data.mbhc == 0)
                        {
                            CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_BHCFile), modLibrary.AddExtension(modLibrary.RemoveNull(ImgInf.mbhc_name.GetString()), ".csv"));
                        }

                        CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_ImageBias), Convert.ToString(ImgInf.image_bias));				//画像バイアス
                        CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_ImageSlope), Convert.ToString(ImgInf.image_slope));				//画像スロープ
                        CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_ConeBeam), (ImgInf.bhc == 0 ? "" : CTResources.LoadResString(StringTable.IDS_ConeBeam)));		//コーンビーム
                        //IPOBJ.DrawText IIf(IsEnglish, "Direction", LoadResString(IDS_ImageDirection)), LoadResString(IIf(.image_direction = 0, IDS_DirectionTop, IDS_DirectionBottom))       '断面像方向（英語の場合、リソースの文字が長いので）
                        //v17.60 ヘッダの幅を広げたのでDirectionをやめてリソースとした by長野 2011/06/15
                        CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_ImageDirection), CTResources.LoadResString(ImgInf.image_direction == 0 ? StringTable.IDS_DirectionTop : StringTable.IDS_DirectionBottom));		//断面像方向（英語の場合、リソースの文字が長いので）
                    
                        //Rev26.00 ファントムレスBHC情報 by井上 2017/01/20
                        if (CTSettings.scaninh.Data.mbhc_phantomless == 0)
                        {
                            CTSettings.IPOBJ.DrawText(CTResources.LoadResString(21003), modLibrary.RemoveNull(ImgInf.mbhc_phantomless_c.GetString()));
                        }                     
                    }
                    else
                    {
                        CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_ScanMode), "－");					//スキャンモード
                        CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_FilterFunc), "－");						//フィルタ関数

                        if (CTSettings.scaninh.Data.filter_process[0] == 0 & CTSettings.scaninh.Data.filter_process[1] == 0)
                        { 						//フィルタ処理   'v14.00追加 byやまおか 2007/06/26
                            CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_FilterProc), "－");		//フィルタ処理   'v14.00追加 byやまおか 2007/06/26
                        }																													//フィルタ処理   'v14.00追加 byやまおか 2007/06/26

                        if (CTSettings.scaninh.Data.rfc == 0)
                        {																					//ＲＦＣ         'v14.00追加 byやまおか 2007/06/26
                            CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_RFC), "－");					//ＲＦＣ         'v14.00追加 byやまおか 2007/06/26
                        }																													//ＲＦＣ         'v14.00追加 byやまおか 2007/06/26

                        //v19.00 BHCファイル(電S2)永井
                        if (CTSettings.scaninh.Data.mbhc == 0)
                        {
                            CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_BHCFile), "－");
                        }

                        CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_ImageBias), Convert.ToString(ImgInf.mscano_bias));				//画像バイアス
                        CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_ImageSlope), Convert.ToString(ImgInf.mscano_slope));				//画像スロープ
                        CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_ConeBeam), "－");		//コーンビーム
                        //IPOBJ.DrawText IIf(IsEnglish, "Direction", LoadResString(IDS_ImageDirection)), LoadResString(IIf(.image_direction = 0, IDS_DirectionTop, IDS_DirectionBottom))       '断面像方向（英語の場合、リソースの文字が長いので）
                        //v17.60 ヘッダの幅を広げたのでDirectionをやめてリソースとした by長野 2011/06/15
                        CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_ImageDirection), "－");		//断面像方向（英語の場合、リソースの文字が長いので）
                    }
                    //CTSettings.IPOBJ.DrawText(CTSettings.gStrFidOrFdd, (ImgInf.fid - ImgInf.fid_offset).ToString("0.000"));								//FID/FDD    'v9.6 change by 間々田 2004/10/13                    //削除2014/10/07hata_v19.51反映
                    CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_FCD), (ImgInf.fcd - ImgInf.fcd_offset).ToString("0.000"));			//FCD
                    //追加2014/10/07hata_v19.51反映
                    CTSettings.IPOBJ.DrawText(CTSettings.gStrFidOrFdd, (ImgInf.fid - ImgInf.fid_offset).ToString("0.000"));     //FID/FDD    'v9.6 change by 間々田 2004/10/13   'v18.00 FCD⇔FID入れ替え byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    CTSettings.IPOBJ.DrawText(CTSettings.AxisName[1], ImgInf.table_x_pos.ToString("0.00"));                     //Y軸(AxisName(1))   'v18.00追加 byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

                    CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_WindowLevel), ImgInf.wl.GetString().Trim());									//ウィンドウレベル
                    CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_WindowWidth), ImgInf.ww.GetString().Trim());									//ウィンドウ幅
                   
                    //CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_Gamma), ImgInf.gamma.GetString().Trim().Format("0.00"));						//ガンマ補正値 v19.00 by長野 2012-02-21
					double gamma = 0;
                    double.TryParse(ImgInf.gamma.GetString(), out gamma);
					if (gamma == 0)
					{
                        ImgInf.gamma.SetString("1.00");     //ガンマ補正値を持たない旧バージョンの画像は初期値1とする
                        gamma = 1.00;
                    }
                    CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_Gamma), gamma.ToString("0.00"));						//ガンマ補正値 v19.00 by長野 2012-02-21
                    
                    
                    //            If CDbl(ImgInf.gamma) = 0 Then
                    //
                    //                IPOBJ.DrawText LoadResString(IDS_Gamma), "1"                                              'ガンマ補正値 v19.00 by長野 2012-02-21
                    //
                    //            End If

                    double ImgInf_matsiz = 0.0;
                    double ImgInf_scale = 0.0;

                    //1画素サイズ(mm)を求めておく
                    double.TryParse(ImgInf.matsiz.GetString(), out ImgInf_matsiz);
                    if (ImgInf_matsiz == 0)
                    {
                        strPixelSize = "";
                    }
                    else
                    {
                        double.TryParse(ImgInf.scale.GetString(), out ImgInf_scale);
                        strPixelSize = (ImgInf_scale / 1000 / ImgInf_matsiz).ToString("0.0000000000");
                    }

                    //拡大倍率　'added by 間々田　2004/01/29
                    string strMagnify = null;
                    int m = 0;

                    double.TryParse(ImgInf.scale.GetString(), out ImgInf_scale);
                    if (ImgInf_scale / 10000 == 0)
                    {
                        strMagnify = "";
                    }
                    else
                    {
                        //4096対応のためコメントアウト v16.10 by 長野 10/01/29
                        //m = IIf(Val(.matsiz) = 2048, 2, 1)    'added by 間々田 2004/05/13 拡大率を2倍にする
                        //v16.10 拡大率mの決め方を変更 by 長野 2010/02/16
                        double.TryParse(ImgInf.matsiz.GetString(), out ImgInf_matsiz);
                        if (ImgInf_matsiz == 2048)
                        {
                            m = 2;
                        }
                        else if (ImgInf_matsiz == 4096)
                        {
                            m = 4;
                        }
                        else
                        {
                            m = 1;
                        }

                        //strMagnify = (m * CTSettings.scancondpar.Data.magnify_para / (ImgInf_scale / 10000)).ToString("0.00000");
                        //Rev23.10 変更 by長野 2015/11/04
                        strMagnify = (m * CTSettings.scancondpar.Data.magnify_para[ImgInf.multi_tube] / (ImgInf_scale / 10000)).ToString("0.00000");
                    }

                    double.TryParse(ImgInf.scale.GetString(), out ImgInf_scale);
                    CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_PixelSize), strPixelSize);							//1画素サイズ(mm)
                    CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_Magnification), strMagnify);							//拡大倍率
                    CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_Scale), Convert.ToString(ImgInf_scale / 10000));		//スケール(mm)

                    int Y = 0;
                    Y = CTSettings.IPOBJ.GetCurrentPointY;

                    //v15.0追加ここまで by 間々田 2009/02/04

                    //スケールバーの表示
                    //2014/11/06hata キャストの修正
                    x1 = Matrix + 150 - Convert.ToInt32(Matrix / 20F);
                    x2 = x1 + Convert.ToInt32(Matrix / 10F);

                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
//                    //rc = IpListPts(Pts(0), CStr(x1) & " 490 " & CStr(x1) & " 500 " & CStr(x2) & " 500 " & CStr(x2) & " 490 ")  'v14.00 下記に変更 byやまおか 2007/06/26
//                    //v14.00 付帯情報の最下行のy位置によってスケールの位置を移動する byやまおか 2007/06/26
//                    //rc = IpListPts(Pts(0), CStr(x1) & " " & CStr(p.Y + 10) & " " & CStr(x1) & " " & CStr(p.Y + 20) & " " & CStr(x2) & " " & CStr(p.Y + 20) & " " & CStr(x2) & " " & CStr(p.Y + 10))
//                    rc = Ipc32v5.IpListPts(ref Ipc32v5.Pts[0], Convert.ToString(x1) + " " + Convert.ToString(Y + 10) + " " + Convert.ToString(x1) + " " + Convert.ToString(Y + 20) + " " + Convert.ToString(x2) + " " + Convert.ToString(Y + 20) + " " + Convert.ToString(x2) + " " + Convert.ToString(Y + 10));

//#if ImageProV3		//v9.5 コンパイルオプションによる分岐を追加 by 間々田 2004/09/17
//                    rc = Ipc32v5.IpAnotLine(ref Ipc32v5.Pts[0], 4, 0, 0);
//#else
//                    //v9.5 追加ここから Image-Pro Ver.5の場合  by 間々田 2004/09/17
//                    //        rc = IpAnCreateObj(GO_OBJ_LINE)             '線分オブジェクトの作成
//                    //        rc = IpAnMove(0, x1, 490)                   '線分の始点座標の指定
//                    //        rc = IpAnMove(2, x1, 500)                   '線分の２点目の座標の指定
//                    //        rc = IpAnMove(3, x2, 500)                   '線分の３点目の座標の指定
//                    //        rc = IpAnMove(4, x2, 490)                   '線分の４点目の座標の指定
//                    rc = Ipc32v5.IpAnCreateObj(Ipc32v5.GO_OBJ_POLY);										//線分オブジェクトの作成
//                    rc = Ipc32v5.IpAnPolyAddPtArray(ref Ipc32v5.Pts[0], 4);
//                    rc = Ipc32v5.IpAnSet(Ipc32v5.GO_ATTR_PENCOLOR, ColorTranslator.ToOle(Color.White));		//色の指定
//                    rc = Ipc32v5.IpAnBurn();																//書込む
//                    rc = Ipc32v5.IpAnDeleteObj();															//v14.00 追加 by yamakage

//                    //v9.5 追加ここまで Image-Pro Ver.5の場合  by 間々田 2004/09/17
//#endif
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    rc = CallImageProFunction.CallFormatConvertStep3(Y, x1,x2);
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

                }


                //v14.00 スケール付加機能追加　ここから by 山影 2007/07/17
                if (chkScale.CheckState == CheckState.Checked)
                {
                    double tmp = 0;
                    int scaleSize = 0;
                    string ScaleUnit = null;
                    #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
                    /*
                    Dim DotPos As String
*/
                    #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
                    int DotPos = 0;

                    double ImgInf_matsiz = 0.0;
                    double ImgInf_scale = 0.0;

                    //1画素サイズ(mm)を求めておく
                    double.TryParse(ImgInf.matsiz.GetString(), out ImgInf_matsiz);
                    if (ImgInf_matsiz == 0)
                    {
                        strPixelSize = "";
                    }
                    else
                    {
                        double.TryParse(ImgInf.scale.GetString(), out ImgInf_scale);
                        strPixelSize = (ImgInf_scale / 1000 / ImgInf_matsiz).ToString("0.0000000000");
                    }


                    string intNum = null;
                    string decNum = null;
                    int cnt = 0;

                    ScaleUnit = "mm";
                    //2014/11/06hata キャストの修正
                    double.TryParse(((double)Matrix / 7D * Convert.ToDouble(strPixelSize)).ToString("0.0000000000"), out tmp);
                    DotPos = Convert.ToString(tmp).IndexOf(".") + 1;

                    //小数点位置の確認
                    if (tmp < 1)
                    {
                        intNum = Convert.ToString(tmp).Substring(0, DotPos - 1);
                        decNum = Convert.ToString(tmp).Substring(DotPos);
                        double.TryParse(decNum, out tmp);
                        //DotPos = -(decNum.IndexOf(Convert.ToString(tmp) + 1));
                        //Rev20.00 修正 by長野 2015/02/24
                        DotPos = -(decNum.IndexOf(Convert.ToString(tmp)) + 1);
                    }

                    //単位合わせ
                    cnt = 0;
                    while (DotPos <= 0)
                    {
                        DotPos = DotPos + 3;
                        cnt = cnt + 1;
                    }
                    if (cnt == 0)
                    {
                        DotPos = DotPos - 2;
                    }
                    else if (cnt == 1)
                    {
                        ScaleUnit = "μm";
                    }
                    else if (cnt == 2)
                    {
                        ScaleUnit = "nm";
                    }

                    //スケールサイズ割り振り
                    scaleSize = Convert.ToInt32(Convert.ToString(tmp).Substring(0,1));
                    if (scaleSize == 1 || scaleSize == 2)
                    {
                        //2014/11/06hata キャストの修正
                        scaleSize = Convert.ToInt32((scaleSize + 1) * Math.Pow(10, DotPos));
                    }
                    else if (scaleSize == 3 || scaleSize == 4)
                    {
                        //2014/11/06hata キャストの修正
                        scaleSize = Convert.ToInt32(5 * Math.Pow(10, DotPos));
                    }
                    else
                    {
                        //2014/11/06hata キャストの修正
                        scaleSize = Convert.ToInt32(10 * Math.Pow(10, DotPos));
                    }

                    if (scaleSize >= 1000 && ScaleUnit == "nm")
                    {
                        //2014/11/06hata キャストの修正
                        scaleSize = Convert.ToInt32(scaleSize / 1000F);
                        ScaleUnit = "μm";
                    }
                    else if (scaleSize >= 1000 & ScaleUnit == "μm")
                    {
                        //2014/11/06hata キャストの修正
                        scaleSize = Convert.ToInt32(scaleSize / 1000F);
                        ScaleUnit = "mm";
                    }

                    int ScalePixel = 0;
                    //2014/11/06hata キャストの修正
                    ScalePixel = Convert.ToInt32(scaleSize / Convert.ToDouble(strPixelSize));
                    if (ScaleUnit == "μm")
                    {
                        //2014/11/06hata キャストの修正
                        ScalePixel = Convert.ToInt32(ScalePixel / 1000F);
                    }
                    else if (ScaleUnit == "nm")
                    {
                        //2014/11/06hata キャストの修正
                        ScalePixel = Convert.ToInt32(ScalePixel / 1000000D);
                    }
                    //2014/11/06hata キャストの修正
                    x2 = Convert.ToInt32(Matrix * 19 / 20F);
                    x1 = x2 - ScalePixel;

                    int y1 = 0;
                    int y2 = 0;
                    int ScaleWidth = 0;

                    ScaleWidth = 3;
                    if (Matrix == 1024)
                    {
                        ScaleWidth = 6;
                    }
                    else if (Matrix == 2048)
                    {
                        ScaleWidth = 12;
                    }
                    //v16.10 4096を追加 by 長野 10/01/29
                    else if (Matrix == 4096)
                    {
                        ScaleWidth = 24;
                    }

                    //2014/11/06hata キャストの修正
                    y1 = Convert.ToInt32(Matrix * 19 / 20F);
                    y2 = y1 + ScaleWidth;

                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//

                    //rc = Ipc32v5.IpAnCreateObj(Ipc32v5.GO_OBJ_RECT);
                    //rc = Ipc32v5.IpAnMove(0, (short)x1, (short)y1);
                    //rc = Ipc32v5.IpAnMove(5, (short)x2, (short)y2);
                    //rc = Ipc32v5.IpAnSet(Ipc32v5.GO_ATTR_PENCOLOR, ColorTranslator.ToOle(Color.White));				//色の指定
                    //rc = Ipc32v5.IpAnSet(Ipc32v5.GO_ATTR_BRUSHCOLOR, ColorTranslator.ToOle(Color.White));
                    //rc = Ipc32v5.IpAnSet(Ipc32v5.GO_ATTR_RECTSTYLE, Ipc32v5.GO_RECTSTYLE_BORDER_FILL);
                    //rc = Ipc32v5.IpAnBurn();																		//書込む

                    //rc = Ipc32v5.IpAnDeleteObj();
                    //
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    rc = CallImageProFunction.CallFormatConvertStep4(x1, y1, x2, y2);
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//
                    


                    //Ipc32v5.POINTAPI pp = default(Ipc32v5.POINTAPI);
                    Winapi.POINTAPI pp  = default( Winapi.POINTAPI);

                    int fontSize = 0;
                    if (Matrix == 512)
                    {
                        fontSize = 16;
                    }
                    else if (Matrix == 1024)
                    {
                        fontSize = 32;
                    }
                    else if (Matrix == 2048)
                    {
                        fontSize = 64;
                    }
                    else if (Matrix == 4096)		//v16.10 4096を追加 by 長野 10/01/29
                    {
                        fontSize = 128;
                    }
                    else
                    {
                        fontSize = 12;
                    }

                    if (ScaleUnit == "μm")
                    {
                        pp.x = x2 - Convert.ToInt32((ScalePixel + (((scaleSize + ScaleUnit).Length + 1) / 2F) * fontSize) / 2F);
                    }
                    else
                    {
                        pp.x = x2 - Convert.ToInt32((ScalePixel + ((scaleSize + ScaleUnit).Length / 2F) * fontSize) / 2F);
                    }

                    pp.y = y1 - fontSize;

                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    //rc = Ipc32v5.IpAnCreateObj(Ipc32v5.GO_OBJ_TEXT);										//注釈オブジェクトの作成
                    //rc = Ipc32v5.IpAnMove(0, (short)pp.x, (short)pp.y);										//オブジェクトの移動
                    //rc = Ipc32v5.IpAnText(scaleSize + ScaleUnit);											//注釈オブジェクトにテキストの書込み
                    //rc = Ipc32v5.IpAnSet(Ipc32v5.GO_ATTR_FONTSIZE, fontSize);								//フォントサイズの設定：画像サイズで変更
                    //rc = Ipc32v5.IpAnSetFontName(CTResources.LoadResString(StringTable.IDS_FImageInfo0));		//フォント名の設定：ＭＳ ゴシック
                    //rc = Ipc32v5.IpAnSet(Ipc32v5.GO_ATTR_TEXTCOLOR, ColorTranslator.ToOle(Color.White));	//テキストの色指定
                    //rc = Ipc32v5.IpAnBurn();
                    //
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    rc = CallImageProFunction.CallFormatConvertStep5(pp.x, pp.y, scaleSize.ToString(),ScaleUnit, fontSize, CTResources.LoadResString(StringTable.IDS_FImageInfo0));
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//
                    
                }
                //v14.00 スケール付加機能追加ここまで by 山影 2007/07/17


                //拡張子の決定
                string Extension = null;
                switch (theFormat)
                {
                    case modImgProc.FormatType.FormatBMP:
                        Extension = "bmp";
                        break;
                    case modImgProc.FormatType.FormatJPG:
                        Extension = "jpg";
                        break;
                    case modImgProc.FormatType.FormatTIF8:
                        Extension = "tif";
                        break;
                    case modImgProc.FormatType.FormatTIF16:
                        Extension = "tif";
                        break;
                    case modImgProc.FormatType.FormatPCT:
                        Extension = "pct";
                        break;
                    case modImgProc.FormatType.FormatRAW:
                        Extension = "raw";
                        break;
                    //v29.99 今のところ不要なのでfalse by長野 2013/04/08'''''ここから'''''
                    //        Case FormatDICOM
                    //v29.99 今のところ不要なのでfalse by長野 2013/04/08'''''ここまで'''''
                }

                //保存ファイル名
                string SaveFileName = null;
                SaveFileName = Path.Combine(SavePath, Path.GetFileNameWithoutExtension(FileName) + "." + Extension);

                //フォーマットごとの処理
                switch (theFormat)
                {

                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    //case modImgProc.FormatType.FormatBMP:
                    //case modImgProc.FormatType.FormatJPG:
                    //case modImgProc.FormatType.FormatPCT:

                    //    //v9.5 コンパイルオプションによる分岐を追加 by 間々田 2004/09/17
//#if ImageProV3
//                        rc = Ipc32v5.IpWsConvertToRGB();													//ImagePro Ver3の場合
//#else
//                        rc = Ipc32v5.IpWsConvertImage(Ipc32v5.IMC_RGB, Ipc32v5.CONV_SCALE, 0, 0, 0, 0);		//ImagePro Ver5の場合
//#endif

//                        rc = Ipc32v5.IpWsSaveAs(SaveFileName, Extension);
//                        if (rc != 0) new Exception();
//                        break;

//                    case modImgProc.FormatType.FormatTIF8:

//                        //v9.5 コンパイルオプションによる分岐を追加 by 間々田 2004/09/17
//#if ImageProV3
//                        rc = Ipc32v5.IpWsConvertToGray();													//Image-Pro Ver.3 の場合
//#else
//                        rc = Ipc32v5.IpWsConvertImage(Ipc32v5.IMC_GRAY, Ipc32v5.CONV_SCALE, 0, 0, 0, 0);	//Image-Pro Ver.5 の場合
//#endif

//                        rc = Ipc32v5.IpWsSaveAs(SaveFileName, Extension);
//                        if (rc != 0) new Exception();
//                        break;

//                    case modImgProc.FormatType.FormatTIF16:

//                        rc = Ipc32v5.IpWsSaveAs(SaveFileName, Extension);
//                        if (rc != 0) new Exception();
//                        break;
                    //
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    case modImgProc.FormatType.FormatBMP:
                    case modImgProc.FormatType.FormatJPG:
                    case modImgProc.FormatType.FormatPCT:
                    case modImgProc.FormatType.FormatTIF8:
                    case modImgProc.FormatType.FormatTIF16:
                        rc = CallImageProFunction.CallFormatConvertStep6((int)theFormat, SaveFileName, Extension);
                        if (rc != 0) new Exception();
                        break;
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//


                    case modImgProc.FormatType.FormatRAW:

                        //汎用フォーマットによる保存(+8192加算あり、コントラスト調整なし）
                        rc = CTLib.ReversByte(FileName, SaveFileName);
                        if (rc != 2401) new Exception();
                        break;

                    //v29.99 今のところ不要なのでfalse by長野 2013/04/08'''''ここから'''''
                    //        Case FormatDICOM        'V6.0 append by 間々田 2002/08/07
                    //
                    //            Dim PatientComments     As String
                    //            Dim strPatientName      As String * 64
                    //            Dim strInstitutionName  As String * 64
                    //
                    //            '患者コメントの合成
                    //            PatientComments = ImgInf.scan_view & vbCrLf & _
                    //'                              ImgInf.integ_number & vbCrLf & _
                    //'                              ImgInf.full_mode & vbCrLf & _
                    //'                              txtComment.Text
                    //
                    //            'DICOM変換関数のコール(戻り値 正常:2401)
                    //            Mid(strPatientName, 1) = txtPatientName.Text
                    //            Mid(strInstitutionName, 1) = txtInstitutionName.Text
                    //            FormatConvert = DICOM_Transfer(FileName, FSO.BuildPath(SavePath, "\"), strPatientName, strInstitutionName, PatientComments)
                    //            Exit Function
                    //v29.99 今のところ不要なのでfalse by長野 2013/04/08'''''ここまで'''''
                }

                //戻り値セット
                functionReturnValue = 2401;										//正常終了
            }
            catch
            {
                if (functionReturnValue == 0) functionReturnValue = 2415;		//画像を保存できません。
            }

            return functionReturnValue;
        }

        #endregion



        #region ???????	ドライブの空き容量を調べる

        //********************************************************************************
        //機    能  ：  ???????
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //********************************************************************************
        private bool Sub_DiskFreeSapceCheck(string driveName)
        {
            int i = 0;
            int TransImgSize = 0;
            int TotalImgSize = 0;
            double w_lamp_size = 0;		//最低空き容量(単位:MB)   'V4.0 append by 鈴山 2001/03/07

            //戻り値初期化
            bool functionReturnValue = false;

            //選択画像ファイルサイズの合計
            TotalImgSize = 0;

            for (i = 0; i <= lstImgFile.ListCount - 1; i++)
            {
                //フォーマット変換後の画像サイズを算出
                //TransImgSize = fnc_TransImgSize(GetImgSize(.List(i)))
                //TransImgSize = GetTransImgSize(.List(i), cmbOutKind.text, chkInformation.Value = vbChecked)   'v11.3変更 by 間々田 2006/02/20
                TransImgSize = GetTransImgSize(lstImgFile.List(i), (modImgProc.FormatType)cmbOutKind.SelectedIndex, chkInformation.CheckState == CheckState.Checked);	//v11.3変更 by 間々田 2006/02/20

                if (TransImgSize == -1)
                {
                    //エラーメッセージ：
                    //   フォーマット変換する画像ファイルのサイズが不正です。
                    //   他の画像ファイルを選択してください。
                    modCT30K.ErrMessage(2411, Icon: MessageBoxIcon.Error);
                    return functionReturnValue;
                }

                //選択した画像ｻｲｽﾞを加算
                TotalImgSize = TotalImgSize + TransImgSize;
            }

            //選択したドライブを調べて最低空き容量を加味すべきか判断する     V4.0 append by 鈴山 2001/03/07
            w_lamp_size = (driveName.Substring(0, 1).ToUpper() == "C" ? modCT30K.HD_LIMIT : 0);
            //2014/11/06hata キャストの修正
            TotalImgSize =Convert.ToInt32(TotalImgSize + w_lamp_size * 1024);

            //Rev22.01/Rev23.00 UNCのパスもできるように変更 by長野 2015/09/14
            Uri pathUni = new Uri(driveName);
            if (pathUni.IsUnc)
            {
                string lpDirectoryName = driveName + "\\";
                ulong lpFreeBytesAvailable = 0;
                ulong lpTotalNumberOfBytes = 0;
                ulong lpNumberOfFreeBytes = 0;
                Winapi.GetDiskFreeSpaceEx(lpDirectoryName, ref lpFreeBytesAvailable, ref lpTotalNumberOfBytes, ref lpNumberOfFreeBytes);

                if ((Convert.ToInt64(lpFreeBytesAvailable / (double)1024)) <= TotalImgSize)
                {
                    modCT30K.ErrMessage(2403, Icon: MessageBoxIcon.Error);
                    return functionReturnValue;
                }
            }
            else
            {
                //change by 間々田 2003/11/18
                DriveInfo theDrive = null;
                theDrive = new DriveInfo(Directory.GetDirectoryRoot(driveName));

                //ﾃﾞｨｽｸ空き容量と選択画像ｻｲｽﾞのﾁｪｯｸ
                if ((theDrive.AvailableFreeSpace / 1024) <= TotalImgSize)
                {
                    modCT30K.ErrMessage(2403, Icon: MessageBoxIcon.Error);
                    return functionReturnValue;
                }
            }


            //正常終了
            functionReturnValue = true;
            return functionReturnValue;
        }

        #endregion

        //*******************************************************************************
        //機　　能： ビット数コンボボックスクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //           V14.00                         ビット数の選択をファイルの種類選択のコンボボックスで行うため削除
        //*******************************************************************************
        //Private Sub cboBit_Click()

        //added by　山本　2003-3-18  'tif16ビットの場合は階調変換の選択を可能とする
        //    chkChangeContrast.Visible = (cmbOutKind.text = ".tif") And (InStr(cboBit.text, "16") > 0)

        //End Sub
        //V14.00　削除 by 山影

        #region 付帯情報付きチェックボックスクリック時処理

        //*******************************************************************************
        //機　　能： 付帯情報付きチェックボックスクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void chkInformation_Click(object sender, EventArgs e)
        {
            if ((cmbOutKind.SelectedIndex == (int)modImgProc.FormatType.FormatRAW) &&
                (chkInformation.CheckState == CheckState.Checked))
            {
                //エラーメッセージ表示：汎用フォーマットでは付帯情報を付けられません。
                //変更2014/11/18hata_MessageBox確認
                //modCT30K.ErrMessage(9967, Icon: MessageBoxIcon.Exclamation);
                MessageBox.Show(CTResources.LoadResString(9967), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  

                chkInformation.CheckState = CheckState.Unchecked;
            }
        }

        #endregion
        #region スケール付きチェックボックスクリック時処理
        //*******************************************************************************
        //機　　能： スケール付きチェックボックスクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V20.00  15/02/04   (検S1)長野               新規作成
        //*******************************************************************************
        private void chkScale_Click(object sender, EventArgs e)
        {
            if ((cmbOutKind.SelectedIndex == (int)modImgProc.FormatType.FormatRAW) &&
                (chkScale.CheckState == CheckState.Checked))
            {
                //エラーメッセージ表示：汎用フォーマットではスケールを付けられません。
                //変更2014/11/18hata_MessageBox確認
                //modCT30K.ErrMessage(9967, Icon: MessageBoxIcon.Exclamation);
                MessageBox.Show(CTResources.LoadResString(9969), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                chkScale.CheckState = CheckState.Unchecked;
            }
        }
        #endregion
        #region ファイルの種類コンボボックスクリック時処理

        //*******************************************************************************
        //機　　能： ファイルの種類コンボボックスクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cmbOutKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            //「階調変換」チェックボックスは，tif16ビットの時だけ表示
            chkChangeContrast.Visible = (cmbOutKind.SelectedIndex == (int)modImgProc.FormatType.FormatTIF16);
            //v14.00 追加 by 山影 2007/07/17

            //v29.99 DICOMは今のところ不要なのでtrue by長野 2013/04/08'''''ここから'''''
            //「スケール付加」チェックボックス：DICOMの時は非表示。それ以外は表示。
            //chkScale.Visible = (.ListIndex <> FormatDICOM)              'v14.00 追加 by 山影 2007/07/24
            chkScale.Visible = true;
            //v29.99 DICOMは今のところ不要なのでtrue by長野 2013/04/08'''''ここから'''''

            //v29.99 今のところ不要なのでfalse by長野 2013/04/08'''''ここから'''''
            //「付帯情報付き」のチェックボックス：DICOMの時は非表示。それ以外は表示。
            //chkInformation.Visible = (.ListIndex <> FormatDICOM)
            chkInformation.Visible = true;
            //v29.99 今のところ不要なのでfalse by長野 2013/04/08'''''ここから'''''


            if (cmbOutKind.SelectedIndex == (int)modImgProc.FormatType.FormatRAW)
            {
                chkInformation.CheckState = CheckState.Unchecked;
            }

            //v29.99 DICOMは今のところ不要なのでfalse by長野 2013/04/08'''''ここから'''''
            //DICOM用フレーム
            //fraDICOM.Visible = (.ListIndex = FormatDICOM)
            fraDICOM.Visible = false;
            //v29.99 今のところ不要なのでfalse by長野 2013/04/08'''''ここまで'''''

            //v29.99 DICOMは今のところ不要なのでfalse by長野 2013/04/08'''''ここから'''''
            //DICOMの場合フォームの高さを調整する
            //Me.Height = IIf(.ListIndex = FormatDICOM, DICOMSize, NormalSize)
            this.Height = NormalSize;
            //v29.99 DICOMは今のところ不要なのでfalse by長野 2013/04/08'''''ここまで'''''
        }

        #endregion

        #region 参照ボタンクリック時処理

        //*******************************************************************************
        //機　　能： 参照ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cmdDirSelect_Click(object sender, EventArgs e)
        {
            //フォルダ参照ダイアログ表示
            string FolderName = modFileIO.GetFolderName(txtDirName.Text, lblOutDirName.Text);

            //ＯＫ選択時：選択したフォルダをテキストボックスに設定
            if (!string.IsNullOrEmpty(FolderName)) txtDirName.Text = FolderName;
        }

        #endregion

        #region 終了ボタンクリック時処理

        //*******************************************************************************
        //機　　能： 終了ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cmdExit_Click(object sender, EventArgs e)
        {
            //フォームのアンロード
            this.Close();
        }

        #endregion

        #region 実行ボタンクリック時処理

        //*******************************************************************************
        //機　　能： 実行ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cmdGo_Click(object sender, EventArgs e)
        {
            const int SUCCESS = 2401;

            //ビジー状態なら「停止」ボタンをクリックしたとみなし、処理を停止
            if (myBusy)
            {
                myCancel = true;
                return;
            }

            //フォーマット変換を実行できる状態か確認する
            if (!fnc_ready()) return;

            //設定値の保存
            modImgProc.SaveType = (modImgProc.FormatType)cmbOutKind.SelectedIndex;
            modImgProc.SaveInfo = chkInformation.CheckState;
            modImgProc.SaveScale = chkScale.CheckState;				//v14.00 追加 by 山影 2007/07/17
            modImgProc.SaveChkCont = (modImgProc.SaveType == modImgProc.FormatType.FormatTIF16 ? chkChangeContrast.CheckState : CheckState.Unchecked);	//v14.00 追加 by 山影 2007/07/17

            //ビジー状態とする
            IsBusy = true;

            //FormatConvert関数戻り値初期化
            int sts = SUCCESS;

            //フォーマット変換成功数
            int i = 0;

            //フォーマット変換を実行
            myCancel = false;
            while ((i < lstImgFile.ListCount) && (!myCancel))
            {
                //ステータスバーに途中経過を表示する
                //Rev20.00 StatusBarではなくtoolStripStatusLabelを使用する by長野 2015/01/29
                //StatusBar1.Text = StringTable.GetResString(StringTable.IDS_Ing, this.Text) + " " + Convert.ToString(i + 1) + "/" + Convert.ToString(lstImgFile.ListCount);
                toolStripStatusLabel1.Text = StringTable.GetResString(StringTable.IDS_Ing, this.Text) + " " + Convert.ToString(i + 1) + "/" + Convert.ToString(lstImgFile.ListCount);
                
                Application.DoEvents();

                //画像フォーマット変換処理
                sts = FormatConvert(lstImgFile.List(i), txtDirName.Text.Trim(), (modImgProc.FormatType)cmbOutKind.SelectedIndex);

                //エラーが発生しているならば抜ける
                if (sts != SUCCESS)
                {
                    myCancel = true;
                }
                else
                {
                    i = i + 1;  //カウントアップ
                }
            }

            //ステータスバーをクリア
            StatusBar1.Text = "";

            //ビジー解除
            IsBusy = false;

            //結果のメッセージを表示
            if (sts != SUCCESS)
            {
                modCT30K.ErrMessage(sts, Icon: MessageBoxIcon.Exclamation);
                //メッセージの表示：画像フォーマット変換処理に失敗しました。
                MessageBox.Show(StringTable.GetResString(StringTable.IDS_WentWrong, StringTable.GetResString(StringTable.IDS_Processing, this.Text)),
                                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (i < lstImgFile.ListCount)
            {
                //メッセージの表示：画像フォーマット変換が途中でキャンセルされました。
                MessageBox.Show(StringTable.GetResString(9907, StringTable.GetResString(StringTable.IDS_Processing, this.Text)),
                                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                //メッセージの表示：画像フォーマット変換が正常に終了しました。
                MessageBox.Show(StringTable.GetResString(StringTable.IDS_CompletedNormally, StringTable.GetResString(StringTable.IDS_Processing, this.Text)),
                                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //実行時の保存フォルダを記憶 by 間々田 2005/02/10
            modFileIO.SaveDefaultFolder(this.Text, txtDirName.Text.Trim());
        }

        #endregion


        // V6.0 append by 間々田 2002/10/02 START
        private void txtInstitutionName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar > 127)
            {
                e.KeyChar = (char)0;
                e.Handled = true;
            }
            else if (e.KeyChar == (char)Keys.Escape ||
                     e.KeyChar == (char)Keys.Back)
            { }
            else if ((int)e.KeyChar == 0x5C)		//バックスラッシュは不可
            {
                e.KeyChar = (char)0;
                e.Handled = true;
            }
            else if ((int)e.KeyChar < 32)
            {
                e.KeyChar = (char)0;
                e.Handled = true;
            }
        }
        // V6.0 append by 間々田 2002/10/02 END


        // V6.0 append by 間々田 2002/08/07 START
        private void txtPatientName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar > 127)
            {
                e.KeyChar = (char)0;
                e.Handled = true;
            }
            else if (e.KeyChar == (char)Keys.Escape ||
                     e.KeyChar == (char)Keys.Back)
            { }
            else if ((int)e.KeyChar == 0x5C)		//バックスラッシュは不可
            {
                e.KeyChar = (char)0;
                e.Handled = true;
            }
            else if ((int)e.KeyChar < 32)
            {
                e.KeyChar = (char)0;
                e.Handled = true;
            }
        }
        // V6.0 append by 間々田 2002/08/07 END


        // V6.0 append by 間々田 2002/08/07 START
        private void txtComment_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar > 127)
            {
                e.KeyChar = (char)0;
                e.Handled = true;
            }
            else if (e.KeyChar == (char)Keys.Escape ||
                     e.KeyChar == (char)Keys.Back ||
                     e.KeyChar == (char)Keys.Return ||
                     e.KeyChar == (char)10 ||
                     e.KeyChar == (char)Keys.Clear)
            { }
            else if ((int)e.KeyChar == 0x5C)		//バックスラッシュは不可
            {
                e.KeyChar = (char)0;
                e.Handled = true;
            }
            else if ((int)e.KeyChar < 32)
            {
                e.KeyChar = (char)0;
                e.Handled = true;
            }
        }


        #region フォームリサイズ時の処理

        //*******************************************************************************
        //機　　能： フォームリサイズ時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*******************************************************************************
        private void frmFormatTransfer_Resize(object sender, EventArgs e)
        {
            //「実行」ボタンと「終了」ボタンの配置
            cmdGo.Top = ClientRectangle.Height - cmdGo.Height - StatusBar1.Height - 8;
            cmdExit.Top = cmdGo.Top;
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
        private void frmFormatTransfer_Load(object sender, EventArgs e)
        {
            //変数初期化
            myCancel = false;

            //実行時はフラグをセット
            modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTFormatTransfer;

            //フォームの位置
            modCT30K.SetPosNormalForm(this);

            //イメージプロ起動
            modCT30K.StartImagePro();

            //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);

            //各コントロールの初期化
            InitControls();
        }

        #endregion

        #region フォームアンロード時の処理

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
        private void frmFormatTransfer_FormClosed(object sender, FormClosedEventArgs e)
        {
            //終了時はフラグをリセット
            modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTFormatTransfer);
        }

        #endregion

        #region 各コントロールの初期化

        //*******************************************************************************
        //機　　能： 各コントロールの初期化
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*******************************************************************************
        private void InitControls()
        {
            //リストコントロールボックス

            //項目削除用のボタンの設定
            lstImgFile.DeleteButton = cmdImgDelete;

            //リンクする「参照」ボタンの設定
            lstImgFile.ReferenceButton = cmdImgSelect;

            //項目数表示ラベルの設定
            lstImgFile.CountLabel = lblCount;

            //このリストの内容：画像ファイル
            lstImgFile.Description = CTResources.LoadResString(StringTable.IDS_CTImage);

            //最大数を変更 1000→9999
            //Rev21.00 9999→32767 by長野 2015/03/20 
            lstImgFile.Max = 32767;		//v16.20追加 byやまおか 2010/04/21

            //最大表示枚数の表示
            lblMaxNum.Text = StringTable.GetResString(StringTable.IDS_FramesWithMax, lstImgFile.Max.ToString());

            //DICOM変換ありの場合
            //If OptDICOMFlag Then
            //コモン初期化した直後はgetMACaddress()を実行していないためDICOM変換を表示しない
            //If OptDICOMFlag And (RemoveNull(t20kinf.macaddress) <> "") Then 'v17.40変更 byやまおか 2010/10/26
            //    cmbOutKind.AddItem "DICOM"
            //End If
            //条件が間違っていた     'v19.02修正 byやまおか 2012/07/09
            //macaddressが空のときはgetMACaddressしてからDICOM項目を追加する
            if (modOPTION.OptDICOMFlag)
            {
                if ((!string.IsNullOrEmpty(modLibrary.RemoveNull(CTSettings.t20kinf.Data.macaddress.GetString()))))
                {
                    ComLib.getMACaddress();
                }
                cmbOutKind.Items.Add("DICOM");
            }

            //DICOMフレームの位置
            fraDICOM.Top = lblOutKind.Top + lblOutKind.Height;

            //デフォルト値の設定
            cmbOutKind.SelectedIndex = (int)modImgProc.SaveType;		//保存ファイルの種類
            chkInformation.CheckState = modImgProc.SaveInfo;			//「付帯情報付き」チェックボックス
            chkScale.CheckState = modImgProc.SaveScale;					//「スケール付加」チェックボックス   v14.00 追加 by 山影 2007/07/17
            chkChangeContrast.CheckState = modImgProc.SaveChkCont;		//「階調変換」チェックボックス       v14.00 追加 by 山影 2007/07/17
            txtDirName.Text = modFileIO.GetDefaultFolder(this.Text);	//保存先フォルダ
            optFolder.Checked = true;									//フォルダ指定       'v17.02追加 byやまおか 2010/07/23
        }

        #endregion

        #region 「指定」フレーム内「ファイル」オプションボタンクリック時処理（イベント処理）

        //*******************************************************************************
        //機　　能： 「指定」フレーム内「ファイル」オプションボタンクリック時処理（イベント処理）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*******************************************************************************
        private void optFile_CheckedChanged(object sender, EventArgs e)
        {
            if (optFile.Checked)
            {
                //「参照...」ボタンクリック時にファイル選択ダイアログを表示するように設定する
                lstImgFile.HowToAdd = CTListBox.HowToAddType.FromFileList;
            }
        }
        #endregion

        #region 「指定」フレーム内「フォルダ」オプションボタンクリック時処理（イベント処理）

        //*******************************************************************************
        //機　　能： 「指定」フレーム内「フォルダ」オプションボタンクリック時処理（イベント処理）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*******************************************************************************
        private void optFolder_CheckedChanged(object sender, EventArgs e)
        {
            if (optFolder.Checked)
            {
                //「参照...」ボタンクリック時にフォルダ選択ダイアログを表示するように設定する
                lstImgFile.HowToAdd = CTListBox.HowToAddType.FromInFolder;
            }
        }

	    #endregion

    }
}
