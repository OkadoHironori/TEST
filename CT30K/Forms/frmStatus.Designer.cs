namespace CT30K
{
	partial class frmStatus
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
            this.stsCapture = new CT30K.CTStatus();
            this.stsScan = new CT30K.CTStatus();
            this.stsCorrect = new CT30K.CTStatus();
            this.stsCPU = new CT30K.CTStatus();
            this.stsMecha = new CT30K.CTStatus();
            this.SuspendLayout();
            // 
            // stsCapture
            // 
            this.stsCapture.Caption = "#キャプチャ";
            this.stsCapture.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.stsCapture.CaptionWidth = 60;
            this.stsCapture.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stsCapture.Location = new System.Drawing.Point(720, 12);
            this.stsCapture.Name = "stsCapture";
            this.stsCapture.Size = new System.Drawing.Size(158, 18);
            this.stsCapture.Status = "";
            this.stsCapture.TabIndex = 4;
            this.stsCapture.Tag = "12299";
            // 
            // stsScan
            // 
            this.stsScan.Caption = "#スキャン";
            this.stsScan.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.stsScan.CaptionWidth = 56;
            this.stsScan.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stsScan.Location = new System.Drawing.Point(36, 12);
            this.stsScan.Name = "stsScan";
            this.stsScan.Size = new System.Drawing.Size(158, 18);
            this.stsScan.Status = "";
            this.stsScan.TabIndex = 3;
            this.stsScan.Tag = "12028";
            // 
            // stsCorrect
            // 
            this.stsCorrect.Caption = "#校正";
            this.stsCorrect.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.stsCorrect.CaptionWidth = 56;
            this.stsCorrect.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stsCorrect.Location = new System.Drawing.Point(536, 12);
            this.stsCorrect.Name = "stsCorrect";
            this.stsCorrect.Size = new System.Drawing.Size(158, 18);
            this.stsCorrect.Status = "";
            this.stsCorrect.TabIndex = 2;
            this.stsCorrect.Tag = "12305";
            // 
            // stsCPU
            // 
            this.stsCPU.Caption = "#ＣＰＵ";
            this.stsCPU.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.stsCPU.CaptionWidth = 56;
            this.stsCPU.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stsCPU.Location = new System.Drawing.Point(200, 12);
            this.stsCPU.Name = "stsCPU";
            this.stsCPU.Size = new System.Drawing.Size(158, 18);
            this.stsCPU.Status = "";
            this.stsCPU.TabIndex = 1;
            this.stsCPU.Tag = "12297";
            // 
            // stsMecha
            // 
            this.stsMecha.Caption = "#メ　カ";
            this.stsMecha.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.stsMecha.CaptionWidth = 56;
            this.stsMecha.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stsMecha.Location = new System.Drawing.Point(368, 12);
            this.stsMecha.Name = "stsMecha";
            this.stsMecha.Size = new System.Drawing.Size(158, 18);
            this.stsMecha.Status = "";
            this.stsMecha.TabIndex = 0;
            this.stsMecha.Tag = "12302";
            // 
            // frmStatus
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1024, 33);
            this.Controls.Add(this.stsCapture);
            this.Controls.Add(this.stsScan);
            this.Controls.Add(this.stsCorrect);
            this.Controls.Add(this.stsCPU);
            this.Controls.Add(this.stsMecha);
            this.Location = new System.Drawing.Point(107, 76);
            this.Name = "frmStatus";
            this.ShowInTaskbar = false;
            this.Tag = "";
            this.Activated += new System.EventHandler(this.frmStatus_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmStatus_FormClosed);
            this.Load += new System.EventHandler(this.frmStatus_Load);
            this.ResumeLayout(false);

		}

		#endregion

        internal CTStatus stsMecha;
        internal CTStatus stsCPU;
        internal CTStatus stsCorrect;
        internal CTStatus stsScan;
        internal CTStatus stsCapture;

    }
}