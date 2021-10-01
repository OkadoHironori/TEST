namespace CT30K {
	partial class CTButton {
		/// <summary> 
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing) {
		if (disposing && (components != null)) {
		components.Dispose();
		}
		base.Dispose(disposing);
		}

		#region コンポーネント デザイナーで生成されたコード

		/// <summary> 
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent() {
            this.myButton = new System.Windows.Forms.Button();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblColon = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // myButton
            // 
            this.myButton.BackColor = System.Drawing.Color.Green;
            this.myButton.Location = new System.Drawing.Point(87, 0);
            this.myButton.Name = "myButton";
            this.myButton.Size = new System.Drawing.Size(58, 32);
            this.myButton.TabIndex = 0;
            this.myButton.UseVisualStyleBackColor = false;
            this.myButton.Click += new System.EventHandler(this.myButton_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.Location = new System.Drawing.Point(0, 4);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(39, 15);
            this.lblHeader.TabIndex = 1;
            this.lblHeader.Text = "ヘッダ";
            // 
            // lblColon
            // 
            this.lblColon.Location = new System.Drawing.Point(68, 4);
            this.lblColon.Name = "lblColon";
            this.lblColon.Size = new System.Drawing.Size(13, 15);
            this.lblColon.TabIndex = 2;
            this.lblColon.Text = ":";
            // 
            // CTButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.myButton);
            this.Controls.Add(this.lblColon);
            this.Controls.Add(this.lblHeader);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Name = "CTButton";
            this.Size = new System.Drawing.Size(174, 32);
            this.Resize += new System.EventHandler(this.CTButton_Resize);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button myButton;
		private System.Windows.Forms.Label lblHeader;
		private System.Windows.Forms.Label lblColon;
	}
}
