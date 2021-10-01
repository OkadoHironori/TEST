namespace CT30K
{
	partial class frmMechaAllResetMove
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
            this.cmdClose = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.Image1 = new System.Windows.Forms.PictureBox();
            this.cwbtnMechaAllResetMove = new CT30K.CWButton();
            ((System.ComponentModel.ISupportInitialize)(this.Image1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdClose
            // 
            this.cmdClose.Location = new System.Drawing.Point(224, 88);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(81, 25);
            this.cmdClose.TabIndex = 0;
            this.cmdClose.Tag = "10008";
            this.cmdClose.Text = "閉じる";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // Label1
            // 
            this.Label1.Location = new System.Drawing.Point(32, 16);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(321, 65);
            this.Label1.TabIndex = 1;
            // 
            // Image1
            // 
            this.Image1.Location = new System.Drawing.Point(4, 4);
            this.Image1.Name = "Image1";
            this.Image1.Size = new System.Drawing.Size(32, 32);
            this.Image1.TabIndex = 3;
            this.Image1.TabStop = false;
            // 
            // cwbtnMechaAllResetMove
            // 
            this.cwbtnMechaAllResetMove.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnMechaAllResetMove.Caption = "OK";
            this.cwbtnMechaAllResetMove.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnMechaAllResetMove.FlatAppearanceBorderSize = 1;
            this.cwbtnMechaAllResetMove.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnMechaAllResetMove.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnMechaAllResetMove.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnMechaAllResetMove.Location = new System.Drawing.Point(64, 88);
            this.cwbtnMechaAllResetMove.Name = "cwbtnMechaAllResetMove";
            this.cwbtnMechaAllResetMove.OffImage = null;
            this.cwbtnMechaAllResetMove.OnColor = System.Drawing.Color.Empty;
            this.cwbtnMechaAllResetMove.OnImage = null;
            this.cwbtnMechaAllResetMove.Size = new System.Drawing.Size(80, 25);
            this.cwbtnMechaAllResetMove.TabIndex = 4;
            this.cwbtnMechaAllResetMove.Value = false;
            this.cwbtnMechaAllResetMove.ValueChanged += new System.EventHandler(this.cwbtnMechaAllResetMove_ValueChanged);
            this.cwbtnMechaAllResetMove.Click += new System.EventHandler(this.cwbtnMechaAllResetMove_Click);
            // 
            // frmMechaAllResetMove
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(362, 120);
            this.ControlBox = false;
            this.Controls.Add(this.cwbtnMechaAllResetMove);
            this.Controls.Add(this.Image1);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.cmdClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(0, -7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMechaAllResetMove";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "20162";
            this.Text = "メカリセット";
            this.Load += new System.EventHandler(this.frmMechaAllResetMove_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Image1)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button cmdClose;
		private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.PictureBox Image1;
        private CWButton cwbtnMechaAllResetMove;
	}
}