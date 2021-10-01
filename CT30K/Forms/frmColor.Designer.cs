namespace CT30K
{
    partial class frmColor
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
            this.tabColor = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblColorVal = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblUpperPercent = new System.Windows.Forms.Label();
            this.lblLowerPercent = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Line1 = new System.Windows.Forms.Label();
            this.cmdColorSet = new System.Windows.Forms.Button();
            this.cmdColorCancel = new System.Windows.Forms.Button();
            this.cmdColorOk = new System.Windows.Forms.Button();
            this.tabColor.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabColor
            // 
            this.tabColor.Controls.Add(this.tabPage1);
            this.tabColor.Controls.Add(this.tabPage2);
            this.tabColor.Controls.Add(this.tabPage3);
            this.tabColor.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.tabColor.Location = new System.Drawing.Point(10, 24);
            this.tabColor.Name = "tabColor";
            this.tabColor.SelectedIndex = 0;
            this.tabColor.Size = new System.Drawing.Size(497, 249);
            this.tabColor.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(489, 219);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Tag = "12425";
            this.tabPage1.Text = "#カラー処理";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(4, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(481, 97);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(489, 219);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Tag = "12424";
            this.tabPage2.Text = "#カラースケール作成";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblColorVal);
            this.groupBox2.Location = new System.Drawing.Point(3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(481, 209);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            // 
            // lblColorVal
            // 
            this.lblColorVal.AutoSize = true;
            this.lblColorVal.Location = new System.Drawing.Point(14, 24);
            this.lblColorVal.Name = "lblColorVal";
            this.lblColorVal.Size = new System.Drawing.Size(67, 16);
            this.lblColorVal.TabIndex = 26;
            this.lblColorVal.Tag = "12429";
            this.lblColorVal.Text = "#色数：８";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox3);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(489, 219);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Tag = "12430";
            this.tabPage3.Text = "#閾値設定";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblUpperPercent);
            this.groupBox3.Controls.Add(this.lblLowerPercent);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.Line1);
            this.groupBox3.Location = new System.Drawing.Point(3, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(481, 209);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            // 
            // lblUpperPercent
            // 
            this.lblUpperPercent.AutoSize = true;
            this.lblUpperPercent.Location = new System.Drawing.Point(463, 183);
            this.lblUpperPercent.Name = "lblUpperPercent";
            this.lblUpperPercent.Size = new System.Drawing.Size(16, 16);
            this.lblUpperPercent.TabIndex = 42;
            this.lblUpperPercent.Text = "%";
            // 
            // lblLowerPercent
            // 
            this.lblLowerPercent.AutoSize = true;
            this.lblLowerPercent.Location = new System.Drawing.Point(463, 123);
            this.lblLowerPercent.Name = "lblLowerPercent";
            this.lblLowerPercent.Size = new System.Drawing.Size(16, 16);
            this.lblLowerPercent.TabIndex = 41;
            this.lblLowerPercent.Text = "%";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(425, 155);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 16);
            this.label7.TabIndex = 40;
            this.label7.Text = "│";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(368, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 16);
            this.label6.TabIndex = 39;
            this.label6.Text = "│";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(310, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 16);
            this.label5.TabIndex = 38;
            this.label5.Text = "│";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(256, 155);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 16);
            this.label4.TabIndex = 37;
            this.label4.Text = "│";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(200, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 16);
            this.label3.TabIndex = 36;
            this.label3.Text = "│";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(142, 155);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 16);
            this.label2.TabIndex = 35;
            this.label2.Text = "│";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(87, 155);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 16);
            this.label1.TabIndex = 34;
            this.label1.Text = "│";
            // 
            // Line1
            // 
            this.Line1.AutoSize = true;
            this.Line1.Location = new System.Drawing.Point(31, 155);
            this.Line1.Name = "Line1";
            this.Line1.Size = new System.Drawing.Size(24, 16);
            this.Line1.TabIndex = 33;
            this.Line1.Text = "│";
            // 
            // cmdColorSet
            // 
            this.cmdColorSet.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdColorSet.Location = new System.Drawing.Point(384, 283);
            this.cmdColorSet.Name = "cmdColorSet";
            this.cmdColorSet.Size = new System.Drawing.Size(113, 33);
            this.cmdColorSet.TabIndex = 7;
            this.cmdColorSet.Tag = "10011";
            this.cmdColorSet.Text = "#表　示";
            this.cmdColorSet.UseVisualStyleBackColor = true;
            this.cmdColorSet.Click += new System.EventHandler(this.cmdColorSet_Click);
            // 
            // cmdColorCancel
            // 
            this.cmdColorCancel.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdColorCancel.Location = new System.Drawing.Point(257, 283);
            this.cmdColorCancel.Name = "cmdColorCancel";
            this.cmdColorCancel.Size = new System.Drawing.Size(113, 33);
            this.cmdColorCancel.TabIndex = 6;
            this.cmdColorCancel.Tag = "10002";
            this.cmdColorCancel.Text = "#キャンセル";
            this.cmdColorCancel.UseVisualStyleBackColor = true;
            this.cmdColorCancel.Click += new System.EventHandler(this.cmdColorCancel_Click);
            // 
            // cmdColorOk
            // 
            this.cmdColorOk.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdColorOk.Location = new System.Drawing.Point(131, 283);
            this.cmdColorOk.Name = "cmdColorOk";
            this.cmdColorOk.Size = new System.Drawing.Size(112, 33);
            this.cmdColorOk.TabIndex = 5;
            this.cmdColorOk.Tag = "10001";
            this.cmdColorOk.Text = "#Ｏ　Ｋ";
            this.cmdColorOk.UseVisualStyleBackColor = true;
            this.cmdColorOk.Click += new System.EventHandler(this.cmdColorOk_Click);
            // 
            // frmColor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(516, 338);
            this.ControlBox = false;
            this.Controls.Add(this.cmdColorSet);
            this.Controls.Add(this.cmdColorCancel);
            this.Controls.Add(this.cmdColorOk);
            this.Controls.Add(this.tabColor);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmColor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Tag = "12425";
            this.Text = "#カラー処理";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmColor_FormClosed);
            this.Load += new System.EventHandler(this.frmColor_Load);
            this.tabColor.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabColor;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button cmdColorSet;
        private System.Windows.Forms.Button cmdColorCancel;
        private System.Windows.Forms.Button cmdColorOk;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblColorVal;
        private System.Windows.Forms.Label Line1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUpperPercent;
        private System.Windows.Forms.Label lblLowerPercent;
    }
}