namespace CT30K
{
    partial class frmCTMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCTMenu));
            this.tmrStatus = new System.Windows.Forms.Timer(this.components);
            this.Toolbar1 = new System.Windows.Forms.ToolStrip();
            this.tsbtnXray = new System.Windows.Forms.ToolStripButton();
            this.tsbtnDoorLock = new System.Windows.Forms.ToolStripButton();
            this.tsbtnIIField = new System.Windows.Forms.ToolStripButton();
            this.tsbtnIIPower = new System.Windows.Forms.ToolStripButton();
            this.tsbtnLiveImage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnOpen = new System.Windows.Forms.ToolStripButton();
            this.tsbtnPrint = new System.Windows.Forms.ToolStripButton();
            this.tsbtnDoubleOblique = new System.Windows.Forms.ToolStripButton();
            this.tsbtnMainte = new System.Windows.Forms.ToolStripButton();
            this.mnuPopUp = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuDebugSub0 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDebugSub1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDebugSub2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDebugSub3 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDebugSub4 = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusBar1 = new System.Windows.Forms.StatusStrip();
            this.tslblDiskFreeSpace = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblScanAvairableNum = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblMatrixSize = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblBinningMode = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblCPU = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblScanPercent = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblScanProgress = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblScanRemain = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblMecha = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblCorrection = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblRunReady = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblDoorInterlock = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblEmergency = new System.Windows.Forms.ToolStripStatusLabel();
            this.ImageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tmrMessage = new System.Windows.Forms.Timer(this.components);
            this.ImageList2 = new System.Windows.Forms.ImageList(this.components);
            this.Toolbar1.SuspendLayout();
            this.mnuPopUp.SuspendLayout();
            this.StatusBar1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrStatus
            // 
            this.tmrStatus.Enabled = true;
            this.tmrStatus.Interval = 1000;
            // 
            // Toolbar1
            // 
            this.Toolbar1.AutoSize = false;
            this.Toolbar1.BackColor = System.Drawing.SystemColors.Control;
            this.Toolbar1.Dock = System.Windows.Forms.DockStyle.Left;
            this.Toolbar1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.Toolbar1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnXray,
            this.tsbtnDoorLock,
            this.tsbtnIIField,
            this.tsbtnIIPower,
            this.tsbtnLiveImage,
            this.toolStripSeparator2,
            this.tsbtnOpen,
            this.tsbtnPrint,
            this.tsbtnDoubleOblique,
            this.tsbtnMainte});
            this.Toolbar1.Location = new System.Drawing.Point(0, 0);
            this.Toolbar1.Name = "Toolbar1";
            this.Toolbar1.Size = new System.Drawing.Size(44, 421);
            this.Toolbar1.TabIndex = 5;
            this.Toolbar1.Text = "toolStrip1";
            // 
            // tsbtnXray
            // 
            this.tsbtnXray.AutoSize = false;
            this.tsbtnXray.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnXray.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnXray.Image")));
            this.tsbtnXray.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnXray.ImageTransparentColor = System.Drawing.Color.Black;
            this.tsbtnXray.Name = "tsbtnXray";
            this.tsbtnXray.Size = new System.Drawing.Size(42, 42);
            this.tsbtnXray.Tag = "12298";
            this.tsbtnXray.Text = "X線";
            // 
            // tsbtnDoorLock
            // 
            this.tsbtnDoorLock.AutoSize = false;
            this.tsbtnDoorLock.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnDoorLock.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnDoorLock.Image")));
            this.tsbtnDoorLock.ImageTransparentColor = System.Drawing.Color.Black;
            this.tsbtnDoorLock.Name = "tsbtnDoorLock";
            this.tsbtnDoorLock.Size = new System.Drawing.Size(42, 42);
            this.tsbtnDoorLock.Tag = "12760";
            this.tsbtnDoorLock.Text = "開く";
            // 
            // tsbtnIIField
            // 
            this.tsbtnIIField.AutoSize = false;
            this.tsbtnIIField.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnIIField.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnIIField.Image")));
            this.tsbtnIIField.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.tsbtnIIField.Name = "tsbtnIIField";
            this.tsbtnIIField.Size = new System.Drawing.Size(42, 42);
            this.tsbtnIIField.Tag = "12810";
            this.tsbtnIIField.Text = "I.I.視野";
            // 
            // tsbtnIIPower
            // 
            this.tsbtnIIPower.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnIIPower.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnIIPower.Image")));
            this.tsbtnIIPower.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnIIPower.Name = "tsbtnIIPower";
            this.tsbtnIIPower.Size = new System.Drawing.Size(42, 36);
            this.tsbtnIIPower.Visible = false;
            // 
            // tsbtnLiveImage
            // 
            this.tsbtnLiveImage.AutoSize = false;
            this.tsbtnLiveImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnLiveImage.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnLiveImage.Image")));
            this.tsbtnLiveImage.ImageTransparentColor = System.Drawing.Color.Black;
            this.tsbtnLiveImage.Name = "tsbtnLiveImage";
            this.tsbtnLiveImage.Size = new System.Drawing.Size(42, 42);
            this.tsbtnLiveImage.Tag = "12595";
            this.tsbtnLiveImage.Text = "ライブ画像";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(42, 6);
            // 
            // tsbtnOpen
            // 
            this.tsbtnOpen.AutoSize = false;
            this.tsbtnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnOpen.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnOpen.Image")));
            this.tsbtnOpen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnOpen.ImageTransparentColor = System.Drawing.Color.Black;
            this.tsbtnOpen.Name = "tsbtnOpen";
            this.tsbtnOpen.Size = new System.Drawing.Size(42, 42);
            this.tsbtnOpen.Text = "画像ファイルを開く";
            // 
            // tsbtnPrint
            // 
            this.tsbtnPrint.AutoSize = false;
            this.tsbtnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnPrint.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnPrint.Image")));
            this.tsbtnPrint.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnPrint.Name = "tsbtnPrint";
            this.tsbtnPrint.Size = new System.Drawing.Size(42, 42);
            this.tsbtnPrint.Tag = "12096";
            this.tsbtnPrint.Text = "印刷";
            this.tsbtnPrint.ToolTipText = "印刷";
            // 
            // tsbtnDoubleOblique
            // 
            this.tsbtnDoubleOblique.AutoSize = false;
            this.tsbtnDoubleOblique.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnDoubleOblique.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnDoubleOblique.Image")));
            this.tsbtnDoubleOblique.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnDoubleOblique.Name = "tsbtnDoubleOblique";
            this.tsbtnDoubleOblique.Size = new System.Drawing.Size(42, 42);
            this.tsbtnDoubleOblique.Tag = "17479";
            this.tsbtnDoubleOblique.Text = "ダブルオブリーク";
            // 
            // tsbtnMainte
            // 
            this.tsbtnMainte.AutoSize = false;
            this.tsbtnMainte.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnMainte.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnMainte.Image")));
            this.tsbtnMainte.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnMainte.Name = "tsbtnMainte";
            this.tsbtnMainte.Size = new System.Drawing.Size(42, 42);
            this.tsbtnMainte.Tag = "12700";
            this.tsbtnMainte.Text = "メンテナンス";
            // 
            // mnuPopUp
            // 
            this.mnuPopUp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDebugSub0,
            this.mnuDebugSub1,
            this.mnuDebugSub2,
            this.mnuDebugSub3,
            this.mnuDebugSub4});
            this.mnuPopUp.Name = "contextMenuStrip";
            this.mnuPopUp.Size = new System.Drawing.Size(191, 114);
            // 
            // mnuDebugSub0
            // 
            this.mnuDebugSub0.Name = "mnuDebugSub0";
            this.mnuDebugSub0.Size = new System.Drawing.Size(190, 22);
            this.mnuDebugSub0.Tag = "";
            this.mnuDebugSub0.Text = "フォーム一覧";
            // 
            // mnuDebugSub1
            // 
            this.mnuDebugSub1.Name = "mnuDebugSub1";
            this.mnuDebugSub1.Size = new System.Drawing.Size(190, 22);
            this.mnuDebugSub1.Tag = "";
            this.mnuDebugSub1.Text = "仮想Ｘ線制御";
            // 
            // mnuDebugSub2
            // 
            this.mnuDebugSub2.Name = "mnuDebugSub2";
            this.mnuDebugSub2.Size = new System.Drawing.Size(190, 22);
            this.mnuDebugSub2.Tag = "";
            this.mnuDebugSub2.Text = "仮想シーケンサ通信";
            // 
            // mnuDebugSub3
            // 
            this.mnuDebugSub3.Name = "mnuDebugSub3";
            this.mnuDebugSub3.Size = new System.Drawing.Size(190, 22);
            this.mnuDebugSub3.Tag = "";
            this.mnuDebugSub3.Text = "ErrMessage関数のテスト";
            // 
            // mnuDebugSub4
            // 
            this.mnuDebugSub4.Name = "mnuDebugSub4";
            this.mnuDebugSub4.Size = new System.Drawing.Size(190, 22);
            this.mnuDebugSub4.Tag = "";
            this.mnuDebugSub4.Text = "タイマーチェック";
            // 
            // StatusBar1
            // 
            this.StatusBar1.AutoSize = false;
            this.StatusBar1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslblDiskFreeSpace,
            this.tslblScanAvairableNum,
            this.tslblMatrixSize,
            this.tslblBinningMode,
            this.tslblCPU,
            this.tslblScanPercent,
            this.tslblScanProgress,
            this.tslblScanRemain,
            this.tslblMecha,
            this.tslblCorrection,
            this.tslblRunReady,
            this.tslblDoorInterlock,
            this.tslblEmergency});
            this.StatusBar1.Location = new System.Drawing.Point(44, 400);
            this.StatusBar1.Name = "StatusBar1";
            this.StatusBar1.Size = new System.Drawing.Size(515, 21);
            this.StatusBar1.TabIndex = 7;
            this.StatusBar1.Text = "statusStrip1";
            // 
            // tslblDiskFreeSpace
            // 
            this.tslblDiskFreeSpace.AutoSize = false;
            this.tslblDiskFreeSpace.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.tslblDiskFreeSpace.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tslblDiskFreeSpace.Name = "tslblDiskFreeSpace";
            this.tslblDiskFreeSpace.Size = new System.Drawing.Size(114, 16);
            // 
            // tslblScanAvairableNum
            // 
            this.tslblScanAvairableNum.AutoSize = false;
            this.tslblScanAvairableNum.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.tslblScanAvairableNum.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tslblScanAvairableNum.Name = "tslblScanAvairableNum";
            this.tslblScanAvairableNum.Size = new System.Drawing.Size(114, 16);
            // 
            // tslblMatrixSize
            // 
            this.tslblMatrixSize.AutoSize = false;
            this.tslblMatrixSize.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.tslblMatrixSize.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tslblMatrixSize.Name = "tslblMatrixSize";
            this.tslblMatrixSize.Size = new System.Drawing.Size(114, 16);
            // 
            // tslblBinningMode
            // 
            this.tslblBinningMode.AutoSize = false;
            this.tslblBinningMode.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.tslblBinningMode.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tslblBinningMode.Name = "tslblBinningMode";
            this.tslblBinningMode.Size = new System.Drawing.Size(134, 16);
            // 
            // tslblCPU
            // 
            this.tslblCPU.AutoSize = false;
            this.tslblCPU.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.tslblCPU.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tslblCPU.Name = "tslblCPU";
            this.tslblCPU.Size = new System.Drawing.Size(134, 16);
            // 
            // tslblScanPercent
            // 
            this.tslblScanPercent.AutoSize = false;
            this.tslblScanPercent.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.tslblScanPercent.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tslblScanPercent.Name = "tslblScanPercent";
            this.tslblScanPercent.Size = new System.Drawing.Size(134, 16);
            // 
            // tslblScanProgress
            // 
            this.tslblScanProgress.AutoSize = false;
            this.tslblScanProgress.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.tslblScanProgress.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tslblScanProgress.Name = "tslblScanProgress";
            this.tslblScanProgress.Size = new System.Drawing.Size(134, 16);
            // 
            // tslblScanRemain
            // 
            this.tslblScanRemain.AutoSize = false;
            this.tslblScanRemain.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.tslblScanRemain.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tslblScanRemain.Name = "tslblScanRemain";
            this.tslblScanRemain.Size = new System.Drawing.Size(134, 16);
            // 
            // tslblMecha
            // 
            this.tslblMecha.AutoSize = false;
            this.tslblMecha.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.tslblMecha.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tslblMecha.Name = "tslblMecha";
            this.tslblMecha.Size = new System.Drawing.Size(134, 16);
            // 
            // tslblCorrection
            // 
            this.tslblCorrection.AutoSize = false;
            this.tslblCorrection.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.tslblCorrection.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tslblCorrection.Name = "tslblCorrection";
            this.tslblCorrection.Size = new System.Drawing.Size(134, 16);
            // 
            // tslblRunReady
            // 
            this.tslblRunReady.AutoSize = false;
            this.tslblRunReady.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.tslblRunReady.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tslblRunReady.Name = "tslblRunReady";
            this.tslblRunReady.Size = new System.Drawing.Size(114, 16);
            // 
            // tslblDoorInterlock
            // 
            this.tslblDoorInterlock.AutoSize = false;
            this.tslblDoorInterlock.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.tslblDoorInterlock.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tslblDoorInterlock.Name = "tslblDoorInterlock";
            this.tslblDoorInterlock.Size = new System.Drawing.Size(92, 16);
            this.tslblDoorInterlock.Text = "インターロック";
            // 
            // tslblEmergency
            // 
            this.tslblEmergency.AutoSize = false;
            this.tslblEmergency.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.tslblEmergency.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tslblEmergency.Name = "tslblEmergency";
            this.tslblEmergency.Size = new System.Drawing.Size(56, 16);
            this.tslblEmergency.Text = "非常停止";
            // 
            // ImageList1
            // 
            this.ImageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList1.ImageStream")));
            this.ImageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList1.Images.SetKeyName(0, "Open");
            this.ImageList1.Images.SetKeyName(1, "Print");
            this.ImageList1.Images.SetKeyName(2, "Mainte");
            this.ImageList1.Images.SetKeyName(3, "DoubleOblique");
            this.ImageList1.Images.SetKeyName(4, "I.I.On");
            this.ImageList1.Images.SetKeyName(5, "I.I.Off");
            this.ImageList1.Images.SetKeyName(6, "2\'\'");
            this.ImageList1.Images.SetKeyName(7, "2.5\'\'");
            this.ImageList1.Images.SetKeyName(8, "4\'\'");
            this.ImageList1.Images.SetKeyName(9, "4.5\'\'");
            this.ImageList1.Images.SetKeyName(10, "6\'\'");
            this.ImageList1.Images.SetKeyName(11, "9\'\'");
            this.ImageList1.Images.SetKeyName(12, "Locked");
            this.ImageList1.Images.SetKeyName(13, "Unlocked");
            this.ImageList1.Images.SetKeyName(14, "CannotLock");
            this.ImageList1.Images.SetKeyName(15, "XrayOnNotAvail");
            this.ImageList1.Images.SetKeyName(16, "XrayOffAvail");
            this.ImageList1.Images.SetKeyName(17, "XrayOffNotAvail");
            this.ImageList1.Images.SetKeyName(18, "XrayOnAvail");
            this.ImageList1.Images.SetKeyName(19, "Wizard");
            this.ImageList1.Images.SetKeyName(20, "CameraOff");
            this.ImageList1.Images.SetKeyName(21, "VideoOn");
            this.ImageList1.Images.SetKeyName(22, "VideoOff");
            this.ImageList1.Images.SetKeyName(23, " - ");
            // 
            // ImageList2
            // 
            this.ImageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList2.ImageStream")));
            this.ImageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList2.Images.SetKeyName(0, "Green");
            this.ImageList2.Images.SetKeyName(1, "Yellow");
            this.ImageList2.Images.SetKeyName(2, "Red");
            this.ImageList2.Images.SetKeyName(3, "DarkGreen");
            this.ImageList2.Images.SetKeyName(4, "DarkRed");
            // 
            // frmCTMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 421);
            this.Controls.Add(this.StatusBar1);
            this.Controls.Add(this.Toolbar1);
            this.Name = "frmCTMenu";
            this.Text = "frmCTMenu";
            this.Toolbar1.ResumeLayout(false);
            this.Toolbar1.PerformLayout();
            this.mnuPopUp.ResumeLayout(false);
            this.StatusBar1.ResumeLayout(false);
            this.StatusBar1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Timer tmrStatus;
        public System.Windows.Forms.ToolStrip Toolbar1;
        private System.Windows.Forms.ToolStripButton tsbtnXray;
        private System.Windows.Forms.ToolStripButton tsbtnDoorLock;
        internal System.Windows.Forms.ToolStripButton tsbtnIIField;
        private System.Windows.Forms.ToolStripButton tsbtnIIPower;
        internal System.Windows.Forms.ToolStripButton tsbtnLiveImage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbtnOpen;
        private System.Windows.Forms.ToolStripButton tsbtnPrint;
        private System.Windows.Forms.ToolStripButton tsbtnDoubleOblique;
        private System.Windows.Forms.ToolStripButton tsbtnMainte;
        private System.Windows.Forms.ContextMenuStrip mnuPopUp;
        private System.Windows.Forms.ToolStripMenuItem mnuDebugSub0;
        private System.Windows.Forms.ToolStripMenuItem mnuDebugSub1;
        private System.Windows.Forms.ToolStripMenuItem mnuDebugSub2;
        private System.Windows.Forms.ToolStripMenuItem mnuDebugSub3;
        private System.Windows.Forms.ToolStripMenuItem mnuDebugSub4;
        private System.Windows.Forms.StatusStrip StatusBar1;
        private System.Windows.Forms.ToolStripStatusLabel tslblDiskFreeSpace;
        private System.Windows.Forms.ToolStripStatusLabel tslblScanAvairableNum;
        private System.Windows.Forms.ToolStripStatusLabel tslblMatrixSize;
        private System.Windows.Forms.ToolStripStatusLabel tslblBinningMode;
        private System.Windows.Forms.ToolStripStatusLabel tslblCPU;
        private System.Windows.Forms.ToolStripStatusLabel tslblScanPercent;
        private System.Windows.Forms.ToolStripStatusLabel tslblScanProgress;
        private System.Windows.Forms.ToolStripStatusLabel tslblScanRemain;
        private System.Windows.Forms.ToolStripStatusLabel tslblMecha;
        private System.Windows.Forms.ToolStripStatusLabel tslblCorrection;
        private System.Windows.Forms.ToolStripStatusLabel tslblRunReady;
        private System.Windows.Forms.ToolStripStatusLabel tslblDoorInterlock;
        private System.Windows.Forms.ToolStripStatusLabel tslblEmergency;
        public System.Windows.Forms.ImageList ImageList1;
        private System.Windows.Forms.ImageList ImageList2;
        internal System.Windows.Forms.Timer tmrMessage;
    }
}