namespace CT30K
{
	partial class frmComInit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmComInit));
            this.Image1 = new System.Windows.Forms.PictureBox();
            this.cmdYes = new System.Windows.Forms.Button();
            this.cmdNo = new System.Windows.Forms.Button();
            this.lblComInitMes = new System.Windows.Forms.Label();
            this.lblComInit = new System.Windows.Forms.Label();
            this.chkComInit = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.Image1)).BeginInit();
            this.SuspendLayout();
            // 
            // Image1
            // 
            this.Image1.Image = ((System.Drawing.Image)(resources.GetObject("Image1.Image")));
            this.Image1.Location = new System.Drawing.Point(8, 8);
            this.Image1.Name = "Image1";
            this.Image1.Size = new System.Drawing.Size(32, 32);
            this.Image1.TabIndex = 0;
            this.Image1.TabStop = false;
            // 
            // cmdYes
            // 
            this.cmdYes.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdYes.Location = new System.Drawing.Point(124, 104);
            this.cmdYes.Name = "cmdYes";
            this.cmdYes.Size = new System.Drawing.Size(81, 25);
            this.cmdYes.TabIndex = 1;
            this.cmdYes.Tag = "10003";
            this.cmdYes.Text = "#はい";
            this.cmdYes.UseVisualStyleBackColor = true;
            this.cmdYes.Click += new System.EventHandler(this.cmdYes_Click);
            // 
            // cmdNo
            // 
            this.cmdNo.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdNo.Location = new System.Drawing.Point(220, 104);
            this.cmdNo.Name = "cmdNo";
            this.cmdNo.Size = new System.Drawing.Size(81, 25);
            this.cmdNo.TabIndex = 2;
            this.cmdNo.Tag = "10004";
            this.cmdNo.Text = "#いいえ";
            this.cmdNo.UseVisualStyleBackColor = true;
            this.cmdNo.Click += new System.EventHandler(this.cmdNo_Click);
            // 
            // lblComInitMes
            // 
            this.lblComInitMes.Location = new System.Drawing.Point(52, 8);
            this.lblComInitMes.Name = "lblComInitMes";
            this.lblComInitMes.Size = new System.Drawing.Size(250, 56);
            this.lblComInitMes.TabIndex = 3;
            this.lblComInitMes.Tag = "9404";
            this.lblComInitMes.Text = "#コモン初期化を行うと、スキャン条件等の設定が初期状態に戻ります。\r\n\r\nよろしいですか？";
            // 
            // lblComInit
            // 
            this.lblComInit.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblComInit.Location = new System.Drawing.Point(8, 96);
            this.lblComInit.Name = "lblComInit";
            this.lblComInit.Size = new System.Drawing.Size(110, 29);
            this.lblComInit.TabIndex = 4;
            this.lblComInit.Tag = "12185";
            this.lblComInit.Text = "#コモン初期化中";
            this.lblComInit.Visible = false;
            // 
            // chkComInit
            // 
            this.chkComInit.Location = new System.Drawing.Point(48, 76);
            this.chkComInit.Name = "chkComInit";
            this.chkComInit.Size = new System.Drawing.Size(201, 16);
            this.chkComInit.TabIndex = 0;
            this.chkComInit.Tag = "12151";
            this.chkComInit.Text = "#校正結果も初期化する";
            this.chkComInit.UseVisualStyleBackColor = true;
            // 
            // frmComInit
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(312, 141);
            this.ControlBox = false;
            this.Controls.Add(this.chkComInit);
            this.Controls.Add(this.lblComInit);
            this.Controls.Add(this.lblComInitMes);
            this.Controls.Add(this.cmdNo);
            this.Controls.Add(this.cmdYes);
            this.Controls.Add(this.Image1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(0, -7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmComInit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "12133";
            this.Text = "#コモン初期化";
            this.Load += new System.EventHandler(this.frmComInit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Image1)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox Image1;
		private System.Windows.Forms.Button cmdYes;
		private System.Windows.Forms.Button cmdNo;
		private System.Windows.Forms.Label lblComInitMes;
		private System.Windows.Forms.Label lblComInit;
		private System.Windows.Forms.CheckBox chkComInit;
	}
}