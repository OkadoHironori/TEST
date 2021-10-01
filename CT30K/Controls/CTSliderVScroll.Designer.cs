namespace CT30K
{
    partial class CTSliderVScroll
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
            this.picField = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picField)).BeginInit();
            this.SuspendLayout();
            // 
            // picField
            // 
            this.picField.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picField.BackColor = System.Drawing.SystemColors.Control;
            this.picField.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picField.Location = new System.Drawing.Point(0, 0);
            this.picField.Name = "picField";
            this.picField.Size = new System.Drawing.Size(29, 246);
            this.picField.TabIndex = 6;
            this.picField.TabStop = false;
            this.picField.Paint += new System.Windows.Forms.PaintEventHandler(this.picField_Paint);
            this.picField.MouseCaptureChanged += new System.EventHandler(this.picField_MouseCaptureChanged);
            this.picField.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picField_MouseDown);
            this.picField.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picField_MouseMove);
            this.picField.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picField_MouseUp);
            this.picField.Resize += new System.EventHandler(this.picField_Resize);
            // 
            // CTSliderVScroll
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.picField);
            this.DoubleBuffered = true;
            this.Name = "CTSliderVScroll";
            this.Size = new System.Drawing.Size(34, 244);
            ((System.ComponentModel.ISupportInitialize)(this.picField)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picField;

    }
}
