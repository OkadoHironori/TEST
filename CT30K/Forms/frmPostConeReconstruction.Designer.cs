namespace CT30K
{
    partial class frmPostConeReconstruction
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
            this.lblHeader = new System.Windows.Forms.Label();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdExe = new System.Windows.Forms.Button();
            this.cmdDelete = new System.Windows.Forms.Button();
            this.cmdSelect = new System.Windows.Forms.Button();
            this.lstFile = new CT30K.CTListBox();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblHeader.Location = new System.Drawing.Point(12, 9);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(96, 16);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "生データ名:";
            // 
            // cmdCancel
            // 
            this.cmdCancel.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdCancel.Location = new System.Drawing.Point(568, 380);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(105, 33);
            this.cmdCancel.TabIndex = 11;
            this.cmdCancel.Tag = "10002";
            this.cmdCancel.Text = "#終　了";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdExe
            // 
            this.cmdExe.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdExe.Location = new System.Drawing.Point(440, 380);
            this.cmdExe.Name = "cmdExe";
            this.cmdExe.Size = new System.Drawing.Size(105, 33);
            this.cmdExe.TabIndex = 10;
            this.cmdExe.Tag = "10005";
            this.cmdExe.Text = "#実　行";
            this.cmdExe.UseVisualStyleBackColor = true;
            this.cmdExe.Click += new System.EventHandler(this.cmdExe_Click);
            // 
            // cmdDelete
            // 
            this.cmdDelete.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdDelete.Location = new System.Drawing.Point(568, 80);
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Size = new System.Drawing.Size(105, 33);
            this.cmdDelete.TabIndex = 9;
            this.cmdDelete.Tag = "10010";
            this.cmdDelete.Text = "#削　除";
            this.cmdDelete.UseVisualStyleBackColor = true;
            // 
            // cmdSelect
            // 
            this.cmdSelect.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdSelect.Location = new System.Drawing.Point(568, 32);
            this.cmdSelect.Name = "cmdSelect";
            this.cmdSelect.Size = new System.Drawing.Size(105, 33);
            this.cmdSelect.TabIndex = 8;
            this.cmdSelect.Tag = "10009";
            this.cmdSelect.Text = "#参照...";
            this.cmdSelect.UseVisualStyleBackColor = true;
            // 
            // lstFile
            // 
            this.lstFile.CountLabel = null;
            this.lstFile.DefaultPath = "";
            this.lstFile.DeleteButton = null;
            this.lstFile.Description = "コーンビームＣＴ用生データファイル";
            this.lstFile.Extension = ".cob";
            this.lstFile.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lstFile.HowToAdd = CT30K.CTListBox.HowToAddType.FromFileList;
            this.lstFile.InfCheck = true;
            this.lstFile.Location = new System.Drawing.Point(16, 32);
            this.lstFile.Margin = new System.Windows.Forms.Padding(6);
            this.lstFile.Max = 32767;
            this.lstFile.Name = "lstFile";
            this.lstFile.NumberingDigits = 3;
            this.lstFile.ReferenceButton = null;
            this.lstFile.Size = new System.Drawing.Size(537, 326);
            this.lstFile.TabIndex = 1;
            // 
            // frmPostConeReconstruction
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(687, 437);
            this.ControlBox = false;
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdExe);
            this.Controls.Add(this.cmdDelete);
            this.Controls.Add(this.cmdSelect);
            this.Controls.Add(this.lstFile);
            this.Controls.Add(this.lblHeader);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmPostConeReconstruction";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "10516";
            this.Text = "#コーン後再構成";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmPostConeReconstruction_FormClosed);
            this.Load += new System.EventHandler(this.frmPostConeReconstruction_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private CTListBox lstFile;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdExe;
        private System.Windows.Forms.Button cmdDelete;
        private System.Windows.Forms.Button cmdSelect;
    }
}