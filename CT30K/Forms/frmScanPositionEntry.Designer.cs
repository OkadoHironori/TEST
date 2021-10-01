namespace CT30K
{
    partial class frmScanPositionEntry
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
            this.Label7 = new System.Windows.Forms.Label();
            this.lblColon1 = new System.Windows.Forms.Label();
            this.cwneSum = new System.Windows.Forms.NumericUpDown();
            this.lblIntegNum = new System.Windows.Forms.Label();
            this.pgbScanPos = new System.Windows.Forms.ProgressBar();
            this.stsScanPos = new CT30K.CTStatus();
            this.lblMessage = new System.Windows.Forms.Label();
            this.cmdEnd = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.fraMode = new System.Windows.Forms.GroupBox();
            this.optManual = new System.Windows.Forms.RadioButton();
            this.optAuto = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSum)).BeginInit();
            this.fraMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Label7.Location = new System.Drawing.Point(264, 147);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(32, 16);
            this.Label7.TabIndex = 18;
            this.Label7.Tag = "10818";
            this.Label7.Text = "#枚";
            // 
            // lblColon1
            // 
            this.lblColon1.AutoSize = true;
            this.lblColon1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblColon1.Location = new System.Drawing.Point(162, 147);
            this.lblColon1.Name = "lblColon1";
            this.lblColon1.Size = new System.Drawing.Size(11, 16);
            this.lblColon1.TabIndex = 17;
            this.lblColon1.Text = ":";
            // 
            // cwneSum
            // 
            this.cwneSum.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneSum.Location = new System.Drawing.Point(176, 144);
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
            this.cwneSum.TabIndex = 16;
            this.cwneSum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneSum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblIntegNum
            // 
            this.lblIntegNum.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblIntegNum.Location = new System.Drawing.Point(11, 147);
            this.lblIntegNum.Name = "lblIntegNum";
            this.lblIntegNum.Size = new System.Drawing.Size(150, 18);
            this.lblIntegNum.TabIndex = 15;
            this.lblIntegNum.Tag = "12809";
            this.lblIntegNum.Text = "#積算枚数";
            this.lblIntegNum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pgbScanPos
            // 
            this.pgbScanPos.Location = new System.Drawing.Point(136, 112);
            this.pgbScanPos.Name = "pgbScanPos";
            this.pgbScanPos.Size = new System.Drawing.Size(181, 21);
            this.pgbScanPos.TabIndex = 14;
            // 
            // stsScanPos
            // 
            this.stsScanPos.Caption = "#ステータス";
            this.stsScanPos.CaptionAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.stsScanPos.CaptionWidth = 128;
            this.stsScanPos.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stsScanPos.Location = new System.Drawing.Point(0, 88);
            this.stsScanPos.Name = "stsScanPos";
            this.stsScanPos.Size = new System.Drawing.Size(312, 18);
            this.stsScanPos.Status = "";
            this.stsScanPos.TabIndex = 13;
            this.stsScanPos.Tag = "12033";
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMessage.Location = new System.Drawing.Point(8, 9);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(305, 73);
            this.lblMessage.TabIndex = 12;
            this.lblMessage.Tag = "9906";
            this.lblMessage.Text = "#lblMessage";
            // 
            // cmdEnd
            // 
            this.cmdEnd.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdEnd.Location = new System.Drawing.Point(184, 276);
            this.cmdEnd.Name = "cmdEnd";
            this.cmdEnd.Size = new System.Drawing.Size(105, 33);
            this.cmdEnd.TabIndex = 22;
            this.cmdEnd.Tag = "10006";
            this.cmdEnd.Text = "#終 了";
            this.cmdEnd.UseVisualStyleBackColor = true;
            this.cmdEnd.Click += new System.EventHandler(this.cmdEnd_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdOK.Location = new System.Drawing.Point(42, 276);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(105, 33);
            this.cmdOK.TabIndex = 21;
            this.cmdOK.Tag = "10001";
            this.cmdOK.Text = "#O K";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // fraMode
            // 
            this.fraMode.Controls.Add(this.optManual);
            this.fraMode.Controls.Add(this.optAuto);
            this.fraMode.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.fraMode.Location = new System.Drawing.Point(32, 168);
            this.fraMode.Name = "fraMode";
            this.fraMode.Size = new System.Drawing.Size(257, 89);
            this.fraMode.TabIndex = 23;
            this.fraMode.TabStop = false;
            this.fraMode.Tag = "12254";
            this.fraMode.Text = "#モード";
            // 
            // optManual
            // 
            this.optManual.AutoSize = true;
            this.optManual.Checked = true;
            this.optManual.Location = new System.Drawing.Point(16, 56);
            this.optManual.Name = "optManual";
            this.optManual.Size = new System.Drawing.Size(66, 20);
            this.optManual.TabIndex = 1;
            this.optManual.TabStop = true;
            this.optManual.Tag = "12024";
            this.optManual.Text = "#手動";
            this.optManual.UseVisualStyleBackColor = true;
            // 
            // optAuto
            // 
            this.optAuto.AutoSize = true;
            this.optAuto.Location = new System.Drawing.Point(16, 24);
            this.optAuto.Name = "optAuto";
            this.optAuto.Size = new System.Drawing.Size(66, 20);
            this.optAuto.TabIndex = 0;
            this.optAuto.Tag = "12023";
            this.optAuto.Text = "#自動";
            this.optAuto.UseVisualStyleBackColor = true;
            // 
            // frmScanPositionEntry
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(325, 325);
            this.ControlBox = false;
            this.Controls.Add(this.fraMode);
            this.Controls.Add(this.cmdEnd);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.lblColon1);
            this.Controls.Add(this.cwneSum);
            this.Controls.Add(this.lblIntegNum);
            this.Controls.Add(this.pgbScanPos);
            this.Controls.Add(this.stsScanPos);
            this.Controls.Add(this.lblMessage);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmScanPositionEntry";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "10901";
            this.Text = "#スキャン位置校正";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmScanPositionEntry_FormClosed);
            this.Load += new System.EventHandler(this.frmScanPositionEntry_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cwneSum)).EndInit();
            this.fraMode.ResumeLayout(false);
            this.fraMode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Label7;
        private System.Windows.Forms.Label lblColon1;
        private System.Windows.Forms.NumericUpDown cwneSum;
        private System.Windows.Forms.Label lblIntegNum;
        private System.Windows.Forms.ProgressBar pgbScanPos;
        private CTStatus stsScanPos;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button cmdEnd;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.GroupBox fraMode;
        private System.Windows.Forms.RadioButton optManual;
        private System.Windows.Forms.RadioButton optAuto;
    }
}