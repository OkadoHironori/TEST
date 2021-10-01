namespace CT30K
{
    partial class frmDistanceCorrect
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
            this.lblTitle1_0 = new System.Windows.Forms.Label();
            this.lblTitle2_1 = new System.Windows.Forms.Label();
            this.lblTitle2_2 = new System.Windows.Forms.Label();
            this.lblTitle1_1 = new System.Windows.Forms.Label();
            this.fraFidFcdOffset = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblFcdOffset = new System.Windows.Forms.Label();
            this.lblFidOffset = new System.Windows.Forms.Label();
            this.lblFcdOffsetName = new System.Windows.Forms.Label();
            this.lblFidOffsetName = new System.Windows.Forms.Label();
            this.lblFidOffsetUni = new System.Windows.Forms.Label();
            this.cmdEnd = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.fraFidFcdOffset.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle1_0
            // 
            this.lblTitle1_0.AutoSize = true;
            this.lblTitle1_0.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblTitle1_0.Location = new System.Drawing.Point(16, 16);
            this.lblTitle1_0.Name = "lblTitle1_0";
            this.lblTitle1_0.Size = new System.Drawing.Size(139, 16);
            this.lblTitle1_0.TabIndex = 0;
            this.lblTitle1_0.Text = "寸法校正用画像１ :";
            // 
            // lblTitle2_1
            // 
            this.lblTitle2_1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblTitle2_1.Location = new System.Drawing.Point(64, 80);
            this.lblTitle2_1.Name = "lblTitle2_1";
            this.lblTitle2_1.Size = new System.Drawing.Size(226, 16);
            this.lblTitle2_1.TabIndex = 3;
            this.lblTitle2_1.Text = "寸法校正ファントム :";
            this.lblTitle2_1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTitle2_2
            // 
            this.lblTitle2_2.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblTitle2_2.Location = new System.Drawing.Point(64, 192);
            this.lblTitle2_2.Name = "lblTitle2_2";
            this.lblTitle2_2.Size = new System.Drawing.Size(226, 16);
            this.lblTitle2_2.TabIndex = 8;
            this.lblTitle2_2.Text = "寸法校正ファントム :";
            this.lblTitle2_2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTitle1_1
            // 
            this.lblTitle1_1.AutoSize = true;
            this.lblTitle1_1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblTitle1_1.Location = new System.Drawing.Point(16, 128);
            this.lblTitle1_1.Name = "lblTitle1_1";
            this.lblTitle1_1.Size = new System.Drawing.Size(139, 16);
            this.lblTitle1_1.TabIndex = 5;
            this.lblTitle1_1.Text = "寸法校正用画像２ :";
            // 
            // fraFidFcdOffset
            // 
            this.fraFidFcdOffset.Controls.Add(this.label2);
            this.fraFidFcdOffset.Controls.Add(this.lblFcdOffset);
            this.fraFidFcdOffset.Controls.Add(this.lblFidOffset);
            this.fraFidFcdOffset.Controls.Add(this.lblFcdOffsetName);
            this.fraFidFcdOffset.Controls.Add(this.lblFidOffsetName);
            this.fraFidFcdOffset.Controls.Add(this.lblFidOffsetUni);
            this.fraFidFcdOffset.Location = new System.Drawing.Point(24, 240);
            this.fraFidFcdOffset.Name = "fraFidFcdOffset";
            this.fraFidFcdOffset.Size = new System.Drawing.Size(161, 105);
            this.fraFidFcdOffset.TabIndex = 10;
            this.fraFidFcdOffset.TabStop = false;
            this.fraFidFcdOffset.Text = "ＦＩＤ／ＦＣＤオフセット";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 10F);
            this.label2.Location = new System.Drawing.Point(131, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 14);
            this.label2.TabIndex = 6;
            this.label2.Text = "mm";
            // 
            // lblFcdOffset
            // 
            this.lblFcdOffset.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblFcdOffset.Location = new System.Drawing.Point(61, 72);
            this.lblFcdOffset.Name = "lblFcdOffset";
            this.lblFcdOffset.Size = new System.Drawing.Size(65, 15);
            this.lblFcdOffset.TabIndex = 5;
            this.lblFcdOffset.Text = "-5.21";
            this.lblFcdOffset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFidOffset
            // 
            this.lblFidOffset.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblFidOffset.Location = new System.Drawing.Point(61, 40);
            this.lblFidOffset.Name = "lblFidOffset";
            this.lblFidOffset.Size = new System.Drawing.Size(65, 15);
            this.lblFidOffset.TabIndex = 3;
            this.lblFidOffset.Text = "12.43";
            this.lblFidOffset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFcdOffsetName
            // 
            this.lblFcdOffsetName.AutoSize = true;
            this.lblFcdOffsetName.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblFcdOffsetName.Location = new System.Drawing.Point(16, 72);
            this.lblFcdOffsetName.Name = "lblFcdOffsetName";
            this.lblFcdOffsetName.Size = new System.Drawing.Size(48, 16);
            this.lblFcdOffsetName.TabIndex = 2;
            this.lblFcdOffsetName.Text = "FCD :";
            // 
            // lblFidOffsetName
            // 
            this.lblFidOffsetName.AutoSize = true;
            this.lblFidOffsetName.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblFidOffsetName.Location = new System.Drawing.Point(16, 40);
            this.lblFidOffsetName.Name = "lblFidOffsetName";
            this.lblFidOffsetName.Size = new System.Drawing.Size(48, 16);
            this.lblFidOffsetName.TabIndex = 1;
            this.lblFidOffsetName.Text = "FDD :";
            // 
            // lblFidOffsetUni
            // 
            this.lblFidOffsetUni.AutoSize = true;
            this.lblFidOffsetUni.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 10F);
            this.lblFidOffsetUni.Location = new System.Drawing.Point(131, 40);
            this.lblFidOffsetUni.Name = "lblFidOffsetUni";
            this.lblFidOffsetUni.Size = new System.Drawing.Size(27, 14);
            this.lblFidOffsetUni.TabIndex = 4;
            this.lblFidOffsetUni.Text = "mm";
            // 
            // cmdEnd
            // 
            this.cmdEnd.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdEnd.Location = new System.Drawing.Point(328, 288);
            this.cmdEnd.Name = "cmdEnd";
            this.cmdEnd.Size = new System.Drawing.Size(105, 33);
            this.cmdEnd.TabIndex = 12;
            this.cmdEnd.Tag = "10006";
            this.cmdEnd.Text = "終　了";
            this.cmdEnd.UseVisualStyleBackColor = true;
            this.cmdEnd.Click += new System.EventHandler(this.cmdEnd_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdOK.Location = new System.Drawing.Point(208, 288);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(105, 33);
            this.cmdOK.TabIndex = 11;
            this.cmdOK.Tag = "";
            this.cmdOK.Text = "ＯＫ";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // frmDistanceCorrect
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(464, 375);
            this.ControlBox = false;
            this.Controls.Add(this.cmdEnd);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.fraFidFcdOffset);
            this.Controls.Add(this.lblTitle2_2);
            this.Controls.Add(this.lblTitle1_1);
            this.Controls.Add(this.lblTitle2_1);
            this.Controls.Add(this.lblTitle1_0);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmDistanceCorrect";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "寸法校正";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDistanceCorrect_FormClosed);
            this.Load += new System.EventHandler(this.frmDistanceCorrect_Load);
            this.fraFidFcdOffset.ResumeLayout(false);
            this.fraFidFcdOffset.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle1_0;
        private System.Windows.Forms.Label lblTitle2_1;
        private System.Windows.Forms.Label lblTitle2_2;
        private System.Windows.Forms.Label lblTitle1_1;
        private System.Windows.Forms.GroupBox fraFidFcdOffset;
        private System.Windows.Forms.Label lblFcdOffsetName;
        private System.Windows.Forms.Label lblFidOffsetName;
        private System.Windows.Forms.Label lblFidOffset;
        private System.Windows.Forms.Label lblFidOffsetUni;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblFcdOffset;
        private System.Windows.Forms.Button cmdEnd;
        private System.Windows.Forms.Button cmdOK;
    }
}