namespace CT30K
{
    partial class frmAddCommentFImage
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblSize = new System.Windows.Forms.Label();
            this.lblColor = new System.Windows.Forms.Label();
            this.lblComment = new System.Windows.Forms.Label();
            this.chkCommentPosBL = new System.Windows.Forms.CheckBox();
            this.chkCommentPosTR = new System.Windows.Forms.CheckBox();
            this.chkCommentPosBR = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.nudFontColor = new System.Windows.Forms.NumericUpDown();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.nudFontSize = new System.Windows.Forms.NumericUpDown();
            this.lblPosition = new System.Windows.Forms.Label();
            this.chkCommentPosTL = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudFontColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFontSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox1.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.textBox1.Location = new System.Drawing.Point(12, 26);
            this.textBox1.MaxLength = 20;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(217, 19);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "ABCDEFGHIJKLMNOPQRST";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(10, 129);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(34, 12);
            this.lblSize.TabIndex = 1;
            this.lblSize.Tag = "22013";
            this.lblSize.Text = "サイズ";
            // 
            // lblColor
            // 
            this.lblColor.AutoSize = true;
            this.lblColor.Location = new System.Drawing.Point(11, 155);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(33, 12);
            this.lblColor.TabIndex = 2;
            this.lblColor.Tag = "22014";
            this.lblColor.Text = "グレー";
            // 
            // lblComment
            // 
            this.lblComment.AutoSize = true;
            this.lblComment.Location = new System.Drawing.Point(12, 11);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(38, 12);
            this.lblComment.TabIndex = 8;
            this.lblComment.Tag = "12816";
            this.lblComment.Text = "コメント";
            // 
            // chkCommentPosBL
            // 
            this.chkCommentPosBL.AutoSize = true;
            this.chkCommentPosBL.Location = new System.Drawing.Point(238, 133);
            this.chkCommentPosBL.Name = "chkCommentPosBL";
            this.chkCommentPosBL.Size = new System.Drawing.Size(15, 14);
            this.chkCommentPosBL.TabIndex = 8;
            this.chkCommentPosBL.UseVisualStyleBackColor = true;
            this.chkCommentPosBL.CheckedChanged += new System.EventHandler(this.chkCommentPosBL_CheckedChanged);
            // 
            // chkCommentPosTR
            // 
            this.chkCommentPosTR.AutoSize = true;
            this.chkCommentPosTR.Location = new System.Drawing.Point(342, 29);
            this.chkCommentPosTR.Name = "chkCommentPosTR";
            this.chkCommentPosTR.Size = new System.Drawing.Size(15, 14);
            this.chkCommentPosTR.TabIndex = 7;
            this.chkCommentPosTR.UseVisualStyleBackColor = true;
            this.chkCommentPosTR.CheckedChanged += new System.EventHandler(this.chkCommentPosTR_CheckedChanged);
            // 
            // chkCommentPosBR
            // 
            this.chkCommentPosBR.AutoSize = true;
            this.chkCommentPosBR.Location = new System.Drawing.Point(342, 133);
            this.chkCommentPosBR.Name = "chkCommentPosBR";
            this.chkCommentPosBR.Size = new System.Drawing.Size(15, 14);
            this.chkCommentPosBR.TabIndex = 9;
            this.chkCommentPosBR.UseVisualStyleBackColor = true;
            this.chkCommentPosBR.CheckedChanged += new System.EventHandler(this.chkCommentPosBR_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(207, 157);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(73, 29);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(286, 157);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(73, 29);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // nudFontColor
            // 
            this.nudFontColor.Location = new System.Drawing.Point(49, 151);
            this.nudFontColor.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudFontColor.Name = "nudFontColor";
            this.nudFontColor.Size = new System.Drawing.Size(55, 19);
            this.nudFontColor.TabIndex = 5;
            this.nudFontColor.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox2.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.textBox2.Location = new System.Drawing.Point(12, 51);
            this.textBox2.MaxLength = 20;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(217, 19);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "01234567890123456789";
            this.textBox2.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox3.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.textBox3.Location = new System.Drawing.Point(12, 76);
            this.textBox3.MaxLength = 20;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(217, 19);
            this.textBox3.TabIndex = 2;
            this.textBox3.Text = "ABCDEFGHIJKLMNOPQRST";
            this.textBox3.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBox4
            // 
            this.textBox4.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox4.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.textBox4.Location = new System.Drawing.Point(12, 101);
            this.textBox4.MaxLength = 20;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(217, 19);
            this.textBox4.TabIndex = 3;
            this.textBox4.Text = "01234567890123456789";
            this.textBox4.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // nudFontSize
            // 
            this.nudFontSize.Location = new System.Drawing.Point(49, 126);
            this.nudFontSize.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.nudFontSize.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudFontSize.Name = "nudFontSize";
            this.nudFontSize.Size = new System.Drawing.Size(43, 19);
            this.nudFontSize.TabIndex = 12;
            this.nudFontSize.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // lblPosition
            // 
            this.lblPosition.AutoSize = true;
            this.lblPosition.Location = new System.Drawing.Point(232, 11);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(29, 12);
            this.lblPosition.TabIndex = 6;
            this.lblPosition.Tag = "22015";
            this.lblPosition.Text = "位置";
            // 
            // chkCommentPosTL
            // 
            this.chkCommentPosTL.AutoSize = true;
            this.chkCommentPosTL.Location = new System.Drawing.Point(237, 29);
            this.chkCommentPosTL.Name = "chkCommentPosTL";
            this.chkCommentPosTL.Size = new System.Drawing.Size(15, 14);
            this.chkCommentPosTL.TabIndex = 6;
            this.chkCommentPosTL.UseVisualStyleBackColor = true;
            this.chkCommentPosTL.CheckedChanged += new System.EventHandler(this.chkCommentPosTL_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(234, 25);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(125, 125);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // frmAddCommentFImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 189);
            this.ControlBox = false;
            this.Controls.Add(this.nudFontSize);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.nudFontColor);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkCommentPosBR);
            this.Controls.Add(this.chkCommentPosTR);
            this.Controls.Add(this.chkCommentPosBL);
            this.Controls.Add(this.chkCommentPosTL);
            this.Controls.Add(this.lblComment);
            this.Controls.Add(this.lblPosition);
            this.Controls.Add(this.lblColor);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddCommentFImage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "コメント編集";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAddCommentFImage_FormClosed);
            this.Load += new System.EventHandler(this.frmAddCommentFImage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudFontColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFontSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.CheckBox chkCommentPosBL;
        private System.Windows.Forms.CheckBox chkCommentPosTR;
        private System.Windows.Forms.CheckBox chkCommentPosBR;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.NumericUpDown nudFontColor;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.NumericUpDown nudFontSize;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.CheckBox chkCommentPosTL;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}