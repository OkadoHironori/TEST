using System.Windows.Forms;
//using CT30K.Modules;

namespace CT30K
{
    /// <summary>
    /// コメント入力フォーム
    /// </summary>
    public partial class frmComment : Form
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmComment()
        {
            InitializeComponent();

            // イベント定義
            InitializeEventHandler();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="title"></param>
        /// <param name="defaultComment"></param>
        /// <param name="maxLength"></param>
        public frmComment(string prompt, string title, string defaultComment, int maxLength) : this()
        {
            // プロンプト文字列
            lblPrompt.Text = prompt;

            // タイトルバーに表示する文字列
            this.Text = string.IsNullOrEmpty(title) ? Application.ProductName : title;

            // デフォルトのコメント文字列
            txtComment.Text = defaultComment;

            // 入力できる最大文字数
            txtComment.MaxLength = maxLength;

            //キャプションのセット
            SetCaption();           //2018/09/19 (検S1)やまおか

        }
        #endregion

        #region イベント定義
        /// <summary>
        /// イベント定義
        /// </summary>
        private void InitializeEventHandler()
        {
            // 入力テキストボックス欄変更時処理：全角文字を２文字とみなした文字数チェック
            txtComment.TextChanged +=(sender, e) => modCT30K.ChangeTextBox(txtComment);
        }
        #endregion

        #region プロパティ
        public string Comment
        {
            get { return txtComment.Text.Trim(); }
        }
        #endregion

        #region パブリックメソッド（スタティック）
        /// <summary>
        /// ダイアログ処理
        /// </summary>
        /// <param name="prompt">プロンプト文字列</param>
        /// <param name="title">タイトルバーに表示する文字列</param>
        /// <param name="defaultComment">デフォルトのコメント文字列</param>
        /// <returns>入力されたコメント文字列</returns>
        public static string Dialog(string prompt, string title, string defaultComment)
        {
            return Dialog( prompt, title, defaultComment, 256);
        }

        /// <summary>
        /// ダイアログ処理
        /// </summary>
        /// <param name="prompt">プロンプト文字列</param>
        /// <param name="title">タイトルバーに表示する文字列</param>
        /// <param name="defaultComment">デフォルトのコメント文字列</param>
        /// <param name="maxLength">入力できる最大文字数</param>
        /// <returns>入力されたコメント文字列</returns>
	    public static string Dialog(string prompt, string title, string defaultComment, int maxLength)
	    {
            // フォーム生成
            using (var dialog = new frmComment(prompt, title, defaultComment, maxLength))
            {
                // このフォームをモーダル表示
                if (dialog.ShowDialog() == DialogResult.OK)
                    return dialog.Comment;
                else
                    return null;
            }
        }
        #endregion

        //*******************************************************************************
        //機　　能： 各コントロールのキャプションに文字列をセットする
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  18/09/19   (検S1)やまおか     新規作成
        //*******************************************************************************
        private void SetCaption()
        {
            //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);

        }

    }
}