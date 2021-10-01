namespace CT30K {
	partial class CWButton {
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // myButton
            // 
            this.myButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myButton.Location = new System.Drawing.Point(0, 0);
            this.myButton.Name = "myButton";
            this.myButton.Size = new System.Drawing.Size(70, 50);
            this.myButton.TabIndex = 1;
            this.myButton.Text = "Button";
            this.myButton.UseVisualStyleBackColor = true;
            this.myButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.myButton_MouseClick);
            this.myButton.MouseCaptureChanged += new System.EventHandler(this.myButton_MouseCaptureChanged);
            this.myButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.myButton_MouseDown);
            this.myButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.myButton_MouseUp);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // CWButton
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.myButton);
            this.Name = "CWButton";
            this.Size = new System.Drawing.Size(70, 50);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Button myButton;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;

    }
}
