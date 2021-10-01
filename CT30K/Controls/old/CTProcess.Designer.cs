namespace CT30K {
	partial class CTProcess {
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CTProcess));
			this.Timer1 = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// Timer1
			// 
			this.Timer1.Interval = 500;
			this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
			// 
			// CTProcess
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.Name = "CTProcess";
			this.Size = new System.Drawing.Size(149, 98);
			this.Resize += new System.EventHandler(this.CTProcess_Resize);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer Timer1;
	}
}
