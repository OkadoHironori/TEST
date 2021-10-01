namespace CT30K
{
	partial class frmBHCImportError
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.lblattention = new System.Windows.Forms.Label();
            this.lblCheckRawdata = new System.Windows.Forms.Label();
            this.lblCheckInffile = new System.Windows.Forms.Label();
            this.lblCheckFileName = new System.Windows.Forms.Label();
            this.lblConfirmTheFollowing = new System.Windows.Forms.Label();
            this.lblCheckSingleScan = new System.Windows.Forms.Label();
            this.lblCheckVolt = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.lblCheckPdata = new System.Windows.Forms.Label();
            this.txtFileList = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.cmdOK.Location = new System.Drawing.Point(180, 448);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(121, 33);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "Ｏ  Ｋ";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // lblattention
            // 
            this.lblattention.Font = new System.Drawing.Font("MS UI Gothic", 11.25F);
            this.lblattention.Location = new System.Drawing.Point(24, 24);
            this.lblattention.Name = "lblattention";
            this.lblattention.Size = new System.Drawing.Size(393, 30);
            this.lblattention.TabIndex = 1;
            this.lblattention.Text = "以下の画像ファイルはBHCテーブルに追加できませんでした。";
            // 
            // lblCheckRawdata
            // 
            this.lblCheckRawdata.Font = new System.Drawing.Font("MS UI Gothic", 11.25F);
            this.lblCheckRawdata.Location = new System.Drawing.Point(48, 288);
            this.lblCheckRawdata.Name = "lblCheckRawdata";
            this.lblCheckRawdata.Size = new System.Drawing.Size(409, 17);
            this.lblCheckRawdata.TabIndex = 2;
            this.lblCheckRawdata.Text = "**********";
            // 
            // lblCheckInffile
            // 
            this.lblCheckInffile.Font = new System.Drawing.Font("MS UI Gothic", 11.25F);
            this.lblCheckInffile.Location = new System.Drawing.Point(48, 312);
            this.lblCheckInffile.Name = "lblCheckInffile";
            this.lblCheckInffile.Size = new System.Drawing.Size(409, 17);
            this.lblCheckInffile.TabIndex = 4;
            this.lblCheckInffile.Text = "**********";
            // 
            // lblCheckFileName
            // 
            this.lblCheckFileName.Font = new System.Drawing.Font("MS UI Gothic", 11.25F);
            this.lblCheckFileName.Location = new System.Drawing.Point(48, 336);
            this.lblCheckFileName.Name = "lblCheckFileName";
            this.lblCheckFileName.Size = new System.Drawing.Size(377, 17);
            this.lblCheckFileName.TabIndex = 5;
            this.lblCheckFileName.Text = "**********";
            // 
            // lblConfirmTheFollowing
            // 
            this.lblConfirmTheFollowing.Font = new System.Drawing.Font("MS UI Gothic", 11.25F);
            this.lblConfirmTheFollowing.Location = new System.Drawing.Point(32, 264);
            this.lblConfirmTheFollowing.Name = "lblConfirmTheFollowing";
            this.lblConfirmTheFollowing.Size = new System.Drawing.Size(345, 17);
            this.lblConfirmTheFollowing.TabIndex = 6;
            this.lblConfirmTheFollowing.Text = "下記の原因が考えられます。";
            // 
            // lblCheckSingleScan
            // 
            this.lblCheckSingleScan.Font = new System.Drawing.Font("MS UI Gothic", 11.25F);
            this.lblCheckSingleScan.Location = new System.Drawing.Point(48, 360);
            this.lblCheckSingleScan.Name = "lblCheckSingleScan";
            this.lblCheckSingleScan.Size = new System.Drawing.Size(377, 17);
            this.lblCheckSingleScan.TabIndex = 7;
            this.lblCheckSingleScan.Text = "**********";
            // 
            // lblCheckVolt
            // 
            this.lblCheckVolt.Font = new System.Drawing.Font("MS UI Gothic", 11.25F);
            this.lblCheckVolt.Location = new System.Drawing.Point(48, 384);
            this.lblCheckVolt.Name = "lblCheckVolt";
            this.lblCheckVolt.Size = new System.Drawing.Size(377, 21);
            this.lblCheckVolt.TabIndex = 8;
            this.lblCheckVolt.Text = "**********";
            // 
            // Label1
            // 
            this.Label1.Font = new System.Drawing.Font("MS UI Gothic", 11.25F);
            this.Label1.Location = new System.Drawing.Point(0, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(377, 17);
            this.Label1.TabIndex = 9;
            this.Label1.Text = "**********";
            // 
            // lblCheckPdata
            // 
            this.lblCheckPdata.Font = new System.Drawing.Font("MS UI Gothic", 11.25F);
            this.lblCheckPdata.Location = new System.Drawing.Point(48, 412);
            this.lblCheckPdata.Name = "lblCheckPdata";
            this.lblCheckPdata.Size = new System.Drawing.Size(377, 17);
            this.lblCheckPdata.TabIndex = 10;
            this.lblCheckPdata.Text = "**********";
            // 
            // txtFileList
            // 
            this.txtFileList.Font = new System.Drawing.Font("MS UI Gothic", 11.25F);
            this.txtFileList.Location = new System.Drawing.Point(32, 64);
            this.txtFileList.MaxLength = 0;
            this.txtFileList.Multiline = true;
            this.txtFileList.Name = "txtFileList";
            this.txtFileList.ReadOnly = true;
            this.txtFileList.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtFileList.Size = new System.Drawing.Size(409, 177);
            this.txtFileList.TabIndex = 11;
            this.txtFileList.TabStop = false;
            this.txtFileList.WordWrap = false;
            // 
            // frmBHCImportError
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(469, 497);
            this.ControlBox = false;
            this.Controls.Add(this.txtFileList);
            this.Controls.Add(this.lblCheckPdata);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.lblCheckVolt);
            this.Controls.Add(this.lblCheckSingleScan);
            this.Controls.Add(this.lblConfirmTheFollowing);
            this.Controls.Add(this.lblCheckFileName);
            this.Controls.Add(this.lblCheckInffile);
            this.Controls.Add(this.lblCheckRawdata);
            this.Controls.Add(this.lblattention);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBHCImportError";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "画像読み込みエラー";
            this.Load += new System.EventHandler(this.frmBHCImportError_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Label lblattention;
		private System.Windows.Forms.Label lblCheckRawdata;
		private System.Windows.Forms.Label lblCheckInffile;
		private System.Windows.Forms.Label lblCheckFileName;
		private System.Windows.Forms.Label lblConfirmTheFollowing;
		private System.Windows.Forms.Label lblCheckSingleScan;
		private System.Windows.Forms.Label lblCheckVolt;
		private System.Windows.Forms.Label Label1;
		private System.Windows.Forms.Label lblCheckPdata;
		private System.Windows.Forms.TextBox txtFileList;
	}
}