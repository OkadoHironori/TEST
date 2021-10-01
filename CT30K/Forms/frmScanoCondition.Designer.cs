namespace CT30K
{
    partial class frmScanoCondition
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
            this.sstMenu = new System.Windows.Forms.TabControl();
            this.tabPage0 = new System.Windows.Forms.TabPage();
            this.fraMenu0 = new System.Windows.Forms.Panel();
            this.fraScanoWidth = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cwneScanoWidthPix = new System.Windows.Forms.NumericUpDown();
            this.lblPixDsp2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cwneScanoWidth = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.lblScanoWidthMinMax = new System.Windows.Forms.Label();
            this.fraScanoBias = new System.Windows.Forms.GroupBox();
            this.ntbScanoSlopeCaption = new System.Windows.Forms.Label();
            this.ntbScanoSlope = new System.Windows.Forms.NumericUpDown();
            this.ntbScanoBiasCaption = new System.Windows.Forms.Label();
            this.ntbScanoBias = new System.Windows.Forms.NumericUpDown();
            this.fraScanoDataRange = new System.Windows.Forms.GroupBox();
            this.lblSlicePitchBetween2 = new System.Windows.Forms.Label();
            this.lblScanoDataRange_min2 = new System.Windows.Forms.Label();
            this.lblSlicePitchBetween = new System.Windows.Forms.Label();
            this.lblScanoDataRange_max = new System.Windows.Forms.Label();
            this.lblScanoDataRange_min = new System.Windows.Forms.Label();
            this.lblSlicePitchUni = new System.Windows.Forms.Label();
            this.cwneScanoDataRange = new System.Windows.Forms.NumericUpDown();
            this.fraScanoMatrix = new System.Windows.Forms.GroupBox();
            this.optScanoMatrix5 = new System.Windows.Forms.RadioButton();
            this.optScanoMatrix4 = new System.Windows.Forms.RadioButton();
            this.optScanoMatrix3 = new System.Windows.Forms.RadioButton();
            this.optScanoMatrix2 = new System.Windows.Forms.RadioButton();
            this.optScanoMatrix1 = new System.Windows.Forms.RadioButton();
            this.fraScanoPitch = new System.Windows.Forms.GroupBox();
            this.fraSliceWidthPix = new System.Windows.Forms.Panel();
            this.Label3 = new System.Windows.Forms.Label();
            this.cwneScanoSlicePitchPix = new System.Windows.Forms.NumericUpDown();
            this.lblPixDsp = new System.Windows.Forms.Label();
            this.fraSliceWidthMM = new System.Windows.Forms.Panel();
            this.cwneScanoSlicePitch = new System.Windows.Forms.NumericUpDown();
            this.lblSliceUni = new System.Windows.Forms.Label();
            this.lblScanoSliceMinMax = new System.Windows.Forms.Label();
            this.fraScanoInteg = new System.Windows.Forms.GroupBox();
            this.lblScanoIntegMinMax = new System.Windows.Forms.Label();
            this.lblScanoIntegUni = new System.Windows.Forms.Label();
            this.cwneScanoInteg = new System.Windows.Forms.NumericUpDown();
            this.CmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.fraPicSize = new System.Windows.Forms.GroupBox();
            this.ntbTransImageHeight = new CT30K.NumTextBox();
            this.ntbTransImageWidth = new CT30K.NumTextBox();
            this.sstMenu.SuspendLayout();
            this.tabPage0.SuspendLayout();
            this.fraMenu0.SuspendLayout();
            this.fraScanoWidth.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneScanoWidthPix)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneScanoWidth)).BeginInit();
            this.fraScanoBias.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ntbScanoSlope)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ntbScanoBias)).BeginInit();
            this.fraScanoDataRange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneScanoDataRange)).BeginInit();
            this.fraScanoMatrix.SuspendLayout();
            this.fraScanoPitch.SuspendLayout();
            this.fraSliceWidthPix.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneScanoSlicePitchPix)).BeginInit();
            this.fraSliceWidthMM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneScanoSlicePitch)).BeginInit();
            this.fraScanoInteg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneScanoInteg)).BeginInit();
            this.fraPicSize.SuspendLayout();
            this.SuspendLayout();
            // 
            // sstMenu
            // 
            this.sstMenu.Controls.Add(this.tabPage0);
            this.sstMenu.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.sstMenu.Location = new System.Drawing.Point(8, 8);
            this.sstMenu.Name = "sstMenu";
            this.sstMenu.SelectedIndex = 0;
            this.sstMenu.Size = new System.Drawing.Size(729, 489);
            this.sstMenu.TabIndex = 1;
            this.sstMenu.Tag = "";
            // 
            // tabPage0
            // 
            this.tabPage0.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage0.Controls.Add(this.fraMenu0);
            this.tabPage0.Location = new System.Drawing.Point(4, 30);
            this.tabPage0.Name = "tabPage0";
            this.tabPage0.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage0.Size = new System.Drawing.Size(721, 455);
            this.tabPage0.TabIndex = 0;
            this.tabPage0.Tag = "12295";
            this.tabPage0.Text = "#条件設定 1";
            // 
            // fraMenu0
            // 
            this.fraMenu0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fraMenu0.Controls.Add(this.fraScanoWidth);
            this.fraMenu0.Controls.Add(this.fraScanoBias);
            this.fraMenu0.Controls.Add(this.fraScanoDataRange);
            this.fraMenu0.Controls.Add(this.fraScanoMatrix);
            this.fraMenu0.Controls.Add(this.fraScanoPitch);
            this.fraMenu0.Controls.Add(this.fraScanoInteg);
            this.fraMenu0.Font = new System.Drawing.Font("MS UI Gothic", 9.75F);
            this.fraMenu0.Location = new System.Drawing.Point(5, 2);
            this.fraMenu0.Name = "fraMenu0";
            this.fraMenu0.Size = new System.Drawing.Size(713, 457);
            this.fraMenu0.TabIndex = 1;
            // 
            // fraScanoWidth
            // 
            this.fraScanoWidth.Controls.Add(this.panel1);
            this.fraScanoWidth.Controls.Add(this.panel2);
            this.fraScanoWidth.Controls.Add(this.lblScanoWidthMinMax);
            this.fraScanoWidth.Location = new System.Drawing.Point(189, 15);
            this.fraScanoWidth.Name = "fraScanoWidth";
            this.fraScanoWidth.Size = new System.Drawing.Size(165, 105);
            this.fraScanoWidth.TabIndex = 125;
            this.fraScanoWidth.TabStop = false;
            this.fraScanoWidth.Tag = "22002";
            this.fraScanoWidth.Text = "#スキャノ厚";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cwneScanoWidthPix);
            this.panel1.Controls.Add(this.lblPixDsp2);
            this.panel1.Location = new System.Drawing.Point(20, 44);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(133, 37);
            this.panel1.TabIndex = 188;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.label1.Location = new System.Drawing.Point(4, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 15);
            this.label1.TabIndex = 93;
            this.label1.Text = "(";
            // 
            // cwneScanoWidthPix
            // 
            this.cwneScanoWidthPix.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.cwneScanoWidthPix.Location = new System.Drawing.Point(15, 8);
            this.cwneScanoWidthPix.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.cwneScanoWidthPix.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cwneScanoWidthPix.Name = "cwneScanoWidthPix";
            this.cwneScanoWidthPix.Size = new System.Drawing.Size(75, 27);
            this.cwneScanoWidthPix.TabIndex = 94;
            this.cwneScanoWidthPix.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneScanoWidthPix.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cwneScanoWidthPix.ValueChanged += new System.EventHandler(this.cwneScanoWidthPix_ValueChanged);
            this.cwneScanoWidthPix.Leave += new System.EventHandler(this.NumicValue_Leave);
            // 
            // lblPixDsp2
            // 
            this.lblPixDsp2.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.lblPixDsp2.Location = new System.Drawing.Point(74, 16);
            this.lblPixDsp2.Name = "lblPixDsp2";
            this.lblPixDsp2.Size = new System.Drawing.Size(50, 13);
            this.lblPixDsp2.TabIndex = 95;
            this.lblPixDsp2.Tag = "10816";
            this.lblPixDsp2.Text = "画素)";
            this.lblPixDsp2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cwneScanoWidth);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(20, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(133, 37);
            this.panel2.TabIndex = 90;
            // 
            // cwneScanoWidth
            // 
            this.cwneScanoWidth.DecimalPlaces = 3;
            this.cwneScanoWidth.Enabled = false;
            this.cwneScanoWidth.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.cwneScanoWidth.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.cwneScanoWidth.Location = new System.Drawing.Point(15, 8);
            this.cwneScanoWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.cwneScanoWidth.Name = "cwneScanoWidth";
            this.cwneScanoWidth.Size = new System.Drawing.Size(75, 27);
            this.cwneScanoWidth.TabIndex = 91;
            this.cwneScanoWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneScanoWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.label4.Location = new System.Drawing.Point(100, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 15);
            this.label4.TabIndex = 92;
            this.label4.Tag = "";
            this.label4.Text = "mm";
            // 
            // lblScanoWidthMinMax
            // 
            this.lblScanoWidthMinMax.AutoSize = true;
            this.lblScanoWidthMinMax.Location = new System.Drawing.Point(40, 84);
            this.lblScanoWidthMinMax.Name = "lblScanoWidthMinMax";
            this.lblScanoWidthMinMax.Size = new System.Drawing.Size(119, 17);
            this.lblScanoWidthMinMax.TabIndex = 96;
            this.lblScanoWidthMinMax.Text = "(0.01～0.84mm)";
            // 
            // fraScanoBias
            // 
            this.fraScanoBias.Controls.Add(this.ntbScanoSlopeCaption);
            this.fraScanoBias.Controls.Add(this.ntbScanoSlope);
            this.fraScanoBias.Controls.Add(this.ntbScanoBiasCaption);
            this.fraScanoBias.Controls.Add(this.ntbScanoBias);
            this.fraScanoBias.Location = new System.Drawing.Point(189, 126);
            this.fraScanoBias.Name = "fraScanoBias";
            this.fraScanoBias.Size = new System.Drawing.Size(165, 105);
            this.fraScanoBias.TabIndex = 124;
            this.fraScanoBias.TabStop = false;
            this.fraScanoBias.Tag = "12213";
            this.fraScanoBias.Text = "#画像バイアス／スロープ";
            // 
            // ntbScanoSlopeCaption
            // 
            this.ntbScanoSlopeCaption.Location = new System.Drawing.Point(4, 60);
            this.ntbScanoSlopeCaption.Name = "ntbScanoSlopeCaption";
            this.ntbScanoSlopeCaption.Size = new System.Drawing.Size(62, 24);
            this.ntbScanoSlopeCaption.TabIndex = 125;
            this.ntbScanoSlopeCaption.Tag = "12038";
            this.ntbScanoSlopeCaption.Text = "#スロープ";
            this.ntbScanoSlopeCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ntbScanoSlope
            // 
            this.ntbScanoSlope.DecimalPlaces = 2;
            this.ntbScanoSlope.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.ntbScanoSlope.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.ntbScanoSlope.Location = new System.Drawing.Point(69, 60);
            this.ntbScanoSlope.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ntbScanoSlope.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            131072});
            this.ntbScanoSlope.Name = "ntbScanoSlope";
            this.ntbScanoSlope.Size = new System.Drawing.Size(88, 27);
            this.ntbScanoSlope.TabIndex = 125;
            this.ntbScanoSlope.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ntbScanoSlope.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ntbScanoSlope.ValueChanged += new System.EventHandler(this.ntbScanoSlope_ValueChanged);
            this.ntbScanoSlope.Leave += new System.EventHandler(this.NumicValue_Leave);
            // 
            // ntbScanoBiasCaption
            // 
            this.ntbScanoBiasCaption.Location = new System.Drawing.Point(4, 28);
            this.ntbScanoBiasCaption.Name = "ntbScanoBiasCaption";
            this.ntbScanoBiasCaption.Size = new System.Drawing.Size(62, 24);
            this.ntbScanoBiasCaption.TabIndex = 124;
            this.ntbScanoBiasCaption.Tag = "12037";
            this.ntbScanoBiasCaption.Text = "#バイアス";
            this.ntbScanoBiasCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ntbScanoBias
            // 
            this.ntbScanoBias.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.ntbScanoBias.Location = new System.Drawing.Point(69, 28);
            this.ntbScanoBias.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.ntbScanoBias.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.ntbScanoBias.Name = "ntbScanoBias";
            this.ntbScanoBias.Size = new System.Drawing.Size(88, 27);
            this.ntbScanoBias.TabIndex = 124;
            this.ntbScanoBias.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ntbScanoBias.ValueChanged += new System.EventHandler(this.ntbScanoBias_ValueChanged);
            this.ntbScanoBias.Leave += new System.EventHandler(this.NumicValue_Leave);
            // 
            // fraScanoDataRange
            // 
            this.fraScanoDataRange.Controls.Add(this.lblSlicePitchBetween2);
            this.fraScanoDataRange.Controls.Add(this.lblScanoDataRange_min2);
            this.fraScanoDataRange.Controls.Add(this.lblSlicePitchBetween);
            this.fraScanoDataRange.Controls.Add(this.lblScanoDataRange_max);
            this.fraScanoDataRange.Controls.Add(this.lblScanoDataRange_min);
            this.fraScanoDataRange.Controls.Add(this.lblSlicePitchUni);
            this.fraScanoDataRange.Controls.Add(this.cwneScanoDataRange);
            this.fraScanoDataRange.Location = new System.Drawing.Point(365, 15);
            this.fraScanoDataRange.Name = "fraScanoDataRange";
            this.fraScanoDataRange.Size = new System.Drawing.Size(165, 105);
            this.fraScanoDataRange.TabIndex = 63;
            this.fraScanoDataRange.TabStop = false;
            this.fraScanoDataRange.Tag = "22003";
            this.fraScanoDataRange.Text = "#収集範囲";
            // 
            // lblSlicePitchBetween2
            // 
            this.lblSlicePitchBetween2.AutoSize = true;
            this.lblSlicePitchBetween2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSlicePitchBetween2.Location = new System.Drawing.Point(104, 23);
            this.lblSlicePitchBetween2.Name = "lblSlicePitchBetween2";
            this.lblSlicePitchBetween2.Size = new System.Drawing.Size(39, 20);
            this.lblSlicePitchBetween2.TabIndex = 70;
            this.lblSlicePitchBetween2.Tag = "10809";
            this.lblSlicePitchBetween2.Text = "#～";
            // 
            // lblScanoDataRange_min2
            // 
            this.lblScanoDataRange_min2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblScanoDataRange_min2.Location = new System.Drawing.Point(28, 23);
            this.lblScanoDataRange_min2.Name = "lblScanoDataRange_min2";
            this.lblScanoDataRange_min2.Size = new System.Drawing.Size(62, 21);
            this.lblScanoDataRange_min2.TabIndex = 69;
            this.lblScanoDataRange_min2.Text = "0.001";
            this.lblScanoDataRange_min2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSlicePitchBetween
            // 
            this.lblSlicePitchBetween.AutoSize = true;
            this.lblSlicePitchBetween.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSlicePitchBetween.Location = new System.Drawing.Point(61, 81);
            this.lblSlicePitchBetween.Name = "lblSlicePitchBetween";
            this.lblSlicePitchBetween.Size = new System.Drawing.Size(30, 15);
            this.lblSlicePitchBetween.TabIndex = 67;
            this.lblSlicePitchBetween.Tag = "10809";
            this.lblSlicePitchBetween.Text = "#～";
            // 
            // lblScanoDataRange_max
            // 
            this.lblScanoDataRange_max.AutoSize = true;
            this.lblScanoDataRange_max.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.lblScanoDataRange_max.Location = new System.Drawing.Point(86, 81);
            this.lblScanoDataRange_max.Name = "lblScanoDataRange_max";
            this.lblScanoDataRange_max.Size = new System.Drawing.Size(42, 15);
            this.lblScanoDataRange_max.TabIndex = 68;
            this.lblScanoDataRange_max.Text = "0.000";
            // 
            // lblScanoDataRange_min
            // 
            this.lblScanoDataRange_min.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblScanoDataRange_min.Location = new System.Drawing.Point(16, 81);
            this.lblScanoDataRange_min.Name = "lblScanoDataRange_min";
            this.lblScanoDataRange_min.Size = new System.Drawing.Size(44, 16);
            this.lblScanoDataRange_min.TabIndex = 66;
            this.lblScanoDataRange_min.Text = "0.001";
            this.lblScanoDataRange_min.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSlicePitchUni
            // 
            this.lblSlicePitchUni.AutoSize = true;
            this.lblSlicePitchUni.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.lblSlicePitchUni.Location = new System.Drawing.Point(112, 48);
            this.lblSlicePitchUni.Name = "lblSlicePitchUni";
            this.lblSlicePitchUni.Size = new System.Drawing.Size(39, 20);
            this.lblSlicePitchUni.TabIndex = 65;
            this.lblSlicePitchUni.Text = "mm";
            // 
            // cwneScanoDataRange
            // 
            this.cwneScanoDataRange.DecimalPlaces = 3;
            this.cwneScanoDataRange.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.cwneScanoDataRange.Increment = new decimal(new int[] {
            2,
            0,
            0,
            196608});
            this.cwneScanoDataRange.Location = new System.Drawing.Point(24, 46);
            this.cwneScanoDataRange.Maximum = new decimal(new int[] {
            1800,
            0,
            0,
            0});
            this.cwneScanoDataRange.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            196608});
            this.cwneScanoDataRange.Name = "cwneScanoDataRange";
            this.cwneScanoDataRange.Size = new System.Drawing.Size(81, 27);
            this.cwneScanoDataRange.TabIndex = 64;
            this.cwneScanoDataRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneScanoDataRange.Value = new decimal(new int[] {
            2,
            0,
            0,
            196608});
            this.cwneScanoDataRange.ValueChanged += new System.EventHandler(this.cwneScanoDataRange_ValueChanged);
            this.cwneScanoDataRange.Leave += new System.EventHandler(this.NumicValue_Leave);
            // 
            // fraScanoMatrix
            // 
            this.fraScanoMatrix.Controls.Add(this.optScanoMatrix5);
            this.fraScanoMatrix.Controls.Add(this.optScanoMatrix4);
            this.fraScanoMatrix.Controls.Add(this.optScanoMatrix3);
            this.fraScanoMatrix.Controls.Add(this.optScanoMatrix2);
            this.fraScanoMatrix.Controls.Add(this.optScanoMatrix1);
            this.fraScanoMatrix.Location = new System.Drawing.Point(9, 125);
            this.fraScanoMatrix.Name = "fraScanoMatrix";
            this.fraScanoMatrix.Size = new System.Drawing.Size(165, 105);
            this.fraScanoMatrix.TabIndex = 43;
            this.fraScanoMatrix.TabStop = false;
            this.fraScanoMatrix.Tag = "12814";
            this.fraScanoMatrix.Text = "#マトリクスサイズ";
            // 
            // optScanoMatrix5
            // 
            this.optScanoMatrix5.AutoSize = true;
            this.optScanoMatrix5.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.optScanoMatrix5.Location = new System.Drawing.Point(24, 85);
            this.optScanoMatrix5.Name = "optScanoMatrix5";
            this.optScanoMatrix5.Size = new System.Drawing.Size(115, 19);
            this.optScanoMatrix5.TabIndex = 199;
            this.optScanoMatrix5.TabStop = true;
            this.optScanoMatrix5.Text = "#4096×4096";
            this.optScanoMatrix5.UseVisualStyleBackColor = true;
            // 
            // optScanoMatrix4
            // 
            this.optScanoMatrix4.AutoSize = true;
            this.optScanoMatrix4.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.optScanoMatrix4.Location = new System.Drawing.Point(24, 69);
            this.optScanoMatrix4.Name = "optScanoMatrix4";
            this.optScanoMatrix4.Size = new System.Drawing.Size(115, 19);
            this.optScanoMatrix4.TabIndex = 47;
            this.optScanoMatrix4.TabStop = true;
            this.optScanoMatrix4.Text = "#2048×2048";
            this.optScanoMatrix4.UseVisualStyleBackColor = true;
            // 
            // optScanoMatrix3
            // 
            this.optScanoMatrix3.AutoSize = true;
            this.optScanoMatrix3.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.optScanoMatrix3.Location = new System.Drawing.Point(24, 53);
            this.optScanoMatrix3.Name = "optScanoMatrix3";
            this.optScanoMatrix3.Size = new System.Drawing.Size(115, 19);
            this.optScanoMatrix3.TabIndex = 46;
            this.optScanoMatrix3.TabStop = true;
            this.optScanoMatrix3.Text = "#1024×1024";
            this.optScanoMatrix3.UseVisualStyleBackColor = true;
            // 
            // optScanoMatrix2
            // 
            this.optScanoMatrix2.AutoSize = true;
            this.optScanoMatrix2.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.optScanoMatrix2.Location = new System.Drawing.Point(24, 37);
            this.optScanoMatrix2.Name = "optScanoMatrix2";
            this.optScanoMatrix2.Size = new System.Drawing.Size(99, 19);
            this.optScanoMatrix2.TabIndex = 45;
            this.optScanoMatrix2.TabStop = true;
            this.optScanoMatrix2.Text = "#512×512";
            this.optScanoMatrix2.UseVisualStyleBackColor = true;
            // 
            // optScanoMatrix1
            // 
            this.optScanoMatrix1.AutoSize = true;
            this.optScanoMatrix1.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.optScanoMatrix1.Location = new System.Drawing.Point(24, 21);
            this.optScanoMatrix1.Name = "optScanoMatrix1";
            this.optScanoMatrix1.Size = new System.Drawing.Size(99, 19);
            this.optScanoMatrix1.TabIndex = 44;
            this.optScanoMatrix1.TabStop = true;
            this.optScanoMatrix1.Text = "#256×256";
            this.optScanoMatrix1.UseVisualStyleBackColor = true;
            // 
            // fraScanoPitch
            // 
            this.fraScanoPitch.Controls.Add(this.fraSliceWidthPix);
            this.fraScanoPitch.Controls.Add(this.fraSliceWidthMM);
            this.fraScanoPitch.Controls.Add(this.lblScanoSliceMinMax);
            this.fraScanoPitch.Location = new System.Drawing.Point(9, 15);
            this.fraScanoPitch.Name = "fraScanoPitch";
            this.fraScanoPitch.Size = new System.Drawing.Size(165, 105);
            this.fraScanoPitch.TabIndex = 89;
            this.fraScanoPitch.TabStop = false;
            this.fraScanoPitch.Tag = "22001";
            this.fraScanoPitch.Text = "#スキャノピッチ";
            // 
            // fraSliceWidthPix
            // 
            this.fraSliceWidthPix.Controls.Add(this.Label3);
            this.fraSliceWidthPix.Controls.Add(this.cwneScanoSlicePitchPix);
            this.fraSliceWidthPix.Controls.Add(this.lblPixDsp);
            this.fraSliceWidthPix.Location = new System.Drawing.Point(20, 44);
            this.fraSliceWidthPix.Name = "fraSliceWidthPix";
            this.fraSliceWidthPix.Size = new System.Drawing.Size(133, 37);
            this.fraSliceWidthPix.TabIndex = 188;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.Label3.Location = new System.Drawing.Point(4, 16);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(12, 15);
            this.Label3.TabIndex = 93;
            this.Label3.Text = "(";
            // 
            // cwneScanoSlicePitchPix
            // 
            this.cwneScanoSlicePitchPix.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.cwneScanoSlicePitchPix.Location = new System.Drawing.Point(15, 8);
            this.cwneScanoSlicePitchPix.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cwneScanoSlicePitchPix.Name = "cwneScanoSlicePitchPix";
            this.cwneScanoSlicePitchPix.Size = new System.Drawing.Size(75, 27);
            this.cwneScanoSlicePitchPix.TabIndex = 94;
            this.cwneScanoSlicePitchPix.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneScanoSlicePitchPix.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cwneScanoSlicePitchPix.ValueChanged += new System.EventHandler(this.cwneScanoSlicePitchPix_ValueChanged);
            this.cwneScanoSlicePitchPix.Leave += new System.EventHandler(this.NumicValue_Leave);
            // 
            // lblPixDsp
            // 
            this.lblPixDsp.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.lblPixDsp.Location = new System.Drawing.Point(74, 16);
            this.lblPixDsp.Name = "lblPixDsp";
            this.lblPixDsp.Size = new System.Drawing.Size(50, 13);
            this.lblPixDsp.TabIndex = 95;
            this.lblPixDsp.Tag = "10816";
            this.lblPixDsp.Text = "画素)";
            this.lblPixDsp.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // fraSliceWidthMM
            // 
            this.fraSliceWidthMM.Controls.Add(this.cwneScanoSlicePitch);
            this.fraSliceWidthMM.Controls.Add(this.lblSliceUni);
            this.fraSliceWidthMM.Location = new System.Drawing.Point(20, 12);
            this.fraSliceWidthMM.Name = "fraSliceWidthMM";
            this.fraSliceWidthMM.Size = new System.Drawing.Size(133, 37);
            this.fraSliceWidthMM.TabIndex = 90;
            // 
            // cwneScanoSlicePitch
            // 
            this.cwneScanoSlicePitch.DecimalPlaces = 3;
            this.cwneScanoSlicePitch.Enabled = false;
            this.cwneScanoSlicePitch.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.cwneScanoSlicePitch.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.cwneScanoSlicePitch.Location = new System.Drawing.Point(15, 8);
            this.cwneScanoSlicePitch.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.cwneScanoSlicePitch.Name = "cwneScanoSlicePitch";
            this.cwneScanoSlicePitch.Size = new System.Drawing.Size(75, 27);
            this.cwneScanoSlicePitch.TabIndex = 91;
            this.cwneScanoSlicePitch.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneScanoSlicePitch.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            // 
            // lblSliceUni
            // 
            this.lblSliceUni.AutoSize = true;
            this.lblSliceUni.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.lblSliceUni.Location = new System.Drawing.Point(100, 16);
            this.lblSliceUni.Name = "lblSliceUni";
            this.lblSliceUni.Size = new System.Drawing.Size(29, 15);
            this.lblSliceUni.TabIndex = 92;
            this.lblSliceUni.Tag = "";
            this.lblSliceUni.Text = "mm";
            // 
            // lblScanoSliceMinMax
            // 
            this.lblScanoSliceMinMax.AutoSize = true;
            this.lblScanoSliceMinMax.Location = new System.Drawing.Point(31, 84);
            this.lblScanoSliceMinMax.Name = "lblScanoSliceMinMax";
            this.lblScanoSliceMinMax.Size = new System.Drawing.Size(119, 17);
            this.lblScanoSliceMinMax.TabIndex = 96;
            this.lblScanoSliceMinMax.Text = "(0.01～0.84mm)";
            // 
            // fraScanoInteg
            // 
            this.fraScanoInteg.Controls.Add(this.lblScanoIntegMinMax);
            this.fraScanoInteg.Controls.Add(this.lblScanoIntegUni);
            this.fraScanoInteg.Controls.Add(this.cwneScanoInteg);
            this.fraScanoInteg.Location = new System.Drawing.Point(365, 126);
            this.fraScanoInteg.Name = "fraScanoInteg";
            this.fraScanoInteg.Size = new System.Drawing.Size(165, 105);
            this.fraScanoInteg.TabIndex = 104;
            this.fraScanoInteg.TabStop = false;
            this.fraScanoInteg.Tag = "12238";
            this.fraScanoInteg.Text = "#画像積算枚数";
            // 
            // lblScanoIntegMinMax
            // 
            this.lblScanoIntegMinMax.AutoSize = true;
            this.lblScanoIntegMinMax.Location = new System.Drawing.Point(40, 64);
            this.lblScanoIntegMinMax.Name = "lblScanoIntegMinMax";
            this.lblScanoIntegMinMax.Size = new System.Drawing.Size(62, 17);
            this.lblScanoIntegMinMax.TabIndex = 107;
            this.lblScanoIntegMinMax.Text = "(1～50)";
            // 
            // lblScanoIntegUni
            // 
            this.lblScanoIntegUni.AutoSize = true;
            this.lblScanoIntegUni.Location = new System.Drawing.Point(112, 40);
            this.lblScanoIntegUni.Name = "lblScanoIntegUni";
            this.lblScanoIntegUni.Size = new System.Drawing.Size(34, 17);
            this.lblScanoIntegUni.TabIndex = 106;
            this.lblScanoIntegUni.Tag = "10818";
            this.lblScanoIntegUni.Text = "#枚";
            // 
            // cwneScanoInteg
            // 
            this.cwneScanoInteg.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.cwneScanoInteg.Location = new System.Drawing.Point(24, 32);
            this.cwneScanoInteg.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cwneScanoInteg.Name = "cwneScanoInteg";
            this.cwneScanoInteg.Size = new System.Drawing.Size(81, 27);
            this.cwneScanoInteg.TabIndex = 105;
            this.cwneScanoInteg.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneScanoInteg.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cwneScanoInteg.ValueChanged += new System.EventHandler(this.cwneScanoInteg_ValueChanged);
            this.cwneScanoInteg.Leave += new System.EventHandler(this.NumicValue_Leave);
            // 
            // CmdCancel
            // 
            this.CmdCancel.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.CmdCancel.Location = new System.Drawing.Point(632, 508);
            this.CmdCancel.Name = "CmdCancel";
            this.CmdCancel.Size = new System.Drawing.Size(107, 33);
            this.CmdCancel.TabIndex = 187;
            this.CmdCancel.Tag = "10002";
            this.CmdCancel.Text = "#キャンセル";
            this.CmdCancel.UseVisualStyleBackColor = true;
            this.CmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Enabled = false;
            this.cmdOK.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.cmdOK.Location = new System.Drawing.Point(520, 508);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(107, 33);
            this.cmdOK.TabIndex = 186;
            this.cmdOK.Tag = "10001";
            this.cmdOK.Text = "#ＯＫ";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // fraPicSize
            // 
            this.fraPicSize.Controls.Add(this.ntbTransImageHeight);
            this.fraPicSize.Controls.Add(this.ntbTransImageWidth);
            this.fraPicSize.Location = new System.Drawing.Point(12, 503);
            this.fraPicSize.Name = "fraPicSize";
            this.fraPicSize.Size = new System.Drawing.Size(105, 46);
            this.fraPicSize.TabIndex = 188;
            this.fraPicSize.TabStop = false;
            this.fraPicSize.Tag = "12247";
            this.fraPicSize.Text = "透視画像サイズ";
            this.fraPicSize.Visible = false;
            // 
            // ntbTransImageHeight
            // 
            this.ntbTransImageHeight.BackColor = System.Drawing.SystemColors.Control;
            this.ntbTransImageHeight.Caption = "縦";
            this.ntbTransImageHeight.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbTransImageHeight.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbTransImageHeight.CaptionWidth = 16;
            this.ntbTransImageHeight.DiscreteInterval = 1F;
            this.ntbTransImageHeight.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbTransImageHeight.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbTransImageHeight.IncDecButton = false;
            this.ntbTransImageHeight.Location = new System.Drawing.Point(8, 29);
            this.ntbTransImageHeight.Max = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ntbTransImageHeight.Min = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.ntbTransImageHeight.Name = "ntbTransImageHeight";
            this.ntbTransImageHeight.ReadOnly = true;
            this.ntbTransImageHeight.Size = new System.Drawing.Size(81, 14);
            this.ntbTransImageHeight.TabIndex = 1;
            this.ntbTransImageHeight.Tag = "12235";
            this.ntbTransImageHeight.TextBackColor = System.Drawing.SystemColors.Window;
            this.ntbTransImageHeight.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbTransImageHeight.Unit = "画素";
            this.ntbTransImageHeight.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // ntbTransImageWidth
            // 
            this.ntbTransImageWidth.BackColor = System.Drawing.SystemColors.Control;
            this.ntbTransImageWidth.Caption = "横";
            this.ntbTransImageWidth.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbTransImageWidth.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbTransImageWidth.CaptionWidth = 16;
            this.ntbTransImageWidth.DiscreteInterval = 1F;
            this.ntbTransImageWidth.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbTransImageWidth.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbTransImageWidth.IncDecButton = false;
            this.ntbTransImageWidth.Location = new System.Drawing.Point(8, 14);
            this.ntbTransImageWidth.Max = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ntbTransImageWidth.Min = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.ntbTransImageWidth.Name = "ntbTransImageWidth";
            this.ntbTransImageWidth.ReadOnly = true;
            this.ntbTransImageWidth.Size = new System.Drawing.Size(81, 14);
            this.ntbTransImageWidth.TabIndex = 0;
            this.ntbTransImageWidth.Tag = "12234";
            this.ntbTransImageWidth.TextBackColor = System.Drawing.SystemColors.Window;
            this.ntbTransImageWidth.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbTransImageWidth.Unit = "画素";
            this.ntbTransImageWidth.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // frmScanoCondition
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(744, 556);
            this.ControlBox = false;
            this.Controls.Add(this.fraPicSize);
            this.Controls.Add(this.CmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.sstMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(107, 73);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmScanoCondition";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "スキャノ条件";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmScanoCondition_FormClosed);
            this.Load += new System.EventHandler(this.frmScanoCondition_Load);
            this.sstMenu.ResumeLayout(false);
            this.tabPage0.ResumeLayout(false);
            this.fraMenu0.ResumeLayout(false);
            this.fraScanoWidth.ResumeLayout(false);
            this.fraScanoWidth.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneScanoWidthPix)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneScanoWidth)).EndInit();
            this.fraScanoBias.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ntbScanoSlope)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ntbScanoBias)).EndInit();
            this.fraScanoDataRange.ResumeLayout(false);
            this.fraScanoDataRange.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneScanoDataRange)).EndInit();
            this.fraScanoMatrix.ResumeLayout(false);
            this.fraScanoMatrix.PerformLayout();
            this.fraScanoPitch.ResumeLayout(false);
            this.fraScanoPitch.PerformLayout();
            this.fraSliceWidthPix.ResumeLayout(false);
            this.fraSliceWidthPix.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneScanoSlicePitchPix)).EndInit();
            this.fraSliceWidthMM.ResumeLayout(false);
            this.fraSliceWidthMM.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneScanoSlicePitch)).EndInit();
            this.fraScanoInteg.ResumeLayout(false);
            this.fraScanoInteg.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneScanoInteg)).EndInit();
            this.fraPicSize.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl sstMenu;
        private System.Windows.Forms.TabPage tabPage0;
        private System.Windows.Forms.Panel fraMenu0;
        private System.Windows.Forms.GroupBox fraScanoDataRange;
        private System.Windows.Forms.Label lblSlicePitchBetween;
        private System.Windows.Forms.Label lblScanoDataRange_max;
        private System.Windows.Forms.Label lblScanoDataRange_min;
        private System.Windows.Forms.Label lblSlicePitchUni;
        internal System.Windows.Forms.NumericUpDown cwneScanoDataRange;
        private System.Windows.Forms.GroupBox fraScanoMatrix;
        private System.Windows.Forms.RadioButton optScanoMatrix5;
        private System.Windows.Forms.RadioButton optScanoMatrix4;
        private System.Windows.Forms.RadioButton optScanoMatrix3;
        private System.Windows.Forms.RadioButton optScanoMatrix2;
        private System.Windows.Forms.RadioButton optScanoMatrix1;
        private System.Windows.Forms.GroupBox fraScanoPitch;
        private System.Windows.Forms.Panel fraSliceWidthPix;
        private System.Windows.Forms.Label Label3;
        private System.Windows.Forms.NumericUpDown cwneScanoSlicePitchPix;
        private System.Windows.Forms.Label lblPixDsp;
        private System.Windows.Forms.Panel fraSliceWidthMM;
        internal System.Windows.Forms.NumericUpDown cwneScanoSlicePitch;
        private System.Windows.Forms.Label lblSliceUni;
        private System.Windows.Forms.Label lblScanoSliceMinMax;
        private System.Windows.Forms.GroupBox fraScanoInteg;
        private System.Windows.Forms.Label lblScanoIntegMinMax;
        private System.Windows.Forms.Label lblScanoIntegUni;
        private System.Windows.Forms.NumericUpDown cwneScanoInteg;
        private System.Windows.Forms.Button CmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.GroupBox fraScanoBias;
        private System.Windows.Forms.Label ntbScanoSlopeCaption;
        private System.Windows.Forms.NumericUpDown ntbScanoSlope;
        private System.Windows.Forms.Label ntbScanoBiasCaption;
        private System.Windows.Forms.NumericUpDown ntbScanoBias;
        private System.Windows.Forms.GroupBox fraScanoWidth;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown cwneScanoWidthPix;
        private System.Windows.Forms.Label lblPixDsp2;
        private System.Windows.Forms.Panel panel2;
        internal System.Windows.Forms.NumericUpDown cwneScanoWidth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblScanoWidthMinMax;
        private System.Windows.Forms.GroupBox fraPicSize;
        internal NumTextBox ntbTransImageHeight;
        internal NumTextBox ntbTransImageWidth;
        private System.Windows.Forms.Label lblSlicePitchBetween2;
        private System.Windows.Forms.Label lblScanoDataRange_min2;
    }
}