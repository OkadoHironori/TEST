namespace CT30K
{
    partial class frmVertical
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
            this.cwneMA = new System.Windows.Forms.NumericUpDown();
            this.lblUDUni = new System.Windows.Forms.Label();
            this.cwneDownTableDistance = new System.Windows.Forms.TextBox();
            this.chkDownTable = new System.Windows.Forms.CheckBox();
            this.cmdOK = new System.Windows.Forms.Button();
            this.lblColon3 = new System.Windows.Forms.Label();
            this.lblMAUni = new System.Windows.Forms.Label();
            this.lblMAName = new System.Windows.Forms.Label();
            this.cmdEnd = new System.Windows.Forms.Button();
            this.lblColon1 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.pgbVertical = new System.Windows.Forms.ProgressBar();
            this.lblMessage = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.lblColon2 = new System.Windows.Forms.Label();
            this.cwneSum = new System.Windows.Forms.NumericUpDown();
            this.lblIntegNum = new System.Windows.Forms.Label();
            this.chkShading = new System.Windows.Forms.CheckBox();
            this.cwneWireDistance = new System.Windows.Forms.TextBox();
            this.stsVertical = new CT30K.CTStatus();
            this.fraTubeCurrent = new System.Windows.Forms.Panel();
            this.fraTableDownAcquire = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.cwneMA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSum)).BeginInit();
            this.fraTubeCurrent.SuspendLayout();
            this.fraTableDownAcquire.SuspendLayout();
            this.SuspendLayout();
            // 
            // cwneMA
            // 
            this.cwneMA.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneMA.Location = new System.Drawing.Point(156, 8);
            this.cwneMA.Name = "cwneMA";
            this.cwneMA.Size = new System.Drawing.Size(81, 24);
            this.cwneMA.TabIndex = 17;
            this.cwneMA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblUDUni
            // 
            this.lblUDUni.AutoSize = true;
            this.lblUDUni.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblUDUni.Location = new System.Drawing.Point(264, 16);
            this.lblUDUni.Name = "lblUDUni";
            this.lblUDUni.Size = new System.Drawing.Size(23, 12);
            this.lblUDUni.TabIndex = 24;
            this.lblUDUni.Text = "mm";
            // 
            // cwneDownTableDistance
            // 
            this.cwneDownTableDistance.BackColor = System.Drawing.Color.White;
            this.cwneDownTableDistance.Enabled = false;
            this.cwneDownTableDistance.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneDownTableDistance.Location = new System.Drawing.Point(192, 8);
            this.cwneDownTableDistance.Name = "cwneDownTableDistance";
            this.cwneDownTableDistance.Size = new System.Drawing.Size(65, 23);
            this.cwneDownTableDistance.TabIndex = 23;
            this.cwneDownTableDistance.Text = "1";
            this.cwneDownTableDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneDownTableDistance.TextChanged += new System.EventHandler(this.cwneDownTableDistance_ValueChanged);
            // 
            // chkDownTable
            // 
            this.chkDownTable.AutoSize = true;
            this.chkDownTable.Checked = true;
            this.chkDownTable.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.chkDownTable.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkDownTable.Location = new System.Drawing.Point(12, 8);
            this.chkDownTable.Name = "chkDownTable";
            this.chkDownTable.Size = new System.Drawing.Size(158, 20);
            this.chkDownTable.TabIndex = 22;
            this.chkDownTable.Tag = "12116";
            this.chkDownTable.Text = "#テーブル下降収集";
            this.chkDownTable.UseVisualStyleBackColor = true;
            this.chkDownTable.CheckedChanged += new System.EventHandler(this.chkDownTable_CheckStateChanged);
            // 
            // cmdOK
            // 
            this.cmdOK.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdOK.Location = new System.Drawing.Point(40, 324);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(105, 33);
            this.cmdOK.TabIndex = 37;
            this.cmdOK.Tag = "10001";
            this.cmdOK.Text = "#O K";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // lblColon3
            // 
            this.lblColon3.AutoSize = true;
            this.lblColon3.Font = new System.Drawing.Font("Arial", 10F);
            this.lblColon3.Location = new System.Drawing.Point(142, 12);
            this.lblColon3.Name = "lblColon3";
            this.lblColon3.Size = new System.Drawing.Size(12, 16);
            this.lblColon3.TabIndex = 18;
            this.lblColon3.Text = ":";
            // 
            // lblMAUni
            // 
            this.lblMAUni.AutoSize = true;
            this.lblMAUni.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMAUni.Location = new System.Drawing.Point(244, 16);
            this.lblMAUni.Name = "lblMAUni";
            this.lblMAUni.Size = new System.Drawing.Size(25, 12);
            this.lblMAUni.TabIndex = 19;
            this.lblMAUni.Text = "μA";
            // 
            // lblMAName
            // 
            this.lblMAName.AutoSize = true;
            this.lblMAName.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMAName.Location = new System.Drawing.Point(15, 13);
            this.lblMAName.Name = "lblMAName";
            this.lblMAName.Size = new System.Drawing.Size(64, 16);
            this.lblMAName.TabIndex = 16;
            this.lblMAName.Tag = "12807";
            this.lblMAName.Text = "#管電流";
            // 
            // cmdEnd
            // 
            this.cmdEnd.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdEnd.Location = new System.Drawing.Point(184, 324);
            this.cmdEnd.Name = "cmdEnd";
            this.cmdEnd.Size = new System.Drawing.Size(105, 33);
            this.cmdEnd.TabIndex = 38;
            this.cmdEnd.Tag = "10006";
            this.cmdEnd.Text = "#終 了";
            this.cmdEnd.UseVisualStyleBackColor = true;
            this.cmdEnd.Click += new System.EventHandler(this.cmdEnd_Click);
            // 
            // lblColon1
            // 
            this.lblColon1.AutoSize = true;
            this.lblColon1.Font = new System.Drawing.Font("Arial", 10F);
            this.lblColon1.Location = new System.Drawing.Point(147, 128);
            this.lblColon1.Name = "lblColon1";
            this.lblColon1.Size = new System.Drawing.Size(12, 16);
            this.lblColon1.TabIndex = 29;
            this.lblColon1.Text = ":";
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Label8.Location = new System.Drawing.Point(232, 160);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(28, 16);
            this.Label8.TabIndex = 30;
            this.Label8.Tag = "";
            this.Label8.Text = "cm";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Label5.Location = new System.Drawing.Point(16, 161);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(115, 16);
            this.Label5.TabIndex = 27;
            this.Label5.Tag = "";
            this.Label5.Text = "垂直線ﾜｲﾔﾋﾟｯﾁ";
            // 
            // pgbVertical
            // 
            this.pgbVertical.Location = new System.Drawing.Point(148, 96);
            this.pgbVertical.Name = "pgbVertical";
            this.pgbVertical.Size = new System.Drawing.Size(177, 21);
            this.pgbVertical.TabIndex = 26;
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMessage.Location = new System.Drawing.Point(16, 16);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(305, 57);
            this.lblMessage.TabIndex = 24;
            this.lblMessage.Tag = "9906";
            this.lblMessage.Text = "#lblMessage";
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.Label7.Location = new System.Drawing.Point(248, 128);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(32, 16);
            this.Label7.TabIndex = 44;
            this.Label7.Tag = "10818";
            this.Label7.Text = "#枚";
            // 
            // lblColon2
            // 
            this.lblColon2.AutoSize = true;
            this.lblColon2.Font = new System.Drawing.Font("Arial", 10F);
            this.lblColon2.Location = new System.Drawing.Point(147, 160);
            this.lblColon2.Name = "lblColon2";
            this.lblColon2.Size = new System.Drawing.Size(12, 16);
            this.lblColon2.TabIndex = 43;
            this.lblColon2.Text = ":";
            // 
            // cwneSum
            // 
            this.cwneSum.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneSum.Location = new System.Drawing.Point(160, 124);
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
            this.cwneSum.TabIndex = 42;
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
            this.lblIntegNum.Location = new System.Drawing.Point(16, 129);
            this.lblIntegNum.Name = "lblIntegNum";
            this.lblIntegNum.Size = new System.Drawing.Size(80, 16);
            this.lblIntegNum.TabIndex = 41;
            this.lblIntegNum.Tag = "12809";
            this.lblIntegNum.Text = "#積算枚数";
            // 
            // chkShading
            // 
            this.chkShading.AutoSize = true;
            this.chkShading.Checked = true;
            this.chkShading.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShading.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.chkShading.Location = new System.Drawing.Point(16, 248);
            this.chkShading.Name = "chkShading";
            this.chkShading.Size = new System.Drawing.Size(162, 20);
            this.chkShading.TabIndex = 45;
            this.chkShading.Tag = "12056";
            this.chkShading.Text = "#シェーディング補正";
            this.chkShading.UseVisualStyleBackColor = true;
            // 
            // cwneWireDistance
            // 
            this.cwneWireDistance.BackColor = System.Drawing.Color.White;
            this.cwneWireDistance.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12.75F);
            this.cwneWireDistance.Location = new System.Drawing.Point(160, 156);
            this.cwneWireDistance.Name = "cwneWireDistance";
            this.cwneWireDistance.ReadOnly = true;
            this.cwneWireDistance.Size = new System.Drawing.Size(66, 24);
            this.cwneWireDistance.TabIndex = 46;
            this.cwneWireDistance.Text = "0";
            this.cwneWireDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // stsVertical
            // 
            this.stsVertical.Caption = "#ステータス";
            this.stsVertical.CaptionAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.stsVertical.CaptionWidth = 130;
            this.stsVertical.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stsVertical.Location = new System.Drawing.Point(13, 76);
            this.stsVertical.Name = "stsVertical";
            this.stsVertical.Size = new System.Drawing.Size(312, 18);
            this.stsVertical.Status = "";
            this.stsVertical.TabIndex = 25;
            this.stsVertical.Tag = "12033";
            // 
            // fraTubeCurrent
            // 
            this.fraTubeCurrent.Controls.Add(this.lblMAUni);
            this.fraTubeCurrent.Controls.Add(this.lblMAName);
            this.fraTubeCurrent.Controls.Add(this.lblColon3);
            this.fraTubeCurrent.Controls.Add(this.cwneMA);
            this.fraTubeCurrent.Location = new System.Drawing.Point(4, 182);
            this.fraTubeCurrent.Name = "fraTubeCurrent";
            this.fraTubeCurrent.Size = new System.Drawing.Size(313, 40);
            this.fraTubeCurrent.TabIndex = 47;
            // 
            // fraTableDownAcquire
            // 
            this.fraTableDownAcquire.Controls.Add(this.lblUDUni);
            this.fraTableDownAcquire.Controls.Add(this.chkDownTable);
            this.fraTableDownAcquire.Controls.Add(this.cwneDownTableDistance);
            this.fraTableDownAcquire.Location = new System.Drawing.Point(4, 272);
            this.fraTableDownAcquire.Name = "fraTableDownAcquire";
            this.fraTableDownAcquire.Size = new System.Drawing.Size(318, 38);
            this.fraTableDownAcquire.TabIndex = 48;
            // 
            // frmVertical
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(331, 369);
            this.ControlBox = false;
            this.Controls.Add(this.fraTableDownAcquire);
            this.Controls.Add(this.fraTubeCurrent);
            this.Controls.Add(this.cwneWireDistance);
            this.Controls.Add(this.chkShading);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.lblColon2);
            this.Controls.Add(this.cwneSum);
            this.Controls.Add(this.lblIntegNum);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdEnd);
            this.Controls.Add(this.lblColon1);
            this.Controls.Add(this.Label8);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.pgbVertical);
            this.Controls.Add(this.stsVertical);
            this.Controls.Add(this.lblMessage);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmVertical";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "10902";
            this.Text = "#幾何歪校正";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmVertical_FormClosed);
            this.Load += new System.EventHandler(this.frmVertical_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cwneMA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSum)).EndInit();
            this.fraTubeCurrent.ResumeLayout(false);
            this.fraTubeCurrent.PerformLayout();
            this.fraTableDownAcquire.ResumeLayout(false);
            this.fraTableDownAcquire.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown cwneMA;
        private System.Windows.Forms.Label lblUDUni;
        private System.Windows.Forms.TextBox cwneDownTableDistance;
        private System.Windows.Forms.CheckBox chkDownTable;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Label lblColon3;
        private System.Windows.Forms.Label lblMAUni;
        private System.Windows.Forms.Label lblMAName;
        private System.Windows.Forms.Button cmdEnd;
        private System.Windows.Forms.Label lblColon1;
        private System.Windows.Forms.Label Label8;
        private System.Windows.Forms.Label Label5;
        private System.Windows.Forms.ProgressBar pgbVertical;
        private CTStatus stsVertical;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label Label7;
        private System.Windows.Forms.Label lblColon2;
        private System.Windows.Forms.NumericUpDown cwneSum;
        private System.Windows.Forms.Label lblIntegNum;
        private System.Windows.Forms.CheckBox chkShading;
        private System.Windows.Forms.TextBox cwneWireDistance;
        private System.Windows.Forms.Panel fraTubeCurrent;
        private System.Windows.Forms.Panel fraTableDownAcquire;
    }
}