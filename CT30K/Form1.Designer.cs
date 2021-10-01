namespace CT30K {
	partial class Form1 {
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

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent() {
            this.numTextBox1 = new CT30K.NumTextBox();
            this.SuspendLayout();
            // 
            // numTextBox1
            // 
            this.numTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.numTextBox1.Caption = "管電圧";
            this.numTextBox1.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.numTextBox1.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numTextBox1.CaptionWidth = 44;
            this.numTextBox1.DiscreteInterval = 0.01F;
            this.numTextBox1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numTextBox1.IncDecButton = false;
            this.numTextBox1.Location = new System.Drawing.Point(54, 96);
            this.numTextBox1.Max = new decimal(new int[] {
            -10,
            4,
            0,
            196608});
            this.numTextBox1.Min = new decimal(new int[] {
            0,
            5,
            0,
            -2147287040});
            this.numTextBox1.Name = "numTextBox1";
            this.numTextBox1.Size = new System.Drawing.Size(113, 22);
            this.numTextBox1.TabIndex = 0;
            this.numTextBox1.Unit = "kV";
            this.numTextBox1.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 283);
            this.Controls.Add(this.numTextBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

		}

		#endregion

        private NumTextBox numTextBox1;

	}
}

