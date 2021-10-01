namespace CT30K
{
    partial class frmMechaControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMechaControl));
            this.tmrSeqComm = new System.Windows.Forms.Timer(this.components);
            this.tmrMecainf = new System.Windows.Forms.Timer(this.components);
            this.tmrTryReloadForms = new System.Windows.Forms.Timer(this.components);
            this.tmrPIOCheck = new System.Windows.Forms.Timer(this.components);
            this.cmdDetails = new System.Windows.Forms.Button();
            this.cmdMechaAllReset = new System.Windows.Forms.Button();
            this.fraMechaControl = new System.Windows.Forms.GroupBox();
            this.lblXrayTube = new System.Windows.Forms.Label();
            this.ImgXrayTube = new System.Windows.Forms.PictureBox();
            this.lblRot1 = new System.Windows.Forms.Label();
            this.lblRot3 = new System.Windows.Forms.Label();
            this.lblRot2 = new System.Windows.Forms.Label();
            this.lblRot0 = new System.Windows.Forms.Label();
            this.ctchkRotate2 = new CT30K.CTCheckBox();
            this.ctchkRotate1 = new CT30K.CTCheckBox();
            this.ctchkRotate3 = new CT30K.CTCheckBox();
            this.ctchkRotate0 = new CT30K.CTCheckBox();
            this.lblFTable = new System.Windows.Forms.Label();
            this.cwbtnFineTable1 = new CT30K.CWButton();
            this.cwbtnFineTable0 = new CT30K.CWButton();
            this.fraIIMove = new System.Windows.Forms.Panel();
            this.lblIIMove3 = new System.Windows.Forms.Label();
            this.lblIIMove2 = new System.Windows.Forms.Label();
            this.lblIIMove1 = new System.Windows.Forms.Label();
            this.lblIIMove0 = new System.Windows.Forms.Label();
            this.ctchkIIMove3 = new CT30K.CTCheckBox();
            this.ctchkIIMove2 = new CT30K.CTCheckBox();
            this.ctchkIIMove1 = new CT30K.CTCheckBox();
            this.ctchkIIMove0 = new CT30K.CTCheckBox();
            this.lblXrayII = new System.Windows.Forms.Label();
            this.cwbtnRotate1 = new CT30K.CWButton();
            this.cwbtnRotate0 = new CT30K.CWButton();
            this.cwbtnMove1 = new CT30K.CWButton();
            this.cwbtnMove0 = new CT30K.CWButton();
            this.cwbtnMove3 = new CT30K.CWButton();
            this.cwbtnMove2 = new CT30K.CWButton();
            this.fraTableRestriction = new System.Windows.Forms.GroupBox();
            this.cmdTableMovePermit = new System.Windows.Forms.Button();
            this.cmdTableMoveRestrict = new System.Windows.Forms.Button();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.ShpFTable = new Microsoft.VisualBasic.PowerPacks.OvalShape();
            this.ShpTable = new Microsoft.VisualBasic.PowerPacks.OvalShape();
            this.ShpCabinet = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.fraChangeDetector = new System.Windows.Forms.GroupBox();
            this.cwbtnChangeDet1 = new CT30K.CTButton();
            this.cwbtnChangeDet0 = new CT30K.CTButton();
            this.fraHighSpeedCamera = new System.Windows.Forms.GroupBox();
            this.cwbtnChangeMode1 = new CT30K.CTButton();
            this.cwbtnChangeMode0 = new CT30K.CTButton();
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
            this.cwbtnMove7 = new CT30K.CWButton();
            this.cwbtnMove8 = new CT30K.CWButton();
            this.cwbtnMove6 = new CT30K.CWButton();
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
            this.lblMechaStatus5 = new System.Windows.Forms.Label();
            this.lblMechaStatus4 = new System.Windows.Forms.Label();
            this.lblMechaStatus3 = new System.Windows.Forms.Label();
            this.lblMechaStatus2 = new System.Windows.Forms.Label();
            this.lblMechaStatus1 = new System.Windows.Forms.Label();
            this.lblMechaStatus0 = new System.Windows.Forms.Label();
            this.cwneEXMErrCode = new System.Windows.Forms.NumericUpDown();
            this.lblEXMErrCodeHeader = new System.Windows.Forms.Label();
            this.fraUpDown = new System.Windows.Forms.GroupBox();
            this.lblUP = new System.Windows.Forms.Label();
            this.lblDOWN = new System.Windows.Forms.Label();
            this.cmdPosExec = new System.Windows.Forms.Button();
            this.lblMax = new System.Windows.Forms.Label();
            this.lblMiddle = new System.Windows.Forms.Label();
            this.lblMin = new System.Windows.Forms.Label();
            this.cwbtnUpDown0 = new CT30K.CWButton();
            this.cwbtnUpDown1 = new CT30K.CWButton();
            this.cwsldUpDown0 = new System.Windows.Forms.TrackBar();
            this.cwnePos = new System.Windows.Forms.NumericUpDown();
            this.Label22 = new System.Windows.Forms.Label();
            this.cwsldUpDown1 = new System.Windows.Forms.TrackBar();
            this.fraAutoScanPos = new System.Windows.Forms.GroupBox();
            this.cmdFromSlice = new System.Windows.Forms.Button();
            this.cmdFromTrans = new System.Windows.Forms.Button();
            this.fraMechaPos = new System.Windows.Forms.GroupBox();
            this.ntbXrayPosY = new CT30K.NumTextBox();
            this.ntbXrayPosX = new CT30K.NumTextBox();
            this.ntbXrayRotPos = new CT30K.NumTextBox();
            this.ntbFTablePosY = new CT30K.NumTextBox();
            this.ntbFTablePosX = new CT30K.NumTextBox();
            this.ntbTableXPos = new CT30K.NumTextBox();
            this.ntbFID = new CT30K.NumTextBox();
            this.ntbFCD = new CT30K.NumTextBox();
            this.cboSpeed9 = new System.Windows.Forms.ComboBox();
            this.cboSpeed8 = new System.Windows.Forms.ComboBox();
            this.cboSpeed7 = new System.Windows.Forms.ComboBox();
            this.cboSpeed4 = new System.Windows.Forms.ComboBox();
            this.cboSpeed3 = new System.Windows.Forms.ComboBox();
            this.cboSpeed6 = new System.Windows.Forms.ComboBox();
            this.cboSpeed2 = new System.Windows.Forms.ComboBox();
            this.cboSpeed1 = new System.Windows.Forms.ComboBox();
            this.cboSpeed0 = new System.Windows.Forms.ComboBox();
            this.ntbUpDown = new CT30K.NumTextBox();
            this.ntbRotate = new CT30K.NumTextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ImageList1 = new System.Windows.Forms.ImageList(this.components);
            this.fraMechaControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImgXrayTube)).BeginInit();
            this.fraIIMove.SuspendLayout();
            this.fraTableRestriction.SuspendLayout();
            this.fraChangeDetector.SuspendLayout();
            this.fraHighSpeedCamera.SuspendLayout();
            this.fraIris.SuspendLayout();
            this.fraXrayRotate.SuspendLayout();
            this.fraCollimator.SuspendLayout();
            this.fraError.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneEXMErrCode)).BeginInit();
            this.fraUpDown.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwsldUpDown0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwnePos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwsldUpDown1)).BeginInit();
            this.fraAutoScanPos.SuspendLayout();
            this.fraMechaPos.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrSeqComm
            // 
            this.tmrSeqComm.Interval = 500;
            // 
            // tmrMecainf
            // 
            this.tmrMecainf.Interval = 1000;
            // 
            // tmrTryReloadForms
            // 
            this.tmrTryReloadForms.Interval = 2000;
            // 
            // cmdDetails
            // 
            this.cmdDetails.Location = new System.Drawing.Point(721, 290);
            this.cmdDetails.Name = "cmdDetails";
            this.cmdDetails.Size = new System.Drawing.Size(81, 29);
            this.cmdDetails.TabIndex = 50;
            this.cmdDetails.Tag = "10033";
            this.cmdDetails.Text = "#詳細...";
            this.cmdDetails.UseVisualStyleBackColor = true;
            // 
            // cmdMechaAllReset
            // 
            this.cmdMechaAllReset.Location = new System.Drawing.Point(721, 258);
            this.cmdMechaAllReset.Name = "cmdMechaAllReset";
            this.cmdMechaAllReset.Size = new System.Drawing.Size(81, 29);
            this.cmdMechaAllReset.TabIndex = 51;
            this.cmdMechaAllReset.Tag = "20162";
            this.cmdMechaAllReset.Text = "メカリセット";
            this.cmdMechaAllReset.UseVisualStyleBackColor = true;
            // 
            // fraMechaControl
            // 
            this.fraMechaControl.Controls.Add(this.lblXrayTube);
            this.fraMechaControl.Controls.Add(this.ImgXrayTube);
            this.fraMechaControl.Controls.Add(this.lblRot1);
            this.fraMechaControl.Controls.Add(this.lblRot3);
            this.fraMechaControl.Controls.Add(this.lblRot2);
            this.fraMechaControl.Controls.Add(this.lblRot0);
            this.fraMechaControl.Controls.Add(this.ctchkRotate2);
            this.fraMechaControl.Controls.Add(this.ctchkRotate1);
            this.fraMechaControl.Controls.Add(this.ctchkRotate3);
            this.fraMechaControl.Controls.Add(this.ctchkRotate0);
            this.fraMechaControl.Controls.Add(this.lblFTable);
            this.fraMechaControl.Controls.Add(this.cwbtnFineTable1);
            this.fraMechaControl.Controls.Add(this.cwbtnFineTable0);
            this.fraMechaControl.Controls.Add(this.fraIIMove);
            this.fraMechaControl.Controls.Add(this.lblXrayII);
            this.fraMechaControl.Controls.Add(this.cwbtnRotate1);
            this.fraMechaControl.Controls.Add(this.cwbtnRotate0);
            this.fraMechaControl.Controls.Add(this.cwbtnMove1);
            this.fraMechaControl.Controls.Add(this.cwbtnMove0);
            this.fraMechaControl.Controls.Add(this.cwbtnMove3);
            this.fraMechaControl.Controls.Add(this.cwbtnMove2);
            this.fraMechaControl.Controls.Add(this.fraTableRestriction);
            this.fraMechaControl.Controls.Add(this.shapeContainer1);
            this.fraMechaControl.Location = new System.Drawing.Point(449, 2);
            this.fraMechaControl.Name = "fraMechaControl";
            this.fraMechaControl.Padding = new System.Windows.Forms.Padding(0);
            this.fraMechaControl.Size = new System.Drawing.Size(233, 353);
            this.fraMechaControl.TabIndex = 44;
            this.fraMechaControl.TabStop = false;
            this.fraMechaControl.Text = "#機構部操作";
            // 
            // lblXrayTube
            // 
            this.lblXrayTube.AutoSize = true;
            this.lblXrayTube.Location = new System.Drawing.Point(136, 32);
            this.lblXrayTube.Name = "lblXrayTube";
            this.lblXrayTube.Size = new System.Drawing.Size(43, 12);
            this.lblXrayTube.TabIndex = 98;
            this.lblXrayTube.Tag = "12218";
            this.lblXrayTube.Text = "#Ｘ線管";
            // 
            // ImgXrayTube
            // 
            this.ImgXrayTube.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ImgXrayTube.BackgroundImage")));
            this.ImgXrayTube.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ImgXrayTube.Location = new System.Drawing.Point(100, 23);
            this.ImgXrayTube.Margin = new System.Windows.Forms.Padding(0);
            this.ImgXrayTube.Name = "ImgXrayTube";
            this.ImgXrayTube.Size = new System.Drawing.Size(32, 32);
            this.ImgXrayTube.TabIndex = 98;
            this.ImgXrayTube.TabStop = false;
            // 
            // lblRot1
            // 
            this.lblRot1.AutoSize = true;
            this.lblRot1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblRot1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblRot1.Location = new System.Drawing.Point(165, 156);
            this.lblRot1.Name = "lblRot1";
            this.lblRot1.Size = new System.Drawing.Size(19, 12);
            this.lblRot1.TabIndex = 94;
            this.lblRot1.Tag = "";
            this.lblRot1.Text = "90";
            // 
            // lblRot3
            // 
            this.lblRot3.AutoSize = true;
            this.lblRot3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblRot3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblRot3.Location = new System.Drawing.Point(46, 157);
            this.lblRot3.Name = "lblRot3";
            this.lblRot3.Size = new System.Drawing.Size(26, 12);
            this.lblRot3.TabIndex = 93;
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
            this.lblRot2.TabIndex = 97;
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
            this.lblRot0.TabIndex = 96;
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
            this.ctchkRotate2.TabIndex = 84;
            this.ctchkRotate2.Value = false;
            // 
            // ctchkRotate1
            // 
            this.ctchkRotate1.BackColor = System.Drawing.SystemColors.Control;
            this.ctchkRotate1.Checked = false;
            this.ctchkRotate1.Location = new System.Drawing.Point(168, 169);
            this.ctchkRotate1.Name = "ctchkRotate1";
            this.ctchkRotate1.Size = new System.Drawing.Size(13, 13);
            this.ctchkRotate1.TabIndex = 82;
            this.ctchkRotate1.Value = false;
            // 
            // ctchkRotate3
            // 
            this.ctchkRotate3.BackColor = System.Drawing.SystemColors.Control;
            this.ctchkRotate3.Checked = false;
            this.ctchkRotate3.Location = new System.Drawing.Point(53, 169);
            this.ctchkRotate3.Name = "ctchkRotate3";
            this.ctchkRotate3.Size = new System.Drawing.Size(13, 13);
            this.ctchkRotate3.TabIndex = 85;
            this.ctchkRotate3.Value = false;
            // 
            // ctchkRotate0
            // 
            this.ctchkRotate0.BackColor = System.Drawing.SystemColors.Control;
            this.ctchkRotate0.Checked = false;
            this.ctchkRotate0.Location = new System.Drawing.Point(110, 111);
            this.ctchkRotate0.Name = "ctchkRotate0";
            this.ctchkRotate0.Size = new System.Drawing.Size(13, 13);
            this.ctchkRotate0.TabIndex = 83;
            this.ctchkRotate0.Value = false;
            // 
            // lblFTable
            // 
            this.lblFTable.AutoSize = true;
            this.lblFTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblFTable.Location = new System.Drawing.Point(101, 147);
            this.lblFTable.Name = "lblFTable";
            this.lblFTable.Size = new System.Drawing.Size(35, 12);
            this.lblFTable.TabIndex = 92;
            this.lblFTable.Tag = "12130";
            this.lblFTable.Text = "#微調";
            // 
            // cwbtnFineTable1
            // 
            this.cwbtnFineTable1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnFineTable1.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnFineTable1.Location = new System.Drawing.Point(130, 159);
            this.cwbtnFineTable1.Name = "cwbtnFineTable1";
            this.cwbtnFineTable1.OffImage = null;
            this.cwbtnFineTable1.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnFineTable1.OnImage")));
            this.cwbtnFineTable1.Size = new System.Drawing.Size(32, 32);
            this.cwbtnFineTable1.TabIndex = 87;
            this.cwbtnFineTable1.TabStop = false;
            this.cwbtnFineTable1.Tag = "";
            this.cwbtnFineTable1.Value = false;
            // 
            // cwbtnFineTable0
            // 
            this.cwbtnFineTable0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnFineTable0.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnFineTable0.Location = new System.Drawing.Point(72, 159);
            this.cwbtnFineTable0.Name = "cwbtnFineTable0";
            this.cwbtnFineTable0.OffImage = null;
            this.cwbtnFineTable0.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnFineTable0.OnImage")));
            this.cwbtnFineTable0.Size = new System.Drawing.Size(32, 32);
            this.cwbtnFineTable0.TabIndex = 86;
            this.cwbtnFineTable0.TabStop = false;
            this.cwbtnFineTable0.Tag = "";
            this.cwbtnFineTable0.Value = false;
            // 
            // fraIIMove
            // 
            this.fraIIMove.Controls.Add(this.lblIIMove3);
            this.fraIIMove.Controls.Add(this.lblIIMove2);
            this.fraIIMove.Controls.Add(this.lblIIMove1);
            this.fraIIMove.Controls.Add(this.lblIIMove0);
            this.fraIIMove.Controls.Add(this.ctchkIIMove3);
            this.fraIIMove.Controls.Add(this.ctchkIIMove2);
            this.fraIIMove.Controls.Add(this.ctchkIIMove1);
            this.fraIIMove.Controls.Add(this.ctchkIIMove0);
            this.fraIIMove.Location = new System.Drawing.Point(160, 284);
            this.fraIIMove.Name = "fraIIMove";
            this.fraIIMove.Size = new System.Drawing.Size(55, 62);
            this.fraIIMove.TabIndex = 71;
            // 
            // lblIIMove3
            // 
            this.lblIIMove3.AutoSize = true;
            this.lblIIMove3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblIIMove3.Location = new System.Drawing.Point(20, 49);
            this.lblIIMove3.Name = "lblIIMove3";
            this.lblIIMove3.Size = new System.Drawing.Size(33, 12);
            this.lblIIMove3.TabIndex = 76;
            this.lblIIMove3.Text = "1000";
            // 
            // lblIIMove2
            // 
            this.lblIIMove2.AutoSize = true;
            this.lblIIMove2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblIIMove2.Location = new System.Drawing.Point(20, 33);
            this.lblIIMove2.Name = "lblIIMove2";
            this.lblIIMove2.Size = new System.Drawing.Size(26, 12);
            this.lblIIMove2.TabIndex = 77;
            this.lblIIMove2.Text = "800";
            // 
            // lblIIMove1
            // 
            this.lblIIMove1.AutoSize = true;
            this.lblIIMove1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblIIMove1.Location = new System.Drawing.Point(20, 17);
            this.lblIIMove1.Name = "lblIIMove1";
            this.lblIIMove1.Size = new System.Drawing.Size(26, 12);
            this.lblIIMove1.TabIndex = 78;
            this.lblIIMove1.Text = "600";
            // 
            // lblIIMove0
            // 
            this.lblIIMove0.AutoSize = true;
            this.lblIIMove0.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblIIMove0.Location = new System.Drawing.Point(20, 0);
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
            // 
            // lblXrayII
            // 
            this.lblXrayII.BackColor = System.Drawing.SystemColors.Window;
            this.lblXrayII.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblXrayII.Location = new System.Drawing.Point(84, 308);
            this.lblXrayII.Name = "lblXrayII";
            this.lblXrayII.Size = new System.Drawing.Size(70, 18);
            this.lblXrayII.TabIndex = 95;
            this.lblXrayII.Tag = "12332";
            this.lblXrayII.Text = "#Ｘ線  Ｉ．Ｉ．";
            this.lblXrayII.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cwbtnRotate1
            // 
            this.cwbtnRotate1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnRotate1.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnRotate1.Location = new System.Drawing.Point(168, 99);
            this.cwbtnRotate1.Name = "cwbtnRotate1";
            this.cwbtnRotate1.OffImage = null;
            this.cwbtnRotate1.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnRotate1.OnImage")));
            this.cwbtnRotate1.Size = new System.Drawing.Size(29, 29);
            this.cwbtnRotate1.TabIndex = 80;
            this.cwbtnRotate1.TabStop = false;
            this.cwbtnRotate1.Value = false;
            // 
            // cwbtnRotate0
            // 
            this.cwbtnRotate0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnRotate0.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnRotate0.Location = new System.Drawing.Point(35, 99);
            this.cwbtnRotate0.Name = "cwbtnRotate0";
            this.cwbtnRotate0.OffImage = null;
            this.cwbtnRotate0.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnRotate0.OnImage")));
            this.cwbtnRotate0.Size = new System.Drawing.Size(29, 29);
            this.cwbtnRotate0.TabIndex = 81;
            this.cwbtnRotate0.TabStop = false;
            this.cwbtnRotate0.Value = false;
            // 
            // cwbtnMove1
            // 
            this.cwbtnMove1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnMove1.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnMove1.Location = new System.Drawing.Point(186, 159);
            this.cwbtnMove1.Name = "cwbtnMove1";
            this.cwbtnMove1.OffImage = null;
            this.cwbtnMove1.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnMove1.OnImage")));
            this.cwbtnMove1.Size = new System.Drawing.Size(32, 32);
            this.cwbtnMove1.TabIndex = 91;
            this.cwbtnMove1.TabStop = false;
            this.cwbtnMove1.Tag = "XRight";
            this.cwbtnMove1.Value = false;
            // 
            // cwbtnMove0
            // 
            this.cwbtnMove0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnMove0.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnMove0.Location = new System.Drawing.Point(15, 159);
            this.cwbtnMove0.Name = "cwbtnMove0";
            this.cwbtnMove0.OffImage = null;
            this.cwbtnMove0.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnMove0.OnImage")));
            this.cwbtnMove0.Size = new System.Drawing.Size(32, 32);
            this.cwbtnMove0.TabIndex = 90;
            this.cwbtnMove0.TabStop = false;
            this.cwbtnMove0.Tag = "XLeft";
            this.cwbtnMove0.Value = false;
            // 
            // cwbtnMove3
            // 
            this.cwbtnMove3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnMove3.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnMove3.Location = new System.Drawing.Point(101, 245);
            this.cwbtnMove3.Name = "cwbtnMove3";
            this.cwbtnMove3.OffImage = null;
            this.cwbtnMove3.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnMove3.OnImage")));
            this.cwbtnMove3.Size = new System.Drawing.Size(32, 32);
            this.cwbtnMove3.TabIndex = 89;
            this.cwbtnMove3.TabStop = false;
            this.cwbtnMove3.Tag = "YBackward";
            this.cwbtnMove3.Value = false;
            // 
            // cwbtnMove2
            // 
            this.cwbtnMove2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnMove2.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnMove2.Location = new System.Drawing.Point(101, 74);
            this.cwbtnMove2.Name = "cwbtnMove2";
            this.cwbtnMove2.OffImage = null;
            this.cwbtnMove2.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnMove2.OnImage")));
            this.cwbtnMove2.Size = new System.Drawing.Size(32, 32);
            this.cwbtnMove2.TabIndex = 88;
            this.cwbtnMove2.TabStop = false;
            this.cwbtnMove2.Tag = "YForward";
            this.cwbtnMove2.Value = false;
            // 
            // fraTableRestriction
            // 
            this.fraTableRestriction.Controls.Add(this.cmdTableMovePermit);
            this.fraTableRestriction.Controls.Add(this.cmdTableMoveRestrict);
            this.fraTableRestriction.Location = new System.Drawing.Point(12, 16);
            this.fraTableRestriction.Name = "fraTableRestriction";
            this.fraTableRestriction.Size = new System.Drawing.Size(85, 73);
            this.fraTableRestriction.TabIndex = 68;
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
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 12);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.ShpFTable,
            this.ShpTable,
            this.ShpCabinet});
            this.shapeContainer1.Size = new System.Drawing.Size(233, 341);
            this.shapeContainer1.TabIndex = 69;
            this.shapeContainer1.TabStop = false;
            // 
            // ShpFTable
            // 
            this.ShpFTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ShpFTable.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Opaque;
            this.ShpFTable.BorderWidth = 2;
            this.ShpFTable.Cursor = System.Windows.Forms.Cursors.Default;
            this.ShpFTable.Location = new System.Drawing.Point(105, 150);
            this.ShpFTable.Name = "ShpFTable";
            this.ShpFTable.Size = new System.Drawing.Size(23, 23);
            // 
            // ShpTable
            // 
            this.ShpTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ShpTable.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Opaque;
            this.ShpTable.BorderWidth = 2;
            this.ShpTable.Location = new System.Drawing.Point(48, 94);
            this.ShpTable.Name = "ShpTable";
            this.ShpTable.Size = new System.Drawing.Size(137, 137);
            // 
            // ShpCabinet
            // 
            this.ShpCabinet.BorderColor = System.Drawing.Color.Blue;
            this.ShpCabinet.BorderWidth = 3;
            this.ShpCabinet.Location = new System.Drawing.Point(8, 2);
            this.ShpCabinet.Name = "ShpCabinet";
            this.ShpCabinet.Size = new System.Drawing.Size(217, 334);
            // 
            // fraChangeDetector
            // 
            this.fraChangeDetector.Controls.Add(this.cwbtnChangeDet1);
            this.fraChangeDetector.Controls.Add(this.cwbtnChangeDet0);
            this.fraChangeDetector.Location = new System.Drawing.Point(361, 434);
            this.fraChangeDetector.Name = "fraChangeDetector";
            this.fraChangeDetector.Size = new System.Drawing.Size(281, 65);
            this.fraChangeDetector.TabIndex = 49;
            this.fraChangeDetector.TabStop = false;
            this.fraChangeDetector.Text = "検出器１⇔検出器２ 切替";
            // 
            // cwbtnChangeDet1
            // 
            this.cwbtnChangeDet1.Caption = "検出器２";
            this.cwbtnChangeDet1.CaptionFont = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwbtnChangeDet1.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwbtnChangeDet1.Header = "";
            this.cwbtnChangeDet1.Location = new System.Drawing.Point(152, 16);
            this.cwbtnChangeDet1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cwbtnChangeDet1.Name = "cwbtnChangeDet1";
            this.cwbtnChangeDet1.OffColor = System.Drawing.Color.Green;
            this.cwbtnChangeDet1.OnColor = System.Drawing.Color.Lime;
            this.cwbtnChangeDet1.Picture = null;
            this.cwbtnChangeDet1.Size = new System.Drawing.Size(120, 40);
            this.cwbtnChangeDet1.TabIndex = 104;
            this.cwbtnChangeDet1.Value = false;
            // 
            // cwbtnChangeDet0
            // 
            this.cwbtnChangeDet0.Caption = "検出器１";
            this.cwbtnChangeDet0.CaptionFont = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwbtnChangeDet0.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwbtnChangeDet0.Header = "";
            this.cwbtnChangeDet0.Location = new System.Drawing.Point(16, 16);
            this.cwbtnChangeDet0.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cwbtnChangeDet0.Name = "cwbtnChangeDet0";
            this.cwbtnChangeDet0.OffColor = System.Drawing.Color.Green;
            this.cwbtnChangeDet0.OnColor = System.Drawing.Color.Lime;
            this.cwbtnChangeDet0.Picture = null;
            this.cwbtnChangeDet0.Size = new System.Drawing.Size(120, 40);
            this.cwbtnChangeDet0.TabIndex = 103;
            this.cwbtnChangeDet0.Value = false;
            // 
            // fraHighSpeedCamera
            // 
            this.fraHighSpeedCamera.Controls.Add(this.cwbtnChangeMode1);
            this.fraHighSpeedCamera.Controls.Add(this.cwbtnChangeMode0);
            this.fraHighSpeedCamera.Location = new System.Drawing.Point(401, 374);
            this.fraHighSpeedCamera.Name = "fraHighSpeedCamera";
            this.fraHighSpeedCamera.Size = new System.Drawing.Size(281, 65);
            this.fraHighSpeedCamera.TabIndex = 48;
            this.fraHighSpeedCamera.TabStop = false;
            this.fraHighSpeedCamera.Text = "#CT⇔高速撮影 切替";
            // 
            // cwbtnChangeMode1
            // 
            this.cwbtnChangeMode1.Caption = "#高速撮影";
            this.cwbtnChangeMode1.CaptionFont = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwbtnChangeMode1.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwbtnChangeMode1.Header = "";
            this.cwbtnChangeMode1.Location = new System.Drawing.Point(152, 16);
            this.cwbtnChangeMode1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cwbtnChangeMode1.Name = "cwbtnChangeMode1";
            this.cwbtnChangeMode1.OffColor = System.Drawing.Color.Green;
            this.cwbtnChangeMode1.OnColor = System.Drawing.Color.Lime;
            this.cwbtnChangeMode1.Picture = null;
            this.cwbtnChangeMode1.Size = new System.Drawing.Size(120, 40);
            this.cwbtnChangeMode1.TabIndex = 101;
            this.cwbtnChangeMode1.Value = false;
            // 
            // cwbtnChangeMode0
            // 
            this.cwbtnChangeMode0.Caption = "#CT";
            this.cwbtnChangeMode0.CaptionFont = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwbtnChangeMode0.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwbtnChangeMode0.Header = "";
            this.cwbtnChangeMode0.Location = new System.Drawing.Point(16, 16);
            this.cwbtnChangeMode0.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cwbtnChangeMode0.Name = "cwbtnChangeMode0";
            this.cwbtnChangeMode0.OffColor = System.Drawing.Color.Green;
            this.cwbtnChangeMode0.OnColor = System.Drawing.Color.Lime;
            this.cwbtnChangeMode0.Picture = null;
            this.cwbtnChangeMode0.Size = new System.Drawing.Size(120, 40);
            this.cwbtnChangeMode0.TabIndex = 100;
            this.cwbtnChangeMode0.Value = false;
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
            this.fraIris.Location = new System.Drawing.Point(689, 322);
            this.fraIris.Name = "fraIris";
            this.fraIris.Size = new System.Drawing.Size(142, 137);
            this.fraIris.TabIndex = 47;
            this.fraIris.TabStop = false;
            this.fraIris.Text = "#Ｘ線Ｉ．Ｉ．絞り";
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
            // 
            // fraXrayRotate
            // 
            this.fraXrayRotate.Controls.Add(this.cwbtnMove7);
            this.fraXrayRotate.Controls.Add(this.cwbtnMove8);
            this.fraXrayRotate.Controls.Add(this.cwbtnMove6);
            this.fraXrayRotate.Controls.Add(this.cwbtnMove9);
            this.fraXrayRotate.Controls.Add(this.cwbtnRotateXray1);
            this.fraXrayRotate.Controls.Add(this.cwbtnRotateXray0);
            this.fraXrayRotate.Location = new System.Drawing.Point(689, 146);
            this.fraXrayRotate.Name = "fraXrayRotate";
            this.fraXrayRotate.Size = new System.Drawing.Size(142, 113);
            this.fraXrayRotate.TabIndex = 46;
            this.fraXrayRotate.TabStop = false;
            this.fraXrayRotate.Tag = "12217";
            this.fraXrayRotate.Text = "#Ｘ線管操作";
            // 
            // cwbtnMove7
            // 
            this.cwbtnMove7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnMove7.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnMove7.Location = new System.Drawing.Point(53, 12);
            this.cwbtnMove7.Name = "cwbtnMove7";
            this.cwbtnMove7.OffImage = null;
            this.cwbtnMove7.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnMove7.OnImage")));
            this.cwbtnMove7.Size = new System.Drawing.Size(32, 32);
            this.cwbtnMove7.TabIndex = 13;
            this.cwbtnMove7.Tag = "XrayXRight";
            this.cwbtnMove7.Value = false;
            // 
            // cwbtnMove8
            // 
            this.cwbtnMove8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnMove8.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnMove8.Location = new System.Drawing.Point(18, 44);
            this.cwbtnMove8.Name = "cwbtnMove8";
            this.cwbtnMove8.OffImage = null;
            this.cwbtnMove8.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnMove8.OnImage")));
            this.cwbtnMove8.Size = new System.Drawing.Size(32, 32);
            this.cwbtnMove8.TabIndex = 14;
            this.cwbtnMove8.Tag = "XrayYForward";
            this.cwbtnMove8.Value = false;
            // 
            // cwbtnMove6
            // 
            this.cwbtnMove6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnMove6.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnMove6.Location = new System.Drawing.Point(53, 77);
            this.cwbtnMove6.Name = "cwbtnMove6";
            this.cwbtnMove6.OffImage = null;
            this.cwbtnMove6.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnMove6.OnImage")));
            this.cwbtnMove6.Size = new System.Drawing.Size(32, 32);
            this.cwbtnMove6.TabIndex = 17;
            this.cwbtnMove6.Tag = "XrayXLeft";
            this.cwbtnMove6.Value = false;
            // 
            // cwbtnMove9
            // 
            this.cwbtnMove9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnMove9.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnMove9.Location = new System.Drawing.Point(87, 44);
            this.cwbtnMove9.Name = "cwbtnMove9";
            this.cwbtnMove9.OffImage = null;
            this.cwbtnMove9.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnMove9.OnImage")));
            this.cwbtnMove9.Size = new System.Drawing.Size(32, 32);
            this.cwbtnMove9.TabIndex = 15;
            this.cwbtnMove9.Tag = "XrayYBackward";
            this.cwbtnMove9.Value = false;
            // 
            // cwbtnRotateXray1
            // 
            this.cwbtnRotateXray1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnRotateXray1.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnRotateXray1.Location = new System.Drawing.Point(91, 80);
            this.cwbtnRotateXray1.Name = "cwbtnRotateXray1";
            this.cwbtnRotateXray1.OffImage = null;
            this.cwbtnRotateXray1.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnRotateXray1.OnImage")));
            this.cwbtnRotateXray1.Size = new System.Drawing.Size(29, 29);
            this.cwbtnRotateXray1.TabIndex = 18;
            this.cwbtnRotateXray1.Tag = "XRAYROTCCW";
            this.cwbtnRotateXray1.Value = false;
            // 
            // cwbtnRotateXray0
            // 
            this.cwbtnRotateXray0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnRotateXray0.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnRotateXray0.Location = new System.Drawing.Point(18, 80);
            this.cwbtnRotateXray0.Name = "cwbtnRotateXray0";
            this.cwbtnRotateXray0.OffImage = null;
            this.cwbtnRotateXray0.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnRotateXray0.OnImage")));
            this.cwbtnRotateXray0.Size = new System.Drawing.Size(29, 29);
            this.cwbtnRotateXray0.TabIndex = 16;
            this.cwbtnRotateXray0.Tag = "XRAYROTCW";
            this.cwbtnRotateXray0.Value = false;
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
            this.fraCollimator.Location = new System.Drawing.Point(689, 2);
            this.fraCollimator.Name = "fraCollimator";
            this.fraCollimator.Size = new System.Drawing.Size(142, 137);
            this.fraCollimator.TabIndex = 45;
            this.fraCollimator.TabStop = false;
            this.fraCollimator.Tag = "12157";
            this.fraCollimator.Text = "#コリメータ";
            // 
            // cmdCollimator0
            // 
            this.cmdCollimator0.Location = new System.Drawing.Point(4, 64);
            this.cmdCollimator0.Margin = new System.Windows.Forms.Padding(0);
            this.cmdCollimator0.Name = "cmdCollimator0";
            this.cmdCollimator0.Size = new System.Drawing.Size(32, 21);
            this.cmdCollimator0.TabIndex = 7;
            this.cmdCollimator0.Tag = "ColliLOpen";
            this.cmdCollimator0.Text = "開";
            this.cmdCollimator0.UseVisualStyleBackColor = true;
            // 
            // cmdCollimator2
            // 
            this.cmdCollimator2.Location = new System.Drawing.Point(105, 64);
            this.cmdCollimator2.Margin = new System.Windows.Forms.Padding(0);
            this.cmdCollimator2.Name = "cmdCollimator2";
            this.cmdCollimator2.Size = new System.Drawing.Size(32, 21);
            this.cmdCollimator2.TabIndex = 10;
            this.cmdCollimator2.Tag = "ColliROpen";
            this.cmdCollimator2.Text = "開";
            this.cmdCollimator2.UseVisualStyleBackColor = true;
            // 
            // cmdCollimator6
            // 
            this.cmdCollimator6.Location = new System.Drawing.Point(54, 111);
            this.cmdCollimator6.Margin = new System.Windows.Forms.Padding(0);
            this.cmdCollimator6.Name = "cmdCollimator6";
            this.cmdCollimator6.Size = new System.Drawing.Size(32, 21);
            this.cmdCollimator6.TabIndex = 6;
            this.cmdCollimator6.Tag = "ColliDOpen";
            this.cmdCollimator6.Text = "開";
            this.cmdCollimator6.UseVisualStyleBackColor = true;
            // 
            // cmdCollimator4
            // 
            this.cmdCollimator4.Location = new System.Drawing.Point(54, 14);
            this.cmdCollimator4.Margin = new System.Windows.Forms.Padding(0);
            this.cmdCollimator4.Name = "cmdCollimator4";
            this.cmdCollimator4.Size = new System.Drawing.Size(32, 21);
            this.cmdCollimator4.TabIndex = 3;
            this.cmdCollimator4.Tag = "ColliUOpen";
            this.cmdCollimator4.Text = "開";
            this.cmdCollimator4.UseVisualStyleBackColor = true;
            // 
            // cmdCollimator3
            // 
            this.cmdCollimator3.Location = new System.Drawing.Point(74, 64);
            this.cmdCollimator3.Margin = new System.Windows.Forms.Padding(0);
            this.cmdCollimator3.Name = "cmdCollimator3";
            this.cmdCollimator3.Size = new System.Drawing.Size(32, 21);
            this.cmdCollimator3.TabIndex = 9;
            this.cmdCollimator3.Tag = "ColliRClose";
            this.cmdCollimator3.Text = "閉";
            this.cmdCollimator3.UseVisualStyleBackColor = true;
            // 
            // cmdCollimator1
            // 
            this.cmdCollimator1.Location = new System.Drawing.Point(35, 64);
            this.cmdCollimator1.Margin = new System.Windows.Forms.Padding(0);
            this.cmdCollimator1.Name = "cmdCollimator1";
            this.cmdCollimator1.Size = new System.Drawing.Size(32, 21);
            this.cmdCollimator1.TabIndex = 8;
            this.cmdCollimator1.Tag = "ColliLClose";
            this.cmdCollimator1.Text = "閉";
            this.cmdCollimator1.UseVisualStyleBackColor = true;
            // 
            // cmdCollimator7
            // 
            this.cmdCollimator7.Location = new System.Drawing.Point(54, 91);
            this.cmdCollimator7.Margin = new System.Windows.Forms.Padding(0);
            this.cmdCollimator7.Name = "cmdCollimator7";
            this.cmdCollimator7.Size = new System.Drawing.Size(32, 21);
            this.cmdCollimator7.TabIndex = 5;
            this.cmdCollimator7.Tag = "ColliDClose";
            this.cmdCollimator7.Text = "閉";
            this.cmdCollimator7.UseVisualStyleBackColor = true;
            // 
            // cmdCollimator5
            // 
            this.cmdCollimator5.Location = new System.Drawing.Point(54, 34);
            this.cmdCollimator5.Margin = new System.Windows.Forms.Padding(0);
            this.cmdCollimator5.Name = "cmdCollimator5";
            this.cmdCollimator5.Size = new System.Drawing.Size(32, 21);
            this.cmdCollimator5.TabIndex = 4;
            this.cmdCollimator5.Tag = "ColliUClose";
            this.cmdCollimator5.Text = "閉";
            this.cmdCollimator5.UseVisualStyleBackColor = true;
            // 
            // fraError
            // 
            this.fraError.Controls.Add(this.lblMechaStatus5);
            this.fraError.Controls.Add(this.lblMechaStatus4);
            this.fraError.Controls.Add(this.lblMechaStatus3);
            this.fraError.Controls.Add(this.lblMechaStatus2);
            this.fraError.Controls.Add(this.lblMechaStatus1);
            this.fraError.Controls.Add(this.lblMechaStatus0);
            this.fraError.Controls.Add(this.cwneEXMErrCode);
            this.fraError.Controls.Add(this.lblEXMErrCodeHeader);
            this.fraError.Location = new System.Drawing.Point(1, 358);
            this.fraError.Name = "fraError";
            this.fraError.Size = new System.Drawing.Size(561, 141);
            this.fraError.TabIndex = 43;
            this.fraError.TabStop = false;
            this.fraError.Text = "エラーステータス・非表示";
            // 
            // lblMechaStatus5
            // 
            this.lblMechaStatus5.AutoSize = true;
            this.lblMechaStatus5.Location = new System.Drawing.Point(280, 120);
            this.lblMechaStatus5.Name = "lblMechaStatus5";
            this.lblMechaStatus5.Size = new System.Drawing.Size(59, 12);
            this.lblMechaStatus5.TabIndex = 23;
            this.lblMechaStatus5.Tag = "stsYstgErr";
            this.lblMechaStatus5.Text = "stsYstgErr";
            // 
            // lblMechaStatus4
            // 
            this.lblMechaStatus4.AutoSize = true;
            this.lblMechaStatus4.Location = new System.Drawing.Point(280, 100);
            this.lblMechaStatus4.Name = "lblMechaStatus4";
            this.lblMechaStatus4.Size = new System.Drawing.Size(59, 12);
            this.lblMechaStatus4.TabIndex = 24;
            this.lblMechaStatus4.Tag = "stsXstgErr";
            this.lblMechaStatus4.Text = "stsXstgErr";
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
            // 
            // lblMechaStatus0
            // 
            this.lblMechaStatus0.AutoSize = true;
            this.lblMechaStatus0.Location = new System.Drawing.Point(280, 20);
            this.lblMechaStatus0.Name = "lblMechaStatus0";
            this.lblMechaStatus0.Size = new System.Drawing.Size(71, 12);
            this.lblMechaStatus0.TabIndex = 28;
            this.lblMechaStatus0.Tag = "stsPhmOnoff";
            this.lblMechaStatus0.Text = "stsPhmOnoff";
            // 
            // cwneEXMErrCode
            // 
            this.cwneEXMErrCode.Location = new System.Drawing.Point(156, 52);
            this.cwneEXMErrCode.Name = "cwneEXMErrCode";
            this.cwneEXMErrCode.Size = new System.Drawing.Size(45, 19);
            this.cwneEXMErrCode.TabIndex = 21;
            this.cwneEXMErrCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
            // fraUpDown
            // 
            this.fraUpDown.Controls.Add(this.lblUP);
            this.fraUpDown.Controls.Add(this.lblDOWN);
            this.fraUpDown.Controls.Add(this.cmdPosExec);
            this.fraUpDown.Controls.Add(this.lblMax);
            this.fraUpDown.Controls.Add(this.lblMiddle);
            this.fraUpDown.Controls.Add(this.lblMin);
            this.fraUpDown.Controls.Add(this.cwbtnUpDown0);
            this.fraUpDown.Controls.Add(this.cwbtnUpDown1);
            this.fraUpDown.Controls.Add(this.cwsldUpDown0);
            this.fraUpDown.Controls.Add(this.cwnePos);
            this.fraUpDown.Controls.Add(this.Label22);
            this.fraUpDown.Controls.Add(this.cwsldUpDown1);
            this.fraUpDown.Location = new System.Drawing.Point(297, 2);
            this.fraUpDown.Name = "fraUpDown";
            this.fraUpDown.Size = new System.Drawing.Size(140, 353);
            this.fraUpDown.TabIndex = 42;
            this.fraUpDown.TabStop = false;
            this.fraUpDown.Text = "#昇降";
            // 
            // lblUP
            // 
            this.lblUP.AutoSize = true;
            this.lblUP.Location = new System.Drawing.Point(20, 36);
            this.lblUP.Name = "lblUP";
            this.lblUP.Size = new System.Drawing.Size(20, 12);
            this.lblUP.TabIndex = 65;
            this.lblUP.Text = "UP";
            // 
            // lblDOWN
            // 
            this.lblDOWN.AutoSize = true;
            this.lblDOWN.Location = new System.Drawing.Point(9, 252);
            this.lblDOWN.Name = "lblDOWN";
            this.lblDOWN.Size = new System.Drawing.Size(38, 12);
            this.lblDOWN.TabIndex = 66;
            this.lblDOWN.Text = "DOWN";
            // 
            // cmdPosExec
            // 
            this.cmdPosExec.Location = new System.Drawing.Point(6, 280);
            this.cmdPosExec.Name = "cmdPosExec";
            this.cmdPosExec.Size = new System.Drawing.Size(120, 28);
            this.cmdPosExec.TabIndex = 63;
            this.cmdPosExec.Tag = "15211";
            this.cmdPosExec.Text = "#指定高さ位置移動";
            this.cmdPosExec.UseVisualStyleBackColor = true;
            // 
            // lblMax
            // 
            this.lblMax.AutoSize = true;
            this.lblMax.Location = new System.Drawing.Point(79, 234);
            this.lblMax.Name = "lblMax";
            this.lblMax.Size = new System.Drawing.Size(23, 12);
            this.lblMax.TabIndex = 67;
            this.lblMax.Text = "200";
            // 
            // lblMiddle
            // 
            this.lblMiddle.AutoSize = true;
            this.lblMiddle.Location = new System.Drawing.Point(79, 144);
            this.lblMiddle.Name = "lblMiddle";
            this.lblMiddle.Size = new System.Drawing.Size(23, 12);
            this.lblMiddle.TabIndex = 67;
            this.lblMiddle.Text = "100";
            // 
            // lblMin
            // 
            this.lblMin.AutoSize = true;
            this.lblMin.Location = new System.Drawing.Point(79, 53);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(11, 12);
            this.lblMin.TabIndex = 67;
            this.lblMin.Text = "0";
            // 
            // cwbtnUpDown0
            // 
            this.cwbtnUpDown0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnUpDown0.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnUpDown0.Location = new System.Drawing.Point(48, 242);
            this.cwbtnUpDown0.Name = "cwbtnUpDown0";
            this.cwbtnUpDown0.OffImage = null;
            this.cwbtnUpDown0.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnUpDown0.OnImage")));
            this.cwbtnUpDown0.Size = new System.Drawing.Size(32, 32);
            this.cwbtnUpDown0.TabIndex = 59;
            this.cwbtnUpDown0.TabStop = false;
            this.cwbtnUpDown0.Tag = "YBackward";
            this.cwbtnUpDown0.Value = false;
            // 
            // cwbtnUpDown1
            // 
            this.cwbtnUpDown1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cwbtnUpDown1.BlinkInterval = CT30K.CWSpeeds.cwSpeedOff;
            this.cwbtnUpDown1.Location = new System.Drawing.Point(48, 24);
            this.cwbtnUpDown1.Name = "cwbtnUpDown1";
            this.cwbtnUpDown1.OffImage = null;
            this.cwbtnUpDown1.OnImage = ((System.Drawing.Image)(resources.GetObject("cwbtnUpDown1.OnImage")));
            this.cwbtnUpDown1.Size = new System.Drawing.Size(32, 32);
            this.cwbtnUpDown1.TabIndex = 60;
            this.cwbtnUpDown1.TabStop = false;
            this.cwbtnUpDown1.Tag = "YForward";
            this.cwbtnUpDown1.Value = false;
            // 
            // cwsldUpDown0
            // 
            this.cwsldUpDown0.BackColor = System.Drawing.SystemColors.Control;
            this.cwsldUpDown0.Location = new System.Drawing.Point(43, 45);
            this.cwsldUpDown0.Maximum = 200;
            this.cwsldUpDown0.Name = "cwsldUpDown0";
            this.cwsldUpDown0.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.cwsldUpDown0.Size = new System.Drawing.Size(42, 207);
            this.cwsldUpDown0.TabIndex = 0;
            this.cwsldUpDown0.TickFrequency = 100;
            this.cwsldUpDown0.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.cwsldUpDown0.Value = 100;
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
            this.cwnePos.Location = new System.Drawing.Point(8, 314);
            this.cwnePos.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.cwnePos.Name = "cwnePos";
            this.cwnePos.Size = new System.Drawing.Size(101, 26);
            this.cwnePos.TabIndex = 62;
            this.cwnePos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Label22
            // 
            this.Label22.AutoSize = true;
            this.Label22.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Label22.Location = new System.Drawing.Point(107, 318);
            this.Label22.Margin = new System.Windows.Forms.Padding(0);
            this.Label22.Name = "Label22";
            this.Label22.Size = new System.Drawing.Size(32, 16);
            this.Label22.TabIndex = 68;
            this.Label22.Text = "mm";
            // 
            // cwsldUpDown1
            // 
            this.cwsldUpDown1.BackColor = System.Drawing.Color.Yellow;
            this.cwsldUpDown1.Location = new System.Drawing.Point(2, 45);
            this.cwsldUpDown1.Maximum = 200;
            this.cwsldUpDown1.Name = "cwsldUpDown1";
            this.cwsldUpDown1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.cwsldUpDown1.Size = new System.Drawing.Size(42, 207);
            this.cwsldUpDown1.TabIndex = 0;
            this.cwsldUpDown1.TickFrequency = 100;
            this.cwsldUpDown1.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.cwsldUpDown1.Value = 100;
            // 
            // fraAutoScanPos
            // 
            this.fraAutoScanPos.Controls.Add(this.cmdFromSlice);
            this.fraAutoScanPos.Controls.Add(this.cmdFromTrans);
            this.fraAutoScanPos.Location = new System.Drawing.Point(33, 274);
            this.fraAutoScanPos.Name = "fraAutoScanPos";
            this.fraAutoScanPos.Size = new System.Drawing.Size(217, 65);
            this.fraAutoScanPos.TabIndex = 41;
            this.fraAutoScanPos.TabStop = false;
            this.fraAutoScanPos.Tag = "15204";
            this.fraAutoScanPos.Text = "#自動スキャン位置指定";
            // 
            // cmdFromSlice
            // 
            this.cmdFromSlice.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdFromSlice.Location = new System.Drawing.Point(112, 24);
            this.cmdFromSlice.Name = "cmdFromSlice";
            this.cmdFromSlice.Size = new System.Drawing.Size(87, 28);
            this.cmdFromSlice.TabIndex = 0;
            this.cmdFromSlice.Tag = "15205";
            this.cmdFromSlice.Text = "#断面";
            this.cmdFromSlice.UseVisualStyleBackColor = true;
            // 
            // cmdFromTrans
            // 
            this.cmdFromTrans.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdFromTrans.Location = new System.Drawing.Point(16, 24);
            this.cmdFromTrans.Name = "cmdFromTrans";
            this.cmdFromTrans.Size = new System.Drawing.Size(87, 28);
            this.cmdFromTrans.TabIndex = 0;
            this.cmdFromTrans.Tag = "15206";
            this.cmdFromTrans.Text = "#透視";
            this.cmdFromTrans.UseVisualStyleBackColor = true;
            // 
            // fraMechaPos
            // 
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
            this.fraMechaPos.Location = new System.Drawing.Point(1, 2);
            this.fraMechaPos.Name = "fraMechaPos";
            this.fraMechaPos.Size = new System.Drawing.Size(281, 265);
            this.fraMechaPos.TabIndex = 40;
            this.fraMechaPos.TabStop = false;
            this.fraMechaPos.Text = "#機構部位置";
            // 
            // ntbXrayPosY
            // 
            this.ntbXrayPosY.Caption = "Ｘ線管Y軸";
            this.ntbXrayPosY.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbXrayPosY.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbXrayPosY.CaptionWidth = 65;
            this.ntbXrayPosY.DiscreteInterval = 0.01F;
            this.ntbXrayPosY.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbXrayPosY.IncDecButton = false;
            this.ntbXrayPosY.Location = new System.Drawing.Point(8, 232);
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
            this.ntbXrayPosY.Size = new System.Drawing.Size(174, 17);
            this.ntbXrayPosY.TabIndex = 45;
            this.ntbXrayPosY.Unit = "mm";
            this.ntbXrayPosY.Value = new decimal(new int[] {
            999999,
            0,
            0,
            131072});
            // 
            // ntbXrayPosX
            // 
            this.ntbXrayPosX.Caption = "Ｘ線管X軸";
            this.ntbXrayPosX.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbXrayPosX.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbXrayPosX.CaptionWidth = 65;
            this.ntbXrayPosX.DiscreteInterval = 0.1F;
            this.ntbXrayPosX.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbXrayPosX.IncDecButton = false;
            this.ntbXrayPosX.Location = new System.Drawing.Point(8, 208);
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
            this.ntbXrayPosX.Size = new System.Drawing.Size(174, 20);
            this.ntbXrayPosX.TabIndex = 44;
            this.ntbXrayPosX.Unit = "mm";
            this.ntbXrayPosX.Value = new decimal(new int[] {
            999999,
            0,
            0,
            65536});
            // 
            // ntbXrayRotPos
            // 
            this.ntbXrayRotPos.Caption = "Ｘ線管回転";
            this.ntbXrayRotPos.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbXrayRotPos.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbXrayRotPos.CaptionWidth = 65;
            this.ntbXrayRotPos.DiscreteInterval = 0.01F;
            this.ntbXrayRotPos.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbXrayRotPos.IncDecButton = false;
            this.ntbXrayRotPos.Location = new System.Drawing.Point(8, 184);
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
            this.ntbXrayRotPos.Size = new System.Drawing.Size(168, 20);
            this.ntbXrayRotPos.TabIndex = 40;
            this.ntbXrayRotPos.Unit = "度";
            this.ntbXrayRotPos.Value = new decimal(new int[] {
            99999,
            0,
            0,
            131072});
            // 
            // ntbFTablePosY
            // 
            this.ntbFTablePosY.Caption = "微調Y軸";
            this.ntbFTablePosY.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbFTablePosY.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbFTablePosY.CaptionWidth = 65;
            this.ntbFTablePosY.DiscreteInterval = 0.01F;
            this.ntbFTablePosY.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbFTablePosY.IncDecButton = false;
            this.ntbFTablePosY.Location = new System.Drawing.Point(8, 160);
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
            this.ntbFTablePosY.Size = new System.Drawing.Size(174, 20);
            this.ntbFTablePosY.TabIndex = 43;
            this.ntbFTablePosY.Unit = "mm";
            this.ntbFTablePosY.Value = new decimal(new int[] {
            999999,
            0,
            0,
            131072});
            // 
            // ntbFTablePosX
            // 
            this.ntbFTablePosX.Caption = "微調X軸";
            this.ntbFTablePosX.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbFTablePosX.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbFTablePosX.CaptionWidth = 65;
            this.ntbFTablePosX.DiscreteInterval = 0.01F;
            this.ntbFTablePosX.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbFTablePosX.IncDecButton = false;
            this.ntbFTablePosX.Location = new System.Drawing.Point(8, 136);
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
            this.ntbFTablePosX.Size = new System.Drawing.Size(174, 20);
            this.ntbFTablePosX.TabIndex = 42;
            this.ntbFTablePosX.Unit = "mm";
            this.ntbFTablePosX.Value = new decimal(new int[] {
            999999,
            0,
            0,
            131072});
            // 
            // ntbTableXPos
            // 
            this.ntbTableXPos.Caption = "Y軸";
            this.ntbTableXPos.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbTableXPos.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbTableXPos.CaptionWidth = 65;
            this.ntbTableXPos.DiscreteInterval = 0.01F;
            this.ntbTableXPos.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbTableXPos.IncDecButton = false;
            this.ntbTableXPos.Location = new System.Drawing.Point(8, 112);
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
            this.ntbTableXPos.Size = new System.Drawing.Size(174, 20);
            this.ntbTableXPos.TabIndex = 46;
            this.ntbTableXPos.Unit = "mm";
            this.ntbTableXPos.Value = new decimal(new int[] {
            999999,
            0,
            0,
            131072});
            // 
            // ntbFID
            // 
            this.ntbFID.Caption = "FID";
            this.ntbFID.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbFID.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbFID.CaptionWidth = 65;
            this.ntbFID.DiscreteInterval = 0.1F;
            this.ntbFID.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbFID.IncDecButton = false;
            this.ntbFID.Location = new System.Drawing.Point(8, 88);
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
            this.ntbFID.Size = new System.Drawing.Size(174, 20);
            this.ntbFID.TabIndex = 48;
            this.ntbFID.Unit = "mm";
            this.ntbFID.Value = new decimal(new int[] {
            999999,
            0,
            0,
            65536});
            // 
            // ntbFCD
            // 
            this.ntbFCD.Caption = "FCD";
            this.ntbFCD.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbFCD.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbFCD.CaptionWidth = 65;
            this.ntbFCD.DiscreteInterval = 0.1F;
            this.ntbFCD.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbFCD.IncDecButton = false;
            this.ntbFCD.Location = new System.Drawing.Point(8, 64);
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
            this.ntbFCD.Size = new System.Drawing.Size(174, 20);
            this.ntbFCD.TabIndex = 3;
            this.ntbFCD.Unit = "mm";
            this.ntbFCD.Value = new decimal(new int[] {
            999999,
            0,
            0,
            65536});
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
            this.cboSpeed9.Location = new System.Drawing.Point(192, 232);
            this.cboSpeed9.Name = "cboSpeed9";
            this.cboSpeed9.Size = new System.Drawing.Size(81, 20);
            this.cboSpeed9.TabIndex = 50;
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
            this.cboSpeed8.Location = new System.Drawing.Point(192, 208);
            this.cboSpeed8.Name = "cboSpeed8";
            this.cboSpeed8.Size = new System.Drawing.Size(81, 20);
            this.cboSpeed8.TabIndex = 51;
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
            this.cboSpeed7.Location = new System.Drawing.Point(192, 184);
            this.cboSpeed7.Name = "cboSpeed7";
            this.cboSpeed7.Size = new System.Drawing.Size(81, 20);
            this.cboSpeed7.TabIndex = 49;
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
            this.cboSpeed4.Location = new System.Drawing.Point(192, 149);
            this.cboSpeed4.Name = "cboSpeed4";
            this.cboSpeed4.Size = new System.Drawing.Size(81, 20);
            this.cboSpeed4.TabIndex = 57;
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
            this.cboSpeed3.Location = new System.Drawing.Point(192, 112);
            this.cboSpeed3.Name = "cboSpeed3";
            this.cboSpeed3.Size = new System.Drawing.Size(81, 20);
            this.cboSpeed3.TabIndex = 56;
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
            this.cboSpeed6.Location = new System.Drawing.Point(192, 88);
            this.cboSpeed6.Name = "cboSpeed6";
            this.cboSpeed6.Size = new System.Drawing.Size(81, 20);
            this.cboSpeed6.TabIndex = 55;
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
            this.cboSpeed2.Location = new System.Drawing.Point(192, 64);
            this.cboSpeed2.Name = "cboSpeed2";
            this.cboSpeed2.Size = new System.Drawing.Size(81, 20);
            this.cboSpeed2.TabIndex = 54;
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
            this.cboSpeed1.Location = new System.Drawing.Point(192, 40);
            this.cboSpeed1.Name = "cboSpeed1";
            this.cboSpeed1.Size = new System.Drawing.Size(81, 20);
            this.cboSpeed1.TabIndex = 53;
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
            this.cboSpeed0.Location = new System.Drawing.Point(192, 16);
            this.cboSpeed0.Name = "cboSpeed0";
            this.cboSpeed0.Size = new System.Drawing.Size(81, 20);
            this.cboSpeed0.TabIndex = 52;
            // 
            // ntbUpDown
            // 
            this.ntbUpDown.Caption = "高さ";
            this.ntbUpDown.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbUpDown.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbUpDown.CaptionWidth = 65;
            this.ntbUpDown.DiscreteInterval = 0.001F;
            this.ntbUpDown.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbUpDown.IncDecButton = false;
            this.ntbUpDown.Location = new System.Drawing.Point(8, 40);
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
            this.ntbUpDown.Size = new System.Drawing.Size(174, 20);
            this.ntbUpDown.TabIndex = 1;
            this.ntbUpDown.Unit = "mm";
            this.ntbUpDown.Value = new decimal(new int[] {
            999999,
            0,
            0,
            196608});
            // 
            // ntbRotate
            // 
            this.ntbRotate.Caption = "回転";
            this.ntbRotate.CaptionAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ntbRotate.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbRotate.CaptionWidth = 65;
            this.ntbRotate.DiscreteInterval = 0.01F;
            this.ntbRotate.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbRotate.IncDecButton = false;
            this.ntbRotate.Location = new System.Drawing.Point(8, 16);
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
            this.ntbRotate.Size = new System.Drawing.Size(168, 20);
            this.ntbRotate.TabIndex = 0;
            this.ntbRotate.Unit = "度";
            this.ntbRotate.Value = new decimal(new int[] {
            99999,
            0,
            0,
            131072});
            // 
            // ImageList1
            // 
            this.ImageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ImageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.ImageList1.TransparentColor = System.Drawing.Color.Silver;
            // 
            // frmMechaControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 513);
            this.Controls.Add(this.cmdDetails);
            this.Controls.Add(this.cmdMechaAllReset);
            this.Controls.Add(this.fraMechaControl);
            this.Controls.Add(this.fraChangeDetector);
            this.Controls.Add(this.fraHighSpeedCamera);
            this.Controls.Add(this.fraIris);
            this.Controls.Add(this.fraXrayRotate);
            this.Controls.Add(this.fraCollimator);
            this.Controls.Add(this.fraError);
            this.Controls.Add(this.fraUpDown);
            this.Controls.Add(this.fraAutoScanPos);
            this.Controls.Add(this.fraMechaPos);
            this.Name = "frmMechaControl";
            this.Text = "frmMechaControl";
            this.Load += new System.EventHandler(this.frmMechaControl_Load);
            this.fraMechaControl.ResumeLayout(false);
            this.fraMechaControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImgXrayTube)).EndInit();
            this.fraIIMove.ResumeLayout(false);
            this.fraIIMove.PerformLayout();
            this.fraTableRestriction.ResumeLayout(false);
            this.fraChangeDetector.ResumeLayout(false);
            this.fraHighSpeedCamera.ResumeLayout(false);
            this.fraIris.ResumeLayout(false);
            this.fraXrayRotate.ResumeLayout(false);
            this.fraCollimator.ResumeLayout(false);
            this.fraError.ResumeLayout(false);
            this.fraError.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneEXMErrCode)).EndInit();
            this.fraUpDown.ResumeLayout(false);
            this.fraUpDown.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwsldUpDown0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwnePos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwsldUpDown1)).EndInit();
            this.fraAutoScanPos.ResumeLayout(false);
            this.fraMechaPos.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Timer tmrSeqComm;
        internal System.Windows.Forms.Timer tmrMecainf;
        private System.Windows.Forms.Timer tmrTryReloadForms;
        internal System.Windows.Forms.Timer tmrPIOCheck;
        private System.Windows.Forms.Button cmdMechaAllReset;
        private System.Windows.Forms.GroupBox fraMechaControl;
        private System.Windows.Forms.Label lblXrayTube;
        internal System.Windows.Forms.PictureBox ImgXrayTube;
        private System.Windows.Forms.Label lblRot1;
        private System.Windows.Forms.Label lblRot3;
        private System.Windows.Forms.Label lblRot2;
        private System.Windows.Forms.Label lblRot0;
        private CTCheckBox ctchkRotate2;
        private CTCheckBox ctchkRotate1;
        private CTCheckBox ctchkRotate3;
        internal CTCheckBox ctchkRotate0;
        private System.Windows.Forms.Label lblFTable;
        private CWButton cwbtnFineTable1;
        private CWButton cwbtnFineTable0;
        private System.Windows.Forms.Panel fraIIMove;
        private System.Windows.Forms.Label lblIIMove3;
        private System.Windows.Forms.Label lblIIMove2;
        private System.Windows.Forms.Label lblIIMove1;
        private System.Windows.Forms.Label lblIIMove0;
        private CTCheckBox ctchkIIMove3;
        private CTCheckBox ctchkIIMove2;
        private CTCheckBox ctchkIIMove1;
        private CTCheckBox ctchkIIMove0;
        private System.Windows.Forms.Label lblXrayII;
        private CWButton cwbtnRotate1;
        private CWButton cwbtnRotate0;
        private CWButton cwbtnMove1;
        private CWButton cwbtnMove0;
        private CWButton cwbtnMove3;
        private CWButton cwbtnMove2;
        private System.Windows.Forms.GroupBox fraTableRestriction;
        private System.Windows.Forms.Button cmdTableMovePermit;
        private System.Windows.Forms.Button cmdTableMoveRestrict;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.OvalShape ShpFTable;
        private Microsoft.VisualBasic.PowerPacks.OvalShape ShpTable;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape ShpCabinet;
        private System.Windows.Forms.GroupBox fraChangeDetector;
        private CTButton cwbtnChangeDet1;
        private CTButton cwbtnChangeDet0;
        private System.Windows.Forms.GroupBox fraHighSpeedCamera;
        private CTButton cwbtnChangeMode1;
        private CTButton cwbtnChangeMode0;
        private System.Windows.Forms.GroupBox fraIris;
        private System.Windows.Forms.Button cmdIris0;
        private System.Windows.Forms.Button cmdIris5;
        private System.Windows.Forms.Button cmdIris2;
        private System.Windows.Forms.Button cmdIris7;
        private System.Windows.Forms.Button cmdIris6;
        private System.Windows.Forms.Button cmdIris1;
        private System.Windows.Forms.Button cmdIris4;
        private System.Windows.Forms.Button cmdIris3;
        private System.Windows.Forms.GroupBox fraXrayRotate;
        private CWButton cwbtnMove7;
        private CWButton cwbtnMove8;
        private CWButton cwbtnMove6;
        private CWButton cwbtnMove9;
        private CWButton cwbtnRotateXray1;
        private CWButton cwbtnRotateXray0;
        private System.Windows.Forms.GroupBox fraCollimator;
        private System.Windows.Forms.Button cmdCollimator0;
        private System.Windows.Forms.Button cmdCollimator2;
        private System.Windows.Forms.Button cmdCollimator6;
        private System.Windows.Forms.Button cmdCollimator4;
        private System.Windows.Forms.Button cmdCollimator3;
        private System.Windows.Forms.Button cmdCollimator1;
        private System.Windows.Forms.Button cmdCollimator7;
        private System.Windows.Forms.Button cmdCollimator5;
        private System.Windows.Forms.GroupBox fraError;
        private System.Windows.Forms.Label lblMechaStatus5;
        private System.Windows.Forms.Label lblMechaStatus4;
        private System.Windows.Forms.Label lblMechaStatus3;
        private System.Windows.Forms.Label lblMechaStatus2;
        private System.Windows.Forms.Label lblMechaStatus1;
        private System.Windows.Forms.Label lblMechaStatus0;
        private System.Windows.Forms.NumericUpDown cwneEXMErrCode;
        private System.Windows.Forms.Label lblEXMErrCodeHeader;
        private System.Windows.Forms.GroupBox fraUpDown;
        private System.Windows.Forms.Label lblUP;
        private System.Windows.Forms.Label lblDOWN;
        private System.Windows.Forms.Button cmdPosExec;
        private System.Windows.Forms.Label lblMax;
        private System.Windows.Forms.Label lblMiddle;
        private System.Windows.Forms.Label lblMin;
        private CWButton cwbtnUpDown0;
        private CWButton cwbtnUpDown1;
        private System.Windows.Forms.TrackBar cwsldUpDown0;
        private System.Windows.Forms.NumericUpDown cwnePos;
        private System.Windows.Forms.Label Label22;
        private System.Windows.Forms.TrackBar cwsldUpDown1;
        private System.Windows.Forms.GroupBox fraAutoScanPos;
        internal System.Windows.Forms.Button cmdFromTrans;
        private System.Windows.Forms.GroupBox fraMechaPos;
        private NumTextBox ntbXrayPosY;
        private NumTextBox ntbXrayPosX;
        private NumTextBox ntbXrayRotPos;
        private NumTextBox ntbFTablePosY;
        private NumTextBox ntbFTablePosX;
        internal NumTextBox ntbTableXPos;
        private NumTextBox ntbFID;
        private NumTextBox ntbFCD;
        private System.Windows.Forms.ComboBox cboSpeed9;
        private System.Windows.Forms.ComboBox cboSpeed8;
        private System.Windows.Forms.ComboBox cboSpeed7;
        private System.Windows.Forms.ComboBox cboSpeed4;
        private System.Windows.Forms.ComboBox cboSpeed3;
        private System.Windows.Forms.ComboBox cboSpeed6;
        private System.Windows.Forms.ComboBox cboSpeed2;
        private System.Windows.Forms.ComboBox cboSpeed1;
        private System.Windows.Forms.ComboBox cboSpeed0;
        private NumTextBox ntbUpDown;
        private NumTextBox ntbRotate;
        private System.Windows.Forms.ToolTip toolTip;
        internal System.Windows.Forms.ImageList ImageList1;
        internal System.Windows.Forms.Button cmdDetails;
        internal System.Windows.Forms.Button cmdFromSlice;

    }
}