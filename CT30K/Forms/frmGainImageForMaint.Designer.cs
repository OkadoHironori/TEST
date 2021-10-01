namespace CT30K
{
	partial class frmGainImageForMaint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGainImageForMaint));
            this.fraChange = new System.Windows.Forms.Panel();
            this.cwsldWidth = new CT30K.CTSliderH();
            this.cmdClose = new System.Windows.Forms.Button();
            this.cwsldLevel = new CT30K.CTSliderH();
            this.fraScale = new System.Windows.Forms.GroupBox();
            this.optScale0 = new System.Windows.Forms.RadioButton();
            this.optScale1 = new System.Windows.Forms.RadioButton();
            this.optScale2 = new System.Windows.Forms.RadioButton();
            this.lblWL = new System.Windows.Forms.Label();
            this.lblWW = new System.Windows.Forms.Label();
            this.lblLevel = new System.Windows.Forms.Label();
            this.lblWidth = new System.Windows.Forms.Label();
            this.ctlTransImage = new CT30K.CTImageCanvas();
            this.fraChange.SuspendLayout();
            this.fraScale.SuspendLayout();
            this.SuspendLayout();
            // 
            // fraChange
            // 
            this.fraChange.Controls.Add(this.cwsldWidth);
            this.fraChange.Controls.Add(this.cmdClose);
            this.fraChange.Controls.Add(this.cwsldLevel);
            this.fraChange.Controls.Add(this.fraScale);
            this.fraChange.Controls.Add(this.lblWL);
            this.fraChange.Controls.Add(this.lblWW);
            this.fraChange.Controls.Add(this.lblLevel);
            this.fraChange.Controls.Add(this.lblWidth);
            this.fraChange.Location = new System.Drawing.Point(152, 0);
            this.fraChange.Name = "fraChange";
            this.fraChange.Size = new System.Drawing.Size(201, 317);
            this.fraChange.TabIndex = 0;
            // 
            // cwsldWidth
            // 
            this.cwsldWidth.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwsldWidth.LargeChange = 5;
            this.cwsldWidth.Location = new System.Drawing.Point(8, 100);
            this.cwsldWidth.Maximum = 256;
            this.cwsldWidth.Minimum = 1;
            this.cwsldWidth.Name = "cwsldWidth";
            this.cwsldWidth.Reverse = false;
            this.cwsldWidth.Size = new System.Drawing.Size(189, 33);
            this.cwsldWidth.SmallChange = 1;
            this.cwsldWidth.TabIndex = 133;
            this.cwsldWidth.TicksLabel = CT30K.CTSliderH.TicksLabelType.TicksLabelMidOff;
            this.cwsldWidth.TicksLine = false;
            this.cwsldWidth.Value = 1;
            this.cwsldWidth.ValueChanged += new System.EventHandler(this.cwsldWidth_ValueChanged);
            // 
            // cmdClose
            // 
            this.cmdClose.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.cmdClose.Location = new System.Drawing.Point(52, 268);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(105, 33);
            this.cmdClose.TabIndex = 9;
            this.cmdClose.Tag = "10008";
            this.cmdClose.Text = "#閉じる";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // cwsldLevel
            // 
            this.cwsldLevel.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwsldLevel.LargeChange = 5;
            this.cwsldLevel.Location = new System.Drawing.Point(8, 40);
            this.cwsldLevel.Maximum = 255;
            this.cwsldLevel.Minimum = 0;
            this.cwsldLevel.Name = "cwsldLevel";
            this.cwsldLevel.Reverse = false;
            this.cwsldLevel.Size = new System.Drawing.Size(189, 33);
            this.cwsldLevel.SmallChange = 1;
            this.cwsldLevel.TabIndex = 134;
            this.cwsldLevel.TicksLabel = CT30K.CTSliderH.TicksLabelType.TicksLabelMidOff;
            this.cwsldLevel.TicksLine = false;
            this.cwsldLevel.Value = 1;
            this.cwsldLevel.ValueChanged += new System.EventHandler(this.cwsldLevel_ValueChanged);
            // 
            // fraScale
            // 
            this.fraScale.Controls.Add(this.optScale0);
            this.fraScale.Controls.Add(this.optScale1);
            this.fraScale.Controls.Add(this.optScale2);
            this.fraScale.Font = new System.Drawing.Font("MS UI Gothic", 9.75F);
            this.fraScale.Location = new System.Drawing.Point(56, 148);
            this.fraScale.Name = "fraScale";
            this.fraScale.Size = new System.Drawing.Size(97, 85);
            this.fraScale.TabIndex = 5;
            this.fraScale.TabStop = false;
            this.fraScale.Tag = "12749";
            this.fraScale.Text = "#倍率";
            // 
            // optScale0
            // 
            this.optScale0.AutoSize = true;
            this.optScale0.Location = new System.Drawing.Point(16, 20);
            this.optScale0.Name = "optScale0";
            this.optScale0.Size = new System.Drawing.Size(53, 17);
            this.optScale0.TabIndex = 8;
            this.optScale0.TabStop = true;
            this.optScale0.Text = " 1 倍";
            this.optScale0.UseVisualStyleBackColor = true;
            this.optScale0.CheckedChanged += new System.EventHandler(this.optScale_CheckedChanged);
            // 
            // optScale1
            // 
            this.optScale1.AutoSize = true;
            this.optScale1.Location = new System.Drawing.Point(16, 40);
            this.optScale1.Name = "optScale1";
            this.optScale1.Size = new System.Drawing.Size(53, 17);
            this.optScale1.TabIndex = 7;
            this.optScale1.TabStop = true;
            this.optScale1.Text = " 4 倍";
            this.optScale1.UseVisualStyleBackColor = true;
            this.optScale1.CheckedChanged += new System.EventHandler(this.optScale_CheckedChanged);
            // 
            // optScale2
            // 
            this.optScale2.AutoSize = true;
            this.optScale2.Location = new System.Drawing.Point(16, 60);
            this.optScale2.Name = "optScale2";
            this.optScale2.Size = new System.Drawing.Size(56, 17);
            this.optScale2.TabIndex = 6;
            this.optScale2.TabStop = true;
            this.optScale2.Text = "16 倍";
            this.optScale2.UseVisualStyleBackColor = true;
            this.optScale2.CheckedChanged += new System.EventHandler(this.optScale_CheckedChanged);
            // 
            // lblWL
            // 
            this.lblWL.AutoSize = true;
            this.lblWL.Font = new System.Drawing.Font("MS UI Gothic", 9.75F);
            this.lblWL.Location = new System.Drawing.Point(16, 24);
            this.lblWL.Name = "lblWL";
            this.lblWL.Size = new System.Drawing.Size(85, 13);
            this.lblWL.TabIndex = 4;
            this.lblWL.Tag = "12825";
            this.lblWL.Text = "ウィンドウレベル";
            // 
            // lblWW
            // 
            this.lblWW.AutoSize = true;
            this.lblWW.Font = new System.Drawing.Font("MS UI Gothic", 9.75F);
            this.lblWW.Location = new System.Drawing.Point(16, 84);
            this.lblWW.Name = "lblWW";
            this.lblWW.Size = new System.Drawing.Size(66, 13);
            this.lblWW.TabIndex = 3;
            this.lblWW.Tag = "12826";
            this.lblWW.Text = "ウィンドウ幅";
            // 
            // lblLevel
            // 
            this.lblLevel.Location = new System.Drawing.Point(100, 24);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(50, 12);
            this.lblLevel.TabIndex = 2;
            this.lblLevel.Text = "128";
            this.lblLevel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblWidth
            // 
            this.lblWidth.Location = new System.Drawing.Point(100, 84);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(50, 12);
            this.lblWidth.TabIndex = 1;
            this.lblWidth.Text = "256";
            this.lblWidth.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ctlTransImage
            // 
            this.ctlTransImage.BackColor = System.Drawing.Color.Black;
            this.ctlTransImage.FCDFIDRate = 0D;
            this.ctlTransImage.Location = new System.Drawing.Point(21, 34);
            this.ctlTransImage.MirrorOn = false;
            this.ctlTransImage.Name = "ctlTransImage";
            this.ctlTransImage.Picture = ((System.Drawing.Bitmap)(resources.GetObject("ctlTransImage.Picture")));
            this.ctlTransImage.Size = new System.Drawing.Size(101, 150);
            this.ctlTransImage.SizeX = 1024;
            this.ctlTransImage.SizeY = 1024;
            this.ctlTransImage.TabIndex = 1;
            // 
            // frmGainImageForMaint
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(367, 323);
            this.Controls.Add(this.ctlTransImage);
            this.Controls.Add(this.fraChange);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(0, -7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGainImageForMaint";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Tag = "12740";
            this.Text = "#ゲイン校正画像";
            this.Load += new System.EventHandler(this.frmGainImageForMaint_Load);
            this.fraChange.ResumeLayout(false);
            this.fraChange.PerformLayout();
            this.fraScale.ResumeLayout(false);
            this.fraScale.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel fraChange;
		private System.Windows.Forms.Label lblWidth;
		private System.Windows.Forms.Label lblLevel;
		private System.Windows.Forms.Label lblWW;
		private System.Windows.Forms.Label lblWL;
		private System.Windows.Forms.GroupBox fraScale;
		private System.Windows.Forms.RadioButton optScale0;
		private System.Windows.Forms.RadioButton optScale1;
		private System.Windows.Forms.RadioButton optScale2;
        private System.Windows.Forms.Button cmdClose;
        private CTImageCanvas ctlTransImage;
        private CTSliderH cwsldWidth;
        private CTSliderH cwsldLevel;
	}
}