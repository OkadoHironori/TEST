namespace CT30K
{
    partial class frmRoiTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRoiTool));
            this.Toolbar1 = new System.Windows.Forms.ToolStrip();
            this.tsbtnRectangle = new System.Windows.Forms.ToolStripButton();
            this.tsbtnGo = new System.Windows.Forms.ToolStripButton();
            this.tsbtnExit = new System.Windows.Forms.ToolStripButton();
            this.lblBefore = new System.Windows.Forms.Label();
            this.pgbBefore = new System.Windows.Forms.ProgressBar();
            this.pgbAfter = new System.Windows.Forms.ProgressBar();
            this.lblAfter = new System.Windows.Forms.Label();
            this.lblProcess = new System.Windows.Forms.Label();
            this.lblInteg = new System.Windows.Forms.Label();
            this.cwneDPixel0 = new System.Windows.Forms.NumericUpDown();
            this.cwneDPixel1 = new System.Windows.Forms.NumericUpDown();
            this.cwneInteg = new System.Windows.Forms.NumericUpDown();
            this.ImageList1 = new System.Windows.Forms.ImageList(this.components);
            this.Toolbar1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneDPixel0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneDPixel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneInteg)).BeginInit();
            this.SuspendLayout();
            // 
            // Toolbar1
            // 
            this.Toolbar1.AutoSize = false;
            this.Toolbar1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Toolbar1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.Toolbar1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnRectangle,
            this.tsbtnGo,
            this.tsbtnExit});
            this.Toolbar1.Location = new System.Drawing.Point(0, 0);
            this.Toolbar1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.Toolbar1.Name = "Toolbar1";
            this.Toolbar1.Size = new System.Drawing.Size(186, 36);
            this.Toolbar1.TabIndex = 0;
            this.Toolbar1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.Toolbar1_ButtonClick);
            this.Toolbar1.MouseEnter += new System.EventHandler(this.Toolbar1_MouseEnter);
            // 
            // tsbtnRectangle
            // 
            this.tsbtnRectangle.AutoToolTip = false;
            this.tsbtnRectangle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnRectangle.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnRectangle.Image")));
            this.tsbtnRectangle.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnRectangle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnRectangle.Name = "tsbtnRectangle";
            this.tsbtnRectangle.Size = new System.Drawing.Size(28, 33);
            this.tsbtnRectangle.Text = "Rectangle";
            // 
            // tsbtnGo
            // 
            this.tsbtnGo.AutoToolTip = false;
            this.tsbtnGo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnGo.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnGo.Image")));
            this.tsbtnGo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnGo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnGo.Name = "tsbtnGo";
            this.tsbtnGo.Size = new System.Drawing.Size(28, 33);
            this.tsbtnGo.Text = "Go";
            // 
            // tsbtnExit
            // 
            this.tsbtnExit.AutoToolTip = false;
            this.tsbtnExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnExit.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnExit.Image")));
            this.tsbtnExit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnExit.Name = "tsbtnExit";
            this.tsbtnExit.Size = new System.Drawing.Size(28, 33);
            this.tsbtnExit.Text = "Exit";
            // 
            // lblBefore
            // 
            this.lblBefore.Location = new System.Drawing.Point(8, 48);
            this.lblBefore.Name = "lblBefore";
            this.lblBefore.Size = new System.Drawing.Size(77, 14);
            this.lblBefore.TabIndex = 1;
            this.lblBefore.Tag = "20140";
            this.lblBefore.Text = "移動前";
            // 
            // pgbBefore
            // 
            this.pgbBefore.Location = new System.Drawing.Point(8, 64);
            this.pgbBefore.Name = "pgbBefore";
            this.pgbBefore.Size = new System.Drawing.Size(169, 21);
            this.pgbBefore.TabIndex = 2;
            // 
            // pgbAfter
            // 
            this.pgbAfter.Location = new System.Drawing.Point(8, 114);
            this.pgbAfter.Name = "pgbAfter";
            this.pgbAfter.Size = new System.Drawing.Size(169, 21);
            this.pgbAfter.TabIndex = 4;
            // 
            // lblAfter
            // 
            this.lblAfter.Location = new System.Drawing.Point(8, 98);
            this.lblAfter.Name = "lblAfter";
            this.lblAfter.Size = new System.Drawing.Size(77, 14);
            this.lblAfter.TabIndex = 3;
            this.lblAfter.Tag = "20141";
            this.lblAfter.Text = "移動後";
            // 
            // lblProcess
            // 
            this.lblProcess.Location = new System.Drawing.Point(8, 147);
            this.lblProcess.Name = "lblProcess";
            this.lblProcess.Size = new System.Drawing.Size(169, 14);
            this.lblProcess.TabIndex = 5;
            this.lblProcess.Text = "進捗表示";
            // 
            // lblInteg
            // 
            this.lblInteg.Location = new System.Drawing.Point(6, 214);
            this.lblInteg.Name = "lblInteg";
            this.lblInteg.Size = new System.Drawing.Size(77, 14);
            this.lblInteg.TabIndex = 8;
            this.lblInteg.Tag = "12579";
            this.lblInteg.Text = "積分";
            // 
            // cwneDPixel0
            // 
            this.cwneDPixel0.Location = new System.Drawing.Point(112, 167);
            this.cwneDPixel0.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.cwneDPixel0.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cwneDPixel0.Name = "cwneDPixel0";
            this.cwneDPixel0.Size = new System.Drawing.Size(59, 19);
            this.cwneDPixel0.TabIndex = 9;
            this.cwneDPixel0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneDPixel0.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // cwneDPixel1
            // 
            this.cwneDPixel1.DecimalPlaces = 1;
            this.cwneDPixel1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.cwneDPixel1.Location = new System.Drawing.Point(112, 190);
            this.cwneDPixel1.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.cwneDPixel1.Name = "cwneDPixel1";
            this.cwneDPixel1.Size = new System.Drawing.Size(59, 19);
            this.cwneDPixel1.TabIndex = 10;
            this.cwneDPixel1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneDPixel1.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            // 
            // cwneInteg
            // 
            this.cwneInteg.Location = new System.Drawing.Point(112, 212);
            this.cwneInteg.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cwneInteg.Name = "cwneInteg";
            this.cwneInteg.Size = new System.Drawing.Size(59, 19);
            this.cwneInteg.TabIndex = 11;
            this.cwneInteg.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneInteg.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // ImageList1
            // 
            this.ImageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList1.ImageStream")));
            this.ImageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList1.Images.SetKeyName(0, "Rectangle");
            this.ImageList1.Images.SetKeyName(1, "Go");
            this.ImageList1.Images.SetKeyName(2, "Exit");
            // 
            // frmRoiTool
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(186, 232);
            this.Controls.Add(this.cwneInteg);
            this.Controls.Add(this.cwneDPixel1);
            this.Controls.Add(this.cwneDPixel0);
            this.Controls.Add(this.lblInteg);
            this.Controls.Add(this.lblProcess);
            this.Controls.Add(this.pgbAfter);
            this.Controls.Add(this.lblAfter);
            this.Controls.Add(this.pgbBefore);
            this.Controls.Add(this.lblBefore);
            this.Controls.Add(this.Toolbar1);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmRoiTool";
            this.ShowInTaskbar = false;
            this.Tag = "15204";
            this.Text = "自動スキャン位置指定";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmRoiTool_FormClosed);
            this.Load += new System.EventHandler(this.frmRoiTool_Load);
            this.Toolbar1.ResumeLayout(false);
            this.Toolbar1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneDPixel0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneDPixel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneInteg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripButton tsbtnRectangle;
        private System.Windows.Forms.ToolStripButton tsbtnGo;
        private System.Windows.Forms.ToolStripButton tsbtnExit;
        private System.Windows.Forms.Label lblBefore;
        private System.Windows.Forms.ProgressBar pgbBefore;
        private System.Windows.Forms.ProgressBar pgbAfter;
		private System.Windows.Forms.Label lblAfter;
        private System.Windows.Forms.Label lblInteg;
        private System.Windows.Forms.NumericUpDown cwneDPixel0;
        private System.Windows.Forms.NumericUpDown cwneDPixel1;
        private System.Windows.Forms.NumericUpDown cwneInteg;
        private System.Windows.Forms.ImageList ImageList1;
		internal System.Windows.Forms.Label lblProcess;
        internal System.Windows.Forms.ToolStrip Toolbar1;
    }
}