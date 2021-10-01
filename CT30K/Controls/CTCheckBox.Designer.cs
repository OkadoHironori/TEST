namespace CT30K
{
    partial class CTCheckBox
    {
        /// <summary> 
        /// 必要なデザイナ変数です。
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

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CTCheckBox));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.myPicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.myPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Checked.ico");
            // 
            // myPicture
            // 
            this.myPicture.BackColor = System.Drawing.Color.White;
            this.myPicture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.myPicture.Location = new System.Drawing.Point(0, 0);
            this.myPicture.Name = "myPicture";
            this.myPicture.Size = new System.Drawing.Size(13, 13);
            this.myPicture.TabIndex = 0;
            this.myPicture.TabStop = false;
            this.myPicture.Click += new System.EventHandler(this.myPicture_Click);
            // 
            // CTCheckBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.myPicture);
            this.Name = "CTCheckBox";
            this.Size = new System.Drawing.Size(46, 28);
            this.Resize += new System.EventHandler(this.CTCheckBox_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.myPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.PictureBox myPicture;


    }
}
