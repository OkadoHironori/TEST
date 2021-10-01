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
	public partial class frmAllwlww : Form
	{
        private char presskye = (char)0;  //Keypressの値          //追加2014/12/19hata

		private static frmAllwlww _Instance = null;

		public frmAllwlww()
		{
			InitializeComponent();
		}

		public static frmAllwlww Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmAllwlww();
				}

				return _Instance;
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
		private void frmAllwlww_Load(object sender, EventArgs e)
		{
			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);

			//v17.60 英語用レイアウト調整 by長野 2011/05/25
			if (modCT30K.IsEnglish)
			{
				EnglishAdjustLayout();
			}

			//各コントロールの初期化
			InitControls();
		}
		

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
            //画像拡張子
            lstImgFile.Extension = ".img";

			//最大表示枚数の表示
			//lstImgFile.Max = 9999;
            //Rev21.00/20.00 9999→32767 by長野 2015/03/20
            lstImgFile.Max = 32767;

			//v17.60追加 byやまおか 2011/06/07
			lblMaxNum.Text = StringTable.GetResString(StringTable.IDS_FramesWithMax, lstImgFile.Max.ToString());

            //変更2015/02/02hata
            //cwneWL.Value = frmImageControl.Instance.cwneWindowLevel.Value;		//v17.02追加 byやまおか 2010/07/22
            //cwneWW.Value = frmImageControl.Instance.cwneWindowWidth.Value;		//v17.02追加 byやまおか 2010/07/22
            //cwneGamma.Value = frmImageControl.Instance.cwneGamma.Value;			//v19.00追加 by長野     2012/05/02
            cwneWL.Value = modLibrary.CorrectInRange(frmImageControl.Instance.cwneWindowLevel.Value, cwneWL.Minimum, cwneWL.Maximum);
            cwneWW.Value = modLibrary.CorrectInRange(frmImageControl.Instance.cwneWindowWidth.Value, cwneWW.Minimum, cwneWW.Maximum);
            cwneGamma.Value = modLibrary.CorrectInRange(frmImageControl.Instance.cwneGamma.Value, cwneGamma.Minimum, cwneGamma.Maximum);
            
            //追加2015/02/03hata_dNet
            txtWL.Text = cwneWL.Value.ToString();
            txtWW.Text = cwneWW.Value.ToString();
            txtWL.Width = cwneWL.Right - txtWL.Left + 1;
            txtWW.Width = cwneWW.Right - txtWW.Left + 1;


           
            optFolder.Checked = true;											//v17.02追加 byやまおか 2010/07/23
		}


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
		private void cmdEnd_Click(object sender, EventArgs e)
		{
			//このフォームのアンロード
			this.Close();
            //Rev20.00 dispose 追加 by長野 2015/02/24
            this.Dispose();
		}


		//*******************************************************************************
		//機　　能： リストボックス変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.3  2006/02/22 (SI3)間々田   新規作成
		//*******************************************************************************
		private void lstImgFile_Changed(object sender, EventArgs e)
		{
			//総数が１以上の時にのみ実行ボタンを使用可能にする
			cmdOK.Enabled = (lstImgFile.ListCount > 0);
		}


		//*******************************************************************************
		//機　　能： 「指定」フレーム内「ファイル」オプションボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void optFile_Click(object sender, EventArgs e)
        {

        }
        private void optFile_CheckedChanged(object sender, EventArgs e)
        {
            if (optFile.Checked)
            {
                //「参照...」ボタンクリック時にファイル選択ダイアログを表示するように設定する
                lstImgFile.HowToAdd = CTListBox.HowToAddType.FromFileList;
            }
        }


		//*******************************************************************************
		//機　　能： 「指定」フレーム内「フォルダ」オプションボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void optFolder_Click(object sender, EventArgs e)
        {

        }
        private void optFolder_CheckedChanged(object sender, EventArgs e)
        {
            if (optFolder.Checked)
            {
                //「参照...」ボタンクリック時にフォルダ選択ダイアログを表示するように設定する
                lstImgFile.HowToAdd = CTListBox.HowToAddType.FromInFolder;
            }
        }

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
		private void cmdOK_Click(object sender, EventArgs e)
		{
			//画像ファイルが未指定の場合                             'v10.01追加 by 間々田 2005/03/03
			if (lstImgFile.ListCount < 1)
			{
				//メッセージ表示：
				//   画像ファイルが１つも指定されていません。
				//   画像ファイルを指定してから再度実行してください。
				MessageBox.Show(StringTable.GetResString(9333, CTResources.LoadResString(StringTable.IDS_CTImage)), 
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			//階調一括変換を実行する
			else if (AllWLWW())
			{
				//メッセージ表示：階調一括変換処理が正常に終了しました。
				MessageBox.Show(StringTable.GetResString(StringTable.IDS_CompletedNormally, StringTable.GetResString(StringTable.IDS_Processing, CTResources.LoadResString(10513))), 
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				//メッセージ表示：階調一括変換処理に失敗しました。
				MessageBox.Show(StringTable.GetResString(StringTable.IDS_WentWrong, StringTable.GetResString(StringTable.IDS_Processing, CTResources.LoadResString(10513))), 
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}


		//********************************************************************************
		//機    能： 階調一括変換を実行する
		//             変数名         [I/O] 型        内容
		//引    数： なし
		//戻 り 値：                  [ /O] Boolean   結果(True:正常, False:異常)
		//補    足： なし
		//
		//履    歴： V4.0   01/02/13  (SI1)鈴山       新規作成
		//           v7.0   03/07/29  (SI4)間々田     コードの整理
		//           V9.0 2004/05/17  (SI4)間々田     不要なコモンの書き換えをやめた。付帯情報には直接アクセスすることにした。
		//********************************************************************************
		private bool AllWLWW()
		{
			//modImageInfo.ImageInfoStruct theInfo = default(modImageInfo.ImageInfoStruct);			//付帯情報構造体
            ImageInfo theInfo = new ImageInfo();

            theInfo.Data.Initialize();

            string FileName = null;
			int i = 0;

			//戻り値の初期化
			bool functionReturnValue = false;

			//エラー時の扱い
            try
            {
                //マウスカーソルを砂時計にする
                Cursor.Current = Cursors.WaitCursor;

                //画面操作を禁止する
                this.Enabled = false;
                cmdOK.Enabled = false;

                //進捗ラベルを表示する   'v17.60追加 byやまおか 2011/06/08
                lblProcess.Text = "";
                lblProcess.Visible = true;

                //階調一括変換
                for (i = 0; i <= lstImgFile.ListCount - 1; i++)
                {
                    //付帯情報ファイル名の取得
                    FileName = modLibrary.RemoveExtension(lstImgFile.List(i), ".img");

                    //付帯情報ファイルの読み込む
                    //if (modImageInfo.ReadImageInfo(ref theInfo, FileName))
                    if (ImageInfo.ReadImageInfo(ref theInfo.Data, FileName))
                    {
                        ////設定されている値をセット
                        //modLibrary.SetField(Convert.ToString(cwneWL.Value), ref theInfo.WL);			//ウィンドウレベル
                        //modLibrary.SetField(Convert.ToString(cwneWW.Value), ref theInfo.ww);			//ウィンドウ幅
                        ////v19.00 ガンマを追加 by長野 2012/02/21
                        //modLibrary.SetField(Convert.ToString(cwneGamma.Value), ref theInfo.gamma);		//ガンマ
                        theInfo.Data.wl.SetString(Convert.ToString(cwneWL.Value));
                        theInfo.Data.ww.SetString(Convert.ToString(cwneWW.Value));
                        theInfo.Data.gamma.SetString(Convert.ToString(cwneGamma.Value));

                        //付帯情報ファイルの書き込み
                        //modImageInfo.WriteImageInfo(ref theInfo, FileName);
                        ImageInfo.WriteImageInfo(ref theInfo.Data, FileName, ".inf");

                        //進捗を表示     'v17.60追加 byやまおか 2011/06/08
                        lblProcess.Text = Convert.ToString(i + 1) + "/" + Convert.ToString(lstImgFile.ListCount);
                        Application.DoEvents();
                    }
                    //読み込み失敗のときは抜ける     'v17.60追加 byやまおか 2011/06/08
                    else
                    {
                        throw new Exception();
                    }
                }
                //戻り値（正常）
                functionReturnValue = true;
            }
            catch
            { 
            }
            finally
			{
				//進捗ラベルを隠す       'v17.60追加 byやまおか 2011/06/08
				lblProcess.Visible = false;

				//マウスカーソルを元に戻す
				Cursor.Current = Cursors.Default;

				//画面操作を許可する
				this.Enabled = true;
				cmdOK.Enabled = true;
			}

			return functionReturnValue;
		}

// 【C#コントロールで代用】
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*

		'*******************************************************************************
		'機　　能： ウィンドウレベル値入力時のエラー処理
		'
		'           変数名          [I/O] 型        内容
		'引　　数： なし
		'戻 り 値： なし
		'
		'補　　足： なし
		'
		'履　　歴： V1.00  99/XX/XX   ????????      新規作成
		'*******************************************************************************
		Private Sub cwneWL_Error(Number As Integer, Description As String, Scode As Long, Source As String, HelpFile As String, HelpContext As Long, CancelDisplay As Boolean)
    
			'コンポーネントワークス側のメッセージを表示しないようにする
			CancelDisplay = True

		End Sub

		'*******************************************************************************
		'機　　能： ウィンドウ幅値入力時のエラー処理
		'
		'           変数名          [I/O] 型        内容
		'引　　数： なし
		'戻 り 値： なし
		'
		'補　　足： なし
		'
		'履　　歴： V1.00  99/XX/XX   ????????      新規作成
		'*******************************************************************************
		Private Sub cwneWW_Error(Number As Integer, Description As String, Scode As Long, Source As String, HelpFile As String, HelpContext As Long, CancelDisplay As Boolean)

			'コンポーネントワークス側のメッセージを表示しないようにする
			CancelDisplay = True

		End Sub

*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版


		//*******************************************************************************
		//機　　能： 英語用レイアウト調整
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v17.60  2011/05/25 (検S1)長野  新規作成
		//*******************************************************************************
		private void EnglishAdjustLayout()
		{
			int margin;

			margin = 1;
			lblMaxNum.Left = 333;
			lblCount.Left = lblMaxNum.Left - lblCount.Width - margin;
			lblNumTitle.Left = lblCount.Left - lblNumTitle.Width - margin;
		}

        //追加2014/12/15hata
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
        private void cwneWL_ValueChanged(object sender, EventArgs e)
        {
            txtWL.Text = cwneWL.Value.ToString();
        }

        //追加2014/12/15hata
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
        private void cwneWW_ValueChanged(object sender, EventArgs e)
        {
            txtWW.Text = cwneWW.Value.ToString();
        }


        //追加2015/02/03hata
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
                    e.Handled = true;
                    break;
            }

        }

        //追加2015/02/03hata
        private void txtWLWW_TextChanged(object sender, EventArgs e)
        {
            int dval = 0;

            //Keys.Returnのときだけ反映する
            if (sender.Equals(txtWL))
            {
                dval = 0;
                if (!int.TryParse(txtWL.Text, out dval))
                {
                    //追加2015/01/21hata
                    //間に(-)がある場合は(-)を消す
                    int pos = txtWL.Text.LastIndexOf("-");
                    if (pos > 0)
                    {
                        //(-)を消す
                        string text = txtWL.Text.Remove(pos, 1);
                        if (string.IsNullOrEmpty(txtWL.Text)) text = cwneWL.Value.ToString();
                        txtWL.Text = text;
                    }
                    presskye = (char)0;
                    return;
                }
                if ((int)cwneWL.Maximum < Convert.ToInt32(txtWL.Text))
                {
                    presskye = (char)0;
                    txtWL.Text = cwneWL.Maximum.ToString();
                    return;
                }
                if ((int)cwneWL.Minimum > Convert.ToInt32(txtWL.Text))
                {
                    presskye = (char)0;
                    txtWL.Text = cwneWL.Minimum.ToString();
                    return;
                }
                if (presskye == (char)Keys.Return)
                {
                    cwneWL.Value = dval;
                }
            }
            else if (sender.Equals(txtWW))
            {
                dval = 1;
                if (!int.TryParse(txtWW.Text, out dval))
                {
                    presskye = (char)0;
                    return;
                }
                if ((int)cwneWW.Maximum < Convert.ToInt32(txtWW.Text))
                {
                    presskye = (char)0;
                    txtWW.Text = cwneWW.Maximum.ToString();
                    return;
                }
                if ((int)cwneWW.Minimum > Convert.ToInt32(txtWW.Text))
                {
                    presskye = (char)0;
                    txtWW.Text = cwneWW.Minimum.ToString();
                    return;
                }
                if (presskye == (char)Keys.Return)
                {
                    cwneWW.Value = dval;
                }
            }
            presskye = (char)0;
        }

        //追加2015/02/03hata
        private void txtWLWW_Leave(object sender, EventArgs e)
        {
            if (sender.Equals(txtWL))
            {
                if (txtWL.Text == "") txtWL.Text = cwneWL.Value.ToString();
                if ((txtWL.Text == "-0") | (txtWL.Text == "-")) txtWL.Text = "0";
            }
            else if (sender.Equals(txtWW))
            {
                if (txtWW.Text == "") txtWW.Text = cwneWW.Value.ToString();
            }
            presskye = (char)Keys.Return;
            txtWLWW_TextChanged(sender, EventArgs.Empty);
        }

        //Rev21.00 追加 by長野 2015/02/24
        private void frmAllwlww_FormClosed(object sender, FormClosedEventArgs e)
        {
            //このフォームのアンロード
            this.Close();
            //Rev20.00 dispose 追加 by長野 2015/02/24
            this.Dispose();

        }

        ////追加2015/02/03hata
        //private void txtWLWW_KeyDown(object sender, KeyEventArgs e)
        //{
        //    presskye = (char)e.KeyCode;
        //    switch (e.KeyCode)
        //    {
        //        //UpDownキー
        //        case Keys.Up:
        //            if (sender.Equals(txtWL))
        //            {
        //                cwneWL.UpButton();
        //            }
        //            else if (sender.Equals(txtWW))
        //            {
        //                cwneWW.UpButton();
        //            }
        //            break;
        //        case Keys.Down:
        //            if (sender.Equals(txtWL))
        //            {
        //                cwneWL.DownButton();
        //            }
        //            else if (sender.Equals(txtWW))
        //            {
        //                cwneWW.DownButton();
        //            }
        //            break;
        //    }
        //}


	}
}
