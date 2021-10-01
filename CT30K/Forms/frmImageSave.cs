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

namespace CT30K
{
    ///* ************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
    ///* 客先　　　　： ?????? 殿                                                   */
    ///* プログラム名： 画像結果保存.frm                                            */
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
    ///*                                                                            */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
    ///* ************************************************************************** */
    public partial class frmImageSave : Form
    {
        //付加的な拡張子
        string myKeyWord;

        //このダイアログでクリックしたボタン
        DialogResult Result;

        #region インスタンスを返すプロパティ

        // frmImageSaveのインスタンス
        private static frmImageSave _Instance = null;

        /// <summary>
        /// frmImageSaveのインスタンスを返す
        /// </summary>
        public static frmImageSave Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmImageSave();
                }

                return _Instance;
            }
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmImageSave()
        {
            InitializeComponent();
        }

        #endregion

        //*************************************************************************************************
        //機　　能： >>（参照）ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void cmdRef_Click(object sender, EventArgs e)
        {
            //ファイル選択ダイアログ表示
            string FileName = modFileIO.GetFileName(Operation: StringTable.IDS_FileSpecify, 
                                                    Description: CTResources.LoadResString(StringTable.IDS_CTImage), 
                                                    Extension: ".img", 
                                                    InitFileName: txtFileName.Text, 
                                                    Purpose: CTResources.LoadResString(StringTable.IDS_Destination));

            if (string.IsNullOrEmpty(FileName))
            {
				return;
            }

            // ※ペンディング 要確認
            string fileNameOnly = Path.GetFileNameWithoutExtension(FileName);
            bool included = false;
            if (0 <= fileNameOnly.IndexOf(myKeyWord))
            {
                included = true;
            }
            txtFileName.Text = GetSaveFileName(FileName, included);
        }

        //'*************************************************************************************************
        //'機　　能： 画像処理後の保存用ファイル名を生成する
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //'*************************************************************************************************
        private string GetSaveFileName(string FileName, bool OverwriteCheck = false)
        {
            //最初の候補
            string dirName = Directory.GetParent(FileName).FullName;
            string fileNameOnly = Path.GetFileNameWithoutExtension(FileName);

            string fileName = modLibrary.AddExtension(fileNameOnly, myKeyWord) + ".img";
            
            string Result = Path.Combine(dirName, fileName);

			//その候補が存在して、かつ上書きチェックありの場合
			if (OverwriteCheck && File.Exists(Result))
            {
				for (int i = 1; i <= 999; i++)
                {
                    string eachDirName = Directory.GetParent(FileName).FullName;
                    string eachFileNameOnly = Path.GetFileNameWithoutExtension(FileName);

                    string eachFileName = modLibrary.AddExtension(eachFileNameOnly, myKeyWord + Convert.ToString(i)) + ".img";

                    Result = Path.Combine(eachDirName, eachFileName);

                    if (!File.Exists(Result))
                    {
						break;
                    }
				}
			}

			return Result;
        }

        //'*************************************************************************************************
        //'機　　能： 「はい」ボタンクリック時処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v1.00  99/XX/XX   ????????      新規作成
        //'*************************************************************************************************
        private void cmdYes_Click(object sender, EventArgs e)
        {
			string FileName = txtFileName.Text.Trim();

			//指定したファイルが既に存在する場合
			if (File.Exists(FileName))
            {
				//確認ダイアログ表示:～は既に存します。上書きしますか？
                if (MessageBox.Show(StringTable.GetResString(9915, FileName), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    return;
                }
			}

			Result = DialogResult.Yes;

			//非表示にする
			//変更2015/1/17hata_非表示のときにちらつくため
            //this.Hide();
            modCT30K.FormHide(this);

        }

        //'*************************************************************************************************
        //'機　　能： 「いいえ」ボタンクリック時処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v1.00  99/XX/XX   ????????      新規作成
        //'*************************************************************************************************
        private void cmdNo_Click(object sender, EventArgs e)
        {
            Result = DialogResult.No;

			//非表示にする
			//変更2015/1/17hata_非表示のときにちらつくため
            //this.Hide();
            modCT30K.FormHide(this);

        }

        //'*************************************************************************************************
        //'機　　能： キャンセルボタンクリック時処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //'*************************************************************************************************
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Cancel;   //v15.10追加 byやまおか 2009/12/01
            
			//非表示にする
			//変更2015/1/17hata_非表示のときにちらつくため
            //this.Hide();
            modCT30K.FormHide(this);

        }

        //'*************************************************************************************************
        //'機　　能： フォームロード時の処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v1.00  99/XX/XX   ????????      新規作成
        //'*************************************************************************************************
        private void Form_Load(object sender, EventArgs e)
        {
            //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);
        }

        //'*************************************************************************************************
        //'機　　能： ダイアログ処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v1.00  99/XX/XX   ????????      新規作成
        //'*************************************************************************************************
        public bool Dialog(string FileName, string KeyWord)
		{
			//戻り値初期化
			bool functionReturnValue = false;

			//変数初期化
			Result = DialogResult.Cancel;

			//付加的な拡張子
			myKeyWord = KeyWord;

			//初期ファイル名
			txtFileName.Text = GetSaveFileName(FileName, true);

			//モーダルで表示
            //変更2014/12/22hata_dNet_オーナーフォームを指定する
            //this.ShowDialog();
            this.ShowDialog(frmCTMenu.Instance);

			//クリックしたボタンによる分岐
			//modImageInfo.ImageInfoStruct InfRec = default(modImageInfo.ImageInfoStruct);
            ImageInfo InfRec = new ImageInfo();
            InfRec.Data.Initialize();
            
            switch (Result)
            {
				//画像を保存する場合
                case DialogResult.Yes:
					//結果保存に必要な付帯情報の取得
                    //if (!modImageInfo.ReadImageInfo(InfRec, modLibrary.RemoveExtension(FileName, ".img")))
                    if (!ImageInfo.ReadImageInfo(ref InfRec.Data, modLibrary.RemoveExtension(FileName, ".img")))
                    {
						//失敗した場合：メッセージ表示
						//MsgBox "結果保存に必要な付帯情報を取得できませんでした。", vbCritical
                        MessageBox.Show(CTResources.LoadResString(20019), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);   //ストリングテーブル化 'v17.60 by 長野 2011/05/22
					}
                    //保存処理
                    else if (SaveResult(txtFileName.Text.Trim(), ref InfRec))
                    {
						//表示
						frmScanImage.Instance.Target = txtFileName.Text.Trim();
						functionReturnValue = true;
					}
					break;

				//画像を保存しない場合
                case DialogResult.No:
					//画像を元に戻す（階調も元に戻す）
					frmScanImage.Instance.DispOrgImage();

					functionReturnValue = true;
					break;

				//キャンセルした場合
                case DialogResult.Cancel:   //v15.10追加 byやまおか 2009/12/01
					switch (frmScanImage.Instance.ImageProc)
                    {
						//ROIを描く必要があるときはROIメッセージを表示する    'v15.10追加 byやまおか 2009/12/01
						case frmScanImage.ImageProcType.roiEnlarge:
						case frmScanImage.ImageProcType.roiProcessing:
						case frmScanImage.ImageProcType.roiProfile:
						case frmScanImage.ImageProcType.roiDistance:
						case frmScanImage.ImageProcType.RoiProfileDistance:
						case frmScanImage.ImageProcType.roiHistgram:
							//ROIメッセージを表示する
							//frmRoiMessage.Show   'v15.10追加 byやまおか 2009/12/01
                            //追加2015/01/24hata_if文追加
                            if (!modLibrary.IsExistForm("frmRoiMessage"))
                            {
                                frmRoiMessage.Instance.Show(frmCTMenu.Instance);//v16.30/v17.00変更 byやまおか 2010/02/24
                            }
                            else
                            {
                                frmRoiMessage.Instance.WindowState = FormWindowState.Normal;
                                frmRoiMessage.Instance.Visible = true;
                            }

							break;
                        default:
                            break;
					}
					break;

                default:
                    break;
			}

			//結果保存ダイアログアンロード
			this.Close();
			return functionReturnValue;
		}

        //*************************************************************************************************
        //機　　能： 保存処理
        //
        //           変数名          [I/O] 型                内容
        //引　　数： FileName        [I/ ] String            結果ファイル名
        //           InfRec          [I/ ] ImageInfoStruct   保存時に使用する付帯情報
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private bool SaveResult(string FileName, ref ImageInfo InfRec)
		{
			//戻り値初期化
			bool functionReturnValue = false;
            int Matrix = 0; //マトリクス
            int rc = 0;
            short[] ImgBuff = null;

            try
            {
                //処理結果画像ファイルを保存
                //'v19.16 'FileCopyで作成した画像ファイルの作成時刻が現在の時刻にならない対策 by長野 2013/07/22
                //FileCopyを使わずにVCの関数を使ってファイルを作成する。バックアップ用に使用している
                //FileCopy関数は修正の必要はない。

                //File.Copy(AppValue.OTEMPIMG, FileName);
                
                //入力画像のサイズからマトリクスを判定する
                //Matrix = System.Math.Sqrt(FileSystem.FileLen(AppValue.OTEMPIMG) / 2);
                FileInfo finfo = new FileInfo(AppValue.OTEMPIMG);
                //2014/11/06hata キャストの修正
                Matrix = Convert.ToInt32(System.Math.Sqrt(finfo.Length / 2F));

                //画像ファイルの取り込み
                ImgBuff = new short[Matrix * Matrix * 2 + 1];
                rc = ScanCorrect.ImageOpen(ref ImgBuff[0], AppValue.OTEMPIMG, Matrix, Matrix);
                rc = ScanCorrect.ImageSave(ref ImgBuff[0], FileName, Matrix, Matrix);

                //処理結果画像の付帯情報の作成
                //modLibrary.SetField(Convert.ToString(frmScanImage.Instance.PicWidth), ref InfRec.matsiz);   //マトリクスサイズ
                //modLibrary.SetField(Convert.ToString(modDispinf.dispinf.level), ref InfRec.WL);             //ウィンドウレベル
                //modLibrary.SetField(Convert.ToString(modDispinf.dispinf.Width), ref InfRec.ww);             //ウィンドウ幅
                InfRec.Data.matsiz.SetString(Convert.ToString(frmScanImage.Instance.PicWidth));
                InfRec.Data.wl.SetString(Convert.ToString(CTSettings.dispinf.Data.level));
                InfRec.Data.ww.SetString(Convert.ToString(CTSettings.dispinf.Data.width));

                //単純拡大の時スケール値を付帯情報に書き込む
                if (myKeyWord == CTResources.LoadResString(10703))
                {
                    float result = 0;
                    float.TryParse(InfRec.Data.scale.GetString(), out result);

                    //modLibrary.SetField((result * modImgProc.EnlargeRatio).ToString("00000.00"), ref InfRec.scale);
                    InfRec.Data.scale.SetString((result * modImgProc.EnlargeRatio).ToString("00000.00"));

                }

                //付帯情報を保存：戻り値もセット
                functionReturnValue = ImageInfo.WriteImageInfo(ref InfRec.Data, modLibrary.RemoveExtension(FileName, ".img"));
            }
            catch (Exception exp)
            {
                //エラー時の扱い
                MessageBox.Show(exp.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

			return functionReturnValue;
		}
    }
}
