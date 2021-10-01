using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CT30K
{
	public partial class frmInvalidFileList : Form
	{
		private const string LV_KEY_FILENAME = "FileName";
		private const string LV_KEY_REASON = "Reason";

		private static frmInvalidFileList _Instance = null;

		public frmInvalidFileList()
		{
			InitializeComponent();

            //Rev25.00/Rev24.00 loadイベントへ変更 by長野 2016/07/11
            //lvwInvalidFile.Columns.Add(LV_KEY_FILENAME, string.Empty, 288);
            //lvwInvalidFile.Columns.Add(LV_KEY_REASON, string.Empty, 288);
		}

		public static frmInvalidFileList Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmInvalidFileList();
				}

				return _Instance;
			}
		}

        //Rev25.00/Rev24.00 追加 by長野 2016/07/11
        public int ListItemsCount
        {
            get
            {
                return lvwInvalidFile.Items.Count;
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
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdClose_Click(object sender, EventArgs e)
		{
            //Rev25.00/Rev24.00 リストはクリア by長野 2016/07/11
            lvwInvalidFile.Clear();

			//アンロード
			this.Close();
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
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void frmInvalidFileList_Load(object sender, EventArgs e)
		{
			//ストリングテーブル化 v17.60 by長野 2011/06/14
			lblMessage.Text = CTResources.LoadResString(20132);
			cmdClose.Text = CTResources.LoadResString(10008);

            //Rev25.00/Rev24.00 loadイベントへ変更 by長野 2016/07/11
            lvwInvalidFile.Columns.Add(LV_KEY_FILENAME, string.Empty, 288);
            lvwInvalidFile.Columns.Add(LV_KEY_REASON, string.Empty, 288);
            
            lvwInvalidFile.Columns[LV_KEY_FILENAME].Text = CTResources.LoadResString(StringTable.IDS_FileName);		//ファイル名
			lvwInvalidFile.Columns[LV_KEY_REASON].Text = CTResources.LoadResString(12511);							//原因
		}


		//*******************************************************************************
		//機　　能： リストにファイルを追加する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： FileName        [I/ ] String    ファイル名
		//           Reason          [I/ ] String    理由
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public void AddFile(string FileName, string Reason = "")
		{
			ListViewItem theItem = null;
			theItem = lvwInvalidFile.Items.Add(FileName);
			theItem.SubItems.Add(Reason);
		}

	}
}
