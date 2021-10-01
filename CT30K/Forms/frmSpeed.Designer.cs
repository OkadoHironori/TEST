namespace CT30K
{
	partial class frmSpeed
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

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.lblSpeedUni = new System.Windows.Forms.Label();
            this.cwknobSpeedMinLabel = new System.Windows.Forms.Label();
            this.cwknobSpeedMaxLabel = new System.Windows.Forms.Label();
            this.cwknobSpeed = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.cwknobSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Location = new System.Drawing.Point(13, 111);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(35, 12);
            this.lblHeader.TabIndex = 2;
            this.lblHeader.Tag = "12179";
            this.lblHeader.Text = "速度：";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSpeed
            // 
            this.lblSpeed.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSpeed.Location = new System.Drawing.Point(48, 109);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(47, 13);
            this.lblSpeed.TabIndex = 1;
            this.lblSpeed.Text = "00.00";
            this.lblSpeed.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSpeedUni
            // 
            this.lblSpeedUni.AutoSize = true;
            this.lblSpeedUni.Location = new System.Drawing.Point(94, 111);
            this.lblSpeedUni.Name = "lblSpeedUni";
            this.lblSpeedUni.Size = new System.Drawing.Size(35, 12);
            this.lblSpeedUni.TabIndex = 3;
            this.lblSpeedUni.Text = "mm/s";
            // 
            // cwknobSpeedMinLabel
            // 
            this.cwknobSpeedMinLabel.Location = new System.Drawing.Point(6, 70);
            this.cwknobSpeedMinLabel.Name = "cwknobSpeedMinLabel";
            this.cwknobSpeedMinLabel.Size = new System.Drawing.Size(40, 20);
            this.cwknobSpeedMinLabel.TabIndex = 4;
            this.cwknobSpeedMinLabel.Text = "1.0";
            // 
            // cwknobSpeedMaxLabel
            // 
            this.cwknobSpeedMaxLabel.Location = new System.Drawing.Point(90, 70);
            this.cwknobSpeedMaxLabel.Name = "cwknobSpeedMaxLabel";
            this.cwknobSpeedMaxLabel.Size = new System.Drawing.Size(40, 20);
            this.cwknobSpeedMaxLabel.TabIndex = 5;
            this.cwknobSpeedMaxLabel.Text = "10.0";
            this.cwknobSpeedMaxLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cwknobSpeed
            // 
            this.cwknobSpeed.AutoSize = false;
            this.cwknobSpeed.Location = new System.Drawing.Point(0, 20);
            this.cwknobSpeed.Maximum = 100;
            this.cwknobSpeed.Minimum = 1;
            this.cwknobSpeed.Name = "cwknobSpeed";
            this.cwknobSpeed.Size = new System.Drawing.Size(134, 57);
            this.cwknobSpeed.TabIndex = 0;
            this.cwknobSpeed.TickFrequency = 0;
            this.cwknobSpeed.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.cwknobSpeed.Value = 1;
            this.cwknobSpeed.ValueChanged += new System.EventHandler(this.cwknobSpeed_ValueChanged);
            this.cwknobSpeed.MouseCaptureChanged += new System.EventHandler(this.cwknobSpeed_MouseCaptureChanged);
            // 
            // frmSpeed
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(135, 132);
            this.Controls.Add(this.cwknobSpeedMaxLabel);
            this.Controls.Add(this.cwknobSpeedMinLabel);
            this.Controls.Add(this.cwknobSpeed);
            this.Controls.Add(this.lblSpeedUni);
            this.Controls.Add(this.lblSpeed);
            this.Controls.Add(this.lblHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Location = new System.Drawing.Point(0, -2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSpeed";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "動作速度";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSpeed_FormClosing);
            this.Load += new System.EventHandler(this.frmSpeed_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cwknobSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblHeader;
		private System.Windows.Forms.Label lblSpeed;
		private System.Windows.Forms.Label lblSpeedUni;
		private System.Windows.Forms.Label cwknobSpeedMinLabel;
		private System.Windows.Forms.Label cwknobSpeedMaxLabel;
        private System.Windows.Forms.TrackBar cwknobSpeed;
	}
}

