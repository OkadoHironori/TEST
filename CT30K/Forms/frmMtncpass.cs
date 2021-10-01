using System.Windows.Forms;
//using CT30K.Modules;

namespace CT30K
{
    /// <summary>
    /// メンテナンスパスワード入力フォームクラス
    /// </summary>
    public partial class frmMtncpass :Form //: BaseDialog
    {
        #region 定数
        /// <summary>
        /// メンテナンスパスワード
        /// </summary>
        private const string Mtncpasswd = "CATE";
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmMtncpass()
        {
            InitializeComponent();

            // イベント定義
            InitializeEventHandler();
        }
        #endregion

        #region イベント定義
        /// <summary>
        /// イベント定義
        /// </summary>
        private void InitializeEventHandler()
        {
            // パスワード欄変更時処理
            txtPasswd.TextChanged += (sender, e) => btnOK.Enabled = (txtPasswd.Text != "");

            // ＯＫボタンクリック時処理
            btnOK.Click += (sender, e) => 
            {
                // パスワードが合っていれば
                if (txtPasswd.Text == Mtncpasswd)
                {
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    // メッセージ表示：入力された ﾒﾝﾃﾅﾝｽ ﾊﾟｽﾜｰﾄﾞが間違っています。
                    MessageBox.Show(StringTable.GetResString(9927, lblPasswd.Text),
                                    Application.ProductName,
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);

                    // パスワード欄を消去
                    txtPasswd.Clear();

                    // パスワード欄にフォーカス
                    txtPasswd.Focus();
                }
            };
        }
        #endregion

        #region スタティックメソッド
        /// <summary>
        /// ダイアログ表示処理
        /// </summary>
        /// <returns></returns>
        public static bool Dialog() 
        {
            using (var dialog = new frmMtncpass()) 
            {
                return (dialog.ShowDialog() == DialogResult.OK);
            }
        }
        #endregion

        // Add Start 2018/08/28 M.Oyama
        private void frmMtncpass_Load(object sender, System.EventArgs e)
        {
            SetCaption();
        }

        private void SetCaption()
        {
            //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);
        }
        // Add End 2018/08/30
    }
}
