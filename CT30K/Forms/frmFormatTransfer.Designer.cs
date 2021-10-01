namespace CT30K
{
    partial class frmFormatTransfer
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
            this.cmdImgSelect = new System.Windows.Forms.Button();
            this.fraSpecify = new System.Windows.Forms.GroupBox();
            this.optFile = new System.Windows.Forms.RadioButton();
            this.optFolder = new System.Windows.Forms.RadioButton();
            this.cmdImgDelete = new System.Windows.Forms.Button();
            this.lblNumTitle = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.lblMaxNum = new System.Windows.Forms.Label();
            this.StatusBar1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblOutDirName = new System.Windows.Forms.Label();
            this.txtDirName = new System.Windows.Forms.TextBox();
            this.cmdDirSelect = new System.Windows.Forms.Button();
            this.lblOutKind = new System.Windows.Forms.Label();
            this.cmbOutKind = new System.Windows.Forms.ComboBox();
            this.chkChangeContrast = new System.Windows.Forms.CheckBox();
            this.chkInformation = new System.Windows.Forms.CheckBox();
            this.chkScale = new System.Windows.Forms.CheckBox();
            this.cmdGo = new System.Windows.Forms.Button();
            this.cmdExit = new System.Windows.Forms.Button();
            this.fraDICOM = new System.Windows.Forms.GroupBox();
            this.lblComment = new System.Windows.Forms.Label();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.txtInstitutionName = new System.Windows.Forms.TextBox();
            this.lblInstitutionName = new System.Windows.Forms.Label();
            this.txtPatientName = new System.Windows.Forms.TextBox();
            this.lblPatientName = new System.Windows.Forms.Label();
            this.lstImgFile = new CT30K.CTListBox();
            this.fraSpecify.SuspendLayout();
            this.StatusBar1.SuspendLayout();
            this.fraDICOM.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSliceName
            // 
            this.lblSliceName.AutoSize = true;
            this.lblSliceName.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblSliceName.Location = new System.Drawing.Point(16, 12);
            this.lblSliceName.Name = "lblSliceName";
            this.lblSliceName.Size = new System.Drawing.Size(94, 16);
            this.lblSliceName.TabIndex = 0;
            this.lblSliceName.Tag = "12762";
            this.lblSliceName.Text = "#スライス名：";
            // 
            // cmdImgSelect
            // 
            this.cmdImgSelect.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdImgSelect.Location = new System.Drawing.Point(452, 32);
            this.cmdImgSelect.Name = "cmdImgSelect";
            this.cmdImgSelect.Size = new System.Drawing.Size(89, 33);
            this.cmdImgSelect.TabIndex = 12;
            this.cmdImgSelect.Tag = "10009";
            this.cmdImgSelect.Text = "#参照...";
            this.cmdImgSelect.UseVisualStyleBackColor = true;
            // 
            // fraSpecify
            // 
            this.fraSpecify.Controls.Add(this.optFile);
            this.fraSpecify.Controls.Add(this.optFolder);
            this.fraSpecify.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fraSpecify.Location = new System.Drawing.Point(444, 76);
            this.fraSpecify.Name = "fraSpecify";
            this.fraSpecify.Size = new System.Drawing.Size(101, 73);
            this.fraSpecify.TabIndex = 13;
            this.fraSpecify.TabStop = false;
            this.fraSpecify.Tag = "12751";
            this.fraSpecify.Text = "#指定";
            // 
            // optFile
            // 
            this.optFile.AutoSize = true;
            this.optFile.Checked = true;
            this.optFile.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 10F);
            this.optFile.Location = new System.Drawing.Point(8, 48);
            this.optFile.Name = "optFile";
            this.optFile.Size = new System.Drawing.Size(80, 18);
            this.optFile.TabIndex = 15;
            this.optFile.TabStop = true;
            this.optFile.Tag = "12753";
            this.optFile.Text = "#ファイル";
            this.optFile.UseVisualStyleBackColor = true;
            this.optFile.CheckedChanged += new System.EventHandler(this.optFile_CheckedChanged);
            // 
            // optFolder
            // 
            this.optFolder.AutoSize = true;
            this.optFolder.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 10F);
            this.optFolder.Location = new System.Drawing.Point(8, 24);
            this.optFolder.Name = "optFolder";
            this.optFolder.Size = new System.Drawing.Size(81, 18);
            this.optFolder.TabIndex = 14;
            this.optFolder.Tag = "12752";
            this.optFolder.Text = "#フォルダ";
            this.optFolder.UseVisualStyleBackColor = true;
            this.optFolder.CheckedChanged += new System.EventHandler(this.optFolder_CheckedChanged);
            // 
            // cmdImgDelete
            // 
            this.cmdImgDelete.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdImgDelete.Location = new System.Drawing.Point(452, 192);
            this.cmdImgDelete.Name = "cmdImgDelete";
            this.cmdImgDelete.Size = new System.Drawing.Size(89, 33);
            this.cmdImgDelete.TabIndex = 16;
            this.cmdImgDelete.Tag = "10010";
            this.cmdImgDelete.Text = "#削　除";
            this.cmdImgDelete.UseVisualStyleBackColor = true;
            // 
            // lblNumTitle
            // 
            this.lblNumTitle.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblNumTitle.Location = new System.Drawing.Point(24, 304);
            this.lblNumTitle.Name = "lblNumTitle";
            this.lblNumTitle.Size = new System.Drawing.Size(240, 16);
            this.lblNumTitle.TabIndex = 2;
            this.lblNumTitle.Tag = "12052";
            this.lblNumTitle.Text = "#入力枚数：";
            this.lblNumTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCount
            // 
            this.lblCount.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblCount.Location = new System.Drawing.Point(272, 304);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(49, 17);
            this.lblCount.TabIndex = 3;
            this.lblCount.Text = "0";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblMaxNum
            // 
            this.lblMaxNum.AutoSize = true;
            this.lblMaxNum.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblMaxNum.Location = new System.Drawing.Point(328, 304);
            this.lblMaxNum.Name = "lblMaxNum";
            this.lblMaxNum.Size = new System.Drawing.Size(120, 16);
            this.lblMaxNum.TabIndex = 4;
            this.lblMaxNum.Text = "枚（最大1000枚）";
            // 
            // StatusBar1
            // 
            this.StatusBar1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.StatusBar1.Location = new System.Drawing.Point(0, 689);
            this.StatusBar1.Name = "StatusBar1";
            this.StatusBar1.Size = new System.Drawing.Size(554, 22);
            this.StatusBar1.SizingGrip = false;
            this.StatusBar1.TabIndex = 27;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // lblOutDirName
            // 
            this.lblOutDirName.AutoSize = true;
            this.lblOutDirName.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblOutDirName.Location = new System.Drawing.Point(16, 341);
            this.lblOutDirName.Name = "lblOutDirName";
            this.lblOutDirName.Size = new System.Drawing.Size(121, 16);
            this.lblOutDirName.TabIndex = 5;
            this.lblOutDirName.Tag = "12522";
            this.lblOutDirName.Text = "#保存先フォルダ";
            // 
            // txtDirName
            // 
            this.txtDirName.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtDirName.Location = new System.Drawing.Point(136, 336);
            this.txtDirName.Name = "txtDirName";
            this.txtDirName.Size = new System.Drawing.Size(297, 23);
            this.txtDirName.TabIndex = 6;
            // 
            // cmdDirSelect
            // 
            this.cmdDirSelect.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdDirSelect.Location = new System.Drawing.Point(452, 336);
            this.cmdDirSelect.Name = "cmdDirSelect";
            this.cmdDirSelect.Size = new System.Drawing.Size(89, 33);
            this.cmdDirSelect.TabIndex = 17;
            this.cmdDirSelect.Tag = "10009";
            this.cmdDirSelect.Text = "#参照...";
            this.cmdDirSelect.UseVisualStyleBackColor = true;
            this.cmdDirSelect.Click += new System.EventHandler(this.cmdDirSelect_Click);
            // 
            // lblOutKind
            // 
            this.lblOutKind.AutoSize = true;
            this.lblOutKind.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblOutKind.Location = new System.Drawing.Point(16, 381);
            this.lblOutKind.Name = "lblOutKind";
            this.lblOutKind.Size = new System.Drawing.Size(118, 16);
            this.lblOutKind.TabIndex = 7;
            this.lblOutKind.Tag = "12517";
            this.lblOutKind.Text = "#ファイルの種類";
            // 
            // cmbOutKind
            // 
            this.cmbOutKind.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbOutKind.FormattingEnabled = true;
            this.cmbOutKind.Items.AddRange(new object[] {
            ".raw",
            ".jpg",
            ".bmp",
            ".pct",
            ".tif (8bit)",
            ".tif (16bit)"});
            this.cmbOutKind.Location = new System.Drawing.Point(136, 376);
            this.cmbOutKind.Name = "cmbOutKind";
            this.cmbOutKind.Size = new System.Drawing.Size(89, 24);
            this.cmbOutKind.TabIndex = 8;
            this.cmbOutKind.SelectedIndexChanged += new System.EventHandler(this.cmbOutKind_SelectedIndexChanged);
            // 
            // chkChangeContrast
            // 
            this.chkChangeContrast.AutoSize = true;
            this.chkChangeContrast.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkChangeContrast.Location = new System.Drawing.Point(272, 380);
            this.chkChangeContrast.Name = "chkChangeContrast";
            this.chkChangeContrast.Size = new System.Drawing.Size(99, 20);
            this.chkChangeContrast.TabIndex = 9;
            this.chkChangeContrast.Tag = "10512";
            this.chkChangeContrast.Text = "#階調変換";
            this.chkChangeContrast.UseVisualStyleBackColor = true;
            // 
            // chkInformation
            // 
            this.chkInformation.AutoSize = true;
            this.chkInformation.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkInformation.Location = new System.Drawing.Point(24, 408);
            this.chkInformation.Name = "chkInformation";
            this.chkInformation.Size = new System.Drawing.Size(129, 20);
            this.chkInformation.TabIndex = 10;
            this.chkInformation.Tag = "12521";
            this.chkInformation.Text = "#付帯情報付き";
            this.chkInformation.UseVisualStyleBackColor = true;
            this.chkInformation.Click += new System.EventHandler(this.chkInformation_Click);
            // 
            // chkScale
            // 
            this.chkScale.AutoSize = true;
            this.chkScale.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkScale.Location = new System.Drawing.Point(24, 436);
            this.chkScale.Name = "chkScale";
            this.chkScale.Size = new System.Drawing.Size(127, 20);
            this.chkScale.TabIndex = 11;
            this.chkScale.Tag = "12518";
            this.chkScale.Text = "#スケール付加";
            this.chkScale.UseVisualStyleBackColor = true;
            // 
            // cmdGo
            // 
            this.cmdGo.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdGo.Location = new System.Drawing.Point(272, 648);
            this.cmdGo.Name = "cmdGo";
            this.cmdGo.Size = new System.Drawing.Size(121, 33);
            this.cmdGo.TabIndex = 25;
            this.cmdGo.Tag = "10005";
            this.cmdGo.Text = "#実　行";
            this.cmdGo.UseVisualStyleBackColor = true;
            this.cmdGo.Click += new System.EventHandler(this.cmdGo_Click);
            // 
            // cmdExit
            // 
            this.cmdExit.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdExit.Location = new System.Drawing.Point(424, 648);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(113, 33);
            this.cmdExit.TabIndex = 26;
            this.cmdExit.Tag = "10006";
            this.cmdExit.Text = "#終　了";
            this.cmdExit.UseVisualStyleBackColor = true;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // fraDICOM
            // 
            this.fraDICOM.Controls.Add(this.txtComment);
            this.fraDICOM.Controls.Add(this.lblComment);
            this.fraDICOM.Controls.Add(this.txtInstitutionName);
            this.fraDICOM.Controls.Add(this.lblInstitutionName);
            this.fraDICOM.Controls.Add(this.txtPatientName);
            this.fraDICOM.Controls.Add(this.lblPatientName);
            this.fraDICOM.Location = new System.Drawing.Point(4, 464);
            this.fraDICOM.Name = "fraDICOM";
            this.fraDICOM.Size = new System.Drawing.Size(545, 177);
            this.fraDICOM.TabIndex = 18;
            this.fraDICOM.TabStop = false;
            // 
            // lblComment
            // 
            this.lblComment.AutoSize = true;
            this.lblComment.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblComment.Location = new System.Drawing.Point(56, 73);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(65, 16);
            this.lblComment.TabIndex = 23;
            this.lblComment.Tag = "12816";
            this.lblComment.Text = "#コメント";
            // 
            // txtComment
            // 
            this.txtComment.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtComment.Location = new System.Drawing.Point(120, 68);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(369, 105);
            this.txtComment.TabIndex = 24;
            this.txtComment.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtComment_KeyPress);
            // 
            // txtInstitutionName
            // 
            this.txtInstitutionName.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtInstitutionName.Location = new System.Drawing.Point(120, 39);
            this.txtInstitutionName.Name = "txtInstitutionName";
            this.txtInstitutionName.Size = new System.Drawing.Size(369, 23);
            this.txtInstitutionName.TabIndex = 22;
            this.txtInstitutionName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInstitutionName_KeyPress);
            // 
            // lblInstitutionName
            // 
            this.lblInstitutionName.AutoSize = true;
            this.lblInstitutionName.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblInstitutionName.Location = new System.Drawing.Point(56, 44);
            this.lblInstitutionName.Name = "lblInstitutionName";
            this.lblInstitutionName.Size = new System.Drawing.Size(64, 16);
            this.lblInstitutionName.TabIndex = 21;
            this.lblInstitutionName.Tag = "12520";
            this.lblInstitutionName.Text = "#施設名";
            // 
            // txtPatientName
            // 
            this.txtPatientName.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtPatientName.Location = new System.Drawing.Point(120, 10);
            this.txtPatientName.Name = "txtPatientName";
            this.txtPatientName.Size = new System.Drawing.Size(369, 23);
            this.txtPatientName.TabIndex = 20;
            this.txtPatientName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPatientName_KeyPress);
            // 
            // lblPatientName
            // 
            this.lblPatientName.AutoSize = true;
            this.lblPatientName.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblPatientName.Location = new System.Drawing.Point(56, 15);
            this.lblPatientName.Name = "lblPatientName";
            this.lblPatientName.Size = new System.Drawing.Size(64, 16);
            this.lblPatientName.TabIndex = 19;
            this.lblPatientName.Tag = "12519";
            this.lblPatientName.Text = "#患者名";
            // 
            // lstImgFile
            // 
            this.lstImgFile.CountLabel = null;
            this.lstImgFile.DefaultPath = "";
            this.lstImgFile.DeleteButton = null;
            this.lstImgFile.Description = "画像ファイル";
            this.lstImgFile.Extension = ".img";
            this.lstImgFile.HowToAdd = CT30K.CTListBox.HowToAddType.FromFileList;
            this.lstImgFile.InfCheck = true;
            this.lstImgFile.Location = new System.Drawing.Point(16, 32);
            this.lstImgFile.Max = 1000;
            this.lstImgFile.Name = "lstImgFile";
            this.lstImgFile.NumberingDigits = 0;
            this.lstImgFile.ReferenceButton = null;
            this.lstImgFile.Size = new System.Drawing.Size(417, 260);
            this.lstImgFile.TabIndex = 1;
            // 
            // frmFormatTransfer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(554, 711);
            this.ControlBox = false;
            this.Controls.Add(this.lstImgFile);
            this.Controls.Add(this.fraDICOM);
            this.Controls.Add(this.cmdExit);
            this.Controls.Add(this.cmdGo);
            this.Controls.Add(this.chkScale);
            this.Controls.Add(this.chkInformation);
            this.Controls.Add(this.chkChangeContrast);
            this.Controls.Add(this.cmbOutKind);
            this.Controls.Add(this.lblOutKind);
            this.Controls.Add(this.cmdDirSelect);
            this.Controls.Add(this.txtDirName);
            this.Controls.Add(this.lblOutDirName);
            this.Controls.Add(this.StatusBar1);
            this.Controls.Add(this.lblMaxNum);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.lblNumTitle);
            this.Controls.Add(this.cmdImgDelete);
            this.Controls.Add(this.fraSpecify);
            this.Controls.Add(this.cmdImgSelect);
            this.Controls.Add(this.lblSliceName);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(107, 73);
            this.Name = "frmFormatTransfer";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Tag = "10518";
            this.Text = "#画像フォーマット変換";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmFormatTransfer_FormClosed);
            this.Load += new System.EventHandler(this.frmFormatTransfer_Load);
            this.Resize += new System.EventHandler(this.frmFormatTransfer_Resize);
            this.fraSpecify.ResumeLayout(false);
            this.fraSpecify.PerformLayout();
            this.StatusBar1.ResumeLayout(false);
            this.StatusBar1.PerformLayout();
            this.fraDICOM.ResumeLayout(false);
            this.fraDICOM.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSliceName;
        private System.Windows.Forms.Button cmdImgSelect;
        private System.Windows.Forms.GroupBox fraSpecify;
        private System.Windows.Forms.RadioButton optFile;
        private System.Windows.Forms.RadioButton optFolder;
        private System.Windows.Forms.Button cmdImgDelete;
        private System.Windows.Forms.Label lblNumTitle;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label lblMaxNum;
        private System.Windows.Forms.StatusStrip StatusBar1;
        private System.Windows.Forms.Label lblOutDirName;
        private System.Windows.Forms.TextBox txtDirName;
        private System.Windows.Forms.Button cmdDirSelect;
        private System.Windows.Forms.Label lblOutKind;
        private System.Windows.Forms.ComboBox cmbOutKind;
        private System.Windows.Forms.CheckBox chkChangeContrast;
        private System.Windows.Forms.CheckBox chkInformation;
        private System.Windows.Forms.CheckBox chkScale;
        private System.Windows.Forms.Button cmdGo;
        private System.Windows.Forms.Button cmdExit;
        private System.Windows.Forms.GroupBox fraDICOM;
        private System.Windows.Forms.TextBox txtInstitutionName;
        private System.Windows.Forms.Label lblInstitutionName;
        private System.Windows.Forms.TextBox txtPatientName;
        private System.Windows.Forms.Label lblPatientName;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Label lblComment;
		private CTListBox lstImgFile;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}