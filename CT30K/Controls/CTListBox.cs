using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace CT30K
{
 	[ToolboxBitmap(typeof(CTListBox),"CTListBox.bmp")]
   public partial class CTListBox : UserControl
    {
		// ShapeLabel の注意事項:

		// 1) このコードの大部分は、ActiveX コントロールインターフェイスウィザードを使用して、生成しました。
		//    手作業でｺｰﾄﾞを追加した部分および変更した部分にはｺﾒﾝﾄをつけてあります。
		// 2) UserControl の ForwardFocus ﾌﾟﾛﾊﾟﾃｨはTrue に設定されているので､通常の Labelｺﾝﾄﾛｰﾙと同様なﾌｫｰｶｽ処理が行われます。

		// ﾌﾟﾛﾊﾟﾃｨ値用の記憶領域。ShapeLabel の大部分のﾌﾟﾛﾊﾟﾃｨ値は、UserControl のﾌﾟﾛﾊﾟﾃｨまたはUserControl を構成するｺﾝﾄﾛｰﾙのﾌﾟﾛﾊﾟﾃｨに格納されています。

		//イベント宣言｡
		[Description("ﾕｰｻﾞｰがｵﾌﾞｼﾞｪｸﾄ上でﾏｳｽ ﾎﾞﾀﾝを押したときと離したときに発生します。")]
		public new event EventHandler Click;		//MappingInfo=UserControl,UserControl,-1,Click
		public event EventHandler Changed;

		private int myNumberingDigits = 0;

		//リストボックスの項目数の最大値
		private int myMax = 0;

		//対応する付帯情報の存在をチェックするか
		private bool myInfCheck = false;

		//ファイル拡張子
		private string myExtension = string.Empty;

		//参照時のデフォルトパス
		private string myDefaultPath = string.Empty;

		//リストボックスの項目に関する記述
		private string myDescription = string.Empty;

		//このリストボックスとリンクしている「削除」ボタン
		private Button _myDeleteButton = null;
		private Button myDeleteButton
		{
			get { return _myDeleteButton; }
			set
			{
				if (_myDeleteButton != null)
				{
					_myDeleteButton.Click -= myDeleteButton_Click;
				}

				_myDeleteButton = value;

				if (_myDeleteButton != null)
				{
					_myDeleteButton.Click += myDeleteButton_Click;
				}
			}
		}

		//このリストボックスとリンクしている「参照」ボタン
		private Button _myReferenceButton = null;
		private Button myReferenceButton
		{
			get { return _myReferenceButton; }
			set
			{
				if (_myReferenceButton != null)
				{
					_myReferenceButton.Click -= myReferenceButton_Click;
				}

				_myReferenceButton = value;

				if (_myReferenceButton != null)
				{
					_myReferenceButton.Click += myReferenceButton_Click;
				}
			}
		}

		//このリストボックスとリンクしているリスト数を表示するラベル
		private Label myCountLabel = null;

		//リンクしている「参照」ボタンクリック時の追加方法の定義
		public enum HowToAddType
		{
			FromFileList,			//ファイル選択ダイアログから
			FromInFolder			//フォルダ選択ダイアログから
		}

		//リンクしている「参照」ボタンクリック時の追加方法
		private HowToAddType myHowToAdd = HowToAddType.FromFileList;

		/// <summary>
		/// 
		/// </summary>
		public CTListBox()
		{
			InitializeComponent();
		}

		//*******************************************************************************
		//機　　能： CountLabel プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public Label CountLabel
		{
			get { return myCountLabel; }
			set { myCountLabel = value;	}
		}

		//*******************************************************************************
		//機　　能： DeleteButton プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public Button DeleteButton
		{
			get { return myDeleteButton; }
			set { myDeleteButton = value; }
		}

		//*******************************************************************************
		//機　　能： ReferenceButton プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public Button ReferenceButton
		{
			get { return myReferenceButton; }
			set { myReferenceButton = value; }
		}

		//*******************************************************************************
		//機　　能： HowToAdd プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public HowToAddType HowToAdd
		{
			get { return myHowToAdd; }
			set { myHowToAdd = value; }
		}

		//*******************************************************************************
		//機　　能： Extension プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public string Extension
		{
			get { return myExtension ?? string.Empty; }
			set { myExtension = value; }
		}

		//*******************************************************************************
		//機　　能： Description プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public string Description
		{
			get { return myDescription ?? string.Empty; }
			set { myDescription = value; }
		}

		//*******************************************************************************
		//機　　能： DefaultPath プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public string DefaultPath
		{
			get { return myDefaultPath ?? string.Empty; }
			set { myDefaultPath = value; }
		}

		//*******************************************************************************
		//機　　能： InfCheck プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public bool InfCheck
		{
			get { return myInfCheck; }
			set { myInfCheck = value; }
		}

		//*******************************************************************************
		//機　　能： Max プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public int Max
		{
			get { return myMax; }
			set { myMax = value; }
		}

		// ｺﾝﾄﾛｰﾙの Enabled ﾌﾟﾛﾊﾟﾃｨと同様の動作をさせるために、Enabled が正しいﾌﾟﾛｼｰｼﾞｬ IDを持つようにする必要があります。
		//   [ﾂｰﾙ] ﾒﾆｭｰで [ﾌﾟﾛｼｰｼﾞｬ属性] ﾀﾞｲｱﾛｸﾞ ﾎﾞｯｸｽを表示し、Enabledﾌﾟﾛﾊﾟﾃｨのﾌﾟﾛｼｰｼﾞｬ ID を Enabled に設定します。[ﾌﾟﾛｼｰｼﾞｬ ID] ﾎﾞｯｸｽは、このﾀﾞｲｱﾛｸﾞ
		//   の [詳細>>] ﾎﾞﾀﾝで表示できます。[名前] ﾎﾞｯｸｽで Enabled を選択し、Enabled ﾌﾟﾛﾊﾟﾃｨの属性を表示してください。
		//
		//警告! 以下のｺﾒﾝﾄ行を削除または変更してはいけません!
		//MappingInfo=UserControl,UserControl,-1,Enabled
		[Description("ｵﾌﾞｼﾞｪｸﾄがﾕｰｻﾞｰが作成したｲﾍﾞﾝﾄに応答できるかどうかを決定する値を設定/取得します。")]
		public new bool Enabled
		{
			get { return base.Enabled; }
			set { base.Enabled = value; }
		}

		//警告! 以下のｺﾒﾝﾄ行を削除または変更してはいけません!
		//MappingInfo=UserControl,UserControl,-1,NumberingDigits
		public int NumberingDigits
		{
			get { return myNumberingDigits; }
			set { myNumberingDigits = value; }
		}

		//警告! 以下のｺﾒﾝﾄ行を削除または変更してはいけません!
		//MappingInfo=lblCaption,lblCaption,-1,Font
		[Description("ﾌｫﾝﾄ ｵﾌﾞｼﾞｪｸﾄを返します。")]
		public new Font Font
		{
			get { return lstUserControl.Font; }
			set { 
                    lstUserControl.Font = value;
                    base.Font = value;
                }
		}

		public int ListCount
		{
			get { return lstUserControl.Items.Count; }
		}

		//警告! 以下のｺﾒﾝﾄ行を削除または変更してはいけません!
		//MappingInfo=UserControl,UserControl,-1,Refresh
		[Description("ｵﾌﾞｼﾞｪｸﾄ全体を強制的に再描画します。")]
		public override void Refresh()
		{
			base.Refresh();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lstUserControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (myDeleteButton != null)
			{
					bool Selected = false;

					//１つでも項目を選択していたらボタンを有効にする
					for (int i = 0; i < lstUserControl.Items.Count; i++)
					{
						if (lstUserControl.GetSelected(i))
						{
							Selected = true;
							break;
						}
					}

					//ボタンの使用可・不可を決定
					myDeleteButton.Enabled = Selected;

			}

			if (Click != null)
			{
				Click(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lstUserControl_KeyDown(object sender, KeyEventArgs e)
		{

			//キーを判定
			switch (e.KeyCode)
			{
				//[Del]キー
				case Keys.Delete:

					//削除ボタンが有効ならば、リストボックスの選択アイテムを削除
					if (myDeleteButton.Enabled)
					{
						RemoveSelectedItem();
					}
					break;

				//[Esc]キー
				case Keys.Escape:

					//全てを非選択にする
					for (int i = 0; i < lstUserControl.Items.Count; i++)
					{
						lstUserControl.SetSelected(i, false);
					}

					//削除ボタンを無効にする
					myDeleteButton.Enabled = false;
					break;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void myDeleteButton_Click(object sender, EventArgs e)
		{
			//選択されている項目をリストから削除
			RemoveSelectedItem();
		}

		// これは、二重ﾏｯﾋﾟﾝｸﾞの例です。ShapeLabelの Click ｲﾍﾞﾝﾄが正しく動作するためには、
		//   ﾕｰｻﾞｰがｺﾝﾄﾛｰﾙの内部をｸﾘｯｸしたときだけでなく、Label をｸﾘｯｸした場合もｲﾍﾞﾝﾄが発生する必要があります。
		private void CTListBox_Click(object sender, EventArgs e)
		{
		}

		//*******************************************************************************
		//機　　能： 「参照」ボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： ReferenceButtonプロパティに設定されているボタンがクリックされた時に実行されます
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		private void myReferenceButton_Click(object sender, EventArgs e)
		{
			if (myHowToAdd == HowToAddType.FromFileList)
			{
				AddItems(modFileIO.GetFileListFromFileDialog(myDescription, myExtension, myDefaultPath));
			}
			else
			{
				AddItems(modFileIO.GetFileListFromFolderDialog(myDescription, myExtension, myDefaultPath));
			}
		}

		//リストを初期化します
		public void Clear()
		{
			lstUserControl.Items.Clear();

			if (myDeleteButton != null)
			{
				myDeleteButton.Enabled = false;
			}

			//リスト数を表示するラベルを更新
			if (myCountLabel != null)
			{
				myCountLabel.Text = Convert.ToString(lstUserControl.Items.Count);
			}

			if (Changed != null)
			{
				Changed(this, EventArgs.Empty);
			}
		}

		//*******************************************************************************
		//機　　能： リストに項目を追加します
		//
		//           変数名          [I/O] 型        内容
		//引　　数： theItems        [I/ ] String    追加する項目
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public void AddItems(IEnumerable<string> theItems)
		{
			//追加しようとする項目のコレクションの中身がなければ何もしない
			if (theItems == null)
			{
				return;
			}

			//追加前の項目数
			int LastListCount = lstUserControl.Items.Count;

			//各項目を追加
			foreach (string theItem in theItems)
			{
				AddItem(theItem, false);
			}

			//実際に追加した項目数
			int AddedItemCount = lstUserControl.Items.Count - LastListCount;

			//追加できなかったファイルがあれば表示する
            //if (modLibrary.IsExistForm(frmInvalidFileList.Instance)) 
            if (modLibrary.IsExistForm("frmInvalidFileList"))    //Openしていないとloadしてしまうため2014/06/05(検S1)hata
			{
				frmInvalidFileList.Instance.ShowDialog();
			}

			//Changedイベント生成
			if (AddedItemCount > 0)
			{
				myDefaultPath = Path.GetDirectoryName(lstUserControl.Items[lstUserControl.Items.Count - 1] as string);

				if (Changed != null)
				{
					Changed(this, EventArgs.Empty);
				}
			}

		}

		//リストに項目を追加します
		//Public Sub AddItem(ByVal theString As String)
		public void AddItem(string theItem, bool EventOn = true)
		{
				//すでに最大数に達しているなら追加しない
				if (lstUserControl.Items.Count >= myMax)
				{
					if (!EventOn)
					{
						//エラー内容：これ以上は追加できません。
                        frmInvalidFileList.Instance.AddFile(theItem, CTResources.LoadResString(14301));
					}
					return;
				}

				//登録されている拡張子と異なる場合追加しない
				if (!string.IsNullOrEmpty(myExtension))
				{
					if (!theItem.EndsWith(myExtension, StringComparison.CurrentCultureIgnoreCase))
					{
						if (!EventOn)
						{
							//エラー内容：拡張子が異なります。
                            frmInvalidFileList.Instance.AddFile(theItem, StringTable.GetResString(9941, CTResources.LoadResString(12031)));
						}
						return;
					}
				}

				//付帯情報が存在しない場合
				if (myInfCheck)
				{
					if (!File.Exists(modFileIO.ChangeExtension(theItem, ".inf")))
					{
						if (!EventOn)
						{
							//エラー内容：付帯情報ファイルが見つかりません。
                            frmInvalidFileList.Instance.AddFile(theItem, StringTable.GetResString(StringTable.IDS_NotFound, CTResources.LoadResString(StringTable.IDS_InfoFile)));
						}
						return;
					}
				}

				if (myNumberingDigits > 0)
				{
					string s = "{0," + myNumberingDigits.ToString() + "}";
					lstUserControl.Items.Add(string.Format(s, lstUserControl.Items.Count + 1) + " " + theItem);
				}
				else
				{
					lstUserControl.Items.Add(theItem);
				}

				//リスト数を表示するラベルを更新
				if (myCountLabel != null)
				{
					myCountLabel.Text = Convert.ToString(lstUserControl.Items.Count);
				}

			//リスト内変更イベント生成
			if (Changed != null)
			{
				Changed(this, EventArgs.Empty);
			}
		}

		//選択されている項目をリストから削除
		public void RemoveSelectedItem()
		{
			while (lstUserControl.SelectedIndex >= 0)
			{
				lstUserControl.Items.RemoveAt(lstUserControl.SelectedIndex);
			}

			if (myNumberingDigits > 0)
			{
				//リストボックス内の番号を修正
				for (int i = 0; i < lstUserControl.Items.Count; i++)
				{
					string s = "{0," + myNumberingDigits.ToString() + "}";
					lstUserControl.Items[i] = string.Format(s, i + 1) + " " + lstUserControl.Items[i].ToString().Substring(myNumberingDigits + 1);
				}
			}

			if (myDeleteButton != null)
			{
				myDeleteButton.Enabled = false;
			}

			//リスト数を表示するラベルを更新
			if (myCountLabel != null)
			{
				myCountLabel.Text = Convert.ToString(lstUserControl.Items.Count);
			}

			if (Changed != null)
			{
				Changed(this, EventArgs.Empty);
			}

		}

		//選択されている項目をリストから削除
		public string List(int theIndex)
		{
			string s = null;

			if (myNumberingDigits > 0)
			{
				s = lstUserControl.Items[theIndex].ToString().Substring(myNumberingDigits + 1);
			}
			else
			{
				s = lstUserControl.Items[theIndex].ToString();
			}

			return s;
		}

		//UserList の背景を提供する ListBox コントロールはコントロール全体を覆うようにサイズ変更されます。
		private void CTListBox_Resize(System.Object eventSender, System.EventArgs eventArgs)
		{
			lstUserControl.SetBounds(0, 0, ClientRectangle.Width, ClientRectangle.Height);
		}

	}
}
