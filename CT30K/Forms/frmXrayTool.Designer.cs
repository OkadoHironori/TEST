namespace CT30K
{
	partial class frmXrayTool
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
            this.fraForcedWarmup = new System.Windows.Forms.GroupBox();
            this.cmdWarmupNone = new System.Windows.Forms.Button();
            this.cmdStartStop = new System.Windows.Forms.Button();
            this.optForcedWarmup1 = new System.Windows.Forms.RadioButton();
            this.optForcedWarmup0 = new System.Windows.Forms.RadioButton();
            this.fraTimer = new System.Windows.Forms.GroupBox();
            this.cmdTimerStartStop = new System.Windows.Forms.Button();
            this.lblSS = new System.Windows.Forms.Label();
            this.cwneTimerSS = new System.Windows.Forms.NumericUpDown();
            this.lblMM = new System.Windows.Forms.Label();
            this.cwneTimerMM = new System.Windows.Forms.NumericUpDown();
            this.cmdClose = new System.Windows.Forms.Button();
            this.fraForcedWarmup.SuspendLayout();
            this.fraTimer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneTimerSS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneTimerMM)).BeginInit();
            this.SuspendLayout();
            // 
            // fraForcedWarmup
            // 
            this.fraForcedWarmup.Controls.Add(this.cmdWarmupNone);
            this.fraForcedWarmup.Controls.Add(this.cmdStartStop);
            this.fraForcedWarmup.Controls.Add(this.optForcedWarmup1);
            this.fraForcedWarmup.Controls.Add(this.optForcedWarmup0);
            this.fraForcedWarmup.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fraForcedWarmup.Location = new System.Drawing.Point(8, 8);
            this.fraForcedWarmup.Name = "fraForcedWarmup";
            this.fraForcedWarmup.Size = new System.Drawing.Size(193, 125);
            this.fraForcedWarmup.TabIndex = 0;
            this.fraForcedWarmup.TabStop = false;
            this.fraForcedWarmup.Tag = "16101";
            this.fraForcedWarmup.Text = "#強制ウォームアップ";
            // 
            // cmdWarmupNone
            // 
            this.cmdWarmupNone.Location = new System.Drawing.Point(104, 84);
            this.cmdWarmupNone.Name = "cmdWarmupNone";
            this.cmdWarmupNone.Size = new System.Drawing.Size(77, 29);
            this.cmdWarmupNone.TabIndex = 3;
            this.cmdWarmupNone.Tag = "16104";
            this.cmdWarmupNone.Text = "#解除";
            this.cmdWarmupNone.UseVisualStyleBackColor = true;
            this.cmdWarmupNone.Click += new System.EventHandler(this.cmdWarmupNone_Click);
            // 
            // cmdStartStop
            // 
            this.cmdStartStop.Location = new System.Drawing.Point(12, 84);
            this.cmdStartStop.Name = "cmdStartStop";
            this.cmdStartStop.Size = new System.Drawing.Size(77, 29);
            this.cmdStartStop.TabIndex = 2;
            this.cmdStartStop.Text = "#開始";
            this.cmdStartStop.UseVisualStyleBackColor = true;
            this.cmdStartStop.Click += new System.EventHandler(this.cmdStartStop_Click);
            // 
            // optForcedWarmup1
            // 
            this.optForcedWarmup1.Location = new System.Drawing.Point(24, 52);
            this.optForcedWarmup1.Name = "optForcedWarmup1";
            this.optForcedWarmup1.Size = new System.Drawing.Size(161, 20);
            this.optForcedWarmup1.TabIndex = 1;
            this.optForcedWarmup1.Tag = "16103";
            this.optForcedWarmup1.Text = "#長期";
            this.optForcedWarmup1.UseVisualStyleBackColor = true;
            // 
            // optForcedWarmup0
            // 
            this.optForcedWarmup0.Checked = true;
            this.optForcedWarmup0.Location = new System.Drawing.Point(24, 28);
            this.optForcedWarmup0.Name = "optForcedWarmup0";
            this.optForcedWarmup0.Size = new System.Drawing.Size(145, 20);
            this.optForcedWarmup0.TabIndex = 0;
            this.optForcedWarmup0.TabStop = true;
            this.optForcedWarmup0.Tag = "16102";
            this.optForcedWarmup0.Text = "#短期";
            this.optForcedWarmup0.UseVisualStyleBackColor = true;
            // 
            // fraTimer
            // 
            this.fraTimer.Controls.Add(this.cmdTimerStartStop);
            this.fraTimer.Controls.Add(this.lblSS);
            this.fraTimer.Controls.Add(this.cwneTimerSS);
            this.fraTimer.Controls.Add(this.lblMM);
            this.fraTimer.Controls.Add(this.cwneTimerMM);
            this.fraTimer.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fraTimer.Location = new System.Drawing.Point(212, 8);
            this.fraTimer.Name = "fraTimer";
            this.fraTimer.Size = new System.Drawing.Size(193, 125);
            this.fraTimer.TabIndex = 1;
            this.fraTimer.TabStop = false;
            this.fraTimer.Tag = "16105";
            this.fraTimer.Text = "#タイマー";
            // 
            // cmdTimerStartStop
            // 
            this.cmdTimerStartStop.Location = new System.Drawing.Point(60, 84);
            this.cmdTimerStartStop.Name = "cmdTimerStartStop";
            this.cmdTimerStartStop.Size = new System.Drawing.Size(77, 29);
            this.cmdTimerStartStop.TabIndex = 4;
            this.cmdTimerStartStop.Text = "#開始";
            this.cmdTimerStartStop.UseVisualStyleBackColor = true;
            this.cmdTimerStartStop.Click += new System.EventHandler(this.cmdTimerStartStop_Click);
            // 
            // lblSS
            // 
            this.lblSS.AutoSize = true;
            this.lblSS.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSS.Location = new System.Drawing.Point(139, 36);
            this.lblSS.Name = "lblSS";
            this.lblSS.Size = new System.Drawing.Size(30, 15);
            this.lblSS.TabIndex = 3;
            this.lblSS.Tag = "12180";
            this.lblSS.Text = "#秒";
            this.lblSS.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cwneTimerSS
            // 
            this.cwneTimerSS.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneTimerSS.Location = new System.Drawing.Point(100, 32);
            this.cwneTimerSS.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.cwneTimerSS.Name = "cwneTimerSS";
            this.cwneTimerSS.Size = new System.Drawing.Size(39, 22);
            this.cwneTimerSS.TabIndex = 2;
            this.cwneTimerSS.Tag = "";
            this.cwneTimerSS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblMM
            // 
            this.lblMM.AutoSize = true;
            this.lblMM.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMM.Location = new System.Drawing.Point(80, 36);
            this.lblMM.Name = "lblMM";
            this.lblMM.Size = new System.Drawing.Size(30, 15);
            this.lblMM.TabIndex = 1;
            this.lblMM.Tag = "12181";
            this.lblMM.Text = "#分";
            // 
            // cwneTimerMM
            // 
            this.cwneTimerMM.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneTimerMM.Location = new System.Drawing.Point(40, 32);
            this.cwneTimerMM.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.cwneTimerMM.Name = "cwneTimerMM";
            this.cwneTimerMM.Size = new System.Drawing.Size(39, 22);
            this.cwneTimerMM.TabIndex = 0;
            this.cwneTimerMM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneTimerMM.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // cmdClose
            // 
            this.cmdClose.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdClose.Location = new System.Drawing.Point(156, 148);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(97, 29);
            this.cmdClose.TabIndex = 2;
            this.cmdClose.Tag = "10008";
            this.cmdClose.Text = "#閉じる";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // frmXrayTool
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(417, 190);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.fraTimer);
            this.Controls.Add(this.fraForcedWarmup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmXrayTool";
            this.ShowInTaskbar = false;
            this.Tag = "16100";
            this.Text = "#Ｘ線ツール";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmXrayTool_FormClosed);
            this.Load += new System.EventHandler(this.frmXrayTool_Load);
            this.fraForcedWarmup.ResumeLayout(false);
            this.fraTimer.ResumeLayout(false);
            this.fraTimer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneTimerSS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneTimerMM)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox fraForcedWarmup;
		private System.Windows.Forms.Button cmdWarmupNone;
		private System.Windows.Forms.Button cmdStartStop;
		private System.Windows.Forms.RadioButton optForcedWarmup1;
		private System.Windows.Forms.RadioButton optForcedWarmup0;
		private System.Windows.Forms.GroupBox fraTimer;
		private System.Windows.Forms.Button cmdTimerStartStop;
		private System.Windows.Forms.Label lblSS;
		private System.Windows.Forms.NumericUpDown cwneTimerSS;
		private System.Windows.Forms.Label lblMM;
		private System.Windows.Forms.NumericUpDown cwneTimerMM;
		private System.Windows.Forms.Button cmdClose;
	}
}