namespace CT30K
{
    partial class frmOffset
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
            this.cmdEnd = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.Label7 = new System.Windows.Forms.Label();
            this.lblColon1 = new System.Windows.Forms.Label();
            this.cwneSum = new System.Windows.Forms.NumericUpDown();
            this.lblIntegNum = new System.Windows.Forms.Label();
            this.pgbOffset = new System.Windows.Forms.ProgressBar();
            this.lblMessage = new System.Windows.Forms.Label();
            this.stsOffset = new CT30K.CTStatus();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSum)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdEnd
            // 
            this.cmdEnd.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdEnd.Location = new System.Drawing.Point(184, 188);
            this.cmdEnd.Name = "cmdEnd";
            this.cmdEnd.Size = new System.Drawing.Size(105, 33);
            this.cmdEnd.TabIndex = 31;
            this.cmdEnd.Tag = "10006";
            this.cmdEnd.Text = "#終 了";
            this.cmdEnd.UseVisualStyleBackColor = true;
            this.cmdEnd.Click += new System.EventHandler(this.cmdEnd_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdOK.Location = new System.Drawing.Point(44, 188);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(105, 33);
            this.cmdOK.TabIndex = 30;
            this.cmdOK.Tag = "10001";
            this.cmdOK.Text = "#O K";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Label7.Location = new System.Drawing.Point(208, 148);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(32, 16);
            this.Label7.TabIndex = 29;
            this.Label7.Tag = "10818";
            this.Label7.Text = "#枚";
            // 
            // lblColon1
            // 
            this.lblColon1.AutoSize = true;
            this.lblColon1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblColon1.Location = new System.Drawing.Point(108, 141);
            this.lblColon1.Name = "lblColon1";
            this.lblColon1.Size = new System.Drawing.Size(15, 24);
            this.lblColon1.TabIndex = 28;
            this.lblColon1.Text = ":";
            // 
            // cwneSum
            // 
            this.cwneSum.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneSum.Location = new System.Drawing.Point(124, 140);
            this.cwneSum.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.cwneSum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cwneSum.Name = "cwneSum";
            this.cwneSum.Size = new System.Drawing.Size(81, 24);
            this.cwneSum.TabIndex = 27;
            this.cwneSum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneSum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblIntegNum
            // 
            this.lblIntegNum.AutoSize = true;
            this.lblIntegNum.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblIntegNum.Location = new System.Drawing.Point(33, 144);
            this.lblIntegNum.Name = "lblIntegNum";
            this.lblIntegNum.Size = new System.Drawing.Size(80, 16);
            this.lblIntegNum.TabIndex = 26;
            this.lblIntegNum.Tag = "12809";
            this.lblIntegNum.Text = "#積算枚数";
            // 
            // pgbOffset
            // 
            this.pgbOffset.Location = new System.Drawing.Point(120, 112);
            this.pgbOffset.Name = "pgbOffset";
            this.pgbOffset.Size = new System.Drawing.Size(181, 21);
            this.pgbOffset.TabIndex = 25;
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMessage.Location = new System.Drawing.Point(16, 16);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(297, 57);
            this.lblMessage.TabIndex = 23;
            this.lblMessage.Tag = "9906";
            this.lblMessage.Text = "#lblMessage";
            // 
            // stsOffset
            // 
            this.stsOffset.Caption = "#ステータス";
            this.stsOffset.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.stsOffset.CaptionWidth = 128;
            this.stsOffset.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stsOffset.Location = new System.Drawing.Point(-16, 88);
            this.stsOffset.Name = "stsOffset";
            this.stsOffset.Size = new System.Drawing.Size(312, 18);
            this.stsOffset.Status = "";
            this.stsOffset.TabIndex = 24;
            this.stsOffset.Tag = "12033";
            // 
            // frmOffset
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(341, 241);
            this.ControlBox = false;
            this.Controls.Add(this.lblIntegNum);
            this.Controls.Add(this.cmdEnd);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.cwneSum);
            this.Controls.Add(this.pgbOffset);
            this.Controls.Add(this.stsOffset);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblColon1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmOffset";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "10905";
            this.Text = "#オフセット校正";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmOffset_FormClosed);
            this.Load += new System.EventHandler(this.frmOffset_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cwneSum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdEnd;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Label Label7;
        private System.Windows.Forms.Label lblColon1;
        private System.Windows.Forms.NumericUpDown cwneSum;
        private System.Windows.Forms.Label lblIntegNum;
        private System.Windows.Forms.ProgressBar pgbOffset;
        private CTStatus stsOffset;
        private System.Windows.Forms.Label lblMessage;
    }
}