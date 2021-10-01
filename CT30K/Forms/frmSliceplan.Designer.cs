namespace CT30K
{
	partial class frmSliceplan
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSliceplan));
            this.Toolbar1 = new System.Windows.Forms.ToolStrip();
            this.Toolbar1_1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnOpen = new System.Windows.Forms.ToolStripButton();
            this.tsbtnSave = new System.Windows.Forms.ToolStripButton();
            this.Toolbar1_4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnComment = new System.Windows.Forms.ToolStripButton();
            this.ImageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lvwTable = new System.Windows.Forms.ListView();
            this.No = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SliceName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Conebeam = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Label2 = new System.Windows.Forms.Label();
            this.txtSliceplan = new System.Windows.Forms.TextBox();
            this.cmdImgSelect = new System.Windows.Forms.Button();
            this.cmdImgDelete = new System.Windows.Forms.Button();
            this.fraDataMode = new System.Windows.Forms.GroupBox();
            this.optConeBeam = new System.Windows.Forms.RadioButton();
            this.optScan = new System.Windows.Forms.RadioButton();
            this.cmdTableSelect = new System.Windows.Forms.Button();
            this.cmdExit = new System.Windows.Forms.Button();
            this.cmdImgSearch = new System.Windows.Forms.Button();
            this.Toolbar1.SuspendLayout();
            this.fraDataMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // Toolbar1
            // 
            this.Toolbar1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.Toolbar1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Toolbar1_1,
            this.tsbtnOpen,
            this.tsbtnSave,
            this.Toolbar1_4,
            this.tsbtnComment});
            this.Toolbar1.Location = new System.Drawing.Point(0, 0);
            this.Toolbar1.Name = "Toolbar1";
            this.Toolbar1.Size = new System.Drawing.Size(687, 31);
            this.Toolbar1.TabIndex = 0;
            // 
            // Toolbar1_1
            // 
            this.Toolbar1_1.Name = "Toolbar1_1";
            this.Toolbar1_1.Size = new System.Drawing.Size(6, 31);
            // 
            // tsbtnOpen
            // 
            this.tsbtnOpen.AutoToolTip = false;
            this.tsbtnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnOpen.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnOpen.Image")));
            this.tsbtnOpen.ImageTransparentColor = System.Drawing.Color.Silver;
            this.tsbtnOpen.Name = "tsbtnOpen";
            this.tsbtnOpen.Size = new System.Drawing.Size(23, 28);
            this.tsbtnOpen.Text = "Open";
            this.tsbtnOpen.Click += new System.EventHandler(this.Toolbar1_ButtonClick);
            // 
            // tsbtnSave
            // 
            this.tsbtnSave.AutoToolTip = false;
            this.tsbtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnSave.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnSave.Image")));
            this.tsbtnSave.ImageTransparentColor = System.Drawing.Color.Silver;
            this.tsbtnSave.Name = "tsbtnSave";
            this.tsbtnSave.Size = new System.Drawing.Size(23, 28);
            this.tsbtnSave.Text = "Save";
            this.tsbtnSave.Click += new System.EventHandler(this.Toolbar1_ButtonClick);
            // 
            // Toolbar1_4
            // 
            this.Toolbar1_4.Name = "Toolbar1_4";
            this.Toolbar1_4.Size = new System.Drawing.Size(6, 31);
            // 
            // tsbtnComment
            // 
            this.tsbtnComment.AutoToolTip = false;
            this.tsbtnComment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnComment.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnComment.Image")));
            this.tsbtnComment.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnComment.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnComment.Name = "tsbtnComment";
            this.tsbtnComment.Size = new System.Drawing.Size(28, 28);
            this.tsbtnComment.Text = "Comment";
            this.tsbtnComment.Click += new System.EventHandler(this.Toolbar1_ButtonClick);
            // 
            // ImageList1
            // 
            this.ImageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList1.ImageStream")));
            this.ImageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList1.Images.SetKeyName(0, "OPEN.BMP");
            this.ImageList1.Images.SetKeyName(1, "SAVE.BMP");
            this.ImageList1.Images.SetKeyName(2, "Comment.ico");
            // 
            // lvwTable
            // 
            this.lvwTable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.No,
            this.SliceName,
            this.Conebeam});
            this.lvwTable.Font = new System.Drawing.Font("MS UI Gothic", 9.75F);
            this.lvwTable.FullRowSelect = true;
            this.lvwTable.GridLines = true;
            this.lvwTable.HoverSelection = true;
            this.lvwTable.LabelEdit = true;
            this.lvwTable.Location = new System.Drawing.Point(12, 52);
            this.lvwTable.Name = "lvwTable";
            this.lvwTable.Size = new System.Drawing.Size(537, 325);
            this.lvwTable.TabIndex = 1;
            this.lvwTable.UseCompatibleStateImageBehavior = false;
            this.lvwTable.View = System.Windows.Forms.View.Details;
            this.lvwTable.SelectedIndexChanged += new System.EventHandler(this.lvwTable_SelectedIndexChanged);
            this.lvwTable.Click += new System.EventHandler(this.lvwTable_Click);
            this.lvwTable.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvwTable_KeyDown);
            // 
            // No
            // 
            this.No.Text = "No.";
            this.No.Width = 59;
            // 
            // SliceName
            // 
            this.SliceName.Tag = "12802";
            this.SliceName.Text = "#スライス名";
            this.SliceName.Width = 380;
            // 
            // Conebeam
            // 
            this.Conebeam.Text = "Conebeam";
            this.Conebeam.Width = 76;
            // 
            // Label2
            // 
            this.Label2.Font = new System.Drawing.Font("MS UI Gothic", 11.25F);
            this.Label2.Location = new System.Drawing.Point(3, 400);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(144, 15);
            this.Label2.TabIndex = 2;
            this.Label2.Tag = "12294";
            this.Label2.Text = "#使用するテーブル：";
            this.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtSliceplan
            // 
            this.txtSliceplan.BackColor = System.Drawing.SystemColors.Control;
            this.txtSliceplan.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.txtSliceplan.Location = new System.Drawing.Point(152, 396);
            this.txtSliceplan.Name = "txtSliceplan";
            this.txtSliceplan.Size = new System.Drawing.Size(397, 23);
            this.txtSliceplan.TabIndex = 3;
            this.txtSliceplan.Text = "C:\\CTUSER\\ｽﾗｲｽﾌﾟﾗﾝﾃｰﾌﾞﾙ\\TEST-SPL.csv";
            // 
            // cmdImgSelect
            // 
            this.cmdImgSelect.Font = new System.Drawing.Font("MS UI Gothic", 11.25F);
            this.cmdImgSelect.Location = new System.Drawing.Point(568, 144);
            this.cmdImgSelect.Name = "cmdImgSelect";
            this.cmdImgSelect.Size = new System.Drawing.Size(105, 29);
            this.cmdImgSelect.TabIndex = 4;
            this.cmdImgSelect.Tag = "10009";
            this.cmdImgSelect.Text = "#参照...";
            this.cmdImgSelect.UseVisualStyleBackColor = true;
            this.cmdImgSelect.Click += new System.EventHandler(this.cmdImgSelect_Click);
            // 
            // cmdImgDelete
            // 
            this.cmdImgDelete.Enabled = false;
            this.cmdImgDelete.Font = new System.Drawing.Font("MS UI Gothic", 11.25F);
            this.cmdImgDelete.Location = new System.Drawing.Point(568, 188);
            this.cmdImgDelete.Name = "cmdImgDelete";
            this.cmdImgDelete.Size = new System.Drawing.Size(105, 29);
            this.cmdImgDelete.TabIndex = 5;
            this.cmdImgDelete.Tag = "10010";
            this.cmdImgDelete.Text = "#削除";
            this.cmdImgDelete.UseVisualStyleBackColor = true;
            this.cmdImgDelete.Click += new System.EventHandler(this.cmdImgDelete_Click);
            // 
            // fraDataMode
            // 
            this.fraDataMode.Controls.Add(this.optConeBeam);
            this.fraDataMode.Controls.Add(this.optScan);
            this.fraDataMode.Font = new System.Drawing.Font("MS UI Gothic", 11.25F);
            this.fraDataMode.Location = new System.Drawing.Point(564, 48);
            this.fraDataMode.Name = "fraDataMode";
            this.fraDataMode.Size = new System.Drawing.Size(117, 73);
            this.fraDataMode.TabIndex = 6;
            this.fraDataMode.TabStop = false;
            this.fraDataMode.Tag = "9014";
            this.fraDataMode.Text = "#データモード";
            // 
            // optConeBeam
            // 
            this.optConeBeam.AutoSize = true;
            this.optConeBeam.Font = new System.Drawing.Font("MS UI Gothic", 9.75F);
            this.optConeBeam.Location = new System.Drawing.Point(16, 44);
            this.optConeBeam.Name = "optConeBeam";
            this.optConeBeam.Size = new System.Drawing.Size(93, 17);
            this.optConeBeam.TabIndex = 8;
            this.optConeBeam.Tag = "12821";
            this.optConeBeam.Text = "#コーンビーム";
            this.optConeBeam.UseVisualStyleBackColor = true;
            this.optConeBeam.CheckedChanged += new System.EventHandler(this.optConeBeam_CheckedChanged);
            // 
            // optScan
            // 
            this.optScan.AutoSize = true;
            this.optScan.Checked = true;
            this.optScan.Font = new System.Drawing.Font("MS UI Gothic", 9.75F);
            this.optScan.Location = new System.Drawing.Point(16, 20);
            this.optScan.Name = "optScan";
            this.optScan.Size = new System.Drawing.Size(72, 17);
            this.optScan.TabIndex = 7;
            this.optScan.TabStop = true;
            this.optScan.Tag = "12028";
            this.optScan.Text = "#スキャン";
            this.optScan.UseVisualStyleBackColor = true;
            this.optScan.CheckedChanged += new System.EventHandler(this.optScan_CheckedChanged);
            // 
            // cmdTableSelect
            // 
            this.cmdTableSelect.Font = new System.Drawing.Font("MS UI Gothic", 11.25F);
            this.cmdTableSelect.Location = new System.Drawing.Point(568, 392);
            this.cmdTableSelect.Name = "cmdTableSelect";
            this.cmdTableSelect.Size = new System.Drawing.Size(105, 29);
            this.cmdTableSelect.TabIndex = 9;
            this.cmdTableSelect.Tag = "10015";
            this.cmdTableSelect.Text = "#変更...";
            this.cmdTableSelect.UseVisualStyleBackColor = true;
            this.cmdTableSelect.Click += new System.EventHandler(this.cmdTableSelect_Click);
            // 
            // cmdExit
            // 
            this.cmdExit.Font = new System.Drawing.Font("MS UI Gothic", 11.25F);
            this.cmdExit.Location = new System.Drawing.Point(568, 438);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(105, 29);
            this.cmdExit.TabIndex = 10;
            this.cmdExit.Tag = "10006";
            this.cmdExit.Text = "#終了";
            this.cmdExit.UseVisualStyleBackColor = true;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // cmdImgSearch
            // 
            this.cmdImgSearch.Enabled = false;
            this.cmdImgSearch.Font = new System.Drawing.Font("MS UI Gothic", 11.25F);
            this.cmdImgSearch.Location = new System.Drawing.Point(568, 232);
            this.cmdImgSearch.Name = "cmdImgSearch";
            this.cmdImgSearch.Size = new System.Drawing.Size(105, 29);
            this.cmdImgSearch.TabIndex = 11;
            this.cmdImgSearch.Tag = "10027";
            this.cmdImgSearch.Text = "#検索";
            this.cmdImgSearch.UseVisualStyleBackColor = true;
            this.cmdImgSearch.Click += new System.EventHandler(this.cmdImgSearch_Click);
            // 
            // frmSliceplan
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(687, 485);
            this.ControlBox = false;
            this.Controls.Add(this.cmdImgSearch);
            this.Controls.Add(this.cmdExit);
            this.Controls.Add(this.cmdTableSelect);
            this.Controls.Add(this.fraDataMode);
            this.Controls.Add(this.cmdImgDelete);
            this.Controls.Add(this.cmdImgSelect);
            this.Controls.Add(this.txtSliceplan);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.lvwTable);
            this.Controls.Add(this.Toolbar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(107, 73);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSliceplan";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "";
            this.Text = "スライスプラン";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSliceplan_FormClosed);
            this.Load += new System.EventHandler(this.frmSliceplan_Load);
            this.Toolbar1.ResumeLayout(false);
            this.Toolbar1.PerformLayout();
            this.fraDataMode.ResumeLayout(false);
            this.fraDataMode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip Toolbar1;
		private System.Windows.Forms.ToolStripSeparator Toolbar1_1;
		private System.Windows.Forms.ToolStripButton tsbtnOpen;
		private System.Windows.Forms.ToolStripButton tsbtnSave;
		private System.Windows.Forms.ToolStripSeparator Toolbar1_4;
		private System.Windows.Forms.ToolStripButton tsbtnComment;
		private System.Windows.Forms.ImageList ImageList1;
		private System.Windows.Forms.ListView lvwTable;
		private System.Windows.Forms.Label Label2;
		private System.Windows.Forms.TextBox txtSliceplan;
		private System.Windows.Forms.Button cmdImgSelect;
		private System.Windows.Forms.Button cmdImgDelete;
		private System.Windows.Forms.GroupBox fraDataMode;
		private System.Windows.Forms.RadioButton optConeBeam;
		private System.Windows.Forms.RadioButton optScan;
		private System.Windows.Forms.Button cmdTableSelect;
		private System.Windows.Forms.Button cmdExit;
		private System.Windows.Forms.ColumnHeader No;
		private System.Windows.Forms.ColumnHeader SliceName;
		private System.Windows.Forms.ColumnHeader Conebeam;
        private System.Windows.Forms.Button cmdImgSearch;

	}
}