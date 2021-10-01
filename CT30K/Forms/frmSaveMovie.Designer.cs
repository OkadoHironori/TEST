namespace CT30K
{
    partial class frmSaveMovie
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
            this.cmdStart = new System.Windows.Forms.Button();
            this.cmdClose = new System.Windows.Forms.Button();
            this.fraFrameRate = new System.Windows.Forms.GroupBox();
            this._optFrameRate_2 = new System.Windows.Forms.RadioButton();
            this._optFrameRate_1 = new System.Windows.Forms.RadioButton();
            this.cwneSaveTime = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMaxSaveTime = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.StatusBar1 = new System.Windows.Forms.StatusStrip();
            this.tslblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tspgbSaveMovie = new System.Windows.Forms.ToolStripProgressBar();
            this.fraFrameRate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSaveTime)).BeginInit();
            this.StatusBar1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdStart
            // 
            this.cmdStart.Location = new System.Drawing.Point(263, 12);
            this.cmdStart.Name = "cmdStart";
            this.cmdStart.Size = new System.Drawing.Size(81, 25);
            this.cmdStart.TabIndex = 0;
            this.cmdStart.Tag = "10018";
            this.cmdStart.Text = "#開始";
            this.cmdStart.UseVisualStyleBackColor = true;
            this.cmdStart.Click += new System.EventHandler(this.cmdStart_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.Location = new System.Drawing.Point(263, 44);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(81, 25);
            this.cmdClose.TabIndex = 1;
            this.cmdClose.Tag = "10008";
            this.cmdClose.Text = "#閉じる";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // fraFrameRate
            // 
            this.fraFrameRate.Controls.Add(this._optFrameRate_2);
            this.fraFrameRate.Controls.Add(this._optFrameRate_1);
            this.fraFrameRate.Location = new System.Drawing.Point(8, 8);
            this.fraFrameRate.Name = "fraFrameRate";
            this.fraFrameRate.Size = new System.Drawing.Size(101, 61);
            this.fraFrameRate.TabIndex = 2;
            this.fraFrameRate.TabStop = false;
            this.fraFrameRate.Tag = "12573";
            this.fraFrameRate.Text = "#フレームレート";
            // 
            // _optFrameRate_2
            // 
            this._optFrameRate_2.AutoSize = true;
            this._optFrameRate_2.Location = new System.Drawing.Point(20, 20);
            this._optFrameRate_2.Name = "_optFrameRate_2";
            this._optFrameRate_2.Size = new System.Drawing.Size(48, 16);
            this._optFrameRate_2.TabIndex = 1;
            this._optFrameRate_2.Text = "15Hz";
            this._optFrameRate_2.UseVisualStyleBackColor = true;
            this._optFrameRate_2.CheckedChanged += new System.EventHandler(this.optFrameRate_CheckedChanged);
            // 
            // _optFrameRate_1
            // 
            this._optFrameRate_1.AutoSize = true;
            this._optFrameRate_1.Location = new System.Drawing.Point(20, 40);
            this._optFrameRate_1.Name = "_optFrameRate_1";
            this._optFrameRate_1.Size = new System.Drawing.Size(48, 16);
            this._optFrameRate_1.TabIndex = 0;
            this._optFrameRate_1.Text = "30Hz";
            this._optFrameRate_1.UseVisualStyleBackColor = true;
            this._optFrameRate_1.CheckedChanged += new System.EventHandler(this.optFrameRate_CheckedChanged);
            // 
            // cwneSaveTime
            // 
            this.cwneSaveTime.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneSaveTime.Location = new System.Drawing.Point(120, 12);
            this.cwneSaveTime.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.cwneSaveTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cwneSaveTime.Name = "cwneSaveTime";
            this.cwneSaveTime.Size = new System.Drawing.Size(82, 23);
            this.cwneSaveTime.TabIndex = 3;
            this.cwneSaveTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneSaveTime.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(119, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 12);
            this.label1.TabIndex = 4;
            this.label1.Tag = "12021";
            this.label1.Text = "最大：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMaxSaveTime
            // 
            this.lblMaxSaveTime.Location = new System.Drawing.Point(152, 44);
            this.lblMaxSaveTime.Name = "lblMaxSaveTime";
            this.lblMaxSaveTime.Size = new System.Drawing.Size(52, 20);
            this.lblMaxSaveTime.TabIndex = 5;
            this.lblMaxSaveTime.Text = "0";
            this.lblMaxSaveTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(216, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 6;
            this.label3.Tag = "12180";
            this.label3.Text = "秒";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(216, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 7;
            this.label4.Tag = "12180";
            this.label4.Text = "秒";
            // 
            // StatusBar1
            // 
            this.StatusBar1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslblStatus,
            this.tspgbSaveMovie});
            this.StatusBar1.Location = new System.Drawing.Point(0, 90);
            this.StatusBar1.Name = "StatusBar1";
            this.StatusBar1.Size = new System.Drawing.Size(365, 22);
            this.StatusBar1.SizingGrip = false;
            this.StatusBar1.TabIndex = 9;
            this.StatusBar1.Text = "statusStrip1";
            // 
            // tslblStatus
            // 
            this.tslblStatus.AutoSize = false;
            this.tslblStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tslblStatus.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tslblStatus.Name = "tslblStatus";
            this.tslblStatus.Size = new System.Drawing.Size(125, 17);
            this.tslblStatus.Text = "toolStripStatusLabel1";
            this.tslblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tspgbSaveMovie
            // 
            this.tspgbSaveMovie.AutoSize = false;
            this.tspgbSaveMovie.Name = "tspgbSaveMovie";
            this.tspgbSaveMovie.Size = new System.Drawing.Size(217, 16);
            // 
            // frmSaveMovie
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(365, 112);
            this.ControlBox = false;
            this.Controls.Add(this.StatusBar1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblMaxSaveTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cwneSaveTime);
            this.Controls.Add(this.fraFrameRate);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.cmdStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSaveMovie";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "12597";
            this.Text = "frmSaveMovie";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSaveMovie_FormClosed);
            this.Load += new System.EventHandler(this.frmSaveMovie_Load);
            this.Resize += new System.EventHandler(this.frmSaveMovie_Resize);
            this.fraFrameRate.ResumeLayout(false);
            this.fraFrameRate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSaveTime)).EndInit();
            this.StatusBar1.ResumeLayout(false);
            this.StatusBar1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdStart;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.GroupBox fraFrameRate;
        private System.Windows.Forms.RadioButton _optFrameRate_2;
        private System.Windows.Forms.RadioButton _optFrameRate_1;
        private System.Windows.Forms.NumericUpDown cwneSaveTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMaxSaveTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.StatusStrip StatusBar1;
        private System.Windows.Forms.ToolStripStatusLabel tslblStatus;
        private System.Windows.Forms.ToolStripProgressBar tspgbSaveMovie;
    }
}