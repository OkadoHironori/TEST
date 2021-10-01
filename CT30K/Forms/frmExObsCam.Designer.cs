﻿namespace CT30K
{
    partial class frmExObsCam
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExObsCam));
            this.mnuPopUp = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuEnlarge = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRoiInput = new System.Windows.Forms.ToolStripMenuItem();
            this.Toolbar1 = new System.Windows.Forms.ToolStrip();
            this.tsbtnArrow = new System.Windows.Forms.ToolStripButton();
            this.tsbtnCircle = new System.Windows.Forms.ToolStripButton();
            this.tsbtnRectangle = new System.Windows.Forms.ToolStripButton();
            this.tsbtnTrace = new System.Windows.Forms.ToolStripButton();
            this.tsbtnSquare = new System.Windows.Forms.ToolStripButton();
            this.tsbtnHLine = new System.Windows.Forms.ToolStripButton();
            this.tsbtnVLine = new System.Windows.Forms.ToolStripButton();
            this.tsbtnLine = new System.Windows.Forms.ToolStripButton();
            this.tsbtnPoint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnCut = new System.Windows.Forms.ToolStripButton();
            this.tsbtnCopy = new System.Windows.Forms.ToolStripButton();
            this.tsbtnPaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnOpen = new System.Windows.Forms.ToolStripButton();
            this.tsbtnSave = new System.Windows.Forms.ToolStripButton();
            this.tsbtnComment = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnGo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnExit = new System.Windows.Forms.ToolStripButton();
            this.fraPrompt = new System.Windows.Forms.Panel();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.cmdRoiOk = new System.Windows.Forms.Button();
            this.cmdRoiCancel = new System.Windows.Forms.Button();
            this.pnlControl = new System.Windows.Forms.Panel();
            this.mnuPopUp.SuspendLayout();
            this.Toolbar1.SuspendLayout();
            this.fraPrompt.SuspendLayout();
            this.pnlControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuPopUp
            // 
            this.mnuPopUp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuEnlarge,
            this.mnuRoiInput});
            this.mnuPopUp.Name = "contextMenuStrip";
            this.mnuPopUp.Size = new System.Drawing.Size(138, 48);
            // 
            // mnuEnlarge
            // 
            this.mnuEnlarge.Name = "mnuEnlarge";
            this.mnuEnlarge.Size = new System.Drawing.Size(137, 22);
            this.mnuEnlarge.Text = "拡大";
            this.mnuEnlarge.Click += new System.EventHandler(this.mnuEnlarge_Click);
            // 
            // mnuRoiInput
            // 
            this.mnuRoiInput.Enabled = false;
            this.mnuRoiInput.Name = "mnuRoiInput";
            this.mnuRoiInput.Size = new System.Drawing.Size(137, 22);
            this.mnuRoiInput.Text = "ROI座標入力";
            this.mnuRoiInput.Click += new System.EventHandler(this.mnuRoiInput_Click);
            // 
            // Toolbar1
            // 
            this.Toolbar1.AutoSize = false;
            this.Toolbar1.BackColor = System.Drawing.SystemColors.Control;
            this.Toolbar1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Toolbar1.Dock = System.Windows.Forms.DockStyle.None;
            this.Toolbar1.ImageScalingSize = new System.Drawing.Size(31, 31);
            this.Toolbar1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnArrow,
            this.tsbtnCircle,
            this.tsbtnRectangle,
            this.tsbtnTrace,
            this.tsbtnSquare,
            this.tsbtnHLine,
            this.tsbtnVLine,
            this.tsbtnLine,
            this.tsbtnPoint,
            this.toolStripSeparator4,
            this.tsbtnCut,
            this.tsbtnCopy,
            this.tsbtnPaste,
            this.toolStripSeparator5,
            this.tsbtnOpen,
            this.tsbtnSave,
            this.tsbtnComment,
            this.toolStripSeparator6,
            this.tsbtnGo,
            this.toolStripSeparator7,
            this.tsbtnExit});
            this.Toolbar1.Location = new System.Drawing.Point(0, 0);
            this.Toolbar1.Name = "Toolbar1";
            this.Toolbar1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.Toolbar1.Size = new System.Drawing.Size(576, 34);
            this.Toolbar1.TabIndex = 7;
            this.Toolbar1.Text = "toolStrip1";
            this.Toolbar1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.Toolbar1_ItemClicked);
            this.Toolbar1.MouseEnter += new System.EventHandler(this.Toolbar1_MouseEnter);
            // 
            // tsbtnArrow
            // 
            this.tsbtnArrow.AutoSize = false;
            this.tsbtnArrow.AutoToolTip = false;
            this.tsbtnArrow.CheckOnClick = true;
            this.tsbtnArrow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnArrow.Enabled = false;
            this.tsbtnArrow.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnArrow.Image")));
            this.tsbtnArrow.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnArrow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnArrow.Name = "tsbtnArrow";
            this.tsbtnArrow.Size = new System.Drawing.Size(31, 31);
            this.tsbtnArrow.Text = "ROIの選択";
            // 
            // tsbtnCircle
            // 
            this.tsbtnCircle.AutoSize = false;
            this.tsbtnCircle.AutoToolTip = false;
            this.tsbtnCircle.CheckOnClick = true;
            this.tsbtnCircle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnCircle.Enabled = false;
            this.tsbtnCircle.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnCircle.Image")));
            this.tsbtnCircle.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnCircle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnCircle.Name = "tsbtnCircle";
            this.tsbtnCircle.Size = new System.Drawing.Size(31, 31);
            this.tsbtnCircle.Tag = "12001";
            this.tsbtnCircle.Text = "円";
            // 
            // tsbtnRectangle
            // 
            this.tsbtnRectangle.AutoSize = false;
            this.tsbtnRectangle.AutoToolTip = false;
            this.tsbtnRectangle.CheckOnClick = true;
            this.tsbtnRectangle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnRectangle.Enabled = false;
            this.tsbtnRectangle.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnRectangle.Image")));
            this.tsbtnRectangle.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnRectangle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnRectangle.Name = "tsbtnRectangle";
            this.tsbtnRectangle.Size = new System.Drawing.Size(31, 31);
            this.tsbtnRectangle.Tag = "12008";
            this.tsbtnRectangle.Text = "長方形";
            // 
            // tsbtnTrace
            // 
            this.tsbtnTrace.AutoSize = false;
            this.tsbtnTrace.AutoToolTip = false;
            this.tsbtnTrace.CheckOnClick = true;
            this.tsbtnTrace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnTrace.Enabled = false;
            this.tsbtnTrace.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnTrace.Image")));
            this.tsbtnTrace.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnTrace.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnTrace.Name = "tsbtnTrace";
            this.tsbtnTrace.Size = new System.Drawing.Size(31, 31);
            this.tsbtnTrace.Tag = "12000";
            this.tsbtnTrace.Text = "トレース";
            // 
            // tsbtnSquare
            // 
            this.tsbtnSquare.AutoSize = false;
            this.tsbtnSquare.AutoToolTip = false;
            this.tsbtnSquare.CheckOnClick = true;
            this.tsbtnSquare.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnSquare.Enabled = false;
            this.tsbtnSquare.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnSquare.Image")));
            this.tsbtnSquare.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnSquare.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSquare.Name = "tsbtnSquare";
            this.tsbtnSquare.Size = new System.Drawing.Size(31, 31);
            this.tsbtnSquare.Tag = "12007";
            this.tsbtnSquare.Text = "正方形";
            // 
            // tsbtnHLine
            // 
            this.tsbtnHLine.AutoSize = false;
            this.tsbtnHLine.AutoToolTip = false;
            this.tsbtnHLine.CheckOnClick = true;
            this.tsbtnHLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnHLine.Enabled = false;
            this.tsbtnHLine.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnHLine.Image")));
            this.tsbtnHLine.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnHLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnHLine.Name = "tsbtnHLine";
            this.tsbtnHLine.Size = new System.Drawing.Size(31, 31);
            this.tsbtnHLine.Tag = "12006";
            this.tsbtnHLine.Text = "水平線";
            // 
            // tsbtnVLine
            // 
            this.tsbtnVLine.AutoSize = false;
            this.tsbtnVLine.AutoToolTip = false;
            this.tsbtnVLine.CheckOnClick = true;
            this.tsbtnVLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnVLine.Enabled = false;
            this.tsbtnVLine.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnVLine.Image")));
            this.tsbtnVLine.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnVLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnVLine.Name = "tsbtnVLine";
            this.tsbtnVLine.Size = new System.Drawing.Size(31, 31);
            this.tsbtnVLine.Tag = "12005";
            this.tsbtnVLine.Text = "垂直線";
            // 
            // tsbtnLine
            // 
            this.tsbtnLine.AutoSize = false;
            this.tsbtnLine.AutoToolTip = false;
            this.tsbtnLine.CheckOnClick = true;
            this.tsbtnLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnLine.Enabled = false;
            this.tsbtnLine.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnLine.Image")));
            this.tsbtnLine.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnLine.Name = "tsbtnLine";
            this.tsbtnLine.Size = new System.Drawing.Size(31, 31);
            this.tsbtnLine.Tag = "12010";
            this.tsbtnLine.Text = "線分";
            // 
            // tsbtnPoint
            // 
            this.tsbtnPoint.AutoSize = false;
            this.tsbtnPoint.AutoToolTip = false;
            this.tsbtnPoint.CheckOnClick = true;
            this.tsbtnPoint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnPoint.Enabled = false;
            this.tsbtnPoint.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnPoint.Image")));
            this.tsbtnPoint.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnPoint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnPoint.Name = "tsbtnPoint";
            this.tsbtnPoint.Size = new System.Drawing.Size(31, 31);
            this.tsbtnPoint.Tag = "12011";
            this.tsbtnPoint.Text = "点";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 34);
            // 
            // tsbtnCut
            // 
            this.tsbtnCut.AutoSize = false;
            this.tsbtnCut.AutoToolTip = false;
            this.tsbtnCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnCut.Enabled = false;
            this.tsbtnCut.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnCut.Image")));
            this.tsbtnCut.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnCut.Name = "tsbtnCut";
            this.tsbtnCut.Size = new System.Drawing.Size(31, 31);
            this.tsbtnCut.Tag = "10133";
            this.tsbtnCut.Text = "ROIの切り取り";
            // 
            // tsbtnCopy
            // 
            this.tsbtnCopy.AutoSize = false;
            this.tsbtnCopy.AutoToolTip = false;
            this.tsbtnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnCopy.Enabled = false;
            this.tsbtnCopy.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnCopy.Image")));
            this.tsbtnCopy.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnCopy.Name = "tsbtnCopy";
            this.tsbtnCopy.Size = new System.Drawing.Size(31, 31);
            this.tsbtnCopy.Tag = "10131";
            this.tsbtnCopy.Text = "ROIのコピー";
            // 
            // tsbtnPaste
            // 
            this.tsbtnPaste.AutoSize = false;
            this.tsbtnPaste.AutoToolTip = false;
            this.tsbtnPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnPaste.Enabled = false;
            this.tsbtnPaste.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnPaste.Image")));
            this.tsbtnPaste.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnPaste.Name = "tsbtnPaste";
            this.tsbtnPaste.Size = new System.Drawing.Size(31, 31);
            this.tsbtnPaste.Tag = "10132";
            this.tsbtnPaste.Text = "ROIの貼り付け";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 34);
            // 
            // tsbtnOpen
            // 
            this.tsbtnOpen.AutoSize = false;
            this.tsbtnOpen.AutoToolTip = false;
            this.tsbtnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnOpen.Enabled = false;
            this.tsbtnOpen.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnOpen.Image")));
            this.tsbtnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnOpen.Name = "tsbtnOpen";
            this.tsbtnOpen.Size = new System.Drawing.Size(30, 28);
            // 
            // tsbtnSave
            // 
            this.tsbtnSave.AutoSize = false;
            this.tsbtnSave.AutoToolTip = false;
            this.tsbtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnSave.Enabled = false;
            this.tsbtnSave.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnSave.Image")));
            this.tsbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSave.Name = "tsbtnSave";
            this.tsbtnSave.Size = new System.Drawing.Size(30, 28);
            // 
            // tsbtnComment
            // 
            this.tsbtnComment.AutoSize = false;
            this.tsbtnComment.AutoToolTip = false;
            this.tsbtnComment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnComment.Enabled = false;
            this.tsbtnComment.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnComment.Image")));
            this.tsbtnComment.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnComment.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnComment.Name = "tsbtnComment";
            this.tsbtnComment.Size = new System.Drawing.Size(31, 31);
            this.tsbtnComment.Tag = "12816";
            this.tsbtnComment.Text = "コメント";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 34);
            // 
            // tsbtnGo
            // 
            this.tsbtnGo.AutoSize = false;
            this.tsbtnGo.AutoToolTip = false;
            this.tsbtnGo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnGo.Enabled = false;
            this.tsbtnGo.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnGo.Image")));
            this.tsbtnGo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnGo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnGo.Name = "tsbtnGo";
            this.tsbtnGo.Size = new System.Drawing.Size(31, 31);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 34);
            // 
            // tsbtnExit
            // 
            this.tsbtnExit.AutoSize = false;
            this.tsbtnExit.AutoToolTip = false;
            this.tsbtnExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnExit.Enabled = false;
            this.tsbtnExit.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnExit.Image")));
            this.tsbtnExit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnExit.Name = "tsbtnExit";
            this.tsbtnExit.Size = new System.Drawing.Size(31, 31);
            this.tsbtnExit.Tag = "12097";
            this.tsbtnExit.Text = "戻る";
            // 
            // fraPrompt
            // 
            this.fraPrompt.BackColor = System.Drawing.SystemColors.Control;
            this.fraPrompt.Controls.Add(this.lblPrompt);
            this.fraPrompt.Controls.Add(this.cmdRoiOk);
            this.fraPrompt.Controls.Add(this.cmdRoiCancel);
            this.fraPrompt.Location = new System.Drawing.Point(579, 0);
            this.fraPrompt.Name = "fraPrompt";
            this.fraPrompt.Size = new System.Drawing.Size(448, 34);
            this.fraPrompt.TabIndex = 16;
            // 
            // lblPrompt
            // 
            this.lblPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Location = new System.Drawing.Point(230, 12);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(53, 12);
            this.lblPrompt.TabIndex = 5;
            this.lblPrompt.Text = "lblPrompt";
            this.lblPrompt.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPrompt.Visible = false;
            // 
            // cmdRoiOk
            // 
            this.cmdRoiOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdRoiOk.Enabled = false;
            this.cmdRoiOk.Location = new System.Drawing.Point(294, 8);
            this.cmdRoiOk.Name = "cmdRoiOk";
            this.cmdRoiOk.Size = new System.Drawing.Size(65, 21);
            this.cmdRoiOk.TabIndex = 6;
            this.cmdRoiOk.Text = "実行";
            this.cmdRoiOk.UseVisualStyleBackColor = true;
            this.cmdRoiOk.Visible = false;
            // 
            // cmdRoiCancel
            // 
            this.cmdRoiCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdRoiCancel.Enabled = false;
            this.cmdRoiCancel.Location = new System.Drawing.Point(374, 8);
            this.cmdRoiCancel.Name = "cmdRoiCancel";
            this.cmdRoiCancel.Size = new System.Drawing.Size(65, 21);
            this.cmdRoiCancel.TabIndex = 7;
            this.cmdRoiCancel.Text = "終了";
            this.cmdRoiCancel.UseVisualStyleBackColor = true;
            this.cmdRoiCancel.Visible = false;
            // 
            // pnlControl
            // 
            this.pnlControl.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pnlControl.Controls.Add(this.Toolbar1);
            this.pnlControl.Controls.Add(this.fraPrompt);
            this.pnlControl.Location = new System.Drawing.Point(0, 1024);
            this.pnlControl.Margin = new System.Windows.Forms.Padding(0);
            this.pnlControl.Name = "pnlControl";
            this.pnlControl.Size = new System.Drawing.Size(1050, 34);
            this.pnlControl.TabIndex = 17;
            // 
            // frmExObsCam
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1024, 1058);
            this.ControlBox = false;
            this.Controls.Add(this.pnlControl);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.Desktop;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmExObsCam";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "外観";
            this.Load += new System.EventHandler(this.frmExObsCam_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmExObsCam_Paint);
            this.DoubleClick += new System.EventHandler(this.frmExObsCam_DoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmExObsCam_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmExObsCam_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmExObsCam_MouseUp);
            this.mnuPopUp.ResumeLayout(false);
            this.Toolbar1.ResumeLayout(false);
            this.Toolbar1.PerformLayout();
            this.fraPrompt.ResumeLayout(false);
            this.fraPrompt.PerformLayout();
            this.pnlControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.ContextMenuStrip mnuPopUp;
        public System.Windows.Forms.ToolStripMenuItem mnuEnlarge;
        private System.Windows.Forms.ToolStripMenuItem mnuRoiInput;
        public System.Windows.Forms.ToolStrip Toolbar1;
        private System.Windows.Forms.ToolStripButton tsbtnArrow;
        private System.Windows.Forms.ToolStripButton tsbtnCircle;
        private System.Windows.Forms.ToolStripButton tsbtnRectangle;
        private System.Windows.Forms.ToolStripButton tsbtnTrace;
        private System.Windows.Forms.ToolStripButton tsbtnSquare;
        private System.Windows.Forms.ToolStripButton tsbtnHLine;
        private System.Windows.Forms.ToolStripButton tsbtnVLine;
        private System.Windows.Forms.ToolStripButton tsbtnLine;
        private System.Windows.Forms.ToolStripButton tsbtnPoint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsbtnCut;
        private System.Windows.Forms.ToolStripButton tsbtnCopy;
        private System.Windows.Forms.ToolStripButton tsbtnPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton tsbtnOpen;
        private System.Windows.Forms.ToolStripButton tsbtnSave;
        private System.Windows.Forms.ToolStripButton tsbtnComment;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        public System.Windows.Forms.ToolStripButton tsbtnGo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton tsbtnExit;
        private System.Windows.Forms.Panel fraPrompt;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.Button cmdRoiOk;
        private System.Windows.Forms.Button cmdRoiCancel;
        private System.Windows.Forms.Panel pnlControl;

    }
}