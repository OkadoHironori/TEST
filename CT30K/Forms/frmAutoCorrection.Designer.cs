namespace CT30K
{
    partial class frmAutoCorrection
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
            this.pgbAutoCorrect = new System.Windows.Forms.ProgressBar();
            this.fraGain = new System.Windows.Forms.GroupBox();
            this.chkXAxisMove = new System.Windows.Forms.CheckBox();
            this.chkYAxisMove = new System.Windows.Forms.CheckBox();
            this.lblGMAUni = new System.Windows.Forms.Label();
            this.Label21 = new System.Windows.Forms.Label();
            this.lblViewGainMinMax = new System.Windows.Forms.Label();
            this.Label18 = new System.Windows.Forms.Label();
            this.chkGainTableRot = new System.Windows.Forms.CheckBox();
            this.lblMAName0 = new System.Windows.Forms.Label();
            this.Label19 = new System.Windows.Forms.Label();
            this.cwneGMA = new System.Windows.Forms.NumericUpDown();
            this.cwneSumGain = new System.Windows.Forms.NumericUpDown();
            this.cwneViewGain = new System.Windows.Forms.NumericUpDown();
            this.Label16 = new System.Windows.Forms.Label();
            this.lblColon9 = new System.Windows.Forms.Label();
            this.lblColon8 = new System.Windows.Forms.Label();
            this.lblColon7 = new System.Windows.Forms.Label();
            this.fraShiftScan = new System.Windows.Forms.Panel();
            this.chkHaFuOfScan = new System.Windows.Forms.CheckBox();
            this.chkShiftScan = new System.Windows.Forms.CheckBox();
            this.fraScanPosi = new System.Windows.Forms.GroupBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.cwneSumSp = new System.Windows.Forms.NumericUpDown();
            this.lblColon1 = new System.Windows.Forms.Label();
            this.chkSPCorrect = new System.Windows.Forms.CheckBox();
            this.fraVertical = new System.Windows.Forms.GroupBox();
            this.Label13 = new System.Windows.Forms.Label();
            this.chkShading = new System.Windows.Forms.CheckBox();
            this.cwneWireVer = new System.Windows.Forms.TextBox();
            this.Label11 = new System.Windows.Forms.Label();
            this.lblVMAUni = new System.Windows.Forms.Label();
            this.lblMAName2 = new System.Windows.Forms.Label();
            this.cwneVMA = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cwneSumVer = new System.Windows.Forms.NumericUpDown();
            this.lblColon5 = new System.Windows.Forms.Label();
            this.lblColon4 = new System.Windows.Forms.Label();
            this.lblColon3 = new System.Windows.Forms.Label();
            this.fraOffset = new System.Windows.Forms.GroupBox();
            this.Label27 = new System.Windows.Forms.Label();
            this.Label25 = new System.Windows.Forms.Label();
            this.cwneSumOff = new System.Windows.Forms.NumericUpDown();
            this.lblColon11 = new System.Windows.Forms.Label();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdEnd = new System.Windows.Forms.Button();
            this.chkAutoJdgResult = new System.Windows.Forms.CheckBox();
            this.stsCorrect3 = new CT30K.CTStatus();
            this.stsCorrect2 = new CT30K.CTStatus();
            this.stsCorrect1 = new CT30K.CTStatus();
            this.stsCorrect0 = new CT30K.CTStatus();
            this.fraGain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneGMA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSumGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneViewGain)).BeginInit();
            this.fraShiftScan.SuspendLayout();
            this.fraScanPosi.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSumSp)).BeginInit();
            this.fraVertical.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneVMA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSumVer)).BeginInit();
            this.fraOffset.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSumOff)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblMessage.Location = new System.Drawing.Point(24, 8);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(385, 41);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "#lblMessage";
            // 
            // pgbAutoCorrect
            // 
            this.pgbAutoCorrect.Location = new System.Drawing.Point(24, 32);
            this.pgbAutoCorrect.Name = "pgbAutoCorrect";
            this.pgbAutoCorrect.Size = new System.Drawing.Size(189, 21);
            this.pgbAutoCorrect.TabIndex = 1;
            this.pgbAutoCorrect.Visible = false;
            // 
            // fraGain
            // 
            this.fraGain.Controls.Add(this.chkXAxisMove);
            this.fraGain.Controls.Add(this.chkYAxisMove);
            this.fraGain.Controls.Add(this.lblGMAUni);
            this.fraGain.Controls.Add(this.Label21);
            this.fraGain.Controls.Add(this.lblViewGainMinMax);
            this.fraGain.Controls.Add(this.Label18);
            this.fraGain.Controls.Add(this.chkGainTableRot);
            this.fraGain.Controls.Add(this.lblMAName0);
            this.fraGain.Controls.Add(this.Label19);
            this.fraGain.Controls.Add(this.cwneGMA);
            this.fraGain.Controls.Add(this.cwneSumGain);
            this.fraGain.Controls.Add(this.cwneViewGain);
            this.fraGain.Controls.Add(this.Label16);
            this.fraGain.Controls.Add(this.stsCorrect0);
            this.fraGain.Controls.Add(this.lblColon9);
            this.fraGain.Controls.Add(this.lblColon8);
            this.fraGain.Controls.Add(this.lblColon7);
            this.fraGain.Controls.Add(this.fraShiftScan);
            this.fraGain.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.fraGain.Location = new System.Drawing.Point(24, 90);
            this.fraGain.Name = "fraGain";
            this.fraGain.Size = new System.Drawing.Size(385, 270);
            this.fraGain.TabIndex = 1;
            this.fraGain.TabStop = false;
            this.fraGain.Tag = "10904";
            this.fraGain.Text = "#ゲイン校正";
            // 
            // chkXAxisMove
            // 
            this.chkXAxisMove.AutoSize = true;
            this.chkXAxisMove.Location = new System.Drawing.Point(24, 170);
            this.chkXAxisMove.Name = "chkXAxisMove";
            this.chkXAxisMove.Size = new System.Drawing.Size(164, 20);
            this.chkXAxisMove.TabIndex = 16;
            this.chkXAxisMove.Text = "テーブルFCD軸移動";
            this.chkXAxisMove.UseVisualStyleBackColor = true;
            // 
            // chkYAxisMove
            // 
            this.chkYAxisMove.AutoSize = true;
            this.chkYAxisMove.Location = new System.Drawing.Point(24, 147);
            this.chkYAxisMove.Name = "chkYAxisMove";
            this.chkYAxisMove.Size = new System.Drawing.Size(143, 20);
            this.chkYAxisMove.TabIndex = 7;
            this.chkYAxisMove.Text = "テーブルY軸移動";
            this.chkYAxisMove.UseVisualStyleBackColor = true;
            // 
            // lblGMAUni
            // 
            this.lblGMAUni.AutoSize = true;
            this.lblGMAUni.Location = new System.Drawing.Point(248, 124);
            this.lblGMAUni.Name = "lblGMAUni";
            this.lblGMAUni.Size = new System.Drawing.Size(34, 16);
            this.lblGMAUni.TabIndex = 11;
            this.lblGMAUni.Text = "μA";
            // 
            // Label21
            // 
            this.Label21.AutoSize = true;
            this.Label21.Location = new System.Drawing.Point(248, 91);
            this.Label21.Name = "Label21";
            this.Label21.Size = new System.Drawing.Size(32, 16);
            this.Label21.TabIndex = 10;
            this.Label21.Tag = "10818";
            this.Label21.Text = "#枚";
            // 
            // lblViewGainMinMax
            // 
            this.lblViewGainMinMax.AutoSize = true;
            this.lblViewGainMinMax.Location = new System.Drawing.Point(288, 60);
            this.lblViewGainMinMax.Name = "lblViewGainMinMax";
            this.lblViewGainMinMax.Size = new System.Drawing.Size(90, 16);
            this.lblViewGainMinMax.TabIndex = 9;
            this.lblViewGainMinMax.Text = "(400～2400)";
            // 
            // Label18
            // 
            this.Label18.AutoSize = true;
            this.Label18.Location = new System.Drawing.Point(248, 60);
            this.Label18.Name = "Label18";
            this.Label18.Size = new System.Drawing.Size(46, 16);
            this.Label18.TabIndex = 8;
            this.Label18.Tag = "10831";
            this.Label18.Text = "#ﾋﾞｭｰ";
            // 
            // chkGainTableRot
            // 
            this.chkGainTableRot.AutoSize = true;
            this.chkGainTableRot.Location = new System.Drawing.Point(24, 241);
            this.chkGainTableRot.Name = "chkGainTableRot";
            this.chkGainTableRot.Size = new System.Drawing.Size(118, 20);
            this.chkGainTableRot.TabIndex = 6;
            this.chkGainTableRot.Text = "テーブル回転";
            this.chkGainTableRot.UseVisualStyleBackColor = true;
            this.chkGainTableRot.CheckedChanged += new System.EventHandler(this.chkGainTableRot_CheckStateChanged);
            // 
            // lblMAName0
            // 
            this.lblMAName0.AutoSize = true;
            this.lblMAName0.Location = new System.Drawing.Point(16, 120);
            this.lblMAName0.Name = "lblMAName0";
            this.lblMAName0.Size = new System.Drawing.Size(64, 16);
            this.lblMAName0.TabIndex = 6;
            this.lblMAName0.Tag = "12807";
            this.lblMAName0.Text = "#管電流";
            // 
            // Label19
            // 
            this.Label19.AutoSize = true;
            this.Label19.Location = new System.Drawing.Point(16, 88);
            this.Label19.Name = "Label19";
            this.Label19.Size = new System.Drawing.Size(80, 16);
            this.Label19.TabIndex = 5;
            this.Label19.Tag = "12809";
            this.Label19.Text = "#積算枚数";
            // 
            // cwneGMA
            // 
            this.cwneGMA.Location = new System.Drawing.Point(163, 116);
            this.cwneGMA.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.cwneGMA.Name = "cwneGMA";
            this.cwneGMA.Size = new System.Drawing.Size(81, 23);
            this.cwneGMA.TabIndex = 3;
            this.cwneGMA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cwneSumGain
            // 
            this.cwneSumGain.Location = new System.Drawing.Point(163, 84);
            this.cwneSumGain.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.cwneSumGain.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cwneSumGain.Name = "cwneSumGain";
            this.cwneSumGain.Size = new System.Drawing.Size(81, 23);
            this.cwneSumGain.TabIndex = 2;
            this.cwneSumGain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneSumGain.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cwneViewGain
            // 
            this.cwneViewGain.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.cwneViewGain.Location = new System.Drawing.Point(163, 52);
            this.cwneViewGain.Name = "cwneViewGain";
            this.cwneViewGain.Size = new System.Drawing.Size(81, 23);
            this.cwneViewGain.TabIndex = 1;
            this.cwneViewGain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneViewGain.ValueChanged += new System.EventHandler(this.cwneViewGain_ValueChanged);
            // 
            // Label16
            // 
            this.Label16.Location = new System.Drawing.Point(16, 56);
            this.Label16.Name = "Label16";
            this.Label16.Size = new System.Drawing.Size(129, 32);
            this.Label16.TabIndex = 1;
            this.Label16.Tag = "12808";
            this.Label16.Text = "#ビュー数";
            // 
            // lblColon9
            // 
            this.lblColon9.AutoSize = true;
            this.lblColon9.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 14F);
            this.lblColon9.Location = new System.Drawing.Point(144, 115);
            this.lblColon9.Name = "lblColon9";
            this.lblColon9.Size = new System.Drawing.Size(19, 19);
            this.lblColon9.TabIndex = 14;
            this.lblColon9.Tag = "";
            this.lblColon9.Text = "：";
            // 
            // lblColon8
            // 
            this.lblColon8.AutoSize = true;
            this.lblColon8.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 14F);
            this.lblColon8.Location = new System.Drawing.Point(144, 83);
            this.lblColon8.Name = "lblColon8";
            this.lblColon8.Size = new System.Drawing.Size(19, 19);
            this.lblColon8.TabIndex = 13;
            this.lblColon8.Tag = "";
            this.lblColon8.Text = "：";
            // 
            // lblColon7
            // 
            this.lblColon7.AutoSize = true;
            this.lblColon7.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 14F);
            this.lblColon7.Location = new System.Drawing.Point(144, 51);
            this.lblColon7.Name = "lblColon7";
            this.lblColon7.Size = new System.Drawing.Size(19, 19);
            this.lblColon7.TabIndex = 12;
            this.lblColon7.Tag = "";
            this.lblColon7.Text = "：";
            // 
            // fraShiftScan
            // 
            this.fraShiftScan.BackColor = System.Drawing.Color.Transparent;
            this.fraShiftScan.Controls.Add(this.chkHaFuOfScan);
            this.fraShiftScan.Controls.Add(this.chkShiftScan);
            this.fraShiftScan.Location = new System.Drawing.Point(16, 192);
            this.fraShiftScan.Name = "fraShiftScan";
            this.fraShiftScan.Size = new System.Drawing.Size(354, 47);
            this.fraShiftScan.TabIndex = 15;
            // 
            // chkHaFuOfScan
            // 
            this.chkHaFuOfScan.AutoSize = true;
            this.chkHaFuOfScan.Checked = true;
            this.chkHaFuOfScan.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHaFuOfScan.Location = new System.Drawing.Point(8, 3);
            this.chkHaFuOfScan.Name = "chkHaFuOfScan";
            this.chkHaFuOfScan.Size = new System.Drawing.Size(261, 20);
            this.chkHaFuOfScan.TabIndex = 4;
            this.chkHaFuOfScan.Tag = "";
            this.chkHaFuOfScan.Text = "#ハーフフルオフセットスキャン収集";
            this.chkHaFuOfScan.UseVisualStyleBackColor = true;
            this.chkHaFuOfScan.CheckStateChanged += new System.EventHandler(this.chkHaFuOfScan_CheckStateChanged);
            // 
            // chkShiftScan
            // 
            this.chkShiftScan.AutoSize = true;
            this.chkShiftScan.Location = new System.Drawing.Point(8, 26);
            this.chkShiftScan.Name = "chkShiftScan";
            this.chkShiftScan.Size = new System.Drawing.Size(160, 20);
            this.chkShiftScan.TabIndex = 5;
            this.chkShiftScan.Tag = "";
            this.chkShiftScan.Text = "#シフトスキャン収集";
            this.chkShiftScan.UseVisualStyleBackColor = true;
            this.chkShiftScan.CheckStateChanged += new System.EventHandler(this.chkShiftScan_CheckStateChanged);
            // 
            // fraScanPosi
            // 
            this.fraScanPosi.Controls.Add(this.Label5);
            this.fraScanPosi.Controls.Add(this.Label3);
            this.fraScanPosi.Controls.Add(this.cwneSumSp);
            this.fraScanPosi.Controls.Add(this.stsCorrect1);
            this.fraScanPosi.Controls.Add(this.lblColon1);
            this.fraScanPosi.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.fraScanPosi.Location = new System.Drawing.Point(24, 366);
            this.fraScanPosi.Name = "fraScanPosi";
            this.fraScanPosi.Size = new System.Drawing.Size(385, 85);
            this.fraScanPosi.TabIndex = 2;
            this.fraScanPosi.TabStop = false;
            this.fraScanPosi.Tag = "10901";
            this.fraScanPosi.Text = "#スキャン位置校正";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(248, 60);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(32, 16);
            this.Label5.TabIndex = 13;
            this.Label5.Tag = "10818";
            this.Label5.Text = "#枚";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(16, 56);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(80, 16);
            this.Label3.TabIndex = 12;
            this.Label3.Tag = "12809";
            this.Label3.Text = "#積算枚数";
            // 
            // cwneSumSp
            // 
            this.cwneSumSp.Location = new System.Drawing.Point(163, 52);
            this.cwneSumSp.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.cwneSumSp.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cwneSumSp.Name = "cwneSumSp";
            this.cwneSumSp.Size = new System.Drawing.Size(81, 23);
            this.cwneSumSp.TabIndex = 1;
            this.cwneSumSp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneSumSp.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblColon1
            // 
            this.lblColon1.AutoSize = true;
            this.lblColon1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 14F);
            this.lblColon1.Location = new System.Drawing.Point(144, 51);
            this.lblColon1.Name = "lblColon1";
            this.lblColon1.Size = new System.Drawing.Size(19, 19);
            this.lblColon1.TabIndex = 15;
            this.lblColon1.Tag = "";
            this.lblColon1.Text = "：";
            // 
            // chkSPCorrect
            // 
            this.chkSPCorrect.AutoSize = true;
            this.chkSPCorrect.Location = new System.Drawing.Point(4, 370);
            this.chkSPCorrect.Name = "chkSPCorrect";
            this.chkSPCorrect.Size = new System.Drawing.Size(15, 14);
            this.chkSPCorrect.TabIndex = 20;
            this.chkSPCorrect.TabStop = false;
            this.chkSPCorrect.UseVisualStyleBackColor = true;
            this.chkSPCorrect.CheckStateChanged += new System.EventHandler(this.chkSPCorrect_CheckStateChanged);
            // 
            // fraVertical
            // 
            this.fraVertical.Controls.Add(this.Label13);
            this.fraVertical.Controls.Add(this.chkShading);
            this.fraVertical.Controls.Add(this.cwneWireVer);
            this.fraVertical.Controls.Add(this.Label11);
            this.fraVertical.Controls.Add(this.lblVMAUni);
            this.fraVertical.Controls.Add(this.lblMAName2);
            this.fraVertical.Controls.Add(this.cwneVMA);
            this.fraVertical.Controls.Add(this.label1);
            this.fraVertical.Controls.Add(this.label2);
            this.fraVertical.Controls.Add(this.cwneSumVer);
            this.fraVertical.Controls.Add(this.stsCorrect2);
            this.fraVertical.Controls.Add(this.lblColon5);
            this.fraVertical.Controls.Add(this.lblColon4);
            this.fraVertical.Controls.Add(this.lblColon3);
            this.fraVertical.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.fraVertical.Location = new System.Drawing.Point(24, 462);
            this.fraVertical.Name = "fraVertical";
            this.fraVertical.Size = new System.Drawing.Size(385, 181);
            this.fraVertical.TabIndex = 3;
            this.fraVertical.TabStop = false;
            this.fraVertical.Tag = "10902";
            this.fraVertical.Text = "#幾何歪校正";
            // 
            // Label13
            // 
            this.Label13.AutoSize = true;
            this.Label13.Location = new System.Drawing.Point(232, 124);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(28, 16);
            this.Label13.TabIndex = 23;
            this.Label13.Text = "cm";
            // 
            // chkShading
            // 
            this.chkShading.AutoSize = true;
            this.chkShading.Location = new System.Drawing.Point(22, 156);
            this.chkShading.Name = "chkShading";
            this.chkShading.Size = new System.Drawing.Size(162, 20);
            this.chkShading.TabIndex = 3;
            this.chkShading.Tag = "12056";
            this.chkShading.Text = "#シェーディング補正";
            this.chkShading.UseVisualStyleBackColor = true;
            // 
            // cwneWireVer
            // 
            this.cwneWireVer.Location = new System.Drawing.Point(163, 116);
            this.cwneWireVer.Name = "cwneWireVer";
            this.cwneWireVer.ReadOnly = true;
            this.cwneWireVer.Size = new System.Drawing.Size(65, 23);
            this.cwneWireVer.TabIndex = 21;
            this.cwneWireVer.Text = "1";
            this.cwneWireVer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Label11
            // 
            this.Label11.AutoSize = true;
            this.Label11.Location = new System.Drawing.Point(16, 120);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(115, 16);
            this.Label11.TabIndex = 20;
            this.Label11.Tag = "";
            this.Label11.Text = "垂直線ﾜｲﾔﾋﾟｯﾁ";
            // 
            // lblVMAUni
            // 
            this.lblVMAUni.AutoSize = true;
            this.lblVMAUni.Location = new System.Drawing.Point(248, 91);
            this.lblVMAUni.Name = "lblVMAUni";
            this.lblVMAUni.Size = new System.Drawing.Size(34, 16);
            this.lblVMAUni.TabIndex = 19;
            this.lblVMAUni.Text = "μA";
            // 
            // lblMAName2
            // 
            this.lblMAName2.AutoSize = true;
            this.lblMAName2.Location = new System.Drawing.Point(16, 88);
            this.lblMAName2.Name = "lblMAName2";
            this.lblMAName2.Size = new System.Drawing.Size(64, 16);
            this.lblMAName2.TabIndex = 18;
            this.lblMAName2.Tag = "12807";
            this.lblMAName2.Text = "#管電流";
            // 
            // cwneVMA
            // 
            this.cwneVMA.Location = new System.Drawing.Point(163, 84);
            this.cwneVMA.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.cwneVMA.Name = "cwneVMA";
            this.cwneVMA.Size = new System.Drawing.Size(81, 23);
            this.cwneVMA.TabIndex = 2;
            this.cwneVMA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(248, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 16);
            this.label1.TabIndex = 16;
            this.label1.Tag = "10818";
            this.label1.Text = "#枚";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 16);
            this.label2.TabIndex = 15;
            this.label2.Tag = "12809";
            this.label2.Text = "#積算枚数";
            // 
            // cwneSumVer
            // 
            this.cwneSumVer.Location = new System.Drawing.Point(163, 52);
            this.cwneSumVer.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.cwneSumVer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cwneSumVer.Name = "cwneSumVer";
            this.cwneSumVer.Size = new System.Drawing.Size(81, 23);
            this.cwneSumVer.TabIndex = 1;
            this.cwneSumVer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneSumVer.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblColon5
            // 
            this.lblColon5.AutoSize = true;
            this.lblColon5.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 14F);
            this.lblColon5.Location = new System.Drawing.Point(144, 115);
            this.lblColon5.Name = "lblColon5";
            this.lblColon5.Size = new System.Drawing.Size(19, 19);
            this.lblColon5.TabIndex = 26;
            this.lblColon5.Tag = "";
            this.lblColon5.Text = "：";
            // 
            // lblColon4
            // 
            this.lblColon4.AutoSize = true;
            this.lblColon4.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 14F);
            this.lblColon4.Location = new System.Drawing.Point(144, 83);
            this.lblColon4.Name = "lblColon4";
            this.lblColon4.Size = new System.Drawing.Size(19, 19);
            this.lblColon4.TabIndex = 25;
            this.lblColon4.Tag = "";
            this.lblColon4.Text = "：";
            // 
            // lblColon3
            // 
            this.lblColon3.AutoSize = true;
            this.lblColon3.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 14F);
            this.lblColon3.Location = new System.Drawing.Point(144, 51);
            this.lblColon3.Name = "lblColon3";
            this.lblColon3.Size = new System.Drawing.Size(19, 19);
            this.lblColon3.TabIndex = 24;
            this.lblColon3.Tag = "";
            this.lblColon3.Text = "：";
            // 
            // fraOffset
            // 
            this.fraOffset.Controls.Add(this.Label27);
            this.fraOffset.Controls.Add(this.Label25);
            this.fraOffset.Controls.Add(this.cwneSumOff);
            this.fraOffset.Controls.Add(this.stsCorrect3);
            this.fraOffset.Controls.Add(this.lblColon11);
            this.fraOffset.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.fraOffset.Location = new System.Drawing.Point(24, 654);
            this.fraOffset.Name = "fraOffset";
            this.fraOffset.Size = new System.Drawing.Size(385, 85);
            this.fraOffset.TabIndex = 4;
            this.fraOffset.TabStop = false;
            this.fraOffset.Tag = "10905";
            this.fraOffset.Text = "#オフセット校正";
            // 
            // Label27
            // 
            this.Label27.AutoSize = true;
            this.Label27.Location = new System.Drawing.Point(248, 60);
            this.Label27.Name = "Label27";
            this.Label27.Size = new System.Drawing.Size(32, 16);
            this.Label27.TabIndex = 16;
            this.Label27.Tag = "10818";
            this.Label27.Text = "#枚";
            // 
            // Label25
            // 
            this.Label25.AutoSize = true;
            this.Label25.Location = new System.Drawing.Point(16, 56);
            this.Label25.Name = "Label25";
            this.Label25.Size = new System.Drawing.Size(80, 16);
            this.Label25.TabIndex = 15;
            this.Label25.Tag = "12809";
            this.Label25.Text = "#積算枚数";
            // 
            // cwneSumOff
            // 
            this.cwneSumOff.Location = new System.Drawing.Point(163, 52);
            this.cwneSumOff.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.cwneSumOff.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cwneSumOff.Name = "cwneSumOff";
            this.cwneSumOff.Size = new System.Drawing.Size(81, 23);
            this.cwneSumOff.TabIndex = 1;
            this.cwneSumOff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneSumOff.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblColon11
            // 
            this.lblColon11.AutoSize = true;
            this.lblColon11.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 14F);
            this.lblColon11.Location = new System.Drawing.Point(144, 51);
            this.lblColon11.Name = "lblColon11";
            this.lblColon11.Size = new System.Drawing.Size(19, 19);
            this.lblColon11.TabIndex = 27;
            this.lblColon11.Tag = "";
            this.lblColon11.Text = "：";
            // 
            // cmdOK
            // 
            this.cmdOK.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdOK.Location = new System.Drawing.Point(88, 754);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(105, 33);
            this.cmdOK.TabIndex = 10;
            this.cmdOK.Tag = "10001";
            this.cmdOK.Text = "#O K";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdEnd
            // 
            this.cmdEnd.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdEnd.Location = new System.Drawing.Point(232, 754);
            this.cmdEnd.Name = "cmdEnd";
            this.cmdEnd.Size = new System.Drawing.Size(105, 33);
            this.cmdEnd.TabIndex = 11;
            this.cmdEnd.Tag = "10006";
            this.cmdEnd.Text = "#終 了";
            this.cmdEnd.UseVisualStyleBackColor = true;
            this.cmdEnd.Click += new System.EventHandler(this.cmdEnd_Click);
            // 
            // chkAutoJdgResult
            // 
            this.chkAutoJdgResult.AutoSize = true;
            this.chkAutoJdgResult.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkAutoJdgResult.Location = new System.Drawing.Point(24, 64);
            this.chkAutoJdgResult.Name = "chkAutoJdgResult";
            this.chkAutoJdgResult.Size = new System.Drawing.Size(220, 20);
            this.chkAutoJdgResult.TabIndex = 21;
            this.chkAutoJdgResult.Tag = "26003";
            this.chkAutoJdgResult.Text = "#校正結果を自動で判定する";
            this.chkAutoJdgResult.UseVisualStyleBackColor = true;
            this.chkAutoJdgResult.CheckedChanged += new System.EventHandler(this.chkAutoJdgResult_CheckedChanged);
            // 
            // stsCorrect3
            // 
            this.stsCorrect3.Caption = "#ステータス";
            this.stsCorrect3.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.stsCorrect3.CaptionWidth = 136;
            this.stsCorrect3.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stsCorrect3.Location = new System.Drawing.Point(16, 28);
            this.stsCorrect3.Name = "stsCorrect3";
            this.stsCorrect3.Size = new System.Drawing.Size(287, 18);
            this.stsCorrect3.Status = "";
            this.stsCorrect3.TabIndex = 2;
            this.stsCorrect3.TabStop = false;
            this.stsCorrect3.Tag = "12033";
            this.stsCorrect3.Changed += new System.EventHandler(this.stsCorrect_Changed);
            // 
            // stsCorrect2
            // 
            this.stsCorrect2.Caption = "#ステータス";
            this.stsCorrect2.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.stsCorrect2.CaptionWidth = 136;
            this.stsCorrect2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stsCorrect2.Location = new System.Drawing.Point(16, 24);
            this.stsCorrect2.Name = "stsCorrect2";
            this.stsCorrect2.Size = new System.Drawing.Size(287, 18);
            this.stsCorrect2.Status = "";
            this.stsCorrect2.TabIndex = 2;
            this.stsCorrect2.TabStop = false;
            this.stsCorrect2.Tag = "12033";
            this.stsCorrect2.Changed += new System.EventHandler(this.stsCorrect_Changed);
            // 
            // stsCorrect1
            // 
            this.stsCorrect1.Caption = "#ステータス";
            this.stsCorrect1.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.stsCorrect1.CaptionWidth = 136;
            this.stsCorrect1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stsCorrect1.Location = new System.Drawing.Point(16, 22);
            this.stsCorrect1.Name = "stsCorrect1";
            this.stsCorrect1.Size = new System.Drawing.Size(287, 18);
            this.stsCorrect1.Status = "";
            this.stsCorrect1.TabIndex = 1;
            this.stsCorrect1.TabStop = false;
            this.stsCorrect1.Tag = "12033";
            this.stsCorrect1.Changed += new System.EventHandler(this.stsCorrect_Changed);
            // 
            // stsCorrect0
            // 
            this.stsCorrect0.Caption = "#ステータス";
            this.stsCorrect0.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.stsCorrect0.CaptionWidth = 136;
            this.stsCorrect0.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stsCorrect0.Location = new System.Drawing.Point(16, 22);
            this.stsCorrect0.Name = "stsCorrect0";
            this.stsCorrect0.Size = new System.Drawing.Size(287, 18);
            this.stsCorrect0.Status = "";
            this.stsCorrect0.TabIndex = 0;
            this.stsCorrect0.TabStop = false;
            this.stsCorrect0.Tag = "12033";
            this.stsCorrect0.Changed += new System.EventHandler(this.stsCorrect_Changed);
            // 
            // frmAutoCorrection
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(433, 798);
            this.ControlBox = false;
            this.Controls.Add(this.chkAutoJdgResult);
            this.Controls.Add(this.cmdEnd);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.fraOffset);
            this.Controls.Add(this.fraVertical);
            this.Controls.Add(this.chkSPCorrect);
            this.Controls.Add(this.fraScanPosi);
            this.Controls.Add(this.fraGain);
            this.Controls.Add(this.pgbAutoCorrect);
            this.Controls.Add(this.lblMessage);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmAutoCorrection";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "10908";
            this.Text = "#自動校正";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAutoCorrection_FormClosed);
            this.Load += new System.EventHandler(this.frmAutoCorrection_Load);
            this.fraGain.ResumeLayout(false);
            this.fraGain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneGMA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSumGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneViewGain)).EndInit();
            this.fraShiftScan.ResumeLayout(false);
            this.fraShiftScan.PerformLayout();
            this.fraScanPosi.ResumeLayout(false);
            this.fraScanPosi.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSumSp)).EndInit();
            this.fraVertical.ResumeLayout(false);
            this.fraVertical.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneVMA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSumVer)).EndInit();
            this.fraOffset.ResumeLayout(false);
            this.fraOffset.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSumOff)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.ProgressBar pgbAutoCorrect;
        private System.Windows.Forms.GroupBox fraGain;
        private CTStatus stsCorrect0;
        private System.Windows.Forms.NumericUpDown cwneGMA;
        private System.Windows.Forms.NumericUpDown cwneSumGain;
        private System.Windows.Forms.NumericUpDown cwneViewGain;
        private System.Windows.Forms.Label Label16;
        private System.Windows.Forms.CheckBox chkGainTableRot;
        private System.Windows.Forms.Label lblMAName0;
        private System.Windows.Forms.Label Label19;
        private System.Windows.Forms.Label lblViewGainMinMax;
        private System.Windows.Forms.Label Label18;
        private System.Windows.Forms.Label lblGMAUni;
        private System.Windows.Forms.Label Label21;
        private System.Windows.Forms.GroupBox fraScanPosi;
        private CTStatus stsCorrect1;
        private System.Windows.Forms.Label Label5;
        private System.Windows.Forms.Label Label3;
        private System.Windows.Forms.NumericUpDown cwneSumSp;
        private System.Windows.Forms.CheckBox chkSPCorrect;
        private System.Windows.Forms.GroupBox fraVertical;
        private System.Windows.Forms.GroupBox fraOffset;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdEnd;
        private CTStatus stsCorrect2;
        private CTStatus stsCorrect3;
        private System.Windows.Forms.CheckBox chkShading;
        private System.Windows.Forms.TextBox cwneWireVer;
        private System.Windows.Forms.Label Label11;
        private System.Windows.Forms.Label lblVMAUni;
        private System.Windows.Forms.Label lblMAName2;
        private System.Windows.Forms.NumericUpDown cwneVMA;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown cwneSumVer;
        private System.Windows.Forms.Label Label27;
        private System.Windows.Forms.Label Label25;
        private System.Windows.Forms.NumericUpDown cwneSumOff;
        private System.Windows.Forms.Label Label13;
        private System.Windows.Forms.Label lblColon7;
        private System.Windows.Forms.Label lblColon9;
        private System.Windows.Forms.Label lblColon8;
        private System.Windows.Forms.Label lblColon1;
        private System.Windows.Forms.Label lblColon5;
        private System.Windows.Forms.Label lblColon4;
        private System.Windows.Forms.Label lblColon3;
        private System.Windows.Forms.Label lblColon11;
        private System.Windows.Forms.Panel fraShiftScan;
        private System.Windows.Forms.CheckBox chkHaFuOfScan;
        private System.Windows.Forms.CheckBox chkShiftScan;
        private System.Windows.Forms.CheckBox chkYAxisMove;
        private System.Windows.Forms.CheckBox chkXAxisMove;
        private System.Windows.Forms.CheckBox chkAutoJdgResult;
    }
}