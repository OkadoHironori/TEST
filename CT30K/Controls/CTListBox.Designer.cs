namespace CT30K
{
    partial class CTListBox
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.lstUserControl = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lstUserControl
            // 
            this.lstUserControl.FormattingEnabled = true;
            this.lstUserControl.ItemHeight = 12;
            this.lstUserControl.Location = new System.Drawing.Point(0, 0);
            this.lstUserControl.Name = "lstUserControl";
            this.lstUserControl.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstUserControl.Size = new System.Drawing.Size(107, 28);
            this.lstUserControl.TabIndex = 2;
            this.lstUserControl.SelectedIndexChanged += new System.EventHandler(this.lstUserControl_SelectedIndexChanged);
            this.lstUserControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstUserControl_KeyDown);
            // 
            // CTListBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.lstUserControl);
            this.Name = "CTListBox";
            this.Size = new System.Drawing.Size(181, 46);
            this.Resize += new System.EventHandler(this.CTListBox_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstUserControl;

    }
}
