namespace CT30K
{
	partial class frmXrayInformation
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.cmdUpdate = new System.Windows.Forms.Button();
            this.cmdClose = new System.Windows.Forms.Button();
            this.lblHeader1 = new System.Windows.Forms.Label();
            this.lblInfo1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmdUpdate
            // 
            this.cmdUpdate.Location = new System.Drawing.Point(28, 284);
            this.cmdUpdate.Name = "cmdUpdate";
            this.cmdUpdate.Size = new System.Drawing.Size(145, 25);
            this.cmdUpdate.TabIndex = 2;
            this.cmdUpdate.Tag = "14121";
            this.cmdUpdate.Text = "#最新の情報に更新";
            this.cmdUpdate.UseVisualStyleBackColor = true;
            this.cmdUpdate.Click += new System.EventHandler(this.cmdUpdate_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.Location = new System.Drawing.Point(196, 284);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(77, 25);
            this.cmdClose.TabIndex = 3;
            this.cmdClose.Tag = "10008";
            this.cmdClose.Text = "#閉じる";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // lblHeader1
            // 
            this.lblHeader1.AutoSize = true;
            this.lblHeader1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblHeader1.Location = new System.Drawing.Point(8, 8);
            this.lblHeader1.Name = "lblHeader1";
            this.lblHeader1.Size = new System.Drawing.Size(105, 16);
            this.lblHeader1.TabIndex = 0;
            this.lblHeader1.Text = "状態確認(STI)";
            // 
            // lblInfo1
            // 
            this.lblInfo1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblInfo1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblInfo1.Location = new System.Drawing.Point(184, 10);
            this.lblInfo1.Name = "lblInfo1";
            this.lblInfo1.Size = new System.Drawing.Size(105, 18);
            this.lblInfo1.TabIndex = 1;
            this.lblInfo1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // frmXrayInformation
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(297, 322);
            this.ControlBox = false;
            this.Controls.Add(this.lblInfo1);
            this.Controls.Add(this.lblHeader1);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.cmdUpdate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmXrayInformation";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "14100";
            this.Text = "#Ｘ線情報";
            this.Load += new System.EventHandler(this.frmXrayInformation_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdUpdate;
		private System.Windows.Forms.Button cmdClose;
		private System.Windows.Forms.Label lblHeader1;
		private System.Windows.Forms.Label lblInfo1;
	}
}