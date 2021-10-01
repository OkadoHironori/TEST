namespace CT30K
{
	partial class frmAllwlww
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
            this.lblSliceName = new System.Windows.Forms.Label();
            this.lblNumTitle = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.lblMaxNum = new System.Windows.Forms.Label();
            this.lblWLevel = new System.Windows.Forms.Label();
            this.lblColon0 = new System.Windows.Forms.Label();
            this.lblColon1 = new System.Windows.Forms.Label();
            this.lblWWidth = new System.Windows.Forms.Label();
            this.lblGamma = new System.Windows.Forms.Label();
            this.cwneGamma = new System.Windows.Forms.NumericUpDown();
            this.cmdImgSelect = new System.Windows.Forms.Button();
            this.cmdImgDelete = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdEnd = new System.Windows.Forms.Button();
            this.lblProcess = new System.Windows.Forms.Label();
            this.fraSpecify = new System.Windows.Forms.GroupBox();
            this.optFile = new System.Windows.Forms.RadioButton();
            this.optFolder = new System.Windows.Forms.RadioButton();
            this.lblColon2 = new System.Windows.Forms.Label();
            this.cwneWL = new System.Windows.Forms.NumericUpDown();
            this.cwneWW = new System.Windows.Forms.NumericUpDown();
            this.lstImgFile = new CT30K.CTListBox();
            ((System.ComponentModel.ISupportInitialize)(this.cwneGamma)).BeginInit();
            this.fraSpecify.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneWL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneWW)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSliceName
            // 
            this.lblSliceName.AutoSize = true;
            this.lblSliceName.Location = new System.Drawing.Point(6, 12);
            this.lblSliceName.Name = "lblSliceName";
            this.lblSliceName.Size = new System.Drawing.Size(58, 12);
            this.lblSliceName.TabIndex = 0;
            this.lblSliceName.Tag = "12802";
            this.lblSliceName.Text = "#スライス名";
            this.lblSliceName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblNumTitle
            // 
            this.lblNumTitle.AutoSize = true;
            this.lblNumTitle.Location = new System.Drawing.Point(176, 12);
            this.lblNumTitle.Name = "lblNumTitle";
            this.lblNumTitle.Size = new System.Drawing.Size(65, 12);
            this.lblNumTitle.TabIndex = 1;
            this.lblNumTitle.Tag = "12052";
            this.lblNumTitle.Text = "#入力枚数 :";
            this.lblNumTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblCount
            // 
            this.lblCount.Location = new System.Drawing.Point(244, 12);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(35, 17);
            this.lblCount.TabIndex = 2;
            this.lblCount.Text = "0";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblMaxNum
            // 
            this.lblMaxNum.AutoSize = true;
            this.lblMaxNum.Location = new System.Drawing.Point(280, 12);
            this.lblMaxNum.Name = "lblMaxNum";
            this.lblMaxNum.Size = new System.Drawing.Size(89, 12);
            this.lblMaxNum.TabIndex = 3;
            this.lblMaxNum.Text = "枚（最大1000枚）";
            // 
            // lblWLevel
            // 
            this.lblWLevel.AutoSize = true;
            this.lblWLevel.Location = new System.Drawing.Point(8, 188);
            this.lblWLevel.Name = "lblWLevel";
            this.lblWLevel.Size = new System.Drawing.Size(83, 12);
            this.lblWLevel.TabIndex = 5;
            this.lblWLevel.Tag = "12825";
            this.lblWLevel.Text = "#ウィンドウレベル";
            this.lblWLevel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblColon0
            // 
            this.lblColon0.AutoSize = true;
            this.lblColon0.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblColon0.Location = new System.Drawing.Point(98, 184);
            this.lblColon0.Name = "lblColon0";
            this.lblColon0.Size = new System.Drawing.Size(11, 14);
            this.lblColon0.TabIndex = 6;
            this.lblColon0.Text = ":";
            // 
            // lblColon1
            // 
            this.lblColon1.AutoSize = true;
            this.lblColon1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblColon1.Location = new System.Drawing.Point(98, 208);
            this.lblColon1.Name = "lblColon1";
            this.lblColon1.Size = new System.Drawing.Size(11, 14);
            this.lblColon1.TabIndex = 9;
            this.lblColon1.Text = ":";
            // 
            // lblWWidth
            // 
            this.lblWWidth.AutoSize = true;
            this.lblWWidth.Location = new System.Drawing.Point(28, 212);
            this.lblWWidth.Name = "lblWWidth";
            this.lblWWidth.Size = new System.Drawing.Size(66, 12);
            this.lblWWidth.TabIndex = 8;
            this.lblWWidth.Tag = "12826";
            this.lblWWidth.Text = "#ウィンドウ幅";
            this.lblWWidth.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblGamma
            // 
            this.lblGamma.AutoSize = true;
            this.lblGamma.Location = new System.Drawing.Point(171, 188);
            this.lblGamma.Name = "lblGamma";
            this.lblGamma.Size = new System.Drawing.Size(39, 12);
            this.lblGamma.TabIndex = 21;
            this.lblGamma.Tag = "12841";
            this.lblGamma.Text = "#ガンマ";
            this.lblGamma.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cwneGamma
            // 
            this.cwneGamma.DecimalPlaces = 2;
            this.cwneGamma.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneGamma.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.cwneGamma.Location = new System.Drawing.Point(217, 184);
            this.cwneGamma.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.cwneGamma.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.cwneGamma.Name = "cwneGamma";
            this.cwneGamma.Size = new System.Drawing.Size(61, 22);
            this.cwneGamma.TabIndex = 20;
            this.cwneGamma.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneGamma.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.cwneGamma.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cmdImgSelect
            // 
            this.cmdImgSelect.Location = new System.Drawing.Point(380, 28);
            this.cmdImgSelect.Name = "cmdImgSelect";
            this.cmdImgSelect.Size = new System.Drawing.Size(65, 21);
            this.cmdImgSelect.TabIndex = 11;
            this.cmdImgSelect.Tag = "10009";
            this.cmdImgSelect.Text = "#参照";
            this.cmdImgSelect.UseVisualStyleBackColor = true;
            // 
            // cmdImgDelete
            // 
            this.cmdImgDelete.Enabled = false;
            this.cmdImgDelete.Location = new System.Drawing.Point(380, 140);
            this.cmdImgDelete.Name = "cmdImgDelete";
            this.cmdImgDelete.Size = new System.Drawing.Size(65, 21);
            this.cmdImgDelete.TabIndex = 15;
            this.cmdImgDelete.Tag = "10010";
            this.cmdImgDelete.Text = "#削除";
            this.cmdImgDelete.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Enabled = false;
            this.cmdOK.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdOK.Location = new System.Drawing.Point(285, 196);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(81, 29);
            this.cmdOK.TabIndex = 16;
            this.cmdOK.Tag = "10005";
            this.cmdOK.Text = "#実行";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdEnd
            // 
            this.cmdEnd.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdEnd.Location = new System.Drawing.Point(372, 196);
            this.cmdEnd.Name = "cmdEnd";
            this.cmdEnd.Size = new System.Drawing.Size(81, 29);
            this.cmdEnd.TabIndex = 17;
            this.cmdEnd.Tag = "10006";
            this.cmdEnd.Text = "#終了";
            this.cmdEnd.UseVisualStyleBackColor = true;
            this.cmdEnd.Click += new System.EventHandler(this.cmdEnd_Click);
            // 
            // lblProcess
            // 
            this.lblProcess.Location = new System.Drawing.Point(280, 180);
            this.lblProcess.Name = "lblProcess";
            this.lblProcess.Size = new System.Drawing.Size(65, 17);
            this.lblProcess.TabIndex = 19;
            this.lblProcess.Text = "9999/9999";
            this.lblProcess.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblProcess.Visible = false;
            // 
            // fraSpecify
            // 
            this.fraSpecify.Controls.Add(this.optFile);
            this.fraSpecify.Controls.Add(this.optFolder);
            this.fraSpecify.Location = new System.Drawing.Point(364, 52);
            this.fraSpecify.Name = "fraSpecify";
            this.fraSpecify.Size = new System.Drawing.Size(89, 65);
            this.fraSpecify.TabIndex = 12;
            this.fraSpecify.TabStop = false;
            this.fraSpecify.Tag = "12751";
            this.fraSpecify.Text = "#指定";
            // 
            // optFile
            // 
            this.optFile.AutoSize = true;
            this.optFile.Location = new System.Drawing.Point(12, 40);
            this.optFile.Name = "optFile";
            this.optFile.Size = new System.Drawing.Size(63, 16);
            this.optFile.TabIndex = 14;
            this.optFile.TabStop = true;
            this.optFile.Tag = "12753";
            this.optFile.Text = "#ファイル";
            this.optFile.UseVisualStyleBackColor = true;
            this.optFile.CheckedChanged += new System.EventHandler(this.optFile_CheckedChanged);
            // 
            // optFolder
            // 
            this.optFolder.AutoSize = true;
            this.optFolder.Location = new System.Drawing.Point(12, 16);
            this.optFolder.Name = "optFolder";
            this.optFolder.Size = new System.Drawing.Size(64, 16);
            this.optFolder.TabIndex = 13;
            this.optFolder.TabStop = true;
            this.optFolder.Tag = "12752";
            this.optFolder.Text = "#フォルダ";
            this.optFolder.UseVisualStyleBackColor = true;
            this.optFolder.CheckedChanged += new System.EventHandler(this.optFolder_CheckedChanged);
            // 
            // lblColon2
            // 
            this.lblColon2.AutoSize = true;
            this.lblColon2.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblColon2.Location = new System.Drawing.Point(74, 9);
            this.lblColon2.Name = "lblColon2";
            this.lblColon2.Size = new System.Drawing.Size(11, 14);
            this.lblColon2.TabIndex = 18;
            this.lblColon2.Text = ":";
            // 
            // cwneWL
            // 
            this.cwneWL.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneWL.Location = new System.Drawing.Point(106, 184);
            this.cwneWL.Maximum = new decimal(new int[] {
            8191,
            0,
            0,
            0});
            this.cwneWL.Minimum = new decimal(new int[] {
            8192,
            0,
            0,
            -2147483648});
            this.cwneWL.Name = "cwneWL";
            this.cwneWL.Size = new System.Drawing.Size(61, 22);
            this.cwneWL.TabIndex = 7;
            this.cwneWL.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cwneWW
            // 
            this.cwneWW.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneWW.Location = new System.Drawing.Point(106, 208);
            this.cwneWW.Maximum = new decimal(new int[] {
            16384,
            0,
            0,
            0});
            this.cwneWW.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cwneWW.Name = "cwneWW";
            this.cwneWW.Size = new System.Drawing.Size(61, 22);
            this.cwneWW.TabIndex = 10;
            this.cwneWW.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneWW.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // lstImgFile
            // 
            this.lstImgFile.CountLabel = null;
            this.lstImgFile.DefaultPath = "";
            this.lstImgFile.DeleteButton = null;
            this.lstImgFile.Description = "";
            this.lstImgFile.Extension = "";
            this.lstImgFile.HowToAdd = CT30K.CTListBox.HowToAddType.FromFileList;
            this.lstImgFile.InfCheck = false;
            this.lstImgFile.Location = new System.Drawing.Point(6, 28);
            this.lstImgFile.Max = 0;
            this.lstImgFile.Name = "lstImgFile";
            this.lstImgFile.NumberingDigits = 0;
            this.lstImgFile.ReferenceButton = null;
            this.lstImgFile.Size = new System.Drawing.Size(353, 148);
            this.lstImgFile.TabIndex = 4;
            this.lstImgFile.Changed += new System.EventHandler(this.lstImgFile_Changed);
            // 
            // frmAllwlww
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(457, 235);
            this.Name = "frmAllwlww";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "10513";
            this.Text = "#階調一括変換";
            this.Load += new System.EventHandler(this.frmAllwlww_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cwneGamma)).EndInit();
            this.fraSpecify.ResumeLayout(false);
            this.fraSpecify.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneWL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneWW)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblSliceName;
		private System.Windows.Forms.Label lblNumTitle;
		private System.Windows.Forms.Label lblCount;
		private System.Windows.Forms.Label lblMaxNum;
		private System.Windows.Forms.Label lblWLevel;
		private System.Windows.Forms.Label lblColon0;
		private System.Windows.Forms.Label lblColon1;
		private System.Windows.Forms.Label lblWWidth;
		private System.Windows.Forms.Label lblGamma;
		private System.Windows.Forms.NumericUpDown cwneGamma;
		private System.Windows.Forms.Button cmdImgSelect;
		private System.Windows.Forms.Button cmdImgDelete;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdEnd;
		private System.Windows.Forms.Label lblProcess;
		private System.Windows.Forms.GroupBox fraSpecify;
		private System.Windows.Forms.RadioButton optFile;
		private System.Windows.Forms.RadioButton optFolder;
		private System.Windows.Forms.Label lblColon2;
		private System.Windows.Forms.NumericUpDown cwneWL;
		private System.Windows.Forms.NumericUpDown cwneWW;
		private CTListBox lstImgFile;
	}
}