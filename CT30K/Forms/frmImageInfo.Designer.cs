namespace CT30K
{
	partial class frmImageInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImageInfo));
            this.lblContext1 = new System.Windows.Forms.Label();
            this.lblItem1 = new System.Windows.Forms.Label();
            this.lblColon1 = new System.Windows.Forms.Label();
            this.fraScale = new System.Windows.Forms.Panel();
            this.Line7 = new System.Windows.Forms.Label();
            this.Line6 = new System.Windows.Forms.Label();
            this.Line5 = new System.Windows.Forms.Label();
            this.Line4 = new System.Windows.Forms.Label();
            this.Line3 = new System.Windows.Forms.Label();
            this.Line2 = new System.Windows.Forms.Label();
            this.Line1 = new System.Windows.Forms.Label();
            this.cmdDetailMode = new System.Windows.Forms.Button();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.fraScale.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblContext1
            // 
            this.lblContext1.AutoSize = true;
            this.lblContext1.Enabled = false;
            this.lblContext1.Location = new System.Drawing.Point(118, 8);
            this.lblContext1.Name = "lblContext1";
            this.lblContext1.Size = new System.Drawing.Size(0, 12);
            this.lblContext1.TabIndex = 0;
            this.lblContext1.TextChanged += new System.EventHandler(this.lblContext_TextChanged);
            this.lblContext1.Click += new System.EventHandler(this.lblContext_Click);
            // 
            // lblItem1
            // 
            this.lblItem1.AutoSize = true;
            this.lblItem1.Location = new System.Drawing.Point(8, 8);
            this.lblItem1.Name = "lblItem1";
            this.lblItem1.Size = new System.Drawing.Size(0, 12);
            this.lblItem1.TabIndex = 1;
            // 
            // lblColon1
            // 
            this.lblColon1.AutoSize = true;
            this.lblColon1.Location = new System.Drawing.Point(108, 8);
            this.lblColon1.Name = "lblColon1";
            this.lblColon1.Size = new System.Drawing.Size(7, 12);
            this.lblColon1.TabIndex = 2;
            this.lblColon1.Text = ":";
            // 
            // fraScale
            // 
            this.fraScale.Controls.Add(this.Line7);
            this.fraScale.Controls.Add(this.Line6);
            this.fraScale.Controls.Add(this.Line5);
            this.fraScale.Controls.Add(this.Line4);
            this.fraScale.Controls.Add(this.Line3);
            this.fraScale.Controls.Add(this.Line2);
            this.fraScale.Controls.Add(this.Line1);
            this.fraScale.Location = new System.Drawing.Point(16, 416);
            this.fraScale.Name = "fraScale";
            this.fraScale.Size = new System.Drawing.Size(105, 25);
            this.fraScale.TabIndex = 3;
            // 
            // Line7
            // 
            this.Line7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Line7.Location = new System.Drawing.Point(102, 0);
            this.Line7.Name = "Line7";
            this.Line7.Size = new System.Drawing.Size(1, 21);
            this.Line7.TabIndex = 13;
            // 
            // Line6
            // 
            this.Line6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Line6.Location = new System.Drawing.Point(80, 8);
            this.Line6.Name = "Line6";
            this.Line6.Size = new System.Drawing.Size(1, 14);
            this.Line6.TabIndex = 12;
            // 
            // Line5
            // 
            this.Line5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Line5.Location = new System.Drawing.Point(60, 8);
            this.Line5.Name = "Line5";
            this.Line5.Size = new System.Drawing.Size(1, 14);
            this.Line5.TabIndex = 11;
            // 
            // Line4
            // 
            this.Line4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Line4.Location = new System.Drawing.Point(40, 8);
            this.Line4.Name = "Line4";
            this.Line4.Size = new System.Drawing.Size(1, 14);
            this.Line4.TabIndex = 10;
            // 
            // Line3
            // 
            this.Line3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Line3.Location = new System.Drawing.Point(20, 8);
            this.Line3.Name = "Line3";
            this.Line3.Size = new System.Drawing.Size(1, 14);
            this.Line3.TabIndex = 9;
            // 
            // Line2
            // 
            this.Line2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Line2.Location = new System.Drawing.Point(0, 0);
            this.Line2.Name = "Line2";
            this.Line2.Size = new System.Drawing.Size(1, 21);
            this.Line2.TabIndex = 8;
            // 
            // Line1
            // 
            this.Line1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Line1.Location = new System.Drawing.Point(0, 21);
            this.Line1.Name = "Line1";
            this.Line1.Size = new System.Drawing.Size(103, 1);
            this.Line1.TabIndex = 7;
            // 
            // cmdDetailMode
            // 
            this.cmdDetailMode.Enabled = false;
            this.cmdDetailMode.Location = new System.Drawing.Point(136, 415);
            this.cmdDetailMode.Name = "cmdDetailMode";
            this.cmdDetailMode.Size = new System.Drawing.Size(84, 25);
            this.cmdDetailMode.TabIndex = 4;
            this.cmdDetailMode.Text = "詳細表示>>";
            this.cmdDetailMode.UseVisualStyleBackColor = true;
            this.cmdDetailMode.Click += new System.EventHandler(this.cmdDetailMode_Click);
            // 
            // frmImageInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(234, 454);
            this.Controls.Add(this.cmdDetailMode);
            this.Controls.Add(this.fraScale);
            this.Controls.Add(this.lblColon1);
            this.Controls.Add(this.lblItem1);
            this.Controls.Add(this.lblContext1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, -7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmImageInfo";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "画像情報";
            this.Activated += new System.EventHandler(this.frmImageInfo_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmImageInfo_FormClosing);
            this.Load += new System.EventHandler(this.frmImageInfo_Load);
            this.Resize += new System.EventHandler(this.frmImageInfo_Resize);
            this.fraScale.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblContext1;
		private System.Windows.Forms.Label lblItem1;
		private System.Windows.Forms.Label lblColon1;
		private System.Windows.Forms.Panel fraScale;
		private System.Windows.Forms.Button cmdDetailMode;
		private System.Windows.Forms.ToolTip ToolTip;
		private System.Windows.Forms.Label Line7;
		private System.Windows.Forms.Label Line6;
		private System.Windows.Forms.Label Line5;
		private System.Windows.Forms.Label Line4;
		private System.Windows.Forms.Label Line3;
		private System.Windows.Forms.Label Line2;
		private System.Windows.Forms.Label Line1;
	}
}