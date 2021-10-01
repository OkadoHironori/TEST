namespace CT30K
{
    partial class frmRotationCenter
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
            this.lblMessage = new System.Windows.Forms.Label();
            this.pgbRotationCenter = new System.Windows.Forms.ProgressBar();
            this.Label7 = new System.Windows.Forms.Label();
            this.lblColon2 = new System.Windows.Forms.Label();
            this.cwneSum = new System.Windows.Forms.NumericUpDown();
            this.lblIntegNum = new System.Windows.Forms.Label();
            this.lblViewMinMax = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.lblColon1 = new System.Windows.Forms.Label();
            this.cwneScanView = new System.Windows.Forms.NumericUpDown();
            this.lblViewNum = new System.Windows.Forms.Label();
            this.chkShading = new System.Windows.Forms.CheckBox();
            this.cmdEnd = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.lblSlicePixUni = new System.Windows.Forms.Label();
            this.lblColon3 = new System.Windows.Forms.Label();
            this.cwneSlicePix = new System.Windows.Forms.NumericUpDown();
            this.Label5 = new System.Windows.Forms.Label();
            this.lblMultiTube = new System.Windows.Forms.Label();
            this.cboMultiTube = new System.Windows.Forms.ComboBox();
            this.lblIntegMinMax = new System.Windows.Forms.Label();
            this.lblSlicePixMinMax = new System.Windows.Forms.Label();
            this.stsRotCor = new CT30K.CTStatus();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneScanView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSlicePix)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMessage.Location = new System.Drawing.Point(12, 9);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(345, 48);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Tag = "9906";
            this.lblMessage.Text = "#lblMessage";
            // 
            // pgbRotationCenter
            // 
            this.pgbRotationCenter.Location = new System.Drawing.Point(176, 96);
            this.pgbRotationCenter.Name = "pgbRotationCenter";
            this.pgbRotationCenter.Size = new System.Drawing.Size(181, 21);
            this.pgbRotationCenter.TabIndex = 3;
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Label7.Location = new System.Drawing.Point(264, 166);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(23, 12);
            this.Label7.TabIndex = 20;
            this.Label7.Tag = "10818";
            this.Label7.Text = "#枚";
            // 
            // lblColon2
            // 
            this.lblColon2.AutoSize = true;
            this.lblColon2.Font = new System.Drawing.Font("Arial", 10F);
            this.lblColon2.Location = new System.Drawing.Point(163, 160);
            this.lblColon2.Name = "lblColon2";
            this.lblColon2.Size = new System.Drawing.Size(12, 16);
            this.lblColon2.TabIndex = 19;
            this.lblColon2.Text = ":";
            // 
            // cwneSum
            // 
            this.cwneSum.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneSum.Location = new System.Drawing.Point(176, 160);
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
            this.cwneSum.TabIndex = 18;
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
            this.lblIntegNum.Location = new System.Drawing.Point(15, 160);
            this.lblIntegNum.Name = "lblIntegNum";
            this.lblIntegNum.Size = new System.Drawing.Size(137, 18);
            this.lblIntegNum.TabIndex = 17;
            this.lblIntegNum.Tag = "12809";
            this.lblIntegNum.Text = "#積算枚数";
            this.lblIntegNum.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblViewMinMax
            // 
            this.lblViewMinMax.AutoSize = true;
            this.lblViewMinMax.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblViewMinMax.Location = new System.Drawing.Point(304, 131);
            this.lblViewMinMax.Name = "lblViewMinMax";
            this.lblViewMinMax.Size = new System.Drawing.Size(67, 12);
            this.lblViewMinMax.TabIndex = 16;
            this.lblViewMinMax.Text = "(400～2400)";
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Label9.Location = new System.Drawing.Point(264, 131);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(34, 12);
            this.Label9.TabIndex = 15;
            this.Label9.Tag = "10831";
            this.Label9.Text = "#ﾋﾞｭｰ";
            // 
            // lblColon1
            // 
            this.lblColon1.AutoSize = true;
            this.lblColon1.Font = new System.Drawing.Font("Arial", 10F);
            this.lblColon1.Location = new System.Drawing.Point(163, 120);
            this.lblColon1.Name = "lblColon1";
            this.lblColon1.Size = new System.Drawing.Size(12, 16);
            this.lblColon1.TabIndex = 14;
            this.lblColon1.Text = ":";
            // 
            // cwneScanView
            // 
            this.cwneScanView.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneScanView.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.cwneScanView.Location = new System.Drawing.Point(176, 120);
            this.cwneScanView.Name = "cwneScanView";
            this.cwneScanView.Size = new System.Drawing.Size(81, 24);
            this.cwneScanView.TabIndex = 13;
            this.cwneScanView.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneScanView.ValueChanged += new System.EventHandler(this.cwneScanView_ValueChanged);
            // 
            // lblViewNum
            // 
            this.lblViewNum.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblViewNum.Location = new System.Drawing.Point(18, 120);
            this.lblViewNum.Name = "lblViewNum";
            this.lblViewNum.Size = new System.Drawing.Size(133, 23);
            this.lblViewNum.TabIndex = 12;
            this.lblViewNum.Tag = "12808";
            this.lblViewNum.Text = "#ビュー数";
            this.lblViewNum.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkShading
            // 
            this.chkShading.AutoSize = true;
            this.chkShading.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.chkShading.Location = new System.Drawing.Point(32, 280);
            this.chkShading.Name = "chkShading";
            this.chkShading.Size = new System.Drawing.Size(162, 20);
            this.chkShading.TabIndex = 23;
            this.chkShading.Tag = "12056";
            this.chkShading.Text = "#シェーディング補正";
            this.chkShading.UseVisualStyleBackColor = true;
            // 
            // cmdEnd
            // 
            this.cmdEnd.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdEnd.Location = new System.Drawing.Point(244, 316);
            this.cmdEnd.Name = "cmdEnd";
            this.cmdEnd.Size = new System.Drawing.Size(105, 33);
            this.cmdEnd.TabIndex = 25;
            this.cmdEnd.Tag = "10006";
            this.cmdEnd.Text = "#終 了";
            this.cmdEnd.UseVisualStyleBackColor = true;
            this.cmdEnd.Click += new System.EventHandler(this.cmdEnd_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdOK.Location = new System.Drawing.Point(100, 316);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(105, 33);
            this.cmdOK.TabIndex = 24;
            this.cmdOK.Tag = "10001";
            this.cmdOK.Text = "#O K";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // lblSlicePixUni
            // 
            this.lblSlicePixUni.AutoSize = true;
            this.lblSlicePixUni.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSlicePixUni.Location = new System.Drawing.Point(263, 204);
            this.lblSlicePixUni.Name = "lblSlicePixUni";
            this.lblSlicePixUni.Size = new System.Drawing.Size(35, 12);
            this.lblSlicePixUni.TabIndex = 29;
            this.lblSlicePixUni.Tag = "10820";
            this.lblSlicePixUni.Text = "#画素";
            // 
            // lblColon3
            // 
            this.lblColon3.AutoSize = true;
            this.lblColon3.Font = new System.Drawing.Font("Arial", 10F);
            this.lblColon3.Location = new System.Drawing.Point(163, 200);
            this.lblColon3.Name = "lblColon3";
            this.lblColon3.Size = new System.Drawing.Size(12, 16);
            this.lblColon3.TabIndex = 28;
            this.lblColon3.Text = ":";
            // 
            // cwneSlicePix
            // 
            this.cwneSlicePix.DecimalPlaces = 3;
            this.cwneSlicePix.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cwneSlicePix.Location = new System.Drawing.Point(176, 200);
            this.cwneSlicePix.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cwneSlicePix.Name = "cwneSlicePix";
            this.cwneSlicePix.Size = new System.Drawing.Size(81, 23);
            this.cwneSlicePix.TabIndex = 27;
            this.cwneSlicePix.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneSlicePix.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Label5
            // 
            this.Label5.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Label5.Location = new System.Drawing.Point(12, 200);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(138, 17);
            this.Label5.TabIndex = 26;
            this.Label5.Tag = "12812";
            this.Label5.Text = "#スライス厚";
            this.Label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblMultiTube
            // 
            this.lblMultiTube.AutoSize = true;
            this.lblMultiTube.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMultiTube.Location = new System.Drawing.Point(93, 240);
            this.lblMultiTube.Name = "lblMultiTube";
            this.lblMultiTube.Size = new System.Drawing.Size(58, 16);
            this.lblMultiTube.TabIndex = 30;
            this.lblMultiTube.Tag = "12218";
            this.lblMultiTube.Text = "#Ｘ線管";
            // 
            // cboMultiTube
            // 
            this.cboMultiTube.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cboMultiTube.FormattingEnabled = true;
            this.cboMultiTube.Location = new System.Drawing.Point(176, 240);
            this.cboMultiTube.Name = "cboMultiTube";
            this.cboMultiTube.Size = new System.Drawing.Size(81, 24);
            this.cboMultiTube.TabIndex = 32;
            // 
            // lblIntegMinMax
            // 
            this.lblIntegMinMax.AutoSize = true;
            this.lblIntegMinMax.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblIntegMinMax.Location = new System.Drawing.Point(304, 165);
            this.lblIntegMinMax.Name = "lblIntegMinMax";
            this.lblIntegMinMax.Size = new System.Drawing.Size(43, 12);
            this.lblIntegMinMax.TabIndex = 33;
            this.lblIntegMinMax.Text = "(1～20)";
            // 
            // lblSlicePixMinMax
            // 
            this.lblSlicePixMinMax.AutoSize = true;
            this.lblSlicePixMinMax.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSlicePixMinMax.Location = new System.Drawing.Point(303, 205);
            this.lblSlicePixMinMax.Name = "lblSlicePixMinMax";
            this.lblSlicePixMinMax.Size = new System.Drawing.Size(43, 12);
            this.lblSlicePixMinMax.TabIndex = 34;
            this.lblSlicePixMinMax.Text = "(1～48)";
            // 
            // stsRotCor
            // 
            this.stsRotCor.Caption = "#ステータス";
            this.stsRotCor.CaptionAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.stsRotCor.CaptionWidth = 140;
            this.stsRotCor.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stsRotCor.Location = new System.Drawing.Point(24, 72);
            this.stsRotCor.Name = "stsRotCor";
            this.stsRotCor.Size = new System.Drawing.Size(336, 18);
            this.stsRotCor.Status = "";
            this.stsRotCor.TabIndex = 2;
            this.stsRotCor.Tag = "12033";
            // 
            // frmRotationCenter
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(387, 366);
            this.ControlBox = false;
            this.Controls.Add(this.lblSlicePixMinMax);
            this.Controls.Add(this.lblIntegMinMax);
            this.Controls.Add(this.cboMultiTube);
            this.Controls.Add(this.lblMultiTube);
            this.Controls.Add(this.lblSlicePixUni);
            this.Controls.Add(this.lblColon3);
            this.Controls.Add(this.cwneSlicePix);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.cmdEnd);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.chkShading);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.lblColon2);
            this.Controls.Add(this.cwneSum);
            this.Controls.Add(this.lblIntegNum);
            this.Controls.Add(this.lblViewMinMax);
            this.Controls.Add(this.Label9);
            this.Controls.Add(this.lblColon1);
            this.Controls.Add(this.cwneScanView);
            this.Controls.Add(this.lblViewNum);
            this.Controls.Add(this.pgbRotationCenter);
            this.Controls.Add(this.stsRotCor);
            this.Controls.Add(this.lblMessage);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmRotationCenter";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "10903";
            this.Text = "#回転中心校正";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmRotationCenter_FormClosed);
            this.Load += new System.EventHandler(this.frmRotationCenter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cwneSum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneScanView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSlicePix)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private CTStatus stsRotCor;
        private System.Windows.Forms.ProgressBar pgbRotationCenter;
        private System.Windows.Forms.Label Label7;
        private System.Windows.Forms.Label lblColon2;
        private System.Windows.Forms.NumericUpDown cwneSum;
        private System.Windows.Forms.Label lblIntegNum;
        private System.Windows.Forms.Label lblViewMinMax;
        private System.Windows.Forms.Label Label9;
        private System.Windows.Forms.Label lblColon1;
        private System.Windows.Forms.NumericUpDown cwneScanView;
        private System.Windows.Forms.Label lblViewNum;
        private System.Windows.Forms.CheckBox chkShading;
        private System.Windows.Forms.Button cmdEnd;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Label lblSlicePixUni;
        private System.Windows.Forms.Label lblColon3;
        private System.Windows.Forms.NumericUpDown cwneSlicePix;
        private System.Windows.Forms.Label Label5;
        private System.Windows.Forms.Label lblMultiTube;
        private System.Windows.Forms.ComboBox cboMultiTube;
        private System.Windows.Forms.Label lblIntegMinMax;
        private System.Windows.Forms.Label lblSlicePixMinMax;
    }
}