namespace CT30K
{
	partial class frmPictureRetouch
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
			this.components = new System.ComponentModel.Container();
			this.lblPtouch0 = new System.Windows.Forms.Label();
			this.txtPtouch0 = new System.Windows.Forms.TextBox();
			this.lblPtouchColum0 = new System.Windows.Forms.Label();
			this.cmdPtouchOK = new System.Windows.Forms.Button();
			this.cmdPtouchCancel = new System.Windows.Forms.Button();
			this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// lblPtouch0
			// 
			this.lblPtouch0.Font = new System.Drawing.Font("MS UI Gothic", 10F);
			this.lblPtouch0.Location = new System.Drawing.Point(8, 8);
			this.lblPtouch0.Name = "lblPtouch0";
			this.lblPtouch0.Size = new System.Drawing.Size(132, 13);
			this.lblPtouch0.TabIndex = 0;
			// 
			// txtPtouch0
			// 
			this.txtPtouch0.Location = new System.Drawing.Point(152, 8);
			this.txtPtouch0.MaxLength = 0;
			this.txtPtouch0.Name = "txtPtouch0";
			this.txtPtouch0.Size = new System.Drawing.Size(153, 19);
			this.txtPtouch0.TabIndex = 1;
			this.txtPtouch0.Validating += new System.ComponentModel.CancelEventHandler(this.txtPtouch_Validating);
			// 
			// lblPtouchColum0
			// 
			this.lblPtouchColum0.AutoSize = true;
			this.lblPtouchColum0.Font = new System.Drawing.Font("MS UI Gothic", 11F);
			this.lblPtouchColum0.Location = new System.Drawing.Point(140, 8);
			this.lblPtouchColum0.Name = "lblPtouchColum0";
			this.lblPtouchColum0.Size = new System.Drawing.Size(10, 15);
			this.lblPtouchColum0.TabIndex = 2;
			this.lblPtouchColum0.Text = ":";
			// 
			// cmdPtouchOK
			// 
			this.cmdPtouchOK.Font = new System.Drawing.Font("MS UI Gothic", 12F);
			this.cmdPtouchOK.Location = new System.Drawing.Point(16, 563);
			this.cmdPtouchOK.Name = "cmdPtouchOK";
			this.cmdPtouchOK.Size = new System.Drawing.Size(121, 33);
			this.cmdPtouchOK.TabIndex = 3;
			this.cmdPtouchOK.Tag = "10016";
			this.cmdPtouchOK.Text = "#更  新";
			this.cmdPtouchOK.UseVisualStyleBackColor = true;
			this.cmdPtouchOK.Click += new System.EventHandler(this.cmdPtouchOK_Click);
			// 
			// cmdPtouchCancel
			// 
			this.cmdPtouchCancel.Font = new System.Drawing.Font("MS UI Gothic", 12F);
			this.cmdPtouchCancel.Location = new System.Drawing.Point(176, 563);
			this.cmdPtouchCancel.Name = "cmdPtouchCancel";
			this.cmdPtouchCancel.Size = new System.Drawing.Size(121, 33);
			this.cmdPtouchCancel.TabIndex = 4;
			this.cmdPtouchCancel.Tag = "10008";
			this.cmdPtouchCancel.Text = "#閉じる";
			this.cmdPtouchCancel.UseVisualStyleBackColor = true;
			this.cmdPtouchCancel.Click += new System.EventHandler(this.cmdPtouchCancel_Click);
			// 
			// frmPictureRetouch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(314, 612);
			this.ControlBox = false;
			this.Controls.Add(this.cmdPtouchCancel);
			this.Controls.Add(this.cmdPtouchOK);
			this.Controls.Add(this.lblPtouchColum0);
			this.Controls.Add(this.txtPtouch0);
			this.Controls.Add(this.lblPtouch0);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Location = new System.Drawing.Point(107, 73);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmPictureRetouch";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Tag = "12462";
			this.Text = "#付帯情報修正";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmPictureRetouch_FormClosed);
			this.Load += new System.EventHandler(this.frmPictureRetouch_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblPtouch0;
		private System.Windows.Forms.TextBox txtPtouch0;
		private System.Windows.Forms.Label lblPtouchColum0;
		private System.Windows.Forms.Button cmdPtouchOK;
		private System.Windows.Forms.Button cmdPtouchCancel;
		private System.Windows.Forms.ToolTip ToolTip;
	}
}