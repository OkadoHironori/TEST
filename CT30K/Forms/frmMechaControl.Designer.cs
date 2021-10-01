namespace CT30K {
	partial class frmMechaControl {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMechaControl));
            this.tmrTryReloadForms = new System.Windows.Forms.Timer(this.components);
            this.tmrMecainf = new System.Windows.Forms.Timer(this.components);
            this.tmrSeqComm = new System.Windows.Forms.Timer(this.components);
            this.tmrPIOCheck = new System.Windows.Forms.Timer(this.components);
            this.ImageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.cmdDetails = new System.Windows.Forms.Button();
            this.cmdMechaAllReset = new System.Windows.Forms.Button();
            this.fraIris = new System.Windows.Forms.GroupBox();
            this.cmdIris0 = new System.Windows.Forms.Button();
            this.cmdIris5 = new System.Windows.Forms.Button();
            this.cmdIris2 = new System.Windows.Forms.Button();
            this.cmdIris7 = new System.Windows.Forms.Button();
            this.cmdIris6 = new System.Windows.Forms.Button();
            this.cmdIris1 = new System.Windows.Forms.Button();
            this.cmdIris4 = new System.Windows.Forms.Button();
            this.cmdIris3 = new System.Windows.Forms.Button();
            this.fraXrayRotate = new System.Windows.Forms.GroupBox();
            this.cwbtnMove6 = new CT30K.CWButton();
            this.cwbtnMove7 = new CT30K.CWButton();
            this.cwbtnMove8 = new CT30K.CWButton();
            this.cwbtnMove9 = new CT30K.CWButton();
            this.cwbtnRotateXray1 = new CT30K.CWButton();
            this.cwbtnRotateXray0 = new CT30K.CWButton();
            this.fraCollimator = new System.Windows.Forms.GroupBox();
            this.cmdCollimator0 = new System.Windows.Forms.Button();
            this.cmdCollimator2 = new System.Windows.Forms.Button();
            this.cmdCollimator6 = new System.Windows.Forms.Button();
            this.cmdCollimator4 = new System.Windows.Forms.Button();
            this.cmdCollimator3 = new System.Windows.Forms.Button();
            this.cmdCollimator1 = new System.Windows.Forms.Button();
            this.cmdCollimator7 = new System.Windows.Forms.Button();
            this.cmdCollimator5 = new System.Windows.Forms.Button();
            this.fraError = new System.Windows.Forms.GroupBox();
            this.lblMechaStatus7 = new System.Windows.Forms.Label();
            this.lblMechaStatus6 = new System.Windows.Forms.Label();
            this.lblMechaStatus5 = new System.Windows.Forms.Label();
            this.lblMechaStatus4 = new System.Windows.Forms.Label();
            this.lblMechaStatus3 = new System.Windows.Forms.Label();
            this.lblMechaStatus2 = new System.Windows.Forms.Label();
            this.lblMechaStatus1 = new System.Windows.Forms.Label();
            this.lblMechaStatus0 = new System.Windows.Forms.Label();
            this.cwneEXMErrCode = new System.Windows.Forms.NumericUpDown();
            this.lblEXMErrCodeHeader = new System.Windows.Forms.Label();
            this.fraHighSpeedCamera = new System.Windows.Forms.Panel();
            this.cwbtnChangeMode1 = new CT30K.CWButton();
            this.label1 = new System.Windows.Forms.Label();
            this.cwbtnChangeMode0 = new CT30K.CWButton();
            this.cwbtnChangeDet1 = new CT30K.CWButton();
            this.cwbtnChangeDet0 = new CT30K.CWButton();
            this.fraAutoScanPos = new System.Windows.Forms.GroupBox();
            this.cmdFromExObsCam = new System.Windows.Forms.Button();
            this.cmdFromSlice = new System.Windows.Forms.Button();
            this.cmdFromTrans = new System.Windows.Forms.Button();
            this.fraMechaScanCondiion = new System.Windows.Forms.GroupBox();
            this.lblPixSizeNum = new System.Windows.Forms.Label();
            this.lblScanAreaNum = new System.Windows.Forms.Label();
            this.lblSelectedScanMode = new System.Windows.Forms.Label();
            this.lblUnit2 = new System.Windows.Forms.Label();
            this.lblUnit = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblColon = new System.Windows.Forms.Label();
            this.lblPixSize = new System.Windows.Forms.Label();
            this.lblScanArea = new System.Windows.Forms.Label();
            this.lblScanMode = new System.Windows.Forms.Label();
            this.fraScanArea = new System.Windows.Forms.GroupBox();
            this.ntbShift = new CT30K.NumTextBox();
            this.ntbOffset = new CT30K.NumTextBox();
            this.ntbFullHalf = new CT30K.NumTextBox();
            this.fraUpDown = new System.Windows.Forms.Panel();
            this.lblMin = new System.Windows.Forms.Label();
            this.lblMiddle = new System.Windows.Forms.Label();
            this.lblMax = new System.Windows.Forms.Label();
            this.cwbtnUpDown0 = new CT30K.CWButton();
            this.cwbtnUpDown1 = new CT30K.CWButton();
            this.ctSliderVScroll1 = new CT30K.CTSliderVScroll();
            this.cwnePos = new System.Windows.Forms.NumericUpDown();
            this.txtUpDownPos = new System.Windows.Forms.TextBox();
            this.lblUP = new System.Windows.Forms.Label();
            this.lblDOWN = new System.Windows.Forms.Label();
            this.cmdPosExec = new System.Windows.Forms.Button();
            this.Label22 = new System.Windows.Forms.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineMax = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineMiddle = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineMin = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.fraMechaControl = new System.Windows.Forms.Panel();
            this.cwbtnFineTable0 = new CT30K.CWButton();
            this.cwbtnFineTable1 = new CT30K.CWButton();
            this.cwbtnMove1 = new CT30K.CWButton();
            this.cwbtnMove0 = new CT30K.CWButton();
            this.cwbtnRotate1 = new CT30K.CWButton();
            this.cwbtnRotate0 = new CT30K.CWButton();
            this.cwbtnMove3 = new CT30K.CWButton();
            this.cwbtnMove2 = new CT30K.CWButton();
            this.fraIIMove = new System.Windows.Forms.Panel();
            this.lblIIMove4 = new System.Windows.Forms.Label();
            this.ctchkIIMove4 = new CT30K.CTCheckBox();
            this.lblIIMove3 = new System.Windows.Forms.Label();
            this.lblIIMove2 = new System.Windows.Forms.Label();
            this.lblIIMove1 = new System.Windows.Forms.Label();
            this.lblIIMove0 = new System.Windows.Forms.Label();
            this.ctchkIIMove3 = new CT30K.CTCheckBox();
            this.ctchkIIMove2 = new CT30K.CTCheckBox();
            this.ctchkIIMove1 = new CT30K.CTCheckBox();
            this.ctchkIIMove0 = new CT30K.CTCheckBox();
            this.lblXrayII = new System.Windows.Forms.Label();
            this.cwbtnDetShift = new CT30K.CWButton();
            this.pnlDetShift = new System.Windows.Forms.Panel();
            this.lblFTable = new System.Windows.Forms.Label();
            this.lblRot1 = new System.Windows.Forms.Label();
            this.lblRot3 = new System.Windows.Forms.Label();
            this.lblRot2 = new System.Windows.Forms.Label();
            this.lblRot0 = new System.Windows.Forms.Label();
            this.ctchkRotate2 = new CT30K.CTCheckBox();
            this.ctchkRotate1 = new CT30K.CTCheckBox();
            this.ctchkRotate3 = new CT30K.CTCheckBox();
            this.ctchkRotate0 = new CT30K.CTCheckBox();
            this.lblXrayTube = new System.Windows.Forms.Label();
            this.ImgXrayTube = new System.Windows.Forms.PictureBox();
            this.fraTableRestriction = new System.Windows.Forms.GroupBox();
            this.cmdTableMovePermit = new System.Windows.Forms.Button();
            this.cmdTableMoveRestrict = new System.Windows.Forms.Button();
            this.shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.ShpFTable = new Microsoft.VisualBasic.PowerPacks.OvalShape();
            this.ShpTable = new Microsoft.VisualBasic.PowerPacks.OvalShape();
            this.ShpCabinet = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.fraChangeDetector = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.tmrMecainfSeqComm = new System.Windows.Forms.Timer(this.components);
            this.fraTiltAndRot = new System.Windows.Forms.GroupBox();
            this.cwbtnTiltAndRot_Rot0 = new CT30K.CWButton();
            this.cwbtnTiltAndRot_Rot1 = new CT30K.CWButton();
            this.cwbtnTiltAndRot_Tilt1 = new CT30K.CWButton();
            this.cwbtnTiltAndRot_Tilt0 = new CT30K.CWButton();
            this.lblTilt = new System.Windows.Forms.Label();
            this.lblRot = new System.Windows.Forms.Label();
            this.ntbRotate = new CT30K.NumTextBox();
            this.ntbUpDown = new CT30K.NumTextBox();
            this.cboSpeed0 = new System.Windows.Forms.ComboBox();
            this.cboSpeed1 = new System.Windows.Forms.ComboBox();
            this.cboSpeed2 = new System.Windows.Forms.ComboBox();
            this.cboSpeed6 = new System.Windows.Forms.ComboBox();
            this.cboSpeed3 = new System.Windows.Forms.ComboBox();
            this.cboSpeed4 = new System.Windows.Forms.ComboBox();
            this.cboSpeed7 = new System.Windows.Forms.ComboBox();
            this.cboSpeed8 = new System.Windows.Forms.ComboBox();
            this.cboSpeed9 = new System.Windows.Forms.ComboBox();
            this.ntbFCD = new CT30K.NumTextBox();
            this.ntbFID = new CT30K.NumTextBox();
            this.ntbTableXPos = new CT30K.NumTextBox();
            this.ntbFTablePosX = new CT30K.NumTextBox();
            this.ntbFTablePosY = new CT30K.NumTextBox();
            this.ntbXrayRotPos = new CT30K.NumTextBox();
            this.ntbXrayPosX = new CT30K.NumTextBox();
            this.ntbXrayPosY = new CT30K.NumTextBox();
            this.ntbTiltRot = new CT30K.NumTextBox();
            this.cboSpeed10 = new System.Windows.Forms.ComboBox();
            this.cboSpeed11 = new System.Windows.Forms.ComboBox();
            this.ntbTilt = new CT30K.NumTextBox();
            this.fraMechaPos = new System.Windows.Forms.Panel();
            this.fraChangeXray = new System.Windows.Forms.Panel();
            this.cwbtnChangeXray1 = new CT30K.CWButton();
            this.lblChangeXray = new System.Windows.Forms.Label();
            this.cwbtnChangeXray0 = new CT30K.CWButton();
            this.fraIris.SuspendLayout();
            this.fraXrayRotate.SuspendLayout();
            this.fraCollimator.SuspendLayout();
            this.fraError.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneEXMErrCode)).BeginInit();
            this.fraHighSpeedCamera.SuspendLayout();
            this.fraAutoScanPos.SuspendLayout();
            this.fraMechaScanCondiion.SuspendLayout();
            this.fraScanArea.SuspendLayout();
            this.fraUpDown.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwnePos)).BeginInit();
            this.fraMechaControl.SuspendLayout();
            this.fraIIMove.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImgXrayTube)).BeginInit();
            this.fraTableRestriction.SuspendLayout();
            this.fraChangeDetector.SuspendLayout();
            this.fraTiltAndRot.SuspendLayout();
            this.fraMechaPos.SuspendLayout();
            this.fraChangeXray.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrTryReloadForms
            // 
            this.tmrTryReloadForms.Interval = 2000;
            this.tmrTryReloadForms.Tick += new System.EventHandler(this.tmrTryReloadForms_Tick);
            // 
            // tmrMecainf
            // 
            this.tmrMecainf.Interval = 1000;
            // 
            // tmrSeqComm
            // 
            this.tmrSeqComm.Interval = 500;
            // 
            // tmrPIOCheck
            // 
            this.tmrPIOCheck.Tick += new System.EventHandler(this.tmrPIOCheck_Timer);
            // 
            // ImageList1
            // 
            this.ImageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList1.ImageStream")));
            this.ImageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList1.Images.SetKeyName(0, "DownLimit");
            this.ImageList1.Images.SetKeyName(1, "UpLimit");
            this.ImageList1.Images.SetKeyName(2, "LeftLimit");
            this.ImageList1.Images.SetKeyName(3, "RightLimit");
            this.ImageList1.Images.SetKeyName(4, "Down");
            this.ImageList1.Images.SetKeyName(5, "Up");
            this.ImageList1.Images.SetKeyName(6, "Left");
            this.ImageList1.Images.SetKeyName(7, "Right");
            this.ImageList1.Images.SetKeyName(8, "XrayTubeOFF");
            this.ImageList1.Images.SetKeyName(9, "XrayTubeON");
            // 
            // cmdDetails
            // 
            this.cmdDetails.Location = new System.Drawing.Point(720, 288);
            this.cmdDetails.Name = "cmdDetails";
            this.cmdDetails.Size = new System.Drawing.Size(81, 29);
            this.cmdDetails.TabIndex = 19;
            this.cmdDetails.Tag = "10033";
            this.cmdDetails.Text = "#詳細...";
            this.cmdDetails.UseVisualStyleBackColor = true;
            this.cmdDetails.Click += new System.EventHandler(this.cmdDetails_Click);
            // 
            // cmdMechaAllReset
            // 
            this.cmdMechaAllReset.Location = new System.Drawing.Point(720, 256);
            this.cmdMechaAllReset.Name = "cmdMechaAllReset";
            this.cmdMechaAllReset.Size = new System.Drawing.Size(81, 29);
            this.cmdMechaAllReset.TabIndex = 39;
            this.cmdMechaAllReset.Tag = "20162";
            this.cmdMechaAllReset.Text = "メカリセット";
            this.cmdMechaAllReset.UseVisualStyleBackColor = true;
            this.cmdMechaAllReset.Visible = false;
            this.cmdMechaAllReset.Click += new System.EventHandler(this.cmdMechaAllReset_Click);
            // 
            // fraIris
            // 
            this.fraIris.Controls.Add(this.cmdIris0);
            this.fraIris.Controls.Add(this.cmdIris5);
            this.fraIris.Controls.Add(this.cmdIris2);
            this.fraIris.Controls.Add(this.cmdIris7);
            this.fraIris.Controls.Add(this.cmdIris6);
            this.fraIris.Controls.Add(this.cmdIris1);
            this.fraIris.Controls.Add(this.cmdIris4);
            this.fraIris.Controls.Add(this.cmdIris3);
            this.fraIris.Location = new System.Drawing.Point(688, 320);
            this.fraIris.Name = "fraIris";
            this.fraIris.Size = new System.Drawing.Size(142, 137);
            this.fraIris.TabIndex = 7;
            this.fraIris.TabStop = false;
            this.fraIris.Text = "#Ｘ線Ｉ．Ｉ．絞り";
            this.fraIris.Visible = false;
            // 
            // cmdIris0
            // 
            this.cmdIris0.Location = new System.Drawing.Point(4, 64);
            this.cmdIris0.Margin = new System.Windows.Forms.Padding(0);
            this.cmdIris0.Name = "cmdIris0";
            this.cmdIris0.Size = new System.Drawing.Size(32, 21);
            this.cmdIris0.TabIndex = 33;
            this.cmdIris0.Tag = "IrisLOpen";
            this.cmdIris0.Text = "開";
            this.cmdIris0.UseVisualStyleBackColor = true;
            this.cmdIris0.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmdIris_MouseDown);
            this.cmdIris0.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cmdIris_MouseUp);
            // 
            // cmdIris5
            // 
            this.cmdIris5.Location = new System.Drawing.Point(54, 34);
            this.cmdIris5.Margin = new System.Windows.Forms.Padding(0);
            this.cmdIris5.Name = "cmdIris5";
            this.cmdIris5.Size = new System.Drawing.Size(32, 21);
            this.cmdIris5.TabIndex = 32;
            this.cmdIris5.Tag = "IrisUClose";
            this.cmdIris5.Text = "閉";
            this.cmdIris5.UseVisualStyleBackColor = true;
            this.cmdIris5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmdIris_MouseDown);
            this.cmdIris5.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cmdIris_MouseUp);
            // 
            // cmdIris2
            // 
            this.cmdIris2.Location = new System.Drawing.Point(105, 64);
            this.cmdIris2.Margin = new System.Windows.Forms.Padding(0);
            this.cmdIris2.Name = "cmdIris2";
            this.cmdIris2.Size = new System.Drawing.Size(32, 21);
            this.cmdIris2.TabIndex = 35;
            this.cmdIris2.Tag = "IrisROpen";
            this.cmdIris2.Text = "開";
            this.cmdIris2.UseVisualStyleBackColor = true;
            this.cmdIris2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmdIris_MouseDown);
            this.cmdIris2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cmdIris_MouseUp);
            // 
            // cmdIris7
            // 
            this.cmdIris7.Location = new System.Drawing.Point(54, 91);
            this.cmdIris7.Margin = new System.Windows.Forms.Padding(0);
            this.cmdIris7.Name = "cmdIris7";
            this.cmdIris7.Size = new System.Drawing.Size(32, 21);
            this.cmdIris7.TabIndex = 36;
            this.cmdIris7.Tag = "IrisDClose";
            this.cmdIris7.Text = "閉";
            this.cmdIris7.UseVisualStyleBackColor = true;
            this.cmdIris7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmdIris_MouseDown);
            this.cmdIris7.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cmdIris_MouseUp);
            // 
            // cmdIris6
            // 
            this.cmdIris6.Location = new System.Drawing.Point(54, 111);
            this.cmdIris6.Margin = new System.Windows.Forms.Padding(0);
            this.cmdIris6.Name = "cmdIris6";
            this.cmdIris6.Size = new System.Drawing.Size(32, 21);
            this.cmdIris6.TabIndex = 37;
            this.cmdIris6.Tag = "IrisDOpen";
            this.cmdIris6.Text = "開";
            this.cmdIris6.UseVisualStyleBackColor = true;
            this.cmdIris6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmdIris_MouseDown);
            this.cmdIris6.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cmdIris_MouseUp);
            // 
            // cmdIris1
            // 
            this.cmdIris1.Location = new System.Drawing.Point(35, 64);
            this.cmdIris1.Margin = new System.Windows.Forms.Padding(0);
            this.cmdIris1.Name = "cmdIris1";
            this.cmdIris1.Size = new System.Drawing.Size(32, 21);
            this.cmdIris1.TabIndex = 30;
            this.cmdIris1.Tag = "IrisLClose";
            this.cmdIris1.Text = "閉";
            this.cmdIris1.UseVisualStyleBackColor = true;
            this.cmdIris1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmdIris_MouseDown);
            this.cmdIris1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cmdIris_MouseUp);
            // 
            // cmdIris4
            // 
            this.cmdIris4.Location = new System.Drawing.Point(54, 14);
            this.cmdIris4.Margin = new System.Windows.Forms.Padding(0);
            this.cmdIris4.Name = "cmdIris4";
            this.cmdIris4.Size = new System.Drawing.Size(32, 21);
            this.cmdIris4.TabIndex = 31;
            this.cmdIris4.Tag = "IrisUOpen";
            this.cmdIris4.Text = "開";
            this.cmdIris4.UseVisualStyleBackColor = true;
            this.cmdIris4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmdIris_MouseDown);
            this.cmdIris4.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cmdIris_MouseUp);
            // 
            // cmdIris3
            // 
            this.cmdIris3.Location = new System.Drawing.Point(74, 64);
            this.cmdIris3.Margin = new System.Windows.Forms.Padding(0);
            this.cmdIris3.Name = "cmdIris3";
            this.cmdIris3.Size = new System.Drawing.Size(32, 21);
            this.cmdIris3.TabIndex = 34;
            this.cmdIris3.Tag = "IrisRClose";
            this.cmdIris3.Text = "閉";
            this.cmdIris3.UseVisualStyleBackColor = true;
            this.cmdIris3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmdIris_MouseDown);
            this.cmdIris3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cmdIris_MouseUp);
            // 
            // fraXrayRotate
            // 
            this.fraXrayRotate.Controls.Add(this.cwbtnMove6);
            this.fraXrayRotate.Controls.Add(this.cwbtnMove7);
            this.fraXrayRotate.Controls.Add(this.cwbtnMove8);
            this.fraXrayRotate.Controls.Add(this.cwbtnMove9);
            this.fraXrayRotate.Controls.Add(this.cwbtnRotateXray1);
            this.fraXrayRotate.Controls.Add(this.cwbtnRotateXray0);
            this.fraXrayRotate.Location = new System.Drawing.Point(688, 144);
            this.fraXrayRotate.Name = "fraXrayRotate";
            this.fraXrayRotate.Size = new System.Drawing.Size(142, 113);
            this.fraXrayRotate.TabIndex = 6;
            this.fraXrayRotate.TabStop = false;
            this.fraXrayRotate.Tag = "12217";
            this.fraXrayRotate.Text = "#Ｘ線管操作";
            // 
            // cwbtnMove6
            // 
            this.cwbtnMove6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnMove6.BlinkInterval = CT30K.CWSpeeds.cwSpeedFastest;
            this.cwbtnMove6.Caption = "";
            this.cwbtnMove6.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnMove6.FlatAppearanceBorderSize = 1;
            this.cwbtnMove6.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnMove6.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnMove6.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnMove6.IsIgnorePerformClick = false;
            this.cwbtnMove6.Location = new System.Drawing.Point(53, 78);
            this.cwbtnMove6.Name = "cwbtnMove6";
            this.cwbtnMove6.OffImage = null;
            this.cwbtnMove6.OnColor = System.Drawing.Color.Empty;
            this.cwbtnMove6.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnMove6.OnImage")));
            this.cwbtnMove6.Size = new System.Drawing.Size(32, 32);
            this.cwbtnMove6.TabIndex = 19;
            this.cwbtnMove6.Tag = "XrayYBackward";
            this.cwbtnMove6.Value = false;
            // 
            // cwbtnMove7
            // 
            this.cwbtnMove7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnMove7.BlinkInterval = CT30K.CWSpeeds.cwSpeedFastest;
            this.cwbtnMove7.Caption = "";
            this.cwbtnMove7.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnMove7.FlatAppearanceBorderSize = 1;
            this.cwbtnMove7.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnMove7.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnMove7.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnMove7.IsIgnorePerformClick = false;
            this.cwbtnMove7.Location = new System.Drawing.Point(53, 12);
            this.cwbtnMove7.Name = "cwbtnMove7";
            this.cwbtnMove7.OffImage = null;
            this.cwbtnMove7.OnColor = System.Drawing.Color.Empty;
            this.cwbtnMove7.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnMove7.OnImage")));
            this.cwbtnMove7.Size = new System.Drawing.Size(32, 32);
            this.cwbtnMove7.TabIndex = 13;
            this.cwbtnMove7.Tag = "XrayXRight";
            this.cwbtnMove7.Value = false;
            this.cwbtnMove7.ValueChanged += new System.EventHandler(this.cwbtnMove_ValueChanged);
            this.cwbtnMove7.MouseCaptureChanged += new System.EventHandler(this.cwbtnMove_MouseCaptureChanged);
            // 
            // cwbtnMove8
            // 
            this.cwbtnMove8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnMove8.BlinkInterval = CT30K.CWSpeeds.cwSpeedFastest;
            this.cwbtnMove8.Caption = "";
            this.cwbtnMove8.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnMove8.FlatAppearanceBorderSize = 1;
            this.cwbtnMove8.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnMove8.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnMove8.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnMove8.IsIgnorePerformClick = false;
            this.cwbtnMove8.Location = new System.Drawing.Point(18, 44);
            this.cwbtnMove8.Name = "cwbtnMove8";
            this.cwbtnMove8.OffImage = null;
            this.cwbtnMove8.OnColor = System.Drawing.Color.Empty;
            this.cwbtnMove8.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnMove8.OnImage")));
            this.cwbtnMove8.Size = new System.Drawing.Size(32, 32);
            this.cwbtnMove8.TabIndex = 14;
            this.cwbtnMove8.Tag = "XrayYForward";
            this.cwbtnMove8.Value = false;
            this.cwbtnMove8.ValueChanged += new System.EventHandler(this.cwbtnMove_ValueChanged);
            this.cwbtnMove8.MouseCaptureChanged += new System.EventHandler(this.cwbtnMove_MouseCaptureChanged);
            // 
            // cwbtnMove9
            // 
            this.cwbtnMove9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnMove9.BlinkInterval = CT30K.CWSpeeds.cwSpeedFastest;
            this.cwbtnMove9.Caption = "";
            this.cwbtnMove9.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnMove9.FlatAppearanceBorderSize = 1;
            this.cwbtnMove9.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnMove9.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnMove9.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnMove9.IsIgnorePerformClick = false;
            this.cwbtnMove9.Location = new System.Drawing.Point(87, 44);
            this.cwbtnMove9.Name = "cwbtnMove9";
            this.cwbtnMove9.OffImage = null;
            this.cwbtnMove9.OnColor = System.Drawing.Color.Empty;
            this.cwbtnMove9.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnMove9.OnImage")));
            this.cwbtnMove9.Size = new System.Drawing.Size(32, 32);
            this.cwbtnMove9.TabIndex = 15;
            this.cwbtnMove9.Tag = "XrayYBackward";
            this.cwbtnMove9.Value = false;
            this.cwbtnMove9.ValueChanged += new System.EventHandler(this.cwbtnMove_ValueChanged);
            this.cwbtnMove9.MouseCaptureChanged += new System.EventHandler(this.cwbtnMove_MouseCaptureChanged);
            // 
            // cwbtnRotateXray1
            // 
            this.cwbtnRotateXray1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnRotateXray1.BlinkInterval = CT30K.CWSpeeds.cwSpeedFastest;
            this.cwbtnRotateXray1.Caption = "";
            this.cwbtnRotateXray1.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnRotateXray1.FlatAppearanceBorderSize = 1;
            this.cwbtnRotateXray1.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnRotateXray1.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnRotateXray1.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnRotateXray1.IsIgnorePerformClick = false;
            this.cwbtnRotateXray1.Location = new System.Drawing.Point(91, 80);
            this.cwbtnRotateXray1.Name = "cwbtnRotateXray1";
            this.cwbtnRotateXray1.OffImage = null;
            this.cwbtnRotateXray1.OnColor = System.Drawing.Color.Empty;
            this.cwbtnRotateXray1.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnRotateXray1.OnImage")));
            this.cwbtnRotateXray1.Size = new System.Drawing.Size(29, 29);
            this.cwbtnRotateXray1.TabIndex = 18;
            this.cwbtnRotateXray1.Tag = "XRAYROTCCW";
            this.cwbtnRotateXray1.Value = false;
            this.cwbtnRotateXray1.ValueChanged += new System.EventHandler(this.cwbtnRotateXray_ValueChanged);
            this.cwbtnRotateXray1.MouseCaptureChanged += new System.EventHandler(this.cwbtnRotateXray_MouseCaptureChanged);
            // 
            // cwbtnRotateXray0
            // 
            this.cwbtnRotateXray0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnRotateXray0.BlinkInterval = CT30K.CWSpeeds.cwSpeedFastest;
            this.cwbtnRotateXray0.Caption = "";
            this.cwbtnRotateXray0.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnRotateXray0.FlatAppearanceBorderSize = 1;
            this.cwbtnRotateXray0.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnRotateXray0.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnRotateXray0.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnRotateXray0.IsIgnorePerformClick = false;
            this.cwbtnRotateXray0.Location = new System.Drawing.Point(18, 80);
            this.cwbtnRotateXray0.Name = "cwbtnRotateXray0";
            this.cwbtnRotateXray0.OffImage = null;
            this.cwbtnRotateXray0.OnColor = System.Drawing.Color.Empty;
            this.cwbtnRotateXray0.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnRotateXray0.OnImage")));
            this.cwbtnRotateXray0.Size = new System.Drawing.Size(29, 29);
            this.cwbtnRotateXray0.TabIndex = 16;
            this.cwbtnRotateXray0.Tag = "XRAYROTCW";
            this.cwbtnRotateXray0.Value = false;
            this.cwbtnRotateXray0.ValueChanged += new System.EventHandler(this.cwbtnRotateXray_ValueChanged);
            this.cwbtnRotateXray0.MouseCaptureChanged += new System.EventHandler(this.cwbtnRotateXray_MouseCaptureChanged);
            // 
            // fraCollimator
            // 
            this.fraCollimator.Controls.Add(this.cmdCollimator0);
            this.fraCollimator.Controls.Add(this.cmdCollimator2);
            this.fraCollimator.Controls.Add(this.cmdCollimator6);
            this.fraCollimator.Controls.Add(this.cmdCollimator4);
            this.fraCollimator.Controls.Add(this.cmdCollimator3);
            this.fraCollimator.Controls.Add(this.cmdCollimator1);
            this.fraCollimator.Controls.Add(this.cmdCollimator7);
            this.fraCollimator.Controls.Add(this.cmdCollimator5);
            this.fraCollimator.Location = new System.Drawing.Point(688, 0);
            this.fraCollimator.Name = "fraCollimator";
            this.fraCollimator.Size = new System.Drawing.Size(142, 137);
            this.fraCollimator.TabIndex = 5;
            this.fraCollimator.TabStop = false;
            this.fraCollimator.Tag = "12157";
            this.fraCollimator.Text = "#コリメータ";
            // 
            // cmdCollimator0
            // 
            this.cmdCollimator0.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.cmdCollimator0.Location = new System.Drawing.Point(4, 64);
            this.cmdCollimator0.Margin = new System.Windows.Forms.Padding(0);
            this.cmdCollimator0.Name = "cmdCollimator0";
            this.cmdCollimator0.Size = new System.Drawing.Size(32, 21);
            this.cmdCollimator0.TabIndex = 7;
            this.cmdCollimator0.Tag = "ColliLOpen";
            this.cmdCollimator0.Text = "開";
            this.cmdCollimator0.UseVisualStyleBackColor = true;
            this.cmdCollimator0.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmdCollimator_MouseDown);
            this.cmdCollimator0.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cmdCollimator_MouseUp);
            // 
            // cmdCollimator2
            // 
            this.cmdCollimator2.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.cmdCollimator2.Location = new System.Drawing.Point(105, 64);
            this.cmdCollimator2.Margin = new System.Windows.Forms.Padding(0);
            this.cmdCollimator2.Name = "cmdCollimator2";
            this.cmdCollimator2.Size = new System.Drawing.Size(32, 21);
            this.cmdCollimator2.TabIndex = 10;
            this.cmdCollimator2.Tag = "ColliROpen";
            this.cmdCollimator2.Text = "開";
            this.cmdCollimator2.UseVisualStyleBackColor = true;
            this.cmdCollimator2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmdCollimator_MouseDown);
            this.cmdCollimator2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cmdCollimator_MouseUp);
            // 
            // cmdCollimator6
            // 
            this.cmdCollimator6.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.cmdCollimator6.Location = new System.Drawing.Point(54, 111);
            this.cmdCollimator6.Margin = new System.Windows.Forms.Padding(0);
            this.cmdCollimator6.Name = "cmdCollimator6";
            this.cmdCollimator6.Size = new System.Drawing.Size(32, 21);
            this.cmdCollimator6.TabIndex = 6;
            this.cmdCollimator6.Tag = "ColliDOpen";
            this.cmdCollimator6.Text = "開";
            this.cmdCollimator6.UseVisualStyleBackColor = true;
            this.cmdCollimator6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmdCollimator_MouseDown);
            this.cmdCollimator6.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cmdCollimator_MouseUp);
            // 
            // cmdCollimator4
            // 
            this.cmdCollimator4.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdCollimator4.Location = new System.Drawing.Point(54, 14);
            this.cmdCollimator4.Margin = new System.Windows.Forms.Padding(0);
            this.cmdCollimator4.Name = "cmdCollimator4";
            this.cmdCollimator4.Size = new System.Drawing.Size(32, 21);
            this.cmdCollimator4.TabIndex = 3;
            this.cmdCollimator4.Tag = "ColliUOpen";
            this.cmdCollimator4.Text = "開";
            this.cmdCollimator4.UseVisualStyleBackColor = true;
            this.cmdCollimator4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmdCollimator_MouseDown);
            this.cmdCollimator4.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cmdCollimator_MouseUp);
            // 
            // cmdCollimator3
            // 
            this.cmdCollimator3.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.cmdCollimator3.Location = new System.Drawing.Point(74, 64);
            this.cmdCollimator3.Margin = new System.Windows.Forms.Padding(0);
            this.cmdCollimator3.Name = "cmdCollimator3";
            this.cmdCollimator3.Size = new System.Drawing.Size(32, 21);
            this.cmdCollimator3.TabIndex = 9;
            this.cmdCollimator3.Tag = "ColliRClose";
            this.cmdCollimator3.Text = "閉";
            this.cmdCollimator3.UseVisualStyleBackColor = true;
            this.cmdCollimator3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmdCollimator_MouseDown);
            this.cmdCollimator3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cmdCollimator_MouseUp);
            // 
            // cmdCollimator1
            // 
            this.cmdCollimator1.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.cmdCollimator1.Location = new System.Drawing.Point(35, 64);
            this.cmdCollimator1.Margin = new System.Windows.Forms.Padding(0);
            this.cmdCollimator1.Name = "cmdCollimator1";
            this.cmdCollimator1.Size = new System.Drawing.Size(32, 21);
            this.cmdCollimator1.TabIndex = 8;
            this.cmdCollimator1.Tag = "ColliLClose";
            this.cmdCollimator1.Text = "閉";
            this.cmdCollimator1.UseVisualStyleBackColor = true;
            this.cmdCollimator1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmdCollimator_MouseDown);
            this.cmdCollimator1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cmdCollimator_MouseUp);
            // 
            // cmdCollimator7
            // 
            this.cmdCollimator7.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.cmdCollimator7.Location = new System.Drawing.Point(54, 91);
            this.cmdCollimator7.Margin = new System.Windows.Forms.Padding(0);
            this.cmdCollimator7.Name = "cmdCollimator7";
            this.cmdCollimator7.Size = new System.Drawing.Size(32, 21);
            this.cmdCollimator7.TabIndex = 5;
            this.cmdCollimator7.Tag = "ColliDClose";
            this.cmdCollimator7.Text = "閉";
            this.cmdCollimator7.UseVisualStyleBackColor = true;
            this.cmdCollimator7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmdCollimator_MouseDown);
            this.cmdCollimator7.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cmdCollimator_MouseUp);
            // 
            // cmdCollimator5
            // 
            this.cmdCollimator5.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.cmdCollimator5.Location = new System.Drawing.Point(54, 34);
            this.cmdCollimator5.Margin = new System.Windows.Forms.Padding(0);
            this.cmdCollimator5.Name = "cmdCollimator5";
            this.cmdCollimator5.Size = new System.Drawing.Size(32, 21);
            this.cmdCollimator5.TabIndex = 4;
            this.cmdCollimator5.Tag = "ColliUClose";
            this.cmdCollimator5.Text = "閉";
            this.cmdCollimator5.UseVisualStyleBackColor = true;
            this.cmdCollimator5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmdCollimator_MouseDown);
            this.cmdCollimator5.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cmdCollimator_MouseUp);
            // 
            // fraError
            // 
            this.fraError.Controls.Add(this.lblMechaStatus7);
            this.fraError.Controls.Add(this.lblMechaStatus6);
            this.fraError.Controls.Add(this.lblMechaStatus5);
            this.fraError.Controls.Add(this.lblMechaStatus4);
            this.fraError.Controls.Add(this.lblMechaStatus3);
            this.fraError.Controls.Add(this.lblMechaStatus2);
            this.fraError.Controls.Add(this.lblMechaStatus1);
            this.fraError.Controls.Add(this.lblMechaStatus0);
            this.fraError.Controls.Add(this.cwneEXMErrCode);
            this.fraError.Controls.Add(this.lblEXMErrCodeHeader);
            this.fraError.Location = new System.Drawing.Point(0, 356);
            this.fraError.Name = "fraError";
            this.fraError.Size = new System.Drawing.Size(561, 141);
            this.fraError.TabIndex = 3;
            this.fraError.TabStop = false;
            this.fraError.Text = "エラーステータス・非表示";
            // 
            // lblMechaStatus7
            // 
            this.lblMechaStatus7.AutoSize = true;
            this.lblMechaStatus7.Location = new System.Drawing.Point(214, 40);
            this.lblMechaStatus7.Name = "lblMechaStatus7";
            this.lblMechaStatus7.Size = new System.Drawing.Size(71, 12);
            this.lblMechaStatus7.TabIndex = 46;
            this.lblMechaStatus7.Tag = "stsTiltRotErr";
            this.lblMechaStatus7.Text = "stsTiltRotErr";
            this.lblMechaStatus7.TextChanged += new System.EventHandler(this.lblMechaStatus_TextChanged);
            // 
            // lblMechaStatus6
            // 
            this.lblMechaStatus6.AutoSize = true;
            this.lblMechaStatus6.Location = new System.Drawing.Point(213, 20);
            this.lblMechaStatus6.Name = "lblMechaStatus6";
            this.lblMechaStatus6.Size = new System.Drawing.Size(53, 12);
            this.lblMechaStatus6.TabIndex = 45;
            this.lblMechaStatus6.Tag = "stsTiltErr";
            this.lblMechaStatus6.Text = "stsTiltErr";
            this.lblMechaStatus6.TextChanged += new System.EventHandler(this.lblMechaStatus_TextChanged);
            // 
            // lblMechaStatus5
            // 
            this.lblMechaStatus5.AutoSize = true;
            this.lblMechaStatus5.Location = new System.Drawing.Point(280, 120);
            this.lblMechaStatus5.Name = "lblMechaStatus5";
            this.lblMechaStatus5.Size = new System.Drawing.Size(60, 12);
            this.lblMechaStatus5.TabIndex = 23;
            this.lblMechaStatus5.Tag = "stsYStgErr";
            this.lblMechaStatus5.Text = "stsYStgErr";
            this.lblMechaStatus5.TextChanged += new System.EventHandler(this.lblMechaStatus_TextChanged);
            // 
            // lblMechaStatus4
            // 
            this.lblMechaStatus4.AutoSize = true;
            this.lblMechaStatus4.Location = new System.Drawing.Point(280, 100);
            this.lblMechaStatus4.Name = "lblMechaStatus4";
            this.lblMechaStatus4.Size = new System.Drawing.Size(60, 12);
            this.lblMechaStatus4.TabIndex = 24;
            this.lblMechaStatus4.Tag = "stsXStgErr";
            this.lblMechaStatus4.Text = "stsXStgErr";
            this.lblMechaStatus4.TextChanged += new System.EventHandler(this.lblMechaStatus_TextChanged);
            // 
            // lblMechaStatus3
            // 
            this.lblMechaStatus3.AutoSize = true;
            this.lblMechaStatus3.Location = new System.Drawing.Point(280, 80);
            this.lblMechaStatus3.Name = "lblMechaStatus3";
            this.lblMechaStatus3.Size = new System.Drawing.Size(58, 12);
            this.lblMechaStatus3.TabIndex = 25;
            this.lblMechaStatus3.Tag = "stsPhmErr";
            this.lblMechaStatus3.Text = "stsPhmErr";
            this.lblMechaStatus3.TextChanged += new System.EventHandler(this.lblMechaStatus_TextChanged);
            // 
            // lblMechaStatus2
            // 
            this.lblMechaStatus2.AutoSize = true;
            this.lblMechaStatus2.Location = new System.Drawing.Point(280, 60);
            this.lblMechaStatus2.Name = "lblMechaStatus2";
            this.lblMechaStatus2.Size = new System.Drawing.Size(52, 12);
            this.lblMechaStatus2.TabIndex = 27;
            this.lblMechaStatus2.Tag = "stsUDErr";
            this.lblMechaStatus2.Text = "stsUDErr";
            this.lblMechaStatus2.TextChanged += new System.EventHandler(this.lblMechaStatus_TextChanged);
            // 
            // lblMechaStatus1
            // 
            this.lblMechaStatus1.AutoSize = true;
            this.lblMechaStatus1.Location = new System.Drawing.Point(280, 40);
            this.lblMechaStatus1.Name = "lblMechaStatus1";
            this.lblMechaStatus1.Size = new System.Drawing.Size(54, 12);
            this.lblMechaStatus1.TabIndex = 28;
            this.lblMechaStatus1.Tag = "stsRotErr";
            this.lblMechaStatus1.Text = "stsRotErr";
            this.lblMechaStatus1.TextChanged += new System.EventHandler(this.lblMechaStatus_TextChanged);
            // 
            // lblMechaStatus0
            // 
            this.lblMechaStatus0.AutoSize = true;
            this.lblMechaStatus0.Location = new System.Drawing.Point(280, 20);
            this.lblMechaStatus0.Name = "lblMechaStatus0";
            this.lblMechaStatus0.Size = new System.Drawing.Size(73, 12);
            this.lblMechaStatus0.TabIndex = 28;
            this.lblMechaStatus0.Tag = "stsPhmOnOff";
            this.lblMechaStatus0.Text = "stsPhmOnOff";
            this.lblMechaStatus0.TextChanged += new System.EventHandler(this.lblMechaStatus_TextChanged);
            // 
            // cwneEXMErrCode
            // 
            this.cwneEXMErrCode.Location = new System.Drawing.Point(156, 52);
            this.cwneEXMErrCode.Name = "cwneEXMErrCode";
            this.cwneEXMErrCode.Size = new System.Drawing.Size(45, 19);
            this.cwneEXMErrCode.TabIndex = 21;
            this.cwneEXMErrCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneEXMErrCode.ValueChanged += new System.EventHandler(this.cwneEXMErrCode_ValueChanged);
            // 
            // lblEXMErrCodeHeader
            // 
            this.lblEXMErrCodeHeader.AutoSize = true;
            this.lblEXMErrCodeHeader.Location = new System.Drawing.Point(16, 56);
            this.lblEXMErrCodeHeader.Name = "lblEXMErrCodeHeader";
            this.lblEXMErrCodeHeader.Size = new System.Drawing.Size(133, 12);
            this.lblEXMErrCodeHeader.TabIndex = 22;
            this.lblEXMErrCodeHeader.Text = "東芝EXM2-150専用エラー";
            // 
            // fraHighSpeedCamera
            // 
            this.fraHighSpeedCamera.Controls.Add(this.cwbtnChangeMode1);
            this.fraHighSpeedCamera.Controls.Add(this.label1);
            this.fraHighSpeedCamera.Controls.Add(this.cwbtnChangeMode0);
            this.fraHighSpeedCamera.Location = new System.Drawing.Point(0, 503);
            this.fraHighSpeedCamera.Name = "fraHighSpeedCamera";
            this.fraHighSpeedCamera.Size = new System.Drawing.Size(281, 65);
            this.fraHighSpeedCamera.TabIndex = 43;
            // 
            // cwbtnChangeMode1
            // 
            this.cwbtnChangeMode1.BackColor = System.Drawing.Color.Green;
            this.cwbtnChangeMode1.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnChangeMode1.Caption = "#高速撮影";
            this.cwbtnChangeMode1.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnChangeMode1.FlatAppearanceBorderSize = 1;
            this.cwbtnChangeMode1.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnChangeMode1.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnChangeMode1.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnChangeMode1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 15.75F, System.Drawing.FontStyle.Bold);
            this.cwbtnChangeMode1.IsIgnorePerformClick = false;
            this.cwbtnChangeMode1.Location = new System.Drawing.Point(142, 16);
            this.cwbtnChangeMode1.Name = "cwbtnChangeMode1";
            this.cwbtnChangeMode1.OffImage = null;
            this.cwbtnChangeMode1.OnColor = System.Drawing.Color.Lime;
            this.cwbtnChangeMode1.OnImage = null;
            this.cwbtnChangeMode1.Size = new System.Drawing.Size(133, 40);
            this.cwbtnChangeMode1.TabIndex = 30;
            this.cwbtnChangeMode1.Value = false;
            this.cwbtnChangeMode1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cwbtnChangeMode_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 12);
            this.label1.TabIndex = 104;
            this.label1.Text = "#CT⇔高速撮影 切替";
            this.label1.Visible = false;
            // 
            // cwbtnChangeMode0
            // 
            this.cwbtnChangeMode0.BackColor = System.Drawing.Color.Green;
            this.cwbtnChangeMode0.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnChangeMode0.Caption = "#CT";
            this.cwbtnChangeMode0.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnChangeMode0.FlatAppearanceBorderSize = 1;
            this.cwbtnChangeMode0.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnChangeMode0.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnChangeMode0.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnChangeMode0.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwbtnChangeMode0.IsIgnorePerformClick = false;
            this.cwbtnChangeMode0.Location = new System.Drawing.Point(6, 16);
            this.cwbtnChangeMode0.Name = "cwbtnChangeMode0";
            this.cwbtnChangeMode0.OffImage = null;
            this.cwbtnChangeMode0.OnColor = System.Drawing.Color.Lime;
            this.cwbtnChangeMode0.OnImage = null;
            this.cwbtnChangeMode0.Size = new System.Drawing.Size(133, 40);
            this.cwbtnChangeMode0.TabIndex = 29;
            this.cwbtnChangeMode0.Value = false;
            this.cwbtnChangeMode0.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cwbtnChangeMode_Click);
            // 
            // cwbtnChangeDet1
            // 
            this.cwbtnChangeDet1.BackColor = System.Drawing.Color.Green;
            this.cwbtnChangeDet1.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnChangeDet1.Caption = "検出器２";
            this.cwbtnChangeDet1.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnChangeDet1.FlatAppearanceBorderSize = 1;
            this.cwbtnChangeDet1.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnChangeDet1.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnChangeDet1.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnChangeDet1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwbtnChangeDet1.IsIgnorePerformClick = false;
            this.cwbtnChangeDet1.Location = new System.Drawing.Point(152, 16);
            this.cwbtnChangeDet1.Name = "cwbtnChangeDet1";
            this.cwbtnChangeDet1.OffImage = null;
            this.cwbtnChangeDet1.OnColor = System.Drawing.Color.Lime;
            this.cwbtnChangeDet1.OnImage = null;
            this.cwbtnChangeDet1.Size = new System.Drawing.Size(120, 40);
            this.cwbtnChangeDet1.TabIndex = 32;
            this.cwbtnChangeDet1.Value = false;
            this.cwbtnChangeDet1.Click += new System.EventHandler(this.cwbtnChangeDet_Click);
            // 
            // cwbtnChangeDet0
            // 
            this.cwbtnChangeDet0.BackColor = System.Drawing.Color.Green;
            this.cwbtnChangeDet0.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnChangeDet0.Caption = "検出器１";
            this.cwbtnChangeDet0.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnChangeDet0.FlatAppearanceBorderSize = 1;
            this.cwbtnChangeDet0.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnChangeDet0.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnChangeDet0.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnChangeDet0.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwbtnChangeDet0.IsIgnorePerformClick = false;
            this.cwbtnChangeDet0.Location = new System.Drawing.Point(16, 16);
            this.cwbtnChangeDet0.Name = "cwbtnChangeDet0";
            this.cwbtnChangeDet0.OffImage = null;
            this.cwbtnChangeDet0.OnColor = System.Drawing.Color.Lime;
            this.cwbtnChangeDet0.OnImage = null;
            this.cwbtnChangeDet0.Size = new System.Drawing.Size(120, 40);
            this.cwbtnChangeDet0.TabIndex = 31;
            this.cwbtnChangeDet0.Value = false;
            this.cwbtnChangeDet0.Click += new System.EventHandler(this.cwbtnChangeDet_Click);
            // 
            // fraAutoScanPos
            // 
            this.fraAutoScanPos.Controls.Add(this.cmdFromExObsCam);
            this.fraAutoScanPos.Controls.Add(this.cmdFromSlice);
            this.fraAutoScanPos.Controls.Add(this.cmdFromTrans);
            this.fraAutoScanPos.Location = new System.Drawing.Point(3, 301);
            this.fraAutoScanPos.Name = "fraAutoScanPos";
            this.fraAutoScanPos.Size = new System.Drawing.Size(287, 56);
            this.fraAutoScanPos.TabIndex = 1;
            this.fraAutoScanPos.TabStop = false;
            this.fraAutoScanPos.Tag = "15204";
            this.fraAutoScanPos.Text = "#自動スキャン位置指定";
            // 
            // cmdFromExObsCam
            // 
            this.cmdFromExObsCam.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdFromExObsCam.Location = new System.Drawing.Point(195, 18);
            this.cmdFromExObsCam.Name = "cmdFromExObsCam";
            this.cmdFromExObsCam.Size = new System.Drawing.Size(87, 33);
            this.cmdFromExObsCam.TabIndex = 1;
            this.cmdFromExObsCam.Tag = "24010";
            this.cmdFromExObsCam.Text = "#外観";
            this.cmdFromExObsCam.UseVisualStyleBackColor = true;
            this.cmdFromExObsCam.Click += new System.EventHandler(this.cmdFromExObsCam_Click);
            // 
            // cmdFromSlice
            // 
            this.cmdFromSlice.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdFromSlice.Location = new System.Drawing.Point(99, 18);
            this.cmdFromSlice.Name = "cmdFromSlice";
            this.cmdFromSlice.Size = new System.Drawing.Size(87, 33);
            this.cmdFromSlice.TabIndex = 0;
            this.cmdFromSlice.Tag = "15205";
            this.cmdFromSlice.Text = "#断面";
            this.cmdFromSlice.UseVisualStyleBackColor = true;
            this.cmdFromSlice.Click += new System.EventHandler(this.cmdFromSlice_Click);
            // 
            // cmdFromTrans
            // 
            this.cmdFromTrans.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdFromTrans.Location = new System.Drawing.Point(5, 18);
            this.cmdFromTrans.Name = "cmdFromTrans";
            this.cmdFromTrans.Size = new System.Drawing.Size(87, 32);
            this.cmdFromTrans.TabIndex = 0;
            this.cmdFromTrans.Tag = "15206";
            this.cmdFromTrans.Text = "#透視";
            this.cmdFromTrans.UseVisualStyleBackColor = true;
            this.cmdFromTrans.Click += new System.EventHandler(this.cmdFromTrans_Click);
            // 
            // fraMechaScanCondiion
            // 
            this.fraMechaScanCondiion.Controls.Add(this.lblPixSizeNum);
            this.fraMechaScanCondiion.Controls.Add(this.lblScanAreaNum);
            this.fraMechaScanCondiion.Controls.Add(this.lblSelectedScanMode);
            this.fraMechaScanCondiion.Controls.Add(this.lblUnit2);
            this.fraMechaScanCondiion.Controls.Add(this.lblUnit);
            this.fraMechaScanCondiion.Controls.Add(this.label4);
            this.fraMechaScanCondiion.Controls.Add(this.label3);
            this.fraMechaScanCondiion.Controls.Add(this.lblColon);
            this.fraMechaScanCondiion.Controls.Add(this.lblPixSize);
            this.fraMechaScanCondiion.Controls.Add(this.lblScanArea);
            this.fraMechaScanCondiion.Controls.Add(this.lblScanMode);
            this.fraMechaScanCondiion.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fraMechaScanCondiion.Location = new System.Drawing.Point(2, 299);
            this.fraMechaScanCondiion.Name = "fraMechaScanCondiion";
            this.fraMechaScanCondiion.Size = new System.Drawing.Size(270, 97);
            this.fraMechaScanCondiion.TabIndex = 55;
            this.fraMechaScanCondiion.TabStop = false;
            // 
            // lblPixSizeNum
            // 
            this.lblPixSizeNum.AutoSize = true;
            this.lblPixSizeNum.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblPixSizeNum.Location = new System.Drawing.Point(127, 67);
            this.lblPixSizeNum.Name = "lblPixSizeNum";
            this.lblPixSizeNum.Size = new System.Drawing.Size(73, 19);
            this.lblPixSizeNum.TabIndex = 65;
            this.lblPixSizeNum.Text = "0.12345";
            this.lblPixSizeNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblScanAreaNum
            // 
            this.lblScanAreaNum.AutoSize = true;
            this.lblScanAreaNum.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblScanAreaNum.Location = new System.Drawing.Point(127, 42);
            this.lblScanAreaNum.Name = "lblScanAreaNum";
            this.lblScanAreaNum.Size = new System.Drawing.Size(73, 19);
            this.lblScanAreaNum.TabIndex = 64;
            this.lblScanAreaNum.Text = "123.456";
            this.lblScanAreaNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSelectedScanMode
            // 
            this.lblSelectedScanMode.AutoSize = true;
            this.lblSelectedScanMode.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSelectedScanMode.Location = new System.Drawing.Point(127, 19);
            this.lblSelectedScanMode.Name = "lblSelectedScanMode";
            this.lblSelectedScanMode.Size = new System.Drawing.Size(0, 15);
            this.lblSelectedScanMode.TabIndex = 63;
            this.lblSelectedScanMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUnit2
            // 
            this.lblUnit2.AutoSize = true;
            this.lblUnit2.Location = new System.Drawing.Point(235, 71);
            this.lblUnit2.Name = "lblUnit2";
            this.lblUnit2.Size = new System.Drawing.Size(23, 12);
            this.lblUnit2.TabIndex = 62;
            this.lblUnit2.Text = "mm";
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(235, 47);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(23, 12);
            this.lblUnit.TabIndex = 61;
            this.lblUnit.Text = "mm";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(110, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 12);
            this.label4.TabIndex = 59;
            this.label4.Text = "：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(110, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 12);
            this.label3.TabIndex = 58;
            this.label3.Text = "：";
            // 
            // lblColon
            // 
            this.lblColon.AutoSize = true;
            this.lblColon.Location = new System.Drawing.Point(110, 20);
            this.lblColon.Name = "lblColon";
            this.lblColon.Size = new System.Drawing.Size(11, 12);
            this.lblColon.TabIndex = 57;
            this.lblColon.Text = "：";
            // 
            // lblPixSize
            // 
            this.lblPixSize.AutoSize = true;
            this.lblPixSize.Location = new System.Drawing.Point(19, 69);
            this.lblPixSize.Name = "lblPixSize";
            this.lblPixSize.Size = new System.Drawing.Size(75, 12);
            this.lblPixSize.TabIndex = 56;
            this.lblPixSize.Text = "#1画素サイズ";
            // 
            // lblScanArea
            // 
            this.lblScanArea.AutoSize = true;
            this.lblScanArea.Location = new System.Drawing.Point(19, 45);
            this.lblScanArea.Name = "lblScanArea";
            this.lblScanArea.Size = new System.Drawing.Size(85, 12);
            this.lblScanArea.TabIndex = 55;
            this.lblScanArea.Text = "#スキャンエリア";
            // 
            // lblScanMode
            // 
            this.lblScanMode.AutoSize = true;
            this.lblScanMode.Location = new System.Drawing.Point(19, 21);
            this.lblScanMode.Name = "lblScanMode";
            this.lblScanMode.Size = new System.Drawing.Size(85, 12);
            this.lblScanMode.TabIndex = 0;
            this.lblScanMode.Text = "#スキャンモード";
            // 
            // fraScanArea
            // 
            this.fraScanArea.Controls.Add(this.ntbShift);
            this.fraScanArea.Controls.Add(this.ntbOffset);
            this.fraScanArea.Controls.Add(this.ntbFullHalf);
            this.fraScanArea.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fraScanArea.Location = new System.Drawing.Point(3, 297);
            this.fraScanArea.Name = "fraScanArea";
            this.fraScanArea.Size = new System.Drawing.Size(209, 109);
            this.fraScanArea.TabIndex = 54;
            this.fraScanArea.TabStop = false;
            this.fraScanArea.Text = "#スキャンエリア";
            // 
            // ntbShift
            // 
            this.ntbShift.BackColor = System.Drawing.SystemColors.Control;
            this.ntbShift.Caption = "#シフト";
            this.ntbShift.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbShift.CaptionFont = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbShift.CaptionWidth = 93;
            this.ntbShift.DiscreteInterval = 5F;
            this.ntbShift.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbShift.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbShift.IncDecButton = false;
            this.ntbShift.Location = new System.Drawing.Point(16, 78);
            this.ntbShift.Max = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.ntbShift.Min = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ntbShift.Name = "ntbShift";
            this.ntbShift.ReadOnly = false;
            this.ntbShift.Size = new System.Drawing.Size(181, 21);
            this.ntbShift.TabIndex = 2;
            this.ntbShift.TextBackColor = System.Drawing.SystemColors.Window;
            this.ntbShift.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbShift.Unit = "mm";
            this.ntbShift.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // ntbOffset
            // 
            this.ntbOffset.BackColor = System.Drawing.SystemColors.Control;
            this.ntbOffset.Caption = "#オフセット";
            this.ntbOffset.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbOffset.CaptionFont = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbOffset.CaptionWidth = 93;
            this.ntbOffset.DiscreteInterval = 5F;
            this.ntbOffset.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbOffset.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbOffset.IncDecButton = false;
            this.ntbOffset.Location = new System.Drawing.Point(16, 48);
            this.ntbOffset.Max = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.ntbOffset.Min = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ntbOffset.Name = "ntbOffset";
            this.ntbOffset.ReadOnly = false;
            this.ntbOffset.Size = new System.Drawing.Size(181, 21);
            this.ntbOffset.TabIndex = 1;
            this.ntbOffset.TextBackColor = System.Drawing.SystemColors.Control;
            this.ntbOffset.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbOffset.Unit = "mm";
            this.ntbOffset.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // ntbFullHalf
            // 
            this.ntbFullHalf.BackColor = System.Drawing.SystemColors.Control;
            this.ntbFullHalf.Caption = "#フル,ハーフ";
            this.ntbFullHalf.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbFullHalf.CaptionFont = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbFullHalf.CaptionWidth = 93;
            this.ntbFullHalf.DiscreteInterval = 5F;
            this.ntbFullHalf.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbFullHalf.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbFullHalf.IncDecButton = false;
            this.ntbFullHalf.Location = new System.Drawing.Point(16, 21);
            this.ntbFullHalf.Max = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.ntbFullHalf.Min = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ntbFullHalf.Name = "ntbFullHalf";
            this.ntbFullHalf.ReadOnly = true;
            this.ntbFullHalf.Size = new System.Drawing.Size(181, 21);
            this.ntbFullHalf.TabIndex = 0;
            this.ntbFullHalf.TextBackColor = System.Drawing.SystemColors.Control;
            this.ntbFullHalf.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbFullHalf.Unit = "mm";
            this.ntbFullHalf.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // fraUpDown
            // 
            this.fraUpDown.Controls.Add(this.lblMin);
            this.fraUpDown.Controls.Add(this.lblMiddle);
            this.fraUpDown.Controls.Add(this.lblMax);
            this.fraUpDown.Controls.Add(this.cwbtnUpDown0);
            this.fraUpDown.Controls.Add(this.cwbtnUpDown1);
            this.fraUpDown.Controls.Add(this.ctSliderVScroll1);
            this.fraUpDown.Controls.Add(this.cwnePos);
            this.fraUpDown.Controls.Add(this.txtUpDownPos);
            this.fraUpDown.Controls.Add(this.lblUP);
            this.fraUpDown.Controls.Add(this.lblDOWN);
            this.fraUpDown.Controls.Add(this.cmdPosExec);
            this.fraUpDown.Controls.Add(this.Label22);
            this.fraUpDown.Controls.Add(this.shapeContainer1);
            this.fraUpDown.Location = new System.Drawing.Point(296, 0);
            this.fraUpDown.Name = "fraUpDown";
            this.fraUpDown.Size = new System.Drawing.Size(146, 353);
            this.fraUpDown.TabIndex = 41;
            // 
            // lblMin
            // 
            this.lblMin.AutoSize = true;
            this.lblMin.Location = new System.Drawing.Point(89, 52);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(11, 12);
            this.lblMin.TabIndex = 81;
            this.lblMin.Text = "0";
            // 
            // lblMiddle
            // 
            this.lblMiddle.AutoSize = true;
            this.lblMiddle.Location = new System.Drawing.Point(89, 142);
            this.lblMiddle.Name = "lblMiddle";
            this.lblMiddle.Size = new System.Drawing.Size(23, 12);
            this.lblMiddle.TabIndex = 80;
            this.lblMiddle.Text = "100";
            // 
            // lblMax
            // 
            this.lblMax.AutoSize = true;
            this.lblMax.Location = new System.Drawing.Point(89, 232);
            this.lblMax.Name = "lblMax";
            this.lblMax.Size = new System.Drawing.Size(23, 12);
            this.lblMax.TabIndex = 79;
            this.lblMax.Text = "200";
            // 
            // cwbtnUpDown0
            // 
            this.cwbtnUpDown0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnUpDown0.BlinkInterval = CT30K.CWSpeeds.cwSpeedFastest;
            this.cwbtnUpDown0.Caption = "";
            this.cwbtnUpDown0.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnUpDown0.FlatAppearanceBorderSize = 1;
            this.cwbtnUpDown0.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnUpDown0.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnUpDown0.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnUpDown0.IsIgnorePerformClick = false;
            this.cwbtnUpDown0.Location = new System.Drawing.Point(48, 242);
            this.cwbtnUpDown0.Name = "cwbtnUpDown0";
            this.cwbtnUpDown0.OffImage = null;
            this.cwbtnUpDown0.OnColor = System.Drawing.Color.Empty;
            this.cwbtnUpDown0.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnUpDown0.OnImage")));
            this.cwbtnUpDown0.Size = new System.Drawing.Size(32, 32);
            this.cwbtnUpDown0.TabIndex = 73;
            this.cwbtnUpDown0.TabStop = false;
            this.cwbtnUpDown0.Tag = "YBackward";
            this.cwbtnUpDown0.Value = false;
            this.cwbtnUpDown0.ValueChanged += new System.EventHandler(this.cwbtnUpDown_ValueChanged);
            this.cwbtnUpDown0.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cwbtnUpDown_MouseUp);
            this.cwbtnUpDown0.MouseCaptureChanged += new System.EventHandler(this.cwbtnUpDown_MouseCaptureChanged);
            // 
            // cwbtnUpDown1
            // 
            this.cwbtnUpDown1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnUpDown1.BlinkInterval = CT30K.CWSpeeds.cwSpeedFastest;
            this.cwbtnUpDown1.Caption = "";
            this.cwbtnUpDown1.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnUpDown1.FlatAppearanceBorderSize = 1;
            this.cwbtnUpDown1.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnUpDown1.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnUpDown1.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnUpDown1.IsIgnorePerformClick = false;
            this.cwbtnUpDown1.Location = new System.Drawing.Point(48, 21);
            this.cwbtnUpDown1.Name = "cwbtnUpDown1";
            this.cwbtnUpDown1.OffImage = null;
            this.cwbtnUpDown1.OnColor = System.Drawing.Color.Empty;
            this.cwbtnUpDown1.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnUpDown1.OnImage")));
            this.cwbtnUpDown1.Size = new System.Drawing.Size(32, 32);
            this.cwbtnUpDown1.TabIndex = 74;
            this.cwbtnUpDown1.TabStop = false;
            this.cwbtnUpDown1.Tag = "YForward";
            this.cwbtnUpDown1.Value = false;
            this.cwbtnUpDown1.ValueChanged += new System.EventHandler(this.cwbtnUpDown_ValueChanged);
            this.cwbtnUpDown1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cwbtnUpDown_MouseUp);
            this.cwbtnUpDown1.MouseCaptureChanged += new System.EventHandler(this.cwbtnUpDown_MouseCaptureChanged);
            // 
            // ctSliderVScroll1
            // 
            this.ctSliderVScroll1.ArrowDirection = CT30K.CTSliderVScroll.ArrowDirectionType.Right;
            this.ctSliderVScroll1.ArrowValue = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.ctSliderVScroll1.ArrowVisible = true;
            this.ctSliderVScroll1.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ctSliderVScroll1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ctSliderVScroll1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ctSliderVScroll1.DecimalPlaces = 0;
            this.ctSliderVScroll1.LargeChange = 5;
            this.ctSliderVScroll1.Location = new System.Drawing.Point(49, 50);
            this.ctSliderVScroll1.Maximum = 200;
            this.ctSliderVScroll1.Minimum = 0;
            this.ctSliderVScroll1.Name = "ctSliderVScroll1";
            this.ctSliderVScroll1.PointChangeMode = CT30K.CTSliderVScroll.PointerChangeType.PointChangeOn;
            this.ctSliderVScroll1.PointerHeight = 16;
            this.ctSliderVScroll1.Reverse = true;
            this.ctSliderVScroll1.Size = new System.Drawing.Size(31, 195);
            this.ctSliderVScroll1.TabIndex = 86;
            this.ctSliderVScroll1.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ctSliderVScroll1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cwsldUpDown_MouseDown);
            this.ctSliderVScroll1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cwsldUpDown_MouseUp);
            // 
            // cwnePos
            // 
            this.cwnePos.DecimalPlaces = 3;
            this.cwnePos.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwnePos.Increment = new decimal(new int[] {
            2,
            0,
            0,
            196608});
            this.cwnePos.Location = new System.Drawing.Point(88, 314);
            this.cwnePos.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.cwnePos.Name = "cwnePos";
            this.cwnePos.Size = new System.Drawing.Size(21, 26);
            this.cwnePos.TabIndex = 75;
            this.cwnePos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwnePos.ValueChanged += new System.EventHandler(this.cwnePos_ValueChanged);
            // 
            // txtUpDownPos
            // 
            this.txtUpDownPos.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtUpDownPos.Location = new System.Drawing.Point(10, 314);
            this.txtUpDownPos.Name = "txtUpDownPos";
            this.txtUpDownPos.Size = new System.Drawing.Size(101, 26);
            this.txtUpDownPos.TabIndex = 85;
            this.txtUpDownPos.Text = "9999.999";
            this.txtUpDownPos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtUpDownPos.WordWrap = false;
            this.txtUpDownPos.TextChanged += new System.EventHandler(this.txtUpDownPos_TextChanged);
            this.txtUpDownPos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUpDownPos_KeyDown);
            this.txtUpDownPos.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUpDownPos_KeyPress);
            this.txtUpDownPos.Leave += new System.EventHandler(this.txtUpDownPos_Leave);
            // 
            // lblUP
            // 
            this.lblUP.AutoSize = true;
            this.lblUP.Location = new System.Drawing.Point(20, 33);
            this.lblUP.Name = "lblUP";
            this.lblUP.Size = new System.Drawing.Size(20, 12);
            this.lblUP.TabIndex = 77;
            this.lblUP.Text = "UP";
            // 
            // lblDOWN
            // 
            this.lblDOWN.AutoSize = true;
            this.lblDOWN.Location = new System.Drawing.Point(9, 253);
            this.lblDOWN.Name = "lblDOWN";
            this.lblDOWN.Size = new System.Drawing.Size(38, 12);
            this.lblDOWN.TabIndex = 78;
            this.lblDOWN.Text = "DOWN";
            // 
            // cmdPosExec
            // 
            this.cmdPosExec.Location = new System.Drawing.Point(6, 280);
            this.cmdPosExec.Name = "cmdPosExec";
            this.cmdPosExec.Size = new System.Drawing.Size(120, 28);
            this.cmdPosExec.TabIndex = 76;
            this.cmdPosExec.Tag = "15211";
            this.cmdPosExec.Text = "#指定高さ位置移動";
            this.cmdPosExec.UseVisualStyleBackColor = true;
            this.cmdPosExec.Click += new System.EventHandler(this.cmdPosExec_Click);
            // 
            // Label22
            // 
            this.Label22.AutoSize = true;
            this.Label22.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Label22.Location = new System.Drawing.Point(112, 318);
            this.Label22.Margin = new System.Windows.Forms.Padding(0);
            this.Label22.Name = "Label22";
            this.Label22.Size = new System.Drawing.Size(32, 16);
            this.Label22.TabIndex = 82;
            this.Label22.Text = "mm";
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineMax,
            this.lineMiddle,
            this.lineMin});
            this.shapeContainer1.Size = new System.Drawing.Size(146, 353);
            this.shapeContainer1.TabIndex = 87;
            this.shapeContainer1.TabStop = false;
            // 
            // lineMax
            // 
            this.lineMax.Name = "lineMax";
            this.lineMax.X1 = 81;
            this.lineMax.X2 = 85;
            this.lineMax.Y1 = 237;
            this.lineMax.Y2 = 237;
            // 
            // lineMiddle
            // 
            this.lineMiddle.Name = "lineMiddle";
            this.lineMiddle.X1 = 81;
            this.lineMiddle.X2 = 85;
            this.lineMiddle.Y1 = 147;
            this.lineMiddle.Y2 = 147;
            // 
            // lineMin
            // 
            this.lineMin.Name = "lineMin";
            this.lineMin.X1 = 81;
            this.lineMin.X2 = 85;
            this.lineMin.Y1 = 58;
            this.lineMin.Y2 = 58;
            // 
            // fraMechaControl
            // 
            this.fraMechaControl.Controls.Add(this.cwbtnFineTable0);
            this.fraMechaControl.Controls.Add(this.cwbtnFineTable1);
            this.fraMechaControl.Controls.Add(this.cwbtnMove1);
            this.fraMechaControl.Controls.Add(this.cwbtnMove0);
            this.fraMechaControl.Controls.Add(this.cwbtnRotate1);
            this.fraMechaControl.Controls.Add(this.cwbtnRotate0);
            this.fraMechaControl.Controls.Add(this.cwbtnMove3);
            this.fraMechaControl.Controls.Add(this.cwbtnMove2);
            this.fraMechaControl.Controls.Add(this.fraIIMove);
            this.fraMechaControl.Controls.Add(this.lblXrayII);
            this.fraMechaControl.Controls.Add(this.cwbtnDetShift);
            this.fraMechaControl.Controls.Add(this.pnlDetShift);
            this.fraMechaControl.Controls.Add(this.lblFTable);
            this.fraMechaControl.Controls.Add(this.lblRot1);
            this.fraMechaControl.Controls.Add(this.lblRot3);
            this.fraMechaControl.Controls.Add(this.lblRot2);
            this.fraMechaControl.Controls.Add(this.lblRot0);
            this.fraMechaControl.Controls.Add(this.ctchkRotate2);
            this.fraMechaControl.Controls.Add(this.ctchkRotate1);
            this.fraMechaControl.Controls.Add(this.ctchkRotate3);
            this.fraMechaControl.Controls.Add(this.ctchkRotate0);
            this.fraMechaControl.Controls.Add(this.lblXrayTube);
            this.fraMechaControl.Controls.Add(this.ImgXrayTube);
            this.fraMechaControl.Controls.Add(this.fraTableRestriction);
            this.fraMechaControl.Controls.Add(this.shapeContainer2);
            this.fraMechaControl.Location = new System.Drawing.Point(448, 0);
            this.fraMechaControl.Name = "fraMechaControl";
            this.fraMechaControl.Size = new System.Drawing.Size(233, 353);
            this.fraMechaControl.TabIndex = 42;
            // 
            // cwbtnFineTable0
            // 
            this.cwbtnFineTable0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnFineTable0.BlinkInterval = CT30K.CWSpeeds.cwSpeedFastest;
            this.cwbtnFineTable0.Caption = "";
            this.cwbtnFineTable0.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnFineTable0.FlatAppearanceBorderSize = 1;
            this.cwbtnFineTable0.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnFineTable0.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnFineTable0.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnFineTable0.IsIgnorePerformClick = false;
            this.cwbtnFineTable0.Location = new System.Drawing.Point(72, 159);
            this.cwbtnFineTable0.Name = "cwbtnFineTable0";
            this.cwbtnFineTable0.OffImage = null;
            this.cwbtnFineTable0.OnColor = System.Drawing.Color.Empty;
            this.cwbtnFineTable0.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnFineTable0.OnImage")));
            this.cwbtnFineTable0.Size = new System.Drawing.Size(32, 32);
            this.cwbtnFineTable0.TabIndex = 108;
            this.cwbtnFineTable0.TabStop = false;
            this.cwbtnFineTable0.Tag = "";
            this.cwbtnFineTable0.Value = false;
            this.cwbtnFineTable0.ValueChanged += new System.EventHandler(this.cwbtnFineTable_ValueChanged);
            this.cwbtnFineTable0.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cwbtnFineTable_MouseUp);
            this.cwbtnFineTable0.MouseCaptureChanged += new System.EventHandler(this.cwbtnFineTable_MouseCaptureChanged);
            // 
            // cwbtnFineTable1
            // 
            this.cwbtnFineTable1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnFineTable1.BlinkInterval = CT30K.CWSpeeds.cwSpeedVeryFast;
            this.cwbtnFineTable1.Caption = "";
            this.cwbtnFineTable1.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnFineTable1.FlatAppearanceBorderSize = 1;
            this.cwbtnFineTable1.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnFineTable1.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnFineTable1.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnFineTable1.IsIgnorePerformClick = false;
            this.cwbtnFineTable1.Location = new System.Drawing.Point(130, 159);
            this.cwbtnFineTable1.Name = "cwbtnFineTable1";
            this.cwbtnFineTable1.OffImage = null;
            this.cwbtnFineTable1.OnColor = System.Drawing.Color.Empty;
            this.cwbtnFineTable1.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnFineTable1.OnImage")));
            this.cwbtnFineTable1.Size = new System.Drawing.Size(32, 32);
            this.cwbtnFineTable1.TabIndex = 109;
            this.cwbtnFineTable1.TabStop = false;
            this.cwbtnFineTable1.Tag = "";
            this.cwbtnFineTable1.Value = false;
            this.cwbtnFineTable1.ValueChanged += new System.EventHandler(this.cwbtnFineTable_ValueChanged);
            this.cwbtnFineTable1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cwbtnFineTable_MouseUp);
            this.cwbtnFineTable1.MouseCaptureChanged += new System.EventHandler(this.cwbtnFineTable_MouseCaptureChanged);
            // 
            // cwbtnMove1
            // 
            this.cwbtnMove1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnMove1.BlinkInterval = CT30K.CWSpeeds.cwSpeedFastest;
            this.cwbtnMove1.Caption = "";
            this.cwbtnMove1.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnMove1.FlatAppearanceBorderSize = 1;
            this.cwbtnMove1.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnMove1.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnMove1.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnMove1.IsIgnorePerformClick = false;
            this.cwbtnMove1.Location = new System.Drawing.Point(189, 159);
            this.cwbtnMove1.Name = "cwbtnMove1";
            this.cwbtnMove1.OffImage = null;
            this.cwbtnMove1.OnColor = System.Drawing.Color.Empty;
            this.cwbtnMove1.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnMove1.OnImage")));
            this.cwbtnMove1.Size = new System.Drawing.Size(32, 32);
            this.cwbtnMove1.TabIndex = 113;
            this.cwbtnMove1.TabStop = false;
            this.cwbtnMove1.Tag = "XRight";
            this.cwbtnMove1.Value = false;
            this.cwbtnMove1.ValueChanged += new System.EventHandler(this.cwbtnMove_ValueChanged);
            this.cwbtnMove1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cwbtnMove_MouseUp);
            this.cwbtnMove1.MouseCaptureChanged += new System.EventHandler(this.cwbtnMove_MouseCaptureChanged);
            // 
            // cwbtnMove0
            // 
            this.cwbtnMove0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnMove0.BlinkInterval = CT30K.CWSpeeds.cwSpeedFastest;
            this.cwbtnMove0.Caption = "";
            this.cwbtnMove0.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnMove0.FlatAppearanceBorderSize = 1;
            this.cwbtnMove0.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnMove0.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnMove0.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnMove0.IsIgnorePerformClick = false;
            this.cwbtnMove0.Location = new System.Drawing.Point(12, 159);
            this.cwbtnMove0.Name = "cwbtnMove0";
            this.cwbtnMove0.OffImage = null;
            this.cwbtnMove0.OnColor = System.Drawing.Color.Empty;
            this.cwbtnMove0.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnMove0.OnImage")));
            this.cwbtnMove0.Size = new System.Drawing.Size(32, 32);
            this.cwbtnMove0.TabIndex = 112;
            this.cwbtnMove0.TabStop = false;
            this.cwbtnMove0.Tag = "XLeft";
            this.cwbtnMove0.Value = false;
            this.cwbtnMove0.ValueChanged += new System.EventHandler(this.cwbtnMove_ValueChanged);
            this.cwbtnMove0.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cwbtnMove_MouseUp);
            this.cwbtnMove0.MouseCaptureChanged += new System.EventHandler(this.cwbtnMove_MouseCaptureChanged);
            // 
            // cwbtnRotate1
            // 
            this.cwbtnRotate1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnRotate1.BlinkInterval = CT30K.CWSpeeds.cwSpeedFastest;
            this.cwbtnRotate1.Caption = "";
            this.cwbtnRotate1.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnRotate1.FlatAppearanceBorderSize = 1;
            this.cwbtnRotate1.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnRotate1.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnRotate1.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnRotate1.IsIgnorePerformClick = false;
            this.cwbtnRotate1.Location = new System.Drawing.Point(172, 99);
            this.cwbtnRotate1.Name = "cwbtnRotate1";
            this.cwbtnRotate1.OffImage = null;
            this.cwbtnRotate1.OnColor = System.Drawing.Color.Empty;
            this.cwbtnRotate1.OnImage = global::CT30K.Properties.Resources.RightRotate;
            this.cwbtnRotate1.Size = new System.Drawing.Size(29, 29);
            this.cwbtnRotate1.TabIndex = 102;
            this.cwbtnRotate1.TabStop = false;
            this.cwbtnRotate1.Value = false;
            this.cwbtnRotate1.ValueChanged += new System.EventHandler(this.cwbtnRotate_ValueChanged);
            this.cwbtnRotate1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cwbtnRotate_MouseUp);
            this.cwbtnRotate1.MouseCaptureChanged += new System.EventHandler(this.cwbtnRotate_MouseCaptureChanged);
            // 
            // cwbtnRotate0
            // 
            this.cwbtnRotate0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnRotate0.BlinkInterval = CT30K.CWSpeeds.cwSpeedFastest;
            this.cwbtnRotate0.Caption = "";
            this.cwbtnRotate0.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnRotate0.FlatAppearanceBorderSize = 1;
            this.cwbtnRotate0.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnRotate0.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnRotate0.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnRotate0.IsIgnorePerformClick = false;
            this.cwbtnRotate0.Location = new System.Drawing.Point(34, 99);
            this.cwbtnRotate0.Name = "cwbtnRotate0";
            this.cwbtnRotate0.OffImage = null;
            this.cwbtnRotate0.OnColor = System.Drawing.Color.Empty;
            this.cwbtnRotate0.OnImage = global::CT30K.Properties.Resources.LeftRotate;
            this.cwbtnRotate0.Size = new System.Drawing.Size(29, 29);
            this.cwbtnRotate0.TabIndex = 103;
            this.cwbtnRotate0.TabStop = false;
            this.cwbtnRotate0.Value = false;
            this.cwbtnRotate0.ValueChanged += new System.EventHandler(this.cwbtnRotate_ValueChanged);
            this.cwbtnRotate0.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cwbtnRotate_MouseUp);
            this.cwbtnRotate0.MouseCaptureChanged += new System.EventHandler(this.cwbtnRotate_MouseCaptureChanged);
            // 
            // cwbtnMove3
            // 
            this.cwbtnMove3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnMove3.BlinkInterval = CT30K.CWSpeeds.cwSpeedFastest;
            this.cwbtnMove3.Caption = "";
            this.cwbtnMove3.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnMove3.FlatAppearanceBorderSize = 1;
            this.cwbtnMove3.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnMove3.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnMove3.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnMove3.IsIgnorePerformClick = false;
            this.cwbtnMove3.Location = new System.Drawing.Point(101, 247);
            this.cwbtnMove3.Name = "cwbtnMove3";
            this.cwbtnMove3.OffImage = null;
            this.cwbtnMove3.OnColor = System.Drawing.Color.Empty;
            this.cwbtnMove3.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnMove3.OnImage")));
            this.cwbtnMove3.Size = new System.Drawing.Size(32, 32);
            this.cwbtnMove3.TabIndex = 111;
            this.cwbtnMove3.TabStop = false;
            this.cwbtnMove3.Tag = "YBackward";
            this.cwbtnMove3.Value = false;
            this.cwbtnMove3.ValueChanged += new System.EventHandler(this.cwbtnMove_ValueChanged);
            this.cwbtnMove3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cwbtnMove_MouseUp);
            this.cwbtnMove3.MouseCaptureChanged += new System.EventHandler(this.cwbtnMove_MouseCaptureChanged);
            // 
            // cwbtnMove2
            // 
            this.cwbtnMove2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnMove2.BlinkInterval = CT30K.CWSpeeds.cwSpeedFastest;
            this.cwbtnMove2.Caption = "";
            this.cwbtnMove2.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnMove2.FlatAppearanceBorderSize = 1;
            this.cwbtnMove2.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnMove2.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnMove2.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnMove2.IsIgnorePerformClick = false;
            this.cwbtnMove2.Location = new System.Drawing.Point(101, 70);
            this.cwbtnMove2.Name = "cwbtnMove2";
            this.cwbtnMove2.OffImage = null;
            this.cwbtnMove2.OnColor = System.Drawing.Color.Empty;
            this.cwbtnMove2.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnMove2.OnImage")));
            this.cwbtnMove2.Size = new System.Drawing.Size(32, 32);
            this.cwbtnMove2.TabIndex = 110;
            this.cwbtnMove2.TabStop = false;
            this.cwbtnMove2.Tag = "YForward";
            this.cwbtnMove2.Value = false;
            this.cwbtnMove2.ValueChanged += new System.EventHandler(this.cwbtnMove_ValueChanged);
            this.cwbtnMove2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cwbtnMove_MouseUp);
            this.cwbtnMove2.MouseCaptureChanged += new System.EventHandler(this.cwbtnMove_MouseCaptureChanged);
            // 
            // fraIIMove
            // 
            this.fraIIMove.Controls.Add(this.lblIIMove4);
            this.fraIIMove.Controls.Add(this.ctchkIIMove4);
            this.fraIIMove.Controls.Add(this.lblIIMove3);
            this.fraIIMove.Controls.Add(this.lblIIMove2);
            this.fraIIMove.Controls.Add(this.lblIIMove1);
            this.fraIIMove.Controls.Add(this.lblIIMove0);
            this.fraIIMove.Controls.Add(this.ctchkIIMove3);
            this.fraIIMove.Controls.Add(this.ctchkIIMove2);
            this.fraIIMove.Controls.Add(this.ctchkIIMove1);
            this.fraIIMove.Controls.Add(this.ctchkIIMove0);
            this.fraIIMove.Location = new System.Drawing.Point(170, 269);
            this.fraIIMove.Name = "fraIIMove";
            this.fraIIMove.Size = new System.Drawing.Size(53, 78);
            this.fraIIMove.TabIndex = 101;
            // 
            // lblIIMove4
            // 
            this.lblIIMove4.AutoSize = true;
            this.lblIIMove4.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblIIMove4.Location = new System.Drawing.Point(19, 65);
            this.lblIIMove4.Name = "lblIIMove4";
            this.lblIIMove4.Size = new System.Drawing.Size(33, 12);
            this.lblIIMove4.TabIndex = 98;
            this.lblIIMove4.Text = "1200";
            // 
            // ctchkIIMove4
            // 
            this.ctchkIIMove4.BackColor = System.Drawing.SystemColors.Control;
            this.ctchkIIMove4.Checked = false;
            this.ctchkIIMove4.Location = new System.Drawing.Point(0, 64);
            this.ctchkIIMove4.Name = "ctchkIIMove4";
            this.ctchkIIMove4.Size = new System.Drawing.Size(13, 13);
            this.ctchkIIMove4.TabIndex = 97;
            this.ctchkIIMove4.Value = false;
            this.ctchkIIMove4.CheckedByClick += new System.EventHandler(this.ctchkIIMove_CheckedByClick);
            // 
            // lblIIMove3
            // 
            this.lblIIMove3.AutoSize = true;
            this.lblIIMove3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblIIMove3.Location = new System.Drawing.Point(19, 49);
            this.lblIIMove3.Name = "lblIIMove3";
            this.lblIIMove3.Size = new System.Drawing.Size(33, 12);
            this.lblIIMove3.TabIndex = 76;
            this.lblIIMove3.Text = "1000";
            // 
            // lblIIMove2
            // 
            this.lblIIMove2.AutoSize = true;
            this.lblIIMove2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblIIMove2.Location = new System.Drawing.Point(19, 33);
            this.lblIIMove2.Name = "lblIIMove2";
            this.lblIIMove2.Size = new System.Drawing.Size(26, 12);
            this.lblIIMove2.TabIndex = 77;
            this.lblIIMove2.Text = "800";
            // 
            // lblIIMove1
            // 
            this.lblIIMove1.AutoSize = true;
            this.lblIIMove1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblIIMove1.Location = new System.Drawing.Point(19, 17);
            this.lblIIMove1.Name = "lblIIMove1";
            this.lblIIMove1.Size = new System.Drawing.Size(26, 12);
            this.lblIIMove1.TabIndex = 78;
            this.lblIIMove1.Text = "600";
            // 
            // lblIIMove0
            // 
            this.lblIIMove0.AutoSize = true;
            this.lblIIMove0.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblIIMove0.Location = new System.Drawing.Point(19, 0);
            this.lblIIMove0.Name = "lblIIMove0";
            this.lblIIMove0.Size = new System.Drawing.Size(26, 12);
            this.lblIIMove0.TabIndex = 79;
            this.lblIIMove0.Text = "400";
            // 
            // ctchkIIMove3
            // 
            this.ctchkIIMove3.BackColor = System.Drawing.SystemColors.Control;
            this.ctchkIIMove3.Checked = false;
            this.ctchkIIMove3.Location = new System.Drawing.Point(0, 48);
            this.ctchkIIMove3.Name = "ctchkIIMove3";
            this.ctchkIIMove3.Size = new System.Drawing.Size(13, 13);
            this.ctchkIIMove3.TabIndex = 75;
            this.ctchkIIMove3.Value = false;
            this.ctchkIIMove3.CheckedByClick += new System.EventHandler(this.ctchkIIMove_CheckedByClick);
            // 
            // ctchkIIMove2
            // 
            this.ctchkIIMove2.BackColor = System.Drawing.SystemColors.Control;
            this.ctchkIIMove2.Checked = false;
            this.ctchkIIMove2.Location = new System.Drawing.Point(0, 32);
            this.ctchkIIMove2.Name = "ctchkIIMove2";
            this.ctchkIIMove2.Size = new System.Drawing.Size(13, 13);
            this.ctchkIIMove2.TabIndex = 74;
            this.ctchkIIMove2.Value = false;
            this.ctchkIIMove2.CheckedByClick += new System.EventHandler(this.ctchkIIMove_CheckedByClick);
            // 
            // ctchkIIMove1
            // 
            this.ctchkIIMove1.BackColor = System.Drawing.SystemColors.Control;
            this.ctchkIIMove1.Checked = false;
            this.ctchkIIMove1.Location = new System.Drawing.Point(0, 16);
            this.ctchkIIMove1.Name = "ctchkIIMove1";
            this.ctchkIIMove1.Size = new System.Drawing.Size(13, 13);
            this.ctchkIIMove1.TabIndex = 73;
            this.ctchkIIMove1.Value = false;
            this.ctchkIIMove1.CheckedByClick += new System.EventHandler(this.ctchkIIMove_CheckedByClick);
            // 
            // ctchkIIMove0
            // 
            this.ctchkIIMove0.BackColor = System.Drawing.SystemColors.Control;
            this.ctchkIIMove0.Checked = false;
            this.ctchkIIMove0.Location = new System.Drawing.Point(0, 0);
            this.ctchkIIMove0.Name = "ctchkIIMove0";
            this.ctchkIIMove0.Size = new System.Drawing.Size(13, 13);
            this.ctchkIIMove0.TabIndex = 96;
            this.ctchkIIMove0.Value = false;
            this.ctchkIIMove0.CheckedByClick += new System.EventHandler(this.ctchkIIMove_CheckedByClick);
            // 
            // lblXrayII
            // 
            this.lblXrayII.BackColor = System.Drawing.SystemColors.Window;
            this.lblXrayII.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblXrayII.Location = new System.Drawing.Point(82, 297);
            this.lblXrayII.Name = "lblXrayII";
            this.lblXrayII.Size = new System.Drawing.Size(74, 21);
            this.lblXrayII.TabIndex = 116;
            this.lblXrayII.Tag = "12332";
            this.lblXrayII.Text = "#Ｘ線  Ｉ．Ｉ．";
            this.lblXrayII.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cwbtnDetShift
            // 
            this.cwbtnDetShift.BackColor = System.Drawing.Color.Green;
            this.cwbtnDetShift.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnDetShift.Caption = "#シフト";
            this.cwbtnDetShift.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnDetShift.FlatAppearanceBorderSize = 1;
            this.cwbtnDetShift.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnDetShift.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnDetShift.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnDetShift.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwbtnDetShift.IsIgnorePerformClick = false;
            this.cwbtnDetShift.Location = new System.Drawing.Point(87, 322);
            this.cwbtnDetShift.Name = "cwbtnDetShift";
            this.cwbtnDetShift.OffImage = null;
            this.cwbtnDetShift.OnColor = System.Drawing.Color.Lime;
            this.cwbtnDetShift.OnImage = null;
            this.cwbtnDetShift.Size = new System.Drawing.Size(64, 22);
            this.cwbtnDetShift.TabIndex = 121;
            this.cwbtnDetShift.Value = false;
            this.cwbtnDetShift.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cwbtnDetShift_Click);
            // 
            // pnlDetShift
            // 
            this.pnlDetShift.BackColor = System.Drawing.Color.Transparent;
            this.pnlDetShift.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDetShift.Location = new System.Drawing.Point(81, 296);
            this.pnlDetShift.Name = "pnlDetShift";
            this.pnlDetShift.Size = new System.Drawing.Size(83, 22);
            this.pnlDetShift.TabIndex = 120;
            // 
            // lblFTable
            // 
            this.lblFTable.AutoSize = true;
            this.lblFTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblFTable.Location = new System.Drawing.Point(101, 147);
            this.lblFTable.Name = "lblFTable";
            this.lblFTable.Size = new System.Drawing.Size(35, 12);
            this.lblFTable.TabIndex = 119;
            this.lblFTable.Tag = "12130";
            this.lblFTable.Text = "#微調";
            // 
            // lblRot1
            // 
            this.lblRot1.AutoSize = true;
            this.lblRot1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblRot1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblRot1.Location = new System.Drawing.Point(164, 157);
            this.lblRot1.Name = "lblRot1";
            this.lblRot1.Size = new System.Drawing.Size(19, 12);
            this.lblRot1.TabIndex = 115;
            this.lblRot1.Tag = "";
            this.lblRot1.Text = "90";
            // 
            // lblRot3
            // 
            this.lblRot3.AutoSize = true;
            this.lblRot3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblRot3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblRot3.Location = new System.Drawing.Point(50, 157);
            this.lblRot3.Name = "lblRot3";
            this.lblRot3.Size = new System.Drawing.Size(26, 12);
            this.lblRot3.TabIndex = 114;
            this.lblRot3.Tag = "";
            this.lblRot3.Text = "270";
            // 
            // lblRot2
            // 
            this.lblRot2.AutoSize = true;
            this.lblRot2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblRot2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblRot2.Location = new System.Drawing.Point(104, 214);
            this.lblRot2.Name = "lblRot2";
            this.lblRot2.Size = new System.Drawing.Size(26, 12);
            this.lblRot2.TabIndex = 118;
            this.lblRot2.Tag = "";
            this.lblRot2.Text = "180";
            // 
            // lblRot0
            // 
            this.lblRot0.AutoSize = true;
            this.lblRot0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblRot0.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblRot0.Location = new System.Drawing.Point(111, 125);
            this.lblRot0.Name = "lblRot0";
            this.lblRot0.Size = new System.Drawing.Size(12, 12);
            this.lblRot0.TabIndex = 117;
            this.lblRot0.Tag = "";
            this.lblRot0.Text = "0";
            // 
            // ctchkRotate2
            // 
            this.ctchkRotate2.BackColor = System.Drawing.SystemColors.Control;
            this.ctchkRotate2.Checked = false;
            this.ctchkRotate2.Location = new System.Drawing.Point(110, 227);
            this.ctchkRotate2.Name = "ctchkRotate2";
            this.ctchkRotate2.Size = new System.Drawing.Size(13, 13);
            this.ctchkRotate2.TabIndex = 106;
            this.ctchkRotate2.Value = false;
            this.ctchkRotate2.CheckedByClick += new System.EventHandler(this.ctchkRotate_CheckedByClick);
            // 
            // ctchkRotate1
            // 
            this.ctchkRotate1.BackColor = System.Drawing.SystemColors.Control;
            this.ctchkRotate1.Checked = false;
            this.ctchkRotate1.Location = new System.Drawing.Point(168, 169);
            this.ctchkRotate1.Name = "ctchkRotate1";
            this.ctchkRotate1.Size = new System.Drawing.Size(13, 13);
            this.ctchkRotate1.TabIndex = 104;
            this.ctchkRotate1.Value = false;
            this.ctchkRotate1.CheckedByClick += new System.EventHandler(this.ctchkRotate_CheckedByClick);
            // 
            // ctchkRotate3
            // 
            this.ctchkRotate3.BackColor = System.Drawing.SystemColors.Control;
            this.ctchkRotate3.Checked = false;
            this.ctchkRotate3.Location = new System.Drawing.Point(53, 169);
            this.ctchkRotate3.Name = "ctchkRotate3";
            this.ctchkRotate3.Size = new System.Drawing.Size(13, 13);
            this.ctchkRotate3.TabIndex = 107;
            this.ctchkRotate3.Value = false;
            this.ctchkRotate3.CheckedByClick += new System.EventHandler(this.ctchkRotate_CheckedByClick);
            // 
            // ctchkRotate0
            // 
            this.ctchkRotate0.BackColor = System.Drawing.SystemColors.Control;
            this.ctchkRotate0.Checked = false;
            this.ctchkRotate0.Location = new System.Drawing.Point(110, 111);
            this.ctchkRotate0.Name = "ctchkRotate0";
            this.ctchkRotate0.Size = new System.Drawing.Size(13, 13);
            this.ctchkRotate0.TabIndex = 105;
            this.ctchkRotate0.Value = false;
            this.ctchkRotate0.CheckedByClick += new System.EventHandler(this.ctchkRotate_CheckedByClick);
            // 
            // lblXrayTube
            // 
            this.lblXrayTube.AutoSize = true;
            this.lblXrayTube.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblXrayTube.Location = new System.Drawing.Point(136, 28);
            this.lblXrayTube.Name = "lblXrayTube";
            this.lblXrayTube.Size = new System.Drawing.Size(43, 12);
            this.lblXrayTube.TabIndex = 100;
            this.lblXrayTube.Tag = "12218";
            this.lblXrayTube.Text = "#Ｘ線管";
            // 
            // ImgXrayTube
            // 
            this.ImgXrayTube.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ImgXrayTube.Image = ((System.Drawing.Image)(resources.GetObject("ImgXrayTube.Image")));
            this.ImgXrayTube.Location = new System.Drawing.Point(100, 18);
            this.ImgXrayTube.Margin = new System.Windows.Forms.Padding(0);
            this.ImgXrayTube.Name = "ImgXrayTube";
            this.ImgXrayTube.Size = new System.Drawing.Size(32, 32);
            this.ImgXrayTube.TabIndex = 99;
            this.ImgXrayTube.TabStop = false;
            // 
            // fraTableRestriction
            // 
            this.fraTableRestriction.Controls.Add(this.cmdTableMovePermit);
            this.fraTableRestriction.Controls.Add(this.cmdTableMoveRestrict);
            this.fraTableRestriction.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fraTableRestriction.Location = new System.Drawing.Point(12, 12);
            this.fraTableRestriction.Name = "fraTableRestriction";
            this.fraTableRestriction.Size = new System.Drawing.Size(85, 73);
            this.fraTableRestriction.TabIndex = 70;
            this.fraTableRestriction.TabStop = false;
            this.fraTableRestriction.Tag = "12350";
            this.fraTableRestriction.Text = "#Ｘ線管干渉";
            // 
            // cmdTableMovePermit
            // 
            this.cmdTableMovePermit.BackColor = System.Drawing.Color.Green;
            this.cmdTableMovePermit.Location = new System.Drawing.Point(12, 44);
            this.cmdTableMovePermit.Name = "cmdTableMovePermit";
            this.cmdTableMovePermit.Size = new System.Drawing.Size(61, 21);
            this.cmdTableMovePermit.TabIndex = 69;
            this.cmdTableMovePermit.Tag = "12351";
            this.cmdTableMovePermit.Text = "#解除";
            this.cmdTableMovePermit.UseVisualStyleBackColor = false;
            this.cmdTableMovePermit.Click += new System.EventHandler(this.cmdTableMovePermit_Click);
            // 
            // cmdTableMoveRestrict
            // 
            this.cmdTableMoveRestrict.BackColor = System.Drawing.Color.Lime;
            this.cmdTableMoveRestrict.Location = new System.Drawing.Point(12, 16);
            this.cmdTableMoveRestrict.Name = "cmdTableMoveRestrict";
            this.cmdTableMoveRestrict.Size = new System.Drawing.Size(61, 21);
            this.cmdTableMoveRestrict.TabIndex = 70;
            this.cmdTableMoveRestrict.Tag = "12352";
            this.cmdTableMoveRestrict.Text = "#制限";
            this.cmdTableMoveRestrict.UseVisualStyleBackColor = false;
            this.cmdTableMoveRestrict.Click += new System.EventHandler(this.cmdTableMoveRestrict_Click);
            // 
            // shapeContainer2
            // 
            this.shapeContainer2.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer2.Name = "shapeContainer2";
            this.shapeContainer2.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.ShpFTable,
            this.ShpTable,
            this.ShpCabinet});
            this.shapeContainer2.Size = new System.Drawing.Size(233, 353);
            this.shapeContainer2.TabIndex = 0;
            this.shapeContainer2.TabStop = false;
            // 
            // ShpFTable
            // 
            this.ShpFTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ShpFTable.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Opaque;
            this.ShpFTable.BorderWidth = 2;
            this.ShpFTable.Cursor = System.Windows.Forms.Cursors.Default;
            this.ShpFTable.Location = new System.Drawing.Point(105, 163);
            this.ShpFTable.Name = "ShpFTable";
            this.ShpFTable.Size = new System.Drawing.Size(23, 23);
            // 
            // ShpTable
            // 
            this.ShpTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ShpTable.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Opaque;
            this.ShpTable.BorderWidth = 2;
            this.ShpTable.Location = new System.Drawing.Point(46, 104);
            this.ShpTable.Name = "ShpTable";
            this.ShpTable.Size = new System.Drawing.Size(141, 141);
            // 
            // ShpCabinet
            // 
            this.ShpCabinet.BorderColor = System.Drawing.Color.Blue;
            this.ShpCabinet.BorderWidth = 3;
            this.ShpCabinet.Location = new System.Drawing.Point(7, 7);
            this.ShpCabinet.Name = "ShpCabinet";
            this.ShpCabinet.Size = new System.Drawing.Size(218, 342);
            // 
            // fraChangeDetector
            // 
            this.fraChangeDetector.Controls.Add(this.cwbtnChangeDet1);
            this.fraChangeDetector.Controls.Add(this.label2);
            this.fraChangeDetector.Controls.Add(this.cwbtnChangeDet0);
            this.fraChangeDetector.Location = new System.Drawing.Point(296, 503);
            this.fraChangeDetector.Name = "fraChangeDetector";
            this.fraChangeDetector.Size = new System.Drawing.Size(281, 65);
            this.fraChangeDetector.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 12);
            this.label2.TabIndex = 107;
            this.label2.Text = "検出器１⇔検出器２ 切替";
            this.label2.Visible = false;
            // 
            // tmrMecainfSeqComm
            // 
            this.tmrMecainfSeqComm.Interval = 1000;
            this.tmrMecainfSeqComm.Tick += new System.EventHandler(this.tmrMecainfSeqComm_Timer);
            // 
            // fraTiltAndRot
            // 
            this.fraTiltAndRot.Controls.Add(this.cwbtnTiltAndRot_Rot0);
            this.fraTiltAndRot.Controls.Add(this.cwbtnTiltAndRot_Rot1);
            this.fraTiltAndRot.Controls.Add(this.cwbtnTiltAndRot_Tilt1);
            this.fraTiltAndRot.Controls.Add(this.cwbtnTiltAndRot_Tilt0);
            this.fraTiltAndRot.Controls.Add(this.lblTilt);
            this.fraTiltAndRot.Controls.Add(this.lblRot);
            this.fraTiltAndRot.Location = new System.Drawing.Point(688, 476);
            this.fraTiltAndRot.Name = "fraTiltAndRot";
            this.fraTiltAndRot.Size = new System.Drawing.Size(142, 113);
            this.fraTiltAndRot.TabIndex = 44;
            this.fraTiltAndRot.TabStop = false;
            this.fraTiltAndRot.Tag = "22024";
            this.fraTiltAndRot.Text = "#回転傾斜";
            this.fraTiltAndRot.Visible = false;
            // 
            // cwbtnTiltAndRot_Rot0
            // 
            this.cwbtnTiltAndRot_Rot0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnTiltAndRot_Rot0.BlinkInterval = CT30K.CWSpeeds.cwSpeedFastest;
            this.cwbtnTiltAndRot_Rot0.Caption = "";
            this.cwbtnTiltAndRot_Rot0.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnTiltAndRot_Rot0.FlatAppearanceBorderSize = 1;
            this.cwbtnTiltAndRot_Rot0.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnTiltAndRot_Rot0.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnTiltAndRot_Rot0.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnTiltAndRot_Rot0.IsIgnorePerformClick = false;
            this.cwbtnTiltAndRot_Rot0.Location = new System.Drawing.Point(49, 23);
            this.cwbtnTiltAndRot_Rot0.Name = "cwbtnTiltAndRot_Rot0";
            this.cwbtnTiltAndRot_Rot0.OffImage = null;
            this.cwbtnTiltAndRot_Rot0.OnColor = System.Drawing.Color.Empty;
            this.cwbtnTiltAndRot_Rot0.OnImage = global::CT30K.Properties.Resources.LeftRotate;
            this.cwbtnTiltAndRot_Rot0.Size = new System.Drawing.Size(29, 29);
            this.cwbtnTiltAndRot_Rot0.TabIndex = 111;
            this.cwbtnTiltAndRot_Rot0.TabStop = false;
            this.cwbtnTiltAndRot_Rot0.Value = false;
            this.cwbtnTiltAndRot_Rot0.ValueChanged += new System.EventHandler(this.cwbtnTiltAndRot_Rot_ValueChanged);
            this.cwbtnTiltAndRot_Rot0.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cwbtnTiltAndRot_Rot_MouseUp);
            this.cwbtnTiltAndRot_Rot0.MouseCaptureChanged += new System.EventHandler(this.cwbtnTiltAndRot_Rot_MouseCaptureChanged);
            // 
            // cwbtnTiltAndRot_Rot1
            // 
            this.cwbtnTiltAndRot_Rot1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnTiltAndRot_Rot1.BlinkInterval = CT30K.CWSpeeds.cwSpeedFastest;
            this.cwbtnTiltAndRot_Rot1.Caption = "";
            this.cwbtnTiltAndRot_Rot1.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnTiltAndRot_Rot1.FlatAppearanceBorderSize = 1;
            this.cwbtnTiltAndRot_Rot1.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnTiltAndRot_Rot1.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnTiltAndRot_Rot1.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnTiltAndRot_Rot1.IsIgnorePerformClick = false;
            this.cwbtnTiltAndRot_Rot1.Location = new System.Drawing.Point(97, 23);
            this.cwbtnTiltAndRot_Rot1.Name = "cwbtnTiltAndRot_Rot1";
            this.cwbtnTiltAndRot_Rot1.OffImage = null;
            this.cwbtnTiltAndRot_Rot1.OnColor = System.Drawing.Color.Empty;
            this.cwbtnTiltAndRot_Rot1.OnImage = global::CT30K.Properties.Resources.RightRotate;
            this.cwbtnTiltAndRot_Rot1.Size = new System.Drawing.Size(29, 29);
            this.cwbtnTiltAndRot_Rot1.TabIndex = 112;
            this.cwbtnTiltAndRot_Rot1.TabStop = false;
            this.cwbtnTiltAndRot_Rot1.Value = false;
            this.cwbtnTiltAndRot_Rot1.ValueChanged += new System.EventHandler(this.cwbtnTiltAndRot_Rot_ValueChanged);
            this.cwbtnTiltAndRot_Rot1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cwbtnTiltAndRot_Rot_MouseUp);
            this.cwbtnTiltAndRot_Rot1.MouseCaptureChanged += new System.EventHandler(this.cwbtnTiltAndRot_Rot_MouseCaptureChanged);
            // 
            // cwbtnTiltAndRot_Tilt1
            // 
            this.cwbtnTiltAndRot_Tilt1.BackColor = System.Drawing.SystemColors.Control;
            this.cwbtnTiltAndRot_Tilt1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.cwbtnTiltAndRot_Tilt1.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnTiltAndRot_Tilt1.Caption = "";
            this.cwbtnTiltAndRot_Tilt1.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnTiltAndRot_Tilt1.FlatAppearanceBorderSize = 1;
            this.cwbtnTiltAndRot_Tilt1.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnTiltAndRot_Tilt1.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnTiltAndRot_Tilt1.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnTiltAndRot_Tilt1.IsIgnorePerformClick = false;
            this.cwbtnTiltAndRot_Tilt1.Location = new System.Drawing.Point(91, 72);
            this.cwbtnTiltAndRot_Tilt1.Name = "cwbtnTiltAndRot_Tilt1";
            this.cwbtnTiltAndRot_Tilt1.OffImage = null;
            this.cwbtnTiltAndRot_Tilt1.OnColor = System.Drawing.Color.Empty;
            this.cwbtnTiltAndRot_Tilt1.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnTiltAndRot_Tilt1.OnImage")));
            this.cwbtnTiltAndRot_Tilt1.Size = new System.Drawing.Size(44, 26);
            this.cwbtnTiltAndRot_Tilt1.TabIndex = 110;
            this.cwbtnTiltAndRot_Tilt1.Value = false;
            this.cwbtnTiltAndRot_Tilt1.ValueChanged += new System.EventHandler(this.cwbtnTiltAndRot_Tilt_ValueChanged);
            this.cwbtnTiltAndRot_Tilt1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cwbtnTiltAndRot_Tilt_MouseUp);
            this.cwbtnTiltAndRot_Tilt1.MouseCaptureChanged += new System.EventHandler(this.cwbtnTiltAndRot_Tilt_MouseCaptureChanged);
            // 
            // cwbtnTiltAndRot_Tilt0
            // 
            this.cwbtnTiltAndRot_Tilt0.BackColor = System.Drawing.SystemColors.Control;
            this.cwbtnTiltAndRot_Tilt0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.cwbtnTiltAndRot_Tilt0.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnTiltAndRot_Tilt0.Caption = "";
            this.cwbtnTiltAndRot_Tilt0.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnTiltAndRot_Tilt0.FlatAppearanceBorderSize = 1;
            this.cwbtnTiltAndRot_Tilt0.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnTiltAndRot_Tilt0.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnTiltAndRot_Tilt0.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnTiltAndRot_Tilt0.IsIgnorePerformClick = false;
            this.cwbtnTiltAndRot_Tilt0.Location = new System.Drawing.Point(43, 72);
            this.cwbtnTiltAndRot_Tilt0.Name = "cwbtnTiltAndRot_Tilt0";
            this.cwbtnTiltAndRot_Tilt0.OffImage = null;
            this.cwbtnTiltAndRot_Tilt0.OnColor = System.Drawing.Color.Empty;
            this.cwbtnTiltAndRot_Tilt0.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnTiltAndRot_Tilt0.OnImage")));
            this.cwbtnTiltAndRot_Tilt0.Size = new System.Drawing.Size(43, 26);
            this.cwbtnTiltAndRot_Tilt0.TabIndex = 109;
            this.cwbtnTiltAndRot_Tilt0.Value = false;
            this.cwbtnTiltAndRot_Tilt0.ValueChanged += new System.EventHandler(this.cwbtnTiltAndRot_Tilt_ValueChanged);
            this.cwbtnTiltAndRot_Tilt0.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cwbtnTiltAndRot_Tilt_MouseUp);
            this.cwbtnTiltAndRot_Tilt0.MouseCaptureChanged += new System.EventHandler(this.cwbtnTiltAndRot_Tilt_MouseCaptureChanged);
            // 
            // lblTilt
            // 
            this.lblTilt.AutoSize = true;
            this.lblTilt.Location = new System.Drawing.Point(8, 80);
            this.lblTilt.Name = "lblTilt";
            this.lblTilt.Size = new System.Drawing.Size(35, 12);
            this.lblTilt.TabIndex = 1;
            this.lblTilt.Tag = "22025";
            this.lblTilt.Text = "#傾斜";
            // 
            // lblRot
            // 
            this.lblRot.AutoSize = true;
            this.lblRot.Location = new System.Drawing.Point(8, 35);
            this.lblRot.Name = "lblRot";
            this.lblRot.Size = new System.Drawing.Size(35, 12);
            this.lblRot.TabIndex = 0;
            this.lblRot.Tag = "20161";
            this.lblRot.Text = "#回転";
            // 
            // ntbRotate
            // 
            this.ntbRotate.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ntbRotate.BackColor = System.Drawing.SystemColors.Control;
            this.ntbRotate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ntbRotate.Caption = "回転";
            this.ntbRotate.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbRotate.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbRotate.CaptionWidth = 65;
            this.ntbRotate.DiscreteInterval = 0.01F;
            this.ntbRotate.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbRotate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbRotate.IncDecButton = false;
            this.ntbRotate.Location = new System.Drawing.Point(8, 8);
            this.ntbRotate.Max = new decimal(new int[] {
            999990,
            0,
            0,
            196608});
            this.ntbRotate.Min = new decimal(new int[] {
            999990,
            0,
            0,
            -2147287040});
            this.ntbRotate.Name = "ntbRotate";
            this.ntbRotate.ReadOnly = true;
            this.ntbRotate.Size = new System.Drawing.Size(168, 20);
            this.ntbRotate.TabIndex = 58;
            this.ntbRotate.TextBackColor = System.Drawing.SystemColors.Control;
            this.ntbRotate.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbRotate.Unit = "度";
            this.ntbRotate.Value = new decimal(new int[] {
            99999,
            0,
            0,
            131072});
            this.ntbRotate.ValueChanged += new CT30K.NumTextBox.ValueChangedEventHandler(this.ntbRotate_ValueChanged);
            // 
            // ntbUpDown
            // 
            this.ntbUpDown.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ntbUpDown.BackColor = System.Drawing.SystemColors.Control;
            this.ntbUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ntbUpDown.Caption = "高さ";
            this.ntbUpDown.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbUpDown.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbUpDown.CaptionWidth = 65;
            this.ntbUpDown.DiscreteInterval = 0.001F;
            this.ntbUpDown.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbUpDown.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbUpDown.IncDecButton = false;
            this.ntbUpDown.Location = new System.Drawing.Point(8, 32);
            this.ntbUpDown.Max = new decimal(new int[] {
            999999,
            0,
            0,
            196608});
            this.ntbUpDown.Min = new decimal(new int[] {
            999999,
            0,
            0,
            -2147287040});
            this.ntbUpDown.Name = "ntbUpDown";
            this.ntbUpDown.ReadOnly = true;
            this.ntbUpDown.Size = new System.Drawing.Size(174, 20);
            this.ntbUpDown.TabIndex = 59;
            this.ntbUpDown.TextBackColor = System.Drawing.SystemColors.Control;
            this.ntbUpDown.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbUpDown.Unit = "mm";
            this.ntbUpDown.Value = new decimal(new int[] {
            999999,
            0,
            0,
            196608});
            this.ntbUpDown.ValueChanged += new CT30K.NumTextBox.ValueChangedEventHandler(this.ntbUpDown_ValueChanged);
            // 
            // cboSpeed0
            // 
            this.cboSpeed0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpeed0.FormattingEnabled = true;
            this.cboSpeed0.Items.AddRange(new object[] {
            "Slow",
            "Middle",
            "Fast",
            "Manual"});
            this.cboSpeed0.Location = new System.Drawing.Point(192, 8);
            this.cboSpeed0.Name = "cboSpeed0";
            this.cboSpeed0.Size = new System.Drawing.Size(81, 20);
            this.cboSpeed0.TabIndex = 71;
            this.cboSpeed0.SelectedIndexChanged += new System.EventHandler(this.cboSpeed_SelectedIndexChanged);
            // 
            // cboSpeed1
            // 
            this.cboSpeed1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpeed1.FormattingEnabled = true;
            this.cboSpeed1.Items.AddRange(new object[] {
            "Slow",
            "Middle",
            "Fast",
            "Manual"});
            this.cboSpeed1.Location = new System.Drawing.Point(192, 32);
            this.cboSpeed1.Name = "cboSpeed1";
            this.cboSpeed1.Size = new System.Drawing.Size(81, 20);
            this.cboSpeed1.TabIndex = 72;
            this.cboSpeed1.SelectedIndexChanged += new System.EventHandler(this.cboSpeed_SelectedIndexChanged);
            // 
            // cboSpeed2
            // 
            this.cboSpeed2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpeed2.FormattingEnabled = true;
            this.cboSpeed2.Items.AddRange(new object[] {
            "Slow",
            "Middle",
            "Fast",
            "Manual"});
            this.cboSpeed2.Location = new System.Drawing.Point(192, 56);
            this.cboSpeed2.Name = "cboSpeed2";
            this.cboSpeed2.Size = new System.Drawing.Size(81, 20);
            this.cboSpeed2.TabIndex = 73;
            this.cboSpeed2.SelectedIndexChanged += new System.EventHandler(this.cboSpeed_SelectedIndexChanged);
            // 
            // cboSpeed6
            // 
            this.cboSpeed6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpeed6.FormattingEnabled = true;
            this.cboSpeed6.Items.AddRange(new object[] {
            "Slow",
            "Middle",
            "Fast",
            "Manual"});
            this.cboSpeed6.Location = new System.Drawing.Point(192, 80);
            this.cboSpeed6.Name = "cboSpeed6";
            this.cboSpeed6.Size = new System.Drawing.Size(81, 20);
            this.cboSpeed6.TabIndex = 74;
            this.cboSpeed6.SelectedIndexChanged += new System.EventHandler(this.cboSpeed_SelectedIndexChanged);
            // 
            // cboSpeed3
            // 
            this.cboSpeed3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpeed3.FormattingEnabled = true;
            this.cboSpeed3.Items.AddRange(new object[] {
            "Slow",
            "Middle",
            "Fast",
            "Manual"});
            this.cboSpeed3.Location = new System.Drawing.Point(192, 104);
            this.cboSpeed3.Name = "cboSpeed3";
            this.cboSpeed3.Size = new System.Drawing.Size(81, 20);
            this.cboSpeed3.TabIndex = 75;
            this.cboSpeed3.SelectedIndexChanged += new System.EventHandler(this.cboSpeed_SelectedIndexChanged);
            // 
            // cboSpeed4
            // 
            this.cboSpeed4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpeed4.FormattingEnabled = true;
            this.cboSpeed4.Items.AddRange(new object[] {
            "Slow",
            "Middle",
            "Fast",
            "Manual"});
            this.cboSpeed4.Location = new System.Drawing.Point(192, 141);
            this.cboSpeed4.Name = "cboSpeed4";
            this.cboSpeed4.Size = new System.Drawing.Size(81, 20);
            this.cboSpeed4.TabIndex = 76;
            this.cboSpeed4.SelectedIndexChanged += new System.EventHandler(this.cboSpeed_SelectedIndexChanged);
            // 
            // cboSpeed7
            // 
            this.cboSpeed7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpeed7.FormattingEnabled = true;
            this.cboSpeed7.Items.AddRange(new object[] {
            "Slow",
            "Middle",
            "Fast",
            "Manual"});
            this.cboSpeed7.Location = new System.Drawing.Point(192, 224);
            this.cboSpeed7.Name = "cboSpeed7";
            this.cboSpeed7.Size = new System.Drawing.Size(81, 20);
            this.cboSpeed7.TabIndex = 68;
            this.cboSpeed7.SelectedIndexChanged += new System.EventHandler(this.cboSpeed_SelectedIndexChanged);
            // 
            // cboSpeed8
            // 
            this.cboSpeed8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpeed8.FormattingEnabled = true;
            this.cboSpeed8.Items.AddRange(new object[] {
            "Slow",
            "Middle",
            "Fast",
            "Manual"});
            this.cboSpeed8.Location = new System.Drawing.Point(192, 248);
            this.cboSpeed8.Name = "cboSpeed8";
            this.cboSpeed8.Size = new System.Drawing.Size(81, 20);
            this.cboSpeed8.TabIndex = 70;
            this.cboSpeed8.SelectedIndexChanged += new System.EventHandler(this.cboSpeed_SelectedIndexChanged);
            // 
            // cboSpeed9
            // 
            this.cboSpeed9.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpeed9.FormattingEnabled = true;
            this.cboSpeed9.Items.AddRange(new object[] {
            "Slow",
            "Middle",
            "Fast",
            "Manual"});
            this.cboSpeed9.Location = new System.Drawing.Point(192, 272);
            this.cboSpeed9.Name = "cboSpeed9";
            this.cboSpeed9.Size = new System.Drawing.Size(81, 20);
            this.cboSpeed9.TabIndex = 69;
            this.cboSpeed9.SelectedIndexChanged += new System.EventHandler(this.cboSpeed_SelectedIndexChanged);
            // 
            // ntbFCD
            // 
            this.ntbFCD.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ntbFCD.BackColor = System.Drawing.SystemColors.Control;
            this.ntbFCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ntbFCD.Caption = "FCD";
            this.ntbFCD.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbFCD.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbFCD.CaptionWidth = 65;
            this.ntbFCD.DiscreteInterval = 0.1F;
            this.ntbFCD.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbFCD.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbFCD.IncDecButton = false;
            this.ntbFCD.Location = new System.Drawing.Point(8, 56);
            this.ntbFCD.Max = new decimal(new int[] {
            999999,
            0,
            0,
            65536});
            this.ntbFCD.Min = new decimal(new int[] {
            999999,
            0,
            0,
            -2147418112});
            this.ntbFCD.Name = "ntbFCD";
            this.ntbFCD.ReadOnly = true;
            this.ntbFCD.Size = new System.Drawing.Size(174, 20);
            this.ntbFCD.TabIndex = 60;
            this.ntbFCD.TextBackColor = System.Drawing.SystemColors.Control;
            this.ntbFCD.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbFCD.Unit = "mm";
            this.ntbFCD.Value = new decimal(new int[] {
            999999,
            0,
            0,
            65536});
            this.ntbFCD.ValueChanged += new CT30K.NumTextBox.ValueChangedEventHandler(this.ntbFCD_ValueChanged);
            // 
            // ntbFID
            // 
            this.ntbFID.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ntbFID.BackColor = System.Drawing.SystemColors.Control;
            this.ntbFID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ntbFID.Caption = "FID";
            this.ntbFID.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbFID.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbFID.CaptionWidth = 65;
            this.ntbFID.DiscreteInterval = 0.1F;
            this.ntbFID.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbFID.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbFID.IncDecButton = false;
            this.ntbFID.Location = new System.Drawing.Point(8, 80);
            this.ntbFID.Max = new decimal(new int[] {
            999999,
            0,
            0,
            65536});
            this.ntbFID.Min = new decimal(new int[] {
            999999,
            0,
            0,
            -2147418112});
            this.ntbFID.Name = "ntbFID";
            this.ntbFID.ReadOnly = true;
            this.ntbFID.Size = new System.Drawing.Size(174, 20);
            this.ntbFID.TabIndex = 67;
            this.ntbFID.TextBackColor = System.Drawing.SystemColors.Control;
            this.ntbFID.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbFID.Unit = "mm";
            this.ntbFID.Value = new decimal(new int[] {
            999999,
            0,
            0,
            65536});
            this.ntbFID.ValueChanged += new CT30K.NumTextBox.ValueChangedEventHandler(this.ntbFID_ValueChanged);
            // 
            // ntbTableXPos
            // 
            this.ntbTableXPos.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ntbTableXPos.BackColor = System.Drawing.SystemColors.Control;
            this.ntbTableXPos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ntbTableXPos.Caption = "Y軸";
            this.ntbTableXPos.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbTableXPos.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbTableXPos.CaptionWidth = 65;
            this.ntbTableXPos.DiscreteInterval = 0.01F;
            this.ntbTableXPos.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbTableXPos.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbTableXPos.IncDecButton = false;
            this.ntbTableXPos.Location = new System.Drawing.Point(8, 104);
            this.ntbTableXPos.Max = new decimal(new int[] {
            9999990,
            0,
            0,
            196608});
            this.ntbTableXPos.Min = new decimal(new int[] {
            9999990,
            0,
            0,
            -2147287040});
            this.ntbTableXPos.Name = "ntbTableXPos";
            this.ntbTableXPos.ReadOnly = true;
            this.ntbTableXPos.Size = new System.Drawing.Size(174, 20);
            this.ntbTableXPos.TabIndex = 66;
            this.ntbTableXPos.TextBackColor = System.Drawing.SystemColors.Control;
            this.ntbTableXPos.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbTableXPos.Unit = "mm";
            this.ntbTableXPos.Value = new decimal(new int[] {
            999999,
            0,
            0,
            131072});
            this.ntbTableXPos.ValueChanged += new CT30K.NumTextBox.ValueChangedEventHandler(this.ntbTableXPos_ValueChanged);
            // 
            // ntbFTablePosX
            // 
            this.ntbFTablePosX.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ntbFTablePosX.BackColor = System.Drawing.SystemColors.Control;
            this.ntbFTablePosX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ntbFTablePosX.Caption = "微調X軸";
            this.ntbFTablePosX.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbFTablePosX.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbFTablePosX.CaptionWidth = 65;
            this.ntbFTablePosX.DiscreteInterval = 0.01F;
            this.ntbFTablePosX.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbFTablePosX.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbFTablePosX.IncDecButton = false;
            this.ntbFTablePosX.Location = new System.Drawing.Point(8, 128);
            this.ntbFTablePosX.Max = new decimal(new int[] {
            9999990,
            0,
            0,
            196608});
            this.ntbFTablePosX.Min = new decimal(new int[] {
            9999990,
            0,
            0,
            -2147287040});
            this.ntbFTablePosX.Name = "ntbFTablePosX";
            this.ntbFTablePosX.ReadOnly = true;
            this.ntbFTablePosX.Size = new System.Drawing.Size(174, 20);
            this.ntbFTablePosX.TabIndex = 62;
            this.ntbFTablePosX.TextBackColor = System.Drawing.SystemColors.Control;
            this.ntbFTablePosX.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbFTablePosX.Unit = "mm";
            this.ntbFTablePosX.Value = new decimal(new int[] {
            999999,
            0,
            0,
            131072});
            this.ntbFTablePosX.ValueChanged += new CT30K.NumTextBox.ValueChangedEventHandler(this.ntbFTablePosX_ValueChanged);
            // 
            // ntbFTablePosY
            // 
            this.ntbFTablePosY.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ntbFTablePosY.BackColor = System.Drawing.SystemColors.Control;
            this.ntbFTablePosY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ntbFTablePosY.Caption = "微調Y軸";
            this.ntbFTablePosY.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbFTablePosY.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbFTablePosY.CaptionWidth = 65;
            this.ntbFTablePosY.DiscreteInterval = 0.01F;
            this.ntbFTablePosY.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbFTablePosY.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbFTablePosY.IncDecButton = false;
            this.ntbFTablePosY.Location = new System.Drawing.Point(8, 152);
            this.ntbFTablePosY.Max = new decimal(new int[] {
            9999990,
            0,
            0,
            196608});
            this.ntbFTablePosY.Min = new decimal(new int[] {
            9999990,
            0,
            0,
            -2147287040});
            this.ntbFTablePosY.Name = "ntbFTablePosY";
            this.ntbFTablePosY.ReadOnly = true;
            this.ntbFTablePosY.Size = new System.Drawing.Size(174, 20);
            this.ntbFTablePosY.TabIndex = 63;
            this.ntbFTablePosY.TextBackColor = System.Drawing.SystemColors.Control;
            this.ntbFTablePosY.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbFTablePosY.Unit = "mm";
            this.ntbFTablePosY.Value = new decimal(new int[] {
            999999,
            0,
            0,
            131072});
            this.ntbFTablePosY.ValueChanged += new CT30K.NumTextBox.ValueChangedEventHandler(this.ntbFTablePosY_ValueChanged);
            // 
            // ntbXrayRotPos
            // 
            this.ntbXrayRotPos.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ntbXrayRotPos.BackColor = System.Drawing.SystemColors.Control;
            this.ntbXrayRotPos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ntbXrayRotPos.Caption = "Ｘ線管回転";
            this.ntbXrayRotPos.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbXrayRotPos.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbXrayRotPos.CaptionWidth = 65;
            this.ntbXrayRotPos.DiscreteInterval = 0.01F;
            this.ntbXrayRotPos.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbXrayRotPos.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbXrayRotPos.IncDecButton = false;
            this.ntbXrayRotPos.Location = new System.Drawing.Point(8, 224);
            this.ntbXrayRotPos.Max = new decimal(new int[] {
            999990,
            0,
            0,
            196608});
            this.ntbXrayRotPos.Min = new decimal(new int[] {
            999990,
            0,
            0,
            -2147287040});
            this.ntbXrayRotPos.Name = "ntbXrayRotPos";
            this.ntbXrayRotPos.ReadOnly = true;
            this.ntbXrayRotPos.Size = new System.Drawing.Size(168, 20);
            this.ntbXrayRotPos.TabIndex = 61;
            this.ntbXrayRotPos.TextBackColor = System.Drawing.SystemColors.Control;
            this.ntbXrayRotPos.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbXrayRotPos.Unit = "度";
            this.ntbXrayRotPos.Value = new decimal(new int[] {
            99999,
            0,
            0,
            131072});
            // 
            // ntbXrayPosX
            // 
            this.ntbXrayPosX.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ntbXrayPosX.BackColor = System.Drawing.SystemColors.Control;
            this.ntbXrayPosX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ntbXrayPosX.Caption = "Ｘ線管X軸";
            this.ntbXrayPosX.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbXrayPosX.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbXrayPosX.CaptionWidth = 65;
            this.ntbXrayPosX.DiscreteInterval = 0.1F;
            this.ntbXrayPosX.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbXrayPosX.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbXrayPosX.IncDecButton = false;
            this.ntbXrayPosX.Location = new System.Drawing.Point(8, 248);
            this.ntbXrayPosX.Max = new decimal(new int[] {
            999999,
            0,
            0,
            65536});
            this.ntbXrayPosX.Min = new decimal(new int[] {
            999999,
            0,
            0,
            -2147418112});
            this.ntbXrayPosX.Name = "ntbXrayPosX";
            this.ntbXrayPosX.ReadOnly = true;
            this.ntbXrayPosX.Size = new System.Drawing.Size(174, 20);
            this.ntbXrayPosX.TabIndex = 64;
            this.ntbXrayPosX.TextBackColor = System.Drawing.SystemColors.Control;
            this.ntbXrayPosX.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbXrayPosX.Unit = "mm";
            this.ntbXrayPosX.Value = new decimal(new int[] {
            999999,
            0,
            0,
            65536});
            // 
            // ntbXrayPosY
            // 
            this.ntbXrayPosY.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ntbXrayPosY.BackColor = System.Drawing.SystemColors.Control;
            this.ntbXrayPosY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ntbXrayPosY.Caption = "Ｘ線管Y軸";
            this.ntbXrayPosY.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbXrayPosY.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbXrayPosY.CaptionWidth = 65;
            this.ntbXrayPosY.DiscreteInterval = 0.01F;
            this.ntbXrayPosY.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbXrayPosY.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbXrayPosY.IncDecButton = false;
            this.ntbXrayPosY.Location = new System.Drawing.Point(8, 272);
            this.ntbXrayPosY.Max = new decimal(new int[] {
            9999990,
            0,
            0,
            196608});
            this.ntbXrayPosY.Min = new decimal(new int[] {
            9999990,
            0,
            0,
            -2147287040});
            this.ntbXrayPosY.Name = "ntbXrayPosY";
            this.ntbXrayPosY.ReadOnly = true;
            this.ntbXrayPosY.Size = new System.Drawing.Size(174, 17);
            this.ntbXrayPosY.TabIndex = 65;
            this.ntbXrayPosY.TextBackColor = System.Drawing.SystemColors.Control;
            this.ntbXrayPosY.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbXrayPosY.Unit = "mm";
            this.ntbXrayPosY.Value = new decimal(new int[] {
            999999,
            0,
            0,
            131072});
            // 
            // ntbTiltRot
            // 
            this.ntbTiltRot.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ntbTiltRot.BackColor = System.Drawing.SystemColors.Control;
            this.ntbTiltRot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ntbTiltRot.Caption = "回転(傾斜)";
            this.ntbTiltRot.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbTiltRot.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbTiltRot.CaptionWidth = 65;
            this.ntbTiltRot.DiscreteInterval = 0.01F;
            this.ntbTiltRot.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbTiltRot.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbTiltRot.IncDecButton = false;
            this.ntbTiltRot.Location = new System.Drawing.Point(8, 200);
            this.ntbTiltRot.Max = new decimal(new int[] {
            999990,
            0,
            0,
            196608});
            this.ntbTiltRot.Min = new decimal(new int[] {
            999990,
            0,
            0,
            -2147287040});
            this.ntbTiltRot.Name = "ntbTiltRot";
            this.ntbTiltRot.ReadOnly = true;
            this.ntbTiltRot.Size = new System.Drawing.Size(168, 20);
            this.ntbTiltRot.TabIndex = 78;
            this.ntbTiltRot.TextBackColor = System.Drawing.SystemColors.Control;
            this.ntbTiltRot.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbTiltRot.Unit = "度";
            this.ntbTiltRot.Value = new decimal(new int[] {
            99999,
            0,
            0,
            131072});
            this.ntbTiltRot.ValueChanged += new CT30K.NumTextBox.ValueChangedEventHandler(this.ntbTiltRot_ValueChanged);
            // 
            // cboSpeed10
            // 
            this.cboSpeed10.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpeed10.FormattingEnabled = true;
            this.cboSpeed10.Items.AddRange(new object[] {
            "Slow",
            "Middle",
            "Fast",
            "Manual"});
            this.cboSpeed10.Location = new System.Drawing.Point(192, 200);
            this.cboSpeed10.Name = "cboSpeed10";
            this.cboSpeed10.Size = new System.Drawing.Size(81, 20);
            this.cboSpeed10.TabIndex = 79;
            this.cboSpeed10.SelectedIndexChanged += new System.EventHandler(this.cboSpeed_SelectedIndexChanged);
            // 
            // cboSpeed11
            // 
            this.cboSpeed11.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpeed11.FormattingEnabled = true;
            this.cboSpeed11.Items.AddRange(new object[] {
            "Slow",
            "Middle",
            "Fast",
            "Manual"});
            this.cboSpeed11.Location = new System.Drawing.Point(192, 176);
            this.cboSpeed11.Name = "cboSpeed11";
            this.cboSpeed11.Size = new System.Drawing.Size(81, 20);
            this.cboSpeed11.TabIndex = 80;
            this.cboSpeed11.SelectedIndexChanged += new System.EventHandler(this.cboSpeed_SelectedIndexChanged);
            // 
            // ntbTilt
            // 
            this.ntbTilt.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ntbTilt.BackColor = System.Drawing.SystemColors.Control;
            this.ntbTilt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ntbTilt.Caption = "傾斜";
            this.ntbTilt.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbTilt.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbTilt.CaptionWidth = 65;
            this.ntbTilt.DiscreteInterval = 0.01F;
            this.ntbTilt.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbTilt.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbTilt.IncDecButton = false;
            this.ntbTilt.Location = new System.Drawing.Point(8, 176);
            this.ntbTilt.Max = new decimal(new int[] {
            999990,
            0,
            0,
            196608});
            this.ntbTilt.Min = new decimal(new int[] {
            999990,
            0,
            0,
            -2147287040});
            this.ntbTilt.Name = "ntbTilt";
            this.ntbTilt.ReadOnly = true;
            this.ntbTilt.Size = new System.Drawing.Size(168, 20);
            this.ntbTilt.TabIndex = 81;
            this.ntbTilt.TextBackColor = System.Drawing.SystemColors.Control;
            this.ntbTilt.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbTilt.Unit = "度";
            this.ntbTilt.Value = new decimal(new int[] {
            99999,
            0,
            0,
            131072});
            this.ntbTilt.ValueChanged += new CT30K.NumTextBox.ValueChangedEventHandler(this.ntbTilt_ValueChanged);
            // 
            // fraMechaPos
            // 
            this.fraMechaPos.Controls.Add(this.ntbTilt);
            this.fraMechaPos.Controls.Add(this.cboSpeed11);
            this.fraMechaPos.Controls.Add(this.cboSpeed10);
            this.fraMechaPos.Controls.Add(this.ntbTiltRot);
            this.fraMechaPos.Controls.Add(this.ntbXrayPosY);
            this.fraMechaPos.Controls.Add(this.ntbXrayPosX);
            this.fraMechaPos.Controls.Add(this.ntbXrayRotPos);
            this.fraMechaPos.Controls.Add(this.ntbFTablePosY);
            this.fraMechaPos.Controls.Add(this.ntbFTablePosX);
            this.fraMechaPos.Controls.Add(this.ntbTableXPos);
            this.fraMechaPos.Controls.Add(this.ntbFID);
            this.fraMechaPos.Controls.Add(this.ntbFCD);
            this.fraMechaPos.Controls.Add(this.cboSpeed9);
            this.fraMechaPos.Controls.Add(this.cboSpeed8);
            this.fraMechaPos.Controls.Add(this.cboSpeed7);
            this.fraMechaPos.Controls.Add(this.cboSpeed4);
            this.fraMechaPos.Controls.Add(this.cboSpeed3);
            this.fraMechaPos.Controls.Add(this.cboSpeed6);
            this.fraMechaPos.Controls.Add(this.cboSpeed2);
            this.fraMechaPos.Controls.Add(this.cboSpeed1);
            this.fraMechaPos.Controls.Add(this.cboSpeed0);
            this.fraMechaPos.Controls.Add(this.ntbUpDown);
            this.fraMechaPos.Controls.Add(this.ntbRotate);
            this.fraMechaPos.Location = new System.Drawing.Point(0, 0);
            this.fraMechaPos.Name = "fraMechaPos";
            this.fraMechaPos.Size = new System.Drawing.Size(281, 295);
            this.fraMechaPos.TabIndex = 40;
            // 
            // fraChangeXray
            // 
            this.fraChangeXray.Controls.Add(this.cwbtnChangeXray1);
            this.fraChangeXray.Controls.Add(this.lblChangeXray);
            this.fraChangeXray.Controls.Add(this.cwbtnChangeXray0);
            this.fraChangeXray.Location = new System.Drawing.Point(0, 572);
            this.fraChangeXray.Name = "fraChangeXray";
            this.fraChangeXray.Size = new System.Drawing.Size(265, 65);
            this.fraChangeXray.TabIndex = 53;
            this.fraChangeXray.Visible = false;
            // 
            // cwbtnChangeXray1
            // 
            this.cwbtnChangeXray1.BackColor = System.Drawing.Color.Green;
            this.cwbtnChangeXray1.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnChangeXray1.Caption = "#160kV";
            this.cwbtnChangeXray1.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnChangeXray1.FlatAppearanceBorderSize = 1;
            this.cwbtnChangeXray1.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnChangeXray1.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnChangeXray1.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnChangeXray1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwbtnChangeXray1.IsIgnorePerformClick = false;
            this.cwbtnChangeXray1.Location = new System.Drawing.Point(135, 15);
            this.cwbtnChangeXray1.Name = "cwbtnChangeXray1";
            this.cwbtnChangeXray1.OffImage = null;
            this.cwbtnChangeXray1.OnColor = System.Drawing.Color.Lime;
            this.cwbtnChangeXray1.OnImage = null;
            this.cwbtnChangeXray1.Size = new System.Drawing.Size(120, 40);
            this.cwbtnChangeXray1.TabIndex = 30;
            this.cwbtnChangeXray1.Value = false;
            this.cwbtnChangeXray1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cwbtnChangeXray_Click);
            // 
            // lblChangeXray
            // 
            this.lblChangeXray.AutoSize = true;
            this.lblChangeXray.Location = new System.Drawing.Point(3, 3);
            this.lblChangeXray.Name = "lblChangeXray";
            this.lblChangeXray.Size = new System.Drawing.Size(101, 12);
            this.lblChangeXray.TabIndex = 104;
            this.lblChangeXray.Text = "#X線1⇔X線2 切替";
            this.lblChangeXray.Visible = false;
            // 
            // cwbtnChangeXray0
            // 
            this.cwbtnChangeXray0.BackColor = System.Drawing.Color.Green;
            this.cwbtnChangeXray0.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnChangeXray0.Caption = "#300kV";
            this.cwbtnChangeXray0.FlatAppearanceBorderColor = System.Drawing.Color.Empty;
            this.cwbtnChangeXray0.FlatAppearanceBorderSize = 1;
            this.cwbtnChangeXray0.FlatAppearanceMouseDownBackColor = System.Drawing.Color.Empty;
            this.cwbtnChangeXray0.FlatAppearanceMouseOverBackColor = System.Drawing.Color.Empty;
            this.cwbtnChangeXray0.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cwbtnChangeXray0.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwbtnChangeXray0.IsIgnorePerformClick = false;
            this.cwbtnChangeXray0.Location = new System.Drawing.Point(9, 15);
            this.cwbtnChangeXray0.Name = "cwbtnChangeXray0";
            this.cwbtnChangeXray0.OffImage = null;
            this.cwbtnChangeXray0.OnColor = System.Drawing.Color.Lime;
            this.cwbtnChangeXray0.OnImage = null;
            this.cwbtnChangeXray0.Size = new System.Drawing.Size(120, 40);
            this.cwbtnChangeXray0.TabIndex = 29;
            this.cwbtnChangeXray0.Value = false;
            this.cwbtnChangeXray0.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cwbtnChangeXray_Click);
            // 
            // frmMechaControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(838, 649);
            this.Controls.Add(this.fraScanArea);
            this.Controls.Add(this.fraMechaScanCondiion);
            this.Controls.Add(this.fraAutoScanPos);
            this.Controls.Add(this.fraHighSpeedCamera);
            this.Controls.Add(this.fraChangeXray);
            this.Controls.Add(this.fraTiltAndRot);
            this.Controls.Add(this.fraChangeDetector);
            this.Controls.Add(this.fraMechaControl);
            this.Controls.Add(this.fraUpDown);
            this.Controls.Add(this.fraMechaPos);
            this.Controls.Add(this.cmdDetails);
            this.Controls.Add(this.cmdMechaAllReset);
            this.Controls.Add(this.fraIris);
            this.Controls.Add(this.fraXrayRotate);
            this.Controls.Add(this.fraCollimator);
            this.Controls.Add(this.fraError);
            this.Name = "frmMechaControl";
            this.ShowInTaskbar = false;
            this.Activated += new System.EventHandler(this.frmMechaControl_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_FormClosed);
            this.Load += new System.EventHandler(this.frmMechaControl_Load);
            this.fraIris.ResumeLayout(false);
            this.fraXrayRotate.ResumeLayout(false);
            this.fraCollimator.ResumeLayout(false);
            this.fraError.ResumeLayout(false);
            this.fraError.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneEXMErrCode)).EndInit();
            this.fraHighSpeedCamera.ResumeLayout(false);
            this.fraHighSpeedCamera.PerformLayout();
            this.fraAutoScanPos.ResumeLayout(false);
            this.fraMechaScanCondiion.ResumeLayout(false);
            this.fraMechaScanCondiion.PerformLayout();
            this.fraScanArea.ResumeLayout(false);
            this.fraUpDown.ResumeLayout(false);
            this.fraUpDown.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwnePos)).EndInit();
            this.fraMechaControl.ResumeLayout(false);
            this.fraMechaControl.PerformLayout();
            this.fraIIMove.ResumeLayout(false);
            this.fraIIMove.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImgXrayTube)).EndInit();
            this.fraTableRestriction.ResumeLayout(false);
            this.fraChangeDetector.ResumeLayout(false);
            this.fraChangeDetector.PerformLayout();
            this.fraTiltAndRot.ResumeLayout(false);
            this.fraTiltAndRot.PerformLayout();
            this.fraMechaPos.ResumeLayout(false);
            this.fraChangeXray.ResumeLayout(false);
            this.fraChangeXray.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.GroupBox fraError;
        private System.Windows.Forms.GroupBox fraXrayRotate;
		private System.Windows.Forms.Button cmdCollimator0;
		private System.Windows.Forms.Button cmdCollimator6;
		private System.Windows.Forms.Button cmdCollimator4;
		private System.Windows.Forms.Button cmdCollimator1;
		private System.Windows.Forms.Button cmdCollimator7;
		private System.Windows.Forms.Button cmdCollimator5;
		private System.Windows.Forms.Button cmdCollimator2;
		private System.Windows.Forms.Button cmdCollimator3;
		private System.Windows.Forms.NumericUpDown cwneEXMErrCode;
		private System.Windows.Forms.Label lblEXMErrCodeHeader;
		private System.Windows.Forms.Label lblMechaStatus5;
		private System.Windows.Forms.Label lblMechaStatus4;
		private System.Windows.Forms.Label lblMechaStatus3;
		private System.Windows.Forms.Label lblMechaStatus2;
		private System.Windows.Forms.Label lblMechaStatus1;
        private System.Windows.Forms.Label lblMechaStatus0;
		private CWButton cwbtnMove7;
        private CWButton cwbtnMove8;
		private CWButton cwbtnMove9;
		private CWButton cwbtnRotateXray1;
		private CWButton cwbtnRotateXray0;
        private System.Windows.Forms.Button cmdMechaAllReset;
		private System.Windows.Forms.Button cmdIris0;
		private System.Windows.Forms.Button cmdIris5;
		private System.Windows.Forms.Button cmdIris2;
		private System.Windows.Forms.Button cmdIris7;
		private System.Windows.Forms.Button cmdIris6;
		private System.Windows.Forms.Button cmdIris1;
		private System.Windows.Forms.Button cmdIris4;
        private System.Windows.Forms.Button cmdIris3;
        private System.Windows.Forms.ToolTip toolTip;
        internal System.Windows.Forms.ImageList ImageList1;
		internal System.Windows.Forms.Button cmdFromTrans;
		internal System.Windows.Forms.Timer tmrPIOCheck;
		internal System.Windows.Forms.Timer tmrMecainf;
        internal System.Windows.Forms.Timer tmrSeqComm;
        internal System.Windows.Forms.Button cmdFromSlice;
        internal System.Windows.Forms.Button cmdDetails;
        private System.Windows.Forms.Label lblUP;
        private System.Windows.Forms.Label lblDOWN;
        private System.Windows.Forms.Button cmdPosExec;
        private System.Windows.Forms.Label lblMax;
        private System.Windows.Forms.Label lblMiddle;
        private System.Windows.Forms.Label lblMin;
        private CWButton cwbtnUpDown0;
        private CWButton cwbtnUpDown1;
        private System.Windows.Forms.Label Label22;
        private CWButton cwbtnFineTable0;
        private System.Windows.Forms.Label lblFTable;
        private System.Windows.Forms.Label lblRot1;
        private System.Windows.Forms.Label lblRot3;
        private System.Windows.Forms.Label lblRot2;
        private System.Windows.Forms.Label lblRot0;
        private CTCheckBox ctchkRotate2;
        private CTCheckBox ctchkRotate1;
        private CTCheckBox ctchkRotate3;
        private CWButton cwbtnFineTable1;
        internal System.Windows.Forms.Panel fraIIMove;
        private System.Windows.Forms.Label lblIIMove3;
        private System.Windows.Forms.Label lblIIMove2;
        private System.Windows.Forms.Label lblIIMove1;
        private System.Windows.Forms.Label lblIIMove0;
        private CTCheckBox ctchkIIMove3;
        private CTCheckBox ctchkIIMove2;
        private CTCheckBox ctchkIIMove1;
        private CTCheckBox ctchkIIMove0;
        private CWButton cwbtnRotate1;
        private CWButton cwbtnRotate0;
        private CWButton cwbtnMove1;
        private CWButton cwbtnMove0;
        private CWButton cwbtnMove3;
        private CWButton cwbtnMove2;
        private System.Windows.Forms.Label lblXrayTube;
        internal System.Windows.Forms.PictureBox ImgXrayTube;
        private System.Windows.Forms.GroupBox fraTableRestriction;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
        private Microsoft.VisualBasic.PowerPacks.OvalShape ShpFTable;
        private Microsoft.VisualBasic.PowerPacks.OvalShape ShpTable;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape ShpCabinet;
        private System.Windows.Forms.Panel fraHighSpeedCamera;
        private System.Windows.Forms.Panel fraChangeDetector;
        private CTCheckBox ctchkRotate0;
        internal System.Windows.Forms.Panel pnlDetShift;
        internal CWButton cwbtnDetShift;
        internal System.Windows.Forms.Label lblXrayII;
        internal System.Windows.Forms.Timer tmrTryReloadForms;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private CWButton cwbtnChangeMode0;
        private CWButton cwbtnChangeMode1;
        private CWButton cwbtnChangeDet1;
        private CWButton cwbtnChangeDet0;
        internal System.Windows.Forms.Button cmdTableMovePermit;
        internal System.Windows.Forms.Button cmdTableMoveRestrict;
        private CTSliderVScroll ctSliderVScroll1;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineMax;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineMiddle;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineMin;
        public System.Windows.Forms.NumericUpDown cwnePos;
        private System.Windows.Forms.GroupBox fraTiltAndRot;
        private System.Windows.Forms.Label lblTilt;
        private System.Windows.Forms.Label lblRot;
        private CWButton cwbtnTiltAndRot_Tilt1;
        private CWButton cwbtnTiltAndRot_Tilt0;
        private CWButton cwbtnTiltAndRot_Rot0;
        private CWButton cwbtnTiltAndRot_Rot1;
        private System.Windows.Forms.Label lblMechaStatus7;
        private System.Windows.Forms.Label lblMechaStatus6;
        internal NumTextBox ntbRotate;
        internal NumTextBox ntbUpDown;
        private System.Windows.Forms.ComboBox cboSpeed0;
        private System.Windows.Forms.ComboBox cboSpeed1;
        private System.Windows.Forms.ComboBox cboSpeed2;
        private System.Windows.Forms.ComboBox cboSpeed6;
        private System.Windows.Forms.ComboBox cboSpeed3;
        private System.Windows.Forms.ComboBox cboSpeed4;
        private System.Windows.Forms.ComboBox cboSpeed7;
        private System.Windows.Forms.ComboBox cboSpeed8;
        private System.Windows.Forms.ComboBox cboSpeed9;
        internal NumTextBox ntbFCD;
        internal NumTextBox ntbFID;
        internal NumTextBox ntbTableXPos;
        internal NumTextBox ntbFTablePosX;
        internal NumTextBox ntbFTablePosY;
        private NumTextBox ntbXrayRotPos;
        private NumTextBox ntbXrayPosX;
        private NumTextBox ntbXrayPosY;
        internal NumTextBox ntbTiltRot;
        private System.Windows.Forms.ComboBox cboSpeed10;
        private System.Windows.Forms.ComboBox cboSpeed11;
        internal NumTextBox ntbTilt;
        private System.Windows.Forms.Panel fraChangeXray;
        private CWButton cwbtnChangeXray1;
        private System.Windows.Forms.Label lblChangeXray;
        private CWButton cwbtnChangeXray0;
        public System.Windows.Forms.Timer tmrMecainfSeqComm;
        private System.Windows.Forms.GroupBox fraScanArea;
        private NumTextBox ntbShift;
        private NumTextBox ntbOffset;
        private NumTextBox ntbFullHalf;
        private System.Windows.Forms.GroupBox fraMechaScanCondiion;
        internal System.Windows.Forms.Button cmdFromExObsCam;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblColon;
        private System.Windows.Forms.Label lblPixSize;
        private System.Windows.Forms.Label lblScanArea;
        private System.Windows.Forms.Label lblScanMode;
        private System.Windows.Forms.Label lblUnit2;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.Label lblPixSizeNum;
        private System.Windows.Forms.Label lblScanAreaNum;
        private System.Windows.Forms.Label lblSelectedScanMode;
        internal System.Windows.Forms.GroupBox fraAutoScanPos;
        internal System.Windows.Forms.GroupBox fraCollimator;
        internal System.Windows.Forms.GroupBox fraIris;
        internal System.Windows.Forms.Panel fraUpDown;
        internal System.Windows.Forms.Panel fraMechaPos;
        internal System.Windows.Forms.Panel fraMechaControl;
        private System.Windows.Forms.Label lblIIMove4;
        private CTCheckBox ctchkIIMove4;
        private CWButton cwbtnMove6;
        internal System.Windows.Forms.TextBox txtUpDownPos;
	}
}