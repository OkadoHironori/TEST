namespace CT30K
{
	partial class frmQueryWarmup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQueryWarmup));
            this.Image1 = new System.Windows.Forms.PictureBox();
            this.cwneWarmupSetVolt = new System.Windows.Forms.NumericUpDown();
            this.cmdYes = new System.Windows.Forms.Button();
            this.cmdNo = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Image1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneWarmupSetVolt)).BeginInit();
            this.SuspendLayout();
            // 
            // Image1
            // 
            this.Image1.Image = ((System.Drawing.Image)(resources.GetObject("Image1.Image")));
            this.Image1.Location = new System.Drawing.Point(4, 8);
            this.Image1.Name = "Image1";
            this.Image1.Size = new System.Drawing.Size(32, 32);
            this.Image1.TabIndex = 0;
            this.Image1.TabStop = false;
            // 
            // cwneWarmupSetVolt
            // 
            this.cwneWarmupSetVolt.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneWarmupSetVolt.Location = new System.Drawing.Point(232, 8);
            this.cwneWarmupSetVolt.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.cwneWarmupSetVolt.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.cwneWarmupSetVolt.Name = "cwneWarmupSetVolt";
            this.cwneWarmupSetVolt.Size = new System.Drawing.Size(57, 23);
            this.cwneWarmupSetVolt.TabIndex = 0;
            this.cwneWarmupSetVolt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cmdYes
            // 
            this.cmdYes.Location = new System.Drawing.Point(176, 96);
            this.cmdYes.Name = "cmdYes";
            this.cmdYes.Size = new System.Drawing.Size(69, 25);
            this.cmdYes.TabIndex = 1;
            this.cmdYes.Tag = "10003";
            this.cmdYes.Text = "はい";
            this.cmdYes.UseVisualStyleBackColor = true;
            this.cmdYes.Click += new System.EventHandler(this.cmdYes_Click);
            // 
            // cmdNo
            // 
            this.cmdNo.Location = new System.Drawing.Point(260, 96);
            this.cmdNo.Name = "cmdNo";
            this.cmdNo.Size = new System.Drawing.Size(69, 25);
            this.cmdNo.TabIndex = 2;
            this.cmdNo.Tag = "10004";
            this.cmdNo.Text = "いいえ";
            this.cmdNo.UseVisualStyleBackColor = true;
            this.cmdNo.Click += new System.EventHandler(this.cmdNo_Click);
            // 
            // Label1
            // 
            this.Label1.Location = new System.Drawing.Point(40, 8);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(181, 29);
            this.Label1.TabIndex = 3;
            this.Label1.Tag = "20137";
            this.Label1.Text = "Ｘ線のウォームアップが必要です。\r\n今すぐウォームアップしますか？";
            // 
            // Label2
            // 
            this.Label2.Location = new System.Drawing.Point(292, 18);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(41, 17);
            this.Label2.TabIndex = 4;
            this.Label2.Tag = "20138";
            this.Label2.Text = "kVまで";
            // 
            // Label3
            // 
            this.Label3.AutoEllipsis = true;
            this.Label3.Location = new System.Drawing.Point(40, 48);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(281, 37);
            this.Label3.TabIndex = 5;
            this.Label3.Tag = "20139";
            this.Label3.Text = "運転準備スイッチを押し、検査室の試料扉を閉じてから\r\n「はい」をクリックしてください。";
            // 
            // Label4
            // 
            this.Label4.Location = new System.Drawing.Point(264, 32);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(33, 17);
            this.Label4.TabIndex = 6;
            this.Label4.Tag = "20173";
            this.Label4.Text = "Up to";
            this.Label4.Visible = false;
            // 
            // Label5
            // 
            this.Label5.Location = new System.Drawing.Point(296, 32);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(25, 17);
            this.Label5.TabIndex = 7;
            this.Label5.Tag = "10813";
            this.Label5.Text = "kV";
            this.Label5.Visible = false;
            // 
            // frmQueryWarmup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(337, 128);
            this.ControlBox = false;
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.cmdNo);
            this.Controls.Add(this.cmdYes);
            this.Controls.Add(this.cwneWarmupSetVolt);
            this.Controls.Add(this.Image1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmQueryWarmup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "CT30K";
            ((System.ComponentModel.ISupportInitialize)(this.Image1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneWarmupSetVolt)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox Image1;
		private System.Windows.Forms.NumericUpDown cwneWarmupSetVolt;
		private System.Windows.Forms.Button cmdYes;
		private System.Windows.Forms.Button cmdNo;
		private System.Windows.Forms.Label Label1;
		private System.Windows.Forms.Label Label2;
		private System.Windows.Forms.Label Label3;
		private System.Windows.Forms.Label Label4;
		private System.Windows.Forms.Label Label5;
	}
}