namespace CT30K
{
    partial class frmFDiff
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
            this.lblDiffFilter = new System.Windows.Forms.Label();
            this.lblDirection_0 = new System.Windows.Forms.Label();
            this.hsbDiffFilter = new System.Windows.Forms.HScrollBar();
            this.lblFilterNo = new System.Windows.Forms.Label();
            this.cmdEnd = new System.Windows.Forms.Button();
            this.cmdExe = new System.Windows.Forms.Button();
            this.lblDirection_1 = new System.Windows.Forms.Label();
            this.lblDirection_2 = new System.Windows.Forms.Label();
            this.lblDirection_3 = new System.Windows.Forms.Label();
            this.lblDirection_4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblDiffFilter
            // 
            this.lblDiffFilter.BackColor = System.Drawing.Color.Transparent;
            this.lblDiffFilter.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblDiffFilter.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDiffFilter.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblDiffFilter.Location = new System.Drawing.Point(112, 16);
            this.lblDiffFilter.Name = "lblDiffFilter";
            this.lblDiffFilter.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblDiffFilter.Size = new System.Drawing.Size(17, 17);
            this.lblDiffFilter.TabIndex = 4;
            this.lblDiffFilter.Text = "3";
            this.lblDiffFilter.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblDirection_0
            // 
            this.lblDirection_0.AutoSize = true;
            this.lblDirection_0.BackColor = System.Drawing.SystemColors.Control;
            this.lblDirection_0.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblDirection_0.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDirection_0.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblDirection_0.Location = new System.Drawing.Point(186, 41);
            this.lblDirection_0.Name = "lblDirection_0";
            this.lblDirection_0.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblDirection_0.Size = new System.Drawing.Size(11, 12);
            this.lblDirection_0.TabIndex = 5;
            this.lblDirection_0.Tag = "";
            this.lblDirection_0.Text = "1";
            // 
            // hsbDiffFilter
            // 
            this.hsbDiffFilter.Cursor = System.Windows.Forms.Cursors.Default;
            this.hsbDiffFilter.LargeChange = 1;
            this.hsbDiffFilter.Location = new System.Drawing.Point(165, 7);
            this.hsbDiffFilter.Maximum = 5;
            this.hsbDiffFilter.Minimum = 1;
            this.hsbDiffFilter.Name = "hsbDiffFilter";
            this.hsbDiffFilter.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.hsbDiffFilter.Size = new System.Drawing.Size(145, 25);
            this.hsbDiffFilter.TabIndex = 3;
            this.hsbDiffFilter.Tag = "";
            this.hsbDiffFilter.Value = 3;
            this.hsbDiffFilter.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsbDiffFilter_Scroll);
            // 
            // lblFilterNo
            // 
            this.lblFilterNo.AutoSize = true;
            this.lblFilterNo.BackColor = System.Drawing.SystemColors.Control;
            this.lblFilterNo.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblFilterNo.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblFilterNo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblFilterNo.Location = new System.Drawing.Point(8, 16);
            this.lblFilterNo.Name = "lblFilterNo";
            this.lblFilterNo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblFilterNo.Size = new System.Drawing.Size(107, 16);
            this.lblFilterNo.TabIndex = 3;
            this.lblFilterNo.Tag = "12570";
            this.lblFilterNo.Text = "#フィルタ番号：";
            // 
            // cmdEnd
            // 
            this.cmdEnd.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.cmdEnd.Location = new System.Drawing.Point(176, 72);
            this.cmdEnd.Name = "cmdEnd";
            this.cmdEnd.Size = new System.Drawing.Size(129, 33);
            this.cmdEnd.TabIndex = 1;
            this.cmdEnd.Tag = "10008";
            this.cmdEnd.Text = "#閉じる";
            this.cmdEnd.UseVisualStyleBackColor = true;
            this.cmdEnd.Click += new System.EventHandler(this.cmdEnd_Click);
            // 
            // cmdExe
            // 
            this.cmdExe.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.cmdExe.Location = new System.Drawing.Point(16, 72);
            this.cmdExe.Name = "cmdExe";
            this.cmdExe.Size = new System.Drawing.Size(129, 33);
            this.cmdExe.TabIndex = 2;
            this.cmdExe.Tag = "10005";
            this.cmdExe.Text = "#実行";
            this.cmdExe.UseVisualStyleBackColor = true;
            this.cmdExe.Click += new System.EventHandler(this.cmdExe_Click);
            // 
            // lblDirection_1
            // 
            this.lblDirection_1.AutoSize = true;
            this.lblDirection_1.BackColor = System.Drawing.SystemColors.Control;
            this.lblDirection_1.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblDirection_1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDirection_1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblDirection_1.Location = new System.Drawing.Point(209, 41);
            this.lblDirection_1.Name = "lblDirection_1";
            this.lblDirection_1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblDirection_1.Size = new System.Drawing.Size(11, 12);
            this.lblDirection_1.TabIndex = 6;
            this.lblDirection_1.Tag = "";
            this.lblDirection_1.Text = "2";
            // 
            // lblDirection_2
            // 
            this.lblDirection_2.AutoSize = true;
            this.lblDirection_2.BackColor = System.Drawing.SystemColors.Control;
            this.lblDirection_2.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblDirection_2.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDirection_2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblDirection_2.Location = new System.Drawing.Point(233, 41);
            this.lblDirection_2.Name = "lblDirection_2";
            this.lblDirection_2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblDirection_2.Size = new System.Drawing.Size(11, 12);
            this.lblDirection_2.TabIndex = 7;
            this.lblDirection_2.Tag = "";
            this.lblDirection_2.Text = "3";
            // 
            // lblDirection_3
            // 
            this.lblDirection_3.AutoSize = true;
            this.lblDirection_3.BackColor = System.Drawing.SystemColors.Control;
            this.lblDirection_3.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblDirection_3.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDirection_3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblDirection_3.Location = new System.Drawing.Point(257, 41);
            this.lblDirection_3.Name = "lblDirection_3";
            this.lblDirection_3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblDirection_3.Size = new System.Drawing.Size(11, 12);
            this.lblDirection_3.TabIndex = 8;
            this.lblDirection_3.Tag = "";
            this.lblDirection_3.Text = "4";
            // 
            // lblDirection_4
            // 
            this.lblDirection_4.AutoSize = true;
            this.lblDirection_4.BackColor = System.Drawing.SystemColors.Control;
            this.lblDirection_4.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblDirection_4.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDirection_4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblDirection_4.Location = new System.Drawing.Point(280, 41);
            this.lblDirection_4.Name = "lblDirection_4";
            this.lblDirection_4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblDirection_4.Size = new System.Drawing.Size(11, 12);
            this.lblDirection_4.TabIndex = 9;
            this.lblDirection_4.Tag = "";
            this.lblDirection_4.Text = "5";
            // 
            // frmFDiff
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(332, 120);
            this.ControlBox = false;
            this.Controls.Add(this.lblDirection_4);
            this.Controls.Add(this.lblDirection_3);
            this.Controls.Add(this.lblDirection_2);
            this.Controls.Add(this.lblDirection_1);
            this.Controls.Add(this.lblDiffFilter);
            this.Controls.Add(this.lblDirection_0);
            this.Controls.Add(this.hsbDiffFilter);
            this.Controls.Add(this.lblFilterNo);
            this.Controls.Add(this.cmdEnd);
            this.Controls.Add(this.cmdExe);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFDiff";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "12590";
            this.Text = "#微分";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmFDiff_FormClosed);
            this.Load += new System.EventHandler(this.frmFDiff_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lblDiffFilter;
        public System.Windows.Forms.Label lblDirection_0;
        public System.Windows.Forms.HScrollBar hsbDiffFilter;
        public System.Windows.Forms.Label lblFilterNo;
        private System.Windows.Forms.Button cmdEnd;
        private System.Windows.Forms.Button cmdExe;
        public System.Windows.Forms.Label lblDirection_1;
        public System.Windows.Forms.Label lblDirection_2;
        public System.Windows.Forms.Label lblDirection_3;
        public System.Windows.Forms.Label lblDirection_4;

    }
}