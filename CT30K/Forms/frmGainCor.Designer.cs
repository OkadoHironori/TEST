namespace CT30K
{
    partial class frmGainCor
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
            this.pgbGain = new System.Windows.Forms.ProgressBar();
            this.lblViewNum = new System.Windows.Forms.Label();
            this.cwneScanView = new System.Windows.Forms.NumericUpDown();
            this.lblColon1 = new System.Windows.Forms.Label();
            this.lblViewNumUni = new System.Windows.Forms.Label();
            this.lblViewMinMax = new System.Windows.Forms.Label();
            this.lblIntegNumUni = new System.Windows.Forms.Label();
            this.lblColon2 = new System.Windows.Forms.Label();
            this.cwneSum = new System.Windows.Forms.NumericUpDown();
            this.lblIntegNum = new System.Windows.Forms.Label();
            this.chkGainTableRot = new System.Windows.Forms.CheckBox();
            this.cmdEnd = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.lblUDUni = new System.Windows.Forms.Label();
            this.cwneDownTableDistance = new System.Windows.Forms.TextBox();
            this.chkDownTable = new System.Windows.Forms.CheckBox();
            this.lblMAUni = new System.Windows.Forms.Label();
            this.lblColon3 = new System.Windows.Forms.Label();
            this.cwneMA = new System.Windows.Forms.NumericUpDown();
            this.lblMAName = new System.Windows.Forms.Label();
            this.fraTubeCurrent = new System.Windows.Forms.Panel();
            this.fraTableDownAcquire = new System.Windows.Forms.Panel();
            this.fraShiftScan = new System.Windows.Forms.Panel();
            this.chkShiftScan = new System.Windows.Forms.CheckBox();
            this.chkHaFuOfScan = new System.Windows.Forms.CheckBox();
            this.lblIntegMinMax = new System.Windows.Forms.Label();
            this.fraTableYAxisMoveAcquire = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.chkGainTableYAxis = new System.Windows.Forms.CheckBox();
            this.cwneTableYAxisDistance = new System.Windows.Forms.TextBox();
            this.fraTableXAxisMoveAcquire = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.chkGainTableXAxis = new System.Windows.Forms.CheckBox();
            this.cwneTableXAxisDistance = new System.Windows.Forms.TextBox();
            this.stsGain = new CT30K.CTStatus();
            ((System.ComponentModel.ISupportInitialize)(this.cwneScanView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneMA)).BeginInit();
            this.fraTubeCurrent.SuspendLayout();
            this.fraTableDownAcquire.SuspendLayout();
            this.fraShiftScan.SuspendLayout();
            this.fraTableYAxisMoveAcquire.SuspendLayout();
            this.fraTableXAxisMoveAcquire.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMessage.Location = new System.Drawing.Point(24, 8);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(305, 73);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Tag = "9906";
            this.lblMessage.Text = "#準備ができたらＯＫをクリックしてください。";
            // 
            // pgbGain
            // 
            this.pgbGain.Location = new System.Drawing.Point(152, 112);
            this.pgbGain.Name = "pgbGain";
            this.pgbGain.Size = new System.Drawing.Size(181, 21);
            this.pgbGain.TabIndex = 2;
            // 
            // lblViewNum
            // 
            this.lblViewNum.AutoSize = true;
            this.lblViewNum.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblViewNum.Location = new System.Drawing.Point(32, 149);
            this.lblViewNum.Name = "lblViewNum";
            this.lblViewNum.Size = new System.Drawing.Size(74, 16);
            this.lblViewNum.TabIndex = 3;
            this.lblViewNum.Tag = "12808";
            this.lblViewNum.Text = "#ビュー数";
            // 
            // cwneScanView
            // 
            this.cwneScanView.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneScanView.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.cwneScanView.Location = new System.Drawing.Point(152, 144);
            this.cwneScanView.Name = "cwneScanView";
            this.cwneScanView.Size = new System.Drawing.Size(81, 24);
            this.cwneScanView.TabIndex = 4;
            this.cwneScanView.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneScanView.ValueChanged += new System.EventHandler(this.cwneScanView_ValueChanged);
            // 
            // lblColon1
            // 
            this.lblColon1.AutoSize = true;
            this.lblColon1.Font = new System.Drawing.Font("Arial", 10F);
            this.lblColon1.Location = new System.Drawing.Point(138, 148);
            this.lblColon1.Name = "lblColon1";
            this.lblColon1.Size = new System.Drawing.Size(12, 16);
            this.lblColon1.TabIndex = 5;
            this.lblColon1.Text = ":";
            // 
            // lblViewNumUni
            // 
            this.lblViewNumUni.AutoSize = true;
            this.lblViewNumUni.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblViewNumUni.Location = new System.Drawing.Point(240, 152);
            this.lblViewNumUni.Name = "lblViewNumUni";
            this.lblViewNumUni.Size = new System.Drawing.Size(43, 12);
            this.lblViewNumUni.TabIndex = 6;
            this.lblViewNumUni.Tag = "10831";
            this.lblViewNumUni.Text = "#ビュー";
            // 
            // lblViewMinMax
            // 
            this.lblViewMinMax.AutoSize = true;
            this.lblViewMinMax.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblViewMinMax.Location = new System.Drawing.Point(280, 152);
            this.lblViewMinMax.Name = "lblViewMinMax";
            this.lblViewMinMax.Size = new System.Drawing.Size(67, 12);
            this.lblViewMinMax.TabIndex = 7;
            this.lblViewMinMax.Text = "(400～2400)";
            // 
            // lblIntegNumUni
            // 
            this.lblIntegNumUni.AutoSize = true;
            this.lblIntegNumUni.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblIntegNumUni.Location = new System.Drawing.Point(240, 182);
            this.lblIntegNumUni.Name = "lblIntegNumUni";
            this.lblIntegNumUni.Size = new System.Drawing.Size(23, 12);
            this.lblIntegNumUni.TabIndex = 11;
            this.lblIntegNumUni.Tag = "10818";
            this.lblIntegNumUni.Text = "#枚";
            // 
            // lblColon2
            // 
            this.lblColon2.AutoSize = true;
            this.lblColon2.Font = new System.Drawing.Font("Arial", 10F);
            this.lblColon2.Location = new System.Drawing.Point(138, 178);
            this.lblColon2.Name = "lblColon2";
            this.lblColon2.Size = new System.Drawing.Size(12, 16);
            this.lblColon2.TabIndex = 10;
            this.lblColon2.Text = ":";
            // 
            // cwneSum
            // 
            this.cwneSum.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneSum.Location = new System.Drawing.Point(152, 174);
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
            this.cwneSum.TabIndex = 9;
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
            this.lblIntegNum.Location = new System.Drawing.Point(32, 179);
            this.lblIntegNum.Name = "lblIntegNum";
            this.lblIntegNum.Size = new System.Drawing.Size(80, 16);
            this.lblIntegNum.TabIndex = 8;
            this.lblIntegNum.Tag = "12809";
            this.lblIntegNum.Text = "#積算枚数";
            // 
            // chkGainTableRot
            // 
            this.chkGainTableRot.AutoSize = true;
            this.chkGainTableRot.Checked = true;
            this.chkGainTableRot.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.chkGainTableRot.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkGainTableRot.Location = new System.Drawing.Point(16, 312);
            this.chkGainTableRot.Name = "chkGainTableRot";
            this.chkGainTableRot.Size = new System.Drawing.Size(126, 20);
            this.chkGainTableRot.TabIndex = 16;
            this.chkGainTableRot.Text = "#テーブル回転";
            this.chkGainTableRot.UseVisualStyleBackColor = true;
            this.chkGainTableRot.CheckStateChanged += new System.EventHandler(this.chkGainTableRot_CheckStateChanged);
            // 
            // cmdEnd
            // 
            this.cmdEnd.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdEnd.Location = new System.Drawing.Point(208, 464);
            this.cmdEnd.Name = "cmdEnd";
            this.cmdEnd.Size = new System.Drawing.Size(105, 33);
            this.cmdEnd.TabIndex = 20;
            this.cmdEnd.Tag = "10006";
            this.cmdEnd.Text = "#終 了";
            this.cmdEnd.UseVisualStyleBackColor = true;
            this.cmdEnd.Click += new System.EventHandler(this.cmdEnd_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdOK.Location = new System.Drawing.Point(56, 464);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(105, 33);
            this.cmdOK.TabIndex = 19;
            this.cmdOK.Tag = "10001";
            this.cmdOK.Text = "#O K";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // lblUDUni
            // 
            this.lblUDUni.AutoSize = true;
            this.lblUDUni.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblUDUni.Location = new System.Drawing.Point(288, 15);
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
            this.cwneDownTableDistance.Location = new System.Drawing.Point(216, 7);
            this.cwneDownTableDistance.Name = "cwneDownTableDistance";
            this.cwneDownTableDistance.Size = new System.Drawing.Size(65, 23);
            this.cwneDownTableDistance.TabIndex = 23;
            this.cwneDownTableDistance.Text = "10";
            this.cwneDownTableDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneDownTableDistance.TextChanged += new System.EventHandler(this.cwneDownTableDistance_ValueChanged);
            this.cwneDownTableDistance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cwneDownTableDistance_KeyPress);
            this.cwneDownTableDistance.Leave += new System.EventHandler(this.cwneDownTableDistance_Leave);
            // 
            // chkDownTable
            // 
            this.chkDownTable.AutoSize = true;
            this.chkDownTable.Checked = true;
            this.chkDownTable.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.chkDownTable.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkDownTable.Location = new System.Drawing.Point(8, 7);
            this.chkDownTable.Name = "chkDownTable";
            this.chkDownTable.Size = new System.Drawing.Size(158, 20);
            this.chkDownTable.TabIndex = 22;
            this.chkDownTable.Tag = "12116";
            this.chkDownTable.Text = "#テーブル下降収集";
            this.chkDownTable.UseVisualStyleBackColor = true;
            this.chkDownTable.CheckStateChanged += new System.EventHandler(this.chkDownTable_CheckStateChanged);
            // 
            // lblMAUni
            // 
            this.lblMAUni.AutoSize = true;
            this.lblMAUni.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMAUni.Location = new System.Drawing.Point(224, 16);
            this.lblMAUni.Name = "lblMAUni";
            this.lblMAUni.Size = new System.Drawing.Size(25, 12);
            this.lblMAUni.TabIndex = 19;
            this.lblMAUni.Text = "μA";
            // 
            // lblColon3
            // 
            this.lblColon3.AutoSize = true;
            this.lblColon3.Font = new System.Drawing.Font("Arial", 10F);
            this.lblColon3.Location = new System.Drawing.Point(122, 12);
            this.lblColon3.Name = "lblColon3";
            this.lblColon3.Size = new System.Drawing.Size(12, 16);
            this.lblColon3.TabIndex = 18;
            this.lblColon3.Text = ":";
            // 
            // cwneMA
            // 
            this.cwneMA.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneMA.Location = new System.Drawing.Point(136, 8);
            this.cwneMA.Name = "cwneMA";
            this.cwneMA.Size = new System.Drawing.Size(81, 24);
            this.cwneMA.TabIndex = 17;
            this.cwneMA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblMAName
            // 
            this.lblMAName.AutoSize = true;
            this.lblMAName.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMAName.Location = new System.Drawing.Point(16, 13);
            this.lblMAName.Name = "lblMAName";
            this.lblMAName.Size = new System.Drawing.Size(64, 16);
            this.lblMAName.TabIndex = 16;
            this.lblMAName.Tag = "12807";
            this.lblMAName.Text = "#管電流";
            // 
            // fraTubeCurrent
            // 
            this.fraTubeCurrent.Controls.Add(this.lblMAUni);
            this.fraTubeCurrent.Controls.Add(this.lblMAName);
            this.fraTubeCurrent.Controls.Add(this.lblColon3);
            this.fraTubeCurrent.Controls.Add(this.cwneMA);
            this.fraTubeCurrent.Location = new System.Drawing.Point(16, 198);
            this.fraTubeCurrent.Name = "fraTubeCurrent";
            this.fraTubeCurrent.Size = new System.Drawing.Size(318, 40);
            this.fraTubeCurrent.TabIndex = 24;
            // 
            // fraTableDownAcquire
            // 
            this.fraTableDownAcquire.Controls.Add(this.lblUDUni);
            this.fraTableDownAcquire.Controls.Add(this.chkDownTable);
            this.fraTableDownAcquire.Controls.Add(this.cwneDownTableDistance);
            this.fraTableDownAcquire.Location = new System.Drawing.Point(7, 416);
            this.fraTableDownAcquire.Name = "fraTableDownAcquire";
            this.fraTableDownAcquire.Size = new System.Drawing.Size(326, 38);
            this.fraTableDownAcquire.TabIndex = 25;
            // 
            // fraShiftScan
            // 
            this.fraShiftScan.Controls.Add(this.chkShiftScan);
            this.fraShiftScan.Controls.Add(this.chkHaFuOfScan);
            this.fraShiftScan.Location = new System.Drawing.Point(8, 240);
            this.fraShiftScan.Name = "fraShiftScan";
            this.fraShiftScan.Size = new System.Drawing.Size(326, 65);
            this.fraShiftScan.TabIndex = 26;
            // 
            // chkShiftScan
            // 
            this.chkShiftScan.AutoSize = true;
            this.chkShiftScan.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkShiftScan.Location = new System.Drawing.Point(8, 38);
            this.chkShiftScan.Name = "chkShiftScan";
            this.chkShiftScan.Size = new System.Drawing.Size(160, 20);
            this.chkShiftScan.TabIndex = 24;
            this.chkShiftScan.Tag = "";
            this.chkShiftScan.Text = "#シフトスキャン収集";
            this.chkShiftScan.UseVisualStyleBackColor = true;
            // 
            // chkHaFuOfScan
            // 
            this.chkHaFuOfScan.AutoSize = true;
            this.chkHaFuOfScan.Checked = true;
            this.chkHaFuOfScan.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.chkHaFuOfScan.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkHaFuOfScan.Location = new System.Drawing.Point(8, 9);
            this.chkHaFuOfScan.Name = "chkHaFuOfScan";
            this.chkHaFuOfScan.Size = new System.Drawing.Size(283, 20);
            this.chkHaFuOfScan.TabIndex = 23;
            this.chkHaFuOfScan.Tag = "";
            this.chkHaFuOfScan.Text = "#ハーフ、フル、オフセットスキャン収集";
            this.chkHaFuOfScan.UseVisualStyleBackColor = true;
            this.chkHaFuOfScan.CheckStateChanged += new System.EventHandler(this.chkHaFuOfScan_CheckStateChanged);
            // 
            // lblIntegMinMax
            // 
            this.lblIntegMinMax.AutoSize = true;
            this.lblIntegMinMax.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblIntegMinMax.Location = new System.Drawing.Point(280, 182);
            this.lblIntegMinMax.Name = "lblIntegMinMax";
            this.lblIntegMinMax.Size = new System.Drawing.Size(43, 12);
            this.lblIntegMinMax.TabIndex = 27;
            this.lblIntegMinMax.Text = "(1～20)";
            // 
            // fraTableYAxisMoveAcquire
            // 
            this.fraTableYAxisMoveAcquire.Controls.Add(this.label1);
            this.fraTableYAxisMoveAcquire.Controls.Add(this.chkGainTableYAxis);
            this.fraTableYAxisMoveAcquire.Controls.Add(this.cwneTableYAxisDistance);
            this.fraTableYAxisMoveAcquire.Location = new System.Drawing.Point(7, 338);
            this.fraTableYAxisMoveAcquire.Name = "fraTableYAxisMoveAcquire";
            this.fraTableYAxisMoveAcquire.Size = new System.Drawing.Size(326, 38);
            this.fraTableYAxisMoveAcquire.TabIndex = 28;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(288, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 24;
            this.label1.Text = "mm";
            // 
            // chkGainTableYAxis
            // 
            this.chkGainTableYAxis.AutoSize = true;
            this.chkGainTableYAxis.Checked = true;
            this.chkGainTableYAxis.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.chkGainTableYAxis.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkGainTableYAxis.Location = new System.Drawing.Point(8, 7);
            this.chkGainTableYAxis.Name = "chkGainTableYAxis";
            this.chkGainTableYAxis.Size = new System.Drawing.Size(183, 20);
            this.chkGainTableYAxis.TabIndex = 22;
            this.chkGainTableYAxis.Tag = "12142";
            this.chkGainTableYAxis.Text = "#テーブルY軸移動収集";
            this.chkGainTableYAxis.UseVisualStyleBackColor = true;
            this.chkGainTableYAxis.CheckStateChanged += new System.EventHandler(this.chkGainTableYAxis_CheckStateChanged);
            // 
            // cwneTableYAxisDistance
            // 
            this.cwneTableYAxisDistance.BackColor = System.Drawing.Color.White;
            this.cwneTableYAxisDistance.Enabled = false;
            this.cwneTableYAxisDistance.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneTableYAxisDistance.Location = new System.Drawing.Point(216, 7);
            this.cwneTableYAxisDistance.Name = "cwneTableYAxisDistance";
            this.cwneTableYAxisDistance.Size = new System.Drawing.Size(65, 23);
            this.cwneTableYAxisDistance.TabIndex = 23;
            this.cwneTableYAxisDistance.Text = "10";
            this.cwneTableYAxisDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneTableYAxisDistance.TextChanged += new System.EventHandler(this.cwneTableYAxisDistance_ValueChanged);
            this.cwneTableYAxisDistance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cwneTableYAxisDistance_KeyPress);
            this.cwneTableYAxisDistance.Leave += new System.EventHandler(this.cwneTableYAxisDistance_Leave);
            // 
            // fraTableXAxisMoveAcquire
            // 
            this.fraTableXAxisMoveAcquire.Controls.Add(this.label2);
            this.fraTableXAxisMoveAcquire.Controls.Add(this.chkGainTableXAxis);
            this.fraTableXAxisMoveAcquire.Controls.Add(this.cwneTableXAxisDistance);
            this.fraTableXAxisMoveAcquire.Location = new System.Drawing.Point(7, 377);
            this.fraTableXAxisMoveAcquire.Name = "fraTableXAxisMoveAcquire";
            this.fraTableXAxisMoveAcquire.Size = new System.Drawing.Size(326, 38);
            this.fraTableXAxisMoveAcquire.TabIndex = 29;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(288, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 24;
            this.label2.Text = "mm";
            // 
            // chkGainTableXAxis
            // 
            this.chkGainTableXAxis.AutoSize = true;
            this.chkGainTableXAxis.Checked = true;
            this.chkGainTableXAxis.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.chkGainTableXAxis.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkGainTableXAxis.Location = new System.Drawing.Point(8, 7);
            this.chkGainTableXAxis.Name = "chkGainTableXAxis";
            this.chkGainTableXAxis.Size = new System.Drawing.Size(204, 20);
            this.chkGainTableXAxis.TabIndex = 22;
            this.chkGainTableXAxis.Tag = "12143";
            this.chkGainTableXAxis.Text = "#テーブルFCD軸移動収集";
            this.chkGainTableXAxis.UseVisualStyleBackColor = true;
            // 
            // cwneTableXAxisDistance
            // 
            this.cwneTableXAxisDistance.BackColor = System.Drawing.Color.White;
            this.cwneTableXAxisDistance.Enabled = false;
            this.cwneTableXAxisDistance.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneTableXAxisDistance.Location = new System.Drawing.Point(216, 7);
            this.cwneTableXAxisDistance.Name = "cwneTableXAxisDistance";
            this.cwneTableXAxisDistance.Size = new System.Drawing.Size(65, 23);
            this.cwneTableXAxisDistance.TabIndex = 23;
            this.cwneTableXAxisDistance.Text = "10";
            this.cwneTableXAxisDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // stsGain
            // 
            this.stsGain.Caption = "#ステータス";
            this.stsGain.CaptionAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.stsGain.CaptionWidth = 128;
            this.stsGain.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stsGain.Location = new System.Drawing.Point(12, 88);
            this.stsGain.Name = "stsGain";
            this.stsGain.Size = new System.Drawing.Size(321, 18);
            this.stsGain.Status = "";
            this.stsGain.TabIndex = 1;
            this.stsGain.Tag = "12033";
            // 
            // frmGainCor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(358, 511);
            this.ControlBox = false;
            this.Controls.Add(this.fraTableXAxisMoveAcquire);
            this.Controls.Add(this.fraTableYAxisMoveAcquire);
            this.Controls.Add(this.lblIntegMinMax);
            this.Controls.Add(this.fraShiftScan);
            this.Controls.Add(this.fraTableDownAcquire);
            this.Controls.Add(this.fraTubeCurrent);
            this.Controls.Add(this.cmdEnd);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.chkGainTableRot);
            this.Controls.Add(this.lblIntegNumUni);
            this.Controls.Add(this.lblColon2);
            this.Controls.Add(this.cwneSum);
            this.Controls.Add(this.lblIntegNum);
            this.Controls.Add(this.lblViewMinMax);
            this.Controls.Add(this.lblViewNumUni);
            this.Controls.Add(this.lblColon1);
            this.Controls.Add(this.cwneScanView);
            this.Controls.Add(this.lblViewNum);
            this.Controls.Add(this.pgbGain);
            this.Controls.Add(this.stsGain);
            this.Controls.Add(this.lblMessage);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmGainCor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "10904";
            this.Text = "#ゲイン校正";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmGainCor_FormClosed);
            this.Load += new System.EventHandler(this.frmGainCor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cwneScanView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneMA)).EndInit();
            this.fraTubeCurrent.ResumeLayout(false);
            this.fraTubeCurrent.PerformLayout();
            this.fraTableDownAcquire.ResumeLayout(false);
            this.fraTableDownAcquire.PerformLayout();
            this.fraShiftScan.ResumeLayout(false);
            this.fraShiftScan.PerformLayout();
            this.fraTableYAxisMoveAcquire.ResumeLayout(false);
            this.fraTableYAxisMoveAcquire.PerformLayout();
            this.fraTableXAxisMoveAcquire.ResumeLayout(false);
            this.fraTableXAxisMoveAcquire.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private CTStatus stsGain;
        private System.Windows.Forms.ProgressBar pgbGain;
        private System.Windows.Forms.Label lblViewNum;
        private System.Windows.Forms.NumericUpDown cwneScanView;
        private System.Windows.Forms.Label lblColon1;
        private System.Windows.Forms.Label lblViewNumUni;
        private System.Windows.Forms.Label lblViewMinMax;
        private System.Windows.Forms.Label lblIntegNumUni;
        private System.Windows.Forms.Label lblColon2;
        private System.Windows.Forms.NumericUpDown cwneSum;
        private System.Windows.Forms.Label lblIntegNum;
        private System.Windows.Forms.CheckBox chkGainTableRot;
        private System.Windows.Forms.Button cmdEnd;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Label lblUDUni;
        private System.Windows.Forms.TextBox cwneDownTableDistance;
        private System.Windows.Forms.CheckBox chkDownTable;
        private System.Windows.Forms.Label lblMAUni;
        private System.Windows.Forms.Label lblColon3;
        private System.Windows.Forms.NumericUpDown cwneMA;
        private System.Windows.Forms.Label lblMAName;
        private System.Windows.Forms.Panel fraTubeCurrent;
        private System.Windows.Forms.Panel fraTableDownAcquire;
        private System.Windows.Forms.Panel fraShiftScan;
        private System.Windows.Forms.CheckBox chkShiftScan;
        private System.Windows.Forms.CheckBox chkHaFuOfScan;
        private System.Windows.Forms.Label lblIntegMinMax;
        private System.Windows.Forms.Panel fraTableYAxisMoveAcquire;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkGainTableYAxis;
        private System.Windows.Forms.TextBox cwneTableYAxisDistance;
        private System.Windows.Forms.Panel fraTableXAxisMoveAcquire;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkGainTableXAxis;
        private System.Windows.Forms.TextBox cwneTableXAxisDistance;
    }
}