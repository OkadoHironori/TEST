namespace CT30K
{
    partial class frmFInteg
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
            this.cmdExe = new System.Windows.Forms.Button();
            this.cmdEnd = new System.Windows.Forms.Button();
            this.lblIntegNumHeader = new System.Windows.Forms.Label();
            this.hsbInteg = new System.Windows.Forms.HScrollBar();
            this.lblMin = new System.Windows.Forms.Label();
            this.lblMax = new System.Windows.Forms.Label();
            this.lblIntegStatus = new System.Windows.Forms.Label();
            this.lblInteg = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // cmdExe
            // 
            this.cmdExe.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdExe.Location = new System.Drawing.Point(32, 116);
            this.cmdExe.Name = "cmdExe";
            this.cmdExe.Size = new System.Drawing.Size(129, 33);
            this.cmdExe.TabIndex = 0;
            this.cmdExe.Tag = "10005";
            this.cmdExe.Text = "#実行";
            this.cmdExe.UseVisualStyleBackColor = true;
            this.cmdExe.Click += new System.EventHandler(this.cmdExe_Click);
            // 
            // cmdEnd
            // 
            this.cmdEnd.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdEnd.Location = new System.Drawing.Point(197, 116);
            this.cmdEnd.Name = "cmdEnd";
            this.cmdEnd.Size = new System.Drawing.Size(129, 33);
            this.cmdEnd.TabIndex = 1;
            this.cmdEnd.Tag = "10008";
            this.cmdEnd.Text = "#閉じる";
            this.cmdEnd.UseVisualStyleBackColor = true;
            this.cmdEnd.Click += new System.EventHandler(this.cmdEnd_Click);
            // 
            // lblIntegNumHeader
            // 
            this.lblIntegNumHeader.AutoSize = true;
            this.lblIntegNumHeader.BackColor = System.Drawing.SystemColors.Control;
            this.lblIntegNumHeader.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblIntegNumHeader.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblIntegNumHeader.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblIntegNumHeader.Location = new System.Drawing.Point(16, 12);
            this.lblIntegNumHeader.Name = "lblIntegNumHeader";
            this.lblIntegNumHeader.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblIntegNumHeader.Size = new System.Drawing.Size(88, 16);
            this.lblIntegNumHeader.TabIndex = 2;
            this.lblIntegNumHeader.Tag = "12763";
            this.lblIntegNumHeader.Text = "#積算枚数：";
            // 
            // hsbInteg
            // 
            this.hsbInteg.Cursor = System.Windows.Forms.Cursors.Default;
            this.hsbInteg.Location = new System.Drawing.Point(203, 9);
            this.hsbInteg.Maximum = 256;
            this.hsbInteg.Minimum = 1;
            this.hsbInteg.Name = "hsbInteg";
            this.hsbInteg.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.hsbInteg.Size = new System.Drawing.Size(145, 25);
            this.hsbInteg.TabIndex = 3;
            this.hsbInteg.Tag = "";
            this.hsbInteg.Value = 1;
            this.hsbInteg.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsbInteg_Scroll);
            this.hsbInteg.ValueChanged += new System.EventHandler(this.hsbInteg_ValueChanged);
            // 
            // lblMin
            // 
            this.lblMin.AutoSize = true;
            this.lblMin.BackColor = System.Drawing.SystemColors.Control;
            this.lblMin.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblMin.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMin.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMin.Location = new System.Drawing.Point(211, 40);
            this.lblMin.Name = "lblMin";
            this.lblMin.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMin.Size = new System.Drawing.Size(11, 12);
            this.lblMin.TabIndex = 4;
            this.lblMin.Tag = "";
            this.lblMin.Text = "1";
            // 
            // lblMax
            // 
            this.lblMax.AutoSize = true;
            this.lblMax.BackColor = System.Drawing.SystemColors.Control;
            this.lblMax.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblMax.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMax.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMax.Location = new System.Drawing.Point(323, 40);
            this.lblMax.Name = "lblMax";
            this.lblMax.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMax.Size = new System.Drawing.Size(23, 12);
            this.lblMax.TabIndex = 5;
            this.lblMax.Tag = "";
            this.lblMax.Text = "256";
            // 
            // lblIntegStatus
            // 
            this.lblIntegStatus.BackColor = System.Drawing.SystemColors.Control;
            this.lblIntegStatus.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblIntegStatus.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblIntegStatus.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblIntegStatus.Location = new System.Drawing.Point(256, 76);
            this.lblIntegStatus.Name = "lblIntegStatus";
            this.lblIntegStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblIntegStatus.Size = new System.Drawing.Size(89, 25);
            this.lblIntegStatus.TabIndex = 6;
            this.lblIntegStatus.Tag = "12572";
            this.lblIntegStatus.Text = "#積分中";
            this.lblIntegStatus.Visible = false;
            // 
            // lblInteg
            // 
            this.lblInteg.BackColor = System.Drawing.Color.Transparent;
            this.lblInteg.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblInteg.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblInteg.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblInteg.Location = new System.Drawing.Point(101, 12);
            this.lblInteg.Name = "lblInteg";
            this.lblInteg.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblInteg.Size = new System.Drawing.Size(92, 16);
            this.lblInteg.TabIndex = 8;
            this.lblInteg.Text = "1 枚";
            this.lblInteg.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(33, 74);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(209, 23);
            this.progressBar1.TabIndex = 9;
            // 
            // timer1
            // 
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmFInteg
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(360, 161);
            this.ControlBox = false;
            this.Controls.Add(this.lblIntegNumHeader);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblInteg);
            this.Controls.Add(this.lblIntegStatus);
            this.Controls.Add(this.lblMax);
            this.Controls.Add(this.lblMin);
            this.Controls.Add(this.hsbInteg);
            this.Controls.Add(this.cmdEnd);
            this.Controls.Add(this.cmdExe);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFInteg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "12580";
            this.Text = "画像積分";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmFInteg_FormClosed);
            this.Load += new System.EventHandler(this.frmFInteg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdExe;
        private System.Windows.Forms.Button cmdEnd;
        public System.Windows.Forms.Label lblIntegNumHeader;
        public System.Windows.Forms.HScrollBar hsbInteg;
        public System.Windows.Forms.Label lblMin;
        public System.Windows.Forms.Label lblMax;
        public System.Windows.Forms.Label lblIntegStatus;
        public System.Windows.Forms.Label lblInteg;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer timer1;
    }
}