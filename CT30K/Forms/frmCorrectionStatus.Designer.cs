namespace CT30K
{
    partial class frmCorrectionStatus
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
            this.Frame1 = new System.Windows.Forms.GroupBox();
            this.fraGainShift = new System.Windows.Forms.GroupBox();
            this.lblComment = new System.Windows.Forms.Label();
            this.fraDistance = new System.Windows.Forms.GroupBox();
            this.fraVertical = new System.Windows.Forms.GroupBox();
            this.fraOffset = new System.Windows.Forms.GroupBox();
            this.fraScanPosi = new System.Windows.Forms.GroupBox();
            this.fraRotate = new System.Windows.Forms.GroupBox();
            this.fraGain = new System.Windows.Forms.GroupBox();
            this.fraScanCorrect = new System.Windows.Forms.GroupBox();
            this.cmdClose = new System.Windows.Forms.Button();
            this.pnlCorStatus = new System.Windows.Forms.Panel();
            this.Frame1.SuspendLayout();
            this.pnlCorStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // Frame1
            // 
            this.Frame1.Controls.Add(this.fraGainShift);
            this.Frame1.Controls.Add(this.lblComment);
            this.Frame1.Controls.Add(this.fraDistance);
            this.Frame1.Controls.Add(this.fraVertical);
            this.Frame1.Controls.Add(this.fraOffset);
            this.Frame1.Controls.Add(this.fraScanPosi);
            this.Frame1.Controls.Add(this.fraRotate);
            this.Frame1.Controls.Add(this.fraGain);
            this.Frame1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11F);
            this.Frame1.Location = new System.Drawing.Point(0, 8);
            this.Frame1.Name = "Frame1";
            this.Frame1.Size = new System.Drawing.Size(505, 513);
            this.Frame1.TabIndex = 0;
            this.Frame1.TabStop = false;
            this.Frame1.Tag = "12092";
            this.Frame1.Text = "#校正ステータス";
            // 
            // fraGainShift
            // 
            this.fraGainShift.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fraGainShift.Location = new System.Drawing.Point(250, 67);
            this.fraGainShift.Name = "fraGainShift";
            this.fraGainShift.Size = new System.Drawing.Size(240, 193);
            this.fraGainShift.TabIndex = 3;
            this.fraGainShift.TabStop = false;
            this.fraGainShift.Tag = "10904";
            this.fraGainShift.Text = "#シフトスキャン用";
            // 
            // lblComment
            // 
            this.lblComment.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblComment.Location = new System.Drawing.Point(32, 472);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(185, 17);
            this.lblComment.TabIndex = 6;
            this.lblComment.Text = "* At execution time / Present value";
            this.lblComment.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblComment.Visible = false;
            // 
            // fraDistance
            // 
            this.fraDistance.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fraDistance.Location = new System.Drawing.Point(259, 326);
            this.fraDistance.Name = "fraDistance";
            this.fraDistance.Size = new System.Drawing.Size(240, 177);
            this.fraDistance.TabIndex = 5;
            this.fraDistance.TabStop = false;
            this.fraDistance.Tag = "10906";
            this.fraDistance.Text = "#寸法校正（実行時／現在値）";
            // 
            // fraVertical
            // 
            this.fraVertical.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fraVertical.Location = new System.Drawing.Point(8, 344);
            this.fraVertical.Name = "fraVertical";
            this.fraVertical.Size = new System.Drawing.Size(240, 97);
            this.fraVertical.TabIndex = 4;
            this.fraVertical.TabStop = false;
            this.fraVertical.Tag = "10902";
            this.fraVertical.Text = "#幾何歪校正（実行時／現在値）";
            // 
            // fraOffset
            // 
            this.fraOffset.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fraOffset.Location = new System.Drawing.Point(259, 223);
            this.fraOffset.Name = "fraOffset";
            this.fraOffset.Size = new System.Drawing.Size(240, 97);
            this.fraOffset.TabIndex = 3;
            this.fraOffset.TabStop = false;
            this.fraOffset.Tag = "10905";
            this.fraOffset.Text = "#オフセット校正（実行時／現在値）";
            // 
            // fraScanPosi
            // 
            this.fraScanPosi.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fraScanPosi.Location = new System.Drawing.Point(8, 223);
            this.fraScanPosi.Name = "fraScanPosi";
            this.fraScanPosi.Size = new System.Drawing.Size(240, 113);
            this.fraScanPosi.TabIndex = 2;
            this.fraScanPosi.TabStop = false;
            this.fraScanPosi.Tag = "10901";
            this.fraScanPosi.Text = "#ｽｷｬﾝ位置校正（実行時／現在値）";
            // 
            // fraRotate
            // 
            this.fraRotate.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fraRotate.Location = new System.Drawing.Point(256, 24);
            this.fraRotate.Name = "fraRotate";
            this.fraRotate.Size = new System.Drawing.Size(240, 193);
            this.fraRotate.TabIndex = 1;
            this.fraRotate.TabStop = false;
            this.fraRotate.Tag = "10903";
            this.fraRotate.Text = "#回転中心校正（実行時／現在値）";
            // 
            // fraGain
            // 
            this.fraGain.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fraGain.Location = new System.Drawing.Point(8, 24);
            this.fraGain.Name = "fraGain";
            this.fraGain.Size = new System.Drawing.Size(240, 193);
            this.fraGain.TabIndex = 0;
            this.fraGain.TabStop = false;
            this.fraGain.Tag = "10904";
            this.fraGain.Text = "#ゲイン校正（実行時／現在値）";
            // 
            // fraScanCorrect
            // 
            this.fraScanCorrect.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11F);
            this.fraScanCorrect.Location = new System.Drawing.Point(508, 8);
            this.fraScanCorrect.Name = "fraScanCorrect";
            this.fraScanCorrect.Size = new System.Drawing.Size(193, 389);
            this.fraScanCorrect.TabIndex = 1;
            this.fraScanCorrect.TabStop = false;
            this.fraScanCorrect.Tag = "10509";
            this.fraScanCorrect.Text = "#スキャン校正";
            // 
            // cmdClose
            // 
            this.cmdClose.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdClose.Location = new System.Drawing.Point(560, 440);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(89, 33);
            this.cmdClose.TabIndex = 2;
            this.cmdClose.Tag = "10008";
            this.cmdClose.Text = "#閉じる";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // pnlCorStatus
            // 
            this.pnlCorStatus.Controls.Add(this.Frame1);
            this.pnlCorStatus.Controls.Add(this.cmdClose);
            this.pnlCorStatus.Controls.Add(this.fraScanCorrect);
            this.pnlCorStatus.Location = new System.Drawing.Point(0, 0);
            this.pnlCorStatus.Name = "pnlCorStatus";
            this.pnlCorStatus.Size = new System.Drawing.Size(703, 528);
            this.pnlCorStatus.TabIndex = 3;
            // 
            // frmCorrectionStatus
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(703, 528);
            this.ControlBox = false;
            this.Controls.Add(this.pnlCorStatus);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCorrectionStatus";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "校正－詳細";
            this.Load += new System.EventHandler(this.frmCorrectionStatus_Load);
            this.Frame1.ResumeLayout(false);
            this.pnlCorStatus.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox fraGain;
        private System.Windows.Forms.GroupBox fraRotate;
        //private System.Windows.Forms.Label lblColonRot0;
        private System.Windows.Forms.GroupBox fraScanPosi;
        private System.Windows.Forms.GroupBox fraOffset;
        private System.Windows.Forms.GroupBox fraVertical;
        private System.Windows.Forms.GroupBox fraDistance;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.GroupBox fraScanCorrect;
        private System.Windows.Forms.Button cmdClose;
        internal System.Windows.Forms.GroupBox Frame1;
        internal System.Windows.Forms.Panel pnlCorStatus;
        private System.Windows.Forms.GroupBox fraGainShift;
    }
}