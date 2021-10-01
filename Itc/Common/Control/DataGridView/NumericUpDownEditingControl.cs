using System;

using System.Windows.Forms;

namespace Itc.Common.Controls
{
    /// <summary>
    /// NumericUpDownをDataGridViewのセル内で使用する
    /// </summary>
    public class NumericUpDownEditingControl : NumericUpDown, IDataGridViewEditingControl
    {   
        #region IDataGridViewEditingControl

        /// <summary>
        /// セルを格納する System.Windows.Forms.DataGridView を取得または設定します。
        /// <returns>編集対象の System.Windows.Forms.DataGridViewCell を格納する System.Windows.Forms.DataGridView。関連付けられたSystem.Windows.Forms.DataGridView がない場合は null。</returns>
        /// </summary>
        public System.Windows.Forms.DataGridView EditingControlDataGridView { get; set; }


        /// <summary>
        /// エディターによって変更されるセルの書式設定された値を取得または設定します。
        /// <returns>セルの書式設定された値を表す System.Object。</returns>
        /// </summary>
        public object EditingControlFormattedValue
        {
            get { return this.Value; }
            set
            {
                var newValue = value as string;

                if (newValue != null)
                {
                    this.Value = decimal.Parse(newValue);
                }
            }
        }

        /// <summary>
        /// ホストしているセルの親行のインデックスを取得または設定します。
        /// <returns>セルを含む行のインデックス。親行がない場合は -1。</returns>
        /// </summary>
        public int EditingControlRowIndex { get; set; }

        /// <summary>
        /// 編集コントロールの値と、そのコントロールをホストしているセルの値とが異なるかどうかを示す値を取得または設定します。
        /// <returns>コントロールの値と、セルの値が異なる場合は true。それ以外の場合は false。</returns>
        /// </summary>
        public bool EditingControlValueChanged { get; set; }


        /// <summary>
        /// マウス ポインターが編集コントロールの上ではなく、System.Windows.Forms.DataGridView.EditingPanel の上にあるときに使用されるカーソルを取得します。
        /// <returns>編集パネルに使用されるマウス ポインターを表す System.Windows.Forms.Cursor。</returns>
        /// </summary>
        public Cursor EditingPanelCursor { get { return base.Cursor; } }


        /// <summary>
        /// 値が変更されるたびに、セルの内容の位置を変更する必要があるかどうかを示す値を取得または設定します。
        /// <returns>内容の位置を変更する必要がある場合は true。それ以外の場合は false。</returns>
        /// </summary>
        public bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }

        /// <summary>
        /// 指定されたセル スタイルと矛盾しないように、コントロールのユーザー インターフェイス (UI) を変更します。
        /// <paramref name="dataGridViewCellStyle">UI のモデルとして使用する System.Windows.Forms.DataGridViewCellStyle。</paramref>
        /// </summary>
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
        }
                
        /// <summary>
        /// 指定されたキーが、編集コントロールによって処理される通常の入力キーか、System.Windows.Forms.DataGridView によって処理される特殊なキーであるかを確認します。
        /// </summary>
        /// <param name="keyData">押されたキーを表す System.Windows.Forms.Keys。</param>
        /// <param name="dataGridViewWantsInputKey">keyData に格納された System.Windows.Forms.Keys を、System.Windows.Forms.DataGridViewに処理させる場合は true。それ以外の場合は false。</param>
        /// <returns>指定されたキーが編集コントロールによって処理される通常の入力キーの場合は true。それ以外の場合は false。</returns>
        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:
                    return true;
                default:
                    return false;
            }
        }
                
        /// <summary>
        /// セルの書式設定された値を取得します。
        /// </summary>
        /// <param name="context">データが必要なコンテキストを指定する System.Windows.Forms.DataGridViewDataErrorContexts 値のビットごとの組み合わせ。</param>
        /// <returns>セルの内容の書式設定されたバージョンを表す System.Object。</returns>
        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        /// <summary>
        /// 現在選択されているセルの編集を準備します。
        /// </summary>
        /// <param name="selectAll">セルの内容をすべて選択する場合は true。それ以外の場合は false。</param>
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            //とりあえず無効？
            if (selectAll)
            {
                //選択状態にする？
                this.Select(0, this.Text.Length);
                //this.SelectAll();
            }
            else
            {
            }
        }

        #endregion

        /// <summary>
        /// NumericUpDown.ValueChangedイベントを発生させます。
        /// </summary>
        /// <param name="e">イベントデータを格納しているEventArgs</param>
        protected override void OnValueChanged(EventArgs e)
        {
            EditingControlValueChanged = true;

            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);

            base.OnValueChanged(e);
        }

        /// <summary>
        /// NumericUpDownEditingControlクラスの新しいインスタンスを初期化します。
        /// </summary>
        public NumericUpDownEditingControl() : base() { }
    }
}
