namespace CT30K
{
	partial class frmStart
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
		if (disposing && (components != null)) {
		components.Dispose();
		}
		base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStart));
			this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.pgbProcessing = new System.Windows.Forms.ProgressBar();
			this.lblProductName = new System.Windows.Forms.Label();
			this.imgLogo = new System.Windows.Forms.PictureBox();
			this.lblMessage = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.imgLogo)).BeginInit();
			this.SuspendLayout();
			// 
			// pgbProcessing
			// 
			this.pgbProcessing.Location = new System.Drawing.Point(96, 236);
			this.pgbProcessing.Name = "pgbProcessing";
			this.pgbProcessing.Size = new System.Drawing.Size(217, 21);
			this.pgbProcessing.TabIndex = 2;
			// 
			// lblProductName
			// 
			this.lblProductName.AutoSize = true;
			this.lblProductName.BackColor = System.Drawing.Color.Transparent;
			this.lblProductName.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblProductName.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblProductName.Location = new System.Drawing.Point(72, 16);
			this.lblProductName.Name = "lblProductName";
			this.lblProductName.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblProductName.Size = new System.Drawing.Size(117, 16);
			this.lblProductName.TabIndex = 1;
			this.lblProductName.Text = "#lblProductName";
			// 
			// imgLogo
			// 
			this.imgLogo.Cursor = System.Windows.Forms.Cursors.Default;
			this.imgLogo.Image = ((System.Drawing.Image)(resources.GetObject("imgLogo.Image")));
			this.imgLogo.Location = new System.Drawing.Point(24, 16);
			this.imgLogo.Name = "imgLogo";
			this.imgLogo.Size = new System.Drawing.Size(33, 33);
			this.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.imgLogo.TabIndex = 3;
			this.imgLogo.TabStop = false;
			// 
			// lblMessage
			// 
			this.lblMessage.BackColor = System.Drawing.Color.Transparent;
			this.lblMessage.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblMessage.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblMessage.Location = new System.Drawing.Point(96, 212);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblMessage.Size = new System.Drawing.Size(217, 17);
			this.lblMessage.TabIndex = 0;
			this.lblMessage.Text = "#lblMessage";
			this.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// frmStart
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.ClientSize = new System.Drawing.Size(401, 269);
			this.ControlBox = false;
			this.Controls.Add(this.pgbProcessing);
			this.Controls.Add(this.lblProductName);
			this.Controls.Add(this.imgLogo);
			this.Controls.Add(this.lblMessage);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.KeyPreview = true;
			this.Location = new System.Drawing.Point(17, 94);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmStart";
			this.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmStart_FormClosed);
			this.Load += new System.EventHandler(this.frmStart_Load);
			((System.ComponentModel.ISupportInitialize)(this.imgLogo)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		public System.Windows.Forms.ToolTip ToolTip1;
		public System.Windows.Forms.ProgressBar pgbProcessing;
		public System.Windows.Forms.Label lblProductName;
		public System.Windows.Forms.PictureBox imgLogo;
		public System.Windows.Forms.Label lblMessage;
	}
}
