namespace CT30K {
	partial class frmXrayControl {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
		if (disposing && (components != null)) {
		components.Dispose();
		}
		base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.tmrViscomLive = new System.Windows.Forms.Timer(this.components);
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.tmrXrayTool = new System.Windows.Forms.Timer(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.cmdDetail = new System.Windows.Forms.Button();
            this.cmdXrayInfo = new System.Windows.Forms.Button();
            this.fraStatus = new System.Windows.Forms.GroupBox();
            this.ctLblXray = new CT30K.CTLabel();
            this.fraVac = new System.Windows.Forms.GroupBox();
            this.ntbVacSVV = new CT30K.NumTextBox();
            this.lblVac5 = new System.Windows.Forms.Label();
            this.lblVac4 = new System.Windows.Forms.Label();
            this.lblVac3 = new System.Windows.Forms.Label();
            this.lblVac2 = new System.Windows.Forms.Label();
            this.lblVac1 = new System.Windows.Forms.Label();
            this.lblVac0 = new System.Windows.Forms.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.Shape1 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.shpYellow = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.ntbFeedbackVac = new CT30K.NumTextBox();
            this.stsVacuum = new CT30K.CTStatus();
            this.fraWarmup = new System.Windows.Forms.GroupBox();
            this.chkFilament = new System.Windows.Forms.CheckBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.cwneWarmupSetVolt = new System.Windows.Forms.NumericUpDown();
            this.chkStepWU = new System.Windows.Forms.CheckBox();
            this.fraRestTime = new System.Windows.Forms.GroupBox();
            this.Label59 = new System.Windows.Forms.Label();
            this.lblWrestTimeS = new System.Windows.Forms.Label();
            this.Label58 = new System.Windows.Forms.Label();
            this.lblWrestTimeM = new System.Windows.Forms.Label();
            this.Label57 = new System.Windows.Forms.Label();
            this.cmdWarmupStart = new System.Windows.Forms.Button();
            this.lblWarmupStatus = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.fraFocus = new System.Windows.Forms.GroupBox();
            this.cmdFocus4 = new System.Windows.Forms.Button();
            this.cmdFocus3 = new System.Windows.Forms.Button();
            this.cmdFocus2 = new System.Windows.Forms.Button();
            this.cmdFocus1 = new System.Windows.Forms.Button();
            this.fraVoltageAndCurrent = new System.Windows.Forms.GroupBox();
            this.cwsldMA = new System.Windows.Forms.HScrollBar();
            this.cwsldKV = new System.Windows.Forms.HScrollBar();
            this.cwneMA = new System.Windows.Forms.NumericUpDown();
            this.ntbActCurrent = new CT30K.NumTextBox();
            this.ntbActVolt = new CT30K.NumTextBox();
            this.lblMAuni = new System.Windows.Forms.Label();
            this.lblKVuni = new System.Windows.Forms.Label();
            this.ntbSetCurrent = new CT30K.NumTextBox();
            this.cwneKV = new System.Windows.Forms.NumericUpDown();
            this.ntbTargetCurrent = new CT30K.NumTextBox();
            this.ntbFilter = new CT30K.NumTextBox();
            this.ntbSetVolt = new CT30K.NumTextBox();
            this.ctbtnManualSet = new CT30K.CTButton();
            this.fraCondition = new System.Windows.Forms.GroupBox();
            this.cmdCondition2 = new System.Windows.Forms.Button();
            this.cmdCondition1 = new System.Windows.Forms.Button();
            this.cmdCondition0 = new System.Windows.Forms.Button();
            this.tmrTitanUpdate = new System.Windows.Forms.Timer(this.components);
            this.pnlDummy = new System.Windows.Forms.Panel();
            this.fraStatus.SuspendLayout();
            this.fraVac.SuspendLayout();
            this.fraWarmup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneWarmupSetVolt)).BeginInit();
            this.fraRestTime.SuspendLayout();
            this.fraFocus.SuspendLayout();
            this.fraVoltageAndCurrent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneMA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneKV)).BeginInit();
            this.fraCondition.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrViscomLive
            // 
            this.tmrViscomLive.Interval = 3000;
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Interval = 1000;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Timer);
            // 
            // tmrXrayTool
            // 
            this.tmrXrayTool.Interval = 1000;
            this.tmrXrayTool.Tick += new System.EventHandler(this.tmrXrayTool_Timer);
            // 
            // cmdDetail
            // 
            this.cmdDetail.Location = new System.Drawing.Point(716, 132);
            this.cmdDetail.Name = "cmdDetail";
            this.cmdDetail.Size = new System.Drawing.Size(97, 29);
            this.cmdDetail.TabIndex = 48;
            this.cmdDetail.Tag = "10033";
            this.cmdDetail.Text = "#詳細...";
            this.cmdDetail.UseVisualStyleBackColor = true;
            this.cmdDetail.Visible = false;
            this.cmdDetail.Click += new System.EventHandler(this.cmdDetail_Click);
            // 
            // cmdXrayInfo
            // 
            this.cmdXrayInfo.Location = new System.Drawing.Point(716, 97);
            this.cmdXrayInfo.Name = "cmdXrayInfo";
            this.cmdXrayInfo.Size = new System.Drawing.Size(97, 29);
            this.cmdXrayInfo.TabIndex = 47;
            this.cmdXrayInfo.Tag = "14100";
            this.cmdXrayInfo.Text = "#Ｘ線情報...";
            this.cmdXrayInfo.UseVisualStyleBackColor = true;
            this.cmdXrayInfo.Visible = false;
            this.cmdXrayInfo.Click += new System.EventHandler(this.cmdXrayInfo_Click);
            // 
            // fraStatus
            // 
            this.fraStatus.Controls.Add(this.ctLblXray);
            this.fraStatus.Controls.Add(this.fraVac);
            this.fraStatus.Controls.Add(this.ntbFeedbackVac);
            this.fraStatus.Controls.Add(this.stsVacuum);
            this.fraStatus.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fraStatus.Location = new System.Drawing.Point(676, 4);
            this.fraStatus.Name = "fraStatus";
            this.fraStatus.Size = new System.Drawing.Size(153, 91);
            this.fraStatus.TabIndex = 36;
            this.fraStatus.TabStop = false;
            this.fraStatus.Tag = "12033";
            this.fraStatus.Text = "#ステータス";
            // 
            // ctLblXray
            // 
            this.ctLblXray.BlinkInterval = CT30K.BlinkSpeeds.cwSpeedOff;
            this.ctLblXray.Caption = "接続中...";
            this.ctLblXray.CaptionAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.ctLblXray.CaptionWidth = 101;
            this.ctLblXray.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ctLblXray.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ctLblXray.Location = new System.Drawing.Point(24, 18);
            this.ctLblXray.Name = "ctLblXray";
            this.ctLblXray.OffColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ctLblXray.OnColor = System.Drawing.Color.Empty;
            this.ctLblXray.Size = new System.Drawing.Size(101, 18);
            this.ctLblXray.TabIndex = 50;
            // 
            // fraVac
            // 
            this.fraVac.Controls.Add(this.ntbVacSVV);
            this.fraVac.Controls.Add(this.lblVac5);
            this.fraVac.Controls.Add(this.lblVac4);
            this.fraVac.Controls.Add(this.lblVac3);
            this.fraVac.Controls.Add(this.lblVac2);
            this.fraVac.Controls.Add(this.lblVac1);
            this.fraVac.Controls.Add(this.lblVac0);
            this.fraVac.Controls.Add(this.shapeContainer1);
            this.fraVac.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fraVac.Location = new System.Drawing.Point(16, 40);
            this.fraVac.Name = "fraVac";
            this.fraVac.Size = new System.Drawing.Size(121, 41);
            this.fraVac.TabIndex = 38;
            this.fraVac.TabStop = false;
            this.fraVac.Tag = "14003";
            this.fraVac.Text = "#真空度";
            // 
            // ntbVacSVV
            // 
            this.ntbVacSVV.BackColor = System.Drawing.SystemColors.Control;
            this.ntbVacSVV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ntbVacSVV.Caption = "#真空度";
            this.ntbVacSVV.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.ntbVacSVV.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbVacSVV.CaptionWidth = 43;
            this.ntbVacSVV.DiscreteInterval = 0.01F;
            this.ntbVacSVV.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbVacSVV.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbVacSVV.IncDecButton = false;
            this.ntbVacSVV.Location = new System.Drawing.Point(48, 8);
            this.ntbVacSVV.Max = new decimal(new int[] {
            128000,
            0,
            0,
            196608});
            this.ntbVacSVV.Min = new decimal(new int[] {
            0,
            0,
            0,
            196608});
            this.ntbVacSVV.Name = "ntbVacSVV";
            this.ntbVacSVV.ReadOnly = true;
            this.ntbVacSVV.Size = new System.Drawing.Size(80, 17);
            this.ntbVacSVV.TabIndex = 20;
            this.ntbVacSVV.TabStop = false;
            this.ntbVacSVV.Tag = "14001";
            this.ntbVacSVV.TextBackColor = System.Drawing.SystemColors.Window;
            this.ntbVacSVV.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbVacSVV.Unit = "";
            this.ntbVacSVV.Value = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.ntbVacSVV.Visible = false;
            // 
            // lblVac5
            // 
            this.lblVac5.BackColor = System.Drawing.Color.Green;
            this.lblVac5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblVac5.Location = new System.Drawing.Point(85, 16);
            this.lblVac5.Name = "lblVac5";
            this.lblVac5.Size = new System.Drawing.Size(13, 13);
            this.lblVac5.TabIndex = 43;
            this.lblVac5.Tag = "V5";
            // 
            // lblVac4
            // 
            this.lblVac4.BackColor = System.Drawing.Color.Green;
            this.lblVac4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblVac4.Location = new System.Drawing.Point(72, 16);
            this.lblVac4.Name = "lblVac4";
            this.lblVac4.Size = new System.Drawing.Size(13, 13);
            this.lblVac4.TabIndex = 42;
            this.lblVac4.Tag = "V4";
            // 
            // lblVac3
            // 
            this.lblVac3.BackColor = System.Drawing.Color.Green;
            this.lblVac3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblVac3.Location = new System.Drawing.Point(59, 16);
            this.lblVac3.Name = "lblVac3";
            this.lblVac3.Size = new System.Drawing.Size(13, 13);
            this.lblVac3.TabIndex = 41;
            this.lblVac3.Tag = "V3";
            // 
            // lblVac2
            // 
            this.lblVac2.BackColor = System.Drawing.Color.Green;
            this.lblVac2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblVac2.Location = new System.Drawing.Point(46, 16);
            this.lblVac2.Name = "lblVac2";
            this.lblVac2.Size = new System.Drawing.Size(13, 13);
            this.lblVac2.TabIndex = 40;
            this.lblVac2.Tag = "V2";
            // 
            // lblVac1
            // 
            this.lblVac1.BackColor = System.Drawing.Color.Green;
            this.lblVac1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblVac1.Location = new System.Drawing.Point(33, 16);
            this.lblVac1.Name = "lblVac1";
            this.lblVac1.Size = new System.Drawing.Size(13, 13);
            this.lblVac1.TabIndex = 46;
            this.lblVac1.Tag = "V1";
            // 
            // lblVac0
            // 
            this.lblVac0.BackColor = System.Drawing.Color.Green;
            this.lblVac0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblVac0.Location = new System.Drawing.Point(20, 16);
            this.lblVac0.Name = "lblVac0";
            this.lblVac0.Size = new System.Drawing.Size(13, 13);
            this.lblVac0.TabIndex = 39;
            this.lblVac0.Tag = "Lo";
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(3, 16);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.Shape1,
            this.shpYellow});
            this.shapeContainer1.Size = new System.Drawing.Size(115, 22);
            this.shapeContainer1.TabIndex = 47;
            this.shapeContainer1.TabStop = false;
            // 
            // Shape1
            // 
            this.Shape1.BackColor = System.Drawing.SystemColors.Control;
            this.Shape1.BorderColor = System.Drawing.Color.Lime;
            this.Shape1.Cursor = System.Windows.Forms.Cursors.Default;
            this.Shape1.FillColor = System.Drawing.Color.Lime;
            this.Shape1.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
            this.Shape1.Location = new System.Drawing.Point(68, 16);
            this.Shape1.Name = "Shape1";
            this.Shape1.Size = new System.Drawing.Size(25, 5);
            // 
            // shpYellow
            // 
            this.shpYellow.BorderColor = System.Drawing.Color.Yellow;
            this.shpYellow.FillColor = System.Drawing.Color.Yellow;
            this.shpYellow.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
            this.shpYellow.Location = new System.Drawing.Point(44, 16);
            this.shpYellow.Name = "shpYellow";
            this.shpYellow.Size = new System.Drawing.Size(25, 5);
            // 
            // ntbFeedbackVac
            // 
            this.ntbFeedbackVac.BackColor = System.Drawing.SystemColors.Control;
            this.ntbFeedbackVac.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ntbFeedbackVac.Caption = "#真空度";
            this.ntbFeedbackVac.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.ntbFeedbackVac.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbFeedbackVac.CaptionWidth = 55;
            this.ntbFeedbackVac.DiscreteInterval = 1F;
            this.ntbFeedbackVac.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbFeedbackVac.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbFeedbackVac.IncDecButton = false;
            this.ntbFeedbackVac.Location = new System.Drawing.Point(6, 60);
            this.ntbFeedbackVac.Max = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ntbFeedbackVac.Min = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.ntbFeedbackVac.Name = "ntbFeedbackVac";
            this.ntbFeedbackVac.ReadOnly = true;
            this.ntbFeedbackVac.Size = new System.Drawing.Size(107, 18);
            this.ntbFeedbackVac.TabIndex = 45;
            this.ntbFeedbackVac.TabStop = false;
            this.ntbFeedbackVac.Tag = "14003";
            this.ntbFeedbackVac.TextBackColor = System.Drawing.SystemColors.Window;
            this.ntbFeedbackVac.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbFeedbackVac.Unit = "";
            this.ntbFeedbackVac.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // stsVacuum
            // 
            this.stsVacuum.Caption = "真空ﾎﾟﾝﾌﾟ";
            this.stsVacuum.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.stsVacuum.CaptionWidth = 65;
            this.stsVacuum.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stsVacuum.Location = new System.Drawing.Point(6, 40);
            this.stsVacuum.Name = "stsVacuum";
            this.stsVacuum.Size = new System.Drawing.Size(133, 18);
            this.stsVacuum.Status = "";
            this.stsVacuum.TabIndex = 44;
            this.stsVacuum.TabStop = false;
            this.stsVacuum.Tag = "14004";
            // 
            // fraWarmup
            // 
            this.fraWarmup.Controls.Add(this.chkFilament);
            this.fraWarmup.Controls.Add(this.Label2);
            this.fraWarmup.Controls.Add(this.cwneWarmupSetVolt);
            this.fraWarmup.Controls.Add(this.chkStepWU);
            this.fraWarmup.Controls.Add(this.fraRestTime);
            this.fraWarmup.Controls.Add(this.cmdWarmupStart);
            this.fraWarmup.Controls.Add(this.lblWarmupStatus);
            this.fraWarmup.Controls.Add(this.Label1);
            this.fraWarmup.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fraWarmup.Location = new System.Drawing.Point(480, 4);
            this.fraWarmup.Name = "fraWarmup";
            this.fraWarmup.Size = new System.Drawing.Size(189, 160);
            this.fraWarmup.TabIndex = 24;
            this.fraWarmup.TabStop = false;
            this.fraWarmup.Tag = "10524";
            this.fraWarmup.Text = "#ウォームアップ";
            // 
            // chkFilament
            // 
            this.chkFilament.AutoSize = true;
            this.chkFilament.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkFilament.Location = new System.Drawing.Point(20, 120);
            this.chkFilament.Name = "chkFilament";
            this.chkFilament.Size = new System.Drawing.Size(124, 16);
            this.chkFilament.TabIndex = 35;
            this.chkFilament.Tag = "16109";
            this.chkFilament.Text = "フィラメント調整も行う";
            this.chkFilament.UseVisualStyleBackColor = true;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Label2.Location = new System.Drawing.Point(88, 102);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(62, 12);
            this.Label2.TabIndex = 34;
            this.Label2.Tag = "16108";
            this.Label2.Text = "kVまで実行";
            // 
            // cwneWarmupSetVolt
            // 
            this.cwneWarmupSetVolt.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneWarmupSetVolt.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.cwneWarmupSetVolt.Location = new System.Drawing.Point(24, 96);
            this.cwneWarmupSetVolt.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.cwneWarmupSetVolt.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.cwneWarmupSetVolt.Name = "cwneWarmupSetVolt";
            this.cwneWarmupSetVolt.Size = new System.Drawing.Size(57, 23);
            this.cwneWarmupSetVolt.TabIndex = 33;
            this.cwneWarmupSetVolt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneWarmupSetVolt.ValueChanged += new System.EventHandler(this.cwneWarmupSetVolt_ValueChanged);
            // 
            // chkStepWU
            // 
            this.chkStepWU.AutoSize = true;
            this.chkStepWU.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkStepWU.Location = new System.Drawing.Point(112, 64);
            this.chkStepWU.Name = "chkStepWU";
            this.chkStepWU.Size = new System.Drawing.Size(58, 16);
            this.chkStepWU.TabIndex = 50;
            this.chkStepWU.Tag = "16113";
            this.chkStepWU.Text = "ステップ";
            this.chkStepWU.UseVisualStyleBackColor = true;
            this.chkStepWU.Click += new System.EventHandler(this.chkStepWU_Click);
            // 
            // fraRestTime
            // 
            this.fraRestTime.Controls.Add(this.Label59);
            this.fraRestTime.Controls.Add(this.lblWrestTimeS);
            this.fraRestTime.Controls.Add(this.Label58);
            this.fraRestTime.Controls.Add(this.lblWrestTimeM);
            this.fraRestTime.Controls.Add(this.Label57);
            this.fraRestTime.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fraRestTime.Location = new System.Drawing.Point(8, 56);
            this.fraRestTime.Name = "fraRestTime";
            this.fraRestTime.Size = new System.Drawing.Size(97, 33);
            this.fraRestTime.TabIndex = 28;
            this.fraRestTime.TabStop = false;
            this.fraRestTime.Visible = false;
            // 
            // Label59
            // 
            this.Label59.AutoSize = true;
            this.Label59.Location = new System.Drawing.Point(69, 16);
            this.Label59.Name = "Label59";
            this.Label59.Size = new System.Drawing.Size(17, 12);
            this.Label59.TabIndex = 32;
            this.Label59.Tag = "12180";
            this.Label59.Text = "秒";
            // 
            // lblWrestTimeS
            // 
            this.lblWrestTimeS.AutoSize = true;
            this.lblWrestTimeS.Location = new System.Drawing.Point(48, 16);
            this.lblWrestTimeS.Name = "lblWrestTimeS";
            this.lblWrestTimeS.Size = new System.Drawing.Size(17, 12);
            this.lblWrestTimeS.TabIndex = 31;
            this.lblWrestTimeS.Text = "00";
            // 
            // Label58
            // 
            this.Label58.AutoSize = true;
            this.Label58.Location = new System.Drawing.Point(32, 16);
            this.Label58.Name = "Label58";
            this.Label58.Size = new System.Drawing.Size(17, 12);
            this.Label58.TabIndex = 30;
            this.Label58.Tag = "12181";
            this.Label58.Text = "分";
            // 
            // lblWrestTimeM
            // 
            this.lblWrestTimeM.AutoSize = true;
            this.lblWrestTimeM.Location = new System.Drawing.Point(10, 16);
            this.lblWrestTimeM.Name = "lblWrestTimeM";
            this.lblWrestTimeM.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblWrestTimeM.Size = new System.Drawing.Size(17, 12);
            this.lblWrestTimeM.TabIndex = 29;
            this.lblWrestTimeM.Text = "11";
            // 
            // Label57
            // 
            this.Label57.AutoSize = true;
            this.Label57.Location = new System.Drawing.Point(0, 0);
            this.Label57.Name = "Label57";
            this.Label57.Size = new System.Drawing.Size(55, 12);
            this.Label57.TabIndex = 27;
            this.Label57.Tag = "12176";
            this.Label57.Text = "#残り時間";
            // 
            // cmdWarmupStart
            // 
            this.cmdWarmupStart.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdWarmupStart.Location = new System.Drawing.Point(112, 28);
            this.cmdWarmupStart.Name = "cmdWarmupStart";
            this.cmdWarmupStart.Size = new System.Drawing.Size(61, 23);
            this.cmdWarmupStart.TabIndex = 26;
            this.cmdWarmupStart.Tag = "10018";
            this.cmdWarmupStart.Text = "#開始";
            this.cmdWarmupStart.UseVisualStyleBackColor = true;
            this.cmdWarmupStart.Click += new System.EventHandler(this.cmdWarmupStart_Click);
            // 
            // lblWarmupStatus
            // 
            this.lblWarmupStatus.BackColor = System.Drawing.Color.Yellow;
            this.lblWarmupStatus.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblWarmupStatus.Location = new System.Drawing.Point(12, 32);
            this.lblWarmupStatus.Name = "lblWarmupStatus";
            this.lblWarmupStatus.Size = new System.Drawing.Size(93, 18);
            this.lblWarmupStatus.TabIndex = 25;
            this.lblWarmupStatus.Text = "準備未完了";
            this.lblWarmupStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblWarmupStatus.TextChanged += new System.EventHandler(this.lblWarmupStatus_Change);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Label1.Location = new System.Drawing.Point(8, 136);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(75, 12);
            this.Label1.TabIndex = 49;
            this.Label1.Tag = "20186";
            this.Label1.Text = "Perform up to";
            this.Label1.Visible = false;
            // 
            // fraFocus
            // 
            this.fraFocus.Controls.Add(this.cmdFocus4);
            this.fraFocus.Controls.Add(this.cmdFocus3);
            this.fraFocus.Controls.Add(this.cmdFocus2);
            this.fraFocus.Controls.Add(this.cmdFocus1);
            this.fraFocus.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fraFocus.Location = new System.Drawing.Point(416, 4);
            this.fraFocus.Name = "fraFocus";
            this.fraFocus.Size = new System.Drawing.Size(57, 160);
            this.fraFocus.TabIndex = 18;
            this.fraFocus.TabStop = false;
            this.fraFocus.Tag = "12164";
            this.fraFocus.Text = "#焦点";
            this.fraFocus.Visible = false;
            // 
            // cmdFocus4
            // 
            this.cmdFocus4.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdFocus4.Location = new System.Drawing.Point(10, 108);
            this.cmdFocus4.Name = "cmdFocus4";
            this.cmdFocus4.Size = new System.Drawing.Size(37, 19);
            this.cmdFocus4.TabIndex = 23;
            this.cmdFocus4.Text = "F4";
            this.cmdFocus4.UseVisualStyleBackColor = true;
            this.cmdFocus4.Click += new System.EventHandler(this.cmdFocus_Click);
            // 
            // cmdFocus3
            // 
            this.cmdFocus3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdFocus3.Location = new System.Drawing.Point(10, 80);
            this.cmdFocus3.Name = "cmdFocus3";
            this.cmdFocus3.Size = new System.Drawing.Size(37, 19);
            this.cmdFocus3.TabIndex = 22;
            this.cmdFocus3.Text = "F3";
            this.cmdFocus3.UseVisualStyleBackColor = true;
            this.cmdFocus3.Click += new System.EventHandler(this.cmdFocus_Click);
            // 
            // cmdFocus2
            // 
            this.cmdFocus2.BackColor = System.Drawing.SystemColors.Control;
            this.cmdFocus2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdFocus2.Location = new System.Drawing.Point(10, 52);
            this.cmdFocus2.Name = "cmdFocus2";
            this.cmdFocus2.Size = new System.Drawing.Size(37, 19);
            this.cmdFocus2.TabIndex = 21;
            this.cmdFocus2.Text = "F2";
            this.cmdFocus2.UseVisualStyleBackColor = true;
            this.cmdFocus2.Click += new System.EventHandler(this.cmdFocus_Click);
            // 
            // cmdFocus1
            // 
            this.cmdFocus1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdFocus1.Location = new System.Drawing.Point(10, 24);
            this.cmdFocus1.Name = "cmdFocus1";
            this.cmdFocus1.Size = new System.Drawing.Size(37, 19);
            this.cmdFocus1.TabIndex = 19;
            this.cmdFocus1.Text = "F1";
            this.cmdFocus1.UseVisualStyleBackColor = true;
            this.cmdFocus1.Click += new System.EventHandler(this.cmdFocus_Click);
            // 
            // fraVoltageAndCurrent
            // 
            this.fraVoltageAndCurrent.Controls.Add(this.cwsldMA);
            this.fraVoltageAndCurrent.Controls.Add(this.cwsldKV);
            this.fraVoltageAndCurrent.Controls.Add(this.cwneMA);
            this.fraVoltageAndCurrent.Controls.Add(this.ntbActCurrent);
            this.fraVoltageAndCurrent.Controls.Add(this.ntbActVolt);
            this.fraVoltageAndCurrent.Controls.Add(this.lblMAuni);
            this.fraVoltageAndCurrent.Controls.Add(this.lblKVuni);
            this.fraVoltageAndCurrent.Controls.Add(this.ntbSetCurrent);
            this.fraVoltageAndCurrent.Controls.Add(this.cwneKV);
            this.fraVoltageAndCurrent.Controls.Add(this.ntbTargetCurrent);
            this.fraVoltageAndCurrent.Controls.Add(this.ntbFilter);
            this.fraVoltageAndCurrent.Controls.Add(this.ntbSetVolt);
            this.fraVoltageAndCurrent.Controls.Add(this.ctbtnManualSet);
            this.fraVoltageAndCurrent.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fraVoltageAndCurrent.Location = new System.Drawing.Point(85, 4);
            this.fraVoltageAndCurrent.Name = "fraVoltageAndCurrent";
            this.fraVoltageAndCurrent.Size = new System.Drawing.Size(325, 160);
            this.fraVoltageAndCurrent.TabIndex = 4;
            this.fraVoltageAndCurrent.TabStop = false;
            this.fraVoltageAndCurrent.Tag = "12242";
            this.fraVoltageAndCurrent.Text = "#管電圧／管電流";
            // 
            // cwsldMA
            // 
            this.cwsldMA.Location = new System.Drawing.Point(8, 112);
            this.cwsldMA.Name = "cwsldMA";
            this.cwsldMA.Size = new System.Drawing.Size(303, 14);
            this.cwsldMA.TabIndex = 9;
            this.cwsldMA.TabStop = true;
            this.cwsldMA.Visible = false;
            this.cwsldMA.Scroll += new System.Windows.Forms.ScrollEventHandler(this.cwsldMA_Scroll);
            this.cwsldMA.ValueChanged += new System.EventHandler(this.cwsldMA_ValueChanged);
            // 
            // cwsldKV
            // 
            this.cwsldKV.Location = new System.Drawing.Point(8, 66);
            this.cwsldKV.Name = "cwsldKV";
            this.cwsldKV.Size = new System.Drawing.Size(303, 14);
            this.cwsldKV.TabIndex = 8;
            this.cwsldKV.TabStop = true;
            this.cwsldKV.Visible = false;
            this.cwsldKV.Scroll += new System.Windows.Forms.ScrollEventHandler(this.cwsldKV_Scroll);
            this.cwsldKV.ValueChanged += new System.EventHandler(this.cwsldKV_ValueChanged);
            // 
            // cwneMA
            // 
            this.cwneMA.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneMA.Location = new System.Drawing.Point(130, 88);
            this.cwneMA.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.cwneMA.Name = "cwneMA";
            this.cwneMA.Size = new System.Drawing.Size(63, 22);
            this.cwneMA.TabIndex = 9;
            this.cwneMA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneMA.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.cwneMA.Visible = false;
            this.cwneMA.ValueChanged += new System.EventHandler(this.cwneMA_ValueChanged);
            // 
            // ntbActCurrent
            // 
            this.ntbActCurrent.BackColor = System.Drawing.SystemColors.Control;
            this.ntbActCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ntbActCurrent.Caption = "Act";
            this.ntbActCurrent.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.ntbActCurrent.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbActCurrent.CaptionWidth = 24;
            this.ntbActCurrent.DiscreteInterval = 1F;
            this.ntbActCurrent.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbActCurrent.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbActCurrent.IncDecButton = false;
            this.ntbActCurrent.Location = new System.Drawing.Point(224, 88);
            this.ntbActCurrent.Max = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ntbActCurrent.Min = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.ntbActCurrent.Name = "ntbActCurrent";
            this.ntbActCurrent.ReadOnly = true;
            this.ntbActCurrent.Size = new System.Drawing.Size(93, 22);
            this.ntbActCurrent.TabIndex = 11;
            this.ntbActCurrent.TabStop = false;
            this.ntbActCurrent.TextBackColor = System.Drawing.SystemColors.Control;
            this.ntbActCurrent.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbActCurrent.Unit = "μA";
            this.ntbActCurrent.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // ntbActVolt
            // 
            this.ntbActVolt.BackColor = System.Drawing.SystemColors.Control;
            this.ntbActVolt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ntbActVolt.Caption = "Act";
            this.ntbActVolt.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.ntbActVolt.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbActVolt.CaptionWidth = 23;
            this.ntbActVolt.DiscreteInterval = 1F;
            this.ntbActVolt.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbActVolt.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbActVolt.IncDecButton = false;
            this.ntbActVolt.Location = new System.Drawing.Point(224, 42);
            this.ntbActVolt.Max = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ntbActVolt.Min = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.ntbActVolt.Name = "ntbActVolt";
            this.ntbActVolt.ReadOnly = true;
            this.ntbActVolt.Size = new System.Drawing.Size(87, 22);
            this.ntbActVolt.TabIndex = 7;
            this.ntbActVolt.TabStop = false;
            this.ntbActVolt.TextBackColor = System.Drawing.SystemColors.Control;
            this.ntbActVolt.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbActVolt.Unit = "kV";
            this.ntbActVolt.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // lblMAuni
            // 
            this.lblMAuni.AutoSize = true;
            this.lblMAuni.Location = new System.Drawing.Point(191, 92);
            this.lblMAuni.Name = "lblMAuni";
            this.lblMAuni.Size = new System.Drawing.Size(28, 13);
            this.lblMAuni.TabIndex = 13;
            this.lblMAuni.Tag = "10815";
            this.lblMAuni.Text = "μA";
            // 
            // lblKVuni
            // 
            this.lblKVuni.AutoSize = true;
            this.lblKVuni.Location = new System.Drawing.Point(194, 46);
            this.lblKVuni.Name = "lblKVuni";
            this.lblKVuni.Size = new System.Drawing.Size(21, 13);
            this.lblKVuni.TabIndex = 8;
            this.lblKVuni.Tag = "10813";
            this.lblKVuni.Text = "kV";
            // 
            // ntbSetCurrent
            // 
            this.ntbSetCurrent.BackColor = System.Drawing.SystemColors.Control;
            this.ntbSetCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ntbSetCurrent.Caption = "管電流";
            this.ntbSetCurrent.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.ntbSetCurrent.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbSetCurrent.CaptionWidth = 44;
            this.ntbSetCurrent.DiscreteInterval = 1F;
            this.ntbSetCurrent.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbSetCurrent.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbSetCurrent.IncDecButton = false;
            this.ntbSetCurrent.Location = new System.Drawing.Point(8, 88);
            this.ntbSetCurrent.Max = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ntbSetCurrent.Min = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.ntbSetCurrent.Name = "ntbSetCurrent";
            this.ntbSetCurrent.ReadOnly = true;
            this.ntbSetCurrent.Size = new System.Drawing.Size(119, 22);
            this.ntbSetCurrent.TabIndex = 10;
            this.ntbSetCurrent.TabStop = false;
            this.ntbSetCurrent.Tag = "12807";
            this.ntbSetCurrent.TextBackColor = System.Drawing.SystemColors.Control;
            this.ntbSetCurrent.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbSetCurrent.Unit = "μA";
            this.ntbSetCurrent.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ntbSetCurrent.ValueChanged += new CT30K.NumTextBox.ValueChangedEventHandler(this.ntbSetCurrent_ValueChanged);
            // 
            // cwneKV
            // 
            this.cwneKV.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneKV.Location = new System.Drawing.Point(130, 42);
            this.cwneKV.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.cwneKV.Name = "cwneKV";
            this.cwneKV.Size = new System.Drawing.Size(63, 22);
            this.cwneKV.TabIndex = 5;
            this.cwneKV.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneKV.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.cwneKV.Visible = false;
            this.cwneKV.ValueChanged += new System.EventHandler(this.cwneKV_ValueChanged);
            // 
            // ntbTargetCurrent
            // 
            this.ntbTargetCurrent.BackColor = System.Drawing.SystemColors.Control;
            this.ntbTargetCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ntbTargetCurrent.Caption = "ﾀｰｹﾞｯﾄ電流";
            this.ntbTargetCurrent.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.ntbTargetCurrent.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbTargetCurrent.CaptionWidth = 65;
            this.ntbTargetCurrent.DiscreteInterval = 0.1F;
            this.ntbTargetCurrent.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbTargetCurrent.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbTargetCurrent.IncDecButton = false;
            this.ntbTargetCurrent.Location = new System.Drawing.Point(184, 132);
            this.ntbTargetCurrent.Max = new decimal(new int[] {
            -2147483617,
            0,
            0,
            65536});
            this.ntbTargetCurrent.Min = new decimal(new int[] {
            -2147483616,
            0,
            0,
            -2147418112});
            this.ntbTargetCurrent.Name = "ntbTargetCurrent";
            this.ntbTargetCurrent.ReadOnly = true;
            this.ntbTargetCurrent.Size = new System.Drawing.Size(137, 22);
            this.ntbTargetCurrent.TabIndex = 14;
            this.ntbTargetCurrent.TabStop = false;
            this.ntbTargetCurrent.Tag = "14001";
            this.ntbTargetCurrent.TextBackColor = System.Drawing.SystemColors.Control;
            this.ntbTargetCurrent.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbTargetCurrent.Unit = "μA";
            this.ntbTargetCurrent.Value = new decimal(new int[] {
            0,
            0,
            0,
            65536});
            // 
            // ntbFilter
            // 
            this.ntbFilter.BackColor = System.Drawing.SystemColors.Control;
            this.ntbFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ntbFilter.Caption = "推奨フィルター厚";
            this.ntbFilter.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.ntbFilter.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbFilter.CaptionWidth = 103;
            this.ntbFilter.DiscreteInterval = 0.1F;
            this.ntbFilter.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbFilter.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbFilter.IncDecButton = false;
            this.ntbFilter.Location = new System.Drawing.Point(7, 132);
            this.ntbFilter.Max = new decimal(new int[] {
            999999,
            0,
            0,
            65536});
            this.ntbFilter.Min = new decimal(new int[] {
            999999,
            0,
            0,
            -2147418112});
            this.ntbFilter.Name = "ntbFilter";
            this.ntbFilter.ReadOnly = true;
            this.ntbFilter.Size = new System.Drawing.Size(165, 22);
            this.ntbFilter.TabIndex = 13;
            this.ntbFilter.TabStop = false;
            this.ntbFilter.Tag = "16110";
            this.ntbFilter.TextBackColor = System.Drawing.SystemColors.Window;
            this.ntbFilter.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbFilter.Unit = "mm";
            this.ntbFilter.Value = new decimal(new int[] {
            0,
            0,
            0,
            65536});
            this.ntbFilter.ValueChanged += new CT30K.NumTextBox.ValueChangedEventHandler(this.ntbFilter_ValueChanged);
            // 
            // ntbSetVolt
            // 
            this.ntbSetVolt.BackColor = System.Drawing.SystemColors.Control;
            this.ntbSetVolt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ntbSetVolt.Caption = "管電圧";
            this.ntbSetVolt.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.ntbSetVolt.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbSetVolt.CaptionWidth = 44;
            this.ntbSetVolt.DiscreteInterval = 1F;
            this.ntbSetVolt.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbSetVolt.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbSetVolt.IncDecButton = false;
            this.ntbSetVolt.Location = new System.Drawing.Point(8, 42);
            this.ntbSetVolt.Max = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ntbSetVolt.Min = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.ntbSetVolt.Name = "ntbSetVolt";
            this.ntbSetVolt.ReadOnly = true;
            this.ntbSetVolt.Size = new System.Drawing.Size(113, 22);
            this.ntbSetVolt.TabIndex = 6;
            this.ntbSetVolt.TabStop = false;
            this.ntbSetVolt.Tag = "12806";
            this.ntbSetVolt.TextBackColor = System.Drawing.SystemColors.Control;
            this.ntbSetVolt.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbSetVolt.Unit = "kV";
            this.ntbSetVolt.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ntbSetVolt.ValueChanged += new CT30K.NumTextBox.ValueChangedEventHandler(this.ntbSetVolt_ValueChanged);
            // 
            // ctbtnManualSet
            // 
            this.ctbtnManualSet.Caption = "手動設定";
            this.ctbtnManualSet.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ctbtnManualSet.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ctbtnManualSet.Header = "";
            this.ctbtnManualSet.Location = new System.Drawing.Point(230, 14);
            this.ctbtnManualSet.Name = "ctbtnManualSet";
            this.ctbtnManualSet.OffColor = System.Drawing.Color.Green;
            this.ctbtnManualSet.OnColor = System.Drawing.Color.Lime;
            this.ctbtnManualSet.Picture = null;
            this.ctbtnManualSet.Size = new System.Drawing.Size(71, 22);
            this.ctbtnManualSet.TabIndex = 5;
            this.ctbtnManualSet.Tag = "20175";
            this.ctbtnManualSet.Value = false;
            this.ctbtnManualSet.Click += new System.EventHandler(this.ctbtnManualSet_Click);
            // 
            // fraCondition
            // 
            this.fraCondition.Controls.Add(this.cmdCondition2);
            this.fraCondition.Controls.Add(this.cmdCondition1);
            this.fraCondition.Controls.Add(this.cmdCondition0);
            this.fraCondition.Font = new System.Drawing.Font("MS UI Gothic", 8.25F);
            this.fraCondition.Location = new System.Drawing.Point(4, 4);
            this.fraCondition.Name = "fraCondition";
            this.fraCondition.Size = new System.Drawing.Size(73, 160);
            this.fraCondition.TabIndex = 0;
            this.fraCondition.TabStop = false;
            this.fraCondition.Tag = "12241";
            this.fraCondition.Text = "Ｘ線条件";
            // 
            // cmdCondition2
            // 
            this.cmdCondition2.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdCondition2.Location = new System.Drawing.Point(12, 108);
            this.cmdCondition2.Name = "cmdCondition2";
            this.cmdCondition2.Size = new System.Drawing.Size(53, 22);
            this.cmdCondition2.TabIndex = 3;
            this.cmdCondition2.Text = "H";
            this.cmdCondition2.UseVisualStyleBackColor = true;
            this.cmdCondition2.Click += new System.EventHandler(this.cmdCondition_Click);
            // 
            // cmdCondition1
            // 
            this.cmdCondition1.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdCondition1.Location = new System.Drawing.Point(12, 72);
            this.cmdCondition1.Name = "cmdCondition1";
            this.cmdCondition1.Size = new System.Drawing.Size(53, 22);
            this.cmdCondition1.TabIndex = 2;
            this.cmdCondition1.Text = "M";
            this.cmdCondition1.UseVisualStyleBackColor = true;
            this.cmdCondition1.Click += new System.EventHandler(this.cmdCondition_Click);
            // 
            // cmdCondition0
            // 
            this.cmdCondition0.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdCondition0.Location = new System.Drawing.Point(12, 36);
            this.cmdCondition0.Name = "cmdCondition0";
            this.cmdCondition0.Size = new System.Drawing.Size(53, 22);
            this.cmdCondition0.TabIndex = 1;
            this.cmdCondition0.Text = "L";
            this.cmdCondition0.UseVisualStyleBackColor = true;
            this.cmdCondition0.Click += new System.EventHandler(this.cmdCondition_Click);
            // 
            // tmrTitanUpdate
            // 
            this.tmrTitanUpdate.Interval = 5000;
            this.tmrTitanUpdate.Tick += new System.EventHandler(this.tmrTitanUpdate_Tick);
            // 
            // pnlDummy
            // 
            this.pnlDummy.Location = new System.Drawing.Point(681, 121);
            this.pnlDummy.Name = "pnlDummy";
            this.pnlDummy.Size = new System.Drawing.Size(19, 39);
            this.pnlDummy.TabIndex = 49;
            // 
            // frmXrayControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(836, 172);
            this.Controls.Add(this.pnlDummy);
            this.Controls.Add(this.cmdDetail);
            this.Controls.Add(this.cmdXrayInfo);
            this.Controls.Add(this.fraStatus);
            this.Controls.Add(this.fraWarmup);
            this.Controls.Add(this.fraFocus);
            this.Controls.Add(this.fraVoltageAndCurrent);
            this.Controls.Add(this.fraCondition);
            this.Name = "frmXrayControl";
            this.ShowInTaskbar = false;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_FormClosed);
            this.Load += new System.EventHandler(this.Form_Load);
            this.fraStatus.ResumeLayout(false);
            this.fraVac.ResumeLayout(false);
            this.fraWarmup.ResumeLayout(false);
            this.fraWarmup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneWarmupSetVolt)).EndInit();
            this.fraRestTime.ResumeLayout(false);
            this.fraRestTime.PerformLayout();
            this.fraFocus.ResumeLayout(false);
            this.fraVoltageAndCurrent.ResumeLayout(false);
            this.fraVoltageAndCurrent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneMA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneKV)).EndInit();
            this.fraCondition.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox fraCondition;
        private System.Windows.Forms.GroupBox fraVoltageAndCurrent;
		private System.Windows.Forms.GroupBox fraWarmup;
		private System.Windows.Forms.GroupBox fraStatus;
		private System.Windows.Forms.Button cmdCondition2;
		private System.Windows.Forms.Button cmdCondition1;
		private System.Windows.Forms.Button cmdCondition0;
		private NumTextBox ntbActVolt;
		private System.Windows.Forms.Label lblMAuni;
        private System.Windows.Forms.Label lblKVuni;
        private CTButton ctbtnManualSet;
        private NumTextBox ntbFilter;
		private System.Windows.Forms.Button cmdFocus4;
		private System.Windows.Forms.Button cmdFocus3;
		private System.Windows.Forms.Button cmdFocus2;
		private System.Windows.Forms.Button cmdFocus1;
        private System.Windows.Forms.GroupBox fraRestTime;
		private System.Windows.Forms.Label Label1;
		private System.Windows.Forms.CheckBox chkFilament;
        private System.Windows.Forms.Label Label2;
		private System.Windows.Forms.Label Label59;
		private System.Windows.Forms.Label lblWrestTimeS;
		private System.Windows.Forms.Label Label58;
		private System.Windows.Forms.Label lblWrestTimeM;
		private System.Windows.Forms.Label Label57;
		private System.Windows.Forms.GroupBox fraVac;
		private System.Windows.Forms.Label lblVac5;
		private System.Windows.Forms.Label lblVac4;
		private System.Windows.Forms.Label lblVac3;
		private System.Windows.Forms.Label lblVac2;
		private System.Windows.Forms.Label lblVac1;
		private System.Windows.Forms.Label lblVac0;
		private NumTextBox ntbFeedbackVac;
        private CTStatus stsVacuum;
		private NumTextBox ntbVacSVV;
		private System.Windows.Forms.Button cmdXrayInfo;
		private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
		private Microsoft.VisualBasic.PowerPacks.RectangleShape Shape1;
		private Microsoft.VisualBasic.PowerPacks.RectangleShape shpYellow;
        private System.Windows.Forms.Timer tmrViscomLive;
        private System.Windows.Forms.ToolTip toolTip;
		internal NumTextBox ntbSetCurrent;
		internal NumTextBox ntbActCurrent;
		internal NumTextBox ntbTargetCurrent;
		internal System.Windows.Forms.Label lblWarmupStatus;
		internal System.Windows.Forms.Timer tmrXrayTool;
		internal System.Windows.Forms.NumericUpDown cwneKV;
		internal NumTextBox ntbSetVolt;
		internal System.Windows.Forms.NumericUpDown cwneWarmupSetVolt;
		internal System.Windows.Forms.Button cmdDetail;
        internal System.Windows.Forms.NumericUpDown cwneMA;
        private System.Windows.Forms.HScrollBar cwsldMA;
        private CTLabel ctLblXray;
        internal System.Windows.Forms.GroupBox fraFocus;
        internal System.Windows.Forms.Timer tmrUpdate;
        internal System.Windows.Forms.Timer tmrTitanUpdate;
        internal System.Windows.Forms.HScrollBar cwsldKV;
        private System.Windows.Forms.Panel pnlDummy;
        internal System.Windows.Forms.CheckBox chkStepWU;
        internal System.Windows.Forms.Button cmdWarmupStart;
	}
}