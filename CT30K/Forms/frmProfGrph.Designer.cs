namespace CT30K
{
    partial class frmProfGrph
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
            this.pic0 = new System.Windows.Forms.PictureBox();
            this.fraView = new System.Windows.Forms.GroupBox();
            this.shpMagenta = new System.Windows.Forms.Label();
            this.chkProf = new System.Windows.Forms.CheckBox();
            this.cmdClose = new System.Windows.Forms.Button();
            this.fraScale = new System.Windows.Forms.GroupBox();
            this.CWSlide1 = new System.Windows.Forms.TrackBar();
            this.CWSlide1Min = new System.Windows.Forms.Label();
            this.CWSlide1Max = new System.Windows.Forms.Label();
            this.CWSlide2 = new System.Windows.Forms.TrackBar();
            this.CWSlide2Min = new System.Windows.Forms.Label();
            this.CWSlide2Max = new System.Windows.Forms.Label();
            this.pnlSlide1 = new System.Windows.Forms.Panel();
            this.pnlSlide2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pic0)).BeginInit();
            this.fraView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CWSlide1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CWSlide2)).BeginInit();
            this.pnlSlide1.SuspendLayout();
            this.pnlSlide2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pic0
            // 
            this.pic0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pic0.Location = new System.Drawing.Point(43, 8);
            this.pic0.Margin = new System.Windows.Forms.Padding(0);
            this.pic0.Name = "pic0";
            this.pic0.Size = new System.Drawing.Size(641, 373);
            this.pic0.TabIndex = 0;
            this.pic0.TabStop = false;
            // 
            // fraView
            // 
            this.fraView.Controls.Add(this.shpMagenta);
            this.fraView.Controls.Add(this.chkProf);
            this.fraView.Location = new System.Drawing.Point(696, 8);
            this.fraView.Margin = new System.Windows.Forms.Padding(4);
            this.fraView.Name = "fraView";
            this.fraView.Padding = new System.Windows.Forms.Padding(4);
            this.fraView.Size = new System.Drawing.Size(193, 57);
            this.fraView.TabIndex = 1;
            this.fraView.TabStop = false;
            this.fraView.Tag = "12203";
            this.fraView.Text = "#表示項目";
            // 
            // shpMagenta
            // 
            this.shpMagenta.BackColor = System.Drawing.Color.Magenta;
            this.shpMagenta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.shpMagenta.Location = new System.Drawing.Point(151, 28);
            this.shpMagenta.Name = "shpMagenta";
            this.shpMagenta.Size = new System.Drawing.Size(25, 12);
            this.shpMagenta.TabIndex = 1;
            // 
            // chkProf
            // 
            this.chkProf.AutoSize = true;
            this.chkProf.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11F);
            this.chkProf.Location = new System.Drawing.Point(16, 24);
            this.chkProf.Name = "chkProf";
            this.chkProf.Size = new System.Drawing.Size(129, 19);
            this.chkProf.TabIndex = 0;
            this.chkProf.Tag = "12202";
            this.chkProf.Text = "#ｵﾌｾｯﾄﾌﾟﾛﾌｨｰﾙ";
            this.chkProf.UseVisualStyleBackColor = true;
            this.chkProf.CheckStateChanged += new System.EventHandler(this.chkProf_CheckStateChanged);
            // 
            // cmdClose
            // 
            this.cmdClose.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdClose.Location = new System.Drawing.Point(744, 344);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(113, 33);
            this.cmdClose.TabIndex = 3;
            this.cmdClose.Tag = "10008";
            this.cmdClose.Text = "#閉じる";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // fraScale
            // 
            this.fraScale.Location = new System.Drawing.Point(696, 80);
            this.fraScale.Name = "fraScale";
            this.fraScale.Size = new System.Drawing.Size(193, 99);
            this.fraScale.TabIndex = 4;
            this.fraScale.TabStop = false;
            this.fraScale.Tag = "12748";
            this.fraScale.Text = "#縦倍率";
            // 
            // CWSlide1
            // 
            this.CWSlide1.AutoSize = false;
            this.CWSlide1.Location = new System.Drawing.Point(0, -23);
            this.CWSlide1.Maximum = 639;
            this.CWSlide1.Name = "CWSlide1";
            this.CWSlide1.Size = new System.Drawing.Size(664, 38);
            this.CWSlide1.TabIndex = 5;
            this.CWSlide1.TickFrequency = 16;
            // 
            // CWSlide1Min
            // 
            this.CWSlide1Min.AutoSize = true;
            this.CWSlide1Min.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.CWSlide1Min.Location = new System.Drawing.Point(8, 10);
            this.CWSlide1Min.Name = "CWSlide1Min";
            this.CWSlide1Min.Size = new System.Drawing.Size(11, 12);
            this.CWSlide1Min.TabIndex = 6;
            this.CWSlide1Min.Text = "0";
            // 
            // CWSlide1Max
            // 
            this.CWSlide1Max.AutoSize = true;
            this.CWSlide1Max.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.CWSlide1Max.Location = new System.Drawing.Point(638, 10);
            this.CWSlide1Max.Name = "CWSlide1Max";
            this.CWSlide1Max.Size = new System.Drawing.Size(23, 12);
            this.CWSlide1Max.TabIndex = 7;
            this.CWSlide1Max.Text = "639";
            // 
            // CWSlide2
            // 
            this.CWSlide2.AutoSize = false;
            this.CWSlide2.Location = new System.Drawing.Point(35, 0);
            this.CWSlide2.Maximum = 1024;
            this.CWSlide2.Name = "CWSlide2";
            this.CWSlide2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CWSlide2.Size = new System.Drawing.Size(57, 395);
            this.CWSlide2.TabIndex = 8;
            this.CWSlide2.TickFrequency = 0;
            this.CWSlide2.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            // 
            // CWSlide2Min
            // 
            this.CWSlide2Min.AutoSize = true;
            this.CWSlide2Min.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.CWSlide2Min.Location = new System.Drawing.Point(28, 376);
            this.CWSlide2Min.Name = "CWSlide2Min";
            this.CWSlide2Min.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CWSlide2Min.Size = new System.Drawing.Size(11, 12);
            this.CWSlide2Min.TabIndex = 9;
            this.CWSlide2Min.Text = "0";
            this.CWSlide2Min.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CWSlide2Max
            // 
            this.CWSlide2Max.AutoSize = true;
            this.CWSlide2Max.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.CWSlide2Max.Location = new System.Drawing.Point(11, 8);
            this.CWSlide2Max.Name = "CWSlide2Max";
            this.CWSlide2Max.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CWSlide2Max.Size = new System.Drawing.Size(29, 12);
            this.CWSlide2Max.TabIndex = 10;
            this.CWSlide2Max.Text = "1024";
            this.CWSlide2Max.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlSlide1
            // 
            this.pnlSlide1.Controls.Add(this.CWSlide1Min);
            this.pnlSlide1.Controls.Add(this.CWSlide1Max);
            this.pnlSlide1.Controls.Add(this.CWSlide1);
            this.pnlSlide1.Location = new System.Drawing.Point(32, 384);
            this.pnlSlide1.Name = "pnlSlide1";
            this.pnlSlide1.Size = new System.Drawing.Size(664, 31);
            this.pnlSlide1.TabIndex = 11;
            // 
            // pnlSlide2
            // 
            this.pnlSlide2.Controls.Add(this.CWSlide2Min);
            this.pnlSlide2.Controls.Add(this.CWSlide2Max);
            this.pnlSlide2.Controls.Add(this.CWSlide2);
            this.pnlSlide2.Location = new System.Drawing.Point(-4, -3);
            this.pnlSlide2.Name = "pnlSlide2";
            this.pnlSlide2.Size = new System.Drawing.Size(45, 400);
            this.pnlSlide2.TabIndex = 12;
            // 
            // frmProfGrph
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(901, 419);
            this.ControlBox = false;
            this.Controls.Add(this.pnlSlide1);
            this.Controls.Add(this.pnlSlide2);
            this.Controls.Add(this.pic0);
            this.Controls.Add(this.fraScale);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.fraView);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmProfGrph";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Tag = "12456";
            this.Text = "#プロフィール";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmProfGrph_FormClosed);
            this.Load += new System.EventHandler(this.frmProfGrph_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pic0)).EndInit();
            this.fraView.ResumeLayout(false);
            this.fraView.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CWSlide1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CWSlide2)).EndInit();
            this.pnlSlide1.ResumeLayout(false);
            this.pnlSlide1.PerformLayout();
            this.pnlSlide2.ResumeLayout(false);
            this.pnlSlide2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pic0;
        private System.Windows.Forms.GroupBox fraView;
        private System.Windows.Forms.CheckBox chkProf;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.GroupBox fraScale;
        private System.Windows.Forms.Label shpMagenta;
        private System.Windows.Forms.TrackBar CWSlide1;
        private System.Windows.Forms.Label CWSlide1Min;
        private System.Windows.Forms.Label CWSlide1Max;
        private System.Windows.Forms.TrackBar CWSlide2;
        private System.Windows.Forms.Label CWSlide2Min;
        private System.Windows.Forms.Label CWSlide2Max;
        private System.Windows.Forms.Panel pnlSlide1;
        private System.Windows.Forms.Panel pnlSlide2;
    }
}